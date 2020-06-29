using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.工程部
{
    /// <summary>
    /// upload 的摘要说明
    /// </summary>
    public class uploadProgramFile : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            var number = context.Request.Form[0];
            var processId = int.Parse(context.Request.Form[1]);
            var file = context.Request.Files;
            using (JDJS_WMS_DB_USEREntities entities = new JDJS_WMS_DB_USEREntities())
            {
                var row = entities.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == processId).FirstOrDefault();

                var programName = row.programName;
               
                PathInfo pathInfo = new PathInfo();
                var root = pathInfo.upLoadPath();

                    var fileName = Path.GetFileNameWithoutExtension(programName)+ Path.GetExtension(file[0].FileName);
                var path = Path.Combine(root, number, "加工文件", fileName);
                var Folder = Path.Combine(root, number, "加工文件");
              
                DirectoryInfo directory = new DirectoryInfo(Folder);
                FileInfo[] files = directory.GetFiles();
                for (int i = 0; i < files.Length; i++)
                {
                    files[i].Delete();
                }

                file[0].SaveAs(path);
                row.programName = fileName;
                entities.SaveChanges();

                context.Response.Write("ok");
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