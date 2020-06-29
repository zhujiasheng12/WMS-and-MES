using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.品质
{
    /// <summary>
    /// sizeEditRead 的摘要说明
    /// </summary>
    public class sizeEditRead : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var id = int.Parse(context.Request["id"]);
            using(JDJS_WMS_DB_USEREntities entities=new JDJS_WMS_DB_USEREntities())
            {
                var row = from size in entities.JDJS_WMS_Quality_ManualInput_Measurement_Table
                          where size.ID==id
                          select new {
                              size.ID,
                              size.SizeName,
                              size.SizeNumber,
                              size.StandardValue,
                              size.ToleranceRangeMin,
                              size.ToleranceRangeMax,
                              size.Measurements
                          };
                if (row.Count() > 0)
                {
                    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var json = serializer.Serialize(row);
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
}