using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.资材部.夹具管理
{
    /// <summary>
    /// 修改设计材料与备注信息 的摘要说明
    /// </summary>
    public class 修改设计材料与备注信息 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                int fxDemandId = int.Parse(context.Request["fxDemandId"]);
                string material = context.Request["material"];
                string remark = context.Request["reamrk"];
                using (JDJS_WMS_DB_USEREntities model = new JDJS_WMS_DB_USEREntities())
                {
                    var demand = model.JDJS_WMS_Fixture_Manage_Demand_Table.Where(r => r.ID == fxDemandId).FirstOrDefault();
                    if (demand == null)
                    {
                        context.Response.Write("该治具需求不存在");
                        return;
                    }
                    if (demand.State !="设计中")
                    {
                        context.Response.Write("该治具设计需求暂不支持修改");
                        return;
                    }
                    using (System.Data.Entity.DbContextTransaction mytran = model.Database.BeginTransaction())
                    {
                        try
                        {
                            demand.Material = material;
                            demand.Remark = remark;
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