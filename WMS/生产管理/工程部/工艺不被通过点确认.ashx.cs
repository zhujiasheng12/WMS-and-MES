using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.工程部
{
    /// <summary>
    /// 工艺不被通过点确认 的摘要说明
    /// </summary>
    public class 工艺不被通过点确认 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var orderId = int.Parse(context.Request["orderId"]);
            var type = context.Request["type"];
            using (JDJS_WMS_DB_USEREntities entities = new JDJS_WMS_DB_USEREntities())
            {
                using (System.Data.Entity.DbContextTransaction mytran = entities.Database.BeginTransaction())
                {
                    try
                    {
                        var order = entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == orderId).FirstOrDefault();

                        if (type=="工艺审核") {
                            if (order != null)
                            {


                                var processes = entities.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == orderId && r.sign == -3);
                                if (processes.Count() < 1)
                                {
                                    context.Response.Write("该订单暂无审核未通过工序");
                                    return;
                                }
                                foreach (var item in processes)
                                {
                                    item.sign = -1;
                                }
                            }
                            else
                            {
                                context.Response.Write("该订单不存在");
                                return;
                            }
                        } else if (type == "编程审核") {
                            if (order != null)
                            {


                                var processes = entities.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == orderId && r.program_audit_sign == -3);
                                if (processes.Count() < 1)
                                {
                                    context.Response.Write("该订单暂无审核未通过工序");
                                    return;
                                }
                                foreach (var item in processes)
                                {
                                    item.program_audit_sign = -1;
                                }
                            }
                            else
                            {
                                context.Response.Write("该订单不存在");
                                return;
                            }
                        }
                       
                        entities.SaveChanges();
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