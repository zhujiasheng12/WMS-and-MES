using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model.设备管理
{
    /// <summary>
    /// machEditRead 的摘要说明
    /// </summary>
    public class machEditRead : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var id = int.Parse(context.Request["id"]);
            using (JDJS_WMS_DB_USEREntities entities = new JDJS_WMS_DB_USEREntities())
            {
                var row = from cnc in entities.JDJS_WMS_Device_Info
                          from brand in entities.JDJS_WMS_Device_Brand_Info
                          from type in entities.JDJS_WMS_Device_Type_Info
                          from work in entities.JDJS_WMS_Location_Info
                          where cnc.MachType == type.ID & type.BrandID == brand.ID&cnc.ID==id&cnc.Position==work.id
                          select new
                          {
                              cnc.MachNum, brand.Brand, type.Type,cnc.IP,cnc.MachState,work.Name,work.id
                          };
                var MachState = row.FirstOrDefault().MachState;
               // var machStateId = entities.JDJS_WMS_Device_Status_Table.Where(r => r.Status == MachState).FirstOrDefault().ID;
                var model = new
                {
                    MachNum = row.FirstOrDefault().MachNum,
                    Brand = row.FirstOrDefault().Brand,
                    Type = row.FirstOrDefault().Type,
                    IP = row.FirstOrDefault().IP,
                    machStateId = MachState,
                    Name = row.FirstOrDefault().Name,
                    id = row.FirstOrDefault().id

                };
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
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