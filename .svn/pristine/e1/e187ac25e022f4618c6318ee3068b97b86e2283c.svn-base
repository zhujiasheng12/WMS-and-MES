using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.工程部
{
    /// <summary>
    /// 重新上传毛坯治具图纸 的摘要说明
    /// </summary>
    public class 重新上传毛坯治具图纸 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var files = context.Request.Files;
            var orderNum = context.Request.Form["orderNum"];
            var processNum = context.Request.Form["processNum"];
            var type = context.Request.Form["type"];
            var processId = context.Request.Form["processId"];

            PathInfo pathInfo = new PathInfo();
            var rootPath = pathInfo.upLoadPath();
            var path = "";
            if (type == "毛坯")
            {
                 path = Path.Combine(rootPath, orderNum, "工序1" , type);
            }
            else if (type == "治具")
            {
                 path = Path.Combine(rootPath, orderNum, "工序" + processNum, type);
            }
            else if (type == "编程文件")
            {
                path = Path.Combine(rootPath, orderNum, "工序" + processNum, type);
            }
            else if (type == "工艺文件")
            {
                path = Path.Combine(rootPath, orderNum, "工序" + processNum, type);
            }

            for (int i = 0; i < files.Count; i++)
            {
                files[i].SaveAs(Path.Combine(path , files[i].FileName));
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