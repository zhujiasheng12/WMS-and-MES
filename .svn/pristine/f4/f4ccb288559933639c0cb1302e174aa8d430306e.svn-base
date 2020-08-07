using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.品质.品质检测手动处理
{
    /// <summary>
    /// 批量删除尺寸 的摘要说明
    /// </summary>
    public class 批量删除尺寸 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                var ids = context.Request["ids"];//需要删除的尺寸主键ID列表，以逗号,分隔
                List<string> idStrList = ids.Split(',').ToList();
                List<int> idList = new List<int>();
                foreach (var item in idStrList)
                {
                    int value = 0;
                    value = int.TryParse(item, out value) ? value : 0;
                    idList.Add(value);
                }
                using (JDJS_WMS_DB_USEREntities model = new JDJS_WMS_DB_USEREntities())
                {
                    using (System.Data.Entity.DbContextTransaction mytran = model.Database.BeginTransaction())
                    {
                        try 
                        {
                            foreach (var item in idList)
                            {
                                var quality = model.JDJS_WMS_Quality_Detection_Measurement_Table.Where(r => r.ID == item);
                                foreach (var real in quality)
                                {
                                    model.JDJS_WMS_Quality_Detection_Measurement_Table.Remove(real);
                                }
                            }
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
            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World");
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