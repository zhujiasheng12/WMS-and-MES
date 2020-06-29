using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.工程部
{
    /// <summary>
    /// 毛坯信息 的摘要说明
    /// </summary>
    public class 毛坯信息 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var form = context.Request.Form;
           
            var orderNumberId = int.Parse(form[0]);
            var workNumber = 1;
               var blankType = int.Parse(form[1]);
            var blankSpecification = form[2];
            using (JDJS_WMS_DB_USEREntities entities = new JDJS_WMS_DB_USEREntities())
            {

                using (System.Data.Entity.DbContextTransaction mytran = entities.Database.BeginTransaction())
                {
                    try
                    {
                       
                        
                        {
                            var orderNumber = entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == orderNumberId).First().Order_Number;
                            var BlankNumber = entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == orderNumberId).First().Product_Output;
                            var judge = entities.JDJS_WMS_Order_Blank_Table.Where(r => r.OrderID == orderNumberId);
                            if (judge.Count() == 0)
                            {
                                var newRow = new JDJS_WMS_Order_Blank_Table
                                {
                                    OrderID = orderNumberId,
                                    BlankSpecification = blankSpecification + "#1#",
                                    BlankType = blankType,
                                    BlankState = "待备料",
                                    BlanktotalPreparedNumber = 0,
                                    BlackNumber = BlankNumber,
                                    BlankAddition = 0
                                };
                                entities.JDJS_WMS_Order_Blank_Table.Add(newRow);
                            }
                            else
                            {
                                foreach (var item in judge)
                                {
                                    item.BlankSpecification = blankSpecification + "#1#";
                                    item.BlankType = blankType;
                                }
                            }
                           


                         



                           
                        }
                        entities.SaveChanges();
                        mytran.Commit();

                    }
                    catch (Exception ex)
                    {
                        mytran.Rollback();
                        context.Response.Write(ex.Message);
                        return;
                    }






                }
            }
            context.Response.Write("ok");
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