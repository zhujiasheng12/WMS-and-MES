using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model.人员管理
{
    /// <summary>
    /// modifySubmit 的摘要说明
    /// </summary>
    public class modifySubmit : IHttpHandler
    {

        System.Web.Script.Serialization.JavaScriptSerializer Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

        public void ProcessRequest(HttpContext context)
        {
            try { 
            context.Response.ContentType = "text/plain";
            var limit = context.Request.Form[0];
            var form = context.Request.Form[1];
            var id = int.Parse(context.Request.Form[2]);
            var obj = Serializer.Deserialize<Form>(form);
            using (JDJS_WMS_DB_USEREntities entities1 = new JDJS_WMS_DB_USEREntities())
            {
                if (obj.user == "")
                {
                    var row = entities1.JDJS_WMS_Staff_Info.Where(r => r.id == id).First();
                    row.limit = null;
                    row.password =null;
                    row.users = null;
                    row.tel = obj.tel;
                    row.position = obj.position;
                    row.staff = obj.staffName;

                }
                else
                {
                    
                 

                    var judges = entities1.JDJS_WMS_Staff_Info.Where(r => r.users == obj.user);
                    if (judges.Count() > 0) {
                        if (judges.First().id != id) {
                            context.Response.Write("登录账户已存在");
                            return;
                        }
                    }
                  
                  
                        if (obj.password == "")
                        {
                            context.Response.Write("密码不能为空");
                            return;
                        }
                      
                            var row = entities1.JDJS_WMS_Staff_Info.Where(r => r.id == id).First();


                            row.staff = obj.staffName;
                            row.position = obj.position;
                            row.tel = obj.tel;

                            row.users = obj.user;
                            row.password = obj.password;
                            row.limit = limit;
                            row.remark = obj.remark;
                            row.mailbox = obj.mailbox;

                }
                entities1.SaveChanges();
            }


            context.Response.Write("ok");
            }
            catch (Exception ex) {
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
    class repeat
    {
        public string user;
        public int id;
    }
}