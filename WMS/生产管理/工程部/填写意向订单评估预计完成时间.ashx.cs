using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.工程部
{
    /// <summary>
    /// 填写意向订单评估预计完成时间 的摘要说明
    /// </summary>
    public class 填写意向订单评估预计完成时间 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                var orderId = int.Parse(context.Request["orderId"]);//订单id
                var time = Convert.ToDateTime(context.Request["time"]);//评估预计完成时间
                using (JDJS_WMS_DB_USEREntities wms = new JDJS_WMS_DB_USEREntities())
                {
                    var order = wms.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == orderId).FirstOrDefault();
                    if (order == null)
                    {
                        context.Response.Write("该订单不存在");
                        return;
                    }
                    order.IntentionAssessPlanEndTime = time;
                    wms.SaveChanges();
                    context.Response.Write("ok");
                    return;
                }
            }
            catch(Exception ex)
            {
                context.Response.Write(ex.Message);
                return;
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