using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Kanban
{
    /// <summary>
    /// 报修维修表格 的摘要说明
    /// </summary>
    public class 报修维修表格 : IHttpHandler
    {
        System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

        public void ProcessRequest(HttpContext context)
        {
            //读取报修维修信息
            List<RepairInfoRead> repairInfoReads = new List<RepairInfoRead>();
            using (JDJS_WMS_DB_USEREntities wms = new  JDJS_WMS_DB_USEREntities ())
            {
                var repairs = wms.JDJS_WMS_Device_Alarm_Repair.ToList();
                foreach (var item in repairs)
                {
                    RepairInfoRead repairInfoRead = new RepairInfoRead();
                    repairInfoRead.CncNum = item.CncNum;
                    int alarmDesc = Convert.ToInt32(item.AlarmDescID);
                    var alarm = wms.JDJS_WMS_Device_Alarm_Description.Where(r => r.ID == alarmDesc).FirstOrDefault();
                    repairInfoRead.RepairDesc = alarm.Description;
                    repairInfoRead.RepairTime = item.RepairTime.ToString();
                    repairInfoRead.FixStaff = item.MaintenanceStaff;
                    repairInfoRead.Receptiontime = item.Receptiontime.ToString();
                    if (item.StartTime != null)
                    {
                        DateTime beginTime = Convert.ToDateTime(item.RepairTime);
                        DateTime endTime = Convert.ToDateTime(item.StartTime);
                        var cncState = wms.JDJS_WMS_Device_ProgState_Info.Where(r => r.CncID == item.CncID && r.EndTime < endTime).ToArray();
                        if (cncState.Count() < 1)
                        {
                            repairInfoRead.StopTime = "";
                        }
                        else
                        {
                            var cnc = cncState.OrderByDescending(r => r.EndTime).ToList();
                            if (cnc.FirstOrDefault().EndTime == null)
                            {
                                repairInfoRead.StopTime = "";
                            }
                            else
                            {
                                repairInfoRead.StopTime = cnc.FirstOrDefault().EndTime.ToString();
                            }
                        }
                    }
                    else
                    {
                        repairInfoRead.StopTime = "";
                    }
                    repairInfoRead.StartTime = item.StartTime.ToString();
                    var state = Convert.ToInt32(item.AlarmStateID);
                    var stateinfo = wms.JDJS_WMS_Device_Maintenance_Status.Where(r => r.ID == state).FirstOrDefault();

                    repairInfoRead.State = stateinfo.Description;
                    repairInfoRead.EndTime = item.EndTime.ToString();
                    repairInfoReads.Add(repairInfoRead);
                }
            }
            var model = new { code = 0, data = repairInfoReads };
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

    public class RepairInfoRead
    {
        /// <summary>
        /// 机床编号
        /// </summary>
        public string CncNum;
        /// <summary>
        /// 报修内容
        /// </summary>
        public string RepairDesc;
        /// <summary>
        /// 报修时间
        /// </summary>
        public string RepairTime;
        /// <summary>
        /// 维修人
        /// </summary>
        public string FixStaff;
        /// <summary>
        /// 停机时间
        /// </summary>
        public string StopTime;
        /// <summary>
        /// 接单时间
        /// </summary>
        public string Receptiontime;
        /// <summary>
        /// 开始时间
        /// </summary>
        public string StartTime;
        /// <summary>
        /// 维修状态
        /// </summary>
        public string State;
        /// <summary>
        /// 完成日期
        /// </summary>
        public string EndTime;
    }
}