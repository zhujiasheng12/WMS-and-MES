using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.资材部.夹具管理
{
    /// <summary>
    /// 根据订单与工序号获取已设计完成特殊治具文件 的摘要说明
    /// </summary>
    public class 根据订单与工序号获取已设计完成特殊治具文件 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string orderNum = context.Request["orderNum"];
            int processNum =int.Parse ( context.Request["processNum"]);

            string fxNum = "";
            using (JDJS_WMS_DB_USEREntities wms = new JDJS_WMS_DB_USEREntities())
            {
                var fix = wms.JDJS_WMS_Fixture_Manage_Demand_Table.Where(r => r.OrderNum == orderNum && r.ProcessNum == processNum && r.State == "审核通过").FirstOrDefault();
                if (fix != null)
                {
                    fxNum = fix.FixtureOrderNum;
                }
            }
            List<File> file = new List<File>();
            if (!string.IsNullOrWhiteSpace(fxNum))
            {
                PathInfo info = new PathInfo();
                string path = System.IO.Path.Combine(info.upLoadPath(), @"特殊治具管理", fxNum, @"设计文件");
                DirectoryInfo root = new DirectoryInfo(path);
                if (!root.Exists)
                {
                    root.Create();
                }
                FileInfo[] files = root.GetFiles();



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
}