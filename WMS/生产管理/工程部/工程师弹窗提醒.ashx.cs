using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace WebApplication2.生产管理.工程部
{
    /// <summary>
    /// 工程师弹窗提醒 的摘要说明
    /// </summary>
    public class 工程师弹窗提醒 : IHttpHandler,IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            //编程人员确定毛坯弹窗
            //登录的用户主键ID
        
            int PersonID = Convert.ToInt32(context.Session["id"]);
            //返回str
            string str = "";
            using (JDJS_WMS_DB_USEREntities wms = new JDJS_WMS_DB_USEREntities ())
            {
                var orders = wms.JDJS_WMS_Order_Entry_Table.Where(r => r.Engine_Program_ManagerId == PersonID);
                foreach (var order in orders)
                {
                    int orderID = Convert.ToInt32(order.Order_ID);
                    var blankInfo = wms.JDJS_WMS_Order_Blank_Table.Where(r => r.OrderID == orderID);
                    if (blankInfo.Count() < 1)
                    {
                        str += order.Order_Number + "请尽快确认毛坯" + "/r/n";
                    }
                }
            }
            context.Response.Write(str);
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