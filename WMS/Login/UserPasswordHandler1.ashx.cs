using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using WebApplication2;

namespace WebApplication2.Model.登录界面
{
    /// <summary>
    /// JDJS_WMS_Tool_Users_Login_InfoHandler1 的摘要说明
    /// </summary>
    public class JDJS_WMS_Tool_Users_Login_InfoHandler1 : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string userName = context.Request.Form[0];
            string pwd = context.Request.Form[1];
            try
            {
                using (JDJS_WMS_DB_USEREntities pm = new JDJS_WMS_DB_USEREntities() )
                {
                    var users = pm.JDJS_WMS_Staff_Info.Where(r => r.users == userName);
                    if (users.Count() < 1)
                    {
                        context.Response.Write("该用户不存在！");
                    }
                    else
                    {
                        var user = users.First();
                        if (user.password == pwd)
                        {
                            context.Session.Timeout = 525600;
                            context.Session["id"] = user.id;
                            context.Session["UserName"] = user.staff;
                            context.Session["Limit"] = user.limit;
                            context.Response.Write("ok");

                        }
                        else
                        {
                            context.Response.Write("密码不正确");
                        }
                    }
                }
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