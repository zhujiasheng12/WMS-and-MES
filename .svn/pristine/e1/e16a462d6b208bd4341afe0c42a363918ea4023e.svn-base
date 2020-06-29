using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace WebApplication2.Model
{
    /// <summary>
    /// MainForm 的摘要说明
    /// </summary>
    public class Jurisdiction : IHttpHandler, IRequiresSessionState
    {
        System.Web.Script.Serialization.JavaScriptSerializer Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            var limit = context.Session["limit"];

            var username = context.Session["UserName"];
            var id = context.Session["id"];
            List<string> vs;
            if (limit != null)
            {
                vs = limit.ToString().Split(',').ToList();
            }
            else
            {
                vs = null;
            }

            var model = new { Limit = vs, UserName = username, id = id };
            var json = Serializer.Serialize(model);
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