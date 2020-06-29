﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebApplication2.Model;


namespace WebApplication2
{
    public class virScheduling
    {

        private static readonly object lockObj = new object();



        //public static string VirScheduleNeedDeviceNum(int orderID) { return ""; }
        /// <summary>
        /// 意向排产
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string VirScheduleNeedDeviceNum(int orderID)
        {

            {

                using (JDJS_WMS_DB_USEREntities JDJSWMS = new JDJS_WMS_DB_USEREntities())
                {
                    var Queue = JDJSWMS.JDJS_WMS_Order_Queue_Table.Where(r => r.isFlag == 0).ToList();
                    //foreach (var order in Queue)
                    {



                        int OrderID = Convert.ToInt32(orderID);

                        DateTime time = DateTime.Now;


                        List<DeviceInfo> deviceInfos = new List<DeviceInfo>();
                        var deviceList = JDJSWMS.JDJS_WMS_Device_Info.ToList();
                        foreach (var item in deviceList)//获取车间所有的机床
                        {
                            //获取机床订单队列中的当前机床使用情况
                            var AllDeviceList = JDJSWMS.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.CncID == item.ID && r.EndTime >= time);
                            if (AllDeviceList.Count() > 0)
                            {
                                Parallel.ForEach(AllDeviceList, new ParallelOptions() { MaxDegreeOfParallelism = 2 }, real =>
                                {
                                    DeviceInfo device = new DeviceInfo();
                                    device.CncID = Convert.ToInt32(real.CncID);
                                    device.order = Convert.ToInt32(real.OrderID);
                                    device.process = Convert.ToInt32(real.ProcessID);

                                    device.Type = item.MachType.ToString();
                                    device.startTime = Convert.ToDateTime(real.StartTime);
                                    device.endTime = Convert.ToDateTime(real.EndTime);
                                    deviceInfos.Add(device);
                                });

                            }
                            else
                            {
                                DeviceInfo device = new DeviceInfo();
                                device.CncID = item.ID;
                                device.order = 0;
                                device.process = 0;

                                device.Type = item.MachType.ToString();
                                device.startTime = time;
                                device.endTime = time.AddMinutes(1);//如果机床没使用过，则将结束时间置位当前时间的后一分钟
                                deviceInfos.Add(device);
                            }

                        }


                        OrderInfo orderInfo = new OrderInfo();//获取订单信息
                        List<double> useTime = new List<double>();
                        orderInfo.OrderID = OrderID;
                        var isFlag = JDJSWMS.JDJS_WMS_Order_Queue_Table.Where(r => r.OrderID == OrderID).First();
                        orderInfo.isFlag = Convert.ToInt32(isFlag.isFlag);
                        var orderNum = JDJSWMS.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == OrderID).First();
                        orderInfo.OrderNum = orderNum.Order_Number;
                        orderInfo.OrderOutPut = orderNum.Product_Output;
                        int maxNums = 0;
                        Dictionary<int, int> cncNums = new Dictionary<int, int>();//每道工序需要使用的机床种类总数数目
                        var processInfo = JDJSWMS.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == OrderID && r.sign == 0).ToList();
                        foreach (var item in processInfo)//获取该订单的工序信息
                        {
                            useTime.Add(Convert.ToDouble(item.ProcessTime));
                            ProcessInfo process = new ProcessInfo();
                            process.id = Convert.ToInt32(item.ID);
                            process.deviceNum = item.MachNumber == null ? 1 : Convert.ToInt32(item.MachNumber);
                            process.processID = Convert.ToInt32(item.ProcessID);
                            process.processTime = Convert.ToDouble(item.ProcessTime);
                            process.deviceType = Convert.ToInt32(item.DeviceType);
                            int thiscncNums = JDJSWMS.JDJS_WMS_Device_Info.Where(r => r.MachType == item.DeviceType).Count();
                            cncNums.Add(Convert.ToInt32(item.ProcessID), thiscncNums);
                            if (thiscncNums > maxNums)
                            {
                                maxNums = thiscncNums;
                            }
                            orderInfo.OrderProcessInfos.Add(process);
                        }
                        double MinTime = useTime.Min();//最小工序时间
                        double Alltime = 0;
                        for (int i = 0; i < useTime.Count(); i++)
                        {
                            Alltime = Alltime + useTime[i];
                            useTime[i] = useTime[i] / MinTime;//单位组各工序占比
                        }
                        //不同的组数对应不同的机床排布
                        Dictionary<int, List<DeviceInfo>> deviceGroupList = new Dictionary<int, List<DeviceInfo>>();

                        // int GroupMaxNum = Math.Min(orderInfo.OrderOutPut, maxNums);
                        //Parallel.For(1, GroupMaxNum, l =>
                        ////for (int l = 1; l <= GroupMaxNum; l++)
                        {
                            List<DeviceInfo> deviceInfosGroup = new List<DeviceInfo>();
                            foreach (var item in deviceInfos)
                            {
                                deviceInfosGroup.Add(item);
                            }
                            deviceGroupList.Add(1, deviceInfosGroup);
                            //List<int> deviceNum = new List<int>();

                            //计算合理的机床配比
                            //if (CalculationProportion(l, orderInfo, useTime, cncNums, deviceNum))
                            {
                                //获取的list中是各工序合理的机床配比
                                Dictionary<int, int> ProcessDeviceNum = new Dictionary<int, int>();
                                for (int i = 0; i < orderInfo.OrderProcessInfos.Count(); i++)
                                {
                                    ProcessDeviceNum.Add(orderInfo.OrderProcessInfos[i].processID, orderInfo.OrderProcessInfos[i].deviceNum);
                                }
                                DateTime ProcessEndTimeFirst = time;
                                int Proportion = 1;
                                for (int k = 0; k < orderInfo.OrderProcessInfos.Count(); k++)
                                {
                                    List<int> CncID = new List<int>();//该序使用的机床集合
                                    int processID = orderInfo.OrderProcessInfos[k].id;
                                    //下一句是获取改序在数据库中已经排产了多少件
                                    var ProcessNum = JDJSWMS.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.OrderID == orderInfo.OrderID && r.ProcessID == processID);
                                    int nu = ProcessNum.Count();
                                    for (int i = 0; i < orderInfo.OrderOutPut - nu; i++)
                                    {

                                        //var info = deviceInfos.Where(r => r.Type == TableThereaxes.Status && r.endTime >= time);
                                        //var info = deviceGroupList[l].Where(r =>(r.endTime >= ProcessEndTimeFirst));
                                        var info = deviceGroupList[1];
                                        int cnc = 0;
                                        TimeSpan ts = TimeSpan.Parse("1000");
                                        DateTime start = DateTime.Now;
                                        Dictionary<int, DateTime> list = new Dictionary<int, DateTime>();//每台机床最晚的结束时间拿出来
                                        int deviceTypeID1 = orderInfo.OrderProcessInfos[k].deviceType;

                                        //foreach (var eume in info)
                                        //{
                                        //     if (eume.Type == deviceTypeID1.ToString())
                                        //    {
                                        //        lock (lockObj)
                                        //        {
                                        //            if (!list.ContainsKey(eume.CncID))
                                        //            {
                                        //                list.Add(eume.CncID, eume.endTime);
                                        //            }
                                        //            else
                                        //            {
                                        //                if (list[eume.CncID] < eume.endTime)
                                        //                {
                                        //                    list[eume.CncID] = eume.endTime;
                                        //                }
                                        //            }
                                        //        }

                                        //    }
                                        //}



                                        Parallel.ForEach(info, new ParallelOptions() { MaxDegreeOfParallelism = 2 }, eume =>
                                        {

                                            //var str1 = JDJSWMS.JDJS_WMS_Device_Status_Table.Where(r => r.ID == deviceTypeID1).FirstOrDefault();
                                            if (eume.Type == deviceTypeID1.ToString())
                                            {
                                                lock (lockObj)
                                                {
                                                    if (!list.ContainsKey(eume.CncID))
                                                    {
                                                        list.Add(eume.CncID, eume.endTime);
                                                    }
                                                    else
                                                    {
                                                        if (list[eume.CncID] < eume.endTime)
                                                        {
                                                            list[eume.CncID] = eume.endTime;
                                                        }
                                                    }
                                                }

                                            }
                                        });
                                        foreach (var real in list)
                                        {
                                            TimeSpan t = real.Value - time;
                                            //TimeSpan t = real.Value - ProcessEndTimeFirst;
                                            if (t < ts)
                                            {
                                                if (CncID.Count() >= ProcessDeviceNum[orderInfo.OrderProcessInfos[k].processID] && (!CncID.Contains(real.Key)))
                                                {

                                                    //如果已经超出最佳机床配比台数
                                                }
                                                else
                                                {
                                                    ts = t;
                                                    if (real.Value < time)
                                                    {
                                                        start = time;
                                                    }
                                                    else
                                                    {
                                                        start = real.Value;
                                                    }
                                                    cnc = real.Key;
                                                }
                                            }
                                        }
                                        if (!CncID.Contains(cnc))
                                        {
                                            CncID.Add(cnc);
                                        }
                                        DeviceInfo device = new DeviceInfo();
                                        device.CncID = cnc;

                                        int deviceTypeID = orderInfo.OrderProcessInfos[k].deviceType;

                                        device.Type = deviceTypeID.ToString();//机床种类需要修改
                                        device.order = orderInfo.OrderID;
                                        device.process = orderInfo.OrderProcessInfos[k].id;
                                        device.startTime = start;
                                        device.endTime = start.AddMinutes(orderInfo.OrderProcessInfos[k].processTime);
                                        if (i == orderInfo.OrderOutPut - nu - 1)//如果是下一件该序的最后一件
                                        {
                                            if (k < orderInfo.OrderProcessInfos.Count() - 1 && orderInfo.OrderProcessInfos[k + 1].processTime > orderInfo.OrderProcessInfos[k].processTime)
                                            {
                                                Proportion = Convert.ToInt32(Math.Ceiling(orderInfo.OrderProcessInfos[k + 1].processTime / orderInfo.OrderProcessInfos[k].processTime));
                                            }
                                            else
                                            {
                                                Proportion = 1;
                                            }
                                            ProcessEndTimeFirst = ProcessEndTimeFirst.AddMinutes(orderInfo.OrderProcessInfos[k].processTime * Proportion);
                                        }
                                        deviceGroupList[1].Add(device);



                                    }
                                }
                            }

                        }
                        TimeSpan duration = TimeSpan.Parse("1000");
                        DateTime EndTime = DateTime.Now.AddYears(100);

