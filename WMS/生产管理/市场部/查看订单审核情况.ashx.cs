using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.市场部
{
    /// <summary>
    /// 查看订单审核情况 的摘要说明
    /// </summary>
    public class 查看订单审核情况 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                int orderId = int.Parse(context.Request["orderId"]);//订单ID
                using (JDJS_WMS_DB_USEREntities model = new JDJS_WMS_DB_USEREntities())
                {
                    var order = model.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == orderId).FirstOrDefault();
                    if (order != null)
                    {
                        var result = new { state=order.AuditResult ,advice=order.AuditAdvice};
                        var str = new { msg = "", code = 0, count = 1, data = result };
                        var json = serializer.Serialize(str);
                        context.Response.Write(json);
                        return;
                    }
                }
                context.Response.Write("未知异常");
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