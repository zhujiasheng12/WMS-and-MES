using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Kanban
{
    /// <summary>
    /// 报修状态圆饼 的摘要说明
    /// </summary>
    public class 报修状态圆饼 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            int guaqi = 0;
            int yiwancheng = 0;
            int daijiedan = 0;
            int daiweixiu = 0;
            int weixiuzhong = 0;
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            using (JDJS_WMS_DB_USEREntities  wms = new JDJS_WMS_DB_USEREntities ())
            {
                
                var repairInfo = wms.JDJS_WMS_Device_Alarm_Repair.ToList();
                foreach (var item in repairInfo)
                {
                    switch (item.AlarmStateID)
                    {
                        case 3:
                            guaqi++;
                            break;
                        case 4:
                            yiwancheng++;
                            break;
                        case 1:
                            daijiedan++;
                            break;
                        case 5:
                            daiweixiu++;
                            break;
                        case 2:
                            weixiuzhong++;
                            break;
                        default:
                            break;

                    }

                }
            }
            var model = new
            {
                guaqi = guaqi,
                yiwancheng = yiwancheng,
                daijiedan = daijiedan,
                daiweixiu = daiweixiu,
                weixiuzhong = weixiuzhong
            };
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