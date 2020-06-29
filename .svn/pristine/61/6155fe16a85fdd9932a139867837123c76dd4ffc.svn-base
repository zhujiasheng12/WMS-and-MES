using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.工程部
{
    /// <summary>
    /// 毛坯确认 的摘要说明
    /// </summary>
    public class 毛坯确认 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var orderNumberId = int.Parse(context.Request["orderId"]);

            using (JDJS_WMS_DB_USEREntities entities = new JDJS_WMS_DB_USEREntities())
            {

                using (System.Data.Entity.DbContextTransaction mytran = entities.Database.BeginTransaction())
                {
                    try
                    {


                        {
                            var blankInfo = entities.JDJS_WMS_Order_Blank_Table.Where(r => r.OrderID == orderNumberId);
                            if (blankInfo.Count() < 1)
                            {
                                context.Response.Write("请先创建毛坯信息");
                                return;
                            }
                            foreach (var item in blankInfo)
                            {
                                if (item.BlankSpecification.Contains("#1#"))
                                {
                                    item.BlankSpecification = item.BlankSpecification.Replace("#1#", "");
                                }
                            }
                        }
                        entities.SaveChanges();

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


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

    }
}