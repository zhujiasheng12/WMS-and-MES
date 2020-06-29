using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace WebApplication2.生产管理.品质
{
    /// <summary>
    /// sizeRead 的摘要说明
    /// </summary>
    public class sizeRead : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {


            var str = context.Request["workpieceNumber"];
            int outNum;
            if (int.TryParse(str, out outNum))
            {

            }
            else
            {
                context.Response.Write("{\"code\":0,\"data\":[]}");
                return;
            }



            var orderId = int.Parse(context.Request["orderId"]);
            var workpieceNumber = int.Parse(context.Request["workpieceNumber"]);
            using (JDJS_WMS_DB_USEREntities entities=new JDJS_WMS_DB_USEREntities())
            {
                var rows = from size in entities.JDJS_WMS_Quality_ManualInput_Measurement_Table
                           where size.OrderID == orderId & size.WorkpieceNumber == workpieceNumber
                           select new
                           {
                               size.SizeName,
                               size.SizeNumber,
                               size.StandardValue,
                               size.ToleranceRangeMax,
                               size.ToleranceRangeMin,
                               size.OutOfTolerance,
                               size.Measurements,
                               size.ID


                           };
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                var model = new { code = 0, msg = "", count = rows.Count(), data = rows };
                var json = serializer.Serialize(model);
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