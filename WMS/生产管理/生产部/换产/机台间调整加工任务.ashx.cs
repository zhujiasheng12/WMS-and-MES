using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.生产部.换产
{
    /// <summary>
    /// 机台间调整加工任务 的摘要说明
    /// </summary>
    public class 机台间调整加工任务 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                var id = int.Parse(context.Request["id"]);
                var machId = int.Parse(context.Request["machId"]);
                string errMsg = "";
                if (AddTask(id, machId, ref errMsg))
                {
                    context.Response.Write("ok");
                }
                else
                {
                    context.Response.Write(errMsg);
                }
            }
            catch (Exception ex)
            {
                context.Response.Write(ex.Message );
            }
        }
        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteTask(int id, ref string errMsg)
        {
            try
            {
                using (JDJS_WMS_DB_USEREntities wms = new JDJS_WMS_DB_USEREntities())
                {
                    using (System.Data.Entity.DbContextTransaction mytran = wms.Database.BeginTransaction())
                    {
                        try
                        {
                            var work = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.ID == id).FirstOrDefault();
                            if (work == null)
                            {
                                mytran.Rollback();
                                errMsg = "该任务不存在";
                                return false;
                            }
                            DateTime time = Convert.ToDateTime(work.StartTime);
                            TimeSpan timespan = Convert.ToDateTime(work.EndTime) - Convert.ToDateTime(work.StartTime);
                            var works = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.CncID == work.CncID && r.isFlag == 1 && r.StartTime > time);
                            foreach (var item in works)
                            {
                                DateTime startTime = Convert.ToDateTime(item.StartTime);
                                item.StartTime = startTime - timespan;
                                DateTime endTime = Convert.ToDateTime(item.EndTime);
                                item.EndTime = endTime - timespan;
                            }

                            wms.JDJS_WMS_Order_Process_Scheduling_Table.Remove(work);
                            wms.SaveChanges();
                            mytran.Commit();
                            errMsg = "ok";
                            return true;
                        }
                        catch (Exception ex)
                        {
                            errMsg = ex.Message;
                            mytran.Rollback();
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return false;
            }
        }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <returns></returns>
        public bool AddTask(int id, int machId, ref string errMsg)
        {
            try
            {
                using (JDJS_WMS_DB_USEREntities wms = new JDJS_WMS_DB_USEREntities())
                {
                    using (System.Data.Entity.DbContextTransaction mytran = wms.Database.BeginTransaction())
                    {
                        try
                        {
                            var work = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.ID == id).FirstOrDefault();
                            if (work == null)
                            {
                                mytran.Rollback();
                                errMsg = "该加工任务不存在！";
                                return false;
                            }
                            if (work.isFlag != 1)
                            {
                                mytran.Rollback();
                                errMsg = "该加工任务不满足替换条件！";
                                return false;
                            }
                            var device = wms.JDJS_WMS_Device_Info.Where(r => r.ID == machId).FirstOrDefault();
                            if (device == null)
                            {
                                mytran.Rollback();
                                errMsg = "该机床不存在！";
                                return false;
                            }
                            TimeSpan span = Convert.ToDateTime(work.EndTime) - Convert.ToDateTime(work.StartTime);
                            int orderId = Convert.ToInt32(work.OrderID);
                            int processId = Convert.ToInt32(work.ProcessID);
                            int count = Convert.ToInt32(work.WorkCount);
                            DateTime time = DateTime.Now;
                            var works = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.CncID == machId && (r.isFlag == 1 || r.isFlag == 2));
                            if (works.Count() > 0)
                            {
                                time = Convert.ToDateTime(works.OrderByDescending(r => r.EndTime).First().EndTime);
                            }
                            JDJS_WMS_Order_Process_Scheduling_Table jd = new JDJS_WMS_Order_Process_Scheduling_Table()
                            {
                                CncID =machId ,
                                EndTime =time+span ,
                                isFlag =1,
                                OrderID =orderId ,
                                ProcessID =processId ,
                                StartTime =time,
                                WorkCount =count 
                            };
                            wms.JDJS_WMS_Order_Process_Scheduling_Table.Add(jd);


                            DateTime time1 = Convert.ToDateTime(work.StartTime);
                            TimeSpan timespan = Convert.ToDateTime(work.EndTime) - Convert.ToDateTime(work.StartTime);
                            var works1 = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.CncID == work.CncID && r.isFlag == 1 && r.StartTime > time1);
                            foreach (var item in works1)
                            {
                                DateTime startTime = Convert.ToDateTime(item.StartTime);
                                item.StartTime = startTime - timespan;
                                DateTime endTime = Convert.ToDateTime(item.EndTime);
                                item.EndTime = endTime - timespan;
                            }

                            wms.JDJS_WMS_Order_Process_Scheduling_Table.Remove(work);

                            wms.SaveChanges();
                            mytran.Commit();
                            errMsg = "ok";
                            return true;


                        }
                        catch (Exception ex)
                        {
                            mytran.Rollback();
                            errMsg = ex.Message;
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return false;
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