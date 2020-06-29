using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.市场部
{
    /// <summary>
    /// 读订单队列 的摘要说明
    /// </summary>
    public class 读订单队列 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            {
                //读取队列表
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                List<OrderQueueRead> orderQueueReads = new List<OrderQueueRead>();
                using (JDJS_WMS_DB_USEREntities  JDJSWMS = new JDJS_WMS_DB_USEREntities())
                {
                    using (System.Data.Entity.DbContextTransaction mytran = JDJSWMS.Database.BeginTransaction())
                    {
                        //从表JDJS_WMS_Order_Queue_Table拿出订单队列，
                        //要确认什么时候订单会加入订单队列，是下推时还是点击纳入排产时
                        //如果是在点击纳入排产时订单加入订单队列表中，那插单无法进行。
                        //需要增加一个标志位3，当订单录入表中订单的状态为3时，订单队列中的订单标志为3
                        var OrderQueue = JDJSWMS.JDJS_WMS_Order_Queue_Table.Where(r => r.isFlag == 1).ToList();
                        foreach (var item in OrderQueue)
                        {
                            OrderQueueRead orderQueue = new OrderQueueRead();
                            
                            orderQueue.OrderID = item.OrderID.ToString();
                            var ordernum = JDJSWMS.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == item.OrderID).FirstOrDefault();
                            orderQueue.OrderNum = ordernum.Order_Number;
                            orderQueue.projectName = ordernum.ProjectName;
                            orderQueue.isFlag = Convert.ToInt32(item.isFlag);
                            if (ordernum.Intention == 3)
                            {
                                orderQueue.OrderState = "等待生产";
                                orderQueue.sign = 1;
                            }
                            else if (ordernum.Intention == 2)
                            {
                                //判断是否有机床已经在生产此订单
                                var firstProcess = JDJSWMS.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == ordernum.Order_ID && r.ProcessID == 1 && r.sign == 1).FirstOrDefault();
                                if (firstProcess != null)
                                {
                                    int firstProcessID = firstProcess.ID;
                                    var firstState = JDJSWMS.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.ProcessID == firstProcessID && (r.isFlag == 2 || r.isFlag == 3));
                                    if (firstState.Count() > 0)
                                    {
                                        orderQueue.OrderState = "在生产";
                                        orderQueue.sign = 0;
                                    }
                                    else
                                    {
                                        var firstState1 = JDJSWMS.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.ProcessID == firstProcessID && (r.isFlag == 1 ));
                                        if (firstState1.Count() > 0)
                                        {
                                            orderQueue.OrderState = "等待生产";
                                            orderQueue.sign = 1;
                                        }
                                        else
                                        {
                                            orderQueue.OrderState = "错误，请检查目标需求数或排产情况";
                                            orderQueue.sign = 0;
                                        }
                                    }
                                }

                            }
                            else if (ordernum.Intention == 1)
                            {
                                orderQueue.OrderState = "意向订单";
                                orderQueue.sign = 0;
                            }
                            orderQueueReads.Add(orderQueue);
                        }
                    }
                }
                var model = new { code = 0, data = orderQueueReads };
                var json = serializer.Serialize(model);
                context.Response.Write(json);
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
    public class OrderQueueRead
    {
        /// <summary>
        /// 订单主键ID
        /// </summary>
        public string OrderID;
        public string projectName;
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNum;
        /// <summary>
        /// 订单状态，订单录入表中的Intention为3时状态为等待生产，Intention为2时为在生产。
        /// </summary>
        public string OrderState;
        /// <summary>
        /// 订单队列中的标志位
        /// </summary>
        public int isFlag;
        /// <summary>
        /// 标志位，sign为0时不使能，订单不允许插单。sign为1时使能，订单允许调整顺序
        /// </summary>
        public int sign;
    }
}