using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Kanban
{
    /// <summary>
    /// 报修处理情况 的摘要说明
    /// </summary>
    public class 报修处理情况 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            List<TreatInfo> treatInfos = new List<TreatInfo>();
            List<TreatInfoRead> treatInfoReads = new List<TreatInfoRead>();
            using (JDJS_WMS_DB_USEREntities wms = new  JDJS_WMS_DB_USEREntities ())
            {
                var repairs = wms.JDJS_WMS_Device_Alarm_Repair.ToList();
                foreach (var item in repairs)
                {
                    if (item.Receptiontime != null)
                    {
                        TreatInfo treat = new TreatInfo();
                        treat.time = Convert.ToDateTime(item.Receptiontime);
                        int alarm = Convert.ToInt32(item.AlarmDescID);
                        var desc = wms.JDJS_WMS_Device_Alarm_Description.Where(r => r.ID == alarm).FirstOrDefault();
                        string alarmdesc = item.MaintenanceStaff + "接受" + item.CncNum + "的报修处理，报修描述：" + desc.Description;
                        treat.desc = alarmdesc;
                        treatInfos.Add(treat);
                    }
                    if (item.StartTime != null)
                    {
                        string str = "开始";
                        TreatInfo treat = new TreatInfo();
                        treat.time = Convert.ToDateTime(item.StartTime);
                        int stateID = Convert.ToInt32(item.AlarmStateID);
                        var state = wms.JDJS_WMS_Device_Maintenance_Status.Where(r => r.ID == stateID).FirstOrDefault();
                        if (state.Description == "挂起")
                        {
                            str = "挂起";
                        }
                        int alarm = Convert.ToInt32(item.AlarmDescID);
                        var desc = wms.JDJS_WMS_Device_Alarm_Description.Where(r => r.ID == alarm).FirstOrDefault();
                        string alarmdesc = item.MaintenanceStaff + str + item.CncNum + "的报修处理，报修描述：" + desc.Description;
                        treat.desc = alarmdesc;
                        treatInfos.Add(treat);
                    }
                    if (item.EndTime != null)
                    {
                        TreatInfo treat = new TreatInfo();
                        treat.time = Convert.ToDateTime(item.EndTime);
                        int alarm = Convert.ToInt32(item.AlarmDescID);
                        var desc = wms.JDJS_WMS_Device_Alarm_Description.Where(r => r.ID == alarm).FirstOrDefault();
                        string alarmdesc = item.MaintenanceStaff + "完成" + item.CncNum + "的报修处理，报修描述：" + desc.Description;
                        treat.desc = alarmdesc;
                        treatInfos.Add(treat);
                    }
                }
                var treatList = treatInfos.OrderByDescending(r => r.time);
                List<string> time = new List<string>();
                
                foreach (var item in treatList)
                {
                    string str = item.time.Year.ToString() + "-" + item.time.Month.ToString() + "-" + item.time.Day.ToString();
                    if (time.Contains(str))
                    {
                        var desc = treatInfoReads.Where(r => r.time == str).FirstOrDefault();
                        desc.desc.Add(item.desc);
                    }
                    else
                    {
                        time.Add(str);
                        TreatInfoRead treatInfoRead = new TreatInfoRead();
                        treatInfoRead.time = str;
                        treatInfoRead.desc = new List<string>();
                        treatInfoRead.desc.Add(item.desc);
                        treatInfoReads.Add(treatInfoRead);
                    }
                }
            }
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var json = serializer.Serialize(treatInfoReads);
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
    public class TreatInfo
    {
        public DateTime time;
        public string desc;
    }
    public class TreatInfoRead
    {
        public string time;
        public List<string> desc;
    }
}