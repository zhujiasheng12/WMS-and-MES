using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.品质.送检模块ashx
{
    /// <summary>
    /// 查看测量报告文件 的摘要说明
    /// </summary>
    public class 查看测量报告文件 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            int id = int.Parse(context.Request["id"]);//处理的申请主键ID
            string path = "";
            using (JDJS_WMS_DB_USEREntities wms = new JDJS_WMS_DB_USEREntities())
            {

                var inspect = wms.JDJS_WMS_Quality_Apply_Measure_Table.Where(r => r.ID == id).FirstOrDefault();
                if (inspect == null)
                {
                    context.Response.Write("该送检申请不存在，请确认后再试！");
                    return;
                }
                path = inspect.SavePath;

            }

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