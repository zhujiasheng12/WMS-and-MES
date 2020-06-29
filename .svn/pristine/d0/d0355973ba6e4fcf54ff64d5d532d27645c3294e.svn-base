using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.品质
{
    /// <summary>
    /// 手动录入删除 的摘要说明
    /// </summary>
    public class 手动录入删除 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            using (JDJS_WMS_DB_USEREntities entities = new JDJS_WMS_DB_USEREntities())
            {
                using (System.Data.Entity.DbContextTransaction db = entities.Database.BeginTransaction())
                {
                    try
                    {


                        var id = int.Parse(context.Request["id"]);
                        var row = entities.JDJS_WMS_Quality_ManualInput_Measurement_Table.Where(r => r.ID == id);
                        if (row.Count() > 0)
                        {
                            entities.JDJS_WMS_Quality_ManualInput_Measurement_Table.Remove(row.FirstOrDefault());
                            entities.SaveChanges();
                            db.Commit();
                            context.Response.Write("ok");
                        }
                    }
                    catch (Exception ex)
                    {
                        db.Rollback();
                        context.Response.Write(ex.Message);
                    }
                }
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