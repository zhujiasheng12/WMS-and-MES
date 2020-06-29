using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.资材部
{
    /// <summary>
    /// 毛坯预计完成时间提交 的摘要说明
    /// </summary>
    public class 毛坯预计完成时间提交 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var orderId = int.Parse(context.Request["orderId"]);
            var time = Convert.ToDateTime(context.Request["time"]);
            using (JDJS_WMS_DB_USEREntities wms = new JDJS_WMS_DB_USEREntities())
            {
                using (System.Data.Entity.DbContextTransaction mytran = wms.Database.BeginTransaction())
                {
                    try
                    {

                        {
                            int OrderID = orderId;
                            var delayInfo = wms.JDJS_WMS_Order_Blank_Table.Where(r => r.OrderID == OrderID);
                          
                            {
                                foreach (var item in delayInfo)
                                {
                                    item.Expected_Completion_Time= time;
                                }
                                wms.SaveChanges();
                            }
                        }
                        wms.SaveChanges();
                        mytran.Commit();
                        context.Response.Write("ok");
                    }
                    catch (Exception ex)
                    {
                        mytran.Rollback();
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