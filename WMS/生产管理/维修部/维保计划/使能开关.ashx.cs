using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.维修部.维保计划
{
    /// <summary>
    /// 使能开关 的摘要说明
    /// </summary>
    public class 使能开关 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            int PlanId = int.Parse(context.Request["MaintenacneID"]);
            int enable = int.Parse(context.Request["MaintenanceEnable"]);
            using (JDJS_WMS_DB_USEREntities  wms = new JDJS_WMS_DB_USEREntities ())
            {
                using (System.Data.Entity.DbContextTransaction mytran = wms.Database.BeginTransaction())
                {
                    try
                    {
                        var plan = wms.JDJS_WMS_Maintenance_Plan_Table.Where(r => r.ID == PlanId);
                        foreach (var item in plan)
                        {
                            item.isFlag = enable;
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
}