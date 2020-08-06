using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.工程部.检测工程图
{
    /// <summary>
    /// 查看检测工程图 的摘要说明
    /// </summary>
    public class 查看检测工程图 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string orderNum = context.Request["orderNum"];//订单编号



            //string path = Path.Combine(@"D:\服务器文件勿动", number);
            PathInfo pathInfo = new PathInfo();
            string path = Path.Combine(pathInfo.upLoadPath(), orderNum, @"品质检测", @"检测工程图");
            DirectoryInfo root = new DirectoryInfo(path);
            if (!root.Exists)
            {
                root.Create();
            }
            FileInfo[] files = root.GetFiles();
            List<File> file = new List<File>();


            foreach (var item in files)
            {
                file.Add(new File
                {
                    fileName = item.Name,
                    filePath = item.FullName,
                    fileSize = (item.Length / 1024).ToString() + "  kB",
                    fileTime = item.CreationTime.ToString()
                });
            }
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var model = new { msg = "", code = 0, count = file.Count, data = file };
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
        public string fileSize;
        public string fileTime;

    }
}