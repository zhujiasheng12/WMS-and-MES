using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.维修部.精度校验
{
    /// <summary>
    /// optionRead 的摘要说明
    /// </summary>
    public class optionRead : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            List<string> str = new List<string>();
            using (JDJS_WMS_DB_USEREntities  wms = new JDJS_WMS_DB_USEREntities ())
            {
                var desc = wms.JDJS_WMS_Device_Accuracy_Verification_Plan.ToList();
                foreach (var item in desc)
                {
                    if (!str.Contains(item.Name))
                    {
                        str.Add(item.Name);
                    }
                }
            }
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var json = serializer.Serialize(str);
            context.Response.Write(json);
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