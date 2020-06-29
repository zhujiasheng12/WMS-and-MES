using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace WebApplication2.生产管理.市场部
{
    /// <summary>
    /// fileDelete 的摘要说明
    /// </summary>
    public class fileDelete : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            var number = context.Request["number"];
            FileInfo file = new FileInfo(number);
            file.Delete();
            context.Response.Write("ok");
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