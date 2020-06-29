using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理
{
    /// <summary>
    /// downLoadPathRead 的摘要说明
    /// </summary>
    public class downLoadPathRead : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            PathInfo pathInfo = new PathInfo();
            context.Response.Write(pathInfo.downLoadPath());
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