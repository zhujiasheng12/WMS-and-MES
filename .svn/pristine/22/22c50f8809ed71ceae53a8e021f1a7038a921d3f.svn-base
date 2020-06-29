using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebApplication2.Kanban.设备监控;

namespace WebApplication2.生产管理.生产部.区域显示
{
    /// <summary>
    /// 区域显示读机床 的摘要说明
    /// </summary>
    public class 区域显示读机床 : IHttpHandler
    {
        private static readonly object lockObject = new object();
        public void ProcessRequest(HttpContext context)
        {


            {
                int LocationID = int.Parse(context.Request["LocationID"]);
                List<DataList> dataLists = new List<DataList>();
                int RunMachNum = 0;
                int Mach80Num = 0;
                int mach100Num = 0;
                using (JDJS_WMS_DB_USEREntities wms = new JDJS_WMS_DB_USEREntities())
                {
                    var works = wms.JDJS_WMS_Location_Info.ToList();
                    var devices = wms.JDJS_WMS_Device_Info.ToList();
                    List<WebApplication2.Kanban.现场.CncRead> objs = new List<WebApplication2.Kanban.现场.CncRead>();
                    List<int> workIds = new List<int>();
                    WebApplication2.Kanban.现场.机台状态 funs = new WebApplication2.Kanban.现场.机台状态();
                    var Devices = funs.fun(LocationID, works, devices, objs, workIds);
                    //Parallel.ForEach(Devices, device =>
                    foreach (var device in Devices)
                    {
                        int cncID = Convert.ToInt32(device.ID);
                        int sataeInt = 0;
                        var state = wms.JDJS_WMS_Device_RealTime_Data.Where(r => r.CncID == cncID).FirstOrDefault();
                        if (state != null)
                        {
                            if (state.ProgState == 1 || state.ProgState == 2)
                            {
                                sataeInt = 1;
                                //int cncid = device.ID;
                                RunMachNum++;//正在运行中的机床
                                //var platecnc = wms.JDJS_WMS_Quickchangbaseplate_Table.Where(r => r.CncID == cncid);
                                //if (platecnc.Count() > 0)
                                //{
                                //    RunMachNum++;
                                //}
                            }
                        }

                        DateTime nowTime = DateTime.Now;
                        DateTime toDayTimeBegin = DateTime.Now.AddDays(-1);
                        double runTime = 0;
                        double openTime = 0;
                        double lilunTime = 0;
                        double shijiTime = 0;


                        var states = wms.JDJS_WMS_Device_ProgState_Info.Where(r => r.CncID == cncID && (r.StartTime > toDayTimeBegin || r.EndTime > toDayTimeBegin || r.EndTime == null));
                        List<progState> progStates = new List<progState>();
                        Parallel.ForEach(states, real =>
                        //foreach (var real in states)
                        {
                            progState progState = new progState();
                            progState.ProgState = Convert.ToInt32(real.ProgState);
                            progState.startTime = Convert.ToDateTime(real.StartTime);
                            if (Convert.ToDateTime(real.StartTime) < toDayTimeBegin)
                            {
                                progState.startTime = toDayTimeBegin;
                            }
                            if (real.EndTime == null)
                            {
                                progState.EndTime = nowTime;
                            }
                            else
                            {
                                progState.EndTime = Convert.ToDateTime(real.EndTime);
                            }
                            lock (lockObject)
                            {
                                progStates.Add(progState);
                            }
                        });
                        Parallel.ForEach(progStates, real =>
                        //foreach (var real in progStates)
                        {
                            if (real.ProgState != -1)
                            {
                                lock (lockObject)
                                {
                                    openTime += Convert.ToDouble((real.EndTime - real.startTime).TotalMinutes);
                                }
                            }
                            if (real.ProgState == 1)
                            {
                                lock (lockObject)
                                {
                                    runTime += Convert.ToDouble((real.EndTime - real.startTime).TotalMinutes);
                                }
                            }
                        });





                        int processID = -2;
                        int processInfoID = -1;
                        var processing = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.CncID == cncID && r.isFlag == 2);
                        if (processing.Count() > 0)
                        {
                            processID = processing.FirstOrDefault().ID;
                            processInfoID = Convert.ToInt32(processing.FirstOrDefault().ProcessID);
                        }
                        else
                        {

                            //processing = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.CncID == cncID && r.isFlag == 1).OrderBy(r => r.StartTime);
                            //if (processing.Count() > 0)
                            //{
                            //    processInfoID = Convert.ToInt32(processing.FirstOrDefault().ProcessID);
                            //    processID = processing.FirstOrDefault().ID;
                            //}
                        }
                        //获取到当前机床加工任务
                        if (processID != -1)
                        {
                            DataList data = new DataList();
                            if (openTime == 0)
                            {
                                data.jiadonglv = "0%";

                            }
                            else
                            {
                                data.jiadonglv = ((runTime / openTime) * 100).ToString("0.0000") + "%";
                            }
                            data.cncNum = device.MachNum;
                            int processNum = 0;
                            string OrderNum = "";
                            

                            var time = DateTime.Now;
                            if (processing.Count() > 0)
                            {
                                 processNum = Convert.ToInt32(wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == processInfoID).FirstOrDefault().ProcessID);
                                 OrderNum = wms.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == processing.FirstOrDefault().OrderID).FirstOrDefault().Order_Number.ToString();
                                data.doingFileName = OrderNum + "-P" + processNum.ToString();
                                data.doingProcess = OrderNum + "-" + processNum.ToString() + "序";
                                var processInfo = processing.ToList();
                                 time =Convert.ToDateTime ( processInfo.Last().EndTime);
                                data.completionTimeOfCurrentTask = Convert.ToDateTime(time);
                                data.completionTimeOfCurrentTaskStr = processInfo.LastOrDefault().EndTime.ToString().Substring(0, processInfo.LastOrDefault().EndTime.ToString().LastIndexOf(':'));
                            }
                            processing = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.CncID == cncID && r.ProcessID != processInfoID && r.isFlag == 1).OrderBy(r => r.StartTime);
                            if (processing.Count() > 0)
                            {
                                var processInfo = processing.ToList();
                                int id = Convert.ToInt32(processInfo.First().ProcessID);
                                var proces = wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == id).ToList();
                                processNum = Convert.ToInt32(proces.FirstOrDefault().ProcessID);
                                int orderId = Convert.ToInt32(processInfo.FirstOrDefault().OrderID);
                                OrderNum = wms.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == orderId).FirstOrDefault().Order_Number.ToString();
                                data.waitingFileName = OrderNum + "-P" + processNum.ToString();
                                data.waitingProcess = OrderNum + "-" + processNum.ToString() + "序";
                            }
                            else
                            {
                                data.waitingFileName = "";
                                data.waitingProcess = "";
                            }
                            var processAll = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.ProcessID == processInfoID && r.isFlag != 0).OrderBy(r => r.StartTime).ToList();
                            DateTime  startTime = Convert.ToDateTime(DateTime .Now).AddDays (1);
                            DateTime endTime = Convert.ToDateTime(DateTime.Now).AddDays (-1);
                            if (processAll.Count() > 0)
                            {
                                startTime = Convert.ToDateTime(processAll.First().StartTime);
                                 endTime = Convert.ToDateTime(processAll.Last().EndTime);
                            }

                            if (DateTime.Now < startTime)
                            {
                                data.progress = "0";
                            }
                            else if (DateTime.Now > endTime)
                            {
                                data.progress = "1";
                                //lock (lockObject)
                                {
                                    mach100Num++;
                                }
                            }
                            else
                            {
                                double allTime = (endTime - startTime).TotalMinutes;
                                double useTime = (DateTime.Now - startTime).TotalMinutes;
                                if (allTime != 0)
                                {
                                    data.processDou = useTime / allTime;
                                    data.progress = (useTime / allTime).ToString("0.0000");
                                }
                                if (data.processDou < 1 && data.processDou > 0.8)
                                {
                                    //lock (lockObject)
                                    {
                                        Mach80Num++;
                                    }
                                }
                                else if (data.processDou >= 1)
                                {
                                    //lock (lockObject)
                                    {
                                        mach100Num++;
                                    }
                                    data.progress = "1";
                                }
                            }
                            if (sataeInt == 1 && data.progress == "1")
                            {
                                data.progress = "0.9999";
                                //lock (lockObject)
                                {
                                    mach100Num--;
                                    Mach80Num++;
                                }
                            }
                            //lock (lockObject)
                            {
                                dataLists.Add(data);
                            }
                        }
                        else
                        {
                            {
                                DataList data = new DataList();
                                if (openTime == 0)
                                {
                                    data.jiadonglv = "0%";

                                }
                                else
                                {
                                    data.jiadonglv = ((runTime / openTime) * 100).ToString("0.0000") + "%";
                                }
                                data.cncNum = device.MachNum;
                                
                                data.doingFileName ="-";
                                data.doingProcess = "-";
                                data.completionTimeOfCurrentTaskStr = "-";
                                {
                                    data.waitingFileName = "-";
                                    data.waitingProcess = "-";
                                }
                                {
                                    {
                                        data.processDou = 0;
                                        data.progress = "-";
                                    }
                                }
                                {
                                    dataLists.Add(data);
                                }
                            }
                        }


                    }


                    Parallel.For(0,dataLists .Count (),i=>
                    //foreach (var item in dataLists)
                    {
                        dataLists[i].mach100Num = mach100Num.ToString();
                        dataLists[i].Mach80Num = Mach80Num.ToString();
                        dataLists[i].RunMachNum = RunMachNum.ToString();
                    }
                    );
                    var lists = dataLists.OrderByDescending(r => r.progress);
                    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var page = int.Parse(context.Request["page"]);
                    var limit = int.Parse(context.Request["limit"]);
                    var layPage = lists.Skip((page - 1) * limit).Take(limit);
                    var model = new { code = 0, data = layPage, count = lists.Count() };
                    var json = serializer.Serialize(model);
                    context.Response.Write(json);
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
    public class DataList
    {/// <summary>
        /// 机床编号
        /// </summary>
        public string cncNum;
        /// <summary>
        /// 稼动率
        /// </summary>
        public double jiadonglvStr;
        public string jiadonglv;
        /// <summary>
        /// 正在进行订单及工序
        /// </summary>
        public string doingProcess;
        /// <summary>
        /// 等待加工订单及工序
        /// </summary>
        public string waitingProcess;
        /// <summary>
        /// 机台当前订单剩余量
        /// </summary>
        public string surplusNumber;
        /// <summary>
        /// 正在加工文件名
        /// </summary>
        public string doingFileName;
        /// <summary>
        /// 等待加工文件名
        /// </summary>
        public string waitingFileName;
        /// <summary>
        /// 当前任务完成进度
        /// </summary>
        public string progress;
        public double processDou;
        public string RunMachNum;
        public string Mach80Num;
        public string mach100Num;
        /// <summary>
        /// 当前任务预计完成时间
        /// </summary>
        public string completionTimeOfCurrentTaskStr;
        public DateTime completionTimeOfCurrentTask;

    }
}