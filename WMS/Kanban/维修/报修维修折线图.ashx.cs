using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Kanban
{
    /// <summary>
    /// 报修维修柱状图 的摘要说明
    /// </summary>
    public class 报修维修柱状图 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            List<string> time = new List<string>();

            List<RepairInfo> repairInfos = new List<RepairInfo>();
            using (JDJS_WMS_DB_USEREntities  wms = new JDJS_WMS_DB_USEREntities ())
            {
                var repairs = wms.JDJS_WMS_Device_Alarm_Repair.ToList();
                foreach (var item in repairs)
                {
                    if (item.RepairTime != null)
                    {
                        DateTime repair = Convert.ToDateTime(item.RepairTime);
                        string repairTime = repair.Year.ToString() + "-" + repair.Month.ToString() + "-" + repair.Day.ToString();
                        if (!(time.Contains(repairTime)))
                        {
                            time.Add(repairTime);
                            RepairInfo repairInfo = new RepairInfo();
                            repairInfo.date = repairTime;
                            repairInfo.repairNum++;
                            repairInfos.Add(repairInfo);
                        }
                        else
                        {
                            var repairinfo = repairInfos.Where(r => r.date == repairTime).FirstOrDefault();
                            repairinfo.repairNum++;
                        }
                    }

                    if (item.StartTime != null)
                    {
                        DateTime repair = Convert.ToDateTime(item.StartTime);
                        string repairTime = repair.Year.ToString() + "-" + repair.Month.ToString() + "-" + repair.Day.ToString();
                        if (!(time.Contains(repairTime)))
                        {
                            time.Add(repairTime);
                            RepairInfo repairInfo = new RepairInfo();
                            repairInfo.date = repairTime;
                            repairInfo.fixNum++;
                            repairInfos.Add(repairInfo);
                        }
                        else
                        {
                            var repairinfo = repairInfos.Where(r => r.date == repairTime).FirstOrDefault();
                            repairinfo.fixNum++;
                        }
                    }

                }

            }
           
            var json = serializer.Serialize(repairInfos);
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
    public class RepairInfo
    {
        public string date;
        public int repairNum = 0;
        public int fixNum = 0;
    }
}