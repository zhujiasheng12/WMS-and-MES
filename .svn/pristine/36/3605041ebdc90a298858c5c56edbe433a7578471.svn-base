using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model.人员管理
{
    /// <summary>
    /// delete 的摘要说明
    /// </summary>
    public class delete : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var id = int.Parse(context.Request["id"]);
            using (JDJS_WMS_DB_USEREntities entities1 = new JDJS_WMS_DB_USEREntities())
            {
                var rows = entities1.JDJS_WMS_Staff_Info.Where(r => r.parentId == id);
                if (rows.Count() > 0)
                {
                    context.Response.Write("请先删除所属子节点");
                }
                else
                {
                    var row = entities1.JDJS_WMS_Staff_Info.Where(r => r.id == id).First();
                    entities1.JDJS_WMS_Staff_Info.Remove(row);
                    entities1.SaveChanges();
                    context.Response.Write("ok");
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