using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace WebApplication2.Model
{
    /// <summary>
    /// JcglProductHandler1 的摘要说明
    /// </summary>
    public class JcglProductHandler1 : IHttpHandler
    {
        public static List<JcglProduct> jcglproducts = new List<JcglProduct>();
        public void ProcessRequest(HttpContext context)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string page = context.Request["page"];
            int page1 = int.Parse(page);
            int limit = int.Parse(context.Request["limit"]);
          
            context.Response.ContentType = "text/plain";


            using (JDJS_WMS_DB_USEREntities pm2 = new JDJS_WMS_DB_USEREntities())
            {
                var user = from JDJS_WMS_Device_Infos in pm2.JDJS_WMS_Device_Info
                           from JDJS_WMS_Device_Brand_Infos in pm2.JDJS_WMS_Device_Brand_Info
                           from JDJS_WMS_Device_Type_Infos in pm2.JDJS_WMS_Device_Type_Info
                           from work in pm2.JDJS_WMS_Location_Info
                           where JDJS_WMS_Device_Infos.MachType == JDJS_WMS_Device_Type_Infos.ID && JDJS_WMS_Device_Type_Infos.BrandID == JDJS_WMS_Device_Brand_Infos.ID && work.id == JDJS_WMS_Device_Infos.Position
                           select new
                           {
                               JDJS_WMS_Device_Brand_Infos.Brand,
                               JDJS_WMS_Device_Type_Infos.Type,
                               JDJS_WMS_Device_Infos.MachNum,
                               JDJS_WMS_Device_Infos.IP,
                               JDJS_WMS_Device_Infos.MachState,
                               JDJS_WMS_Device_Infos.ID,
                               work.Name
                           };
                var uses = user.OrderBy(r => r.ID).Skip((page1 - 1) * limit).Take(limit);
                var model = new { code = 0, msg = "", count = user.Count(), data = uses };
                string json = serializer.Serialize(model);

                context.Response.Write(json);
            };
           
         
           
           
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
    public class JcglProduct
    {
        public string id
        {
            get;
            set;
        }
        public string jcpp
        {
            get;
            set;
        }
        public string jcxh
        {
            get;
            set;
        }
        public string jcbh
        {
            get;
            set;
        }
        public string jcip
        {
            get;
            set;
        }
        public string zt
        {
            get;
            set;
        }
        public string cz
        {
            get;
            set;
        }
    }
}