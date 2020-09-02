using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.工程部
{
    /// <summary>
    /// 提价审核 的摘要说明
    /// </summary>
    public class 提价审核 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try {
            var orderId = int.Parse(context.Request.QueryString["orderId"]);
            using(JDJS_WMS_DB_USEREntities entities=new JDJS_WMS_DB_USEREntities())
            {
                var rows = entities.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == orderId & r.sign == -1);
                    if (rows.Count() < 1)
                    {
                        context.Response.Write("暂无需提交审核工序");
                        return;

                    }
                foreach (var item in entities.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == orderId&&r.sign !=0))
                {
                    item.sign = -2;

                }
                entities.SaveChanges();
                context.Response.Write("ok");
            }
            }
            catch (Exception ex)
            {
                context.Response.Write(ex.Message);

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