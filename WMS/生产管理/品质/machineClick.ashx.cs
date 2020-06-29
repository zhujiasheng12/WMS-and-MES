using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model.生产管理.品质
{
    /// <summary>
    /// machineClick 的摘要说明
    /// </summary>
    public class machineClick : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            {

                
                //using (JDJS_WMS_DB_USEREntities jdjs = new JDJS_WMS_DB_USEREntities())
                //{
                //    var ordernumCnc = context.Request["orderNumber"];
                //    var Serial = int.Parse(context.Request["serialNumber"]);
                //    var OrderCncNum = context.Request["machine"];

                //    int orderIdCnc = 0;


                //    int idCnc = 0;
                //    List<string> sizename = new List<string>();
                //    var orderCnc = jdjs.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_Number == ordernumCnc);
                //    if (orderCnc.Count() > 0)
                //    {
                //        orderIdCnc = orderCnc.First().Order_ID;
                //    }

                //    var Cnc = jdjs.JDJS_WMS_Quality_Onmachine_Measurement_Data.Where(r => r.OrderID == orderIdCnc && r.SerialNumber == Serial);
                //    foreach (var item in Cnc)
                //    {
                //        var CncNum = jdjs.JDJS_WMS_Device_Info.Where(r => r.ID == item.CncID);
                //        foreach (var real in CncNum)
                //        {
                //            if (real.MachNum == OrderCncNum)
                //            {
                //                idCnc = real.ID;
                //                break;
                //            }
                //            //if (!OrderCncNum.Contains(real.MachNum))
                //            //{
                //            //    OrderCncNum.Add(real.MachNum);
                //            //}
                //        }
                //    }

                //    var info = jdjs.JDJS_WMS_Quality_Onmachine_Measurement_Data.Where(r => r.OrderID == orderIdCnc && r.SerialNumber == Serial && r.CncID == idCnc);
                //    foreach (var item in info)
                //    {
                //        if (!sizename.Contains(item.SizeName))
                //        {
                //            sizename.Add(item.SizeName);
                //        }
                //    }

                //    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                //    var json = serializer.Serialize(sizename);
                //    context.Response.Write(json);
                //}
            }
            using (JDJS_WMS_DB_USEREntities jdjs = new JDJS_WMS_DB_USEREntities())
            {
                var ordernumCnc = context.Request["orderNumber"];
                var Serial = int.Parse(context.Request["serialNumber"]);
                var OrderCncNum = context.Request["machine"];
                var Size = context.Request["size"];

                int orderIdCnc = 0;

                int idCnc = 0;

               
                //List<double> val = new List<double>();
                List<Val> vallists = new List<Val>();

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
                var SerialNumbers1 = jdjs.JDJS_WMS_Quality_Onmachine_Measurement_Data.Where(r => r.OrderID == orderIdCnc && r.CncID == idCnc).ToList();
                var SerialNumbers = SerialNumbers1.Where((r, i) => SerialNumbers1.FindIndex(p => p.SizeName == r.SizeName) == i);
                foreach (var item1 in SerialNumbers)
                {

                    List<Val> vallist = new List<Val>();
                    var info = jdjs.JDJS_WMS_Quality_Onmachine_Measurement_Data.Where(r => r.OrderID == orderIdCnc && r.SerialNumber == Serial && r.CncID == idCnc&&r.SizeName==item1.SizeName);
                  
                    foreach (var item in info)
                    {
                       
                        {
                            Val val = new Val();
                            val.Max = Convert.ToDouble(item.StandardValue + item.ToleranceRangeMax);
                            val.Min = Convert.ToDouble(item.StandardValue +item.ToleranceRangeMin);
                            val.Standard = Convert.ToDouble(item.StandardValue);
                            val.Measure = Convert.ToDouble(item.Measurements);
                            val.sizeName = item.SizeName;
                            vallist.Add(val);
                        }
                    }

                    vallists.Add(new Val { Vals = vallist });
                };
             

                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                var json = serializer.Serialize(vallists);
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
    public class Val
    {
        public double Max;
        public double Min;
        public double Standard;
        public double Measure;
        public string sizeName;
       public  List<Val> Vals;
    }
}