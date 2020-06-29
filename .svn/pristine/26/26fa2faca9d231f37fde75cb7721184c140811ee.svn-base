using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model.设备管理
{
    /// <summary>
    /// BrandModifyHandler1 的摘要说明
    /// </summary>
    public class BrandModifyHandler1 : IHttpHandler
    {



        public void ProcessRequest(HttpContext context)
        {
            var brand = context.Request["brand"];
          

            context.Response.ContentType = "text/plain";
            int id = int.Parse(context.Request["id"]);
            using (JDJS_WMS_DB_USEREntities data = new JDJS_WMS_DB_USEREntities())
            {
               

              
                var data2 = data.JDJS_WMS_Device_Brand_Info.Where(p => p.ID != id);
              
                var data3 = from r in data2
                            where r.Brand==brand
                            select new
                            {
                                r.Brand
                            };


                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                var data4 = serializer.Serialize(data3);
                if (data4 == "[]")
                {
                    var data5 = data.JDJS_WMS_Device_Brand_Info.Where(r => r.ID == id);
                    foreach (var item in data5)
                    {
                        item.Brand = brand;
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