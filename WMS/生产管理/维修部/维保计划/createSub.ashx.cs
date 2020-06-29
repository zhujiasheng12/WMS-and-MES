using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.维修部.维保计划
{
    /// <summary>
    /// createSub 的摘要说明
    /// </summary>
    public class createSub : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var data = context.Request["data"];
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var obj = serializer.Deserialize<List>(data);

            int CncID = int.Parse(obj.cncId);
            string maintenanceContent = obj.content;
            DateTime maintrenanceTime = Convert.ToDateTime(obj.time);
            double cycleNum =Convert.ToDouble(obj.cycle);
            string timeUnit =obj.dateFormat;



            double cycle = 0;
            if (timeUnit == "日")
            {
                cycle = cycleNum;
            }
            else if (timeUnit == "月")
            {
                cycle = cycleNum * 30;
            }
            else if (timeUnit == "年")
            {
                cycle = cycleNum * 365;
            }
            else if(timeUnit =="周")
            {
                cycle = cycleNum * 7;
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
                        JDJS_WMS_Maintenance_Plan_Table plan = new JDJS_WMS_Maintenance_Plan_Table()
                        {
                            CncID = CncID,
                            MaintenanceContence = maintenanceContent,
                            MaintenanceTime = maintrenanceTime,
                            Cycle = cycle,
                            isFlag = 1,
                            TimeStr = cycleNum.ToString ()+timeUnit 
                        };
                        wms.JDJS_WMS_Maintenance_Plan_Table.Add(plan);
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
    class List
    {
        public string cncId;
        public string content;
        public string time;
        public string cycle;
        public string dateFormat;
    }
}