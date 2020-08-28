using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using WebApplication2.生产管理.资材部.夹具管理.特殊治具管理Access数据库;

namespace WebApplication2.生产管理.资材部.夹具管理.录入系统治具库
{
    /// <summary>
    /// 系统治具根据ID获取SurfMill文件 的摘要说明
    /// </summary>
    public class 系统治具根据ID获取SurfMill文件 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                int id = int.Parse(context.Request["id"]);
                string fxNum = "";
                using (FixtureModel wms = new FixtureModel())
                {
                    var fx = wms.JDJS_WMS_Fixture_System_Table.Where(r => r.Id == id).FirstOrDefault();
                    if (fx != null)
                    {
                        fxNum = fx.FileName;
                    }
                }
                PathInfo info = new PathInfo();
                string path = System.IO.Path.Combine(info.GetFixtrue_SurfMillFilePath(), fxNum);

                List<FileInfo> files = new List<FileInfo>();
                if (System.IO.File.Exists(path))
                {
                    files.Add(new FileInfo(path));
                }
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
            catch (Exception ex)
            {
                context.Response.Write(ex.Message);
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