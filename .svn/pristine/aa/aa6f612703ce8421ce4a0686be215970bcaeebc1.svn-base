using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.资材部.夹具管理
{
    /// <summary>
    /// 填写治具设计需求设计计划完成时间 的摘要说明
    /// </summary>
    public class 填写治具设计需求设计计划完成时间 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                int fxDemandId = int.Parse(context.Request["fxDemandId"]);
                DateTime time = DateTime.Parse(context.Request["time"]);
                using (JDJS_WMS_DB_USEREntities model = new JDJS_WMS_DB_USEREntities())
                {
                    var demand = model.JDJS_WMS_Fixture_Manage_Demand_Table.Where(r => r.ID == fxDemandId).FirstOrDefault();
                    if (demand == null)
                    {
                        context.Response.Write("该治具需求不存在");
                        return;
                    }
                    
                    using (System.Data.Entity.DbContextTransaction mytran = model.Database.BeginTransaction())
                    {
                        try
                        {
                            demand.PlanEndTime = time;
                            model.SaveChanges();
                            mytran.Commit();
                            context.Response.Write("ok");
                            return;
                        }
                        catch (Exception ex)
                        {
                            mytran.Rollback();
                            context.Response.Write(ex.Message);
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                context.Response.Write(ex.Message);
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
}