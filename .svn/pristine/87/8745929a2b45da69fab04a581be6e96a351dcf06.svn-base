using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.生产部.产能计数
{
    /// <summary>
    /// 修改机床工件计数 的摘要说明
    /// </summary>
    public class 修改机床工件计数 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var id = int.Parse(context.Request["id"]);
            var num = int.Parse(context.Request["num"]);
            using (JDJS_WMS_DB_USEREntities wms = new JDJS_WMS_DB_USEREntities())
            {
                using (System.Data.Entity.DbContextTransaction mytran = wms.Database.BeginTransaction())
                {
                    try
                    {
                        var work = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.ID == id);
                        if (work.Count() < 1)
                        {
                            context.Response.Write("该加工任务不存在");
                            return;
                        }
                        int orderId =Convert.ToInt32 ( work.FirstOrDefault().OrderID) ;
                        int processId =Convert.ToInt32 ( work.FirstOrDefault().ProcessID);
                        int blankCount = 0;
                        int processfirstId = 0;
                        var processfirst = wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == orderId && r.ProcessID == 1).FirstOrDefault();
                        if (processfirst != null)
                        {
                            processfirstId = processfirst.ID;
                        }
                        var blank = wms.JDJS_WMS_Warehouse_OutBlank_History_Table.Where(r => r.Process == processfirstId);
                        foreach (var item in blank)
                        {
                            blankCount += Convert.ToInt32(item.OutNum);
                        }
                        var process = wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID== processId);
                        int processNum =Convert.ToInt32 ( process.FirstOrDefault().ProcessID);

                        var processEs = wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == orderId && r.sign != 0&&r.ProcessID <=processNum).OrderBy (r=>r.ProcessID );
                        foreach (var item in processEs)
                        {
                            blankCount = blankCount *Convert.ToInt32 ( item.Modulus);
                        }
                        int all = 0;
                        var works = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.ProcessID == processId && r.isFlag != 0&&r.ID !=id);
                        foreach (var item in works)
                        {
                            
                            all += (item.WorkCount == null ? 0 : Convert.ToInt32(item.WorkCount));
                            
                        }

                        if (all + num > blankCount)
                        {
                            context.Response.Write("修改后成品数量大于毛坯数量，请确认后再试！");
                            return;
                        }



                        foreach (var item in work)
                        {
                            item.WorkCount = num;
                        }
                        wms.SaveChanges();
                        mytran.Commit();
                        context.Response.Write("ok");

                    }
                    catch (Exception ex)
                    {
                        mytran.Rollback();
                        context.Response.Write(ex.Message);
                    }
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
}