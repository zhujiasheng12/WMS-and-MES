using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.工程部
{
    /// <summary>
    /// 写治具数量 的摘要说明
    /// </summary>
    public class 写治具数量 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
                {
                int OrderID = int.Parse(context.Request["orderId"]);
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                List<string> str = new List<string>();

                using (JDJS_WMS_DB_USEREntities  wms = new JDJS_WMS_DB_USEREntities ())
                {
                    using (System.Data.Entity.DbContextTransaction date = wms.Database.BeginTransaction())
                    {
                        try
                        {
                            List<int> processid = new List<int>();
                            var process = wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == OrderID && r.sign != 0);
                            foreach (var item in process)
                            {
                                if (!processid.Contains(Convert.ToInt32(item.ProcessID)))
                                {
                                    processid.Add(Convert.ToInt32(item.ProcessID));


                                }
                            }
                            processid.Sort();
                            List<int> ProcessId = new List<int>();
                            foreach (var item in processid)
                            {
                                var processed = wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == OrderID && r.sign != 0 && r.ProcessID == item);
                                ProcessId.Add(Convert.ToInt32(processed.FirstOrDefault().ID));
                            }
                            foreach (var item in ProcessId)
                            {
                                List<int> CncID = new List<int>();

                                var mach = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.ProcessID == item && r.isFlag != 0);
                                foreach (var real in mach)
                                {
                                    if (!CncID.Contains(Convert.ToInt32(real.CncID)))
                                    {
                                        CncID.Add(Convert.ToInt32(real.CncID));
                                    }
                                }
                                var fix = wms.JDJS_WMS_Order_Fixture_Manager_Table.Where(r => r.ProcessID == item);
                                fix.FirstOrDefault().FixtureNumber = CncID.Count();
                                wms.SaveChanges();
                                str.Add(CncID.Count().ToString());
                            }
                            wms.SaveChanges();
                            date.Commit();
                        }
                        catch(Exception ex)
                        {
                            date.Rollback();
                            context.Response.Write(ex.Message);
                        }
                    }

                }
                var json = serializer.Serialize(str);
                context.Response.Write("ok");
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