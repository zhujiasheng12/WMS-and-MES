using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.工程部.Controller
{
    /// <summary>
    /// 判断工艺审核 的摘要说明
    /// </summary>
    public class 判断编程审核 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var orderId =int.Parse(context.Request["orderId"]);
            JDJS_WMS_DB_USEREntities entities = new JDJS_WMS_DB_USEREntities();
            var processRow = entities.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == orderId&r.program_audit_sign!=1);
            if (processRow.Count() > 0)
            {
                context.Response.Write("您的编程未审核通过");
            }
            else
            {
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