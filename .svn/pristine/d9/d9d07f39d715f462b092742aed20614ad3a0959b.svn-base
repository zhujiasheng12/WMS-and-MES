using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.品质
{
    /// <summary>
    /// addSize 的摘要说明
    /// </summary>
    public class addSize : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            var form = context.Request["form"];
            var orderId = int.Parse(context.Request["orderId"]);
            var workpieceNumber =int.Parse(context.Request["workpieceNumber"]);
           
            var obj = serializer.Deserialize<AddSize>(form);
            using(JDJS_WMS_DB_USEREntities entities=new JDJS_WMS_DB_USEREntities())
            {
                var row = new JDJS_WMS_Quality_ManualInput_Measurement_Table
                {
                    OrderID = orderId,
                    WorkpieceNumber = workpieceNumber,
                    SizeNumber=int.Parse(obj.SizeNumber),
                    SizeName=obj.SizeName,
                    StandardValue=double.Parse(obj.StandardValue),
                    ToleranceRangeMin=double.Parse(obj.ToleranceRangeMin),
                    ToleranceRangeMax=double.Parse(obj.ToleranceRangeMax),
                    Measurements=double.Parse(obj.Measurements),


                };
                entities.JDJS_WMS_Quality_ManualInput_Measurement_Table.Add(row);
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
    class AddSize
    {
      
        public string SizeNumber;
        public string SizeName;
        public string StandardValue;
        public string ToleranceRangeMin;
        public string ToleranceRangeMax;
        public string Measurements;
         
    }
}