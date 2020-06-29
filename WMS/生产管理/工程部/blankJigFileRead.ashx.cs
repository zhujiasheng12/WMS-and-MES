using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace WebApplication2.生产管理.工程部
{
    /// <summary>
    /// blankJigFileRead 的摘要说明
    /// </summary>
    public class blankJigFileRead : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            
            var processId =int.Parse( context.Request.QueryString["processId"]);//工序Id
            var type = context.Request.QueryString["type"];//判断毛坯还是治具
            using(JDJS_WMS_DB_USEREntities entities=new JDJS_WMS_DB_USEREntities())
            {
                var row = from process in entities.JDJS_WMS_Order_Process_Info_Table
                              from order in entities.JDJS_WMS_Order_Entry_Table
                              where process.ID == processId & process.OrderID == order.Order_ID
                              select new
                              {
                                  process.ProcessID,
                                  order.Order_ID,
                                  order.Order_Number
                              };
                var orderNum = row.First().Order_Number;
                var processNum = row.First().ProcessID;
                PathInfo path = new PathInfo();
                var filePath = "";
                if (type == "毛坯")
                {
                   filePath = Path.Combine(path.upLoadPath(), orderNum, "工序1" ,"毛坯");
                }
                else if(type=="治具")
                {
                     filePath = Path.Combine(path.upLoadPath(), orderNum, "工序" + processNum,"治具");
                }
                else if (type == "编程文件")
                {
                    filePath = Path.Combine(path.upLoadPath(), orderNum, "工序" + processNum, "编程文件");
                }
                else if (type == "工艺文件")
                {
                    filePath = Path.Combine(path.upLoadPath(), orderNum, "工序" + processNum, "工艺文件");
                }

                DirectoryInfo directory = new DirectoryInfo(filePath);
                if (!directory.Exists)
                {
                    directory.Create();
                }
                FileInfo[] files = directory.GetFiles();
                List<FileInfomation> fileInfos = new List<FileInfomation>();

                foreach (var item in files)
                {
                    fileInfos.Add(new FileInfomation { fileName = item.Name, filePath = item.FullName, fileSize = item.Length.ToString(),uploadTime=item.LastWriteTime.ToString () });
                }
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                var model = new { code = 0, data = fileInfos };
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
     class FileInfomation{
        public string fileName;
        public string fileSize;
        public string filePath;
        public string uploadTime;


    }
}