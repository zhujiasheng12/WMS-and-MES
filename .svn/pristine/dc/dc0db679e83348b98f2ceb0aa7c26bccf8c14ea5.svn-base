using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Kanban.订单ashx
{
    /// <summary>
    /// 柱状图 的摘要说明
    /// </summary>
    public class 柱状图 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //读取每日产量
            List<OutPutDays> outPuts = new List<OutPutDays>();
            List<string> times = new List<string>();
            using (JDJS_WMS_DB_USEREntities  wms = new JDJS_WMS_DB_USEREntities ())
            {
                var outs = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.isFlag == 3);
                foreach (var item in outs)
                {
                    DateTime time = Convert.ToDateTime(item.EndTime);
                    string timestr = time.Year.ToString() + "-" + time.Month.ToString() + "-" + time.Day.ToString();
                    if (times.Contains(timestr))
                    {
                        var output = outPuts.Where(r => r.day == timestr).FirstOrDefault();
                        output.OutPut++;
                    }
                    else
                    {
                        times.Add(timestr);
                        OutPutDays outPut = new OutPutDays();
                        outPut.day = timestr;
                        outPut.OutPut = 1;
                        outPuts.Add(outPut);
                    }
                }
            }
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var json = serializer.Serialize(outPuts);
            context.Response.Write("data:"+json+"\n\n");
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
    public class OutPutDays
    {
        public string day;
        public int OutPut;
    }
}