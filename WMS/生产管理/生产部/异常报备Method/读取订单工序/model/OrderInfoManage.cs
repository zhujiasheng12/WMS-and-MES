using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.生产管理.生产部.订单跟踪.Model;

namespace WebApplication2.生产管理.生产部.异常报备Method.读取订单工序.model
{
    public class OrderInfoManage
    {
        public static List<Order_Abnormal_AllOrderInfo> GetOrderInfo()
        {
            List<Order_Abnormal_AllOrderInfo> infos = new List<Order_Abnormal_AllOrderInfo>();
            try
            {
                using (JDJS_WMS_DB_USEREntities wms = new JDJS_WMS_DB_USEREntities())
                {
                    var orders = wms.JDJS_WMS_Order_Entry_Table.Where(r => r.Intention != -1 && r.Intention != 1 && r.Intention != 5 && r.Intention != -2 && r.Intention != -3);
                    foreach (var order in orders)
                    {
                        Order_Abnormal_AllOrderInfo info = new Order_Abnormal_AllOrderInfo();
                        info.Leader = order.Order_Leader;
                        info.Output = order.Product_Output;
                        info.Id = order.Order_ID;
                        info.State = "生产中";
                        info.orderState = Order_Abnormal_State.生产中;
                        info.PlanEndTime = order.Order_Plan_End_Time == null ? "-" : order.Order_Plan_End_Time.ToString();
                        if (order.Order_Actual_End_Time != null)
                        {
                            info.EndTime = order.Order_Actual_End_Time.ToString();
                        }
                        else
                        {
                            info.EndTime = "-";
                        }
                        info.OrderNum = order.Order_Number;
                        info.ProductName = order.Product_Name;
                        info.ProjectName = order.ProjectName;
                        info.IsOver = false;
                        if (order.Intention == 4)
                        {
                            info.IsOver = true;
                        }
                        var info2 = DataManage.GetWorkInfo(order.Order_ID);
                        if (info2.waitNum > 0)
                        {
                            info.State = "生产中";
                            info.orderState = Order_Abnormal_State.生产中;
                        }
                        else if (info2.waitNum == 0 && info2.Finish == 0)
                        {
                            info.State = "生产中";
                            info.orderState = Order_Abnormal_State.生产中;
                        }


                        var process = wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == order.Order_ID && r.sign != 0).FirstOrDefault();
                        info.NCEndTime = "-";
                        info.NCPlanEndTime = "-";
                        info.NCPerson = order.Engine_Program_Manager == null ? "-" : order.Engine_Program_Manager;
                        info.NCIsOver = false;
                        var guide = wms.JDJS_WMS_Order_Guide_Schedu_Table.Where(r => r.OrderID == order.Order_ID).FirstOrDefault();
                        if (process != null)
                        {
                            if (process.program_audit_sign == 1)
                            {
                                info.NCIsOver = true;
                                info.NCEndTime = process.ProgramePassTime == null ? "-" : process.ProgramePassTime.ToString();
                            }
                        }
                        if (guide != null)
                        {
                            if (guide.ExpectEndTime != null)
                            {
                                info.NCPlanEndTime = guide.ExpectEndTime.ToString();
                            }
                        }
                        info.order_Abnormal_ProcessInfos = new List<Order_Abnormal_ProcessInfo>();

                        info.BlankEndTime = "-";
                        info.BlankIsOver = false;
                        info.BlankPerson = "王克全";
                        info.BlankPlanEndTime = "-";
                        var blankDelay = wms.JDJS_WMS_Order_DelayTime_Table.Where(r => r.OrderID == order.Order_ID).FirstOrDefault();
                        if (blankDelay != null)
                        {
                            if (blankDelay.BlankTime != null)
                            {
                                info.BlankPlanEndTime = Convert.ToDateTime(blankDelay.BlankTime).ToString();
                            }
                        }
                        var row = wms.JDJS_WMS_Order_Blank_Table.Where(r => r.OrderID == order.Order_ID).FirstOrDefault();
                        if (row != null)
                        {
                            if (row.BlankState == "已完成")
                            {
                                info.BlankIsOver = true;
                            }
                            if (row.Expected_Completion_Time != null)
                            {
                                info.BlankPlanEndTime = Convert.ToDateTime(row.Expected_Completion_Time).ToString();
                            }
                        }
                        var processes = wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == order.Order_ID && r.sign != 0);
                        foreach (var item in processes)
                        {
                            Order_Abnormal_ProcessInfo processInfo = new Order_Abnormal_ProcessInfo();
                            processInfo.JiaEndTime = "-";
                            processInfo.JiaIsOver = true;
                            processInfo.JiaPerson = "高杰";
                            processInfo.JiaPlanEndTime = "-";

                            if (process != null)
                            {
                                if (process.Jig_Expected_Completion_Time != null)
                                {
                                    processInfo.JiaPlanEndTime = process.Jig_Expected_Completion_Time.ToString();
                                }
                            }
                            else
                            {
                                processInfo.JiaIsOver = false;
                            }


                            processInfo.ProcessId = item.ID;
                            processInfo.ProcessNum = item.ProcessID.ToString();
                            var fixtureInfo = wms.JDJS_WMS_Order_Fixture_Manager_Table.Where(r => r.ProcessID == item.ID);
                            if (fixtureInfo.Count() > 0)
                            {
                                int DeviceNum = 0;
                                int preparenum = 0;
                                if (fixtureInfo.First().FixtureNumber == null)
                                {

                                    var cnc = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.ProcessID == item.ID && r.isFlag != 0);
                                    List<int> cncid = new List<int>();
                                    foreach (var real in cnc)
                                    {
                                        if (!cncid.Contains(Convert.ToInt32(real.CncID)))
                                        {
                                            cncid.Add(Convert.ToInt32(real.CncID));
                                        }
                                    }
                                    DeviceNum = cncid.Count();
                                }
                                else
                                {
                                    DeviceNum = Convert.ToInt32(fixtureInfo.First().FixtureNumber);
                                }
                                preparenum = Convert.ToInt32(fixtureInfo.First().FixtureFinishPerpareNumber);
                                if (DeviceNum > preparenum)
                                {
                                    processInfo.JiaIsOver = false;
                                }


                            }


                            processInfo.ToolEndTime = "-";
                            processInfo.ToolIsOver = true;
                            processInfo.ToolPerson = "于欢";
                            processInfo.ToolPlanEndTime = "-";
                            var toolDelay = wms.JDJS_WMS_Order_DelayTime_Table.Where(r => r.OrderID == order.Order_ID).FirstOrDefault();
                            if (toolDelay != null)
                            {
                                if (toolDelay.ToolTime != null)
                                {
                                    processInfo.ToolPlanEndTime = Convert.ToDateTime(toolDelay.ToolTime).ToString();
                                }
                            }
                            List<int> cncIds = new List<int>();

                            if (item.toolPreparation != 1)
                            {
                                processInfo.ToolIsOver = false;
                            }
                            var cnces = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.ProcessID == item.ID && (r.isFlag == 1 || r.isFlag == 2));
                            foreach (var real in cnces)
                            {
                                if (!cncIds.Contains(Convert.ToInt32(real.CncID)))
                                {
                                    cncIds.Add(Convert.ToInt32(real.CncID));
                                }
                            }

                            foreach (var cnc in cncIds)
                            {

                                int cncID = cnc;
                                #region 是否完成
                                List<int> shankToolNums = new List<int>();
                                List<int> ToolStandInfos = new List<int>();
                                List<int> ProcessToolInfos = new List<int>();
                                Dictionary<int, string> shanlToolInfo = new Dictionary<int, string>();
                                Dictionary<int, string> ProcessNeedToolInfo = new Dictionary<int, string>();
                                {
                                    var cncs = wms.JDJS_WMS_Device_Info.Where(r => r.ID == cncID);
                                    var shangs = wms.JDJS_WMS_Tool_Shank_Table.Where(r => r.CncID == cncID);
                                    foreach (var real in shangs)
                                    {
                                        var toolID = real.ToolID;
                                        var sp = wms.JDJS_WMS_ToolHolder_Tool_Table.Where(r => r.ID == toolID).FirstOrDefault();
                                        if (sp != null)
                                        {
                                            var spID = sp.ToolSpecifications;
                                            var spinfo = wms.JDJS_WMS_Tool_Stock_History.Where(r => r.Id == spID).FirstOrDefault();
                                            if (spinfo != null)
                                            {
                                                if (!shanlToolInfo.ContainsKey(Convert.ToInt32(real.ToolNum)))
                                                {
                                                    shanlToolInfo.Add(Convert.ToInt32(real.ToolNum), spinfo.KnifeName);
                                                }
                                            }
                                        }
                                        shankToolNums.Add(Convert.ToInt32(real.ToolNum));

                                    }
                                    var process1 = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.CncID == cncID && r.OrderID == order.Order_ID && (r.isFlag == 1 || r.isFlag == 2)).OrderBy(r => r.StartTime).FirstOrDefault();
                                    if (process1 != null)
                                    {
                                        int processID = Convert.ToInt32(process1.ProcessID);
                                        var cncTypeID = cncs.FirstOrDefault().MachType;
                                        var standTool = wms.JDJS_WMS_Tool_Standard_Table.Where(r => r.MachTypeID == cncTypeID);
                                        foreach (var real in standTool)
                                        {
                                            var toolnumstr = real.ToolID;
                                            if (toolnumstr.Length > 1)
                                            {
                                                int toolNum = Convert.ToInt32(toolnumstr.Substring(1));

                                                ToolStandInfos.Add(toolNum);
                                            }
                                        }
                                        var processTools = wms.JDJS_WMS_Order_Process_Tool_Info_Table.Where(r => r.ProcessID == processID);
                                        foreach (var real in processTools)
                                        {
                                            if (!ToolStandInfos.Contains(Convert.ToInt32(real.ToolNO)))
                                            {
                                                ProcessToolInfos.Add(Convert.ToInt32(real.ToolNO));
                                                var ToolSTR = real.ToolName;
                                                int index0 = ToolSTR.IndexOf("[");
                                                int index1 = ToolSTR.IndexOf("]");
                                                if (index1 > index0)
                                                {
                                                    ToolSTR = ToolSTR.Substring(index0 + 1, index1 - index0 - 1);
                                                    if (!ProcessNeedToolInfo.ContainsKey(Convert.ToInt32(real.ToolNO)))
                                                    {
                                                        ProcessNeedToolInfo.Add(Convert.ToInt32(real.ToolNO), ToolSTR);
                                                    }
                                                }
                                            }
                                        }
                                        foreach (var real in ProcessToolInfos)
                                        {
                                            if (!shankToolNums.Contains(real))
                                            {
                                                processInfo.ToolIsOver = false;
                                            }
                                        }

                                    }
                                }
                                if (processInfo.ToolIsOver)
                                {
                                    foreach (var real in ProcessNeedToolInfo)
                                    {
                                        if (shanlToolInfo.ContainsKey(real.Key))
                                        {
                                            if (!(shanlToolInfo[real.Key].Contains(real.Value) || real.Value.Contains(shanlToolInfo[real.Key])))
                                            {
                                                processInfo.ToolIsOver = false;
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            processInfo.ToolIsOver = false;
                                            break;
                                        }
                                    }
                                }
                                #endregion


                            }

                            
                            info.order_Abnormal_ProcessInfos.Add(processInfo);
                        }
                        var info1 = DataManage.GetInfo(order.Order_ID);
                        if (info1[0].IsOver == false)
                        {
                            info.State = "下单中";
                            info.orderState = Order_Abnormal_State.下单中;
                        }
                        else if (info1[0].IsOver == true && info1[1].IsOver == false)
                        {
                            info.State = "编程中";
                            info.orderState = Order_Abnormal_State.编程中;
                        }
                        else if (info1[0].IsOver == true && info1[1].IsOver == true && info1[2].IsOver == false)
                        {
                            info.State = "待生产";
                            info.orderState = Order_Abnormal_State.待生产;
                        }
                        if (order.Intention == 4)
                        {
                            info.State = "已完成";
                            info.orderState = Order_Abnormal_State.已完成;
                        }



