using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model.设备管理
{
    /// <summary>
    /// BrandCreateHandler1 的摘要说明
    /// </summary>
    public class BrandCreateHandler1 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            var brand = context.Request["brand"];
            
            using (JDJS_WMS_DB_USEREntities data = new JDJS_WMS_DB_USEREntities())
            {
             
                var data2 = 
                            from brands in data.JDJS_WMS_Device_Brand_Info
                           where brands.Brand==brand
                            select new
                            {
                                brands.Brand
                            };
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                var data3 = serializer.Serialize(data2);
                if (data3 == "[]")
                {
                    var data4 = new JDJS_WMS_Device_Brand_Info() { Brand=brand};
                    data.JDJS_WMS_Device_Brand_Info.Add(data4);
                    data.SaveChanges();

                    context.Response.Write("ok");
                }
                else
                {
                    context.Response.Write("false");
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