﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Kanban.设备监控
{
    /// <summary>
    /// 稼动率 的摘要说明
    /// </summary>
    public class 稼动率 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            int workId=0;
           
            if(Int32.TryParse(context.Request["workId"], out workId))
            {
                {
                    int LocationID = workId;
                    double TimeC = 0;
                    double xingnnegC = 0;
                    {
                        //稼动率评估

                        DateTime nowTime = DateTime.Now;
                        DateTime toDayTimeBegin = DateTime.Now.AddDays(-1);

                        double runTime = 0;
                        double openTime = 0;
                        double lilunTime = 0;
                        double shijiTime = 0;
                        using (JDJS_WMS_DB_USEREntities wms = new JDJS_WMS_DB_USEREntities())
                        {

                            var works = wms.JDJS_WMS_Location_Info.ToList();
                            var devices = wms.JDJS_WMS_Device_Info.ToList();
                            List<WebApplication2.Kanban.现场.CncRead> objs = new List<WebApplication2.Kanban.现场.CncRead>();
                            List<int> workIds = new List<int>();
                            WebApplication2.Kanban.现场.机台状态 funs = new 现场.机台状态();
                            var Devices = funs.fun(workId, works, devices, objs, workIds);

                            //var Devices = wms.JDJS_WMS_Device_Info.Where(r => r.Position == LocationID);
                            foreach (var item in Devices)
                            {

                                int cncID =Convert.ToInt32( item.ID);
                                var states = wms.JDJS_WMS_Device_ProgState_Info.Where(r => r.CncID == cncID && (r.StartTime > toDayTimeBegin || r.EndTime > toDayTimeBegin || r.EndTime == null));
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

                                foreach (var real in progStates)
                                {
                                    if (real.ProgState != -1)
                                    {
                                        openTime += Convert.ToDouble((real.EndTime - real.startTime).TotalMinutes);
                                    }
                                    if (real.ProgState == 1)
                                    {
                                        runTime += Convert.ToDouble((real.EndTime - real.startTime).TotalMinutes);
                                    }
                                }
                            }
                            if (openTime != 0)
                            {
                                TimeC = runTime / openTime;
                            }
                            else
                            {
                                TimeC = 0;
                            }
                            foreach (var item in Devices)
                            {
                                int cncID =Convert.ToInt32( item.ID);

                                var processes = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.CncID == cncID && r.EndTime > toDayTimeBegin && r.isFlag == 3);
                                foreach (var real in processes)
                                {
                                    int processID = Convert.ToInt32(real.ProcessID);
                                    var processInfo = wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == processID).FirstOrDefault();
                                    double processTime = Convert.ToDouble(processInfo.ProcessTime);
                                    lilunTime += processTime;
                                    double processTimeLilun = (Convert.ToDateTime(real.EndTime) - Convert.ToDateTime(real.StartTime)).TotalMinutes;
                                    shijiTime += processTimeLilun;

                                }
                            }
                            if (shijiTime == 0)
                            {

                                xingnnegC = 0;
                            }
                            else
                            {
                                xingnnegC = lilunTime / shijiTime;
                            }

                        }

                    }

                    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var model = new { TimeC = TimeC, xingnnegC = xingnnegC };
                    var json = serializer.Serialize(model);
                
                    context.Response.Write("data:" + json + "\n\n");
                    context.Response.ContentType = "text/event-stream";

                }
            }
            else
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
    //public class DeviceStateInfo
    //{
    //    /// <summary>
    //    /// 机床编号
    //    /// </summary>
    //    public string CncNum;
    //    /// <summary>
    //    /// 机床状态，-1关机，0停止，1运行，2暂停，3复位，4报警
    //    /// </summary>
    //    public int state;
    //    /// <summary>
    //    /// 当前加工任务
    //    /// </summary>
    //    public string CurrTask;
    //    /// <summary>
    //    /// 当前加工任务进度
    //    /// </summary>
    //    public string CurrTaskRate;
    //    /// <summary>
    //    /// 下个任务
    //    /// </summary>
    //    public string NextTask;
    //    /// <summary>
    //    /// 开机时长
    //    /// </summary>
    //    public string OpenTime;
    //    /// <summary>
    //    /// 运行时长
    //    /// </summary>
    //    public string RunTime;
    //    /// <summary>
    //    /// 工件计数
    //    /// </summary>
    //    public int WorkPieceCount;
    //    /// <summary>
    //    /// 开机率
    //    /// </summary>
    //    public string OpenRate;
    //    /// <summary>
    //    /// 运行率
    //    /// </summary>
    //    public string RunRate;
    //    /// <summary>
    //    /// 加工率
    //    /// </summary>
    //    public string ProcessRate;
    //    /// <summary>
    //    /// 故障报警
    //    /// </summary>
    //    public double alarm;
    //    /// <summary>
    //    /// 暂停等待
    //    /// </summary>
    //    public double stop;
    //    /// <summary>
    //    /// 运行
    //    /// </summary>
    //    public double run;
    //    /// <summary>
    //    /// 其他
    //    /// </summary>
    //    public double other;
    //}
    //public class progState
    //{
    //    public int ProgState;
    //    public DateTime startTime;
    //    public DateTime EndTime;
    //}
}