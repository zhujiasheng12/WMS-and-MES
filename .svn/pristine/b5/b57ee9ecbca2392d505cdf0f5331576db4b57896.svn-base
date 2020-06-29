using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Kanban.设备监控
{
    /// <summary>
    /// 设备利用占比 的摘要说明
    /// </summary>
    public class 设备利用占比 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //设备利用占比
            Int32 workId = 0;
           if( Int32.TryParse(context.Request["workId"],out workId)){

            }else
            {
                return;
            }
          
            List<DeviceUseRate> deviceUseRates = new List<DeviceUseRate>();
            List<string> timeList = new List<string>();
            using (JDJS_WMS_DB_USEREntities  wms = new JDJS_WMS_DB_USEREntities ())
            {
                var realTime = wms.JDJS_WMS_Device_ProgState_Info;
                List<progState> progStates = new List<progState>();
                foreach (var item in realTime)
                {
                    progState state = new progState();
                    if (item.EndTime == null)
                    {
                        state.EndTime = DateTime.Now;
                    }
                    else
                    {
                        state.EndTime = Convert.ToDateTime(item.EndTime);
                    }
                    state.startTime = Convert.ToDateTime(item.StartTime);
                    state.ProgState = Convert.ToInt32(item.ProgState);
                    DateTime startTime = state.startTime;
                    DateTime endtime = state.startTime;
                    int day = 1;
                    while (startTime.Date != endtime.Date)
                    {
                        progState progState = new progState();
                        progState.ProgState = Convert.ToInt32(item.ProgState);
                        progState.startTime = state.startTime;
                        progState.EndTime = state.startTime.AddDays(1).Date.AddSeconds(-1);
                        progStates.Add(progState);
                        state.startTime = state.startTime.AddDays(1).Date;
                        startTime = state.startTime;
                        day++;
                    }
                    progState progStateover = new progState();
                    progStateover.startTime = state.startTime;
                    progStateover.EndTime = state.EndTime;
                    progStateover.ProgState = state.ProgState;
                    progStates.Add(progStateover);
                }
                progStates.OrderByDescending(r => r.startTime);
                foreach (var item in progStates)
                {
                    double time = (item.EndTime - item.startTime).TotalMinutes;
                    string timestr = item.startTime.Year.ToString() + "-" + item.startTime.Month.ToString() + "-" + item.startTime.Day.ToString();
                    if (timeList.Contains(timestr))
                    {
                        var rate = deviceUseRates.Where(r => r.time == timestr).FirstOrDefault();
                        switch (item.ProgState)
                        {
                            case -1:
                                rate.other += time;
                                break;
                            case 0:
                                rate.stop = +time;
                                break;
                            case 1:
                                rate.run += time;
                                break;
                            case 2:
                                rate.other += time;
                                break;
                            case 3:
                                rate.stop += time;
                                break;
                            case 4:
                                rate.alarm += time;

                                break;
                            default:
                                break;

                        }
                    }
                    else
                    {
                        timeList.Add(timestr);
                        DeviceUseRate deviceUseRate = new DeviceUseRate();
                        deviceUseRate.time = timestr;
                        switch (item.ProgState)
                        {
                            case -1:
                                deviceUseRate.other = time;
                                break;
                            case 0:
                                deviceUseRate.stop = time;
                                break;
                            case 1:
                                deviceUseRate.run = time;
                                break;
                            case 2:
                                deviceUseRate.other = time;
                                break;
                            case 3:
                                deviceUseRate.stop = time;
                                break;
                            case 4:
                                deviceUseRate.alarm = time;

                                break;
                            default:
                                break;

                        }
                        deviceUseRates.Add(deviceUseRate);

                    }
                }
            }
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var json = serializer.Serialize(deviceUseRates);
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
    public class progState
    {
        public int ProgState;
        public DateTime startTime;
        public DateTime EndTime;
    }
    public class DeviceUseRate
    {
        public string time;
        public double alarm;
        public double stop;
        public double run;
        public double other;

    }
}