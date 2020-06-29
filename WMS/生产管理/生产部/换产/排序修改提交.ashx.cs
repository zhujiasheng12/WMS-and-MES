﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace WebApplication2.生产管理.生产部.换产
{
    /// <summary>
    /// 排序修改提交 的摘要说明
    /// </summary>
    public class 排序修改提交 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                var taskIDss = context.Request.Form["taskIDss"].Split(',');

                List<int> sortTaskIDs = new List<int>();
                for (int i = 0; i < taskIDss.Count(); i++)
                {
                    if (taskIDss[i] != "") {
                        var taskIDssInt = Convert.ToInt32(taskIDss[i]);
                        sortTaskIDs.Add(taskIDssInt);
                    }
           
                }
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                List<TaskSortInfo> taskSortInfos = new List<TaskSortInfo>();
                //sortTaskIDs = serializer.Deserialize<List<int>>("");//排序后的单个任务ID
                DateTime startTime = DateTime.Now.AddYears(1000);
                using (JDJS_WMS_DB_USEREntities wms = new JDJS_WMS_DB_USEREntities())
                {
                    using (System.Data.Entity.DbContextTransaction mytran = wms.Database.BeginTransaction())
                    {
                        try
                        {
                            foreach (var item in sortTaskIDs)
                            {
                                var task = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.ID == item).FirstOrDefault();
                                if (task == null || task.isFlag == 2 || task.isFlag == 3)
                                {
                                    throw new Exception("该任务加工中，无法修改生产顺序，请重新加载生产数据");
                                }
                                TaskSortInfo taskSort = new TaskSortInfo() { id = item, startTime = Convert.ToDateTime(task.StartTime), endTime = Convert.ToDateTime(task.EndTime) };
                                taskSortInfos.Add(taskSort);
                                if (startTime > Convert.ToDateTime(task.StartTime))
                                {
                                    startTime = Convert.ToDateTime(task.StartTime);
                                }

                            }

                            //开始时间拿到了之后
                            for (int i = 0; i < taskSortInfos.Count(); i++)
                            {
                                TimeSpan span = taskSortInfos[i].endTime - taskSortInfos[i].startTime;
                                taskSortInfos[i].startTime = startTime;
                                taskSortInfos[i].endTime = startTime + span;
                                startTime = startTime + span;
                            }

                            foreach (var item in taskSortInfos)
                            {
                                var task = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.ID == item.id).FirstOrDefault();
                                if (task == null || task.isFlag == 2 || task.isFlag == 3)
                                {
                                    throw new Exception("该任务加工中，无法修改生产顺序，请重新加载生产数据");
                                }
                                task.StartTime = item.startTime;
                                task.EndTime = item.endTime;
                            }
                            wms.SaveChanges();
                            mytran.Commit();
                            context.Response.Write("ok");
                            return;
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
            catch (Exception ex)
            {
                context.Response.Write(ex.Message);
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
    public class TaskSortInfo
    {
        public int id;
        public DateTime startTime;
        public DateTime endTime;
    }
}