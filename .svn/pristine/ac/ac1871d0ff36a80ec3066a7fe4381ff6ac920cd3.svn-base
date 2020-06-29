using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.工程部
{
    /// <summary>
    /// virtualProEditRead 的摘要说明
    /// </summary>
    public class virtualProEditRead : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var processId = int.Parse(context.Request["processId"]);
            using (JDJS_WMS_DB_USEREntities entities=new JDJS_WMS_DB_USEREntities())
            {
                var row = entities.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == processId);
                var model = new
                {
                    process = row.First().ProcessID,
                    time = row.First().ProcessTime,
                    deviceType = row.First().DeviceType,
                    processId = row.First().ID,
                    cncNumber=row.First().MachNumber
                };
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