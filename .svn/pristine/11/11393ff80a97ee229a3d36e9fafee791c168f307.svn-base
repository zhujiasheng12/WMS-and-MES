using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Kanban.维修
{
    /// <summary>
    /// TOP维修占用时长 的摘要说明
    /// </summary>
    public class TOP维修占用时长 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //维修占用时长
            int Location = int.Parse(context.Request["workId"]);
            List<FixtureTime> fixtureTimes = new List<FixtureTime>();
            List<FixtureTimeInfo> fixtureTimeInfos = new List<FixtureTimeInfo>();
            DateTime now = DateTime.Now;
            DateTime nowLast = now.AddMonths(-1);
            TimeSpan span = now - now.AddSeconds(-1);
            using (JDJS_WMS_DB_USEREntities  wms = new JDJS_WMS_DB_USEREntities())
            {
                var repairs = wms.JDJS_WMS_Device_Alarm_Repair.Where(r => r.RepairTime > nowLast);
                foreach (var item in repairs)
                {
                    int cncID = Convert.ToInt32(item.CncID);
                    var cnc = wms.JDJS_WMS_Device_Info.Where(r => r.ID == cncID).FirstOrDefault();
                    if (cnc != null && cnc.Position == Location)
                    {
                        DateTime endTime = now;
                        DateTime startTime = Convert.ToDateTime(item.RepairTime);
                        if (item.EndTime != null)
                        {
                            endTime = Convert.ToDateTime(item.EndTime);
                        }

                        FixtureTimeInfo fixtureTimeInfo = new FixtureTimeInfo();
                        fixtureTimeInfo.cncNum = item.CncNum;
                        fixtureTimeInfo.Time = endTime - startTime;
                        fixtureTimeInfos.Add(fixtureTimeInfo);
                        if (fixtureTimeInfo.Time > span)
                        {
                            span = fixtureTimeInfo.Time;
                        }
                    }
                }
            }
            for (int i = 0; i < fixtureTimeInfos.Count(); i++)
            {
                fixtureTimeInfos[i].process = (fixtureTimeInfos[i].Time.TotalMinutes) / span.TotalMinutes;
            }
            fixtureTimeInfos = fixtureTimeInfos.OrderByDescending(r => r.process).ToList();
            for (int i = 0; i < 3; i++)
            {
                if (fixtureTimeInfos.Count() > i)
                {
                    FixtureTime fixtureTime = new FixtureTime();
                    fixtureTime.cncNum = fixtureTimeInfos[i].cncNum;
                    fixtureTime.process = fixtureTimeInfos[i].process.ToString("0.0000");
                    fixtureTime.Time = fixtureTimeInfos[i].Time.TotalHours.ToString() + "H";
                    fixtureTimes.Add(fixtureTime);
                }
                else
                {
                    FixtureTime fixtureTime = new FixtureTime();
                    fixtureTime.cncNum = "";
                    fixtureTime.process = "";
                    fixtureTime.Time = "";
                    fixtureTimes.Add(fixtureTime);

                }
            }
            var model = new { code = 0, data = fixtureTimes };
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var json = serializer.Serialize(model);
            context.Response.Write("data:"+json+"\n\n");
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
    public class FixtureTime
    {
        public string cncNum;
        public string Time;
        public string process;
    }
    public class FixtureTimeInfo
    {
        public string cncNum;
        public TimeSpan Time;
        public double process;
    }
}