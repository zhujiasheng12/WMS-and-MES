using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model.生产管理.品质
{
    /// <summary>
    /// orderClick 的摘要说明
    /// </summary>
    public class orderClick : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            List<int> id = new List<int>();
            var ordernum = context.Request["orderNumber"];
            using (JDJS_WMS_DB_USEREntities jdjs=new JDJS_WMS_DB_USEREntities())
            {
                int orderId = 0; ;
               
                var order = jdjs.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_Number == ordernum);
                if (order.Count() > 0)
                {
                    orderId = order.First().Order_ID;


                }
                var Data = jdjs.JDJS_WMS_Quality_Onmachine_Measurement_Data.Where(r => r.OrderID == orderId);
                
                foreach (var item in Data)
                {
                    if (id.Contains(Convert.ToInt32(item.SerialNumber)))
                    { }
                    else
                    {
                        id.Add(Convert.ToInt32(item.SerialNumber));
                    }
                }
            }
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var json = serializer.Serialize(id);
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