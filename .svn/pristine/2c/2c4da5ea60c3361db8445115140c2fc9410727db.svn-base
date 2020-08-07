using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.品质.送检模块ashx
{
    /// <summary>
    /// 删除送检申请 的摘要说明
    /// </summary>
    public class 删除送检申请 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                int id = int.Parse(context.Request["id"]);//处理的申请主键ID
                using (JDJS_WMS_DB_USEREntities model = new JDJS_WMS_DB_USEREntities())
                {
                    var inspect = model.JDJS_WMS_Quality_Apply_Measure_Table.Where(r => r.ID == id).FirstOrDefault();
                    if (inspect == null)
                    {
                        context.Response.Write("该送检申请不存在，请确认后再试！");
                        return;
                    }
                    //if (inspect.State != Enum.GetName(typeof(InspectStateType), 1))
                    //{
                    //    context.Response.Write("该测量记录状态不符，请确认后再试！");
                    //    return;
                    //}
                    using (System.Data.Entity.DbContextTransaction mytran = model.Database.BeginTransaction())
                    {
                        try
                        {
                            model.JDJS_WMS_Quality_Apply_Measure_Table.Remove(inspect);
                            model.SaveChanges();
                            mytran.Commit();
                            context.Response.Write("ok");
                            return;
                        }
                        catch (Exception ex)
                        {
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