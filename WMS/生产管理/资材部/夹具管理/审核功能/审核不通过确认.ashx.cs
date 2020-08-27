using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
namespace WebApplication2.生产管理.资材部.夹具管理.审核功能
{
    /// <summary>
    /// 审核不通过确认 的摘要说明
    /// </summary>
    public class 审核不通过确认 : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                int fxDemandId = int.Parse(context.Request["fxDemandId"]);
                int personId = int.Parse(context.Session["id"].ToString());
                using (JDJS_WMS_DB_USEREntities model = new JDJS_WMS_DB_USEREntities())
                {
                    var demand = model.JDJS_WMS_Fixture_Manage_Demand_Table.Where(r => r.ID == fxDemandId).FirstOrDefault();
                    if (demand == null)
                    {
                        context.Response.Write("该治具需求不存在");
                        return;
                    }
                    if (demand.DesignPersonId != personId)
                    {
                        context.Response.Write("当前用户无此权限！");
                        return;
                    }
                    if (demand.State != "审核不通过")
                    {
                        context.Response.Write("该治具设计需求暂不支持审核不通过确认");
                        return;
                    }
                    using (System.Data.Entity.DbContextTransaction mytran = model.Database.BeginTransaction())
                    {
                        try
                        {
                            demand.State = "设计中";
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