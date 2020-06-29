using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model.人员管理
{
    /// <summary>
    /// departmentModify 的摘要说明
    /// </summary>
    public class departmentModify : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var id = int.Parse(context.Request["id"]);
            var department = context.Request["department"];
            var remark = context.Request["remark"];
            using (JDJS_WMS_DB_USEREntities entities1=new JDJS_WMS_DB_USEREntities())
            {
                var row = entities1.JDJS_WMS_Staff_Info.Where(r => r.id == id).First();
                row.staff= department;
                row.remark = remark;
                entities1.SaveChanges();
                context.Response.Write("ok");
                
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