using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.资材部
{
    /// <summary>
    /// 毛坯延期提交 的摘要说明
    /// </summary>
    public class 毛坯延期提交 : IHttpHandler
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
                                var delayInfo = wms.JDJS_WMS_Order_DelayTime_Table.Where(r => r.OrderID == OrderID);
                                if (delayInfo.Count() < 1)
                                {
                                    JDJS_WMS_Order_DelayTime_Table delay = new JDJS_WMS_Order_DelayTime_Table()
                                    {
                                        OrderID = OrderID,
                                        BlankTime = time,
                                    };
                                    wms.JDJS_WMS_Order_DelayTime_Table.Add(delay);
                                    wms.SaveChanges();
                                }
                                else
                                {
                                    foreach (var item in delayInfo)
                                    {
                                        item.BlankTime = time;
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