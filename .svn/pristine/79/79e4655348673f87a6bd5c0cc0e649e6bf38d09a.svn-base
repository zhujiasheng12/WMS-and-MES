using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.市场部
{
    /// <summary>
    /// 审核不通过确认 的摘要说明
    /// </summary>
    public class 审核不通过确认 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                int orderId = int.Parse(context.Request["orderId"]);//订单ID
                string str = "";
                using (JDJS_WMS_DB_USEREntities model = new JDJS_WMS_DB_USEREntities())
                {
                    var order = model.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == orderId && r.AuditResult == "审核不通过" && (r.Intention == 2 || r.Intention == 3)).FirstOrDefault();
                    if (order == null)
                    {
                        str = "该订单不存在或者该订单暂不符合审核条件！";
                    }
                    else
                    {
                        using (System.Data.Entity.DbContextTransaction mytran = model.Database.BeginTransaction())
                        {
                            try
                            {
                                order.AuditResult = "待审核";
                                str = "ok";
                                model.SaveChanges();
                                mytran.Commit();
                            }
                            catch (Exception ex)
                            {
                                str = ex.Message;
                                mytran.Rollback();
                            }
                        }
                    }
                }
                context.Response.Write(str);
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