using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model.人员管理
{
    /// <summary>
    /// modify 的摘要说明
    /// </summary>
    public class modify : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var id = int.Parse(context.Request["id"]);
            using(JDJS_WMS_DB_USEREntities entities1=new JDJS_WMS_DB_USEREntities())
            {
                var row = entities1.JDJS_WMS_Staff_Info.Where(r => r.id == id).First();
                var rowFather = entities1.JDJS_WMS_Staff_Info.Where(r => r.id == row.parentId).First();
                string[] arr;
                if (row.limit == null)
                {
                    arr = null;
                }
                else
                {
                    arr = row.limit.Split(',');
                }
            
                var model = new
                {
                    parent = rowFather.staff,
                    staff = row.staff,
                    position = row.position,
                    tel = row.tel,
                    user = row.users,
                    password = row.password,
                    remark = row.remark,
                    limit=arr,
                    mailbox=row.mailbox

                };
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                var json = serializer.Serialize(model);
                context.Response.Write(json);
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