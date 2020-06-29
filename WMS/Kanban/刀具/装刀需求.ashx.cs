using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Kanban.刀具
{
    /// <summary>
    /// 装刀需求 的摘要说明
    /// </summary>
    public class 装刀需求 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //装刀需求
            int alreadyRegularToolNum = 0;//已装固定刀具
            int alreadySpecialToolNum = 0;//已装特殊刀具
            int pendingRegularToolNum = 0;//待装固定刀具
            int pendingSpecialToolNum = 0;//待装特殊刀具
            using (JDJS_WMS_DB_USEREntities  wms = new  JDJS_WMS_DB_USEREntities ())
            {
                
                var tools = wms.JDJS_WMS_Device_Tool_History_Table;
                foreach (var item in tools)
                {
                    int cncid =Convert.ToInt32 ( item.CncID);
                    var cnc = wms.JDJS_WMS_Device_Info.Where(r => r.ID == cncid).First();
                    int toolNo = Convert.ToInt32(item.ToolNum);
                    List<string> ToolNos = new List<string>();
                    var standTool = wms.JDJS_WMS_Tool_Standard_Table.Where(r => r.MachTypeID == cnc.MachType);
                    foreach (var mo in standTool)
                    {
                        ToolNos.Add(mo.ToolID);
                    }
                    if (!ToolNos .Contains ("T"+item.ToolNum .ToString ()))
                    {
                        alreadySpecialToolNum++;
                    }
                    else
                    {
                        alreadyRegularToolNum++;
                    }
                }
                var toolLifes = wms.JDJS_WMS_Tool_LifeTime_Management;
                foreach (var item in toolLifes)
                {
                    double MaxTime = Convert.ToDouble(item.ToolMaxTime);
                    double CurrTime = Convert.ToDouble(item.ToolCurrTime);
                    if (CurrTime >= MaxTime)
                    {
                        pendingRegularToolNum++;
                    }
                }
                var alreadyTools = wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.toolPreparation == 1);
                foreach (var item in alreadyTools)
                {
                    List<int> cncID = new List<int>();
                    int processId = Convert.ToInt32(item.ID);
                    int toolAllNum = 0;
                    var toos = wms.JDJS_WMS_Order_Process_Tool_Info_Table.Where(r => r.ProcessID == processId);
                    foreach (var tool in toos)
                    {
                        var pro = wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == processId).First();
                        List<string> ToolNos = new List<string>();
                        var standTool = wms.JDJS_WMS_Tool_Standard_Table.Where(r => r.MachTypeID == pro.DeviceType);
                        foreach (var mo in standTool)
                        {
                            ToolNos.Add(mo.ToolID);
                        }
                        int tooNo = Convert.ToInt32(tool.ToolNO);
                        if (!ToolNos.Contains ("T"+tooNo.ToString ()))
                        {
                            toolAllNum++;
                        }
                    }

                    var devices = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.ProcessID == processId && (r.isFlag == 3 || r.isFlag == 2));
                    foreach (var real in devices)
                    {
                        int cnc = Convert.ToInt32(real.CncID);
                        if (!cncID.Contains(cnc))
                        {
                            cncID.Add(cnc);
                        }
                    }
                    foreach (var real in cncID)
                    {


                        var devicees = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.CncID == real && r.isFlag == 1).OrderBy(r => r.StartTime);
                        if (devicees.Count() != 0)
                        {
                            if (devicees.FirstOrDefault().ProcessID == processId)
                            {
                                var pro = wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == processId).First();
                                List<string> ToolNos = new List<string>();
                                var standTool = wms.JDJS_WMS_Tool_Standard_Table.Where(r => r.MachTypeID == pro.DeviceType);
                                foreach (var mo in standTool)
                                {
                                    ToolNos.Add(mo.ToolID);
                                }
                                int toolsCount = 0;
                                DateTime time = Convert.ToDateTime(devicees.FirstOrDefault().StartTime);
                                var ToolS = wms.JDJS_WMS_Device_Tool_History_Table.Where(r => r.CncID == real && r.Time > time );
                                foreach (var mode in ToolS)
                                {
                                    if (!ToolNos.Contains("T" + mode.ToolNum.ToString()))
                                    {
                                        toolsCount ++;
                                    }
                                }
                                
                                int notTool = toolAllNum - toolsCount;
                                pendingSpecialToolNum += notTool;
                            }
                        }
                    }
                }


            }
            var model = new
            {
                alreadyRegularToolNum = alreadyRegularToolNum,
                alreadySpecialToolNum = alreadySpecialToolNum,
                pendingRegularToolNum = pendingRegularToolNum,
                pendingSpecialToolNum = pendingSpecialToolNum
            };
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var json = serializer.Serialize(model);
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
}