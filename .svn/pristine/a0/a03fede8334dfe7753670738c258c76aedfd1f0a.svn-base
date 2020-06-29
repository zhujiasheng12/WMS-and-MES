using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApplication2.Model.生产管理.工程部
{
    /// <summary>
    /// editFileP 的摘要说明
    /// </summary>
    public class editFileP : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var file = context.Request.Files;
            var processId = int.Parse(context.Request.Form["processId"]);
            using (JDJS_WMS_DB_USEREntities entities=new JDJS_WMS_DB_USEREntities())
            {
                var row = entities.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == processId).First();
                var orderNumber = entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == row.OrderID).First().Order_Number;

               var exten= Path.GetExtension(file[0].FileName);
                var fileName = orderNumber + "-P" + row.ProcessID + exten;
                var oldFileName = row.programName;

                PathInfo pathInfo = new PathInfo();
               
                DirectoryInfo directoryP = new DirectoryInfo(Path.Combine(pathInfo.upLoadPath(), orderNumber, "加工文件"));
                if (!directoryP.Exists)
                {
                    directoryP.Create();
                }

                DirectoryInfo directoryT = new DirectoryInfo(Path.Combine(pathInfo.upLoadPath(), orderNumber, "刀具表"));
                if (!directoryT.Exists)
                {
                    directoryT.Create();
                }
                var path = Path.Combine(pathInfo.upLoadPath(), orderNumber,"加工文件", fileName);
               
                if (oldFileName != null)
                {
                    var oldPath = Path.Combine(pathInfo.upLoadPath(), orderNumber, "加工文件", oldFileName);
                    int i = 1;
                    while (File.Exists(oldPath + "-" + i.ToString()))
                    {
                        i++;
                    }
                    if (File.Exists(oldPath)) {
                        File.Copy(oldPath, oldPath + "-" + i.ToString());
                        File.Delete(oldPath);
                    }
                  
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