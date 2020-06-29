using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Kanban.设备监控
{
    /// <summary>
    /// TOP停机情况 的摘要说明
    /// </summary>
    public class TOP停机情况 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            if (context.Request["workId"] == "全部")
            {
                {
                    //var workId = int.Parse(context.Request["workId"]);
                    {
                        //停机Top5
                        //int LocationID = workId;
                        List<CncStopInfo> cncStopInfos = new List<CncStopInfo>();
                        List<CncStop> cncStops = new List<CncStop>();
                        TimeSpan span = DateTime.Now - DateTime.Now.AddSeconds(-1);
                        DateTime timeNow = DateTime.Now;
                        using (JDJS_WMS_DB_USEREntities wms = new JDJS_WMS_DB_USEREntities())
                        {
                            var devices = wms.JDJS_WMS_Device_Info;
                            foreach (var item in devices)
                            {
                                int cncID = Convert.ToInt32(item.ID);
                                var schedu = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.CncID == cncID && (r.isFlag == 1 || r.isFlag == 2)).ToList();
                                if (schedu.Count() > 0)
                                {
                                    schedu = schedu.OrderByDescending(r => r.EndTime).ToList();
                                    var endTime = Convert.ToDateTime(schedu.FirstOrDefault().EndTime);
                                    var time = endTime - timeNow;
                                    if (time > span)
                                    {
                                        span = time;
                                    }
                                    CncStopInfo cncStopInfo = new CncStopInfo();
                                    cncStopInfo.cncNum = item.MachNum;
                                    cncStopInfo.stopTime = endTime;
                                    cncStopInfo.time = time;
                                    cncStopInfos.Add(cncStopInfo);

                                }
                                else
                                {
                                    CncStopInfo cncStopInfo = new CncStopInfo();
                                    cncStopInfo.cncNum = item.MachNum;
                                    cncStopInfo.progress = 1.1;
                                    cncStopInfo.stopTime = timeNow.AddDays(-100);
                                    var cncstate = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.CncID == cncID && r.isFlag == 3);
                                    if (cncstate.Count() > 0)
                                    {
                                        var times = cncstate.OrderByDescending(r => r.EndTime).ToList().FirstOrDefault().EndTime;
                                        cncStopInfo.time = Convert.ToDateTime(times) - timeNow;
                                    }
                                    else
                                    {
                                        cncStopInfo.time = timeNow - timeNow;
                                    }

                                    cncStopInfos.Add(cncStopInfo);
                                }
                            }
                            for (int i = 0; i < cncStopInfos.Count(); i++)
                            {
                                cncStopInfos[i].progress = cncStopInfos[i].time.TotalMilliseconds / span.TotalMilliseconds;
                            }
                            cncStopInfos = cncStopInfos.OrderBy(r => r.stopTime).ToList();
                            for (int i = 0; i < 5; i++)
                            {
                                if (cncStopInfos.Count() > 5)
                                {
                                    CncStop cncStop = new CncStop();
                                    cncStop.cncNum = cncStopInfos[i].cncNum;
                                    if (cncStopInfos[i].stopTime == timeNow.AddDays(-100))
                                    {
                                        cncStop.progress = "0";
                                        cncStop.stopTime = "已停机";
                                        cncStop.time = cncStopInfos[i].time.TotalHours.ToString() + "H";
                                    }
                                    else
                                    {
                                        cncStop.progress = cncStopInfos[i].progress.ToString("0.000000");
                                        cncStop.stopTime = cncStopInfos[i].stopTime.ToString();
                                        cncStop.time = cncStopInfos[i].time.TotalHours.ToString() + "H";
                                    }
                                    cncStops.Add(cncStop);
                                }
                                else
                                {
                                    CncStop cncStop = new CncStop();
                                    cncStop.cncNum = "";
                                    cncStop.progress = "";
                                    cncStop.stopTime = "";
                                    cncStop.time = "";
                                    cncStops.Add(cncStop);
                                }
                            }
                        }
                        System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var model = new { code = 0, data = cncStops };
                        var json = serializer.Serialize(model);
                        context.Response.Write("data:" + json + "\n\n"); context.Response.ContentType = "text/event-stream";
                    }
                }
            }
            else if (context.Request["workId"] == "34台")
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
                {
                    //var workId = int.Parse(context.Request["workId"]);
                    {
                        //停机Top5
                        //int LocationID = workId;
                        List<CncStopInfo> cncStopInfos = new List<CncStopInfo>();
                        List<CncStop> cncStops = new List<CncStop>();
                        TimeSpan span = DateTime.Now - DateTime.Now.AddSeconds(-1);
                        DateTime timeNow = DateTime.Now;
                        using (JDJS_WMS_DB_USEREntities wms = new JDJS_WMS_DB_USEREntities())
                        {
                            var devices = wms.JDJS_WMS_Device_Info;
                            foreach (var item in devices)
                            {
                                int cncID = Convert.ToInt32(item.ID);
                                if (cncIDs.Contains(cncID))
                                {
                                    var schedu = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.CncID == cncID && (r.isFlag == 1 || r.isFlag == 2)).ToList();
                                    if (schedu.Count() > 0)
                                    {
                                        schedu = schedu.OrderByDescending(r => r.EndTime).ToList();
                                        var endTime = Convert.ToDateTime(schedu.FirstOrDefault().EndTime);
                                        var time = endTime - timeNow;
                                        if (time > span)
                                        {
                                            span = time;
                                        }
                                        CncStopInfo cncStopInfo = new CncStopInfo();
                                        cncStopInfo.cncNum = item.MachNum;
                                        cncStopInfo.stopTime = endTime;
                                        cncStopInfo.time = time;
                                        cncStopInfos.Add(cncStopInfo);

                                    }
                                    else
                                    {
                                        CncStopInfo cncStopInfo = new CncStopInfo();
                                        cncStopInfo.cncNum = item.MachNum;
                                        cncStopInfo.progress = 1.1;
                                        cncStopInfo.stopTime = timeNow.AddDays(-100);
                                        var cncstate = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.CncID == cncID && r.isFlag == 3);
                                        if (cncstate.Count() > 0)
                                        {
                                            var times = cncstate.OrderByDescending(r => r.EndTime).ToList().FirstOrDefault().EndTime;
                                            cncStopInfo.time = Convert.ToDateTime(times) - timeNow;
                                        }
                                        else
                                        {
                                            cncStopInfo.time = timeNow - timeNow;
                                        }

                                        cncStopInfos.Add(cncStopInfo);
                                    }
                                }
                            }
                            for (int i = 0; i < cncStopInfos.Count(); i++)
                            {
                                cncStopInfos[i].progress = cncStopInfos[i].time.TotalMilliseconds / span.TotalMilliseconds;
                            }
                            cncStopInfos = cncStopInfos.OrderBy(r => r.stopTime).ToList();
                            for (int i = 0; i < 5; i++)
                            {
                                if (cncStopInfos.Count() > 5)
                                {
                                    CncStop cncStop = new CncStop();
                                    cncStop.cncNum = cncStopInfos[i].cncNum;
                                    if (cncStopInfos[i].stopTime == timeNow.AddDays(-100))
                                    {
                                        cncStop.progress = "0";
                                        cncStop.stopTime = "已停机";
                                        cncStop.time = cncStopInfos[i].time.TotalHours.ToString() + "H";
                                    }
                                    else
                                    {
                                        cncStop.progress = cncStopInfos[i].progress.ToString("0.000000");
                                        cncStop.stopTime = cncStopInfos[i].stopTime.ToString();
                                        cncStop.time = cncStopInfos[i].time.TotalHours.ToString() + "H";
                                    }
                                    cncStops.Add(cncStop);
                                }
                                else
                                {
                                    CncStop cncStop = new CncStop();
                                    cncStop.cncNum = "";
                                    cncStop.progress = "";
                                    cncStop.stopTime = "";
                                    cncStop.time = "";
                                    cncStops.Add(cncStop);
                                }
                            }
                        }
                        System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var model = new { code = 0, data = cncStops };
                        var json = serializer.Serialize(model);
                        context.Response.Write("data:" + json + "\n\n"); context.Response.ContentType = "text/event-stream";
                    }
                }
            }
            else
            {
                var workId = int.Parse(context.Request["workId"]);
                {
                    //停机Top5
                    int LocationID = workId;
                    List<CncStopInfo> cncStopInfos = new List<CncStopInfo>();
                    List<CncStop> cncStops = new List<CncStop>();
                    TimeSpan span = DateTime.Now - DateTime.Now.AddSeconds(-1);
                    DateTime timeNow = DateTime.Now;
                    using (JDJS_WMS_DB_USEREntities wms = new JDJS_WMS_DB_USEREntities())
                    {
                        var works = wms.JDJS_WMS_Location_Info.ToList();
                        var device = wms.JDJS_WMS_Device_Info.ToList();
                        List<WebApplication2.Kanban.现场.CncRead> objs = new List<WebApplication2.Kanban.现场.CncRead>();
                        List<int> workIds = new List<int>();
                        WebApplication2.Kanban.现场.机台状态 funs = new WebApplication2.Kanban.现场.机台状态();
                        var devices = funs.fun(workId, works, device, objs, workIds);
                       // var devices = wms.JDJS_WMS_Device_Info.Where(r => r.Position == LocationID);
                        foreach (var item in devices)
                        {
                            int cncID = Convert.ToInt32(item.ID);
                            var schedu = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.CncID == cncID && (r.isFlag == 1 || r.isFlag == 2)).ToList();
                            if (schedu.Count() > 0)
                            {
                                schedu = schedu.OrderByDescending(r => r.EndTime).ToList();
                                var endTime = Convert.ToDateTime(schedu.FirstOrDefault().EndTime);
                                var time = endTime - timeNow;
                                if (time > span)
                                {
                                    span = time;
                                }
                                CncStopInfo cncStopInfo = new CncStopInfo();
                                cncStopInfo.cncNum = item.MachNum;
                                cncStopInfo.stopTime = endTime;
                                cncStopInfo.time = time;
                                cncStopInfos.Add(cncStopInfo);

                            }
                            else
                            {
                                CncStopInfo cncStopInfo = new CncStopInfo();
                                cncStopInfo.cncNum = item.MachNum;
                                cncStopInfo.progress = 1.1;
                                cncStopInfo.stopTime = timeNow.AddDays(-100);
                                var cncstate = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.CncID == cncID && r.isFlag == 3);
                                if (cncstate.Count() > 0)
                                {
                                    var times = cncstate.OrderByDescending(r => r.EndTime).ToList().FirstOrDefault().EndTime;
                                    cncStopInfo.time = Convert.ToDateTime(times) - timeNow;
                                }
                                else
                                {
                                    cncStopInfo.time = timeNow - timeNow;
                                }

                                cncStopInfos.Add(cncStopInfo);
                            }
                        }
                        for (int i = 0; i < cncStopInfos.Count(); i++)
                        {
                            cncStopInfos[i].progress = cncStopInfos[i].time.TotalMilliseconds / span.TotalMilliseconds;
                        }
                        cncStopInfos = cncStopInfos.OrderBy(r => r.stopTime).ToList();
                        for (int i = 0; i < 5; i++)
                        {
                            if (cncStopInfos.Count() > 5)
                            {
                                CncStop cncStop = new CncStop();
                                cncStop.cncNum = cncStopInfos[i].cncNum;
                                if (cncStopInfos[i].stopTime == timeNow.AddDays(-100))
                                {
                                    cncStop.progress = "0";
                                    cncStop.stopTime = "已停机";
                                    cncStop.time = cncStopInfos[i].time.TotalHours.ToString() + "H";
                                }
                                else
                                {
                                    cncStop.progress = cncStopInfos[i].progress.ToString("0.000000");
                                    cncStop.stopTime = cncStopInfos[i].stopTime.ToString();
                                    cncStop.time = cncStopInfos[i].time.TotalHours.ToString() + "H";
                                }
                                cncStops.Add(cncStop);
                            }
                            else
                            {
                                CncStop cncStop = new CncStop();
                                cncStop.cncNum = "";
                                cncStop.progress = "";
                                cncStop.stopTime = "";
                                cncStop.time = "";
                                cncStops.Add(cncStop);
                            }
                        }
                    }
                    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var model = new { code = 0, data = cncStops };
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
    class CncStop
    {
        public string cncNum;
        /// <summary>
        /// 0-1小数
        /// 差值比
        /// 
        /// </summary>
        public string  progress;
        /// <summary>
        /// 停机时间
        /// 从早到晚显示
        /// </summary>
        public string stopTime;
        /// <summary>
        /// 差值（停机-当前）
        /// </summary>
        public string time;
    }
    class CncStopInfo
    {
        public string cncNum;
        /// <summary>
        /// 0-1小数
        /// 差值比
        /// 
        /// </summary>
        public double progress;
        /// <summary>
        /// 停机时间
        /// 从早到晚显示
        /// </summary>
        public DateTime stopTime;
        /// <summary>
        /// 差值（停机-当前）
        /// </summary>
        public TimeSpan time;
    }
}