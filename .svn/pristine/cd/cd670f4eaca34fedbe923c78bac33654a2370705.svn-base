using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.资材部
{
    /// <summary>
    /// 追加提交 的摘要说明
    /// </summary>
    public class 追加提交 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var orderId = int.Parse(context.Request["orderId"]);
            var number = int.Parse(context.Request["number"]);
            {
                int orderID = orderId;//订单主键id
                int BlankNum = number;//追加的毛坯数量
                using (JDJS_WMS_DB_USEREntities  wms = new JDJS_WMS_DB_USEREntities ())
                {

                    {

                        //增加机床排产
                        var processInfo = wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == orderID && r.sign == 1).OrderBy(r => r.ProcessID);
                        Dictionary<int, double> processTimeInfo = new Dictionary<int, double>();//工序的主键id和工序需要的时间。
                        foreach (var item in processInfo)
                        {
                            processTimeInfo.Add(item.ID, Convert.ToDouble(item.ProcessTime));
                        }

                        for (int i = 0; i < BlankNum; i++)//开始按，每一件毛坯增加
                        {
                            foreach (var item in processTimeInfo)//给每一序增加
                            {
                                using (System.Data.Entity.DbContextTransaction mytran = wms.Database.BeginTransaction())
                                {
                                    try
                                    {

                                        int deviceID = 0;
                                        var processDevice = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.ProcessID == item.Key && (r.isFlag == 2 || r.isFlag == 1)).OrderBy(r => r.EndTime);
                                        if (processDevice.Count() > 0)//如果该序还有没干或正在干的机床
                                        {
                                            Dictionary<int, DateTime> deviceiNFO = new Dictionary<int, DateTime>();
                                            foreach (var CNC in processDevice)//将每台机床的该序的最晚的结束时间拿出
                                            {
                                                if (!deviceiNFO.ContainsKey(Convert.ToInt32(CNC.CncID)))
                                                {
                                                    deviceiNFO.Add(Convert.ToInt32(CNC.CncID), Convert.ToDateTime(CNC.EndTime));
                                                }
                                                else
                                                {
                                                    if (Convert.ToDateTime(CNC.EndTime) > deviceiNFO[Convert.ToInt32(CNC.CncID)])
                                                    {
                                                        deviceiNFO[Convert.ToInt32(CNC.CncID)] = Convert.ToDateTime(CNC.EndTime);
                                                    }
                                                }
                                            }
                                            DateTime time = DateTime.Now.AddYears(1);
                                            foreach (var real in deviceiNFO)
                                            {
                                                if (real.Value < time)
                                                {
                                                    deviceID = real.Key;
                                                    time = real.Value;
                                                }
                                            }

                                            var nextprocess = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.CncID == deviceID && r.isFlag == 1 && r.ProcessID != item.Key);
                                            foreach (var real in nextprocess)
                                            {
                                                DateTime start = Convert.ToDateTime(real.StartTime);
                                                real.StartTime = start.AddMinutes(item.Value);
                                                DateTime end = Convert.ToDateTime(real.EndTime);
                                                real.EndTime = end.AddMinutes(item.Value);
                                            }
                                            wms.SaveChanges();
                                            JDJS_WMS_Order_Process_Scheduling_Table scheduling_Table = new JDJS_WMS_Order_Process_Scheduling_Table()
                                            {
                                                CncID = deviceID,
                                                OrderID = orderID,
                                                ProcessID = item.Key,
                                                StartTime = time,
                                                EndTime = time.AddMinutes(item.Value),
                                                isFlag = 1
                                            };
                                            wms.JDJS_WMS_Order_Process_Scheduling_Table.Add(scheduling_Table);
                                            wms.SaveChanges();
                                           

                                        }

                                        else//没有机床干这一序
                                        {
                                            context.Response.Write("订单已下机，无法追加！");
                                            mytran.Rollback();
                                            return;
                                        }
                                        wms.SaveChanges();
                                        mytran.Commit();
                                    }
                                    catch (Exception ex)
                                    {
                                        context.Response.Write(ex.Message);
                                        mytran.Rollback();
                                        return;

                                    }
                                }
                            }

                        }


                    }
                    
                    using (System.Data.Entity.DbContextTransaction mytran = wms.Database.BeginTransaction())
                    {
                        try
                        {
                            //JDJS_WMS_Blank_Additional_History_Table blankAdd = new JDJS_WMS_Blank_Additional_History_Table()
                            //{
                            //    OrderID = orderID,
                            //    BlankAddNum = BlankNum,
                            //    AddTime = DateTime.Now
                            //};
                            //wms.JDJS_WMS_Blank_Additional_History_Table.Add(blankAdd);//添加到毛坯添加历史记录表中
                            //wms.SaveChanges();
                            var process = wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == orderID && r.ProcessID == 1).FirstOrDefault();
                            process.BlankNumber += BlankNum;

                            wms.JDJS_WMS_Order_Blank_Table.Where(r => r.OrderID == orderID).FirstOrDefault().BlankAddition += BlankNum;
                            wms.SaveChanges();//将毛坯数量加上

                            wms.SaveChanges();
                            mytran.Commit();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            mytran.Rollback();
                        }
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