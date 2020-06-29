using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.生产部.换产
{
    /// <summary>
    /// 暂停机床当前加工任务 的摘要说明
    /// </summary>
    public class 暂停机床当前加工任务 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            int machId = int.Parse(context.Request["machId"]);
            var str = ScanMachDownProofingPauseSubmission(machId);
            context.Response.Write(str);
        }

        /// <summary>
        /// 大批量暂停提交
        /// </summary>
        /// <param name="MachInfo"></param>
        /// <param name="PersonIDStr"></param>
        /// <returns></returns>
        public static string ScanMachDownProofingPauseSubmission(int MachID)
        {
            try
            {
                
                using (JDJS_WMS_DB_USEREntities  jdjs = new  JDJS_WMS_DB_USEREntities ())
                {
                    using (System.Data.Entity.DbContextTransaction mytran = jdjs.Database.BeginTransaction())
                    {
                        try
                        {
                            var mach = jdjs.JDJS_WMS_Device_Info.Where(r => r.ID == MachID);
                            if (mach.Count() < 1)
                            {
                                return "该机床不存在，请检查二维码是否正确";
                            }

                            
                            DateTime now = DateTime.Now;
                            {
                                //var work = jdjs.JDJS_WMS_Device_Working_Process_Table.Where(r => r.Flag == 2 && r.CncID == MachID);
                                //foreach (var item in work)
                                //{
                                //    item.Flag = -2;
                                //    item.EndTime = now;
                                //}
                                int orderID = -1;
                                int processId = -1;
                                var schedus2 = jdjs.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.CncID == MachID && r.isFlag == 2);
                                if (schedus2.Count() > 0)
                                {
                                    var work = jdjs.JDJS_WMS_Device_Working_Process_Table.Where(r => r.CncID == MachID && r.Flag != -2);
                                    foreach (var item in work)
                                    {
                                        item.Flag = -2;
                                        item.EndTime = DateTime.Now;
                                    }
                                    var porcessID = Convert.ToInt32(schedus2.FirstOrDefault().ProcessID);
                                    var porcessInfo = jdjs.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == porcessID);
                                    orderID = Convert.ToInt32(porcessInfo.FirstOrDefault().OrderID);

                                    processId = Convert.ToInt32(porcessInfo.FirstOrDefault().ID);
                                    schedus2.FirstOrDefault().isFlag = 1;
                                    //schedus2.FirstOrDefault().EndTime = now;
                                }
                                


                                
                            }


                            jdjs.SaveChanges();
                            mytran.Commit();
                            return "ok";
                        }
                        catch (Exception ex)
                        {
                            mytran.Rollback();
                            return ex.Message;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
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