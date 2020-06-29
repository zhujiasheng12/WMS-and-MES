using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.设备管理
{
    /// <summary>
    /// 固定刀具表删除行 的摘要说明
    /// </summary>
    public class 固定刀具表删除行 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var id = int.Parse(context.Request["id"]);
            context.Response.Write(id);
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