using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.维修部.精度校验
{
    /// <summary>
    /// 读历史数据 的摘要说明
    /// </summary>
    public class 读历史数据 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var MaintenacneID =int.Parse( context.Request.Form["MaintenacneID"]);
            using (JDJS_WMS_DB_USEREntities entities = new JDJS_WMS_DB_USEREntities()) {
                var rows = entities.JDJS_WMS_Device_Accuracy_Verification_Data.Where(r => r.PlanID == MaintenacneID).ToList();
                List<object> json = new List<object>();
                foreach (var item in rows)
                {
                    json.Add(new { time = item.RecordTime.ToString(), data = item.Data });
                }
                context.Response.Write(new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(json));

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