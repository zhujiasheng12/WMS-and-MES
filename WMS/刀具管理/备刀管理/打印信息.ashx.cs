using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.刀具管理.备刀管理
{
    /// <summary>
    /// 打印信息 的摘要说明
    /// </summary>
    public class 打印信息 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            {
                //装刀打印
                List<LoadToolPrint> loadToolPrints = new List<LoadToolPrint>();
                DateTime timeNow = DateTime.Now;
                TimeSpan span = timeNow - timeNow.AddSeconds(-1);
                using (JDJS_WMS_DB_USEREntities  wms = new JDJS_WMS_DB_USEREntities ())
                {
                    var devices = wms.JDJS_WMS_Device_Info;
                   
                    foreach (var item in devices)
                    {
                        List<string> toolsNo = new List<string>();
                        var toolStand = wms.JDJS_WMS_Tool_Standard_Table.Where (r=>r.MachTypeID ==item.MachType).ToList();
                        foreach (var meal in toolStand)
                        {
                            toolsNo.Add(meal.ToolID);
                        }
                        int cncID = Convert.ToInt32(item.ID);
                        string cncNum = item.MachNum;
                        var cncState = wms.JDJS_WMS_Device_RealTime_Data.Where(r => r.CncID == cncID).FirstOrDefault();
                        var toolLife = wms.JDJS_WMS_Tool_LifeTime_Management.Where(r => r.CncID == cncID);
                        foreach (var real in toolLife)
                        {
                            if (real.ToolCurrTime > real.ToolMaxTime)
                            {
                                if (toolsNo.Contains ("T"+real.ToolID.ToString ()))
                                {
                                    string toolStr = "T" + real.ToolID.ToString();
                                    var toolInfo = wms.JDJS_WMS_Tool_Standard_Table.Where(r => r.ToolID == toolStr&&r.MachTypeID ==item.MachType).FirstOrDefault();
                                    if (toolInfo != null)
                                    {
                                        LoadToolPrint loadToolPrint = new LoadToolPrint();
                                        loadToolPrint.cncNum = cncNum;
                                        loadToolPrint.LoadType = "换刀装刀";
                                        loadToolPrint.stack = toolInfo.Shank;
                                        loadToolPrint.ToolLength = toolInfo.ToolLength.ToString();
                                        loadToolPrint.ToolName = toolInfo.Name;
                                        loadToolPrint.ToolNo = toolStr;
                                        loadToolPrint.ToolSpecification = toolInfo.Specification;
                                        loadToolPrints.Add(loadToolPrint);
                                    }
                                }
                                else
                                {
                                    var schedu = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.CncID == cncID && r.isFlag == 1);
                                    var processID = schedu.OrderBy(r => r.StartTime).ToList().FirstOrDefault();
                                    if (processID != null)
                                    {
                                        int processId = Convert.ToInt32(processID.ProcessID);
                                        var tools = wms.JDJS_WMS_Order_Process_Tool_Info_Table.Where(r => r.ProcessID == processId && r.ToolNO == real.ToolID).FirstOrDefault();
                                        if (tools != null)
                                        {
                                            LoadToolPrint loadToolPrint = new LoadToolPrint();
                                            loadToolPrint.cncNum = cncNum;
                                            loadToolPrint.LoadType = "换刀装刀";
                                            loadToolPrint.stack = tools.Shank;
                                            loadToolPrint.ToolLength = tools.ToolLength.ToString();
                                            loadToolPrint.ToolName = tools.ToolName;
                                            loadToolPrint.ToolNo = "T" + real.ToolID.ToString();
                                            loadToolPrint.ToolSpecification = tools.ToolName;
                                            loadToolPrints.Add(loadToolPrint);
                                        }
                                    }

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
                                    List<int> toolNos = new List<int>();
                                    var toolInfo = wms.JDJS_WMS_Order_Process_Tool_Info_Table.Where(r => r.ProcessID == NextProcessID);
                                    foreach (var real in toolInfo)
                                    {
                                        int toolNo = Convert.ToInt32(real.ToolNO);
                                        if (!toolsNo.Contains("T"+toolNo.ToString ())   )
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
                                            if (!toolsNo.Contains("T" + tool.ToString()))
                                            {
                                                var tools = wms.JDJS_WMS_Order_Process_Tool_Info_Table.Where(r => r.ProcessID == NextProcessID && r.ToolNO == tool).FirstOrDefault();
                                                LoadToolPrint loadToolPrint = new LoadToolPrint();
                                                loadToolPrint.cncNum = cncNum;
                                                loadToolPrint.LoadType = "备刀装刀";
                                                loadToolPrint.stack = tools.Shank;
                                                loadToolPrint.ToolLength = tools.ToolLength.ToString();
                                                loadToolPrint.ToolName = tools.ToolName;
                                                loadToolPrint.ToolNo = "T" + tool.ToString();
                                                loadToolPrint.ToolSpecification = tools.ToolName;
                                                loadToolPrints.Add(loadToolPrint);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                var model = new { code = 0, data = loadToolPrints };
                var json = serializer.Serialize(model);
                context.Response.Write(json);
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
    class LoadToolPrint
    {
        public string cncNum;
        public string LoadType;
        public string ToolNo;
        public string ToolName;
        public string ToolSpecification;
        public string ToolLength;
        public string stack;
    }
}