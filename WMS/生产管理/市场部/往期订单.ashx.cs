using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.市场部
{
    /// <summary>
    /// 往期订单 的摘要说明
    /// </summary>
    public class 往期订单 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<string> OrderNum = new List<string>();
            using (JDJS_WMS_DB_USEREntities  wms = new JDJS_WMS_DB_USEREntities ())
            {
                var Intention = wms.JDJS_WMS_Order_Intention_History_Table.ToList();
                foreach (var item in Intention)
                {

                    int id = Convert.ToInt32(item.OrderID);
                    var rows = wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == id);
                    if (rows.Count() > 0)
                    {
                        var order = wms.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == id);
                        var str = order.First().Order_Number;
                        OrderNum.Add(str);
                    }
                  
                }
            }
            var json = serializer.Serialize(OrderNum);
            context.Response.Write(json);
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