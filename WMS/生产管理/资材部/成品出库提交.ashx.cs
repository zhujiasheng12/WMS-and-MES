using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.资材部
{
    /// <summary>
    /// 成品入库提交 的摘要说明
    /// </summary>
    public class 成品出库提交 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var orderId = int.Parse(context.Request["orderId"]);
            var outputNumber = int.Parse(context.Request["outputNumber"]);
            using(JDJS_WMS_DB_USEREntities entities=new JDJS_WMS_DB_USEREntities())
            {
                var row = entities.JDJS_WMS_Finished_Product_Manager.Where(r => r.OrderID == orderId);

                if (row.Count() > 0)
                {
                    if (row.First().stock >= outputNumber)
                    {
                        var order = entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == orderId);
                        int produstOUT = order.First().Product_Output;
                        int outnum =Convert.ToInt32 ( row.FirstOrDefault().outputNumber);
                        row.FirstOrDefault().stock -= outputNumber;
                        row.FirstOrDefault().outputNumber += outputNumber;
                        row.FirstOrDefault().outputTime = DateTime.Now;
                        
                        JDJS_WMS_Finished_Product_OutPut_History_Manager jdjsout = new JDJS_WMS_Finished_Product_OutPut_History_Manager()
                        {
                            OrderID =orderId ,
                            OutPutNum =outputNumber ,
                            Time =DateTime .Now
                        };
                        entities.JDJS_WMS_Finished_Product_OutPut_History_Manager.Add(jdjsout);
                        entities.SaveChanges();
                        if (outnum + outputNumber >= produstOUT)
                        {
                            foreach (var item in order)
                            {
                                item.Intention = 4;
                            }
                        }
                        entities.SaveChanges();

                    }
                    else
                    {
                        context.Response.Write("库存不足");
                        return;
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