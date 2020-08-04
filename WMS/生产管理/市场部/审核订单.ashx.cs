using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.市场部
{
    /// <summary>
    /// 审核订单 的摘要说明
    /// </summary>
    public class 审核订单 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                int orderId = int.Parse(context.Request["orderId"]);//订单ID
                string result = context.Request["result"];//审核结果
                string advice = context.Request["advice"];//审核说明
                advice = DateTime.Now.ToString()+"：" + advice+"\n";
                string str = "";
                using (JDJS_WMS_DB_USEREntities model = new JDJS_WMS_DB_USEREntities())
                {
                    var order = model.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == orderId && r.AuditResult == "待审核" && (r.Intention == 2 || r.Intention == 3 || r.Intention == 6) ).FirstOrDefault();
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
                                order.AuditResult = result;
                                string adviceStr = "";
                                if (order.AuditAdvice != null)
                                {
                                    adviceStr = order.AuditAdvice;
                                }
                                order.AuditAdvice = adviceStr + advice;
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