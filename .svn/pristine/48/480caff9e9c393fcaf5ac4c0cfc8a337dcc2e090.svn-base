using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Kanban.维修
{
    /// <summary>
    /// TOP报警时长 的摘要说明
    /// </summary>
    public class TOP报警时长 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            int Location = int.Parse(context.Request["workId"]);
            List<alarmTime> alarmTimes = new List<alarmTime>();
            List<alarmTimeInfo> alarmTimeInfos = new List<alarmTimeInfo>();
            TimeSpan span = DateTime.Now - DateTime.Now.AddSeconds(-1);
            DateTime now = DateTime.Now;
            DateTime nowLast = DateTime.Now.AddDays(-1);
            using (JDJS_WMS_DB_USEREntities  wms = new JDJS_WMS_DB_USEREntities ())
            {
                var alarms = wms.JDJS_WMS_Device_ProgState_Info.Where(r => r.ProgState == 4 && r.StartTime > nowLast);
                foreach (var item in alarms)
                {
                    int cncID = Convert.ToInt32(item.CncID);
                    var cnc = wms.JDJS_WMS_Device_Info.Where(r => r.ID == cncID).FirstOrDefault();
                    if (cnc != null && cnc.Position == Location)
                    {
                        var cncInfo = cnc .MachNum;
                        alarmTimeInfo alarmNumInfo = new alarmTimeInfo();
                        alarmNumInfo.cncNum = cncInfo;
                        if (item.EndTime == null)
                        {
                            alarmNumInfo.Time = now - Convert.ToDateTime(item.StartTime);
                        }
                        else
                        {
                            alarmNumInfo.Time = Convert.ToDateTime(item.EndTime) - Convert.ToDateTime(item.StartTime);
                        }
                        if (alarmNumInfo.Time > span)
                        {
                            span = alarmNumInfo.Time;
                        }
                        alarmTimeInfos.Add(alarmNumInfo);
                    }
                }
            }
            for (int i = 0; i < alarmTimeInfos.Count(); i++)
            {
                alarmTimeInfos[i].process = (alarmTimeInfos[i].Time.TotalMinutes) / span.TotalMinutes;
            }
            alarmTimeInfos = alarmTimeInfos.OrderByDescending(r => r.process).ToList();
            for (int i = 0; i < 3; i++)
            {
                if (alarmTimeInfos.Count() > i)
                {
                    alarmTime alarmTime = new alarmTime();
                    alarmTime.cncNum = alarmTimeInfos[i].cncNum;
                    alarmTime.process = alarmTimeInfos[i].process.ToString("0.0000");
                    alarmTime.Time = alarmTimeInfos[i].Time.TotalMinutes.ToString("0.0000")+"Min";
                    alarmTimes.Add(alarmTime);
                }
                else
                {
                    alarmTime alarmTime = new alarmTime();
                    alarmTime.cncNum = "";
                    alarmTime.process = "";
                    alarmTime.Time = "";
                    alarmTimes.Add(alarmTime);
                }
            }

            var model = new { code = 0, data = alarmTimes };
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var json = serializer.Serialize(model);
            context.Response.Write("data:" + json + "\n\n");
            context.Response.ContentType = "text/event-stream";

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
    public class alarmTime
    {
        public string cncNum;
        public string Time;
        public string process;
    }
    public class alarmTimeInfo
    {
        public string cncNum;
        public TimeSpan Time;
        public double process;
    }
}