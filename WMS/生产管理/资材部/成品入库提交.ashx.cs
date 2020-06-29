using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

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
                if (row.Count() > 0){
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