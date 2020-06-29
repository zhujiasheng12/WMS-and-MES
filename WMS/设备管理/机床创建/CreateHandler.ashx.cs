using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace WebApplication2.Model
{
    /// <summary>
    /// CreateHandler 的摘要说明
    /// </summary>
    public class CreateHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            context.Response.ContentType = "text/plain";
            string brand = context.Request["brand"], model = context.Request["model"], number = context.Request["number"],
                 ip = context.Request["ip"],id=context.Request["id"];
            int stateId = int.Parse(context.Request["state"]);
            using (JDJS_WMS_DB_USEREntities pm1 = new JDJS_WMS_DB_USEREntities())
            {

               // var state = pm1.JDJS_WMS_Device_Status_Table.Where(r => r.ID == stateId).FirstOrDefault().Status;
                var use2 =

                        from JDJS_WMS_Device_Type_Infos in pm1.JDJS_WMS_Device_Type_Info
                       
                        where JDJS_WMS_Device_Type_Infos.Type == model
                        select new
                        {
                            JDJS_WMS_Device_Type_Infos.ID
                        };
                var InspectIp = from ips in pm1.JDJS_WMS_Device_Info
                                where ips.IP == ip
                                select new
                                {
                                    ips.IP
                                };
                var InspectNumber = from numbers in pm1.JDJS_WMS_Device_Info
                                    where numbers.MachNum == number
                                    select new
                                    {
                                        numbers.MachNum
                                    };
                var InspectIps = serializer.Serialize(InspectIp);
                var InspectNumbers = serializer.Serialize(InspectNumber);
                if (InspectIps == "[]" && InspectNumbers == "[]")
                {
                    foreach (var item in use2)
                    {
                        var use3 = item.ID.ToString();

                        var use4 = int.Parse(use3);
                        var user1 = new JDJS_WMS_Device_Info() { MachNum = number, IP = ip, MachType = use4, MachState = stateId.ToString(), Position=int.Parse(id) };
                        pm1.JDJS_WMS_Device_Info.Add(user1);

                        context.Response.Write("ok");
                    }
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
                //JDJS_WMS_Device_Type_Info user = new JDJS_WMS_Device_Type_Info() { Type = "GR800", BrandID = 2 };
                //pm1.JDJS_WMS_Device_Type_Info.Add(user);

                pm1.SaveChanges();
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