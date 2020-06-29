using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model.设备管理
{
    /// <summary>
    /// BrandDeleteHandler1 的摘要说明
    /// </summary>
    public class BrandDeleteHandler1 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int id = int.Parse(context.Request["id"]);
            using (JDJS_WMS_DB_USEREntities pm2 = new JDJS_WMS_DB_USEREntities())
            {

                var model = pm2.JDJS_WMS_Device_Type_Info.Where(r => r.BrandID == id);
                if (model.Count() > 0)
                {
                    context.Response.Write("请先删除该品牌机床型号");
                }
                else
                {
                    var user = pm2.JDJS_WMS_Device_Brand_Info.Where(r => r.ID == id).First();
                    pm2.JDJS_WMS_Device_Brand_Info.Remove(user);
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