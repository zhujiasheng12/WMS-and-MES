using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.工程部
{
    /// <summary>
    /// 新建虚拟工序获取工序编号 的摘要说明
    /// </summary>
    public class 新建虚拟工序获取工序编号 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var orderId = int.Parse(context.Request["orderId"]);
            using(JDJS_WMS_DB_USEREntities entities=new JDJS_WMS_DB_USEREntities())
            {
                var rows = entities.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == orderId & r.sign == 0);
                context.Response.Write(rows.Count());
                    
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