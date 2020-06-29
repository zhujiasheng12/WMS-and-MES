using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.资材部
{
    /// <summary>
    /// 治具延期提交 的摘要说明
    /// </summary>
    public class 治具延期提交 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var processId = int.Parse(context.Request["processId"]);
            var time =Convert.ToDateTime( context.Request["time"]);
            using (JDJS_WMS_DB_USEREntities wms = new JDJS_WMS_DB_USEREntities())
            {
                using (System.Data.Entity.DbContextTransaction mytran = wms.Database.BeginTransaction())
                {
                    try
                    {
                        var processInfo = wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == processId).FirstOrDefault();
                        if (processInfo != null)
                        {
                            int OrderID =Convert.ToInt32( processInfo.OrderID);
                            var delayInfo = wms.JDJS_WMS_Order_DelayTime_Table.Where(r => r.OrderID == OrderID);
                            if (delayInfo.Count() < 1)
                            {
                                JDJS_WMS_Order_DelayTime_Table delay = new JDJS_WMS_Order_DelayTime_Table()
                                {
                                    OrderID =OrderID ,
                                    Fixture =time,
                                };
                                wms.JDJS_WMS_Order_DelayTime_Table.Add(delay);
                                wms.SaveChanges();
                            }
                            else
                            {
                                foreach (var item in delayInfo)
                                {
                                    item.Fixture = time;
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