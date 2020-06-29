using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.工程部
{
    /// <summary>
    /// uploadPPT 的摘要说明
    /// </summary>
    public class virtualUpload : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var files = context.Request.Files;
            var orderNumber = context.Request.Form[0];

           
            {

                PathInfo pathInfo = new PathInfo();
                var folderPath = Path.Combine(pathInfo.upLoadPath(), orderNumber, "虚拟加工方案文档");
                DirectoryInfo directory = new DirectoryInfo(folderPath);
                if (!directory.Exists)
                {
                    directory.Create();
                }
                for (int i = 0; i < files.Count; i++)
                {
                    var path = Path.Combine(folderPath, files[i].FileName);
                    files[i].SaveAs(path);
                }
              
           
                DirectoryInfo directory1 = new DirectoryInfo(Path.Combine(pathInfo.upLoadPath(), orderNumber));
                FTPDLL.FTPUser fTPUser = new FTPDLL.FTPUser();
               
                string ftpErr = "";
                string FtpUrl = pathInfo.ftpUrl();
                string UserName = pathInfo.ftpUser();
                string PassCode = pathInfo.ftpPassword();
                string errStr = "";


                context.Response.Write("ok");
                //if (fTPUser.Login(FtpUrl, UserName, PassCode, ref errStr)) {

                //    if (fTPUser.isExist(orderNumber, FtpUrl,ref errStr))
                //    {
                //        fTPUser.DirDelete(directory1, FtpUrl, ref errStr);
                      
                //    }
                //   if(fTPUser.DireUpload(directory1, FtpUrl, ref errStr))
                //    {
                //        context.Response.Write("ok");
                //    }
                //    else
                //    {
                //        context.Response.Write(errStr);
                //    }

                //} else
                //{

                //    context.Response.Write(errStr);
                //}
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