using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model.生产管理.维保
{
    /// <summary>
    /// stateNumberRead 的摘要说明
    /// </summary>
    public class stateNumberRead : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            using (JDJS_WMS_DB_USEREntities JDJS_WMS_Device_Info = new JDJS_WMS_DB_USEREntities())
            {
                int RepairNum = 0;
                int MaintenancingNum = 0;
                int MaintenancedNum = 0;
                int ReceptionNum = 0;
                var list = JDJS_WMS_Device_Info.JDJS_WMS_Device_Alarm_Repair.ToList();
                foreach (var item in list)
                {
                    if (item.AlarmStateID != 4)
                    {
                        RepairNum++;
                    }
                    if (item.AlarmStateID != 4 && (item.AlarmStateID == 2 || item.AlarmStateID == 3))
                    {
                        MaintenancingNum++;
                    }
                    if (item.AlarmStateID != 4 && (item.AlarmStateID == 1 || item.AlarmStateID == 5))
                    {
                        MaintenancedNum++;
                    }
                    if (item.AlarmStateID == 1)
                    {
                        ReceptionNum++;
                    }
                }

                var model = new { black = RepairNum, green = MaintenancingNum, orange = MaintenancedNum, red = ReceptionNum };
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
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
}