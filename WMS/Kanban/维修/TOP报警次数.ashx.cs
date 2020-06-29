using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Kanban.维修
{
    /// <summary>
    /// TOP报警次数 的摘要说明
    /// </summary>
    public class TOP报警次数 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            {
                // TOP报警次数
                int Location = int.Parse(context.Request["workId"]);
                DateTime now = DateTime.Now;
                DateTime nowLast = now.AddMonths(-1);
                List<string> cncNums = new List<string>();
                List<alarmNum> alarmNums = new List<alarmNum>();
                List<alarmNumInfo> alarmNumInfos = new List<alarmNumInfo>();
                using (JDJS_WMS_DB_USEREntities  wms = new JDJS_WMS_DB_USEREntities ())
                {
                    var alarms = wms.JDJS_WMS_Device_ProgState_Info.Where(r => r.ProgState == 4 && r.StartTime > nowLast);
                    foreach (var item in alarms)
                    {
                        int cncID = Convert.ToInt32(item.CncID);
                        var cnc = wms.JDJS_WMS_Device_Info.Where(r => r.ID == cncID).FirstOrDefault();
                        if (cnc != null && cnc.Position == Location)
                        {
                            var cncInfo = cnc.MachNum;
                            if (cncNums.Contains(cncInfo))
                            {
                                var alarmnum = alarmNumInfos.Where(r => r.cncNum == cncInfo).FirstOrDefault();
                                alarmnum.num++;
                            }
                            else
                            {
                                cncNums.Add(cncInfo);
                                alarmNumInfo alarmNumInfo = new alarmNumInfo();
                                alarmNumInfo.cncNum = cncInfo;
                                alarmNumInfo.num = 1;
                                alarmNumInfos.Add(alarmNumInfo);
                            }
                        }

                    }
                }
                alarmNumInfos = alarmNumInfos.OrderByDescending(r => r.num).ToList();
                for (int i = 0; i < 3; i++)
                {
                    if (alarmNumInfos.Count() > i)
                    {
                        alarmNum alarmNum = new alarmNum();
                        alarmNum.cncNum = alarmNumInfos[i].cncNum;
                        alarmNum.num = alarmNumInfos[i].num.ToString();
                        alarmNum.process = (((float)alarmNumInfos[i].num / alarmNumInfos[0].num)).ToString ("0.0000");
                        alarmNums.Add(alarmNum);
                    }
                    else
                    {
                        alarmNum alarmNum = new alarmNum();
                        alarmNum.cncNum = "";
                        alarmNum.num = "";
                        alarmNum.process = "";
                        alarmNums.Add(alarmNum);

                    }
                }
                var model = new { code = 0, data = alarmNums };
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                var json = serializer.Serialize(model);
                context.Response.Write("data:" + json + "\n\n");
                context.Response.ContentType = "text/event-stream";
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
    public class alarmNum
    {
        public string cncNum;
        public string num;
        public string process;
    }

    public class alarmNumInfo
    {
        public string cncNum;
        public int num;
        public double process;
    }
}