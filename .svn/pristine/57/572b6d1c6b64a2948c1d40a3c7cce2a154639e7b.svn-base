using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Kanban.现场
{
    /// <summary>
    /// 语音播报 的摘要说明
    /// </summary>
    public class 语音播报 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //y语音播报
            string VoiceStr = "";
            using (JDJS_WMS_DB_USEREntities wms = new  JDJS_WMS_DB_USEREntities ())
            {
                var Status11Code = wms.JDJS_WMS_Device_Maintenance_Status.Where(r => r.DescID == 11).First();
                var Status14Code = wms.JDJS_WMS_Device_Maintenance_Status.Where(r => r.DescID == 14).First();
                var Status16Code = wms.JDJS_WMS_Device_Maintenance_Status.Where(r => r.DescID == 16).First();
                var Status19Code = wms.JDJS_WMS_Device_Maintenance_Status.Where(r => r.DescID == 19).First();
                var Status1Code = wms.JDJS_WMS_Device_Maintenance_Status.Where(r => r.DescID == 1).First();
                var Status0Code = wms.JDJS_WMS_Device_Maintenance_Status.Where(r => r.DescID == 0).First();
                var devices = wms.JDJS_WMS_Device_Info.ToList();
                foreach (var item in devices)
                {
                    int cncID = item.ID;
                    string cncNum = item.MachNum;
                    var alarmInfo = wms.JDJS_WMS_Device_Alarm_Repair.Where(r => r.CncID == cncID && r.AlarmStateID == Status19Code.ID);
                    if (alarmInfo.Count() > 0)
                    {
                        VoiceStr += (cncNum + "机床维修完成，请尽快确认");
                    }
                }
            }
            VoiceStr = VoiceStr.Replace("-", "杠").Replace ("#","号");
            context.Response.Write(VoiceStr);
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