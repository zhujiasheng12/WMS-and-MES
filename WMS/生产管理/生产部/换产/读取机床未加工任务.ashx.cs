﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.生产部.换产
{
    /// <summary>
    /// 读取机床未加工任务 的摘要说明
    /// </summary>
    public class 读取机床未加工任务 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            int cncID = int.Parse(context.Request["cncId"]);
            List<TaskInfo> taskInfos = new List<TaskInfo>();
            using (JDJS_WMS_DB_USEREntities wms = new JDJS_WMS_DB_USEREntities())
            {
                var cnc = wms.JDJS_WMS_Device_Info.Where(r => r.ID == cncID).FirstOrDefault();
                if (cnc != null)
                {
                    var type = wms.JDJS_WMS_Device_Type_Info .Where (r=>r.ID ==cnc.MachType ).FirstOrDefault ();
                    var tasks = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.CncID == cncID && r.isFlag == 1);
                    foreach (var task in tasks)
                    {
                        TaskInfo taskInfo = new TaskInfo();
                        taskInfo.id = task.ID;
                        taskInfo.cncNum = cnc.MachNum;
                        taskInfo.deviceType = type.Type;
                        taskInfo.endTime = Convert.ToDateTime(task.EndTime);
                        taskInfo.endTimeStr = task.EndTime.ToString().Substring(0, task.EndTime.ToString().LastIndexOf(':'));
                        taskInfo.startTime = Convert.ToDateTime(task.StartTime);
                        taskInfo.startTimeStr = task.StartTime.ToString().Substring(0, task.StartTime.ToString().LastIndexOf(':'));
                        int processID =Convert.ToInt32 ( task.ProcessID) ;
                        taskInfo.processID =processID ;
                        var process = wms.JDJS_WMS_Order_Process_Info_Table .Where (r=>r.ID ==processID ).FirstOrDefault ();
                        taskInfo.ProcessNum =Convert.ToInt32 ( process .ProcessID) ;
                        int fixID = Convert.ToInt32(process.JigType);
                        string fixName = "";
                        var fixTable = wms.JDJS_WMS_Device_Status_Table.Where(r => r.ID == fixID).FirstOrDefault();
                        if (fixTable != null)
                        {
                            fixName = fixTable.Status + "(" + fixTable.explain + ")";
                        }
                        taskInfo.jiaType = fixName;
                        taskInfo.nonCuttingTime = process.NonCuttingTime == null ? "0Min" : process.NonCuttingTime.ToString() + "Min";
                        taskInfo.orderID =Convert .ToInt32 ( task.OrderID);
                        var order = wms.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == task.OrderID).FirstOrDefault();
                        taskInfo.orderName = order.Product_Name;
                        taskInfo.orderNum = order.Order_Number;
                        taskInfo.projectName = order.ProjectName == null ? "" : order.ProjectName;
                        var oldTask= taskInfos.Where(r => r.processID == taskInfo.processID).FirstOrDefault ();
                        if (oldTask == null)
                        {
                            taskInfo.taskNum = 1;
                            taskInfo.taskIDs = new List<int>();
                            taskInfo.taskIDs.Add(taskInfo.id);
                            taskInfos.Add(taskInfo);
                        }
                        else
                        {
                            oldTask.taskNum++;
                            oldTask.taskIDs.Add(taskInfo.id);
                        }
                    }
                    
                }
            }
            taskInfos = taskInfos.OrderBy(r => r.startTime).ToList();
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var json = serializer.Serialize(new { code = 0, data = taskInfos });
            context.Response.Write(json);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
    public class TaskInfo
    {
        /// <summary>
        /// 加工单元主键ID
        /// </summary>
        public int id;
        /// <summary>
        /// 订单主键ID
        /// </summary>
        public int orderID;
        /// <summary>
        /// 订单号
        /// </summary>
        public string orderNum;
        /// <summary>
        /// 产品名称
        /// </summary>
        public string orderName;
        /// <summary>
        /// 项目名称
        /// </summary>
        public string projectName;
        /// <summary>
        /// 工序号
        /// </summary>
        public int ProcessNum;
        /// <summary>
        /// 工序主键ID
        /// </summary>
        public int processID;
        /// <summary>
        /// 机床类型
        /// </summary>
        public string deviceType;
        /// <summary>
        /// 治具类型
        /// </summary>
        public string jiaType;
        /// <summary>
        /// 机床编号
        /// </summary>
        public string cncNum;
        /// <summary>
        /// 加工预计开始时间字符串
        /// </summary>
        public string startTimeStr;
        /// <summary>
        /// 加工预计开始时间（排序使用）
        /// </summary>
        public DateTime startTime;
        /// <summary>
        /// 加工预计结束时间（排序用）
        /// </summary>
        public DateTime endTime;
        /// <summary>
        /// 加工预计结束时间
        /// </summary>
        public string endTimeStr;
        /// <summary>
        /// 辅助时间
        /// </summary>
        public string nonCuttingTime;

        /// <summary>
        /// 该工序工件数量
        /// </summary>
        public int taskNum;
        /// <summary>
        /// 单件ID集合
        /// </summary>
        public List<int> taskIDs;

    }
}