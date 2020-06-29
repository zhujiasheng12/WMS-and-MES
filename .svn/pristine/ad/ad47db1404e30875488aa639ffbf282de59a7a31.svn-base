using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Kanban.设备监控
{
    /// <summary>
    /// TOP机台维修情况 的摘要说明
    /// </summary>
    public class TOP机台维修情况 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            if (context.Request["workId"] == "全部")
            {
                {
                    //var workId = int.Parse(context.Request["workId"]);
                    {
                        //报修Top5
                        List<RepairInfo> repairs = new List<RepairInfo>();
                        List<Repair> repairUse = new List<Repair>();
                        TimeSpan span = DateTime.Now - DateTime.Now.AddSeconds(-1);
                        using (JDJS_WMS_DB_USEREntities wms = new JDJS_WMS_DB_USEREntities())
                        {
                            var Status11Code = wms.JDJS_WMS_Device_Maintenance_Status.Where(r => r.DescID == 11).First();
                            var Status14Code = wms.JDJS_WMS_Device_Maintenance_Status.Where(r => r.DescID == 14).First();
                            var Status16Code = wms.JDJS_WMS_Device_Maintenance_Status.Where(r => r.DescID == 16).First();
                            var Status19Code = wms.JDJS_WMS_Device_Maintenance_Status.Where(r => r.DescID == 19).First();
                            var Status1Code = wms.JDJS_WMS_Device_Maintenance_Status.Where(r => r.DescID == 1).First();
                            var Status0Code = wms.JDJS_WMS_Device_Maintenance_Status.Where(r => r.DescID == 0).First();
                            var repairInfos = wms.JDJS_WMS_Device_Alarm_Repair.Where(r => r.AlarmStateID != Status0Code.ID);

                            foreach (var item in repairInfos)
                            {
                                int cncID = Convert.ToInt32(item.CncID);
                                var cncInfo = wms.JDJS_WMS_Device_Info.Where(r => r.ID == cncID).FirstOrDefault();
                                if (cncInfo != null)
                                {
                                    //if (cncInfo.Position == workId)

                                    {
                                        if (!cncInfo.MachState.Contains("维修"))
                                        {
                                            RepairInfo repair = new RepairInfo();
                                            repair.cncNum = cncInfo.MachNum;
                                            repair.startTime = item.RepairTime.ToString();
                                            repair.time = (DateTime.Now - Convert.ToDateTime(item.RepairTime));
                                            if ((DateTime.Now - Convert.ToDateTime(item.RepairTime)) > span)
                                            {
                                                span = (DateTime.Now - Convert.ToDateTime(item.RepairTime));
                                            }
                                            repairs.Add(repair);
                                        }
                                    }
                                }
                            }
                        }
                        for (int i = 0; i < repairs.Count(); i++)
                        {
                            repairs[i].progress = (repairs[i].time.TotalMilliseconds / span.TotalMilliseconds);
                        }
                        repairs = repairs.OrderByDescending(r => r.progress).ToList();
                        for (int i = 0; i < 5; i++)
                        {
                            if (repairs.Count() > i)
                            {
                                Repair repair = new Repair();
                                repair.cncNum = repairs[i].cncNum;
                                repair.progress = repairs[i].progress.ToString("0.0000");
                                repair.startTime = repairs[i].startTime;
                                repair.time = repairs[i].time.TotalHours.ToString("0.0000") + "H";
                                repairUse.Add(repair);
                            }
                            else
                            {
                                Repair repair = new Repair();
                                repair.cncNum = "";
                                repair.progress = "";
                                repair.startTime = "";
                                repair.time = "";
                                repairUse.Add(repair);
                            }
                        }
                        System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var model = new { code = 0, data = repairUse };
                        var json = serializer.Serialize(model);
                        context.Response.Write("data:" + json + "\n\n"); context.Response.ContentType = "text/event-stream";
                    }
                }
            }
            else if (context.Request["workId"] == "34台")
            {
                {

                    List<int> cncIDs = new List<int>();
                    for (int i = 95; i < 112; i++)
                    {
                        cncIDs.Add(i);
                    }
                    for (int i = 122; i < 139; i++)
                    {
                        cncIDs.Add(i);
                    }
                    //var workId = int.Parse(context.Request["workId"]);
                    {
                        //报修Top5
                        List<RepairInfo> repairs = new List<RepairInfo>();
                        List<Repair> repairUse = new List<Repair>();
                        TimeSpan span = DateTime.Now - DateTime.Now.AddSeconds(-1);
                        using (JDJS_WMS_DB_USEREntities wms = new JDJS_WMS_DB_USEREntities())
                        {
                            var Status11Code = wms.JDJS_WMS_Device_Maintenance_Status.Where(r => r.DescID == 11).First();
                            var Status14Code = wms.JDJS_WMS_Device_Maintenance_Status.Where(r => r.DescID == 14).First();
                            var Status16Code = wms.JDJS_WMS_Device_Maintenance_Status.Where(r => r.DescID == 16).First();
                            var Status19Code = wms.JDJS_WMS_Device_Maintenance_Status.Where(r => r.DescID == 19).First();
                            var Status1Code = wms.JDJS_WMS_Device_Maintenance_Status.Where(r => r.DescID == 1).First();
                            var Status0Code = wms.JDJS_WMS_Device_Maintenance_Status.Where(r => r.DescID == 0).First();
                            var repairInfos = wms.JDJS_WMS_Device_Alarm_Repair.Where(r => r.AlarmStateID != Status0Code.ID);

                            foreach (var item in repairInfos)
                            {
                                int cncID = Convert.ToInt32(item.CncID);
                                var cncInfo = wms.JDJS_WMS_Device_Info.Where(r => r.ID == cncID).FirstOrDefault();
                                if (cncInfo != null)
                                {
                                    //if (cncInfo.Position == workId)
                                    if(cncIDs .Contains (cncID))
                                    {
                                        if (!cncInfo.MachState.Contains("维修"))
                                        {
                                            RepairInfo repair = new RepairInfo();
                                            repair.cncNum = cncInfo.MachNum;
                                            repair.startTime = item.RepairTime.ToString();
                                            repair.time = (DateTime.Now - Convert.ToDateTime(item.RepairTime));
                                            if ((DateTime.Now - Convert.ToDateTime(item.RepairTime)) > span)
                                            {
                                                span = (DateTime.Now - Convert.ToDateTime(item.RepairTime));
                                            }
                                            repairs.Add(repair);
                                        }
                                    }
                                }
                            }
                        }
                        for (int i = 0; i < repairs.Count(); i++)
                        {
                            repairs[i].progress = (repairs[i].time.TotalMilliseconds / span.TotalMilliseconds);
                        }
                        repairs = repairs.OrderByDescending(r => r.progress).ToList();
                        for (int i = 0; i < 5; i++)
                        {
                            if (repairs.Count() > i)
                            {
                                Repair repair = new Repair();
                                repair.cncNum = repairs[i].cncNum;
                                repair.progress = repairs[i].progress.ToString("0.0000");
                                repair.startTime = repairs[i].startTime;
                                repair.time = repairs[i].time.TotalHours.ToString("0.0000") + "H";
                                repairUse.Add(repair);
                            }
                            else
                            {
                                Repair repair = new Repair();
                                repair.cncNum = "";
                                repair.progress = "";
                                repair.startTime = "";
                                repair.time = "";
                                repairUse.Add(repair);
                            }
                        }
                        System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var model = new { code = 0, data = repairUse };
                        var json = serializer.Serialize(model);
                        context.Response.Write("data:" + json + "\n\n"); context.Response.ContentType = "text/event-stream";
                    }
                }
            }
            else
            {
                var workId = int.Parse(context.Request["workId"]);
                {
                    //报修Top5
                    List<RepairInfo> repairs = new List<RepairInfo>();
                    List<Repair> repairUse = new List<Repair>();
                    TimeSpan span = DateTime.Now - DateTime.Now.AddSeconds(-1);
                    using (JDJS_WMS_DB_USEREntities wms = new JDJS_WMS_DB_USEREntities())
                    {
                        var Status11Code = wms.JDJS_WMS_Device_Maintenance_Status.Where(r => r.DescID == 11).First();
                        var Status14Code = wms.JDJS_WMS_Device_Maintenance_Status.Where(r => r.DescID == 14).First();
                        var Status16Code = wms.JDJS_WMS_Device_Maintenance_Status.Where(r => r.DescID == 16).First();
                        var Status19Code = wms.JDJS_WMS_Device_Maintenance_Status.Where(r => r.DescID == 19).First();
                        var Status1Code = wms.JDJS_WMS_Device_Maintenance_Status.Where(r => r.DescID == 1).First();
                        var Status0Code = wms.JDJS_WMS_Device_Maintenance_Status.Where(r => r.DescID == 0).First();
                        var repairInfos = wms.JDJS_WMS_Device_Alarm_Repair.Where(r => r.AlarmStateID != Status0Code.ID);

                        foreach (var item in repairInfos)
                        {
                            int cncID = Convert.ToInt32(item.CncID);
                            var cncInfo = wms.JDJS_WMS_Device_Info.Where(r => r.ID == cncID).FirstOrDefault();
                            if (cncInfo != null)
                            {
                                if (cncInfo.Position == workId)

                                {
                                    if (!cncInfo.MachState.Contains("维修"))
                                    {
                                        RepairInfo repair = new RepairInfo();
                                        repair.cncNum = cncInfo.MachNum;
                                        repair.startTime = item.RepairTime.ToString();
                                        repair.time = (DateTime.Now - Convert.ToDateTime(item.RepairTime));
                                        if ((DateTime.Now - Convert.ToDateTime(item.RepairTime)) > span)
                                        {
                                            span = (DateTime.Now - Convert.ToDateTime(item.RepairTime));
                                        }
                                        repairs.Add(repair);
                                    }
                                }
                            }
                        }
                    }
                    for (int i = 0; i < repairs.Count(); i++)
                    {
                        repairs[i].progress = (repairs[i].time.TotalMilliseconds / span.TotalMilliseconds);
                    }
                    repairs = repairs.OrderByDescending(r => r.progress).ToList();
                    for (int i = 0; i < 5; i++)
                    {
                        if (repairs.Count() > i)
                        {
                            Repair repair = new Repair();
                            repair.cncNum = repairs[i].cncNum;
                            repair.progress = repairs[i].progress.ToString("0.0000");
                            repair.startTime = repairs[i].startTime;
                            repair.time = repairs[i].time.TotalHours.ToString("0.0000") + "H";
                            repairUse.Add(repair);
                        }
                        else
                        {
                            Repair repair = new Repair();
                            repair.cncNum = "";
                            repair.progress = "";
                            repair.startTime = "";
                            repair.time = "";
                            repairUse.Add(repair);
                        }
                    }
                    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var model = new { code = 0, data = repairUse };
                    var json = serializer.Serialize(model);
                    context.Response.Write("data:" + json + "\n\n"); context.Response.ContentType = "text/event-stream";
                }
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
    class Repair
    {
        public string cncNum;
        /// <summary>
        /// 已维修时间比例
        /// </summary>
        public string progress;
        /// <summary>
        /// 报修时间
        /// </summary>
        public string startTime;
        /// <summary>
        /// 已维修时间
        /// </summary>
        public string time;
    }
    class RepairInfo
    {
        public string cncNum;
        /// <summary>
        /// 已维修时间比例
        /// </summary>
        public double progress;
        /// <summary>
        /// 报修时间
        /// </summary>
        public string startTime;
        /// <summary>
        /// 已维修时间
        /// </summary>
        public TimeSpan time;
    }
}