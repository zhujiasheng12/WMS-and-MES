using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.维修部.维保计划
{
    /// <summary>
    /// editRead 的摘要说明
    /// </summary>
    public class editRead : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var id = context.Request["id"];
            int PlanID = int.Parse(id);
            using (JDJS_WMS_DB_USEREntities  JDJS_WMS_Device_Info = new JDJS_WMS_DB_USEREntities ())
            {
                var plan = from planm in JDJS_WMS_Device_Info.JDJS_WMS_Maintenance_Plan_Table
                           where planm.ID == PlanID
                           select new
                           {
                               planm.MaintenanceContence,
                               planm.MaintenanceTime,
                               planm.isFlag,
                               planm.CncID,
                               planm.TimeStr,
                               planm.ID

                           };
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                var jsonplan = new { MaintenanceContence = plan.First().MaintenanceContence, MaintenanceTime = plan.First().MaintenanceTime.ToString().Replace ('/','-'), CncID = plan.First().CncID, Cycle = plan.First().TimeStr.Substring(0, plan.First().TimeStr.Length - 1), Format = plan.First().TimeStr.Substring(plan.First().TimeStr.Length - 1),id=plan.FirstOrDefault().ID };
                var json = serializer.Serialize(jsonplan);
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