using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model.设备管理
{
    /// <summary>
    /// ReadHandler1 的摘要说明
    /// </summary>
    public class ReadHandler1 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            int page = int.Parse(context.Request["page"]);
            int limit = int.Parse(context.Request["limit"]);
            using (JDJS_WMS_DB_USEREntities data = new JDJS_WMS_DB_USEREntities())
            {
                var data1 = from brand in data.JDJS_WMS_Device_Brand_Info
                            from type in data.JDJS_WMS_Device_Type_Info
                           
                            where brand.ID == type.BrandID 
                            select new
                            {
                                type.Type,brand.Brand,type.ID
                            };
                List<CreateList> createLists = new List<CreateList>();
                foreach (var item in data1)
                {
                    createLists.Add(new CreateList() { Type = item.Type, Brand = item.Brand, ID = item.ID });
                };
                foreach (var item in createLists)
                {
                    item.MachState = "正常";
                }


                var data2 = createLists.Skip((page - 1) * limit).Take(limit);
                var data3 = new { code = 0, mag = "", count = data1.Count(), data = data2 };
                var json = serializer.Serialize(data3);
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
        public class CreateList
        {
           
            public string Type
            {
                get;
                set;
            }
          
           
           
            public string  MachState
            {
                get;
                set;
            }
            public int  ID
            {
                get;
                set;
            }
            public string  Brand
            {
                get;
                set;
            }
        }
    }
}