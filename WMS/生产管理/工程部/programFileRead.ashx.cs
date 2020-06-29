using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.工程部
{
    /// <summary>
    /// fileRead 的摘要说明
    /// </summary>
    public class programFileRead : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var page = int.Parse(context.Request["page"]);
            var limit = int.Parse(context.Request["limit"]);
            context.Response.ContentType = "text/plain";
            var number = context.Request["number"];
            var processId =int.Parse( context.Request["processId"]);
            var fileName = "";
            using (JDJS_WMS_DB_USEREntities entities=new JDJS_WMS_DB_USEREntities())
            {
                fileName = entities.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == processId).First().programName;
            }
            PathInfo pathInfo = new PathInfo();
            string path = Path.Combine(pathInfo.upLoadPath(), number,"加工文件");
            
            DirectoryInfo root = new DirectoryInfo(path);
            if (!root.Exists)
            {
                root.Create();
            }

            if (fileName == null)
            {
                fileName = "";
            }
            FileInfo[] files = root.GetFiles(fileName);
            List<File> file = new List<File>();


            foreach (var item in files)
            {
                file.Add(new File { fileName = item.Name, filePath = item.FullName,uploadTime=item.LastWriteTime.ToString(),
                fileSize=(item.Length/1024).ToString()+"  Kb"});
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
    //class File
    //{
    //    public string fileName;
    //    public string filePath;
    //    public string  fileSize;
    //}
}