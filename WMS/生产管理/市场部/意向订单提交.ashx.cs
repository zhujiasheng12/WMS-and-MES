using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.市场部
{
    /// <summary>
    /// 意向订单提交 的摘要说明
    /// </summary>
    public class 意向订单提交 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var orderId = int.Parse(context.Request["orderId"]);
            using (JDJS_WMS_DB_USEREntities entities=new JDJS_WMS_DB_USEREntities())
            {
                var row = entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == orderId);
                if (row.Count() > 0)
                {
                    if (row.First().Intention == 5)
                    {
                        row.First().Intention = -3;
                       
                        entities.SaveChanges();
                        context.Response.Write("ok");
                        return;
                    }else if (row.First().Intention == 6)
                    {
                        row.First().Intention = -3;
                        var newRow = new JDJS_WMS_Order_Queue_Table { isFlag = 0, OrderID = orderId };
                        entities.JDJS_WMS_Order_Queue_Table.Add(newRow);
                        entities.SaveChanges();
                        context.Response.Write("ok");
                        return;
                    }
                    else
                    {
                        context.Response.Write("已提交");
                        return;
                    }
                }
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