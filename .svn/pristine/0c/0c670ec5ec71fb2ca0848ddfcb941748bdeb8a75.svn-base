using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
namespace WebApplication2.人员管理
{
    /// <summary>
    /// 获取所在部门 的摘要说明
    /// </summary>
    public class 获取所在部门 : IHttpHandler,IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            var userId = Convert.ToInt32(context.Session["id"]);
            using(JDJS_WMS_DB_USEREntities entities=new JDJS_WMS_DB_USEREntities())
            {
                var departmentId = entities.JDJS_WMS_Staff_Info.Where(r => r.id == userId).FirstOrDefault().parentId;
                var department = entities.JDJS_WMS_Staff_Info.Where(r => r.id == departmentId).FirstOrDefault().staff;
                context.Response.Write(department);
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