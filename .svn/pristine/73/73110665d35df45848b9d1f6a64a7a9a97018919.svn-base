using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.市场部
{
    /// <summary>
    /// 订单进度读数据 的摘要说明
    /// </summary>
    public class 订单进度读数据 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
           
            {
                List<OrderProcessRead> orderProcesses = new List<OrderProcessRead>();
                using (JDJS_WMS_DB_USEREntities  wms = new JDJS_WMS_DB_USEREntities ())
                {
                    var orders = wms.JDJS_WMS_Order_Entry_Table.Where(r => r.Intention != -1);
                    foreach (var order in orders)
                    {
                        OrderProcessRead orderProcessRead = new OrderProcessRead();
                        orderProcessRead.orderName = order.Product_Name;
                        orderProcessRead.projectName = order.ProjectName;
                        orderProcessRead.OrderID = order.Order_ID.ToString();
                        orderProcessRead.OrderNum = order.Order_Number;
                        orderProcessRead.OrderOwner = order.Order_Leader.ToString();
                        orderProcessRead.EngineProgramManager = order.Engine_Program_Manager;
                        orderProcessRead.OrderRequireNumber = order.Product_Output.ToString();
                        int orderstate = Convert.ToInt32(order.Intention);
                        string clientName = "";
                        var clientList = wms.JDJS_WMS_Order_Guide_Schedu_Table.Where(r => r.OrderID == order.Order_ID);
                        if (clientList.Count() > 0)
                        {
                            clientName = clientList.First().ClientName;
                        }
                        orderProcessRead.clientName = clientName;
                        switch (orderstate)
                        {
                            case 1:
                                orderProcessRead.OrderState = "意向订单";
                                orderProcessRead.EngineState = "";
                                orderProcessRead.AllProcessedNumber = "-";
                                orderProcessRead.GoodProcessNumber = "-";
                                orderProcessRead.PassRate = "-";
                                orderProcessRead.ProcessBeginTime = "-";
                                orderProcessRead.ProcessEndTime = "-";
                                orderProcessRead.ShipNumber = "-";
                                orderProcessRead.TargetProductRatio = "-";

                                break;
                            case 2:
                                orderProcessRead.OrderState = "进行中";
                                var processstate12 = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.OrderID == order.Order_ID && (r.isFlag == 2 || r.isFlag == 1));
                                var processstate3 = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.OrderID == order.Order_ID && (r.isFlag == 3));
                                var processstate1 = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.OrderID == order.Order_ID && (r.isFlag == 1));
                                var processstate2 = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.OrderID == order.Order_ID && (r.isFlag == 2));
                                if (processstate12.Count() > 0)
                                {
                                    if (processstate3.Count() > 0)
                                    {
                                        orderProcessRead.EngineState = "进行中";
                                        var begin = processstate3.OrderBy(r => r.StartTime);
                                        orderProcessRead.ProcessBeginTime = "实际" + begin.First().StartTime.ToString();
                                        var end = processstate12.OrderByDescending(r => r.EndTime);
                                        orderProcessRead.ProcessEndTime = "预计" + end.First().EndTime.ToString();
                                    }
                                    else
                                    {
                                        orderProcessRead.EngineState = "进行中";
                                        if (processstate2.Count() > 0)
                                        {
                                            var begin = processstate12.OrderBy(r => r.StartTime);
                                            orderProcessRead.ProcessBeginTime = "实际" + begin.First().StartTime.ToString();
                                        }
                                        else
                                        {
                                            var begin = processstate12.OrderBy(r => r.StartTime);
                                            orderProcessRead.ProcessBeginTime = "预计" + begin.First().StartTime.ToString();
                                        }
                                        var end = processstate12.OrderByDescending(r => r.EndTime);
                                        orderProcessRead.ProcessEndTime = "预计" + end.First().EndTime.ToString();
                                    }

                                }
                                else
                                {
                                    if (processstate3.Count() > 0)
                                    {
                                        orderProcessRead.EngineState = "已完成";
                                        var begin = processstate3.OrderBy(r => r.StartTime);
                                        orderProcessRead.ProcessBeginTime = "实际" + begin.First().StartTime.ToString();
                                        var end = processstate3.OrderByDescending(r => r.EndTime);
                                        orderProcessRead.ProcessEndTime = "实际" + end.First().EndTime;
                                    }
                                    else
                                    {
                                        orderProcessRead.EngineState = "未进行";
                                        orderProcessRead.ProcessBeginTime = "-";
                                        orderProcessRead.ProcessEndTime = "-";
                                    }
                                }
                                int OverNum = 0;
                                List<int> processID = new List<int>();
                                var orderprocess = wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == order.Order_ID && r.sign != 0);
                                if (orderprocess.Count() > 0)
                                {
                                    foreach (var item in orderprocess)
                                    {
                                        int id = Convert.ToInt32(item.ProcessID);
                                        if (!processID.Contains(id))
                                        {
                                            processID.Add(id);
                                        }
                                    }
                                    int max = processID.Max();
                                    var MaxProcess = wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == order.Order_ID && r.sign != 0 && r.ProcessID == max).First();
                                    var over = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.ProcessID == MaxProcess.ID && r.isFlag == 3);
                                    OverNum = over.Count();
                                }
                                orderProcessRead.AllProcessedNumber = OverNum.ToString();
                                var confirm = wms.JDJS_WMS_Quality_Confirmation_Table.Where(r => r.OrderID == order.Order_ID);
                                if (confirm.Count() > 0)
                                {

                                    if (OverNum < 1)
                                    {
                                        orderProcessRead.PassRate = "0";
                                        orderProcessRead.GoodProcessNumber = "0";
                                        orderProcessRead.ShipNumber = "0";
                                        orderProcessRead.TargetProductRatio = "0";
                                    }
                                    else
                                    {
                                        orderProcessRead.GoodProcessNumber = confirm.First().QualifiedProductNumber.ToString();
                                        orderProcessRead.PassRate = (Convert.ToDouble(confirm.First().QualifiedProductNumber) / OverNum).ToString("0.000000");
                                        var finish = wms.JDJS_WMS_Finished_Product_Manager.Where(r => r.OrderID == order.Order_ID);
                                        if (finish.Count() > 0)
                                        {
                                            orderProcessRead.ShipNumber = finish.First().outputNumber.ToString(); ;
                                        }
                                        else
                                        {
                                            orderProcessRead.ShipNumber = "0";
                                        }
                                        orderProcessRead.TargetProductRatio = (Convert.ToDouble(confirm.First().QualifiedProductNumber) / Convert.ToDouble(order.Product_Output)).ToString("0.000000");
                                    }

                                }
                                else
                                {
                                    orderProcessRead.GoodProcessNumber = "0";
                                    orderProcessRead.PassRate = "0";

                                    orderProcessRead.ShipNumber = "0";
                                    orderProcessRead.TargetProductRatio = "0";
                                }
                                break;
                            case 3:
                                orderProcessRead.OrderState = "进行中";
                                orderProcessRead.EngineState = "未进行";
                                orderProcessRead.AllProcessedNumber = "0";
                                orderProcessRead.GoodProcessNumber = "0";
                                orderProcessRead.PassRate = "0";
                                orderProcessRead.ProcessBeginTime = "-";
                                orderProcessRead.ProcessEndTime = "-";
                                orderProcessRead.ShipNumber = "0";
                                orderProcessRead.TargetProductRatio = "0";
                                break;
                            case 4:
                                orderProcessRead.OrderState = "已完成";
                                orderProcessRead.EngineState = "已完成";
                                int OverNums = 0;
                                List<int> processIDs = new List<int>();
                                var orderprocesss = wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == order.Order_ID && r.sign != 0);
                                if (orderprocesss.Count() > 0)
                                {
                                    foreach (var item in orderprocesss)
                                    {
                                        int id = Convert.ToInt32(item.ProcessID);
                                        if (!processIDs.Contains(id))
                                        {
                                            processIDs.Add(id);
                                        }
                                    }
                                    int max = processIDs.Max();
                                    var MaxProcess = wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == order.Order_ID && r.sign != 0 && r.ProcessID == max).First();
                                    var over = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.ProcessID == MaxProcess.ID && r.isFlag == 3);
                                    OverNums = over.Count();
                                }
                                orderProcessRead.AllProcessedNumber = OverNums.ToString();

                                var confirms = wms.JDJS_WMS_Quality_Confirmation_Table.Where(r => r.OrderID == order.Order_ID);
                                if (confirms.Count() > 0)
                                {

                                    if (OverNums < 1)
                                    {
                                        orderProcessRead.PassRate = "0";
                                        orderProcessRead.GoodProcessNumber = "0";
                                        orderProcessRead.ShipNumber = "0";
                                        orderProcessRead.TargetProductRatio = "0";
                                    }
                                    else
                                    {
                                        orderProcessRead.GoodProcessNumber = confirms.First().QualifiedProductNumber.ToString();
                                        orderProcessRead.PassRate = (Convert.ToDouble(confirms.First().QualifiedProductNumber) / OverNums).ToString("0.000000");
                                        var finish = wms.JDJS_WMS_Finished_Product_Manager.Where(r => r.OrderID == order.Order_ID);
                                        if (finish.Count() > 0)
                                        {
                                            orderProcessRead.ShipNumber = finish.First().outputNumber.ToString(); ;
                                        }
                                        else
                                        {
                                            orderProcessRead.ShipNumber = "0";
                                        }
                                        orderProcessRead.TargetProductRatio = (Convert.ToDouble(confirms.First().QualifiedProductNumber) / Convert.ToDouble(order.Product_Output)).ToString("0.000000");
                                    }

                                }
                                else
                                {
                                    orderProcessRead.GoodProcessNumber = "0";
                                    orderProcessRead.PassRate = "0";

                                    orderProcessRead.ShipNumber = "0";
                                    orderProcessRead.TargetProductRatio = "0";
                                }
                                var process = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.OrderID == order.Order_ID && r.isFlag == 3);
                                var begins = process.OrderBy(r => r.StartTime);
                                orderProcessRead.ProcessBeginTime = "实际" + begins.First().StartTime.ToString();
                                var ends = process.OrderByDescending(r => r.EndTime);
                                orderProcessRead.ProcessEndTime = "实际" + ends.First().EndTime.ToString();
                                break;
                            default:
                                orderProcessRead.OrderState = "已完成";
                                orderProcessRead.EngineState = "已完成";
                                orderProcessRead.AllProcessedNumber = "-";
                                orderProcessRead.GoodProcessNumber = "-";
                                orderProcessRead.PassRate = "-";
                                orderProcessRead.ProcessBeginTime = "-";
                                orderProcessRead.ProcessEndTime = "-";
                                orderProcessRead.ShipNumber = "-";
                                orderProcessRead.TargetProductRatio = "-";
                                break;
                        }
                        orderProcesses.Add(orderProcessRead);

                    }


                }
                var page = int.Parse(context.Request["page"]);
                var limit = int.Parse(context.Request["limit"]);
                var layPage = orderProcesses.Skip((page - 1) * limit).Take(limit);
                var model = new { code = 0, data = layPage,count=orderProcesses.Count };
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
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
    public class OrderProcessRead
    {
        /// <summary>
        /// 订单主键ID
        /// </summary>
        public string OrderID;
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNum;
        public string orderName;
        /// <summary>
        /// 订单责任人
        /// </summary>
        public string OrderOwner;
        /// <summary>
        /// 工程编程责任人
        /// </summary>
        public string EngineProgramManager;
        /// <summary>
        /// 订单需求量
        /// </summary>
        public string OrderRequireNumber;
        /// <summary>
        /// 已发货量
        /// </summary>
        public string ShipNumber;
        /// <summary>
        /// 当前已加工总数
        /// </summary>
        public string AllProcessedNumber;
        /// <summary>
        /// 当前已加工良品数
        /// </summary>
        public string GoodProcessNumber;
        /// <summary>
        /// 当前良品率
        /// </summary>
        public string PassRate;
        /// <summary>
        /// 目标产量占比
        /// </summary>
        public string TargetProductRatio;
        /// <summary>
        /// 工程开始时间
        /// </summary>
        public string ProcessBeginTime;
        /// <summary>
        /// 工程结束时间
        /// </summary>
        public string ProcessEndTime;
        /// <summary>
        /// 订单状态
        /// </summary>
        public string OrderState;
        /// <summary>
        /// 工程状态
        /// </summary>
        public string EngineState;
        public string projectName;
        public string clientName;
    }
}