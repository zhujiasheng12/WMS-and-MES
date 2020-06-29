using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Kanban.设备监控
{
    /// <summary>
    /// 机台实时物料 的摘要说明
    /// </summary>
    public class 机台实时物料 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
         
            //机台实时物料  
            Int32 workId = 0;
            if (Int32.TryParse(context.Request["workId"], out workId))
            {

            }
            else
            {
                return;
            }

            int LocationID = workId;
            List<string> materials = new List<string>();
            using (JDJS_WMS_DB_USEREntities  wms = new  JDJS_WMS_DB_USEREntities ())
            {
                var devices = wms.JDJS_WMS_Device_Info.Where(r => r.Position == LocationID);
                foreach (var item in devices)
                {
                    string cncNUm = item.MachNum;
                    int cncID = Convert.ToInt32(item.ID);
                    var real = wms.JDJS_WMS_Device_RealTime_Data.Where(r => r.CncID == cncID).FirstOrDefault();
                    string str = "";
                    if (real != null)
                    {


                        if (real.ProgState == 0 || real.ProgState == 3)
                        {
                            var task = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.CncID == cncID && r.isFlag == 1);
                            if (task.Count() > 0)
                            {
                                str = "换料";

                                var tasknext = task.OrderBy(r => r.StartTime);
                                var over = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.CncID == cncID && r.isFlag == 1).OrderByDescending(r => r.EndTime);
                                if (over.Count() > 0)
                                {
                                    if (tasknext.FirstOrDefault().ProcessID != over.FirstOrDefault().ProcessID)
                                    {
                                        DateTime time3 = Convert.ToDateTime(over.FirstOrDefault().EndTime);
                                        var tools = wms.JDJS_WMS_Device_Tool_History_Table.Where(r => r.CncID == cncID && r.Time > time3);
                                        if (tools.Count() < 1)
                                        {
                                            str = "备刀换刀";
                                        }
                                    }
                                }
                                materials.Add(cncNUm + "等待" + str);
                            }
                        }
                    }
                }
            }
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var json = serializer.Serialize(materials);
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