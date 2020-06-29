using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model.人员管理
{
    /// <summary>
    /// modifyPasswordSubmit 的摘要说明
    /// </summary>
    public class modifyPasswordSubmit : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var id = int.Parse(context.Request["id"]);
            var oldPassword = context.Request["oldPassword"];
            var newPassword = context.Request["newPassword"];
            var summitPassword = context.Request["submitPassword"];
            using(JDJS_WMS_DB_USEREntities entities1=new JDJS_WMS_DB_USEREntities())
            {
                var row = entities1.JDJS_WMS_Staff_Info.Where(r => r.id == id).First();
                if (row.password == oldPassword)
                {
                    if(newPassword== summitPassword)
                    {
                        row.password = newPassword;
                        entities1.SaveChanges();
                        context.Response.Write("ok");
                    }
                    else
                    {
                        context.Response.Write("两次新密码不一致");
                        return;
                    }
                }
                else
                {
                    context.Response.Write("原始密码错误");
                    return;
                }
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