using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.工程部
{
    /// <summary>
    /// 预计编程完成时间提交 的摘要说明
    /// </summary>
    public class 预计编程完成时间提交 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var date = context.Request["date"];
        
            var  orderId =Convert.ToInt32 ( context.Request["orderId"]);
            using (JDJS_WMS_DB_USEREntities wms = new JDJS_WMS_DB_USEREntities())
            {
                
                var guide = wms.JDJS_WMS_Order_Guide_Schedu_Table.Where(r => r.OrderID == orderId).FirstOrDefault ();
                if (guide != null)
                {
                    guide.ExpectEndTime =Convert.ToDateTime ( date);
                }
                wms.SaveChanges();
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