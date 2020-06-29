using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.工程部
{
    /// <summary>
    /// 手动排产提交 的摘要说明
    /// </summary>
    public class 手动排产提交 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var form = context.Request.Form;
            var orderId = form[0];
            int orderID = int.Parse(orderId);
            Dictionary<int, List<int>> ProcessCncInfo = new Dictionary<int, List<int>>();
            using (JDJS_WMS_DB_USEREntities entities = new JDJS_WMS_DB_USEREntities())
            {

                //var personId = Convert.ToInt32(context.Session["id"]);
                //var Engine_Program_ManagerId = entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == orderID).FirstOrDefault().Engine_Program_ManagerId;
                //if (personId != Engine_Program_ManagerId)
                //{
                //    context.Response.Write("您不是该订单编程负责人");
                //    return;
                //}

                entities.JDJS_WMS_Order_Guide_Schedu_Table.Where(r => r.OrderID == orderID).FirstOrDefault().EndTime = DateTime.Now;
                var row = entities.JDJS_WMS_Order_Queue_Table.Where(r => r.OrderID == orderID);
                var judge = entities.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.OrderID == orderID & r.isFlag == 1);
                entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == orderID).FirstOrDefault().Intention = 2;
                entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == orderID).FirstOrDefault().Order_Actual_Start_Time = DateTime.Now;

                if (judge.Count() > 0)
                {
                    context.Response.Write("请勿重复排产");
                    return;
                }

                var processInfos = entities.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == orderID && r.sign != 0);
                foreach (var item in processInfos)
                {
                    var blankInfo = item.BlankSpecification;
                    if (blankInfo.Contains("#1#"))
                    {
                        item.BlankSpecification = blankInfo.Replace("#1#", "");
                    }
                }
                entities.SaveChanges();
                var rows = entities.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.OrderID == orderID & r.isFlag == 0);
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
                    var confirm = entities.JDJS_WMS_Quality_Confirmation_Table.Where(r => r.OrderID == orderID);
                    if (confirm.Count() == 0)
                    {
                        var Quality_Confirmation = new JDJS_WMS_Quality_Confirmation_Table { OrderID = orderID };
                        entities.JDJS_WMS_Quality_Confirmation_Table.Add(Quality_Confirmation);

                    }
                    var judge1 = entities.JDJS_WMS_Finished_Product_Manager.Where(r => r.OrderID == orderID);
                    if (judge1.Count() == 0)
                    {
                        var Finished_Product_Manager = new JDJS_WMS_Finished_Product_Manager { OrderID = orderID, outputTime = DateTime.Now, warehousingTime = DateTime.Now, outputNumber = 0, stock = 0, waitForWarehousing = 0, warehousingNumber = 0 };
                        entities.JDJS_WMS_Finished_Product_Manager.Add(Finished_Product_Manager);
                    }




                    entities.SaveChanges();

                    for (int i = 1; i < form.Count; i++)
                    {

                        var cncIds = form[i];
                        var lists = cncIds.Split(',').ToList();
                        List<int> vs = new List<int>();
                        foreach (var item in lists)
                        {
                            vs.Add(int.Parse(item));
                        }
                        ProcessCncInfo.Add(i, vs);


                    }

                    var flag = entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == orderID).First().ProofingORProduct;
                    if (flag == -1)
                    {
                        context.Response.Write(virScheduling.MassScheduling(ProcessCncInfo, orderID));
                    }
                    else
                    {
                        context.Response.Write(virScheduling.ManualScheduling(ProcessCncInfo, orderID));
                    }
                   
                    return;
                }
                else
                {
                    var newRow = new JDJS_WMS_Order_Queue_Table
                    {
                        isFlag = 1,
                        OrderID = orderID
                    };
                    entities.JDJS_WMS_Order_Queue_Table.Add(newRow);
                    var Quality_Confirmation = new JDJS_WMS_Quality_Confirmation_Table { OrderID = orderID, QualifiedProductNumber = 0, DetectionNumber = 0, CurrFinishedProductNumber = 0, PassRate = 0, PefectiveProductNumber = 0, PendingNumber = 0 };
                    entities.JDJS_WMS_Quality_Confirmation_Table.Add(Quality_Confirmation);

                    var Finished_Product_Manager = new JDJS_WMS_Finished_Product_Manager { OrderID = orderID, outputTime = DateTime.Now, warehousingTime = DateTime.Now, outputNumber = 0, stock = 0, waitForWarehousing = 0, warehousingNumber = 0 };
                    entities.JDJS_WMS_Finished_Product_Manager.Add(Finished_Product_Manager);

                    entities.SaveChanges();

                    for (int i = 1; i < form.Count; i++)
                    {

                        var cncIds = form[i];
                        var lists = cncIds.Split(',').ToList();
                        List<int> vs = new List<int>();
                        foreach (var item in lists)
                        {
                            vs.Add(int.Parse(item));
                        }
                        ProcessCncInfo.Add(i, vs);


                    }

                    var flag = entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == orderID).First().ProofingORProduct;
                    if (flag == -1)
                    {
                        context.Response.Write(virScheduling.MassScheduling(ProcessCncInfo, orderID));
                    }
                    else
                    {
                        context.Response.Write(virScheduling.ManualScheduling(ProcessCncInfo, orderID));
                    }


                    return;
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