using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.品质
{
    /// <summary>
    /// addWorkpiece 的摘要说明
    /// </summary>
    public class addWorkpiece : IHttpHandler
    {
        System.Web.Script.Serialization.JavaScriptSerializer Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();


        public void ProcessRequest(HttpContext context)
        {
            using (JDJS_WMS_DB_USEREntities entities=new JDJS_WMS_DB_USEREntities())
            {
                using (System.Data.Entity.DbContextTransaction db = entities.Database.BeginTransaction())
                {
                    try
                    {
                        var data = context.Request["form"];
                        var list = Serializer.Deserialize<WorkPiece>(data);
                        var orderNumber = int.Parse(list.orderNumber);
                        var orderId = entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_Number == list.orderNumber).FirstOrDefault().Order_ID;
                        var newRow = new JDJS_WMS_Quality_ManualInput_Measurement_Table
                        {
                            OrderID = orderId,
                           Measurements=Convert.ToDouble(list.Measurements),
                           SizeName=list.SizeName,
                           WorkpieceNumber= Convert.ToInt32(list.workpieceNumber),
                           SizeNumber=Convert.ToInt32(list.SizeNumber),
                           StandardValue =Convert.ToDouble(list.StandardValue),
                           ToleranceRangeMax=Convert.ToDouble(list.ToleranceRangeMax),
                           ToleranceRangeMin=Convert.ToDouble(list.ToleranceRangeMin),
                           
                        };
                        entities.JDJS_WMS_Quality_ManualInput_Measurement_Table.Add(newRow);
                        entities.SaveChanges();
                        db.Commit();
                        context.Response.Write("ok");
                    }
                    catch(Exception ex)
                    {
                        context.Response.Write(ex.Message);
                        return;
                    }
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
    class WorkPiece
    {
        public string orderNumber;
        public string workpieceNumber;
        public string SizeNumber;
        public string SizeName;
        public string StandardValue;
        public string ToleranceRangeMin;
        public string ToleranceRangeMax;
        public string Measurements;
        
    }
}