                        //判断不同的组数需要的时长与交期时间

                        List<JDJS_WMS_Order_Process_Scheduling_Table> tables = new List<JDJS_WMS_Order_Process_Scheduling_Table>();

                        //判断出最佳组数后填写入数据库


                        foreach (var item in deviceGroupList[1])
                        {
                            if (!deviceInfos.Contains(item))
                            {
                                JDJS_WMS_Order_Process_Scheduling_Table Scheduling = new JDJS_WMS_Order_Process_Scheduling_Table()
                                {
                                    CncID = item.CncID,
                                    OrderID = item.order,
                                    ProcessID = item.process,
                                    StartTime = item.startTime,
                                    EndTime = item.endTime,
                                    isFlag = 0

                                };
                                tables.Add(Scheduling);
                                //JDJSWMS.JDJS_WMS_Order_Process_Scheduling_Table.Add(Scheduling);
                                //JDJSWMS.SaveChanges();
                            }

                        }
                        using (System.Data.Entity.DbContextTransaction mytran = JDJSWMS.Database.BeginTransaction())
                        {
                            try
                            {
                                JDJSWMS.JDJS_WMS_Order_Process_Scheduling_Table.BulkInsert<JDJS_WMS_Order_Process_Scheduling_Table>(tables);
                                JDJSWMS.BulkSaveChanges();
                                //Parallel.ForEach(deviceGroupList[groupNum], new ParallelOptions() { MaxDegreeOfParallelism =50},item=>                                 
                                //{
                                //    if (!deviceInfos.Contains(item))
                                //    {
                                //        JDJS_WMS_Order_Process_Scheduling_Table Scheduling = new JDJS_WMS_Order_Process_Scheduling_Table()
                                //        {
                                //            CncID = item.CncID,
                                //            OrderID = item.order,
                                //            ProcessID = item.process,
                                //            StartTime = item.startTime,
                                //            EndTime = item.endTime,
                                //            isFlag = 0

                                //        };
                                //        tables.Add(Scheduling);


                                //    }

                                //}
                                // );
                                //JDJSWMS.JDJS_WMS_Order_Process_Scheduling_Table.AddRange(tables);
                                JDJSWMS.SaveChanges();
                                mytran.Commit();
                            }
                            catch (Exception ex)
                            {

                                //Console.ReadKey();
                                mytran.Rollback();
                                return ex.Message;
                            }
                        }
                    }
                    return "ok";

                }

            }
        }

        /// <summary>
        /// 意向排产
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string VirSchedule(int orderID)
        {

            {

                using (JDJS_WMS_DB_USEREntities JDJSWMS = new JDJS_WMS_DB_USEREntities())
                {
                    var Queue = JDJSWMS.JDJS_WMS_Order_Queue_Table.Where(r => r.isFlag == 0).ToList();
                    //foreach (var order in Queue)
                    {



                        int OrderID = Convert.ToInt32(orderID);

                        DateTime time = DateTime.Now;


                        List<DeviceInfo> deviceInfos = new List<DeviceInfo>();
                        var deviceList = JDJSWMS.JDJS_WMS_Device_Info.ToList();
                        foreach (var item in deviceList)//获取车间所有的机床
                        {
                            //获取机床订单队列中的当前机床使用情况
                            var AllDeviceList = JDJSWMS.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.CncID == item.ID && r.EndTime >= time);
                            if (AllDeviceList.Count() > 0)
                            {
                                Parallel.ForEach(AllDeviceList, new ParallelOptions() { MaxDegreeOfParallelism = 2 }, real =>
                                 {
                                     DeviceInfo device = new DeviceInfo();
                                     device.CncID = Convert.ToInt32(real.CncID);
                                     device.order = Convert.ToInt32(real.OrderID);
                                     device.process = Convert.ToInt32(real.ProcessID);

                                     device.Type = item.MachType.ToString();
                                     device.startTime = Convert.ToDateTime(real.StartTime);
                                     device.endTime = Convert.ToDateTime(real.EndTime);
                                     deviceInfos.Add(device);
                                 });

                            }
                            else
                            {
                                DeviceInfo device = new DeviceInfo();
                                device.CncID = item.ID;
                                device.order = 0;
                                device.process = 0;

                                device.Type = item.MachType.ToString();
                                device.startTime = time;
                                device.endTime = time.AddMinutes(1);//如果机床没使用过，则将结束时间置位当前时间的后一分钟
                                deviceInfos.Add(device);
                            }

                        }


                        OrderInfo orderInfo = new OrderInfo();//获取订单信息
                        List<double> useTime = new List<double>();
                        orderInfo.OrderID = OrderID;
                        var isFlag = JDJSWMS.JDJS_WMS_Order_Queue_Table.Where(r => r.OrderID == OrderID).First();
                        orderInfo.isFlag = Convert.ToInt32(isFlag.isFlag);
                        var orderNum = JDJSWMS.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == OrderID).First();
                        orderInfo.OrderNum = orderNum.Order_Number;
                        orderInfo.OrderOutPut = orderNum.Product_Output;
                        int maxNums = 0;
                        Dictionary<int, int> cncNums = new Dictionary<int, int>();//每道工序需要使用的机床种类总数数目
                        var processInfo = JDJSWMS.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == OrderID && r.sign == 0).ToList();
                        foreach (var item in processInfo)//获取该订单的工序信息
                        {
                            useTime.Add(Convert.ToDouble(item.ProcessTime));
                            ProcessInfo process = new ProcessInfo();
                            process.id = Convert.ToInt32(item.ID);
                            process.processID = Convert.ToInt32(item.ProcessID);
                            process.processTime = Convert.ToDouble(item.ProcessTime);
                            process.deviceType = Convert.ToInt32(item.DeviceType);
                            int thiscncNums = JDJSWMS.JDJS_WMS_Device_Info.Where(r => r.MachType == item.DeviceType).Count();
                            cncNums.Add(Convert.ToInt32(item.ProcessID), thiscncNums);
                            if (thiscncNums > maxNums)
                            {
                                maxNums = thiscncNums;
                            }
                            orderInfo.OrderProcessInfos.Add(process);
                        }
                        double MinTime = useTime.Min();//最小工序时间
                        double Alltime = 0;
                        for (int i = 0; i < useTime.Count(); i++)
                        {
                            Alltime = Alltime + useTime[i];
                            useTime[i] = useTime[i] / MinTime;//单位组各工序占比
                        }
                        //不同的组数对应不同的机床排布
                        Dictionary<int, List<DeviceInfo>> deviceGroupList = new Dictionary<int, List<DeviceInfo>>();

                        int GroupMaxNum = Math.Min(orderInfo.OrderOutPut, maxNums);
                        Parallel.For(1, GroupMaxNum, l =>
                        //for (int l = 1; l <= GroupMaxNum; l++)
                        {
                            List<DeviceInfo> deviceInfosGroup = new List<DeviceInfo>();
                            foreach (var item in deviceInfos)
                            {
                                deviceInfosGroup.Add(item);
                            }
                            deviceGroupList.Add(l, deviceInfosGroup);
                            List<int> deviceNum = new List<int>();

                            //计算合理的机床配比
                            if (CalculationProportion(l, orderInfo, useTime, cncNums, deviceNum))
                            {
                                //获取的list中是各工序合理的机床配比
                                Dictionary<int, int> ProcessDeviceNum = new Dictionary<int, int>();
                                for (int i = 0; i < deviceNum.Count(); i++)
                                {
                                    ProcessDeviceNum.Add(orderInfo.OrderProcessInfos[i].processID, deviceNum[i]);
                                }
                                DateTime ProcessEndTimeFirst = time;
                                int Proportion = 1;
                                for (int k = 0; k < orderInfo.OrderProcessInfos.Count(); k++)
                                {
                                    List<int> CncID = new List<int>();//该序使用的机床集合
                                    int processID = orderInfo.OrderProcessInfos[k].id;
                                    //下一句是获取改序在数据库中已经排产了多少件
                                    var ProcessNum = JDJSWMS.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.OrderID == orderInfo.OrderID && r.ProcessID == processID);
                                    int nu = ProcessNum.Count();
                                    for (int i = 0; i < orderInfo.OrderOutPut - nu; i++)
                                    {

                                        //var info = deviceInfos.Where(r => r.Type == TableThereaxes.Status && r.endTime >= time);
                                        //var info = deviceGroupList[l].Where(r =>(r.endTime >= ProcessEndTimeFirst));
                                        var info = deviceGroupList[l];
                                        int cnc = 0;
                                        TimeSpan ts = TimeSpan.Parse("1000");
                                        DateTime start = DateTime.Now;
                                        Dictionary<int, DateTime> list = new Dictionary<int, DateTime>();//每台机床最晚的结束时间拿出来
                                        int deviceTypeID1 = orderInfo.OrderProcessInfos[k].deviceType;
                                        Parallel.ForEach(info, new ParallelOptions() { MaxDegreeOfParallelism = 2 }, eume =>
                                        {

                                            //var str1 = JDJSWMS.JDJS_WMS_Device_Status_Table.Where(r => r.ID == deviceTypeID1).FirstOrDefault();
                                            if (eume.Type == deviceTypeID1.ToString())
                                            {
                                                lock (lockObj)
                                                {
                                                    if (!list.ContainsKey(eume.CncID))
                                                    {
                                                        list.Add(eume.CncID, eume.endTime);
                                                    }
                                                    else
                                                    {
                                                        if (list[eume.CncID] < eume.endTime)
                                                        {
                                                            list[eume.CncID] = eume.endTime;
                                                        }
                                                    }
                                                }

                                            }
                                        });
                                        foreach (var real in list)
                                        {
                                            TimeSpan t = real.Value - time;
                                            //TimeSpan t = real.Value - ProcessEndTimeFirst;
                                            if (t < ts)
                                            {
                                                if (CncID.Count() >= ProcessDeviceNum[orderInfo.OrderProcessInfos[k].processID] && (!CncID.Contains(real.Key)))
                                                {

                                                    //如果已经超出最佳机床配比台数
                                                }
                                                else
                                                {
                                                    ts = t;
                                                    if (real.Value < time)
                                                    {
                                                        start = time;
                                                    }
                                                    else
                                                    {
                                                        start = real.Value;
                                                    }
                                                    cnc = real.Key;
                                                }
                                            }
                                        }
                                        if (!CncID.Contains(cnc))
                                        {
                                            CncID.Add(cnc);
                                        }
                                        DeviceInfo device = new DeviceInfo();
                                        device.CncID = cnc;

                                        int deviceTypeID = orderInfo.OrderProcessInfos[k].deviceType;

                                        device.Type = deviceTypeID.ToString();//机床种类需要修改
                                        device.order = orderInfo.OrderID;
                                        device.process = orderInfo.OrderProcessInfos[k].id;
                                        device.startTime = start;
                                        device.endTime = start.AddMinutes(orderInfo.OrderProcessInfos[k].processTime);
                                        if (i == orderInfo.OrderOutPut - nu - 1)//如果是下一件该序的最后一件
                                        {
                                            if (k < orderInfo.OrderProcessInfos.Count() - 1 && orderInfo.OrderProcessInfos[k + 1].processTime > orderInfo.OrderProcessInfos[k].processTime)
                                            {
                                                Proportion = Convert.ToInt32(Math.Ceiling(orderInfo.OrderProcessInfos[k + 1].processTime / orderInfo.OrderProcessInfos[k].processTime));
                                            }
                                            else
                                            {
                                                Proportion = 1;
                                            }
                                            ProcessEndTimeFirst = ProcessEndTimeFirst.AddMinutes(orderInfo.OrderProcessInfos[k].processTime * Proportion);
                                        }
                                        deviceGroupList[l].Add(device);



                                    }
                                }
                            }
                            else
                            {
                                deviceGroupList.Remove(l);
                                //goto lableA;
                            }
                        });
                    lableA: TimeSpan duration = TimeSpan.Parse("1000");
                        int groupNum = 0;
                        DateTime EndTime = DateTime.Now.AddYears(100);

                        //判断不同的组数需要的时长与交期时间
                        foreach (var item in deviceGroupList)
                        {
                            DateTime beninTime = DateTime.Now.AddYears(100);
                            DateTime finishTime = DateTime.Now.AddYears(-100);
                            foreach (var real in item.Value)
                            {
                                if (real.order == orderInfo.OrderID)
                                {
                                    if (real.startTime < beninTime)
                                    {
                                        beninTime = real.startTime;
                                    }
                                    if (real.endTime > finishTime)
                                    {
                                        finishTime = real.endTime;
                                    }
                                }

                            }
                            if (finishTime < EndTime)
                            {
                                EndTime = finishTime;
                                groupNum = item.Key;
                            }
                        }
                        List<JDJS_WMS_Order_Process_Scheduling_Table> tables = new List<JDJS_WMS_Order_Process_Scheduling_Table>();

                        //判断出最佳组数后填写入数据库


                        foreach (var item in deviceGroupList[groupNum])
                        {
                            if (!deviceInfos.Contains(item))
                            {
                                JDJS_WMS_Order_Process_Scheduling_Table Scheduling = new JDJS_WMS_Order_Process_Scheduling_Table()
                                {
                                    CncID = item.CncID,
                                    OrderID = item.order,
                                    ProcessID = item.process,
                                    StartTime = item.startTime,
                                    EndTime = item.endTime,
                                    isFlag = 0

                                };
                                tables.Add(Scheduling);
                                //JDJSWMS.JDJS_WMS_Order_Process_Scheduling_Table.Add(Scheduling);
                                //JDJSWMS.SaveChanges();
                            }

                        }
                        using (System.Data.Entity.DbContextTransaction mytran = JDJSWMS.Database.BeginTransaction())
                        {
                            try
                            {
                                JDJSWMS.JDJS_WMS_Order_Process_Scheduling_Table.BulkInsert<JDJS_WMS_Order_Process_Scheduling_Table>(tables);
                                JDJSWMS.BulkSaveChanges();
                                //Parallel.ForEach(deviceGroupList[groupNum], new ParallelOptions() { MaxDegreeOfParallelism =50},item=>                                 
                                //{
                                //    if (!deviceInfos.Contains(item))
                                //    {
                                //        JDJS_WMS_Order_Process_Scheduling_Table Scheduling = new JDJS_WMS_Order_Process_Scheduling_Table()
                                //        {
                                //            CncID = item.CncID,
                                //            OrderID = item.order,
                                //            ProcessID = item.process,
                                //            StartTime = item.startTime,
                                //            EndTime = item.endTime,
                                //            isFlag = 0

                                //        };
                                //        tables.Add(Scheduling);


                                //    }

                                //}
                                // );
                                //JDJSWMS.JDJS_WMS_Order_Process_Scheduling_Table.AddRange(tables);
                                JDJSWMS.SaveChanges();
                                mytran.Commit();
                            }
                            catch (Exception ex)
                            {

                                //Console.ReadKey();
                                mytran.Rollback();
                                return ex.Message;
                            }
                        }
                    }
                    return "ok";

                }

            }
        }

        /// <summary>
        /// 生产排产
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string ProcessSchedule(int orderID)
        {
            {
                using (JDJS_WMS_DB_USEREntities JDJSWMS = new JDJS_WMS_DB_USEREntities())
                {


                    {


                        using (System.Data.Entity.DbContextTransaction mytran = JDJSWMS.Database.BeginTransaction())
                        {
                            try
                            {
                                int OrderID = Convert.ToInt32(orderID);

                                DateTime time = DateTime.Now;


                                List<DeviceInfo> deviceInfos = new List<DeviceInfo>();
                                var deviceList = JDJSWMS.JDJS_WMS_Device_Info.ToList();
                                foreach (var item in deviceList)//获取车间所有的机床
                                {
                                    //获取机床订单队列中的当前机床使用情况
                                    var AllDeviceList = JDJSWMS.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.CncID == item.ID && r.EndTime >= time && r.isFlag != 0);
                                    if (AllDeviceList.Count() > 0)
                                    {
                                        Parallel.ForEach(AllDeviceList, new ParallelOptions() { MaxDegreeOfParallelism = 2 }, real =>
                                        {
                                            DeviceInfo device = new DeviceInfo();
                                            device.CncID = Convert.ToInt32(real.CncID);
                                            device.order = Convert.ToInt32(real.OrderID);
                                            device.process = Convert.ToInt32(real.ProcessID);

                                            device.Type = item.MachType.ToString();
                                            device.startTime = Convert.ToDateTime(real.StartTime);
                                            device.endTime = Convert.ToDateTime(real.EndTime);
                                            deviceInfos.Add(device);
                                        });

                                    }
                                    else
                                    {
                                        DeviceInfo device = new DeviceInfo();
                                        device.CncID = item.ID;
                                        device.order = 0;
                                        device.process = 0;

                                        device.Type = item.MachType.ToString();
                                        device.startTime = time;
                                        device.endTime = time.AddMinutes(1);//如果机床没使用过，则将结束时间置位当前时间的后一分钟
                                        deviceInfos.Add(device);
                                    }

                                }

                                ///*Dictio*/nary<int, int> cncNums = new Dictionary<int, int>();//每道工序需要使用的机床种类总数数目
                                OrderInfo orderInfo = new OrderInfo();//获取订单信息
                                List<double> useTime = new List<double>();
                                List<int> moduluses = new List<int>();
                                orderInfo.OrderID = OrderID;

                                var isFlag = JDJSWMS.JDJS_WMS_Order_Queue_Table.Where(r => r.OrderID == OrderID).First();
                                orderInfo.isFlag = Convert.ToInt32(isFlag.isFlag);
                                var orderNum = JDJSWMS.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == OrderID).First();
                                orderInfo.OrderNum = orderNum.Order_Number;
                                orderInfo.OrderOutPut = orderNum.Product_Output;
                                var processInfo = JDJSWMS.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == OrderID && r.sign == 1).ToList();
                                int maxNums = 0;
                                Dictionary<int, int> cncNums = new Dictionary<int, int>();//每道工序需要使用的机床种类总数数目
                                int blankNum = 0;
                                int modelusfenmu = 1;
                                var blanks = JDJSWMS.JDJS_WMS_Order_Blank_Table.Where(r => r.OrderID == OrderID).FirstOrDefault();
                                if (blanks != null)
                                {

                                    blankNum = Convert.ToInt32(blanks.BlackNumber);
                                }
                                foreach (var item in processInfo)//获取该订单的工序信息
                                {
                                    useTime.Add(Convert.ToDouble(item.ProcessTime));
                                    ProcessInfo process = new ProcessInfo();
                                    process.id = Convert.ToInt32(item.ID);
                                    process.processID = Convert.ToInt32(item.ProcessID);
                                    process.processTime = Convert.ToDouble(item.ProcessTime);
                                    process.deviceType = Convert.ToInt32(item.DeviceType);
                                    process.modulus = Convert.ToInt32(item.Modulus);
                                    moduluses.Add(Convert.ToInt32(item.Modulus));
                                    process.ScheduNum = blankNum * modelusfenmu;
                                    modelusfenmu = modelusfenmu * Convert.ToInt32(item.Modulus);

                                    int thiscncNums = JDJSWMS.JDJS_WMS_Device_Info.Where(r => r.MachType == item.DeviceType).Count();
                                    cncNums.Add(Convert.ToInt32(item.ProcessID), thiscncNums);
                                    if (thiscncNums > maxNums)
                                    {
                                        maxNums = thiscncNums;
                                    }
                                    orderInfo.OrderProcessInfos.Add(process);
                                }
                                for (int i = 0; i < useTime.Count(); i++)
                                {
                                    useTime[i] = useTime[i] / (((double)moduluses[i]) / modelusfenmu);
                                }
                                double MinTime = useTime.Min();//最小工序时间
                                double Alltime = 0;
                                for (int i = 0; i < useTime.Count(); i++)
                                {
                                    Alltime = Alltime + useTime[i];
                                    useTime[i] = useTime[i] / MinTime;//单位组各工序占比
                                }
                                //不同的组数对应不同的机床排布
                                Dictionary<int, List<DeviceInfo>> deviceGroupList = new Dictionary<int, List<DeviceInfo>>();

                                int GroupMaxNum = Math.Min (orderInfo.OrderOutPut, maxNums);
                                for (int l = 1; l <= GroupMaxNum; l++)
                                {
                                    List<DeviceInfo> deviceInfosGroup = new List<DeviceInfo>();
                                    foreach (var item in deviceInfos)
                                    {
                                        deviceInfosGroup.Add(item);
                                    }
                                    deviceGroupList.Add(l, deviceInfosGroup);
                                    List<int> deviceNum = new List<int>();

                                    //计算合理的机床配比
                                    if (CalculationProportion(l, orderInfo, useTime, cncNums, deviceNum))
                                    {
                                        //获取的list中是各工序合理的机床配比
                                        Dictionary<int, int> ProcessDeviceNum = new Dictionary<int, int>();
                                        for (int i = 0; i < deviceNum.Count(); i++)
                                        {
                                            ProcessDeviceNum.Add(orderInfo.OrderProcessInfos[i].processID, deviceNum[i]);
                                        }
                                        DateTime ProcessEndTimeFirst = time;
                                        int Proportion = 1;
                                        for (int k = 0; k < orderInfo.OrderProcessInfos.Count(); k++)
                                        {
                                            List<int> CncID = new List<int>();//该序使用的机床集合
                                            int processID = orderInfo.OrderProcessInfos[k].id;
                                            //下一句是获取改序在数据库中已经排产了多少件
                                            var ProcessNum = JDJSWMS.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.OrderID == orderInfo.OrderID && r.ProcessID == processID && r.isFlag != 0);
                                            int nu = ProcessNum.Count();
                                            for (int i = 0; i < orderInfo.OrderProcessInfos[k].ScheduNum - nu; i++)
                                            {

                                                //var info = deviceInfos.Where(r => r.Type == TableThereaxes.Status && r.endTime >= time);
                                                //var info = deviceGroupList[l].Where(r =>(r.endTime >= ProcessEndTimeFirst));
                                                var info = deviceGroupList[l];
                                                int cnc = 0;
                                                TimeSpan ts = TimeSpan.Parse("1000");
                                                DateTime start = DateTime.Now;
                                                Dictionary<int, DateTime> list = new Dictionary<int, DateTime>();//每台机床最晚的结束时间拿出来
                                                int deviceTypeID1 = orderInfo.OrderProcessInfos[k].deviceType;
                                                Parallel.ForEach(info, new ParallelOptions() { MaxDegreeOfParallelism = 2 }, eume =>
                                                {

                                                    //var str1 = JDJSWMS.JDJS_WMS_Device_Status_Table.Where(r => r.ID == deviceTypeID1).FirstOrDefault();
                                                    if (eume.Type == deviceTypeID1.ToString())
                                                    {
                                                        lock (lockObj)
                                                        {
                                                            if (!list.ContainsKey(eume.CncID))
                                                            {
                                                                list.Add(eume.CncID, eume.endTime);
                                                            }
                                                            else
                                                            {
                                                                if (list[eume.CncID] < eume.endTime)
                                                                {
                                                                    list[eume.CncID] = eume.endTime;
                                                                }
                                                            }
                                                        }

                                                    }
                                                });
                                                foreach (var real in list)
                                                {
                                                    TimeSpan t = real.Value - time;
                                                    //TimeSpan t = real.Value - ProcessEndTimeFirst;
                                                    if (t < ts)
                                                    {
                                                        if (CncID.Count() >= ProcessDeviceNum[orderInfo.OrderProcessInfos[k].processID] && (!CncID.Contains(real.Key)))
                                                        {

                                                            //如果已经超出最佳机床配比台数
                                                        }
                                                        else
                                                        {
                                                            ts = t;
                                                            if (real.Value < time)
                                                            {
                                                                start = time;
                                                            }
                                                            else
                                                            {
                                                                start = real.Value;
                                                            }
                                                            cnc = real.Key;
                                                        }
                                                    }
                                                }
                                                if (!CncID.Contains(cnc))
                                                {
                                                    CncID.Add(cnc);
                                                }
                                                DeviceInfo device = new DeviceInfo();
                                                device.CncID = cnc;

                                                int deviceTypeID = orderInfo.OrderProcessInfos[k].deviceType;

                                                device.Type = deviceTypeID.ToString();//机床种类需要修改
                                                device.order = orderInfo.OrderID;
                                                device.process = orderInfo.OrderProcessInfos[k].id;
                                                device.startTime = start;
                                                device.endTime = start.AddMinutes(orderInfo.OrderProcessInfos[k].processTime);
                                                if (i == orderInfo.OrderOutPut - nu - 1)//如果是下一件该序的最后一件
                                                {
                                                    if (k < orderInfo.OrderProcessInfos.Count() - 1 && orderInfo.OrderProcessInfos[k + 1].processTime > orderInfo.OrderProcessInfos[k].processTime)
                                                    {
                                                        Proportion = Convert.ToInt32(Math.Ceiling(orderInfo.OrderProcessInfos[k + 1].processTime / orderInfo.OrderProcessInfos[k].processTime));
                                                    }
                                                    else
                                                    {
                                                        Proportion = 1;
                                                    }
                                                    ProcessEndTimeFirst = ProcessEndTimeFirst.AddMinutes(orderInfo.OrderProcessInfos[k].processTime * Proportion);
                                                }
                                                deviceGroupList[l].Add(device);



                                            }
                                        }
                                    }
                                    else
                                    {
                                        deviceGroupList.Remove(l);
                                        goto lableA;
                                    }
                                }
                            lableA: TimeSpan duration = TimeSpan.Parse("1000");
                                int groupNum = 0;
                                DateTime EndTime = DateTime.Now.AddYears(100);

                                //判断不同的组数需要的时长与交期时间
                                foreach (var item in deviceGroupList)
                                {
                                    DateTime beninTime = DateTime.Now.AddYears(100);
                                    DateTime finishTime = DateTime.Now.AddYears(-100);
                                    foreach (var real in item.Value)
                                    {
                                        if (real.order == orderInfo.OrderID)
                                        {
                                            if (real.startTime < beninTime)
                                            {
                                                beninTime = real.startTime;
                                            }
                                            if (real.endTime > finishTime)
                                            {
                                                finishTime = real.endTime;
                                            }
                                        }

                                    }
                                    if (finishTime < EndTime)
                                    {
                                        EndTime = finishTime;
                                        groupNum = item.Key;
                                    }
                                }
                                //判断出最佳组数后填写入数据库
                                List<JDJS_WMS_Order_Process_Scheduling_Table> tables = new List<JDJS_WMS_Order_Process_Scheduling_Table>();

                                //判断出最佳组数后填写入数据库


                                foreach (var item in deviceGroupList[groupNum])
                                {
                                    if (!deviceInfos.Contains(item))
                                    {
                                        JDJS_WMS_Order_Process_Scheduling_Table Scheduling = new JDJS_WMS_Order_Process_Scheduling_Table()
                                        {
                                            CncID = item.CncID,
                                            OrderID = item.order,
                                            ProcessID = item.process,
                                            StartTime = item.startTime,
                                            EndTime = item.endTime,
                                            isFlag = 1,
                                            WorkCount = 0
                                        };
                                        tables.Add(Scheduling);
                                        //JDJSWMS.JDJS_WMS_Order_Process_Scheduling_Table.Add(Scheduling);
                                        //JDJSWMS.SaveChanges();
                                    }

                                }
                                JDJSWMS.JDJS_WMS_Order_Process_Scheduling_Table.BulkInsert<JDJS_WMS_Order_Process_Scheduling_Table>(tables);
                                JDJSWMS.BulkSaveChanges();
                                mytran.Commit();
                            }
                            catch (Exception ex)
                            {

                                //Console.ReadKey();
                                mytran.Rollback();
                                return ex.Message;
                            }
                        }
                    }
                    return "ok";
                }
            }
        }



        /// <summary>
        /// 手动排产
        /// </summary>
        /// <param name="ProcessCncInfo"></param>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public static string ManualScheduling(Dictionary<int, List<int>> ProcessCncInfo, int orderID)
        {
            List<int> cncIDs = new List<int>();
            foreach (var item in ProcessCncInfo)
            {
                foreach (var real in item.Value)
                {
                    cncIDs.Add(real);
                }
            }
            using (JDJS_WMS_DB_USEREntities JDJSWMS = new JDJS_WMS_DB_USEREntities())
            {
                //var Queue = JDJSWMS.JDJS_WMS_Order_Queue_Table.Where(r => r.isFlag == 1).ToList();
                //foreach (var order in Queue)
                {

                    List<DeviceInfo> deviceInfosGroup = new List<DeviceInfo>();
                    using (System.Data.Entity.DbContextTransaction mytran = JDJSWMS.Database.BeginTransaction())
                    {
                        try
                        {
                            int OrderID = Convert.ToInt32(orderID);
                            DateTime time = DateTime.Now;
                            List<DeviceInfo> deviceInfos = new List<DeviceInfo>();
                            var deviceList = JDJSWMS.JDJS_WMS_Device_Info.ToList();
                            foreach (var item in deviceList)//获取车间所有的机床
                            {
                                //获取机床订单队列中的当前机床使用情况
                                var AllDeviceList = JDJSWMS.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.CncID == item.ID && r.EndTime >= time && r.isFlag != 0);
                                if (AllDeviceList.Count() > 0)
                                {
                                    Parallel.ForEach(AllDeviceList, new ParallelOptions() { MaxDegreeOfParallelism = 2 }, real =>
                                    {
                                        if (cncIDs.Contains(Convert.ToInt32(real.CncID)))
                                        {
                                            DeviceInfo device = new DeviceInfo();
                                            device.CncID = Convert.ToInt32(real.CncID);
                                            device.order = Convert.ToInt32(real.OrderID);
                                            device.process = Convert.ToInt32(real.ProcessID);

                                            device.Type = item.MachType.ToString();
                                            device.startTime = Convert.ToDateTime(real.StartTime);
                                            device.endTime = Convert.ToDateTime(real.EndTime);
                                            deviceInfos.Add(device);
                                        }
                                    });

                                }
                                else
                                {
                                    if (cncIDs.Contains(Convert.ToInt32(item.ID)))
                                    {
                                        DeviceInfo device = new DeviceInfo();
                                        device.CncID = item.ID;
                                        device.order = 0;
                                        device.process = 0;

                                        device.Type = item.MachType.ToString();
                                        device.startTime = time;
                                        device.endTime = time.AddMinutes(1);//如果机床没使用过，则将结束时间置位当前时间的后一分钟
                                        deviceInfos.Add(device);
                                    }
                                }

                            }


                            OrderInfo orderInfo = new OrderInfo();//获取订单信息
                            List<double> useTime = new List<double>();
                            orderInfo.OrderID = OrderID;
                            var isFlag = JDJSWMS.JDJS_WMS_Order_Queue_Table.Where(r => r.OrderID == OrderID).First();
                            orderInfo.isFlag = Convert.ToInt32(isFlag.isFlag);
                            var orderNum = JDJSWMS.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == OrderID).First();
                            orderInfo.OrderNum = orderNum.Order_Number;
                            orderInfo.OrderOutPut = orderNum.Product_Output;
                            int blankNum = 0;
                            int modelusfenmu = 1;
                            var blanks = JDJSWMS.JDJS_WMS_Order_Blank_Table.Where(r => r.OrderID == OrderID).FirstOrDefault();
                            if (blanks != null)
                            {

                                blankNum = Convert.ToInt32(blanks.BlackNumber);
                            }
                            var processInfo = JDJSWMS.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == OrderID && r.sign == 1).ToList();
                            foreach (var item in processInfo)//获取该订单的工序信息
                            {
                                useTime.Add(Convert.ToDouble(item.ProcessTime));
                                ProcessInfo process = new ProcessInfo();
                                process.id = Convert.ToInt32(item.ID);
                                process.processID = Convert.ToInt32(item.ProcessID);
                                process.processTime = Convert.ToDouble(item.ProcessTime);
                                process.deviceType = Convert.ToInt32(item.DeviceType);
                                process.modulus = Convert.ToInt32(item.Modulus);
                                process.ScheduNum = blankNum * modelusfenmu;
                                modelusfenmu = modelusfenmu * Convert.ToInt32(item.Modulus);

                                orderInfo.OrderProcessInfos.Add(process);
                            }
                            if (ProcessCncInfo.Count() != orderInfo.OrderProcessInfos.Count())
                            {
                                return "机床有误，请按工序数目输入机床";
                            }
                            //不同的组数对应不同的机床排布




                            {
                                //List<DeviceInfo> deviceInfosGroup = new List<DeviceInfo>();
                                foreach (var item in deviceInfos)
                                {
                                    deviceInfosGroup.Add(item);
                                }
                                List<int> deviceNum = new List<int>();

                                //计算合理的机床配比

                                {

                                    DateTime ProcessEndTimeFirst = time;
                                    int Proportion = 1;
                                    for (int k = 0; k < orderInfo.OrderProcessInfos.Count(); k++)//对于每一序
                                    {
                                        List<int> CncID = new List<int>();//该序使用的机床集合
                                        int processID = orderInfo.OrderProcessInfos[k].id;
                                        //下一句是获取改序在数据库中已经排产了多少件
                                        var ProcessNum = JDJSWMS.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.OrderID == orderInfo.OrderID && r.ProcessID == processID && r.isFlag != 0);
                                        int nu = ProcessNum.Count();
                                        for (int i = 0; i < orderInfo.OrderProcessInfos[k].ScheduNum - nu; i++)
                                        {

                                            //var info = deviceInfos.Where(r => r.Type == TableThereaxes.Status && r.endTime >= time);
                                            //var info = deviceGroupList[l].Where(r =>(r.endTime >= ProcessEndTimeFirst));
                                            var info = deviceInfosGroup;
                                            int cnc = 0;
                                            TimeSpan ts = TimeSpan.Parse("1000");
                                            DateTime start = DateTime.Now;
                                            Dictionary<int, DateTime> list = new Dictionary<int, DateTime>();//每台机床最晚的结束时间拿出来
                                            int deviceTypeID1 = orderInfo.OrderProcessInfos[k].deviceType;
                                            Parallel.ForEach(info, new ParallelOptions() { MaxDegreeOfParallelism = 2 }, eume =>
                                            {

                                                //var str1 = JDJSWMS.JDJS_WMS_Device_Status_Table.Where(r => r.ID == deviceTypeID1).FirstOrDefault();
                                                if (ProcessCncInfo[orderInfo.OrderProcessInfos[k].processID].Contains(eume.CncID))
                                                {
                                                    if (eume.Type == deviceTypeID1.ToString())
                                                    {
                                                        lock (lockObj)
                                                        {
                                                            if (!list.ContainsKey(eume.CncID))
                                                            {
                                                                list.Add(eume.CncID, eume.endTime);
                                                            }
                                                            else
                                                            {
                                                                if (list[eume.CncID] < eume.endTime)
                                                                {
                                                                    list[eume.CncID] = eume.endTime;
                                                                }
                                                            }
                                                        }

                                                    }
                                                }
                                            });
                                            //foreach (var eume in info)
                                            //{
                                            //    if (ProcessCncInfo[orderInfo.OrderProcessInfos[k].processID].Contains(eume.CncID))
                                            //    {
                                            //        int deviceTypeID1 = orderInfo.OrderProcessInfos[k].deviceType;

                                            //        if (eume.Type == deviceTypeID1.ToString())
                                            //        {
                                            //            if (!list.ContainsKey(eume.CncID))
                                            //            {
                                            //                list.Add(eume.CncID, eume.endTime);
                                            //            }
                                            //            else
                                            //            {
                                            //                if (list[eume.CncID] < eume.endTime)
                                            //                {
                                            //                    list[eume.CncID] = eume.endTime;
                                            //                }
                                            //            }
                                            //        }
                                            //    }
                                            //}
                                            foreach (var real in list)
                                            {
                                                TimeSpan t = real.Value - time;
                                                //TimeSpan t = real.Value - ProcessEndTimeFirst;
                                                if (t < ts)
                                                {
                                                    {
                                                        ts = t;
                                                        if (real.Value < time)
                                                        {
                                                            start = time;
                                                        }
                                                        else
                                                        {
                                                            start = real.Value;
                                                        }
                                                        cnc = real.Key;
                                                    }
                                                }
                                            }
                                            //if (!CncID.Contains(cnc))
                                            {
                                                CncID.Add(cnc);
                                            }
                                            DeviceInfo device = new DeviceInfo();
                                            device.CncID = cnc;

                                            int deviceTypeID = orderInfo.OrderProcessInfos[k].deviceType;

                                            device.Type = deviceTypeID.ToString();//机床种类需要修改
                                            device.order = orderInfo.OrderID;
                                            device.process = orderInfo.OrderProcessInfos[k].id;
                                            device.startTime = start;
                                            device.endTime = start.AddMinutes(orderInfo.OrderProcessInfos[k].processTime);
                                            if (i == orderInfo.OrderOutPut - nu - 1)//如果是下一件该序的最后一件
                                            {
                                                if (k < orderInfo.OrderProcessInfos.Count() - 1 && orderInfo.OrderProcessInfos[k + 1].processTime > orderInfo.OrderProcessInfos[k].processTime)
                                                {
                                                    Proportion = Convert.ToInt32(Math.Ceiling(orderInfo.OrderProcessInfos[k + 1].processTime / orderInfo.OrderProcessInfos[k].processTime));
                                                }
                                                else
                                                {
                                                    Proportion = 1;
                                                }
                                                ProcessEndTimeFirst = ProcessEndTimeFirst.AddMinutes(orderInfo.OrderProcessInfos[k].processTime * Proportion);
                                            }
                                            deviceInfosGroup.Add(device);



                                        }
                                    }
                                }

                            }
                            List<JDJS_WMS_Order_Process_Scheduling_Table> tables = new List<JDJS_WMS_Order_Process_Scheduling_Table>();

                            //判断出最佳组数后填写入数据库


                            foreach (var item in deviceInfosGroup)
                            {
                                if (!deviceInfos.Contains(item))
                                {
                                    JDJS_WMS_Order_Process_Scheduling_Table Scheduling = new JDJS_WMS_Order_Process_Scheduling_Table()
                                    {
                                        CncID = item.CncID,
                                        OrderID = item.order,
                                        ProcessID = item.process,
                                        StartTime = item.startTime,
                                        EndTime = item.endTime,
                                        isFlag = 1,
                                        WorkCount = 0

                                    };
                                    //tables.Add(Scheduling);
                                    JDJSWMS.JDJS_WMS_Order_Process_Scheduling_Table.Add(Scheduling);
                                    //JDJSWMS.SaveChanges();
                                }

                            }
                            //JDJSWMS.JDJS_WMS_Order_Process_Scheduling_Table.BulkInsert<JDJS_WMS_Order_Process_Scheduling_Table>(tables);
                            //JDJSWMS.BulkSaveChanges();
                            //foreach (var item in deviceInfosGroup)
                            //{
                            //    if (!deviceInfos.Contains(item))
                            //    {
                            //        JDJS_WMS_Order_Process_Scheduling_Table Scheduling = new JDJS_WMS_Order_Process_Scheduling_Table()
                            //        {
                            //            CncID = item.CncID,
                            //            OrderID = item.order,
                            //            ProcessID = item.process,
                            //            StartTime = item.startTime,
                            //            EndTime = item.endTime,
                            //            isFlag = 1

                            //        };
                            //        JDJSWMS.JDJS_WMS_Order_Process_Scheduling_Table.Add(Scheduling);
                            //        JDJSWMS.SaveChanges();
                            //    }

                            //}
                            JDJSWMS.SaveChanges();
                            mytran.Commit();
                        }
                        catch (Exception ex)
                        {

                            //Console.ReadKey();
                            mytran.Rollback();
                            return ex.Message;
                        }
                    }
                }
                return "ok";


            }




        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="GroupNum">组数</param>
        /// <param name="orderInfo">订单信息</param>
        /// <param name="useTime">时间比</param>
        /// <param name="Status">机床主键</param>
        /// <param name="Num">机床总数</param>
        /// <param name="result">结果</param>
        /// <returns></returns>
        private static bool CalculationProportion(int GroupNum, OrderInfo orderInfo, List<double> useTime, Dictionary<int, int> cncNums, List<int> result)
        {
            result.Clear();
            for (int j = 0; j < orderInfo.OrderProcessInfos.Count(); j++)
            {
                int Number = 0;
                Number = Convert.ToInt32(Math.Ceiling(useTime[j] * GroupNum));
                result.Add(Number);
            }
            if (GroupNum == 1)
            {
                return true;
            }
            else
            {
                for (int i = 0; i < result.Count(); i++)
                {
                    var cncnum = cncNums[i + 1];
                    if (result[i] > cncnum)
                    {
                        return false;
                    }
                }
                return true;
            }

        }

        /// <summary>
        /// 大批量排产
        /// </summary>
        /// <param name="ProcessCncInfo"></param>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public static string MassScheduling(Dictionary<int, List<int>> ProcessCncInfo, int orderID)
        {
            List<int> cncIDs = new List<int>();
            foreach (var item in ProcessCncInfo)
            {
                foreach (var real in item.Value)
                {
                    cncIDs.Add(real);
                }
            }
            using (JDJS_WMS_DB_USEREntities JDJSWMS = new JDJS_WMS_DB_USEREntities())
            {
                //var Queue = JDJSWMS.JDJS_WMS_Order_Queue_Table.Where(r => r.isFlag == 1).ToList();
                //foreach (var order in Queue)
                {

                    List<DeviceInfo> deviceInfosGroup = new List<DeviceInfo>();
                    using (System.Data.Entity.DbContextTransaction mytran = JDJSWMS.Database.BeginTransaction())
                    {
                        try
                        {
                            int OrderID = Convert.ToInt32(orderID);
                            DateTime time = DateTime.Now;
                            List<DeviceInfo> deviceInfos = new List<DeviceInfo>();
                            var deviceList = JDJSWMS.JDJS_WMS_Device_Info.ToList();
                            foreach (var item in deviceList)//获取车间所有的机床
                            {
                                //获取机床订单队列中的当前机床使用情况
                                var AllDeviceList = JDJSWMS.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.CncID == item.ID && r.EndTime >= time && r.isFlag != 0);
                                if (AllDeviceList.Count() > 0)
                                {
                                    Parallel.ForEach(AllDeviceList, new ParallelOptions() { MaxDegreeOfParallelism = 2 }, real =>
                                    {
                                        if (cncIDs.Contains(Convert.ToInt32(real.CncID)))
                                        {
                                            DeviceInfo device = new DeviceInfo();
                                            device.CncID = Convert.ToInt32(real.CncID);
                                            device.order = Convert.ToInt32(real.OrderID);
                                            device.process = Convert.ToInt32(real.ProcessID);

                                            device.Type = item.MachType.ToString();
                                            device.startTime = Convert.ToDateTime(real.StartTime);
                                            device.endTime = Convert.ToDateTime(real.EndTime);
                                            deviceInfos.Add(device);
                                        }
                                    });

                                }
                                else
                                {
                                    if (cncIDs.Contains(Convert.ToInt32(item.ID)))
                                    {
                                        DeviceInfo device = new DeviceInfo();
                                        device.CncID = item.ID;
                                        device.order = 0;
                                        device.process = 0;

                                        device.Type = item.MachType.ToString();
                                        device.startTime = time;
                                        device.endTime = time.AddMinutes(1);//如果机床没使用过，则将结束时间置位当前时间的后一分钟
                                        deviceInfos.Add(device);
                                    }
                                }

                            }


                            OrderInfo orderInfo = new OrderInfo();//获取订单信息
                            List<double> useTime = new List<double>();
                            orderInfo.OrderID = OrderID;
                            var isFlag = JDJSWMS.JDJS_WMS_Order_Queue_Table.Where(r => r.OrderID == OrderID).First();
                            orderInfo.isFlag = Convert.ToInt32(isFlag.isFlag);
                            var orderNum = JDJSWMS.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == OrderID).First();
                            orderInfo.OrderNum = orderNum.Order_Number;
                            orderInfo.OrderOutPut = orderNum.Product_Output;
                            int blankNum = 0;
                            int modelusfenmu = 1;
                            var blanks = JDJSWMS.JDJS_WMS_Order_Blank_Table.Where(r => r.OrderID == OrderID).FirstOrDefault();
                            if (blanks != null)
                            {

                                blankNum = Convert.ToInt32(blanks.BlackNumber);
                            }
                            var processInfo = JDJSWMS.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == OrderID && r.sign == 1).ToList();
                            foreach (var item in processInfo)//获取该订单的工序信息
                            {
                                useTime.Add(Convert.ToDouble(item.ProcessTime));
                                ProcessInfo process = new ProcessInfo();
                                process.id = Convert.ToInt32(item.ID);
                                process.processID = Convert.ToInt32(item.ProcessID);
                                process.processTime = Convert.ToDouble(item.ProcessTime) * (1.2);
                                process.deviceType = Convert.ToInt32(item.DeviceType);
                                process.modulus = Convert.ToInt32(item.Modulus);
                                process.ScheduNum = blankNum * modelusfenmu;
                                process.allTime = blankNum * modelusfenmu * Convert.ToDouble(item.ProcessTime) * (1.2);
                                modelusfenmu = modelusfenmu * Convert.ToInt32(item.Modulus);

                                orderInfo.OrderProcessInfos.Add(process);
                            }
                            if (ProcessCncInfo.Count() != orderInfo.OrderProcessInfos.Count())
                            {
                                return "机床有误，请按工序数目输入机床";
                            }
                            //不同的组数对应不同的机床排布




                            {
                                //List<DeviceInfo> deviceInfosGroup = new List<DeviceInfo>();
                                foreach (var item in deviceInfos)
                                {
                                    deviceInfosGroup.Add(item);
                                }
                                List<int> deviceNum = new List<int>();

                                //计算合理的机床配比

                                {

                                    DateTime ProcessEndTimeFirst = time;
                                    for (int k = 0; k < orderInfo.OrderProcessInfos.Count(); k++)//对于每一序
                                    {
                                        List<int> CncID = new List<int>();//该序使用的机床集合
                                        int processID = orderInfo.OrderProcessInfos[k].id;
                                        //下一句是获取改序在数据库中已经排产了多少件
                                        var ProcessNum = JDJSWMS.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.OrderID == orderInfo.OrderID && r.ProcessID == processID && r.isFlag != 0);
                                        if (!(ProcessNum.Count() > 0))
                                        {
                                            var info = deviceInfosGroup;
                                            double spantime = (orderInfo.OrderProcessInfos[k].allTime) / Convert.ToDouble(ProcessCncInfo[orderInfo.OrderProcessInfos[k].processID].Count());
                                            foreach (var item in ProcessCncInfo[orderInfo.OrderProcessInfos[k].processID])
                                            {
                                                DateTime startTime = DateTime.Now;
                                                var listInfo = info.Where(r => r.CncID == item);
                                                foreach (var eume in listInfo)
                                                {

                                                    {
                                                        if (startTime < eume.endTime)
                                                        {
                                                            startTime = eume.endTime;
                                                        }


                                                    }
                                                }

                                                DeviceInfo device = new DeviceInfo();
                                                device.CncID = item;

                                                int deviceTypeID = orderInfo.OrderProcessInfos[k].deviceType;

                                                device.Type = deviceTypeID.ToString();//机床种类需要修改
                                                device.order = orderInfo.OrderID;
                                                device.process = orderInfo.OrderProcessInfos[k].id;
                                                device.startTime = startTime;
                                                device.endTime = startTime.AddMinutes(spantime);
                                                deviceInfosGroup.Add(device);
                                            }


                                        }




                                    }
                                }

                            }
                            foreach (var item in deviceInfosGroup)
                            {
                                if (!deviceInfos.Contains(item))
                                {
                                    JDJS_WMS_Order_Process_Scheduling_Table Scheduling = new JDJS_WMS_Order_Process_Scheduling_Table()
                                    {
                                        CncID = item.CncID,
                                        OrderID = item.order,
                                        ProcessID = item.process,
                                        StartTime = item.startTime,
                                        EndTime = item.endTime,
                                        isFlag = 1,
                                        WorkCount = 0,
                                        SystemCount =0
                                    };
                                    JDJSWMS.JDJS_WMS_Order_Process_Scheduling_Table.Add(Scheduling);
                                    JDJSWMS.SaveChanges();
                                }

                            }
                            mytran.Commit();
                        }
                        catch (Exception ex)
                        {

                            //Console.ReadKey();
                            mytran.Rollback();
                            return ex.Message;
                        }
                    }
                }
                return "ok";


            }




        }
    }
}