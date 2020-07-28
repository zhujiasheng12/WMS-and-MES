using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace WebApplication2.生产管理.生产部.排产
{
    /// <summary>
    /// 排产提交预计完成时间 的摘要说明
    /// </summary>
    public class 排产提交预计完成时间 : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                var orderId = int.Parse(context.Request.Form["orderId"]);
                var time = Convert.ToDateTime(context.Request.Form["time"]);
                var loginID = Convert.ToInt32(context.Session["id"]);
                using (JDJS_WMS_DB_USEREntities wms = new JDJS_WMS_DB_USEREntities())
                {
                    var order = wms.JDJS_WMS_Order_Machine_Scheduing_Time_Table.Where(r => r.OrderID == orderId).FirstOrDefault();
                    if (order == null)
                    {
                        JDJS_WMS_Order_Machine_Scheduing_Time_Table jd = new JDJS_WMS_Order_Machine_Scheduing_Time_Table()
                        {
                            OrderID = orderId,
                            PlanPersonID = loginID,
                            PlanEndTime = time
                        };
                        wms.JDJS_WMS_Order_Machine_Scheduing_Time_Table.Add(jd);
                    }
                    else
                    {
                        order.PlanEndTime = time;
                        order.PlanPersonID = loginID;
                    }
                    wms.SaveChanges();
                }
                context.Response.Write("ok");
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