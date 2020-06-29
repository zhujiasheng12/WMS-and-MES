using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model.设备管理
{
    /// <summary>
    /// CreateHandler1 的摘要说明
    /// </summary>
    public class CreateHandler1 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            var brand = context.Request["brand"];
            var type = context.Request["type"];
            using (JDJS_WMS_DB_USEREntities data = new JDJS_WMS_DB_USEREntities())
            {
                var data1 = from brands in data.JDJS_WMS_Device_Brand_Info
                            where brands.Brand==brand
                            select new
                            {
                                brands.ID
                            };
                var data2 = from type1 in data.JDJS_WMS_Device_Type_Info
                            from brands in data.JDJS_WMS_Device_Brand_Info
                            where type1.Type == type&&type1.BrandID==brands.ID&&brands.Brand==brand
                            select new
                            {
                                type1.Type
                            };
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                var data3 = serializer.Serialize(data2);
                if (data3 == "[]")
                {
                    foreach (var item in data1)
                    {
                        data.JDJS_WMS_Device_Type_Info.Add(new JDJS_WMS_Device_Type_Info() { Type = type, BrandID = item.ID });
                    }
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