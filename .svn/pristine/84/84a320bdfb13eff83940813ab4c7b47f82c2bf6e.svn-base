using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Kanban.设备监控
{
    /// <summary>
    /// TOP待料状态 的摘要说明
    /// </summary>
    public class TOP待料状态 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            if (context.Request["workId"] == "全部")
            {
               
                {
                    //Top5待料
                    List<Pending> pendings = new List<Pending>();
                    List<PendingInfo> pendingInfos = new List<PendingInfo>();
                    int LocationID =0;
                    DateTime timeNow = DateTime.Now;
                    TimeSpan span = timeNow - timeNow.AddSeconds(-1);
                    using (JDJS_WMS_DB_USEREntities wms = new JDJS_WMS_DB_USEREntities())
                    {
                        
                        var devices = wms.JDJS_WMS_Device_Info;
                        foreach (var item in devices)
                        {
                            int cncID = Convert.ToInt32(item.ID);
                            string cncNum = item.MachNum;
                            var cncBlank = wms.JDJS_WMS_Quickchangbaseplate_Table.Where(r => r.CncID == cncID);
                            var cncState = wms.JDJS_WMS_Device_RealTime_Data.Where(r => r.CncID == cncID).FirstOrDefault();
                            int FormateProcessID = -1;
                            int NextProcessID = -1;
                            //if (cncState != null)
                            {
                                if (cncBlank.Count() == 0)
                                {
                                    var schedus3 = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.CncID == cncID && r.isFlag == 3);
                                    if (schedus3.Count() > 0)
                                    {
                                        FormateProcessID = Convert.ToInt32(schedus3.OrderByDescending(r => r.EndTime).ToList().FirstOrDefault().ProcessID);
                                    }
                                    var schedu12 = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.CncID == cncID && (r.isFlag == 2 || r.isFlag == 1));
                                    if (schedu12.Count() > 0)
                                    {
                                        NextProcessID = Convert.ToInt32(schedu12.OrderBy(r => r.StartTime).ToList().FirstOrDefault().ProcessID);
                                    }
                                    if (FormateProcessID == NextProcessID && NextProcessID != -1)
                                    {
                                        var nextInfo = wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == NextProcessID);
                                        var orderid = wms.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == nextInfo.FirstOrDefault().OrderID);
                                        PendingInfo pendingInfo = new PendingInfo();
                                        pendingInfo.cncNum = cncNum;
                                        pendingInfo.startTime = Convert.ToDateTime(schedus3.OrderByDescending(r => r.EndTime).ToList().FirstOrDefault().EndTime);
                                        pendingInfo.time = timeNow - Convert.ToDateTime(schedus3.OrderByDescending(r => r.EndTime).ToList().FirstOrDefault().EndTime);
                                        if (timeNow - Convert.ToDateTime(schedus3.OrderByDescending(r => r.EndTime).ToList().FirstOrDefault().EndTime) > span)
                                        {
                                            span = timeNow - Convert.ToDateTime(schedus3.OrderByDescending(r => r.EndTime).ToList().FirstOrDefault().EndTime);
                                        }
                                        pendingInfo.info = orderid.FirstOrDefault().Order_Number + "-P" + nextInfo.FirstOrDefault().ProcessID.ToString();
                                        pendingInfos.Add(pendingInfo);
                                    }
                                    if (FormateProcessID != NextProcessID && NextProcessID != -1)
                                    {
                                        List<string> toolsNo = new List<string>();
                                        var pro = wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == NextProcessID).First();
                                        var toolStand = wms.JDJS_WMS_Tool_Standard_Table.Where (r=>r.MachTypeID ==pro.DeviceType);
                                        foreach (var mo in toolStand)
                                        {
                                            toolsNo.Add(mo.ToolID);
                                        }
                                        List<int> toolNos = new List<int>();
                                        var toolInfo = wms.JDJS_WMS_Order_Process_Tool_Info_Table.Where(r => r.ProcessID == NextProcessID);
                                        foreach (var real in toolInfo)
                                        {
                                            int toolNo = Convert.ToInt32(real.ToolNO);
                                            if (!toolsNo.Contains("T" + toolNo.ToString()))
                                            {
                                                if (!toolNos.Contains(toolNo))
                                                {
                                                    toolNos.Add(toolNo);
                                                }
                                            }
                                        }
                                        DateTime startTime = new DateTime();
                                        if (schedus3.Count() > 0)
                                        {
                                            startTime = Convert.ToDateTime(schedus3.OrderByDescending(r => r.EndTime).ToList().FirstOrDefault().EndTime);
                                        }
                                        else
                                        {
                                            var toolPrepareInfo = wms.JDJS_WMS_Order_Tool_Prepare_History_table.Where(r => r.ProcessID == NextProcessID);
                                            if (toolPrepareInfo.Count() > 0)
                                            {
                                                startTime = Convert.ToDateTime(toolPrepareInfo.FirstOrDefault().PrepareTime);
                                            }
                                            else
                                            {
                                                if (schedu12.Count() > 0)
                                                {
                                                    startTime = Convert.ToDateTime(schedu12.OrderBy(r => r.StartTime).FirstOrDefault().StartTime);
                                                }
                                                else
                                                {
                                                    startTime = timeNow.AddYears(-2000);
                                                }
                                            }
                                        }
                                        var Tools = wms.JDJS_WMS_Device_Tool_History_Table.Where(r => r.CncID == cncID && r.Time >= startTime);
                                        foreach (var real in Tools)
                                        {
                                            int toolno = Convert.ToInt32(real.ToolNum);
                                            if (toolNos.Contains(toolno))
                                            {
                                                toolNos.Remove(toolno);
                                            }
                                        }
                                        if (toolNos.Count() == 0 && startTime != timeNow.AddYears(-2000))
                                        {
                                            var nextInfo = wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == NextProcessID);
                                            var orderid = wms.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == nextInfo.FirstOrDefault().OrderID);
                                            PendingInfo pendingInfo = new PendingInfo();
                                            pendingInfo.cncNum = cncNum;
                                            pendingInfo.startTime = startTime;
                                            pendingInfo.time = timeNow - startTime;
                                            if ((timeNow - startTime) > span)
                                            {
                                                if (schedus3.OrderByDescending(r => r.EndTime).ToList().FirstOrDefault() != null)
                                                {
                                                    span = timeNow - Convert.ToDateTime(schedus3.OrderByDescending(r => r.EndTime).ToList().FirstOrDefault().EndTime);
                                                }
                                                else
                                                {
                                                    span = timeNow - startTime;
                                                }
                                            }
                                            pendingInfo.info = orderid.FirstOrDefault().Order_Number + "-P" + nextInfo.FirstOrDefault().ProcessID.ToString();
                                            pendingInfos.Add(pendingInfo);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    for (int i = 0; i < pendingInfos.Count(); i++)
                    {
                        pendingInfos[i].progress = pendingInfos[i].time.TotalMilliseconds / span.TotalMilliseconds;
                    }
                    pendingInfos = pendingInfos.OrderByDescending(r => r.progress).ToList();
                    for (int i = 0; i < 5; i++)
                    {
                        if (pendingInfos.Count() > i)
                        {
                            Pending pending = new Pending();
                            pending.cncNum = pendingInfos[i].cncNum;
                            pending.progress = pendingInfos[i].progress.ToString();
                            pending.startTime = pendingInfos[i].info.ToString();
                            pending.time = pendingInfos[i].time.TotalHours.ToString() + "H";
                            pendings.Add(pending);
                        }
                        else
                        {
                            Pending pending = new Pending();
                            pending.cncNum = "";
                            pending.progress = "";
                            pending.startTime = "";
                            pending.time = "";
                            pendings.Add(pending);
                        }
                    }
                    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var model = new { code = 0, data = pendings };
                    var json = serializer.Serialize(model);
                    context.Response.Write("data:" + json + "\n\n"); context.Response.ContentType = "text/event-stream";
                }
            }
            else if (context.Request["workId"] == "34台")
            {
                
                {
                    //Top5待料
                    List<int> cncIDs = new List<int>() ;
                    for (int i = 95; i < 112; i++)
                    {
                        cncIDs.Add(i);
                    }
                    for (int i = 122; i < 139; i++)
                    {
                        cncIDs.Add(i);
                    }
                    List<Pending> pendings = new List<Pending>();
                    List<PendingInfo> pendingInfos = new List<PendingInfo>();
                    int LocationID = 1;
                    DateTime timeNow = DateTime.Now;
                    TimeSpan span = timeNow - timeNow.AddSeconds(-1);
                    using (JDJS_WMS_DB_USEREntities wms = new JDJS_WMS_DB_USEREntities())
                    {
                       
                        var devices = wms.JDJS_WMS_Device_Info;
                        foreach (var item in devices)
                        {
                            int cncID = Convert.ToInt32(item.ID);
                            if (cncIDs.Contains(cncID))
                            {
                                string cncNum = item.MachNum;
                                var cncBlank = wms.JDJS_WMS_Quickchangbaseplate_Table.Where(r => r.CncID == cncID);
                                var cncState = wms.JDJS_WMS_Device_RealTime_Data.Where(r => r.CncID == cncID).FirstOrDefault();
                                int FormateProcessID = -1;
                                int NextProcessID = -1;
                                //if (cncState != null)
                                {
                                    if (cncBlank.Count() == 0)
                                    {
                                        var schedus3 = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.CncID == cncID && r.isFlag == 3);
                                        if (schedus3.Count() > 0)
                                        {
                                            FormateProcessID = Convert.ToInt32(schedus3.OrderByDescending(r => r.EndTime).ToList().FirstOrDefault().ProcessID);
                                        }
                                        var schedu12 = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.CncID == cncID && (r.isFlag == 2 || r.isFlag == 1));
                                        if (schedu12.Count() > 0)
                                        {
                                            NextProcessID = Convert.ToInt32(schedu12.OrderBy(r => r.StartTime).ToList().FirstOrDefault().ProcessID);
                                        }
                                        if (FormateProcessID == NextProcessID && NextProcessID != -1)
                                        {
                                            var nextInfo = wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == NextProcessID);
                                            var orderid = wms.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == nextInfo.FirstOrDefault().OrderID);
                                            PendingInfo pendingInfo = new PendingInfo();
                                            pendingInfo.cncNum = cncNum;
                                            pendingInfo.startTime = Convert.ToDateTime(schedus3.OrderByDescending(r => r.EndTime).ToList().FirstOrDefault().EndTime);
                                            pendingInfo.time = timeNow - Convert.ToDateTime(schedus3.OrderByDescending(r => r.EndTime).ToList().FirstOrDefault().EndTime);
                                            if (timeNow - Convert.ToDateTime(schedus3.OrderByDescending(r => r.EndTime).ToList().FirstOrDefault().EndTime) > span)
                                            {
                                                span = timeNow - Convert.ToDateTime(schedus3.OrderByDescending(r => r.EndTime).ToList().FirstOrDefault().EndTime);
                                            }
                                            pendingInfo.info = orderid.FirstOrDefault().Order_Number + "-P" + nextInfo.FirstOrDefault().ProcessID.ToString();
                                            pendingInfos.Add(pendingInfo);
                                        }
                                        if (FormateProcessID != NextProcessID && NextProcessID != -1)
                                        {
                                            List<int> toolNos = new List<int>();
                                            var toolInfo = wms.JDJS_WMS_Order_Process_Tool_Info_Table.Where(r => r.ProcessID == NextProcessID);
                                            var pro = wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == NextProcessID).First();

                                            List<string> toolsNo = new List<string>();
                                            var toolStand = wms.JDJS_WMS_Tool_Standard_Table.Where (r=>r.MachTypeID ==pro.DeviceType);
                                            foreach (var mo in toolStand)
                                            {
                                                toolsNo.Add(mo.ToolID);
                                            }
                                            foreach (var real in toolInfo)
                                            {
                                                int toolNo = Convert.ToInt32(real.ToolNO);
                                                if (!toolsNo.Contains("T" + toolNo.ToString()))
                                                {
                                                    if (!toolNos.Contains(toolNo))
                                                    {
                                                        toolNos.Add(toolNo);
                                                    }
                                                }
                                            }
                                            DateTime startTime = new DateTime();
                                            if (schedus3.Count() > 0)
                                            {
                                                startTime = Convert.ToDateTime(schedus3.OrderByDescending(r => r.EndTime).ToList().FirstOrDefault().EndTime);
                                            }
                                            else
                                            {
                                                var toolPrepareInfo = wms.JDJS_WMS_Order_Tool_Prepare_History_table.Where(r => r.ProcessID == NextProcessID);
                                                if (toolPrepareInfo.Count() > 0)
                                                {
                                                    startTime = Convert.ToDateTime(toolPrepareInfo.FirstOrDefault().PrepareTime);
                                                }
                                                else
                                                {
                                                    if (schedu12.Count() > 0)
                                                    {
                                                        startTime = Convert.ToDateTime(schedu12.OrderBy(r => r.StartTime).FirstOrDefault().StartTime);
                                                    }
                                                    else
                                                    {
                                                        startTime = timeNow.AddYears(-2000);
                                                    }
                                                }
                                            }
                                            var Tools = wms.JDJS_WMS_Device_Tool_History_Table.Where(r => r.CncID == cncID && r.Time >= startTime);
                                            foreach (var real in Tools)
                                            {
                                                int toolno = Convert.ToInt32(real.ToolNum);
                                                if (toolNos.Contains(toolno))
                                                {
                                                    toolNos.Remove(toolno);
                                                }
                                            }
                                            if (toolNos.Count() == 0 && startTime != timeNow.AddYears(-2000))
                                            {
                                                var nextInfo = wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == NextProcessID);
                                                var orderid = wms.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == nextInfo.FirstOrDefault().OrderID);
                                                PendingInfo pendingInfo = new PendingInfo();
                                                pendingInfo.cncNum = cncNum;
                                                pendingInfo.startTime = startTime;
                                                pendingInfo.time = timeNow - startTime;
                                                if ((timeNow - startTime) > span)
                                                {
                                                    if (schedus3.OrderByDescending(r => r.EndTime).ToList().FirstOrDefault() != null)
                                                    {
                                                        span = timeNow - Convert.ToDateTime(schedus3.OrderByDescending(r => r.EndTime).ToList().FirstOrDefault().EndTime);
                                                    }
                                                    else
                                                    {
                                                        span = timeNow - startTime;
                                                    }
                                                }
                                                pendingInfo.info = orderid.FirstOrDefault().Order_Number + "-P" + nextInfo.FirstOrDefault().ProcessID.ToString();
                                                pendingInfos.Add(pendingInfo);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    for (int i = 0; i < pendingInfos.Count(); i++)
                    {
                        pendingInfos[i].progress = pendingInfos[i].time.TotalMilliseconds / span.TotalMilliseconds;
                    }
                    pendingInfos = pendingInfos.OrderByDescending(r => r.progress).ToList();
                    for (int i = 0; i < 5; i++)
                    {
                        if (pendingInfos.Count() > i)
                        {
                            Pending pending = new Pending();
                            pending.cncNum = pendingInfos[i].cncNum;
                            pending.progress = pendingInfos[i].progress.ToString();
                            pending.startTime = pendingInfos[i].info.ToString();
                            pending.time = pendingInfos[i].time.TotalHours.ToString() + "H";
                            pendings.Add(pending);
                        }
                        else
                        {
                            Pending pending = new Pending();
                            pending.cncNum = "";
                            pending.progress = "";
                            pending.startTime = "";
                            pending.time = "";
                            pendings.Add(pending);
                        }
                    }
                    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var model = new { code = 0, data = pendings };
                    var json = serializer.Serialize(model);
                    context.Response.Write("data:" + json + "\n\n"); context.Response.ContentType = "text/event-stream";
                }
            }
            else
            {
                var workId = int.Parse(context.Request["workId"]);
                {
                    //Top5待料
                    List<Pending> pendings = new List<Pending>();
                    List<PendingInfo> pendingInfos = new List<PendingInfo>();
                    int LocationID = workId;
                    DateTime timeNow = DateTime.Now;
                    TimeSpan span = timeNow - timeNow.AddSeconds(-1);
                    using (JDJS_WMS_DB_USEREntities wms = new JDJS_WMS_DB_USEREntities())
                    {
                        var works = wms.JDJS_WMS_Location_Info.ToList();
                        var device = wms.JDJS_WMS_Device_Info.ToList();
                        List<WebApplication2.Kanban.现场.CncRead> objs = new List<WebApplication2.Kanban.现场.CncRead>();
                        List<int> workIds = new List<int>();
                        WebApplication2.Kanban.现场.机台状态 funs = new WebApplication2.Kanban.现场.机台状态();
                        var devices = funs.fun(workId, works, device, objs, workIds);
                        //var devices = wms.JDJS_WMS_Device_Info.Where(r => r.Position == LocationID);
                        foreach (var item in devices)
                        {
                            int cncID = Convert.ToInt32(item.ID);
                            string cncNum = item.MachNum;
                            var cncBlank = wms.JDJS_WMS_Quickchangbaseplate_Table.Where(r => r.CncID == cncID);
                            var cncState = wms.JDJS_WMS_Device_RealTime_Data.Where(r => r.CncID == cncID).FirstOrDefault();
                            int FormateProcessID = -1;
                            int NextProcessID = -1;
                            //if (cncState != null)
                            {
                                if (cncBlank.Count() == 0)
                                {
                                    var schedus3 = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.CncID == cncID && r.isFlag == 3);
                                    if (schedus3.Count() > 0)
                                    {
                                        FormateProcessID = Convert.ToInt32(schedus3.OrderByDescending(r => r.EndTime).ToList().FirstOrDefault().ProcessID);
                                    }
                                    var schedu12 = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.CncID == cncID && (r.isFlag == 2 || r.isFlag == 1));
                                    if (schedu12.Count() > 0)
                                    {
                                        NextProcessID = Convert.ToInt32(schedu12.OrderBy(r => r.StartTime).ToList().FirstOrDefault().ProcessID);
                                    }
                                    if (FormateProcessID == NextProcessID && NextProcessID != -1)
                                    {
                                        var nextInfo = wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == NextProcessID);
                                        var orderid = wms.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == nextInfo.FirstOrDefault().OrderID);
                                        PendingInfo pendingInfo = new PendingInfo();
                                        pendingInfo.cncNum = cncNum;
                                        pendingInfo.startTime = Convert.ToDateTime(schedus3.OrderByDescending(r => r.EndTime).ToList().FirstOrDefault().EndTime);
                                        pendingInfo.time = timeNow - Convert.ToDateTime(schedus3.OrderByDescending(r => r.EndTime).ToList().FirstOrDefault().EndTime);
                                        if (timeNow - Convert.ToDateTime(schedus3.OrderByDescending(r => r.EndTime).ToList().FirstOrDefault().EndTime) > span)
                                        {
                                            span = timeNow - Convert.ToDateTime(schedus3.OrderByDescending(r => r.EndTime).ToList().FirstOrDefault().EndTime);
                                        }
                                        pendingInfo.info = orderid.FirstOrDefault().Order_Number + "-P" + nextInfo.FirstOrDefault().ProcessID.ToString();
                                        pendingInfos.Add(pendingInfo);
                                    }
                                    if (FormateProcessID != NextProcessID && NextProcessID != -1)
                                    {
                                        var pro = wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == NextProcessID).First ();
                                        List<string> toolsNo = new List<string>();
                                        var toolStand = wms.JDJS_WMS_Tool_Standard_Table.Where (r=>r.MachTypeID ==pro.DeviceType);
                                        foreach (var mo in toolStand)
                                        {
                                            toolsNo.Add(mo.ToolID);
                                        }
                                        List<int> toolNos = new List<int>();
                                        var toolInfo = wms.JDJS_WMS_Order_Process_Tool_Info_Table.Where(r => r.ProcessID == NextProcessID);
                                        foreach (var real in toolInfo)
                                        {
                                            int toolNo = Convert.ToInt32(real.ToolNO);
                                            if (!toolsNo.Contains("T" + toolNo.ToString()))
                                            {
                                                if (!toolNos.Contains(toolNo))
                                                {
                                                    toolNos.Add(toolNo);
                                                }
                                            }
                                        }
                                        DateTime startTime = new DateTime();
                                        if (schedus3.Count() > 0)
                                        {
                                            startTime = Convert.ToDateTime(schedus3.OrderByDescending(r => r.EndTime).ToList().FirstOrDefault().EndTime);
                                        }
                                        else
                                        {
                                            var toolPrepareInfo = wms.JDJS_WMS_Order_Tool_Prepare_History_table.Where(r => r.ProcessID == NextProcessID);
                                            if (toolPrepareInfo.Count() > 0)
                                            {
                                                startTime = Convert.ToDateTime(toolPrepareInfo.FirstOrDefault().PrepareTime);
                                            }
                                            else
                                            {
                                                if (schedu12.Count() > 0)
                                                {
                                                    startTime = Convert.ToDateTime(schedu12.OrderBy(r => r.StartTime).FirstOrDefault().StartTime);
                                                }
                                                else
                                                {
                                                    startTime = timeNow.AddYears(-2000);
                                                }
                                            }
                                        }
                                        var Tools = wms.JDJS_WMS_Device_Tool_History_Table.Where(r => r.CncID == cncID && r.Time >= startTime);
                                        foreach (var real in Tools)
                                        {
                                            int toolno = Convert.ToInt32(real.ToolNum);
                                            if (toolNos.Contains(toolno))
                                            {
                                                toolNos.Remove(toolno);
                                            }
                                        }
                                        if (toolNos.Count() == 0 && startTime != timeNow.AddYears(-2000))
                                        {
                                            var nextInfo = wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == NextProcessID);
                                            var orderid = wms.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == nextInfo.FirstOrDefault().OrderID);
                                            PendingInfo pendingInfo = new PendingInfo();
                                            pendingInfo.cncNum = cncNum;
                                            pendingInfo.startTime = startTime;
                                            pendingInfo.time = timeNow - startTime;
                                            if ((timeNow - startTime) > span)
                                            {
                                                if (schedus3.OrderByDescending(r => r.EndTime).ToList().FirstOrDefault() != null)
                                                {
                                                    span = timeNow - Convert.ToDateTime(schedus3.OrderByDescending(r => r.EndTime).ToList().FirstOrDefault().EndTime);
                                                }
                                                else
                                                {
                                                    span = timeNow - startTime;
                                                }
                                            }
                                            pendingInfo.info = orderid.FirstOrDefault().Order_Number + "-P" + nextInfo.FirstOrDefault().ProcessID.ToString();
                                            pendingInfos.Add(pendingInfo);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    for (int i = 0; i < pendingInfos.Count(); i++)
                    {
                        pendingInfos[i].progress = pendingInfos[i].time.TotalMilliseconds / span.TotalMilliseconds;
                    }
                    pendingInfos = pendingInfos.OrderByDescending(r => r.progress).ToList();
                    for (int i = 0; i < 5; i++)
                    {
                        if (pendingInfos.Count() > i)
                        {
                            Pending pending = new Pending();
                            pending.cncNum = pendingInfos[i].cncNum;
                            pending.progress = pendingInfos[i].progress.ToString();
                            pending.startTime = pendingInfos[i].info.ToString();
                            pending.time = pendingInfos[i].time.TotalHours.ToString() + "H";
                            pendings.Add(pending);
                        }
                        else
                        {
                            Pending pending = new Pending();
                            pending.cncNum = "";
                            pending.progress = "";
                            pending.startTime = "";
                            pending.time = "";
                            pendings.Add(pending);
                        }
                    }
                    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var model = new { code = 0, data = pendings };
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
    class Pending
    {
        public string cncNum;
        /// <summary>
        /// 0-1的小数
        /// 分子为当前机床待料时间差值
        /// 分母为最大的待料时间差值
        /// </summary>
        public string progress;
        /// <summary>
        /// 待料开始时间
        /// </summary>
        public string startTime;
        /// <summary>
        /// 待料时间差值（当前时间-待料开始时间）
        /// </summary>
        public string time;
    }
    class PendingInfo
    {
        public string cncNum;
        /// <summary>
        /// 0-1的小数
        /// 分子为当前机床待料时间差值
        /// 分母为最大的待料时间差值
        /// </summary>
        public double progress;
        /// <summary>
        /// 待料开始时间
        /// </summary>
        public DateTime startTime;
        /// <summary>
        /// 待料时间差值（当前时间-待料开始时间）
        /// </summary>
        public TimeSpan time;
        public string info;
    }
}