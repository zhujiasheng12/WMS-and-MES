using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace WebApplication2.生产管理.工程部
{
    /// <summary>
    /// arrangement 的摘要说明
    /// </summary>
    public class arrangement : IHttpHandler,IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            var personId = Convert.ToInt32(context.Session["id"]);

                var orderNumberId = int.Parse(context.Request["orderNumberId"]);
                using (JDJS_WMS_DB_USEREntities entities = new JDJS_WMS_DB_USEREntities())
                {
                var Engine_Program_ManagerId = entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == orderNumberId).FirstOrDefault().Engine_Program_ManagerId;
                if (personId != Engine_Program_ManagerId)
                {
                    context.Response.Write("您不是该订单编程负责人");
                    return;
                }
                using (System.Data.Entity.DbContextTransaction db = entities.Database.BeginTransaction())
                    {
                    try
                    {
                        entities.JDJS_WMS_Order_Guide_Schedu_Table.Where(r => r.OrderID == orderNumberId).FirstOrDefault().EndTime = DateTime.Now;
                        var row = entities.JDJS_WMS_Order_Queue_Table.Where(r => r.OrderID == orderNumberId);
                        var judge = entities.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.OrderID == orderNumberId & r.isFlag == 1);
                        entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == orderNumberId).FirstOrDefault().Intention = 2;
                        entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == orderNumberId).FirstOrDefault().Order_Actual_Start_Time=DateTime.Now;

                        if (judge.Count() > 0)
                        {
                            context.Response.Write("请勿重复排产");
                            return;
                        }
                        var processInfos = entities.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == orderNumberId && r.sign != 0);
                        foreach (var item in processInfos)
                        {
                            var blankInfo = item.BlankSpecification;
                            if (blankInfo.Contains("#1#"))
                            {
                                item.BlankSpecification = blankInfo.Replace("#1#", "");
                            }
                        }
                        entities.SaveChanges();
                        var rows = entities.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.OrderID == orderNumberId & r.isFlag == 0);
                        if (rows.Count() > 0)
                        {
                            foreach (var item in rows)
                            {
                                entities.JDJS_WMS_Order_Process_Scheduling_Table.Remove(item);
                            }
                        }
                        if (row.Count() > 0)
                        {
                            row.First().isFlag = 1;
                            var confirm = entities.JDJS_WMS_Quality_Confirmation_Table.Where(r => r.OrderID == orderNumberId);
                            if (confirm.Count() == 0)
                            {
                                var Quality_Confirmation = new JDJS_WMS_Quality_Confirmation_Table { OrderID = orderNumberId,CurrFinishedProductNumber =0,DetectionNumber =0,PassRate =0,PefectiveProductNumber =0,PendingNumber =0,QualifiedProductNumber =0 };
                                entities.JDJS_WMS_Quality_Confirmation_Table.Add(Quality_Confirmation);

                            }
                            var judge1 = entities.JDJS_WMS_Finished_Product_Manager.Where(r => r.OrderID == orderNumberId);
                            if (judge1.Count() == 0)
                            {
                                var Finished_Product_Manager = new JDJS_WMS_Finished_Product_Manager { OrderID = orderNumberId, outputTime = DateTime.Now, warehousingTime = DateTime.Now, outputNumber = 0, stock = 0, waitForWarehousing = 0, warehousingNumber = 0,DefectiveProductNumber =0 };
                                entities.JDJS_WMS_Finished_Product_Manager.Add(Finished_Product_Manager);
                            }
                          



                            entities.SaveChanges();
                            db.Commit();
                            context.Response.Write(virScheduling.ProcessSchedule(orderNumberId, personId));
                            return;
                        }
                        else
                        {
                            var newRow = new JDJS_WMS_Order_Queue_Table
                            {
                                isFlag = 1,
                                OrderID = orderNumberId
                            };
                            entities.JDJS_WMS_Order_Queue_Table.Add(newRow);
                            var Quality_Confirmation = new JDJS_WMS_Quality_Confirmation_Table { OrderID = orderNumberId, QualifiedProductNumber = 0, DetectionNumber = 0, CurrFinishedProductNumber = 0, PassRate = 0, PefectiveProductNumber = 0, PendingNumber = 0 };
                            entities.JDJS_WMS_Quality_Confirmation_Table.Add(Quality_Confirmation);

                            var Finished_Product_Manager = new JDJS_WMS_Finished_Product_Manager { OrderID = orderNumberId, outputTime = DateTime.Now, warehousingTime = DateTime.Now, outputNumber = 0, stock = 0, waitForWarehousing = 0, warehousingNumber = 0,DefectiveProductNumber =0};
                            entities.JDJS_WMS_Finished_Product_Manager.Add(Finished_Product_Manager);

                            entities.SaveChanges();
                            db.Commit();
                            context.Response.Write(virScheduling.ProcessSchedule(orderNumberId, personId));
                          
                          
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        db.Rollback();
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