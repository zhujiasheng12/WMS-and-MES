using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Kanban.品质ashx
{
    /// <summary>
    /// 品质历史 的摘要说明
    /// </summary>
    public class 品质历史 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //品质历史
            List<QualtityHistory> qualtityHistories = new List<QualtityHistory>();
            List<string> times = new List<string>();
            using (JDJS_WMS_DB_USEREntities  wms = new JDJS_WMS_DB_USEREntities ())
            {
                var qualtity = wms.JDJS_WMS_Quality_Confirmation_History_Table;
                foreach (var item in qualtity)
                {
                    DateTime operateTime = Convert.ToDateTime(item.OperateTime);
                    string timeStr = operateTime.Year.ToString() + "-" + operateTime.Month.ToString() + "-" + operateTime.Day.ToString();
                    if (times.Contains(timeStr))
                    {
                        var history = qualtityHistories.Where(r => r.time == timeStr);
                        history.FirstOrDefault().good += Convert.ToInt32(item.GoodNum);
                        history.FirstOrDefault().pool += Convert.ToInt32(item.PoolNum);
                    }
                    else
                    {
                        times.Add(timeStr);
                        QualtityHistory history = new QualtityHistory();
                        history.time = timeStr;
                        history.good = Convert.ToInt32(item.GoodNum);
                        history.pool = Convert.ToInt32(item.PoolNum);
                        qualtityHistories.Add(history);
                    }
                }
            }
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var json = serializer.Serialize(qualtityHistories);
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
    public class QualtityHistory
    {
        public string time;
        public int good;
        public int pool;
    }

    public class OutPutDays
    {
        public string day;
        public int OutPut;
    }
}