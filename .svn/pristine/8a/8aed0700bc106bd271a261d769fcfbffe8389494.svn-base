using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Kanban.维修
{
    /// <summary>
    /// 现场报修维修饼图1 的摘要说明
    /// </summary>
    public class 现场报修维修饼图1 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            int LocationID = int.Parse(context.Request["workId"]);
            int FixingNum = 0;
            int FixOverWaitConfirmNum = 0;
            using (JDJS_WMS_DB_USEREntities  wms = new JDJS_WMS_DB_USEREntities ())
            {
                var Status11Code = wms.JDJS_WMS_Device_Maintenance_Status.Where(r => r.DescID == 11).First();
                var Status14Code = wms.JDJS_WMS_Device_Maintenance_Status.Where(r => r.DescID == 14).First();
                var Status16Code = wms.JDJS_WMS_Device_Maintenance_Status.Where(r => r.DescID == 16).First();
                var Status19Code = wms.JDJS_WMS_Device_Maintenance_Status.Where(r => r.DescID == 19).First();
                var Status1Code = wms.JDJS_WMS_Device_Maintenance_Status.Where(r => r.DescID == 1).First();
                var Status0Code = wms.JDJS_WMS_Device_Maintenance_Status.Where(r => r.DescID == 0).First();
                var devices = wms.JDJS_WMS_Device_Info.Where(r => r.Position == LocationID);
                foreach (var item in devices)
                {
                    int cncID = Convert.ToInt32(item.ID);
                    var repairInfo = wms.JDJS_WMS_Device_Alarm_Repair.Where(r => r.CncID == cncID && r.AlarmStateID != Status0Code.ID && r.AlarmStateID != Status19Code.ID);
                    if (repairInfo.Count() > 0)
                    {
                        FixingNum++;
                    }
                    var repairConfirm = wms.JDJS_WMS_Device_Alarm_Repair.Where(r => r.CncID == cncID && r.AlarmStateID == Status19Code.ID);
                    if (repairConfirm.Count() > 0)
                    {
                        FixOverWaitConfirmNum++;
                    }
                }
            }
          var json=(FixingNum.ToString() + ',' + FixOverWaitConfirmNum.ToString());
            context.Response.Write("data:" + json + "\n\n");
            context.Response.ContentType = "text/event-stream";
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