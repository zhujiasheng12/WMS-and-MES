using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model.人员管理
{
    /// <summary>
    /// modifyPassword 的摘要说明
    /// </summary>
    public class modifyPassword : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var id = int.Parse(context.Request["id"]);
            using (JDJS_WMS_DB_USEREntities entities1 = new JDJS_WMS_DB_USEREntities())
            {
                var row = entities1.JDJS_WMS_Staff_Info.Where(r => r.id == id).First();
                var model = new { staff = row.staff, user = row.users };
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                var json = serializer.Serialize(model);
                context.Response.Write(json);
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