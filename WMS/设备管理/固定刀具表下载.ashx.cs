using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace WebApplication2.设备管理
{
    /// <summary>
    /// 固定刀具表下载 的摘要说明
    /// </summary>
    public class 固定刀具表下载 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var cncTypeId = context.Request["cncTypeId"];
            PathInfo pathInfo = new PathInfo();
            var directoryPath = Path.Combine(pathInfo.upLoadPath(), "固定刀具表", cncTypeId);
          
            if (File.Exists(Path.Combine(directoryPath, "固定刀具表.xls")))
            {
                context.Response.Write(Path.Combine(pathInfo.downLoadPath(), "固定刀具表", cncTypeId, "固定刀具表.xls"));
                return;
            }
            if (File.Exists(Path.Combine(directoryPath, "固定刀具表.xlsx")))
            {
                context.Response.Write(Path.Combine(pathInfo.downLoadPath(), "固定刀具表", cncTypeId, "固定刀具表.xlsx"));
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