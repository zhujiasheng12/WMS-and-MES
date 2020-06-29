using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.工程部
{
    /// <summary>
    /// virtualProcessRead 的摘要说明
    /// </summary>
    public class virtualProcessRead : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var orderId = int.Parse(context.Request["orderId"]);
            using(JDJS_WMS_DB_USEREntities entities=new JDJS_WMS_DB_USEREntities())
            {
                var rows = entities.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == orderId&r.sign==0);
                List<VirtualProcess> processes = new List<VirtualProcess>();
                if (rows.Count() > 0)
                {
                    foreach (var item in rows)
                    {
                        var deviceId = item.DeviceType;
                        var deviceType = entities.JDJS_WMS_Device_Type_Info .Where(r => r.ID == deviceId).First().Type ;
                        processes.Add(new VirtualProcess
                        {
                            processNum = item.ProcessID.ToString(),
                            processTime = item.ProcessTime.ToString(),
                            deviceType = deviceType,
                            id=item.ID.ToString(),
                            cncNumber=item.MachNumber.ToString()
                        });
                      
                    }
                    var model = new { code = 0, msg = "", count = processes.Count(), data = processes };
                    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var json = serializer.Serialize(model);
                    context.Response.Write(json);
                }
                else
                {
                    context.Response.Write("{\"code\":0,\"msg\":\"\",\"count\":1,\"data\":[]}");
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
    class VirtualProcess
    {
        public string processNum;
        public string processTime;
        public string deviceType;
        public string id;
        public string cncNumber;
    }
}