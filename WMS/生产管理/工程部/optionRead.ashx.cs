using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model.生产管理.工程部
{
    /// <summary>
    /// optionRead 的摘要说明
    /// </summary>
    public class optionRead : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            using (JDJS_WMS_DB_USEREntities entities=new JDJS_WMS_DB_USEREntities())
            {
                List<Option> options = new List<Option>();
                var rows = entities.JDJS_WMS_Device_Type_Info ;
                if (rows.Count() > 0)
                {
                    foreach (var item in rows)
                    {
                        options.Add(new Option { machType = item.Type, id = item.ID.ToString() });
                    }
                    var json = serializer.Serialize(options);
                    context.Response.Write(json);
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
    class Option
    {
        public string machType;
        public string id;
    }
}