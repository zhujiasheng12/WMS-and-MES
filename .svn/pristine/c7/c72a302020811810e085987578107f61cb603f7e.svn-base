using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.测试
{
    /// <summary>
    /// 获取机床Id 的摘要说明
    /// </summary>
    public class 获取机床Id : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var workId = int.Parse(context.Request["workId"]);
            using (JDJS_WMS_DB_USEREntities entities = new JDJS_WMS_DB_USEREntities())
            {
                var obj = from cnc in entities.JDJS_WMS_Device_Info.Where(r => r.Position == workId)
                          select new
                          {
                              cnc.ID,
                              cnc.MachNum
                          };
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                var json = serializer.Serialize(obj);
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