                        infos.Add(info);
                    }
                    infos = infos.OrderBy(r => r.orderState).ToList(); ;
                }



            }
            catch (Exception ex)
            {

            }

            return infos;
        }
    }

    public struct Order_Abnormal_AllOrderInfo
    {
        public int Id { get; set; }
        public string OrderNum { get; set; }
        public string ProductName { get; set; }
        public string ProjectName { get; set; }
        public int Output { get; set; }
        public string EndTime { get; set; }
        public string PlanEndTime { get; set; }
        public bool IsOver { get; set; }
        public string State { get; set; }
        public Order_Abnormal_State orderState { get; set; }
        private string _leader;
        public string Leader
        {
            get
            {
                return _leader;
            }
            set
            {
                _leader = value;
                if (value.Length > 0)
                {
                    Xing = _leader[0];
                }
            }
        }
        public char Xing { get; set; }
        public string NCContent { get { return "NC准备"; } }
        public string NCPerson { get; set; }
        public string NCPlanEndTime { get; set; }
        public string NCEndTime { get; set; }
        public bool NCIsOver { get; set; }
        public string BlankContent { get { return "毛坯准备"; } }
        public string BlankPerson { get; set; }
        public string BlankPlanEndTime { get; set; }
        public string BlankEndTime { get; set; }
        public bool BlankIsOver { get; set; }
        public List<Order_Abnormal_ProcessInfo> order_Abnormal_ProcessInfos { get; set; }
    }

    public class Order_Abnormal_ProcessInfo
    {
        public int ProcessId { get; set; }
        public string ProcessNum { get; set; }
        public string JiaContent { get { return "治具准备"; } }
        public string JiaPerson { get; set; }
        public string JiaPlanEndTime { get; set; }
        public string JiaEndTime { get; set; }
        public bool JiaIsOver { get; set; }
        public string ToolContent { get { return "刀具准备"; } }
        public string ToolPerson { get; set; }
        public string ToolPlanEndTime { get; set; }
        public string ToolEndTime { get; set; }
        public bool ToolIsOver { get; set; }
    }

    public enum Order_Abnormal_State
    {
        下单中,
        编程中,
        待生产,
        生产中,
        已完成
    }
}