using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model.生产管理.维保
{
    /// <summary>
    /// 受理 的摘要说明
    /// </summary>
    public class 受理 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var CncId = int.Parse(context.Request["cncId"]);
            var StaffName = context.Request["userName"];
          
              
               
                using (JDJS_WMS_DB_USEREntities JDJS_WMS_Device_Info = new JDJS_WMS_DB_USEREntities())
                {
                    var StateID = JDJS_WMS_Device_Info.JDJS_WMS_Device_Maintenance_Status.Where(r => r.DescID == 11).First();
                    var OverStateID = JDJS_WMS_Device_Info.JDJS_WMS_Device_Maintenance_Status.Where(r => r.DescID == 1).First();
                    var alarminfo = JDJS_WMS_Device_Info.JDJS_WMS_Device_Alarm_Repair.Where(r => r.CncID == CncId && r.AlarmStateID == StateID.ID).First();
                    alarminfo.MaintenanceStaff = StaffName;
                    alarminfo.Receptiontime = DateTime.Now;
                    alarminfo.AlarmStateID = OverStateID.ID;
                    JDJS_WMS_Device_Info.SaveChanges();
                context.Response.Write("ok");
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