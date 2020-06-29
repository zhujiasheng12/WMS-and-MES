using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model.生产管理.工程部
{
    /// <summary>
    /// cncRead 的摘要说明
    /// </summary>
    public class cncRead : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var processId = int.Parse(context.Request["processId"]);
            using (JDJS_WMS_DB_USEREntities entities=new JDJS_WMS_DB_USEREntities())
            {
                List<Cnc> cncs = new List<Cnc>();
                var rows = entities.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.ProcessID == processId&r.isFlag!=0);
                 if (rows.Count() == 0)
                {

                    context.Response.Write("{\"code\":0,\"msg\":\"\",\"count\":1,\"data\":[]}");
                    return;

                };
                foreach (var item in rows)
                {
                    cncs.Add(new Cnc { cncId = Convert.ToInt32(item.CncID), startTime = Convert.ToDateTime(item.StartTime), endTime = Convert.ToDateTime(item.EndTime) });
                }
                var singles = cncs.Where((r, i) => cncs.FindIndex(p=>p.cncId==r.cncId)==i);
                List<Cnc> cncs1 = new List<Cnc>();
                foreach (var item in singles)
                {
                    var repeat = cncs.Where(r => r.cncId == item.cncId);
                    var dd = repeat.OrderBy(r => r.startTime);
                    var startTime = repeat.OrderBy(r => r.startTime).First().startTime;
                 
                    var endTime = repeat.OrderBy(r => r.startTime).Last().endTime ;
                    var cncNumber = entities.JDJS_WMS_Device_Info.Where(r => r.ID == item.cncId).First().MachNum;

                    cncs1.Add(new Cnc { cncNumber = cncNumber, startTimeStr = startTime.ToString(), endTimeStr = endTime.ToString() });

                }
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                var model = new { code = 0, msg = "", count = cncs1.Count, data = cncs1 };
                var json = serializer.Serialize(model);
                context.Response.Write(json);
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
    class Cnc
    {
        public int cncId;
        public string cncNumber;
        public DateTime startTime;
        public DateTime endTime;
        public string startTimeStr;
        public string endTimeStr;
    }
}