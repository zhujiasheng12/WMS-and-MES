using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.生产部.换产
{
    /// <summary>
    /// 读取机台当前加工任务 的摘要说明
    /// </summary>
    public class 读取机台当前加工任务 : IHttpHandler
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
                    var type = wms.JDJS_WMS_Device_Type_Info.Where(r => r.ID == cnc.MachType).FirstOrDefault();
                    var tasks = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.CncID == cncID && r.isFlag == 2);
                    if (tasks.Count() > 0)
                    {
                        var task = tasks.First();
                        TaskInfo taskInfo = new TaskInfo();
                        taskInfo.id = task.ID;
                        taskInfo.cncNum = cnc.MachNum;
                        taskInfo.deviceType = type.Type;
                        taskInfo.endTime = Convert.ToDateTime(task.EndTime);
                        taskInfo.endTimeStr = task.EndTime.ToString().Substring(0, task.EndTime.ToString().LastIndexOf(':'));
                        taskInfo.startTime = Convert.ToDateTime(task.StartTime);
                        taskInfo.startTimeStr = task.StartTime.ToString().Substring(0, task.StartTime.ToString().LastIndexOf(':'));
                        int processID = Convert.ToInt32(task.ProcessID);
                        taskInfo.processID = processID;
                        var process = wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == processID).FirstOrDefault();
                        taskInfo.ProcessNum = Convert.ToInt32(process.ProcessID);
                        int fixID = Convert.ToInt32(process.JigType);
                        string fixName = "";
                        var fixTable = wms.JDJS_WMS_Device_Status_Table.Where(r => r.ID == fixID).FirstOrDefault();
                        if (fixTable != null)
                        {
                            fixName = fixTable.Status + "(" + fixTable.explain + ")";
                        }
                        taskInfo.jiaType = fixName;
                        taskInfo.nonCuttingTime = process.NonCuttingTime == null ? "0Min" : process.NonCuttingTime.ToString() + "Min";
                        taskInfo.orderID = Convert.ToInt32(task.OrderID);
                        var order = wms.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == task.OrderID).FirstOrDefault();
                        taskInfo.orderName = order.Product_Name;
                        taskInfo.orderNum = order.Order_Number;
                        taskInfo.projectName = order.ProjectName == null ? "" : order.ProjectName;
                        var oldTask = taskInfos.Where(r => r.processID == taskInfo.processID).FirstOrDefault();
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
}