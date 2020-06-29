using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Kanban
{
    /// <summary>
    /// 报警列表 的摘要说明
    /// </summary>
    public class 报警列表 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            {
                //报警列表
                List<string> time = new List<string>();
                List<AlarmInfoRead> alarmInfoReads = new List<AlarmInfoRead>();
                using (JDJS_WMS_DB_USEREntities wms = new JDJS_WMS_DB_USEREntities ())
                {
                    var alarms = wms.JDJS_WMS_Device_Alarm_History_Table.ToArray();
                    foreach (var item in alarms)
                    {
                        var itemAlarmTime = Convert.ToDateTime(item.StartTime);
                        string alarmTimeStr = itemAlarmTime.Year.ToString() + "-" + itemAlarmTime.Month.ToString() + "-" + itemAlarmTime.Day.ToString();
                        if (time.Contains(alarmTimeStr))
                        {
                            var alarminfo = alarmInfoReads.Where(r => r.data == alarmTimeStr).FirstOrDefault();
                            int cncID = Convert.ToInt32(item.CncID);
                            var cnc = wms.JDJS_WMS_Device_Info.Where(r => r.ID == cncID).FirstOrDefault();
                            string alarmDesc = cnc.MachNum + "报警  " + "报警描述：" + item.ErrDesc;
                            alarminfo.alarms.Insert(0, alarmDesc);
                        }
                        else
                        {
                            time.Add(alarmTimeStr);
                            AlarmInfoRead alarmInfoRead = new AlarmInfoRead();
                            alarmInfoRead.alarms = new List<string>();
                            alarmInfoRead.data = alarmTimeStr;
                            int cncID = Convert.ToInt32(item.CncID);
                            var cnc = wms.JDJS_WMS_Device_Info.Where(r => r.ID == cncID).FirstOrDefault();
                            string alarmDesc = cnc.MachNum + "报警  " + "报警描述：" + item.ErrDesc;
                            alarmInfoRead.alarms.Add(alarmDesc);
                            alarmInfoReads.Add(alarmInfoRead);
                        }
                    }
                }
                System.Web.Script.Serialization.JavaScriptSerializer  serialization = new System.Web.Script.Serialization.JavaScriptSerializer ();
                var json = serialization.Serialize(alarmInfoReads);
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

    public class AlarmInfoRead
    {
        public string data;
        public List<string> alarms;
    }
}