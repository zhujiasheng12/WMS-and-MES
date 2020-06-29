using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.市场部
{
    /// <summary>
    /// getProjectName 的摘要说明
    /// </summary>
    public class getProjectName : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            using (JDJS_WMS_DB_USEREntities entities = new JDJS_WMS_DB_USEREntities()) {

               
                List<string> strs = new List<string>();
                foreach (var item in entities.JDJS_WMS_Order_Entry_Table)
                {
                    strs.Add(item.ProjectName);
                }
                strs = strs.Distinct().ToList();
                System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                context.Response.Write(ser.Serialize(strs));
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