using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.市场部
{
    /// <summary>
    /// upload 的摘要说明
    /// </summary>
    public class upload : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            var number = context.Request.Form[0];
            var file = context.Request.Files;
            //var root = @"D://服务器文件勿动";
            PathInfo pathInfo = new PathInfo();
            var root = pathInfo.upLoadPath();
            
            for (int i = 0; i < file.Count; i++)
            {
                var fileName = file[i].FileName;
                var path = Path.Combine(root, number,@"客供图纸", fileName);
                file[i].SaveAs(path);
            }


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