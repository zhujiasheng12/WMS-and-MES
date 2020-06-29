using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.工程部
{
    /// <summary>
    /// virtualProcessDel 的摘要说明
    /// </summary>
    public class virtualProcessDel : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var id = int.Parse(context.Request["id"]);
            using (JDJS_WMS_DB_USEREntities entities=new JDJS_WMS_DB_USEREntities())
            {
                var orderId = entities.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == id).FirstOrDefault().OrderID;
                var judge = entities.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == orderId&r.sign==0);
                var process= entities.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == id).FirstOrDefault().ProcessID;
                if (process != judge.Count())
                {
                    context.Response.Write("请从最后一道序开始删除");
                    return;
                }
                var row = entities.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == id);
                entities.JDJS_WMS_Order_Process_Info_Table.Remove(row.First());
                entities.SaveChanges();
            }
            context.Response.Write("ok");
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