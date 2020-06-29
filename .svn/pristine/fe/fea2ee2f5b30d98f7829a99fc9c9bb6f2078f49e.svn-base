using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Kanban.现场
{
    /// <summary>
    /// 读取车间名 的摘要说明
    /// </summary>
    public class 读取车间名 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
           using(JDJS_WMS_DB_USEREntities entities=new JDJS_WMS_DB_USEREntities())
            {
                var workId = int.Parse(context.Request["workId"]);
                var name = entities.JDJS_WMS_Location_Info.Where(r => r.id == workId).First().Name;
                context.Response.Write(name);
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