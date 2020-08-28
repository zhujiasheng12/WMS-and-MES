using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.生产管理.资材部.夹具管理.特殊治具管理Access数据库;

namespace WebApplication2.生产管理.资材部.夹具管理.治具种类管理
{
    /// <summary>
    /// 删除治具种类 的摘要说明
    /// </summary>
    public class 删除治具种类 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                int id = int.Parse(context.Request["id"]);
                using (FixtureModel model = new FixtureModel())
                {
                    var fx = model.JDJS_WMS_Fixture_Type_Table.Where(r => r.Id == id).FirstOrDefault();
                    if (fx == null)
                    {
                        context.Response.Write("该治具种类不存在！");
                        return;
                    }
                    using (System.Data.Entity.DbContextTransaction mytran = model.Database.BeginTransaction())
                    {
                        try
                        {
                            
                            model.JDJS_WMS_Fixture_Type_Table.Remove(fx);
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
                return;
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