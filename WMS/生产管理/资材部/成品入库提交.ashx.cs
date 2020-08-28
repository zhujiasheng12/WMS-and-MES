using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using WebApplication2.生产管理.资材部.夹具管理.特殊治具管理Access数据库;

namespace WebApplication2.生产管理.资材部
{
    /// <summary>
    /// 成品入库提交 的摘要说明
    /// </summary>
    public class 成品入库提交 : IHttpHandler,IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            var orderId = int.Parse(context.Request["orderId"]);
            var warehousingNumber = int.Parse(context.Request["warehousingNumber"]);
            var defectiveProductNumber = int.Parse(context.Request["defectiveProductNumber"]);
            var loginUserId=Convert .ToInt32 ( context.Session["id"]);
            using(JDJS_WMS_DB_USEREntities entities=new JDJS_WMS_DB_USEREntities())
            {
                var row = entities.JDJS_WMS_Finished_Product_Manager.Where(r => r.OrderID == orderId);
                if (row.Count() > 0)
                {
                    var count =Convert .ToInt32 ( row.FirstOrDefault().warehousingNumber);
                    count += warehousingNumber;
                    row.FirstOrDefault().waitForWarehousing -= (warehousingNumber+defectiveProductNumber);//待入库数
                    row.FirstOrDefault().warehousingNumber += warehousingNumber;//入库数
                    row.FirstOrDefault().stock += warehousingNumber;//库存数
                    row.FirstOrDefault().warehousingTime = DateTime.Now;
                    row.FirstOrDefault().DefectiveProductNumber += defectiveProductNumber;
                    JDJS_WMS_Finished_Product_In_History_Manager jDJS_WMS_Finished_Product_In_History_Manager = new JDJS_WMS_Finished_Product_In_History_Manager()
                    {
                        Num = warehousingNumber,
                        OrderID =orderId ,
                        Time =DateTime .Now
                    };
                    entities.JDJS_WMS_Finished_Product_In_History_Manager.Add(jDJS_WMS_Finished_Product_In_History_Manager);
                    JDJS_WMS_Finished_Defective_Product_In_History_Manager jd = new JDJS_WMS_Finished_Defective_Product_In_History_Manager()
                    {
                        CreatPersonID =loginUserId ,
                        CreatTime =DateTime .Now ,
                        InNumber =defectiveProductNumber ,
                        OrderID =orderId ,
                        Time =DateTime .Now 
                    };
                    entities.JDJS_WMS_Finished_Defective_Product_In_History_Manager.Add(jd);
                    entities.SaveChanges();
                    var order = entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == orderId).FirstOrDefault();
                    if (order != null)
                    {
                        if (count >= order.Product_Output)
                        {
                            order.Intention = 4;
                            order.Order_Actual_End_Time = DateTime.Now;
                            //检测是否为特殊治具订单，是的话将其填入临时治具库
                            if (order.Order_Number.StartsWith("FX"))
                            {
                                string str = "";
                                var info = entities.JDJS_WMS_Fixture_Manage_Demand_Table.Where(r => r.FixtureOrderNum == order.Order_Number).FirstOrDefault();
                                if (info != null)
                                {
                                    str = info.FixtureSpecification;
                                }
                                //加入临时治具库
                                using (FixtureModel model = new FixtureModel())
                                {
                                    JDJS_WMS_Fixture_Temporary_Table jdTem = new JDJS_WMS_Fixture_Temporary_Table()
                                    {
                                        Name = order.Product_Name,
                                        Remark = "",
                                        FixtureOrderNum = order.Order_Number,
                                        FixtureSpecification = str,
                                        InTime = DateTime.Now,
                                        StockNum = row.FirstOrDefault().warehousingNumber
                                    };
                                    model.JDJS_WMS_Fixture_Temporary_Table.Add(jdTem);
                                    model.SaveChanges();
                                }
                            }
                        }
                    }
                }
                entities.SaveChanges();
                context.Response.Write("ok");

            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}