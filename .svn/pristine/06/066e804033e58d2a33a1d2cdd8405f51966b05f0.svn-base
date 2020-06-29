using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApplication2.测试
{
    /// <summary>
    /// test 的摘要说明
    /// </summary>
    public class test : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                var workId = int.Parse(context.Request["workId"]);

                PathInfo path = new PathInfo();
                var cncLayoutPath = path.cncLayoutPath();
                var form0 = context.Request.Form[0];

                if (Directory.Exists(cncLayoutPath + workId.ToString() + ".svg"))
                {
                    File.Delete(cncLayoutPath + workId + ".svg");
                }
                FileStream fs1 = new FileStream(cncLayoutPath + workId + ".svg", FileMode.Create, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs1);

                sw.WriteLine(form0);//开始写入值
                sw.Close();
                fs1.Close();
                context.Response.Write("ok");
            }
            catch(Exception ex)
            {
                context.Response.Write(ex);
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