using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model.人员管理
{
    /// <summary>
    /// tree 的摘要说明
    /// </summary>
    public class tree : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            
               
            
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