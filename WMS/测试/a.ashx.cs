using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.测试
{
    /// <summary>
    /// a 的摘要说明
    /// </summary>
    public class a : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/event-stream";
            context.Response.Write("data:Hello World\n\n");
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