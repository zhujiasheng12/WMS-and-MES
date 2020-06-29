using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace WebApplication2.生产管理.工程部.Controller
{
    /// <summary>
    /// 判断工艺规划负责人 的摘要说明
    /// </summary>
    public class 判断工程编程负责人 : IHttpHandler,IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            var orderId = int.Parse(context.Request["orderId"]);
            JDJS_WMS_DB_USEREntities entities = new JDJS_WMS_DB_USEREntities();
            var Engine_Program_ManagerId = entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == orderId).First().Engine_Program_ManagerId;
            var sessionId =int.Parse( context.Session["id"].ToString());
            if(Engine_Program_ManagerId == sessionId)
            {
                context.Response.Write("ok");
            }
            else
            {
                context.Response.Write("您不是该功能负责人");
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