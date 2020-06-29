using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model.人员管理
{
    /// <summary>
    /// submitDepartment 的摘要说明
    /// </summary>
    public class submitDepartment : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var id = int.Parse(context.Request["id"]);
            var department = context.Request["department"];
            var remark = context.Request["remark"];
            using (JDJS_WMS_DB_USEREntities entities1=new JDJS_WMS_DB_USEREntities())
            {
                var row = new JDJS_WMS_Staff_Info { staff= department, parentId = id,
                remark=remark};
                entities1.JDJS_WMS_Staff_Info.Add(row);
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