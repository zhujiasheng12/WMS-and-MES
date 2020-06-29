using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.工程部.装夹示意图
{
    /// <summary>
    /// upload 的摘要说明
    /// </summary>
    public class upload : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var  ClampingFile=context.Request.Files[0];
            var orderNum = context.Request.Form["orderNum"];
            var processNum = context.Request.Form["workNum"];

            PathInfo pathInfo1 = new PathInfo();
            var ClampingFileDir = Path.Combine(pathInfo1.upLoadPath(), orderNum, "工序" + processNum, "装夹示意图");
            if (Directory.Exists(ClampingFileDir))
            {
                Directory.Delete(ClampingFileDir, true);

            }
            Directory.CreateDirectory(ClampingFileDir);
            var ClampingFilePath = Path.Combine(ClampingFileDir, ClampingFile.FileName);
            ClampingFile.SaveAs(ClampingFilePath);
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