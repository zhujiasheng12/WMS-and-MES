using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model.生产管理.品质
{
    /// <summary>
    /// serialClick 的摘要说明
    /// </summary>
    public class serialClick : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            List<string> OrderCncNum = new List<string>();
            var ordernumCnc = context.Request["orderNumber"];
            var Serial =int.Parse(context.Request["serialNumber"]);
            using(JDJS_WMS_DB_USEREntities jdjs=new JDJS_WMS_DB_USEREntities())
            {
                int orderIdCnc = 0;
              
                
                var orderCnc = jdjs.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_Number == ordernumCnc);
                if (orderCnc.Count() > 0)
                {
                    orderIdCnc = orderCnc.First().Order_ID;
                }

                var Cnc = jdjs.JDJS_WMS_Quality_Onmachine_Measurement_Data.Where(r => r.OrderID == orderIdCnc && r.SerialNumber == Serial);
                foreach (var item in Cnc)
                {
                    var CncNum = jdjs.JDJS_WMS_Device_Info.Where(r => r.ID == item.CncID);
                    foreach (var real in CncNum)
                    {
                        if (!OrderCncNum.Contains(real.MachNum))
                        {
                            OrderCncNum.Add(real.MachNum);
                        }
                    }
                }
            }
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var json = serializer.Serialize(OrderCncNum);
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