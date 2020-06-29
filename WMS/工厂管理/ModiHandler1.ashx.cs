using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model.工厂管理
{
    /// <summary>
    /// ModiHandler1 的摘要说明
    /// </summary>
    public class ModiHandler1 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var name = context.Request["name"];
            int id = int.Parse(context.Request["id"]);
            //int father = int.Parse(context.Request["father"]);
            var team = context.Request["team"];
            context.Response.ContentType = "text/plain";

            using (JDJS_WMS_DB_USEREntities data = new JDJS_WMS_DB_USEREntities())
            {

                var data1 = data.JDJS_WMS_Location_Info.Where(r => r.id == id).First();
                //if (id==father)
                //{
                //    context.Response.Write("false");
                //}
                //else
                //{
                    data1.Name = name; /*data1.parentId = father;*/ data1.EndDate = team;
                    data.SaveChanges();
                    context.Response.Write("ok");
               // }
               
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