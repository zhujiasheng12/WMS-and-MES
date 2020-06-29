using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Kanban.刀具
{
    /// <summary>
    /// 装刀请求 的摘要说明
    /// </summary>
    public class 装刀请求 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                //装刀请求
                List<LoadToolRequair> loadToolRequairs = new List<LoadToolRequair>();
                using (JDJS_WMS_DB_USEREntities wms = new JDJS_WMS_DB_USEREntities ())
                {
                   
                    //拿出换刀完成后没有启动机床的
                    var ToolHistory = wms.JDJS_WMS_Device_Tool_History_Table;
                    foreach (var item in ToolHistory)
                    {
                        int cncId = Convert.ToInt32(item.CncID);
                        DateTime time = Convert.ToDateTime(item.Time);
                        int ToolNo = Convert.ToInt32(item.ToolNum);
                        var cnc = wms.JDJS_WMS_Device_Info.Where(r => r.ID == cncId).First ();
                        var progState = wms.JDJS_WMS_Device_ProgState_Info.Where(r => r.CncID == cncId && r.ProgState == 1 && r.StartTime > time);
                        if (progState.Count() < 1)
                        {
                            List<string> ToolNos = new List<string>();
                            var standTool = wms.JDJS_WMS_Tool_Standard_Table.Where (r=>r.MachTypeID ==cnc.MachType);
                            foreach (var mo in standTool)
                            {
                                ToolNos.Add(mo.ToolID);
                            }
                            if (ToolNos .Contains ("T"+ToolNo.ToString ()))
                            {
                                LoadToolRequair loadToolRequair = new LoadToolRequair();
                                int cncid = Convert.ToInt32(item.CncID);
                                loadToolRequair.CncNum = wms.JDJS_WMS_Device_Info.Where(r => r.ID == cncid).FirstOrDefault().MachNum;
                                loadToolRequair.Type = "换刀装刀";
                                loadToolRequair.RequairNum = 1;
                                loadToolRequair.RequairToolNo = ToolNo.ToString();
                                loadToolRequair.ToolState = "已完成";
                                var processes = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.CncID == cncid && r.isFlag == 3).OrderByDescending(r => r.EndTime);
                                if (processes.Count() > 0)
                                {
                                    loadToolRequair.StartTime = processes.FirstOrDefault().EndTime.ToString();
                                }
                                else
                                {
                                    loadToolRequair.StartTime = DateTime.Now.ToString();
                                }

                                loadToolRequair.EndTime = time.ToString();
                                loadToolRequairs.Add(loadToolRequair);
                            }
                            else
                            {
                                LoadToolRequair loadToolRequair = new LoadToolRequair();
                                int cncid = Convert.ToInt32(item.CncID);
                                loadToolRequair.CncNum = wms.JDJS_WMS_Device_Info.Where(r => r.ID == cncid).FirstOrDefault().MachNum;
                                loadToolRequair.Type = "备刀装刀";
                                loadToolRequair.RequairNum = 1;
                                loadToolRequair.RequairToolNo = ToolNo.ToString();
                                loadToolRequair.ToolState = "已完成";
                                var processes = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.CncID == cncid && r.isFlag == 3).OrderByDescending(r => r.EndTime);
                                if (processes.Count() > 0)
                                {
                                    loadToolRequair.StartTime = processes.FirstOrDefault().EndTime.ToString();
                                }
                                else
                                {
                                    loadToolRequair.StartTime = DateTime.Now.ToString();
                                }

                                loadToolRequair.EndTime = time.ToString();
                                loadToolRequairs.Add(loadToolRequair);
                            }
                        }
                    }

                    var toolLifes = wms.JDJS_WMS_Tool_LifeTime_Management;
                    //拿出因为刀具寿命到期而需要换刀的
                    foreach (var item in toolLifes)
                    {
                        double MaxTime = Convert.ToDouble(item.ToolMaxTime);
                        double CurrTime = Convert.ToDouble(item.ToolCurrTime);
                        if (CurrTime >= MaxTime)
                        {
                            LoadToolRequair loadToolRequair = new LoadToolRequair();
                            int cncid = Convert.ToInt32(item.CncID);
                            loadToolRequair.CncNum = wms.JDJS_WMS_Device_Info.Where(r => r.ID == cncid).FirstOrDefault().MachNum;
                            loadToolRequair.Type = "换刀装刀";
                            loadToolRequair.RequairNum = 1;
                            loadToolRequair.RequairToolNo = item.ToolID.ToString();
                            loadToolRequair.ToolState = "待装刀";
                            loadToolRequair.StartTime = DateTime.Now.AddMinutes(CurrTime - MaxTime).ToString();
                            loadToolRequair.EndTime = "-";
                            loadToolRequairs.Add(loadToolRequair);
                        }
                    }

                    //拿出因为换工序而换刀的，备刀换刀
                    var processesTool = wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.toolPreparation == 1);
                    foreach (var item in processesTool)
                    {
                        List<int> toolNo = new List<int>();
                        int processID = item.ID;
                        var toolInfo = wms.JDJS_WMS_Order_Process_Tool_Info_Table.Where(r => r.ProcessID == processID);
                        foreach (var tool in toolInfo)
                        {
                            toolNo.Add(Convert.ToInt32(tool.ToolNO));
                        }
                        var devices = wms.JDJS_WMS_Device_Info;
                        foreach (var device in devices)
                        {
                            int cncID = device.ID;
                            var deviceState3 = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.CncID == cncID && r.isFlag == 3).OrderByDescending(r => r.EndTime).FirstOrDefault();
                            var deviceState1 = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.CncID == cncID && r.isFlag == 1 && r.ProcessID == processID).OrderBy(r => r.StartTime).FirstOrDefault();
                            if (deviceState1 != null)
                            {
                                if (deviceState3 == null || deviceState3.ProcessID != deviceState1.ProcessID)
                                {
                                    List<int> toolNos = new List<int>();
                                    foreach (var tool in toolNo)
                                    {
                                        toolNos.Add(tool);
                                    }
                                    DateTime dateTime = Convert.ToDateTime(deviceState1.StartTime);
                                    var ToolPers = wms.JDJS_WMS_Device_Tool_History_Table.Where(r => r.CncID == cncID && r.Time > dateTime);
                                    foreach (var toolper in ToolPers)
                                    {
                                        if (toolNos.Contains(Convert.ToInt32(toolper.ToolNum)))
                                        {
                                            toolNos.Remove(Convert.ToInt32(toolper.ToolNum));
                                        }
                                    }
                                    foreach (var tool in toolNos)
                                    {
                                        LoadToolRequair loadToolRequair = new LoadToolRequair();
                                        int cncid = Convert.ToInt32(cncID);
                                        loadToolRequair.CncNum = wms.JDJS_WMS_Device_Info.Where(r => r.ID == cncid).FirstOrDefault().MachNum;
                                        loadToolRequair.Type = "备刀装刀";
                                        loadToolRequair.RequairNum = 1;
                                        loadToolRequair.RequairToolNo = tool.ToString();
                                        loadToolRequair.ToolState = "待装刀";
                                        loadToolRequair.StartTime = dateTime.ToString();
                                        loadToolRequair.EndTime = "-";
                                        loadToolRequairs.Add(loadToolRequair);
                                    }
                                }
                            }
                        }

                    }
                }

                var model = new { code = 0, data = loadToolRequairs };
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                var json = serializer.Serialize(model);
                context.Response.Write(json);
            }catch(Exception ex)
            {
                return;
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
    public class LoadToolRequair
    {
        /// <summary>
        /// 机床编号
        /// </summary>
        public string CncNum;
        /// <summary>
        /// 需求装刀类型
        /// </summary>
        public string Type;
        /// <summary>
        /// 需求装刀数量
        /// </summary>
        public int RequairNum;
        /// <summary>
        /// 需求装刀刀号
        /// </summary>
        public string RequairToolNo;
        /// <summary>
        /// 装刀状态
        /// </summary>
        public string ToolState;
        /// <summary>
        /// 需求下发时间
        /// </summary>
        public string StartTime;
        /// <summary>
        /// 需求完成时间
        /// </summary>
        public string EndTime;
    }
}