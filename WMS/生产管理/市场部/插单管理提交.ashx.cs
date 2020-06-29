using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.市场部
{
    /// <summary>
    /// 插单管理提交 的摘要说明
    /// </summary>
    public class 插单管理提交 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var form = context.Request.Form;
        
            {
                //写入队列表，先将队列表数据清空，再按照前端传入的数据的顺序写入
                List<OrderQueueWrite> orderQueueWrites = new List<OrderQueueWrite>();//定义的一个用于接收前端传入数据的类
                for (int i = 0; i < form.Count; i++)
                {
                    if (form[i] != null)
                    {
                        string[] str = form[i].ToString().Split(',');
                        OrderQueueWrite write = new OrderQueueWrite();
                        write.OrderID = Convert.ToInt32(str[0]);
                        write.sign = Convert.ToInt32(str[1]);
                        orderQueueWrites.Add(write);//向类的实例中添加数据。每个订单占一个
                    }
                }
                if (true)//是否修改
                {


                    using (JDJS_WMS_DB_USEREntities JDJSWMS = new JDJS_WMS_DB_USEREntities())
                    {
                        using (System.Data.Entity.DbContextTransaction mytran = JDJSWMS.Database.BeginTransaction())
                        {
                            try
                            {


                                foreach (var item in orderQueueWrites)
                                {
                                    if (item.sign == 1)
                                    {
                                        var queue = JDJSWMS.JDJS_WMS_Order_Queue_Table.Where(r => r.OrderID == item.OrderID && r.isFlag == 1);
                                        foreach (var real in queue)
                                        {
                                            JDJSWMS.JDJS_WMS_Order_Queue_Table.Remove(real);//将订单队列表中的不是意向的清空
                                        }
                                        JDJSWMS.SaveChanges();
                                        var procesDevice = JDJSWMS.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.OrderID == item.OrderID && r.isFlag == 1);
                                        foreach (var real in procesDevice)
                                        {
                                            JDJSWMS.JDJS_WMS_Order_Process_Scheduling_Table.Remove(real);
                                        }
                                        JDJSWMS.SaveChanges();
                                        JDJS_WMS_Order_Queue_Table queue_Table = new JDJS_WMS_Order_Queue_Table()
                                        {
                                            OrderID = item.OrderID,
                                            isFlag = item.sign
                                        };
                                        JDJSWMS.JDJS_WMS_Order_Queue_Table.Add(queue_Table);
                                        JDJSWMS.SaveChanges();
                                    }
                                }
                                JDJSWMS.SaveChanges();
                                mytran.Commit();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                mytran.Rollback();
                                return;
                            }
                        }
                    }


                    virScheduling virScheduling = new virScheduling();
                    string str = virScheduling.ProcessSchedule(1);
                    context.Response.Write(str);
                    //调用排产程序    
                }
                else
                {
                    context.Response.Write("ok");
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
    /// <summary>
    /// 写入队列表的类
    /// </summary>
    public class OrderQueueWrite
    {
        /// <summary>
        /// 订单主键ID
        /// </summary>
        public int OrderID;
        /// <summary>
        /// 队列表中的标志位
        /// </summary>
        public int sign;
    }
}