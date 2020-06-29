using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.市场部
{
    /// <summary>
    /// fileRead 的摘要说明
    /// </summary>
    public class fileRead : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var page = int.Parse(context.Request["page"]);
            var limit = int.Parse(context.Request["limit"]);
            context.Response.ContentType = "text/plain";
            var number = context.Request["number"];

            //string path = Path.Combine(@"D:\服务器文件勿动", number);
            PathInfo pathInfo = new PathInfo();
            string path= Path.Combine(pathInfo.upLoadPath(), number,@"客供图纸");
            DirectoryInfo root = new DirectoryInfo(path);
            if (!root.Exists)
            {
                root.Create();
            }
            FileInfo[] files = root.GetFiles();
            List<File> file = new List<File>();


            foreach (var item in files)
            {
                file.Add(new File { fileName = item.Name, filePath = item.FullName,
                fileSize=(item.Length/1024).ToString()+"  kB",
                fileTime=item.CreationTime.ToString()
});
            }
            var file1 =file.Skip((page - 1) * limit).Take(limit);
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var model = new { msg = "", code = 0, count = file.Count, data = file1 };
            var json = serializer.Serialize(model);
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
    class File
    {
        public string fileName;
        public string filePath;
        public string  fileSize;
        public string fileTime;
    }
}