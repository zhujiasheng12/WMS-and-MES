using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.维修部.维保计划
{
    /// <summary>
    /// editSub 的摘要说明
    /// </summary>
    public class editSub : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var data = context.Request["data"];
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var da = serializer.Deserialize<read>(data);

            int PlanID =da.id ;
            int CncID = da.cncId ;
            string maintenanceContent = da.content  ;
            DateTime maintrenanceTime = da.time  ;
            double cycleNum = da.cycle ;
            string timeUnit = da.dateFormat;



            double cycle = 0;
            if (timeUnit == "日")
            {
                cycle = cycleNum;
            }
            else if (timeUnit == "月")
            {
                cycle = cycleNum * 30;
            }
            else if (timeUnit == "周")
            {
                cycle = cycleNum * 7;
            }
            else if (timeUnit == "年")
            {
                cycle = cycleNum * 365;
            }
            else
            {
                //加一句如果输入的时间单位不是年月天就返回提醒用户
            }
            using (JDJS_WMS_DB_USEREntities  wms = new JDJS_WMS_DB_USEREntities ())
            {
                using (System.Data.Entity.DbContextTransaction mytran = wms.Database.BeginTransaction())
                {
                    try
                    {
                        var plan = wms.JDJS_WMS_Maintenance_Plan_Table.Where(r => r.ID == PlanID);
                        foreach (var item in plan)
                        {
                            item.CncID = CncID;
                            item.Cycle = cycle;
                            item.MaintenanceContence = maintenanceContent;
                            item.MaintenanceTime = maintrenanceTime;
                            item.TimeStr = cycleNum.ToString() + timeUnit;

                        }
                        wms.SaveChanges();
                        mytran.Commit();
                    }
                    catch
                    {
                        mytran.Rollback();
                    }
                }
            }
            context.Response.Write("ok");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }

    public class read
    {
        public int id;
        public int cncId;
        public string content;
        public DateTime time;
        public double cycle;
        public string dateFormat;
    }
}