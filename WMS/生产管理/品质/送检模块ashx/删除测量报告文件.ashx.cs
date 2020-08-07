using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.品质.送检模块ashx
{
    /// <summary>
    /// 删除测量报告文件 的摘要说明
    /// </summary>
    public class 删除测量报告文件 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var path = context.Request["path"];//文件全路径
            FileInfo file = new FileInfo(path);
            if (System.IO.File.Exists(path))
            {
                file.Delete();
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