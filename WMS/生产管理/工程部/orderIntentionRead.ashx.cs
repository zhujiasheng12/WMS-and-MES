﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.市场部
{
    /// <summary>
    /// orderEntry 的摘要说明
    /// </summary>
    public class orderIntentionRead : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            using(JDJS_WMS_DB_USEREntities entities =new JDJS_WMS_DB_USEREntities())
            {
                var page = int.Parse(context.Request["page"]);
                var limit = int.Parse(context.Request["limit"]);
                var rows = from orders in entities.JDJS_WMS_Order_Entry_Table.Where(r=>r.Intention!=5&r.Intention!=6)
                           from intention in entities.JDJS_WMS_Order_Intention_History_Table
                           from client in entities.JDJS_WMS_Order_Guide_Schedu_Table
                           where orders.Order_ID == intention.OrderID & client.OrderID == orders.Order_ID
                           select new
                           {
                               orders.Order_ID,
                               orders.Order_Leader,
                               orders.Order_Number,
                               orders.Order_Plan_End_Time,
                               orders.Order_Plan_Start_Time,
                               orders.Order_State,
                               orders.Product_Material,
                               orders.Product_Name,
                               orders.Product_Output,
                               orders.virtualProgPersId,
                               orders.virtualReturnTime,
                               orders.Intention,
                               orders.Engine_Program_Manager,
                               orders.Engine_Program_ManagerId,
                               orders.Engine_Status,
                               orders.Engine_Technology_Manager,
                               orders.Engine_Technology_ManagerId,
                               orders.Order_Actual_End_Time,
                               orders.Order_Actual_Start_Time,
                               intention.examineResult,
                               orders.ProjectName ,
                               orders.Priority,
                               client.ClientName
                           };
                
                
                            
               List< OrderRead >order = new List<OrderRead>();
                foreach (var item in rows)
                {
                    var flag = entities.JDJS_WMS_Order_Queue_Table.Where(r => r.OrderID == item.Order_ID);
                    //string  Order_State="0";
                    //if (flag.Count() > 0)
                    //{
                    //   Order_State = flag.First().isFlag.ToString();
                    //}
                    var virtualProgPers = "";
                    var virtualProgPersRow = entities.JDJS_WMS_Staff_Info.Where(r => r.id == item.virtualProgPersId);
                    if (virtualProgPersRow.Count() > 0)
                    {
                        virtualProgPers = virtualProgPersRow.First().staff;
                    }
                    PathInfo pathInfo = new PathInfo();
                    var folderPath = Path.Combine(pathInfo.upLoadPath(), item.Order_Number, "虚拟加工方案文档");
                    DirectoryInfo directory = new DirectoryInfo(folderPath);
                    if (!directory.Exists)
                    {
                        directory.Create();
                    }
                    FileInfo[] files = directory.GetFiles();
                    var fileName = "";
                    if (files.Count() > 0)
                    {
                        fileName = files[0].Name;
                    }
                    //var endTimeLast = "";
                    //if (entities.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.OrderID == item.Order_ID).Count() > 0)
                    //{
                    //     endTimeLast = entities .JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.OrderID == item.Order_ID).OrderByDescending(r => r.EndTime).First().EndTime.ToString();

                    //}


                    var endTimeLast = "";
                    double time = 0;
                    var virtualReturnTimeList = entities.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == item.Order_ID && r.sign == 0);
                    foreach (var process in virtualReturnTimeList)
                    {
                        if (process.ProcessTime != null)
                        {
                            time += Convert.ToDouble(process.ProcessTime);
                        }
                    }

                    endTimeLast = MinuteToHour(time);
                    order.Add(new OrderRead
                    {
                        Engine_Program_Manager = item.Engine_Program_Manager,
                        Order_Number = item.Order_Number.ToString(),
                        projectName =item.ProjectName ==null?"":item.ProjectName ,
                        Engine_Status = item.Engine_Status,
                        Order_Leader = item.Order_Leader,
                        Order_ID = item.Order_ID.ToString(),
                        Product_Drawing = "110",
                        Product_Material = item.Product_Material,
                        Product_Name = item.Product_Name,
                        Product_Output = item.Product_Output.ToString(),
                        Order_Plan_End_Time = item.Order_Plan_End_Time.ToString(),
                        Order_Plan_Start_Time = item.Order_Plan_Start_Time.ToString(),
                        Order_State = item.Intention.ToString(),
                        orderState =Convert.ToInt32 (item.Intention ),
                        Engine_Technology_Manager = item.Engine_Technology_Manager,
                        virtualProgPers = virtualProgPers,
                        virtualPPT = fileName,
                        virtualReturnTime = endTimeLast,
                        examineResult =item.examineResult,
                        Priority = item.Priority.ToString(),
                        clientName=item.ClientName
                    }) ;
                    
      
                }
                order = order.OrderBy(r => r.orderState).ToList();
                var key = context.Request["key"];
                if (key != null)
                {
                    var search = order.Where(r => r.Order_Leader == key | r.Order_Number == key | r.Order_State == key | r.Product_Name == key |
                  r.Product_Material == key);

                    var orderKey = search.Skip((page - 1) * limit).Take(limit);
                    var modelKey = new { msg = "", code = 0, count = search.Count(), data = orderKey };
                    var jsonKey = serializer.Serialize(modelKey);
                    context.Response.Write(jsonKey);
                    return;
                }
                var order1 = order.Skip((page - 1) * limit).Take(limit);
                var model = new { msg = "", code = 0, count = rows.Count(), data = order1};
                var json = serializer.Serialize(model);
                context.Response.Write(json);
            }
        }

        #region 分钟转换小时 SecondToHour
        /// <summary>
        ///分钟转换小时
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string MinuteToHour(double time)
        {
            int hour = 0;
            int minute = 0;

            minute = Convert.ToInt32(time);


            if (minute > 60)
            {
                hour = minute / 60;
                minute = minute % 60;
            }
            return (hour + "小时" + minute + "分钟"
                );
        }
        #endregion

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
    class OrderRead
    {
        public string Order_ID;
        public string Order_Number;
        public string projectName;
        public string Order_Leader;
        public string Product_Name;
        public string Product_Material;
        public string Product_Drawing;
        public string Product_Output;
        public string Order_Plan_Start_Time;
        public string Order_Plan_End_Time;
        public string Order_State;
        public int orderState;
        public string Engine_Program_Manager;
        public string Engine_Technology_Manager;
        public string Engine_Status;
        public string virtualProgPers;
        public string virtualPPT;
        public string virtualReturnTime;
        public string examineResult;
        public string Priority;
        public string clientName;
    }
}