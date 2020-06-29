using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.资材部
{
    /// <summary>
    /// 获取装夹示意图 的摘要说明
    /// </summary>
    public class 获取装夹示意图 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            var orderNum = context.Request.QueryString["orderNum"];
            var processNum = context.Request.QueryString["processNum"];
            PathInfo pathInfo = new PathInfo();
            var path = Path.Combine(pathInfo.upLoadPath(), orderNum, "工序" + processNum, "装夹示意图");
            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }
            DirectoryInfo dir = new DirectoryInfo(path);
            FileInfo[] arr = dir.GetFiles();
            if (arr.Count() > 0)
            {

                context.Response.WriteFile(arr[0].FullName);
            }
            else
            {
                context.Response.Write("");
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