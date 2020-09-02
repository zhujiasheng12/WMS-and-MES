using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace WebApplication2.生产管理.生产部.异常报备Method
{
    /// <summary>
    /// 添加异常报备 的摘要说明
    /// </summary>
    public class 添加异常报备 : IHttpHandler,IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                int orderId = int.Parse(context.Request["orderId"]);
                int processId = int.Parse(context.Request["processId"]);//没有就给0
                string type = context.Request["type"];//NC,毛坯，治具，刀具
                string remark = context.Request["remark"];//备注

                int personId = int.Parse(context.Session["id"].ToString());
                string personName = context.Session["UserName"].ToString();


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