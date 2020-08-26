using DocumentFormat.OpenXml.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.资材部.夹具管理.特殊治具设计文件管理
{
    /// <summary>
    /// 上传设计文件 的摘要说明
    /// </summary>
    public class 上传设计文件 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string fxNum = context.Request["fxNum"];
                var files = context.Request.Files;
                PathInfo info = new PathInfo();
                string path = System.IO.Path.Combine(info.upLoadPath(),@"特殊治具管理", fxNum,@"设计文件");
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                for (int i = 0; i < files.Count; i++)
                {
                    files[i].SaveAs(System.IO.Path.Combine(path, files[i].FileName));
                }
                context.Response.Write("ok");
                return;
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