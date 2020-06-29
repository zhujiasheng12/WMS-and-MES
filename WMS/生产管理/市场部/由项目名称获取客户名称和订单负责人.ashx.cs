using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.市场部
{
    /// <summary>
    /// 由项目名称获取客户名称和订单负责人 的摘要说明
    /// </summary>
    /// 
   
    public class 由项目名称获取客户名称和订单负责人 : IHttpHandler
    {
        System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
        public void ProcessRequest(HttpContext context)
        {
            var projectName = context.Request.Form["projectName"];
            using (JDJS_WMS_DB_USEREntities entities = new JDJS_WMS_DB_USEREntities()) {
                var row = from order in entities.JDJS_WMS_Order_Entry_Table.Where(r => r.ProjectName == projectName)
                          from guide in entities.JDJS_WMS_Order_Guide_Schedu_Table
                          where order.Order_ID == guide.OrderID
                          select new
                          {
                              order.Order_Leader,
                              guide.ClientName
                          };

                if (row.Count() > 0)
                {
                    var model = new { Order_Leader = row.First().Order_Leader, clientName = row.First().ClientName };
                    var json = ser.Serialize(model);
                    context.Response.Write(json);
                }
                else {
                    var model = new { Order_Leader = "", clientName = "" };
                    var json = ser.Serialize(model);
                    context.Response.Write(json);
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