using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApplication2.工厂管理.设计布局图
{
    /// <summary>
    /// 删除布局图 的摘要说明
    /// </summary>
    public class 删除布局图 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                var workId = context.Request["workId"];
                PathInfo path = new PathInfo();
                var layoutPath = path.cncLayoutPath() + workId + ".svg";
                File.Delete(layoutPath);
                context.Response.Write("ok");
            }
            catch(Exception ex)
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