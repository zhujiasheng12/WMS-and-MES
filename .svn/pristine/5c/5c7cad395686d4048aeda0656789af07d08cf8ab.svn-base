using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace WebApplication2.Model
{
    /// <summary>
    /// modifyHandler1 的摘要说明
    /// </summary>
    public class modifyHandler1 : IHttpHandler
    {
        public static int use3;
        public void ProcessRequest(HttpContext context)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            context.Response.ContentType = "text/plain";
            string brand = context.Request["brand"], model = context.Request["model"], number = context.Request["number"],
                ip = context.Request["ip"]; int id = int.Parse(context.Request["id"]), position = int.Parse(context.Request["position"]);
               var stateId =int.Parse( context.Request["machState"]);

            using (JDJS_WMS_DB_USEREntities pm4 = new JDJS_WMS_DB_USEREntities())
            {
               // var machState = pm4.JDJS_WMS_Device_Status_Table.Where(r => r.ID == stateId).FirstOrDefault().Status;
                var user = 
                           from JDJS_WMS_Device_Brand_Infos in pm4.JDJS_WMS_Device_Brand_Info
                           from JDJS_WMS_Device_Type_Infos in pm4.JDJS_WMS_Device_Type_Info
                           where  JDJS_WMS_Device_Type_Infos.BrandID == JDJS_WMS_Device_Brand_Infos.ID&&JDJS_WMS_Device_Brand_Infos.Brand==brand&& JDJS_WMS_Device_Type_Infos.Type==model
                           select new
                           {
                               JDJS_WMS_Device_Type_Infos.ID
                              
                              
                           };
                var list = from JDJS_WMS_Device_Infos in pm4.JDJS_WMS_Device_Info
                           select new
                           {
                               JDJS_WMS_Device_Infos.ID,
                               JDJS_WMS_Device_Infos.MachNum,
                               JDJS_WMS_Device_Infos.IP
                           };
                var lists = list.Where(r => r.ID != id);
                var InspectIp = lists.Where(r => r.IP == ip);
                var InspectNumber = lists.Where(r => r.MachNum == number);


                //var InspectIp = from ips in pm4.JDJS_WMS_Device_Info
                //                where ips.IP == ip
                //                select new
                //                {
                //                    ips.IP
                //                };
                //var InspectNumber = from numbers in pm4.JDJS_WMS_Device_Info
                //                    where numbers.MachNum == number
                //                    select new
                //                    {
                //                        numbers.MachNum
                //                    };
                var InspectIps = serializer.Serialize(InspectIp);
                var InspectNumbers = serializer.Serialize(InspectNumber);
                if (InspectIps == "[]" && InspectNumbers == "[]")
                {
                  

                    foreach (var item in user)
                    {
                        use3 = item.ID;

                    };
                    var users = pm4.JDJS_WMS_Device_Info.Where(r => r.ID == id);
                    foreach (var item in users)
                    {
                        item.IP = ip; item.MachNum = number; item.MachType = use3;item.Position = position;
                        item.MachState = stateId.ToString();
                    }
                    pm4.SaveChanges();
                    context.Response.Write("ok");

                }
                else if (InspectIps != "[]" && InspectNumbers == "[]")
                {
                    context.Response.Write("IpFalse");
                }
                else if (InspectIps == "[]" && InspectNumbers != "[]")
                {
                    context.Response.Write("NumberFalse");
                }
                else
                {
                    context.Response.Write("AllFalse");
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