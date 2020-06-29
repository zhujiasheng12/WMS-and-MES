using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.资材部
{
    /// <summary>
    /// 毛坯图纸读数据 的摘要说明
    /// </summary>
    public class 毛坯图纸读数据 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var orderNum = context.Request.QueryString["orderNum"];
            PathInfo pathInfo = new PathInfo();

            var rootPath = pathInfo.upLoadPath();
            var path = Path.Combine(rootPath, orderNum, "工序1", "毛坯");
            DirectoryInfo directory = new DirectoryInfo(path);
            if (!directory.Exists)
            {
                directory.Create();
            }
            FileInfo[] files = directory.GetFiles();
            List<FileInfomation> fileInfos = new List<FileInfomation>();

            foreach (var item in files)
            {
                fileInfos.Add(new FileInfomation { fileName = item.Name, filePath = item.FullName, fileSize = item.Length.ToString() });
            }
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var model = new { code = 0, data = fileInfos };
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
    class FileInfomation
    {
        public string fileName;
        public string fileSize;
        public string filePath;



    }
}