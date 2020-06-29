using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace WebApplication2.生产管理.工程部.装夹示意图
{
    /// <summary>
    /// del 的摘要说明
    /// </summary>
    public class del : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var orderNum = context.Request.Form["orderNum"];
            var processNum = context.Request.Form["workNum"];
            PathInfo pathInfo = new PathInfo();
            var path = Path.Combine(pathInfo.upLoadPath(), orderNum, "工序" + processNum, "装夹示意图");
            if (Directory.Exists(path)) {
                Directory.Delete(path,true);
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