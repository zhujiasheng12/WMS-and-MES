using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Kanban.设备监控
{
    /// <summary>
    /// 机台生产状态 的摘要说明
    /// </summary>
    public class 机台生产状态 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //机台生产状态
            Int32 workId = 0;
            if (Int32.TryParse(context.Request["workId"], out workId))
            {

            }
            else
            {
                return;
            }
            int LocationID = workId;//车间位置主键ID
            List<DeviceStateInfo> deviceStateInfos = new List<DeviceStateInfo>();
            using (JDJS_WMS_DB_USEREntities  wms = new  JDJS_WMS_DB_USEREntities ())
            {
                var devices = wms.JDJS_WMS_Device_Info.Where(r => r.Position == LocationID);
                foreach (var item in devices)
                {
                    int cncID = item.ID;
                    DeviceStateInfo deviceStateInfo = new DeviceStateInfo();
                    deviceStateInfo.CncNum = item.MachNum;
                    var realstate = wms.JDJS_WMS_Device_RealTime_Data.Where(r => r.CncID == cncID);
                    if (realstate.Count() > 0)
                    {
                        deviceStateInfo.state = Convert.ToInt32(realstate.FirstOrDefault().ProgState);
                    }
                    
                    var processing = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.CncID == cncID && r.isFlag == 2);
                    if (processing.Count() > 0)
                    {
                        int id = Convert.ToInt32(processing.FirstOrDefault().ProcessID);
                        int processNum = Convert.ToInt32(wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == id).FirstOrDefault().ProcessID);
                        string OrderNum = wms.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == processing.FirstOrDefault().OrderID).FirstOrDefault().Order_Number.ToString();

                        deviceStateInfo.CurrTask = OrderNum + "-" + processNum.ToString() + "序";
                        var pro3 = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.ProcessID == id && r.isFlag == 3 && r.CncID == cncID);
                        var pro1 = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.ProcessID == id && r.isFlag == 1 && r.CncID == cncID);
                        var pro2 = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.ProcessID == id && r.isFlag == 2 && r.CncID == cncID);

                        deviceStateInfo.CurrTaskRate = (pro3.Count() / (pro1.Count() + pro2.Count() + pro3.Count())).ToString();

                        var nextTask = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.CncID == cncID && r.isFlag == 1 && r.ProcessID != processing.FirstOrDefault().ProcessID).OrderBy(r => r.StartTime);
                        if (nextTask.Count() > 0)
                        {
                            int ids = Convert.ToInt32(nextTask.FirstOrDefault().ProcessID);
                            int processNums = Convert.ToInt32(wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == ids).FirstOrDefault().ProcessID);
                            string OrderNums = wms.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == nextTask.FirstOrDefault().OrderID).FirstOrDefault().Order_Number.ToString();
                            deviceStateInfo.NextTask = OrderNums + "-" + processNums.ToString() + "序";
                        }
                    }
                    else
                    {
                        processing = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.CncID == cncID && r.isFlag == 1).OrderBy(r => r.StartTime);
                        if (processing.Count() > 0)
                        {
                            if (processing.Count() > 1)
                            {
                                var pro = processing.ToList();
                                int id = Convert.ToInt32(processing.FirstOrDefault().ProcessID);
                                int processNum = Convert.ToInt32(wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == id).FirstOrDefault().ProcessID);
                                string OrderNum = wms.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == processing.FirstOrDefault().OrderID).FirstOrDefault().Order_Number.ToString();

                                deviceStateInfo.CurrTask = OrderNum + "-" + processNum.ToString() + "序";
                                var pro3 = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.ProcessID == id && r.isFlag == 3 && r.CncID == cncID);
                                var pro1 = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.ProcessID == id && r.isFlag == 1 && r.CncID == cncID);
                                var pro2 = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.ProcessID == id && r.isFlag == 2 && r.CncID == cncID);

                                deviceStateInfo.CurrTaskRate = (pro3.Count() / (pro1.Count() + pro2.Count() + pro3.Count())).ToString();

                                var willdo = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.CncID == cncID && r.ProcessID == processing.FirstOrDefault().ProcessID && r.isFlag == 1);



                                int processID =Convert.ToInt32( pro[1].ProcessID);

                                var nextTask = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.CncID == cncID && r.isFlag == 1 && r.ProcessID !=processID).OrderBy(r => r.StartTime);
                                if (nextTask.Count() > 0)
                                {
                                    int idss = Convert.ToInt32(nextTask.FirstOrDefault().ProcessID);
                                    int processNumss = Convert.ToInt32(wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == idss).FirstOrDefault().ProcessID);
                                    string OrderNumss = wms.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == nextTask.FirstOrDefault().OrderID).FirstOrDefault().Order_Number.ToString();
                                    deviceStateInfo.NextTask = OrderNumss + "-" + processNumss.ToString() + "序";
                                }
                            }
                            else
                            {
                                int id = Convert.ToInt32(processing.FirstOrDefault().ProcessID);
                                int processNum = Convert.ToInt32(wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == id).FirstOrDefault().ProcessID);
                                string OrderNum = wms.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == processing.FirstOrDefault().OrderID).FirstOrDefault().Order_Number.ToString();

                                deviceStateInfo.CurrTask = OrderNum + "-" + processNum.ToString() + "序";
                                var willdo = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.CncID == cncID && r.ProcessID == processing.FirstOrDefault().ProcessID && r.isFlag == 1);



                            }
                        }
                    }
                    DateTime timenow = DateTime.Now;
                    var jdjs = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.CncID == cncID && r.isFlag == 3);
                    foreach (var real in jdjs)
                    {
                        DateTime endtime = Convert.ToDateTime(real.EndTime);
                        if (endtime.Date == timenow.Date)
                        {
                            deviceStateInfo.WorkPieceCount++;
                        }
                    }
                    DateTime nowTime = DateTime.Now;
                    DateTime toDayTimeBegin = DateTime.Now.Date;
                    var states = wms.JDJS_WMS_Device_ProgState_Info.Where(r => (r.CncID == cncID) && (r.StartTime > toDayTimeBegin || r.EndTime > toDayTimeBegin || r.EndTime == null));
                    List<progState> progStates = new List<progState>();
                    foreach (var real in states)
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
                        progStates.Add(progState);
                    }
                    double runTime = 0;
                    double openTime = 0;
                    foreach (var real in progStates)
                    {
                        if (real.ProgState != -1)
                        {
                            openTime += Convert.ToDouble((real.EndTime - real.startTime).Minutes);
                        }
                        if (real.ProgState == 1)
                        {
                            runTime += Convert.ToDouble((real.EndTime - real.startTime).Minutes);
                        }
                    }
                    double time = (nowTime - toDayTimeBegin).Minutes;
                    deviceStateInfo.OpenRate = (openTime / time).ToString("0.0000");
                    deviceStateInfo.RunRate = (runTime / time).ToString("0.0000");
                    deviceStateInfo.ProcessRate = (runTime / openTime).ToString("0.0000");
                    deviceStateInfo.OpenTime = openTime.ToString("0.0000");
                    deviceStateInfo.RunTime = runTime.ToString("0.0000");
                    deviceStateInfos.Add(deviceStateInfo);
                }
            }
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var json = serializer.Serialize(deviceStateInfos);
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
    public class DeviceStateInfo
    {
        /// <summary>
        /// 机床编号
        /// </summary>
        public string CncNum;
        /// <summary>
        /// 机床状态，-1关机，0停止，1运行，2暂停，3复位，4报警
        /// </summary>
        public int state;
        /// <summary>
        /// 当前加工任务
        /// </summary>
        public string CurrTask;
        /// <summary>
        /// 当前加工任务进度
        /// </summary>
        public string CurrTaskRate;
        /// <summary>
        /// 下个任务
        /// </summary>
        public string NextTask;
        /// <summary>
        /// 开机时长
        /// </summary>
        public string OpenTime;
        /// <summary>
        /// 运行时长
        /// </summary>
        public string RunTime;
        /// <summary>
        /// 工件计数
        /// </summary>
        public int WorkPieceCount;
        /// <summary>
        /// 开机率
        /// </summary>
        public string OpenRate;
        /// <summary>
        /// 运行率
        /// </summary>
        public string RunRate;
        /// <summary>
        /// 加工率
        /// </summary>
        public string ProcessRate;
        /// <summary>
        /// 故障报警
        /// </summary>
        public double alarm;
        /// <summary>
        /// 暂停等待
        /// </summary>
        public double stop;
        /// <summary>
        /// 运行
        /// </summary>
        public double run;
        /// <summary>
        /// 其他
        /// </summary>
        public double other;
    }
    //public class progState
    //{
    //    public int ProgState;
    //    public DateTime startTime;
    //    public DateTime EndTime;
    //}
}