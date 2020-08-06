using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.工程部.检测工程图
{
    /// <summary>
    /// 上传检测工程图 的摘要说明
    /// </summary>
    public class 上传检测工程图 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                
                string orderNum = context.Request["orderNum"];//订单编号
                var files = context.Request.Files;
                PathInfo pathInfo = new PathInfo();
                string formatPath = pathInfo.upLoadPath();

                string filePath = "";
                filePath = System.IO.Path.Combine(formatPath, orderNum, @"品质检测", @"检测工程图");
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                for (int i = 0; i < files.Count; i++)
                {
                    files[i].SaveAs(filePath + "//" + files[i].FileName);
                }
                context.Response.Write("ok");
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