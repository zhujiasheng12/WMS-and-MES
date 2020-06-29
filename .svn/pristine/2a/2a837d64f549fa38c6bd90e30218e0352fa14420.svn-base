using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Kanban.设备监控
{
    /// <summary>
    /// 车间机床状态88 的摘要说明
    /// </summary>
    public class 车间机床状态88 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //车间机台状态
            int deviceAllNum = 0;
            int RunNum = 0;
            int AlarmNum = 0;
            int RepairNum = 0;
            int StopNum = 0;
            int xianzhi = 0;
            using (JDJS_WMS_DB_USEREntities wms = new JDJS_WMS_DB_USEREntities())
            {
                var devices = wms.JDJS_WMS_Device_Info;
                deviceAllNum = devices.Count();
                foreach (var item in devices)
                {
                    int cncID = item.ID;
                    var realData = wms.JDJS_WMS_Device_RealTime_Data.Where(r => r.CncID == cncID).FirstOrDefault();
                    if (realData != null)
                    {
                        if (realData.ProgState == 1 || realData.ProgState == 2)
                        {
                            RunNum++;
                        }
                        else if (realData.ProgState == 4)
                        {
                            AlarmNum++;
                        }
                        else if (realData.ProgState == -1)
                        {
                            xianzhi++;
                        }
                        else
                        {
                            var cncdo = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.CncID == cncID && r.isFlag == 1);
                            if (cncdo.Count() > 0)
                            {
                                StopNum++;
                                var repair = wms.JDJS_WMS_Device_Alarm_Repair.Where(r => r.CncID == cncID && r.AlarmStateID != 4);
                                if (repair.Count() > 0)
                                {
                                    RepairNum++;
                                    StopNum--;
                                }
                            }
                            else
                            {
                                xianzhi++;
                                var repair = wms.JDJS_WMS_Device_Alarm_Repair.Where(r => r.CncID == cncID && r.AlarmStateID != 4);
                                if (repair.Count() > 0)
                                {
                                    RepairNum++;
                                }
                            }
                        }
                    }
                }

            }
            var model = new
            {
                deviceAllNum = deviceAllNum,
                RunNum = RunNum,
                AlarmNum = AlarmNum,
                RepairNum = RepairNum,
                StopNum = StopNum,
                xianzhi = xianzhi
            };
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var json = serializer.Serialize(model);
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