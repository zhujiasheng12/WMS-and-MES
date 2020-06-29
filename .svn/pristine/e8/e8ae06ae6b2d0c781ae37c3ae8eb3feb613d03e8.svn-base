using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model.生产管理.品质
{
    /// <summary>
    /// sizeClick 的摘要说明
    /// </summary>
    public class sizeClick : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            using(JDJS_WMS_DB_USEREntities jdjs=new JDJS_WMS_DB_USEREntities())
            {
                var ordernumCnc = context.Request["orderNumber"];
                var Serial = int.Parse(context.Request["serialNumber"]);
                var OrderCncNum = context.Request["machine"];
                var Size = context.Request["size"];

                int orderIdCnc = 0;
               
                int idCnc = 0;
               
                List<Val> vallist = new List<Val>();
                //List<double> val = new List<double>();


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
                        if (real.MachNum == OrderCncNum)
                        {
                            idCnc = real.ID;
                            break;
                        }
                    }
                }

                var info = jdjs.JDJS_WMS_Quality_Onmachine_Measurement_Data.Where(r => r.OrderID == orderIdCnc && r.SerialNumber == Serial && r.CncID == idCnc);
                foreach (var item in info)
                {
                    if (item.SizeName == Size)
                    {
                        Val val = new Val();
                        val.Max  = Convert.ToDouble(item.StandardValue + item.ToleranceRangeMax);
                        val.Min = Convert.ToDouble(item.StandardValue - item.ToleranceRangeMin);
                        val.Standard = Convert.ToDouble(item.StandardValue);
                        val.Measure = Convert.ToDouble(item.Measurements);
                        vallist.Add(val);
                    }
                }

                        System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                var json = serializer.Serialize(vallist);
                context.Response.Write(json);
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