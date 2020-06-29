using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace WebApplication2.生产管理.工程部
{
    /// <summary>
    /// virtualFile 的摘要说明
    /// </summary>
    public class virtualFile : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var orderNumber= context.Request["orderNumber"];
            using(JDJS_WMS_DB_USEREntities entities=new JDJS_WMS_DB_USEREntities())
            {
               // var orderNumber = entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == orderId).First().Order_Number;
                PathInfo pathInfo = new PathInfo();
                var path = Path.Combine(pathInfo.upLoadPath(),orderNumber, "虚拟加工方案文档");
                DirectoryInfo directory = new DirectoryInfo(path);
                if (!directory.Exists)
                {
                    directory.Create();
                }
                FileInfo[] files = directory.GetFiles();
                List<File> files1 = new List<File>();
                foreach (var item in files)
                {
                    files1.Add(new File { fileName = item.Name, filePath = item.FullName, fileSize = (item.Length / 1024).ToString() + "  Kb" });
                }
                var model = new { code = 0, msg = "", count = files1.Count, data = files1 };
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                var json = serializer.Serialize(model);
                context.Response.Write(json);
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