using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model.生产管理.维保
{
    /// <summary>
    /// knifeRead 的摘要说明
    /// </summary>
    public class knifeRead : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {

            int CncId = int.Parse(context.Request["id"]);
            List<AlarmInfo> alarmInfos = new List<AlarmInfo>();

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            using (JDJS_WMS_DB_USEREntities JDJS_WMS_Device_Info = new JDJS_WMS_DB_USEREntities())
            {

              
                
              
                

                    var Info = JDJS_WMS_Device_Info.JDJS_WMS_Device_Alarm_Repair.Where(r => r.CncID == CncId);
                    foreach (var item in Info)
                    {
                        AlarmInfo alarmInfo = new AlarmInfo();
                    
                        alarmInfo.RepairTime = Convert.ToString(item.RepairTime);
                        var desc = JDJS_WMS_Device_Info.JDJS_WMS_Device_Alarm_Description.Where(r => r.ID == item.AlarmDescID).First();
                        alarmInfo.RepairDesc = desc.Description;
                        alarmInfo.Receptiontime = Convert.ToString(item.Receptiontime);
                        alarmInfo.StartTime = Convert.ToString(item.StartTime);
                        alarmInfo.EndTime = Convert.ToString(item.EndTime);
                        alarmInfo.Staff = item.MaintenanceStaff;
                        var state = JDJS_WMS_Device_Info.JDJS_WMS_Device_Maintenance_Status.Where(r => r.ID == item.AlarmStateID).First();
                        alarmInfo.State = state.Description;
                        alarmInfo.CncID =Convert.ToInt32 ( item.CncID);
                        alarmInfos.Add(alarmInfo);

                    }
                }
            
            var page = int.Parse(context.Request["page"]);
            var limit = int.Parse(context.Request["limit"]);
            var order = alarmInfos.OrderBy(r => r.RepairTime);
            var date = order.Skip((page - 1) * limit).Take(limit);
            var model = new { code = 0, msg = "", count = alarmInfos.Count, data = date };
            string json = serializer.Serialize(model);
            context.Response.Write(json);

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
    public class AlarmInfo
    {
        public string RepairTime;
        public string RepairDesc;
        public string Receptiontime;
        public string StartTime;
        public string EndTime;
        public string Staff;
        public string State;
        public int CncID;
    }
}