using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.刀具管理.备刀管理
{
    /// <summary>
    /// cncRead 的摘要说明
    /// </summary>
    public class cncRead : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            using(JDJS_WMS_DB_USEREntities entities=new JDJS_WMS_DB_USEREntities())
            {
                var rows = from cnc in entities.JDJS_WMS_Device_Info
                           select new
                           {
                               cnc.MachNum
                           };
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                var json = serializer.Serialize(rows);
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