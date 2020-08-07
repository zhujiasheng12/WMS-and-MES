using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.品质.送检模块ashx
{
    /// <summary>
    /// 上传测量报告文件 的摘要说明
    /// </summary>
    public class 上传测量报告文件 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                int id = int.Parse(context.Request["id"]);//处理的申请主键ID
                var files = context.Request.Files;
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
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                for (int i = 0; i < files.Count; i++)
                {
                    files[i].SaveAs(path + "//" + files[i].FileName);
                }
                context.Response.Write("ok");
            }
            catch (Exception ex)
            {
                context.Response.Write(ex.Message);
                return;
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