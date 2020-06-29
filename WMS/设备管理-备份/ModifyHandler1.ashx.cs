using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model.设备管理
{
    /// <summary>
    /// ModiHandler1 的摘要说明
    /// </summary>
    public class ModiHandler1 : IHttpHandler
    {
         int n;

        public void ProcessRequest(HttpContext context)
        {
            var brand = context.Request["brand"];
            var type = context.Request["type"];
           
        context.Response.ContentType = "text/plain";
            int id = int.Parse(context.Request["id"]);
            using (JDJS_WMS_DB_USEREntities data = new JDJS_WMS_DB_USEREntities())
            {
                var data1 = from brands in data.JDJS_WMS_Device_Brand_Info
                            where brands.Brand == brand
                            select new
                            {
                                brands.ID
                            };
                
                foreach (var item in data1)
                {
                    n = item.ID;
                }
                var data2 = data.JDJS_WMS_Device_Type_Info.Where(p => p.ID != id);
                //var data3 = data2.where(r => r.Type == type && r.BrandID == n);
                var data3 = from r in data2
                            where r.Type == type && r.BrandID == n
                            select new
                            {
                                r.Type
                            };


                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                var data4 = serializer.Serialize(data3);
                if (data4 == "[]")
                {
                    var data5 = data.JDJS_WMS_Device_Type_Info.Where(r => r.ID == id);
                    foreach(var item in data5)
                    {
                        item.Type = type;item.BrandID = n;
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