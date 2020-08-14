using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace WebApplication2.生产管理.市场部
{
    /// <summary>
    /// virSubmit 的摘要说明
    /// </summary>
    public class virSubmit : IHttpHandler,IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            var orderId = int.Parse(context.Request["orderId"]);
            using (JDJS_WMS_DB_USEREntities entities = new JDJS_WMS_DB_USEREntities())
            {
                using (System.Data.Entity.DbContextTransaction db = entities.Database.BeginTransaction())
                {
                    try
                    {

                  
                    //if (entities.JDJS_WMS_Order_Queue_Table.Where(r => r.OrderID == orderId).Count() > 0)
                    //{
                    //    context.Response.Write("已提交");
                    //    return;
                    //}
                    var userId = entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == orderId).First().virtualProgPersId;
                    var sessionUserId = context.Session["id"];
                    if (userId == Convert.ToInt32(sessionUserId))
                    {
                        var flag = new JDJS_WMS_Order_Queue_Table
                        {
                            OrderID = orderId,
                            isFlag = 0
                        };
                        entities.JDJS_WMS_Order_Queue_Table.Add(flag);
                            entities.SaveChanges();
                            db.Commit();
                            //Func<int, string> func = virScheduling.VirScheduleNeedDeviceNum;
                            //IAsyncResult ar= func.BeginInvoke(orderId,null,null);
                            //string result = func.EndInvoke(ar);
                            string result = "ok";
                            //var result = virScheduling.VirSchedule(orderId);
                            context.Response.Write(result);
                            if (result == "ok")
                            {
                                entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == orderId).First().Intention = 1;
                                entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == orderId).First().IntentionEndTime =DateTime .Now ;
                                var orderInter = entities.JDJS_WMS_Order_Intention_History_Table.Where(r => r.OrderID == orderId).FirstOrDefault();
                                if (orderInter != null)
                                {
                                    orderInter.SubmitTime = DateTime.Now;
                                    orderInter.CraftPersonID = Convert.ToInt32(sessionUserId);
                                    orderInter.LastAlterPersonID = Convert.ToInt32(sessionUserId);
                                    orderInter.LastAlterTime = DateTime.Now;
                                }
                                else
                                {
                                    JDJS_WMS_Order_Intention_History_Table jd = new JDJS_WMS_Order_Intention_History_Table() { 
                                    OrderID =orderId ,
                                    CraftPersonID =Convert.ToInt32 ( sessionUserId),
                                    CreatPersonID =Convert.ToInt32 ( sessionUserId),
                                    CreatTime =DateTime .Now ,
                                    examineResult ="",
                                    flag =1,
                                    LastAlterPersonID =Convert.ToInt32 ( sessionUserId),
                                    LastAlterTime =DateTime .Now ,
                                    SubmitTime =DateTime .Now ,
                                    };
                                    entities.JDJS_WMS_Order_Intention_History_Table.Add(jd);
                                }
                                entities.SaveChanges();
                              
                                return;
                            }
                            else
                            {
                                var row = entities.JDJS_WMS_Order_Queue_Table.Where(r => r.OrderID == orderId);
                                if (row.Count() > 0)
                                {
                                    entities.JDJS_WMS_Order_Queue_Table.Remove(row.First());
                                    entities.SaveChanges();
                                    //db.Commit();
                                    return;
                                }
                            }

                           
                    }
                    else
                    {
                        context.Response.Write("仅责任人可提交");
                        return;
                    }
                    }
                    catch(Exception ex)
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