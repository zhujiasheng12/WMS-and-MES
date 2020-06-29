using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.刀具管理.备刀管理
{
    /// <summary>
    /// 装刀管理读数据 的摘要说明
    /// </summary>
    public class 装刀管理读数据 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            {
                //List<ToolInstallationRead> toolReads = new List<ToolInstallationRead>();
                //toolReads.Add(new ToolInstallationRead
                //{
                //    machNum = "20190729",
                //    currentOrderTask = "20190729-P1",

                //    toolNum = "T24",
                //    toolName = "平底",
                //    toolSpecification = "D4R2",
                //    currentTaskCompletionDegree = "50%",
                //    nextOrderTask = "20190730",
                //    nextToolNum = "T24",
                //    nextToolName = "平底",
                //    nextToolSpecification = "D4R2",

                //});

                //toolReads.Add(new ToolInstallationRead
                //{
                //    machNum = "20190729",
                //    currentOrderTask = "20190729-P1",

                //    toolNum = "T25",
                //    toolName = "平底",
                //    toolSpecification = "D4R2",
                //    currentTaskCompletionDegree = "50%",
                //    nextOrderTask = "20190730",
                //    nextToolNum = "T25",
                //    nextToolName = "平底",
                //    nextToolSpecification = "D4R2",

                //});
            }

            List<ToolInstallationRead> toolInstallationReads = new List<ToolInstallationRead>();
            using (JDJS_WMS_DB_USEREntities CIE = new JDJS_WMS_DB_USEREntities())
            {
                using (System.Data.Entity.DbContextTransaction mytran = CIE.Database.BeginTransaction())
                {
                    try
                    {
                       
                        var Cncs = CIE.JDJS_WMS_Device_Info.ToArray();
                        foreach (var cnc in Cncs)
                        {
                            List<string> toolsNo = new List<string>();
                            var toolStand = CIE.JDJS_WMS_Tool_Standard_Table.Where (r=>r.MachTypeID ==cnc.MachType).ToList();
                            foreach (var meal in toolStand)
                            {
                                toolsNo.Add(meal.ToolID);
                            }
                            var processInfo = CIE.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.CncID == cnc.ID && r.isFlag == 2).OrderBy(r => r.StartTime);
                            var OverProcess = CIE.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.ProcessID == processInfo.FirstOrDefault().ProcessID && r.isFlag != 0 && r.isFlag == 3);
                            int OverCount = OverProcess.Count();
                            if (processInfo.Count() > 0)
                            {

                                var tools = CIE.JDJS_WMS_Order_Process_Tool_Info_Table.Where(r => r.ProcessID == processInfo.FirstOrDefault().ProcessID ).ToList ();
                                List<JDJS_WMS_Order_Process_Tool_Info_Table> tool = new List<JDJS_WMS_Order_Process_Tool_Info_Table>();
                                foreach (var real in tools)
                                {
                                    if (!toolsNo.Contains("T" + real.ToolNO.ToString()))
                                    {
                                        tool.Add(real);
                                    }
                                }
                                var nextProcessInfo = CIE.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.CncID == cnc.ID && r.isFlag == 1 && r.ProcessID != processInfo.FirstOrDefault().ProcessID).OrderBy(r => r.StartTime);
                                if (nextProcessInfo.Count() > 0)

                                {
                                    var nextTools = CIE.JDJS_WMS_Order_Process_Tool_Info_Table.Where(r => r.ProcessID == nextProcessInfo.FirstOrDefault().ProcessID ).ToList();
                                    List<JDJS_WMS_Order_Process_Tool_Info_Table> nextTool = new List<JDJS_WMS_Order_Process_Tool_Info_Table>();
                                    foreach (var real in nextTools)
                                    {
                                        if (!toolsNo.Contains("T" + real.ToolNO.ToString()))
                                        {
                                            nextTool.Add(real);
                                        }
                                    }
                                    int toolNum = tool.Count();
                                    int nextToolNum = nextTool.Count();
                                    int maxNum = Math.Max(toolNum, nextToolNum);
                                    for (int i = 0; i < maxNum; i++)
                                    {
                                        ToolInstallationRead toolInstallation = new ToolInstallationRead();
                                        if (i < toolNum && i < nextToolNum)
                                        {
                                            toolInstallation.machNum = cnc.MachNum;
                                            var processid = CIE.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == processInfo.FirstOrDefault().ProcessID && r.sign != 0);
                                            var order = CIE.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == processInfo.FirstOrDefault().OrderID);
                                            int OrderNumber = Convert.ToInt32(order.FirstOrDefault().Product_Output);
                                            toolInstallation.currentOrderTask = order.FirstOrDefault().Order_Number.ToString() + "-P" + processid.FirstOrDefault().ProcessID.ToString();
                                            toolInstallation.toolNum = tool[i].ToolNO.ToString();
                                            toolInstallation.toolName = tool[i].ToolName.ToString();
                                            toolInstallation.toolSpecification = tool[i].ToolName.ToString();
                                            toolInstallation.currentTaskCompletionDegree = (OverCount / OrderNumber).ToString("0.0000");
                                            var nextProcessid = CIE.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == nextProcessInfo.FirstOrDefault().ProcessID && r.sign != 0);
                                            var NextOrder = CIE.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == nextProcessInfo.FirstOrDefault().OrderID);
                                            toolInstallation.nextOrderTask = NextOrder.FirstOrDefault().Order_Number + "-P" + nextProcessid.FirstOrDefault().ProcessID.ToString();
                                            toolInstallation.nextToolNum = nextTool[i].ToolNO.ToString();
                                            toolInstallation.nextToolName = nextTool[i].ToolName.ToString();
                                            toolInstallation.nextToolSpecification = nextTool[i].ToolName.ToString();

                                        }
                                        else if (i >= toolNum && i < nextToolNum)//下一个订单刀多
                                        {
                                            toolInstallation.machNum = cnc.MachNum;
                                            var processid = CIE.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == processInfo.FirstOrDefault().ProcessID && r.sign != 0);
                                            var order = CIE.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == processInfo.FirstOrDefault().OrderID);
                                            int OrderNumber = Convert.ToInt32(order.FirstOrDefault().Product_Output);
                                            toolInstallation.currentOrderTask = order.FirstOrDefault().Order_Number.ToString() + "-P" + processid.FirstOrDefault().ProcessID.ToString();
                                            toolInstallation.toolNum = "";
                                            toolInstallation.toolName = "";
                                            toolInstallation.toolSpecification = "";
                                            toolInstallation.currentTaskCompletionDegree = (OverCount / OrderNumber).ToString("0.0000");
                                            var nextProcessid = CIE.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == nextProcessInfo.FirstOrDefault().ProcessID && r.sign != 0);
                                            var NextOrder = CIE.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == nextProcessInfo.FirstOrDefault().OrderID);
                                            toolInstallation.nextOrderTask = NextOrder.FirstOrDefault().Order_Number + "-P" + nextProcessid.FirstOrDefault().ProcessID.ToString();
                                            toolInstallation.nextToolNum = nextTool[i].ToolNO.ToString();
                                            toolInstallation.nextToolName = nextTool[i].ToolName.ToString();
                                            toolInstallation.nextToolSpecification = nextTool[i].ToolName.ToString();

                                        }
                                        else if (i >= nextToolNum && i < toolNum)//现在订单刀多
                                        {
                                            toolInstallation.machNum = cnc.MachNum;
                                            var processid = CIE.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == processInfo.FirstOrDefault().ProcessID && r.sign != 0);
                                            var order = CIE.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == processInfo.FirstOrDefault().OrderID);
                                            int OrderNumber = Convert.ToInt32(order.FirstOrDefault().Product_Output);
                                            toolInstallation.currentOrderTask = order.FirstOrDefault().Order_Number.ToString() + "-P" + processid.FirstOrDefault().ProcessID.ToString();
                                            toolInstallation.toolNum = tool[i].ToolNO.ToString();
                                            toolInstallation.toolName = tool[i].ToolName.ToString();
                                            toolInstallation.toolSpecification = tool[i].ToolName.ToString();
                                            toolInstallation.currentTaskCompletionDegree = (OverCount / OrderNumber).ToString("0.0000");
                                            var nextProcessid = CIE.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == nextProcessInfo.FirstOrDefault().ProcessID && r.sign != 0);
                                            var NextOrder = CIE.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == nextProcessInfo.FirstOrDefault().OrderID);
                                            toolInstallation.nextOrderTask = NextOrder.FirstOrDefault().Order_Number + "-P" + nextProcessid.FirstOrDefault().ProcessID.ToString();
                                            toolInstallation.nextToolNum = "";
                                            toolInstallation.nextToolName = "";
                                            toolInstallation.nextToolSpecification = "";
                                        }

                                        toolInstallationReads.Add(toolInstallation);
                                    }
                                }
                                else//下个订单为空
                                {
                                    int toolNum = tool.Count();
                                    for (int i = 0; i < toolNum; i++)
                                    {
                                        ToolInstallationRead toolInstallation = new ToolInstallationRead();

                                        toolInstallation.machNum = cnc.MachNum;
                                        var processid = CIE.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == processInfo.FirstOrDefault().ProcessID && r.sign != 0);
                                        var order = CIE.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == processInfo.FirstOrDefault().OrderID);
                                        int OrderNumber = Convert.ToInt32(order.FirstOrDefault().Product_Output);
                                        toolInstallation.currentOrderTask = order.FirstOrDefault().Order_Number.ToString() + "-P" + processid.FirstOrDefault().ProcessID.ToString();
                                        toolInstallation.toolNum = tool[i].ToolNO.ToString();
                                        toolInstallation.toolName = tool[i].ToolName.ToString();
                                        toolInstallation.toolSpecification = tool[i].ToolName.ToString();
                                        toolInstallation.currentTaskCompletionDegree = (OverCount / OrderNumber).ToString("0.0000");

                                        toolInstallation.nextOrderTask = "";
                                        toolInstallation.nextToolNum = "";
                                        toolInstallation.nextToolName = "";
                                        toolInstallation.nextToolSpecification = "";

                                        toolInstallationReads.Add(toolInstallation);

                                    }
                                }
                            }
                            else//没有正在进行的订单
                            {
                                processInfo = CIE.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.CncID == cnc.ID && r.isFlag == 1).OrderBy(r => r.StartTime);
                                if (processInfo.Count() > 0)
                                {

                                        var tools = CIE.JDJS_WMS_Order_Process_Tool_Info_Table.Where(r => r.ProcessID == processInfo.FirstOrDefault().ProcessID).ToList();
                                    List<JDJS_WMS_Order_Process_Tool_Info_Table> tool = new List<JDJS_WMS_Order_Process_Tool_Info_Table>();
                                    foreach (var real in tools)
                                    {
                                        if (!toolsNo.Contains("T" + real.ToolNO.ToString()))
                                        {
                                            tool.Add(real);
                                        }
                                    }
                                        var nextProcessInfo = CIE.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.CncID == cnc.ID && r.isFlag == 1 && r.ProcessID != processInfo.FirstOrDefault().ProcessID).OrderBy(r => r.StartTime);
                                        if (nextProcessInfo.Count() > 0)

                                        {
                                            var nextTools = CIE.JDJS_WMS_Order_Process_Tool_Info_Table.Where(r => r.ProcessID == nextProcessInfo.FirstOrDefault().ProcessID).ToList();
                                        List<JDJS_WMS_Order_Process_Tool_Info_Table> nextTool = new List<JDJS_WMS_Order_Process_Tool_Info_Table>();
                                        foreach (var real in nextTools)
                                        {
                                            if (!toolsNo.Contains("T" + real.ToolNO.ToString()))
                                            {
                                                nextTool.Add(real);
                                            }
                                        }
                                        int toolNum = tool.Count();
                                            int nextToolNum = nextTool.Count();
                                            int maxNum = Math.Max(toolNum, nextToolNum);
                                            for (int i = 0; i < maxNum; i++)
                                            {
                                                ToolInstallationRead toolInstallation = new ToolInstallationRead();
                                                if (i < toolNum && i < nextToolNum)
                                                {
                                                    toolInstallation.machNum = cnc.MachNum;
                                                    var processid = CIE.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == processInfo.FirstOrDefault().ProcessID && r.sign != 0);
                                                    var order = CIE.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == processInfo.FirstOrDefault().OrderID);
                                                    int OrderNumber = Convert.ToInt32(order.FirstOrDefault().Product_Output);
                                                    toolInstallation.currentOrderTask = order.FirstOrDefault().Order_Number.ToString() + "-P" + processid.FirstOrDefault().ProcessID.ToString();
                                                    toolInstallation.toolNum = tool[i].ToolNO.ToString();
                                                    toolInstallation.toolName = tool[i].ToolName.ToString();
                                                    toolInstallation.toolSpecification = tool[i].ToolName.ToString();
                                                    toolInstallation.currentTaskCompletionDegree = (OverCount / OrderNumber).ToString("0.0000");
                                                    var nextProcessid = CIE.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == nextProcessInfo.FirstOrDefault().ProcessID && r.sign != 0);
                                                    var NextOrder = CIE.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == nextProcessInfo.FirstOrDefault().OrderID);
                                                    toolInstallation.nextOrderTask = NextOrder.FirstOrDefault().Order_Number + "-P" + nextProcessid.FirstOrDefault().ProcessID.ToString();
                                                    toolInstallation.nextToolNum = nextTool[i].ToolNO.ToString();
                                                    toolInstallation.nextToolName = nextTool[i].ToolName.ToString();
                                                    toolInstallation.nextToolSpecification = nextTool[i].ToolName.ToString();

                                                }
                                                else if (i >= toolNum && i < nextToolNum)//下一个订单刀多
                                                {
                                                    toolInstallation.machNum = cnc.MachNum;
                                                    var processid = CIE.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == processInfo.FirstOrDefault().ProcessID && r.sign != 0);
                                                    var order = CIE.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == processInfo.FirstOrDefault().OrderID);
                                                    int OrderNumber = Convert.ToInt32(order.FirstOrDefault().Product_Output);
                                                    toolInstallation.currentOrderTask = order.FirstOrDefault().Order_Number.ToString() + "-P" + processid.FirstOrDefault().ProcessID.ToString();
                                                    toolInstallation.toolNum = "";
                                                    toolInstallation.toolName = "";
                                                    toolInstallation.toolSpecification = "";
                                                    toolInstallation.currentTaskCompletionDegree = (OverCount / OrderNumber).ToString("0.0000");
                                                    var nextProcessid = CIE.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == nextProcessInfo.FirstOrDefault().ProcessID && r.sign != 0);
                                                    var NextOrder = CIE.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == nextProcessInfo.FirstOrDefault().OrderID);
                                                    toolInstallation.nextOrderTask = NextOrder.FirstOrDefault().Order_Number + "-P" + nextProcessid.FirstOrDefault().ProcessID.ToString();
                                                    toolInstallation.nextToolNum = nextTool[i].ToolNO.ToString();
                                                    toolInstallation.nextToolName = nextTool[i].ToolName.ToString();
                                                    toolInstallation.nextToolSpecification = nextTool[i].ToolName.ToString();

                                                }
                                                else if (i >= nextToolNum && i < toolNum)//现在订单刀多
                                                {
                                                    toolInstallation.machNum = cnc.MachNum;
                                                    var processid = CIE.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == processInfo.FirstOrDefault().ProcessID && r.sign != 0);
                                                    var order = CIE.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == processInfo.FirstOrDefault().OrderID);
                                                    int OrderNumber = Convert.ToInt32(order.FirstOrDefault().Product_Output);
                                                    toolInstallation.currentOrderTask = order.FirstOrDefault().Order_Number.ToString() + "-P" + processid.FirstOrDefault().ProcessID.ToString();
                                                    toolInstallation.toolNum = tool[i].ToolNO.ToString();
                                                    toolInstallation.toolName = tool[i].ToolName.ToString();
                                                    toolInstallation.toolSpecification = tool[i].ToolName.ToString();
                                                    toolInstallation.currentTaskCompletionDegree = (OverCount / OrderNumber).ToString("0.0000");
                                                    var nextProcessid = CIE.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == nextProcessInfo.FirstOrDefault().ProcessID && r.sign != 0);
                                                    var NextOrder = CIE.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == nextProcessInfo.FirstOrDefault().OrderID);
                                                    toolInstallation.nextOrderTask = NextOrder.FirstOrDefault().Order_Number + "-P" + nextProcessid.FirstOrDefault().ProcessID.ToString();
                                                    toolInstallation.nextToolNum = "";
                                                    toolInstallation.nextToolName = "";
                                                    toolInstallation.nextToolSpecification = "";
                                                }

                                                toolInstallationReads.Add(toolInstallation);
                                            }
                                        } 
                                    else//没有下一个订单
                                    {
                                        //var tool = CIE.JDJS_WMS_Order_Process_Tool_Info_Table.Where(r => r.ProcessID == processInfo.FirstOrDefault().ProcessID && r.ToolNO > 23).ToArray();
                                        int toolNum = tool.Count();
                                        for (int i = 0; i < toolNum; i++)
                                        {
                                            ToolInstallationRead toolInstallation = new ToolInstallationRead();

                                            toolInstallation.machNum = cnc.MachNum;
                                            var processid = CIE.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == processInfo.FirstOrDefault().ProcessID && r.sign != 0);
                                            var order = CIE.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == processInfo.FirstOrDefault().OrderID);
                                            int OrderNumber = Convert.ToInt32(order.FirstOrDefault().Product_Output);
                                            toolInstallation.currentOrderTask = order.FirstOrDefault().Order_Number.ToString() + "-P" + processid.FirstOrDefault().ProcessID.ToString();
                                            toolInstallation.toolNum = tool[i].ToolNO.ToString();
                                            toolInstallation.toolName = tool[i].ToolName.ToString();
                                            toolInstallation.toolSpecification = tool[i].ToolName.ToString();
                                            toolInstallation.currentTaskCompletionDegree = (OverCount / OrderNumber).ToString("0.0000");

                                            toolInstallation.nextOrderTask = "";
                                            toolInstallation.nextToolNum = "";
                                            toolInstallation.nextToolName = "";
                                            toolInstallation.nextToolSpecification = "";

                                            toolInstallationReads.Add(toolInstallation);
                                        }
                                    }
                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        context.Response.Write(ex.Message);
                        mytran.Rollback();
                    }
                }
            }










            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var model = new { code = 0, data = toolInstallationReads };
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
    class ToolInstallationRead
    {
        /// <summary>
        /// 机台号
        /// </summary>
        public string machNum;
        /// <summary>
        /// 当前订单任务
        /// </summary>
        public string currentOrderTask;
        public string toolNum;
        public string toolName;
        public string toolSpecification;
        /// <summary>
        /// 当前任务完成度
        /// </summary>
        public string currentTaskCompletionDegree;
        /// <summary>
        /// 下个订单任务
        /// </summary>
        public string nextOrderTask;
        public string nextToolNum;
        public string nextToolName;
        public string nextToolSpecification;

      
    }
}