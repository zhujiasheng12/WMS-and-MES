using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.品质
{
    /// <summary>
    /// 在机测量读数据 的摘要说明
    /// </summary>
    public class 在机测量读数据 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var cncId = int.Parse(context.Request["cncId"]);
            int orderId=0;
            int Serial =0;
          
            {
                
                using (JDJS_WMS_DB_USEREntities jdjs = new JDJS_WMS_DB_USEREntities())
                {

                    var arr = jdjs.JDJS_WMS_Quality_Onmachine_Measurement_Data.Where(r => r.CncID == cncId).ToList();
                    if (arr.Count() > 0)
                    {
                        orderId = Convert.ToInt32(arr.Last().OrderID);
                         Serial = Convert.ToInt32( arr.Last().SerialNumber);
                      
                    }
                   
                   


               
                    List<Val> vallists = new List<Val>();


                    var SerialNumbers1 = jdjs.JDJS_WMS_Quality_Onmachine_Measurement_Data.Where(r => r.OrderID == orderId && r.CncID == cncId).ToList();
                    var SerialNumbers = SerialNumbers1.Where((r, i) => SerialNumbers1.FindIndex(p => p.SizeName == r.SizeName) == i);
                    foreach (var item1 in SerialNumbers)
                    {

                        List<Val> vallist = new List<Val>();
                        var info = jdjs.JDJS_WMS_Quality_Onmachine_Measurement_Data.Where(r => r.OrderID == orderId && r.SerialNumber == Serial && r.CncID == cncId && r.SizeName == item1.SizeName);

                        foreach (var item in info)
                        {

                            {
                                Val val = new Val();
                                val.Max = Convert.ToDouble(item.StandardValue + item.ToleranceRangeMax);
                                val.Min = Convert.ToDouble(item.StandardValue + item.ToleranceRangeMin);
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
        public List<Val> Vals;
    }
}