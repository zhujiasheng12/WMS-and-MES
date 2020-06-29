using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.生产部
{
    /// <summary>
    /// 生产流程读数据 的摘要说明
    /// </summary>
    public class 生产流程读数据 : IHttpHandler
    {
        System.Web.Script.Serialization.JavaScriptSerializer Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

        public void ProcessRequest(HttpContext context)
        {
            var page = int.Parse(context.Request["page"]);
            var limit = int.Parse(context.Request["limit"]);

            {
                //var model1 = new { 订单队列 = 20190801, 待上机数1 = 10 };
                //var model2 = new { 订单队列 = 20190802, 待上机数1 = 20 };

                //var str1 = Serializer.Serialize(model1);
                //var str2 = Serializer.Serialize(model2);
                //var str3 = str1 + "," + str2;

                //var dd = "{\"code\":0,\"data\":[" + str3 + "]}";

                //context.Response.Write(dd);
            }
          
            {

                {
                    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    string str = "";
                    List<PruductionProcssRead> pruductionProcssReads = new List<PruductionProcssRead>();
                    using (JDJS_WMS_DB_USEREntities wms = new JDJS_WMS_DB_USEREntities ())
                    {
                        var orders = wms.JDJS_WMS_Order_Entry_Table.Where(r => r.Intention == 2);
                        foreach (var order in orders)
                        {
                            PruductionProcssRead pruduction = new PruductionProcssRead();

                            pruduction.processInfo = new List<Process>();
                            pruduction.OrderID = order.Order_ID;
                            pruduction.OrderNum = order.Order_Number;
                            pruduction.OutPut = order.Product_Output.ToString();
                            var processes = wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == order.Order_ID && r.sign != 0);
                            foreach (var process in processes)
                            {
                                Process pro = new Process();
                                var not = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.ProcessID == process.ID && r.isFlag == 1);
                                pro.notProcess = not.Count();
                                var donow = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.ProcessID == process.ID && r.isFlag == 2);
                                pro.processing = donow.Count();
                                var done = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.ProcessID == process.ID && r.isFlag == 3);
                                pro.processed = done.Count();
                                pruduction.processInfo.Add(pro);
                            }

                            var confirm = wms.JDJS_WMS_Quality_Confirmation_Table.Where(r => r.OrderID == order.Order_ID);
                            if (confirm.Count() > 0)
                            {
                                if (confirm.First().PefectiveProductNumber == null)
                                {
                                    pruduction.defectiveNumber = "0";
                                }
                                else
                                {
                                    pruduction.defectiveNumber = confirm.First().PefectiveProductNumber.ToString();
                                }
                                if (confirm.First().QualifiedProductNumber == null)
                                {
                                    pruduction.storageNumber = "0";
                                }
                                else
                                {
                                    pruduction.storageNumber = confirm.First().QualifiedProductNumber.ToString();
                                }
                                if (confirm.First().PassRate == null)
                                {
                                    pruduction.passRate = "0";
                                }
                                else
                                {
                                    pruduction.passRate = (confirm.First().PassRate*100).ToString()+"%";
                                }

                            }
                            else
                            {
                                pruduction.storageNumber = "0";
                                pruduction.defectiveNumber = "0";
                                pruduction.passRate = "0";
                            }
                            pruductionProcssReads.Add(pruduction);
                            object orderInfo = "";
                            int ProcessNum = pruduction.processInfo.Count();
                            switch (ProcessNum)
                            {
                                case 0:
                                    orderInfo = new
                                    {
                                        订单队列 = pruduction.OrderNum,
                                        订单总量 = pruduction.OutPut,
                                        不良品数 = pruduction.defectiveNumber,
                                        合格品数 = pruduction.storageNumber,
                                        良率 = pruduction.passRate,
                                    };
                                    break;
                                case 1:
                                    orderInfo = new
                                    {
                                        订单队列 = pruduction.OrderNum,
                                        订单总量 = pruduction.OutPut,
                                        不良品数 = pruduction.defectiveNumber,
                                        合格品数 = pruduction.storageNumber,
                                        良率 = pruduction.passRate,
                                        待上机数1 = pruduction.processInfo[0].notProcess.ToString(),
                                        正在加工数1 = pruduction.processInfo[0].processing.ToString(),
                                        已加工数1 = pruduction.processInfo[0].processed.ToString(),
                                    };
                                    break;
                                case 2:
                                    orderInfo = new
                                    {
                                        订单队列 = pruduction.OrderNum,
                                        订单总量 = pruduction.OutPut,
                                        不良品数 = pruduction.defectiveNumber,
                                        合格品数 = pruduction.storageNumber,
                                        良率 = pruduction.passRate,
                                        待上机数1 = pruduction.processInfo[0].notProcess.ToString(),
                                        正在加工数1 = pruduction.processInfo[0].processing.ToString(),
                                        已加工数1 = pruduction.processInfo[0].processed.ToString(),
                                        待上机数2 = pruduction.processInfo[1].notProcess.ToString(),
                                        正在加工数2 = pruduction.processInfo[1].processing.ToString(),
                                        已加工数2 = pruduction.processInfo[1].processed.ToString(),
                                    };
                                    break;
                                case 3:
                                    orderInfo = new
                                    {
                                        订单队列 = pruduction.OrderNum,
                                        订单总量 = pruduction.OutPut,
                                        不良品数 = pruduction.defectiveNumber,
                                        合格品数 = pruduction.storageNumber,
                                        良率 = pruduction.passRate,
                                        待上机数1 = pruduction.processInfo[0].notProcess.ToString(),
                                        正在加工数1 = pruduction.processInfo[0].processing.ToString(),
                                        已加工数1 = pruduction.processInfo[0].processed.ToString(),
                                        待上机数2 = pruduction.processInfo[1].notProcess.ToString(),
                                        正在加工数2 = pruduction.processInfo[1].processing.ToString(),
                                        已加工数2 = pruduction.processInfo[1].processed.ToString(),
                                        待上机数3 = pruduction.processInfo[2].notProcess.ToString(),
                                        正在加工数3 = pruduction.processInfo[2].processing.ToString(),
                                        已加工数3 = pruduction.processInfo[2].processed.ToString(),
                                    };
                                    break;
                                case 4:
                                    orderInfo = new
                                    {
                                        订单队列 = pruduction.OrderNum,
                                        订单总量 = pruduction.OutPut,
                                        不良品数 = pruduction.defectiveNumber,
                                        合格品数 = pruduction.storageNumber,
                                        良率 = pruduction.passRate,
                                        待上机数1 = pruduction.processInfo[0].notProcess.ToString(),
                                        正在加工数1 = pruduction.processInfo[0].processing.ToString(),
                                        已加工数1 = pruduction.processInfo[0].processed.ToString(),
                                        待上机数2 = pruduction.processInfo[1].notProcess.ToString(),
                                        正在加工数2 = pruduction.processInfo[1].processing.ToString(),
                                        已加工数2 = pruduction.processInfo[1].processed.ToString(),
                                        待上机数3 = pruduction.processInfo[2].notProcess.ToString(),
                                        正在加工数3 = pruduction.processInfo[2].processing.ToString(),
                                        已加工数3 = pruduction.processInfo[2].processed.ToString(),
                                        待上机数4 = pruduction.processInfo[3].notProcess.ToString(),
                                        正在加工数4 = pruduction.processInfo[3].processing.ToString(),
                                        已加工数4 = pruduction.processInfo[3].processed.ToString(),
                                    };
                                    break;
                                case 5:
                                    orderInfo = new
                                    {
                                        订单队列 = pruduction.OrderNum,
                                        订单总量 = pruduction.OutPut,
                                        不良品数 = pruduction.defectiveNumber,
                                        合格品数 = pruduction.storageNumber,
                                        良率 = pruduction.passRate,
                                        待上机数1 = pruduction.processInfo[0].notProcess.ToString(),
                                        正在加工数1 = pruduction.processInfo[0].processing.ToString(),
                                        已加工数1 = pruduction.processInfo[0].processed.ToString(),
                                        待上机数2 = pruduction.processInfo[1].notProcess.ToString(),
                                        正在加工数2 = pruduction.processInfo[1].processing.ToString(),
                                        已加工数2 = pruduction.processInfo[1].processed.ToString(),
                                        待上机数3 = pruduction.processInfo[2].notProcess.ToString(),
                                        正在加工数3 = pruduction.processInfo[2].processing.ToString(),
                                        已加工数3 = pruduction.processInfo[2].processed.ToString(),
                                        待上机数4 = pruduction.processInfo[3].notProcess.ToString(),
                                        正在加工数4 = pruduction.processInfo[3].processing.ToString(),
                                        已加工数4 = pruduction.processInfo[3].processed.ToString(),
                                        待上机数5 = pruduction.processInfo[4].notProcess.ToString(),
                                        正在加工数5 = pruduction.processInfo[4].processing.ToString(),
                                        已加工数5 = pruduction.processInfo[4].processed.ToString(),
                                    };
                                    break;
                                case 6:
                                    orderInfo = new
                                    {
                                        订单队列 = pruduction.OrderNum,
                                        订单总量 = pruduction.OutPut,
                                        不良品数 = pruduction.defectiveNumber,
                                        合格品数 = pruduction.storageNumber,
                                        良率 = pruduction.passRate,
                                        待上机数1 = pruduction.processInfo[0].notProcess.ToString(),
                                        正在加工数1 = pruduction.processInfo[0].processing.ToString(),
                                        已加工数1 = pruduction.processInfo[0].processed.ToString(),
                                        待上机数2 = pruduction.processInfo[1].notProcess.ToString(),
                                        正在加工数2 = pruduction.processInfo[1].processing.ToString(),
                                        已加工数2 = pruduction.processInfo[1].processed.ToString(),
                                        待上机数3 = pruduction.processInfo[2].notProcess.ToString(),
                                        正在加工数3 = pruduction.processInfo[2].processing.ToString(),
                                        已加工数3 = pruduction.processInfo[2].processed.ToString(),
                                        待上机数4 = pruduction.processInfo[3].notProcess.ToString(),
                                        正在加工数4 = pruduction.processInfo[3].processing.ToString(),
                                        已加工数4 = pruduction.processInfo[3].processed.ToString(),
                                        待上机数5 = pruduction.processInfo[4].notProcess.ToString(),
                                        正在加工数5 = pruduction.processInfo[4].processing.ToString(),
                                        已加工数5 = pruduction.processInfo[4].processed.ToString(),
                                        待上机数6 = pruduction.processInfo[5].notProcess.ToString(),
                                        正在加工数6 = pruduction.processInfo[5].processing.ToString(),
                                        已加工数6 = pruduction.processInfo[5].processed.ToString(),
                                    };
                                    break;
                            }
                            var json = serializer.Serialize(orderInfo);
                            str += "," + json;
                        }
                        if (str != "")
                        {
                            str = str.Remove(0, 1);
                        }

                    
                        var dd = "{\"code\":0,\"data\":[" + str + "]}";
                       
                        context.Response.Write(dd);
                    }



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
    public interface IPruductionProcssRead
    {
        /// <summary>
        /// 订单主键ID
        /// </summary>
        int OrderID { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        string OrderNum { get; set; }
        /// <summary>
        /// 订单总量
        /// </summary>
        string OutPut { get; set; }
        /// <summary>
        /// 合格品数
        /// </summary>
        string storageNumber { get; set; }
        /// <summary>
        /// 不良品数
        /// </summary>
        string defectiveNumber { get; set; }
        /// <summary>
        /// 良率
        /// </summary>
        string passRate { get; set; }
    }
    public class PruductionProcssRead : IPruductionProcssRead
    {

        public List<Process> processInfo;
        public int OrderID { get; set; }
        public string OrderNum { get; set; }
        public string OutPut { get; set; }
        public string storageNumber { get; set; }
        public string defectiveNumber { get; set; }
        public string passRate { get; set; }
    }

    public class Process
    {

        /// <summary>
        /// 正在加工数
        /// </summary>
        public int processing;
        /// <summary>
        /// 已加工数
        /// </summary>
        public int processed;
        /// <summary>
        /// 待加工数
        /// </summary>
        public int notProcess;
    }
    class Test
    {
        public string test;
        public string 订单队列;
    }
}