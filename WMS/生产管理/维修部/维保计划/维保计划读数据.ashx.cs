using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.维修部.维保计划
{
    /// <summary>
    /// 维保计划读数据 的摘要说明
    /// </summary>
    public class 维保计划读数据 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
           
            {
                int CncID = 33;
                List<MaintenanceRead> maintenanceReads = new List<MaintenanceRead>();
                using (JDJS_WMS_DB_USEREntities wms = new JDJS_WMS_DB_USEREntities())
                {



                        var mainOn = wms.JDJS_WMS_Maintenance_Plan_Table.Where(r =>  r.isFlag == 1);
                        foreach (var item in mainOn)
                        {
                            MaintenanceRead maintenanceRead = new MaintenanceRead();
                            maintenanceRead.MaintenacneID = item.ID;
                            maintenanceRead.CncID = item.CncID.ToString();
                            var cnc = wms.JDJS_WMS_Device_Info.Where(r => r.ID == item.CncID);
                            maintenanceRead.CncNum = cnc.First().MachNum;
                            maintenanceRead.MaintenanceContence = item.MaintenanceContence.ToString();
                            maintenanceRead.MaintenanceCycle = item.Cycle.ToString();
                            maintenanceRead.MaintenanceEnable = Convert.ToInt32(item.isFlag);
                            maintenanceRead.MaintenanceTime = item.MaintenanceTime.ToString();
                            maintenanceRead.MaintenanceGuide = "";
                            maintenanceRead.MaintenacneCycleStr = item.TimeStr;
                            maintenanceRead.NextMaintenTime = Convert.ToDateTime(item.MaintenanceTime).AddDays(Convert.ToDouble(item.Cycle)).ToString();
                            maintenanceRead.days = (Convert.ToDateTime(item.MaintenanceTime).AddDays(Convert.ToDouble(item.Cycle)) - DateTime.Now).Days.ToString()+"天";
                            maintenanceReads.Add(maintenanceRead);
                        }
                        DateTime time = DateTime.Now.AddYears(-1);
                        foreach (var item in maintenanceReads)
                        {
                            if (Convert.ToDateTime(item.MaintenanceTime).AddDays(Convert.ToDouble(item.MaintenanceCycle)) > time)
                            {
                                time = Convert.ToDateTime(item.MaintenanceTime).AddDays(Convert.ToDouble(item.MaintenanceCycle));
                            }
                        }

                        foreach (var item in maintenanceReads)
                        {
                            double guide = 1 - ((Convert.ToDateTime(item.MaintenanceTime).AddDays(Convert.ToDouble(item.MaintenanceCycle)) - DateTime.Now).TotalMilliseconds / (time - DateTime.Now).TotalMilliseconds);
                            item.MaintenanceGuide = guide.ToString("0.000000");
                        }
                    

                    maintenanceReads = maintenanceReads.OrderByDescending(r => r.MaintenanceGuide).ToList();



                    var mainOff = wms.JDJS_WMS_Maintenance_Plan_Table.Where(r =>r.isFlag == 0);
                    foreach (var item in mainOff)
                    {
                        MaintenanceRead maintenanceRead = new MaintenanceRead();
                        maintenanceRead.MaintenacneID = item.ID;
                        maintenanceRead.CncID =item.CncID.ToString();
                        var cnc = wms.JDJS_WMS_Device_Info.Where(r => r.ID == item.CncID);
                        maintenanceRead.CncNum = cnc.First().MachNum;
                        maintenanceRead.MaintenanceContence = item.MaintenanceContence.ToString();
                        maintenanceRead.MaintenanceCycle = item.Cycle.ToString();
                        maintenanceRead.MaintenanceEnable = Convert.ToInt32(item.isFlag);
                        maintenanceRead.MaintenanceTime = item.MaintenanceTime.ToString();
                        maintenanceRead.MaintenanceGuide = "";
                        maintenanceRead.MaintenacneCycleStr = item.TimeStr;
                        maintenanceRead.NextMaintenTime = "";
                        maintenanceRead.days = "";
                        maintenanceReads.Add(maintenanceRead);
                    }

                }
                var page = int.Parse(context.Request["page"]);
                var limit = int.Parse(context.Request["limit"]);
                var layPage = maintenanceReads.Skip((page - 1) * limit).Take(limit);
                var model = new { code = 0, data = layPage,count=maintenanceReads.Count };
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
    public class MaintenanceRead
    {
        /// <summary>
        /// 机床主键ID
        /// </summary>
        public string CncID;
        /// <summary>
        /// 机床编号
        /// </summary>
        public string CncNum;
        /// <summary>
        /// 维保内容
        /// </summary>
        public string MaintenanceContence;
        /// <summary>
        /// 维保周期
        /// </summary>
        public string MaintenanceCycle;
        /// <summary>
        /// 维保周期
        /// </summary>
        public string MaintenacneCycleStr;
        /// <summary>
        /// 上一次维保时间
        /// </summary>
        public string MaintenanceTime;
        public string days;
        public string NextMaintenTime;
        /// <summary>
        /// 维保指导，上一次维保日期加周期距离当前时间的距离
        /// </summary>
        public string MaintenanceGuide;
        /// <summary>
        /// 维保使能，1代表有效，0代表暂停
        /// </summary>
        public int MaintenanceEnable;
        /// <summary>
        /// 维保计划表ID
        /// </summary>
        public int MaintenacneID;


    }
}