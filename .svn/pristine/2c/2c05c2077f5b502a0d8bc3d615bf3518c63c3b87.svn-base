using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model.设备管理
{
    /// <summary>
    /// DeleateHandler1 的摘要说明
    /// </summary>
    public class DeleateHandler1 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int id = int.Parse(context.Request["id"]);
            using (JDJS_WMS_DB_USEREntities pm2 = new JDJS_WMS_DB_USEREntities())
            {
                var mach = pm2.JDJS_WMS_Device_Info.Where(r => r.MachType == id);
                if (mach.Count() > 0)
                {
                    context.Response.Write("请先删除该型号机床");
                }
                else
                {
                    var user = pm2.JDJS_WMS_Device_Type_Info.Where(r => r.ID == id).First();
                    pm2.JDJS_WMS_Device_Type_Info.Remove(user);
                    pm2.SaveChanges();
                    context.Response.Write("ok");
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