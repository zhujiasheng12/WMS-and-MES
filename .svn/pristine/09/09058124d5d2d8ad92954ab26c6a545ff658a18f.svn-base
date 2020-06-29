using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace WebApplication2.生产管理.工程部
{
    /// <summary>
    /// fileDelete 的摘要说明
    /// </summary>
    public class fileDeleteVirtual : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            var filePath = context.Request["filePath"];
            var orderNumber= context.Request["orderNumber"];
            FileInfo file = new FileInfo(filePath);
            file.Delete();


            PathInfo pathInfo = new PathInfo();
            DirectoryInfo directory1 = new DirectoryInfo(Path.Combine(pathInfo.upLoadPath(), orderNumber));
            FTPDLL.FTPUser fTPUser = new FTPDLL.FTPUser();
         
            string ftpErr = "";
            string FtpUrl = pathInfo.ftpUrl();
            string UserName = pathInfo.ftpUser();
            string PassCode = pathInfo.ftpPassword();
            string errStr = "";




            context.Response.Write("ok");
            //if (fTPUser.Login(FtpUrl, UserName, PassCode, ref errStr))
            //{

            //    if (fTPUser.isExist(orderNumber, FtpUrl, ref errStr))
            //    {
            //        fTPUser.DirDelete(directory1, FtpUrl, ref errStr);

            //    }
            //    if (fTPUser.DireUpload(directory1, FtpUrl, ref errStr))
            //    {
            //        context.Response.Write("ok");
            //    }
            //    else
            //    {
            //        context.Response.Write(errStr);
            //    }

            //}
            //else
            //{

            //    context.Response.Write(errStr);
            //}
         
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