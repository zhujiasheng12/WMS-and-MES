using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Kanban.资材
{
    /// <summary>
    /// 物料历史 的摘要说明
    /// </summary>
    public class 物料历史 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //物料历史
            List<MaterialHistory> materialHistories = new List<MaterialHistory>();
            List<string> times = new List<string>();
            using (JDJS_WMS_DB_USEREntities  wms = new JDJS_WMS_DB_USEREntities ())
            {
                var blankInfos = wms.JDJS_WMS_Blank_Additional_History_Table;
                foreach (var item in blankInfos)
                {
                    DateTime time = Convert.ToDateTime(item.AddTime);
                    string timeStr = time.Year.ToString() + "-" + time.Month.ToString() + "-" + time.Day.ToString();
                    int addNum = Convert.ToInt32(item.BlankAddNum);
                    if (times.Contains(timeStr))
                    {
                        var materialHistory = materialHistories.Where(r => r.time == timeStr).FirstOrDefault();
                        if (materialHistory != null)
                        {
                            materialHistory.blankNum += addNum;
                        }
                    }
                    else
                    {
                        times.Add(timeStr);
                        MaterialHistory materialHistory = new MaterialHistory();
                        materialHistory.time = timeStr;
                        materialHistory.blankNum = addNum;
                        materialHistories.Add(materialHistory);
                    }
                }
                var fixtureInfos = wms.JDJS_WMS_Fixture_Additional_History_Table;
                if (fixtureInfos.Count() > 0)
                {
                    foreach (var item in fixtureInfos)
                    {
                        DateTime time = Convert.ToDateTime(item.AddTime);
                        string timeStr = time.Year.ToString() + "-" + time.Month.ToString() + "-" + time.Day.ToString();
                        int addNum = Convert.ToInt32(item.AddNum);
                        if (times.Contains(timeStr))
                        {
                            var materialHistory = materialHistories.Where(r => r.time == timeStr).FirstOrDefault();
                            if (materialHistory != null)
                            {
                                materialHistory.fixtureNum += addNum;
                            }
                        }
                        else
                        {
                            times.Add(timeStr);
                            MaterialHistory materialHistory = new MaterialHistory();
                            materialHistory.time = timeStr;
                            materialHistory.fixtureNum = addNum;
                            materialHistories.Add(materialHistory);
                        }
                    }
                }
            }

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var json = serializer.Serialize(materialHistories);
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
    public class MaterialHistory
    {
        /// <summary>
        /// 日期
        /// </summary>
        public string time;
        /// <summary>
        /// 特殊治具准备数
        /// </summary>
        public int fixtureNum;
        /// <summary>
        /// 毛坯准备数
        /// </summary>
        public int blankNum;
    }
}