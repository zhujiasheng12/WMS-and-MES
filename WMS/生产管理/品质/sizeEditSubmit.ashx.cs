using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.品质
{
    /// <summary>
    /// sizeEditSubmit 的摘要说明
    /// </summary>
    public class sizeEditSubmit : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var data = context.Request["data"];
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var obj = serializer.Deserialize<SizeEditSub>(data);
            using(JDJS_WMS_DB_USEREntities entities=new JDJS_WMS_DB_USEREntities())
            {
                var id = int.Parse(obj.ID);

                var row = entities.JDJS_WMS_Quality_ManualInput_Measurement_Table.Where(r => r.ID == id);
                  if(row.Count()>0)
                {
                    row.First().Measurements = Convert.ToDouble(obj.Measurements);
                    row.First().SizeName = obj.SizeName;
                    row.First().SizeNumber = int.Parse(obj.SizeNumber);
                    row.First().StandardValue = Convert.ToDouble(obj.StandardValue);
                    row.First().ToleranceRangeMax = Convert.ToDouble(obj.ToleranceRangeMax);
                    row.First().ToleranceRangeMin = Convert.ToDouble(obj.ToleranceRangeMin);

                }
                entities.SaveChanges();
                context.Response.Write("ok");
              
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
    class SizeEditSub
    {
        public string SizeNumber;
        public string SizeName;
        public string StandardValue;
        public string ToleranceRangeMin;
        public string ToleranceRangeMax;
        public string Measurements;
        public string ID;
    }
}