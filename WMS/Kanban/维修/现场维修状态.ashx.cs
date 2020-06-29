using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Kanban.维修
{
    /// <summary>
    /// 现场维修状态 的摘要说明
    /// </summary>
    public class 现场维修状态 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            {
                //现场维修看板至维修概况
                int Location =int.Parse(context.Request["workId"]);
                List<RepairStateInfo> repairStateInfos19 = new List<RepairStateInfo>();
                List<RepairStateInfo> repairStateInfos = new List<RepairStateInfo>();
                List<RepairState> repairStates = new List<RepairState>();
                TimeSpan span = DateTime.Now - DateTime.Now.AddSeconds(-1);
                using (JDJS_WMS_DB_USEREntities wms = new JDJS_WMS_DB_USEREntities ())
                {
                    var Status11Code = wms.JDJS_WMS_Device_Maintenance_Status.Where(r => r.DescID == 11).First();
                    var Status14Code = wms.JDJS_WMS_Device_Maintenance_Status.Where(r => r.DescID == 14).First();
                    var Status16Code = wms.JDJS_WMS_Device_Maintenance_Status.Where(r => r.DescID == 16).First();
                    var Status19Code = wms.JDJS_WMS_Device_Maintenance_Status.Where(r => r.DescID == 19).First();
                    var Status1Code = wms.JDJS_WMS_Device_Maintenance_Status.Where(r => r.DescID == 1).First();
                    var Status0Code = wms.JDJS_WMS_Device_Maintenance_Status.Where(r => r.DescID == 0).First();
                    var state19 = wms.JDJS_WMS_Device_Alarm_Repair.Where(r => r.AlarmStateID == Status19Code.ID);
                    foreach (var item in state19)
                    {
                        int cncID = Convert.ToInt32(item.CncID);
                        var cnc = wms.JDJS_WMS_Device_Info.Where(r => r.ID == cncID).FirstOrDefault();
                        if (cnc != null && cnc.Position == Location)
                        {
                            var cncInfo = cnc.MachNum;
                            RepairStateInfo repairStateInfo = new RepairStateInfo();
                            repairStateInfo.cncNum = cncInfo;
                            int descID = Convert.ToInt32(item.AlarmDescID);
                            var desc = wms.JDJS_WMS_Device_Alarm_Description.Where(r => r.ID == descID).FirstOrDefault().Description;
                            repairStateInfo.Desc = desc;
                            var timeSpan = DateTime.Now - Convert.ToDateTime(item.RepairTime);
                            if (timeSpan > span)
                            {
                                span = timeSpan;
                            }
                            repairStateInfo.repairTime = timeSpan;
                            repairStateInfo.startTime = Convert.ToDateTime(item.RepairTime);
                            repairStateInfo.state = "已完成(机床可使用)";
                            repairStateInfos19.Add(repairStateInfo);
                        }

                    }
                    var state11 = wms.JDJS_WMS_Device_Alarm_Repair.Where(r => r.AlarmStateID != Status0Code.ID && r.AlarmStateID != Status19Code.ID);
                    foreach (var item in state11)
                    {
                        int cncID = Convert.ToInt32(item.CncID);
                        var cnc = wms.JDJS_WMS_Device_Info.Where(r => r.ID == cncID).FirstOrDefault();
                        if (cnc != null && cnc.Position == Location)
                        {
                            var cncInfo = cnc.MachNum;
                            RepairStateInfo repairStateInfo = new RepairStateInfo();
                            repairStateInfo.cncNum = cncInfo;
                            int descID = Convert.ToInt32(item.AlarmDescID);
                            var desc = wms.JDJS_WMS_Device_Alarm_Description.Where(r => r.ID == descID).FirstOrDefault().Description;
                            repairStateInfo.Desc = desc;
                            var timeSpan = DateTime.Now - Convert.ToDateTime(item.RepairTime);
                            if (timeSpan > span)
                            {
                                span = timeSpan;
                            }
                            repairStateInfo.repairTime = timeSpan;
                            repairStateInfo.startTime = Convert.ToDateTime(item.RepairTime);
                            repairStateInfo.state = "维修中";
                            repairStateInfos.Add(repairStateInfo);
                        }
                    }
                    for (int i = 0; i < repairStateInfos19.Count(); i++)
                    {
                        repairStateInfos19[i].process = (repairStateInfos19[i].repairTime.TotalMinutes) / span.TotalMinutes;
                    }
                    repairStateInfos19 = repairStateInfos19.OrderByDescending(r => r.process).ToList();
                    for (int i = 0; i < repairStateInfos.Count(); i++)
                    {
                        repairStateInfos[i].process = (repairStateInfos[i].repairTime.TotalMinutes) / span.TotalMinutes;
                    }
                    repairStateInfos = repairStateInfos.OrderByDescending(r => r.process).ToList();

                }
                for (int i = 0; i < repairStateInfos19.Count(); i++)
                {
                    RepairState repairState = new RepairState();
                    repairState.cncNum = repairStateInfos19[i].cncNum;
                    repairState.Desc = repairStateInfos19[i].Desc;
                    repairState.process = repairStateInfos19[i].process.ToString("0.0000");
                    repairState.repairTime = repairStateInfos19[i].repairTime.TotalHours.ToString("0.0000");
                    repairState.startTime = repairStateInfos19[i].startTime.ToString();
                    repairState.state = repairStateInfos19[i].state;
                    repairStates.Add(repairState);
                }
                for (int i = 0; i < repairStateInfos.Count(); i++)
                {
                    RepairState repairState = new RepairState();
                    repairState.cncNum = repairStateInfos[i].cncNum;
                    repairState.Desc = repairStateInfos[i].Desc;
                    repairState.process = repairStateInfos[i].process.ToString("0.0000");
                    repairState.repairTime = repairStateInfos[i].repairTime.TotalHours.ToString("0.0000");
                    repairState.startTime = repairStateInfos[i].startTime.ToString();
                    repairState.state = repairStateInfos[i].state;
                    repairStates.Add(repairState);
                }
                var model = new { code = 0, data = repairStates };
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                var json = serializer.Serialize(model);
                context.Response.Write("data:" + json + "\n\n");
                context.Response.ContentType = "text/event-stream";
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
    public class RepairStateInfo
    {
        public string cncNum;
        public string Desc;
        public TimeSpan repairTime;
        public double process;
        public DateTime startTime;
        public string state;
    }
    public class RepairState
    {
        public string cncNum;
        public string Desc;
        public string repairTime;
        public string process;
        public string startTime;
        public string state;
    }
}