using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.品质.送检模块ashx
{
    /// <summary>
    /// 读取送检申请 的摘要说明
    /// </summary>
    public class 读取送检申请 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                var infos = InspectManage.GetAllInspectInfo();
                    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                var json = serializer.Serialize(infos);
                context.Response.Write(json);
                return;
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