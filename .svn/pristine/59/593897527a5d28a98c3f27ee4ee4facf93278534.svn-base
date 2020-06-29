using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Kanban.刀具
{
    /// <summary>
    /// 刀具现场饼图 的摘要说明
    /// </summary>
    public class 刀具现场饼图 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //维修占用时长
            //装刀需求

            int LocationID = 1;
            int PendingTool = 0;//待装刀具数
            int OverTool = 0;//已装刀具数
            List<ToolInfo> toolInfos = new List<ToolInfo>();
            List<ToolInfo> toolInfos1 = new List<ToolInfo>();
            List<tool> tools = new List<tool>();//装刀需求
            DateTime timeNow = DateTime.Now;
            TimeSpan span = timeNow - timeNow.AddSeconds(-1);
            List<string> ToolNos = new List<string>();
            using (JDJS_WMS_DB_USEREntities  wms = new JDJS_WMS_DB_USEREntities ())
            {
               

                
                var devices = wms.JDJS_WMS_Device_Info.Where(r => r.Position == LocationID);
                foreach (var item in devices)
                {
                    int cncID = Convert.ToInt32(item.ID);
                    string cncNum = item.MachNum;
                    var cncState = wms.JDJS_WMS_Device_RealTime_Data.Where(r => r.CncID == cncID).FirstOrDefault();
                    var toolLife = wms.JDJS_WMS_Tool_LifeTime_Management.Where(r => r.CncID == cncID);
                    foreach (var real in toolLife)
                    {
                        if (real.ToolCurrTime > real.ToolMaxTime)
                        {

                            DateTime startTimeT = timeNow.AddYears(-100);
                            var toolOver = wms.JDJS_WMS_Tool_LifeTimeOver_History_Table.Where(r => r.CncID == cncID && r.ToolID == real.ToolID);
                            if (toolOver.Count() > 0)
                            {
                                startTimeT = Convert.ToDateTime(toolOver.OrderByDescending(R => R.Time).ToList().FirstOrDefault().Time);
                            }
                            if (startTimeT != timeNow.AddYears(-100))
                            {
                                ToolInfo toolInfo1 = new ToolInfo();
                                toolInfo1.cncNum = cncNum;
                                toolInfo1.toolNum = Convert.ToInt32(real.ToolID);

                                toolInfo1.time = timeNow - startTimeT;
                                if ((timeNow - startTimeT) > span)
                                {
                                    span = (timeNow - startTimeT);
                                }
                                toolInfos.Add(toolInfo1);
                            }
                            else
                            {
                                ToolInfo toolInfo1 = new ToolInfo();
                                toolInfo1.cncNum = cncNum;
                                toolInfo1.toolNum = Convert.ToInt32(real.ToolID);

                                toolInfo1.time = timeNow - timeNow.AddMinutes(Convert.ToDouble(real.ToolMaxTime - real.ToolCurrTime));
                                if ((timeNow - startTimeT) > span)
                                {
                                    span = (timeNow - timeNow.AddMinutes(Convert.ToDouble(real.ToolMaxTime - real.ToolCurrTime)));
                                }
                                toolInfos.Add(toolInfo1);
                            }
                        }
                    }
                    int FormateProcessID = -1;
                    int NextProcessID = -1;
                    if (cncState != null)
                    {
                        if (cncState.ProgState != 1)
                        {
                            var schedus3 = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.CncID == cncID && r.isFlag == 3);
                            if (schedus3.Count() > 0)
                            {
                                FormateProcessID = Convert.ToInt32(schedus3.OrderByDescending(r => r.EndTime).FirstOrDefault().ProcessID);
                            }
                            var schedu12 = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.CncID == cncID && (r.isFlag == 2 || r.isFlag == 1));
                            if (schedu12.Count() > 0)
                            {
                                NextProcessID = Convert.ToInt32(schedu12.OrderBy(r => r.StartTime).FirstOrDefault().ProcessID);
                            }
                            if (FormateProcessID != NextProcessID && NextProcessID != -1)
                            {
                                ToolNos.Clear();
                                List<int> toolNos = new List<int>();
                                var proce = wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == NextProcessID).First ();
                                var standTool = wms.JDJS_WMS_Tool_Standard_Table.Where (r=>r.MachTypeID ==proce.DeviceType);
                                ToolNos.Clear();
                                foreach (var mode in standTool)
                                {
                                    ToolNos.Add(mode.ToolID);
                                }
                                var toolInfo = wms.JDJS_WMS_Order_Process_Tool_Info_Table.Where(r => r.ProcessID == NextProcessID);
                                foreach (var real in toolInfo)
                                {
                                    int toolNo = Convert.ToInt32(real.ToolNO);
                                    if (!ToolNos.Contains ("T"+toolNo.ToString ()))
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
                                    startTime = Convert.ToDateTime(schedus3.OrderByDescending(r => r.EndTime).FirstOrDefault().EndTime);
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
                                        startTime = timeNow.AddYears(-2000);
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
                                if (toolNos.Count() > 0 && startTime != timeNow.AddYears(-2000))
                                {
                                    foreach (var tool in toolNos)
                                    {
                                        ToolInfo toolInfo1 = new ToolInfo();
                                        toolInfo1.cncNum = cncNum;
                                        toolInfo1.toolNum = tool;
                                        toolInfo1.time = timeNow - startTime;
                                        if ((timeNow - startTime) > span)
                                        {
                                            span = (timeNow - startTime);
                                        }
                                        toolInfos.Add(toolInfo1);
                                    }
                                }
                            }
                        }
                    }
                }
                var changeTools = wms.JDJS_WMS_Device_Tool_History_Table;
                foreach (var item in changeTools)
                {
                    int cncID = Convert.ToInt32(item.CncID);
                    var cnc = wms.JDJS_WMS_Device_Info.Where(r => r.ID == cncID).FirstOrDefault();
                    if (cnc != null && cnc.Position == LocationID)
                    {
                        int ToolNo = Convert.ToInt32(item.ToolNum);
                        DateTime dateTime = Convert.ToDateTime(item.Time);
                        var progstates = wms.JDJS_WMS_Device_ProgState_Info.Where(r => r.CncID == cncID && r.ProgState == 1 && r.StartTime > dateTime);
                        if (progstates.Count() < 1)
                        {
                            ToolInfo toolInfo = new ToolInfo();
                            toolInfo.cncNum = cnc.MachNum;
                            toolInfo.toolNum = ToolNo;
                            toolInfos1.Add(toolInfo);
                        }

                    }
                }
            }
            for (int i = 0; i < toolInfos.Count(); i++)
            {
                toolInfos[i].progress = toolInfos[i].time.TotalMilliseconds / span.TotalMilliseconds;
            }
            toolInfos.OrderByDescending(r => r.progress);
            foreach (var item in toolInfos1)
            {
                tool tool = new tool();
                tool.cncNum = item.cncNum;
                tool.state = "已完成，机床可使用";
                tool.ToolNo = "T" + item.toolNum.ToString();
                if (item.toolNum > 27)
                {
                    tool.Type = "特殊刀具";
                }
                else
                {
                    tool.Type = "固定刀具";
                }
                tools.Add(tool);
            }

            foreach (var item in toolInfos)
            {
                tool tool = new tool();
                tool.cncNum = item.cncNum;
                tool.state = "待装刀";
                tool.ToolNo = "T" + item.toolNum.ToString();
                if (!ToolNos.Contains ("T"+item.toolNum.ToString ()))
                {
                    tool.Type = "特殊刀具";
                }
                else
                {
                    tool.Type = "固定刀具";
                }
                tools.Add(tool);
            }
            PendingTool = toolInfos.Count();
            OverTool = toolInfos1.Count();




            var model = new { OverTool = OverTool, PendingTool = PendingTool };
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
    class ToolInfo
    {
        public string cncNum;
        public double progress;
        public int toolNum;
        public TimeSpan time;
    }
    class tool
    {
        public string cncNum;
        public string ToolNo;
        public string Type;
        public string state;
    }
}