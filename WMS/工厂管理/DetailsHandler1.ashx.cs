using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model.工厂管理
{
    /// <summary>
    /// DetailsHandler1 的摘要说明
    /// </summary>
    public class DetailsHandler1 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int id = int.Parse(context.Request["id"]);
            using (JDJS_WMS_DB_USEREntities data = new JDJS_WMS_DB_USEREntities())
            {
                var data1 = data.JDJS_WMS_Location_Info.Where(r => r.id == id).First();
                if (data1.parentId == 0)
                {
                    context.Response.Write(data1.Name);
                }
                else
                {
                    var data2 = data.JDJS_WMS_Location_Info.Where(r => r.id == data1.parentId).First();
                    var data3 = data2.Name;
                    context.Response.Write(data3);
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