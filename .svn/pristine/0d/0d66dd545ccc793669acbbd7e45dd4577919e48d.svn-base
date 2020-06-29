using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Kanban.刀具
{
    /// <summary>
    /// 装刀历史 的摘要说明
    /// </summary>
    public class 装刀历史 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //装刀历史
            List<string> times = new List<string>();
            List<LoadToolHistory> loadToolHistories = new List<LoadToolHistory>();
            using (JDJS_WMS_DB_USEREntities  wms = new JDJS_WMS_DB_USEREntities ())
            {
               
                var tools = wms.JDJS_WMS_Device_Tool_History_Table.ToList();
                foreach (var item in tools)
                {
                    int cncID =Convert.ToInt32 ( item.CncID);
                    var cnc = wms.JDJS_WMS_Device_Info.Where(r => r.ID == cncID).First();
                    List<string> ToolNos = new List<string>();
                    var standTool = wms.JDJS_WMS_Tool_Standard_Table.Where (r=>r.MachTypeID ==cnc.MachType);
                    foreach (var mo in standTool)
                    {
                        ToolNos.Add(mo.ToolID);
                    }
                    int toolNo = Convert.ToInt32(item.ToolNum);
                    DateTime time = Convert.ToDateTime(item.Time);
                    string timeStr = time.Year.ToString() + "-" + time.Month.ToString() + "-" + time.Day.ToString();
                    if (times.Contains(timeStr))
                    {
                        var loadToolHistory = loadToolHistories.Where(r => r.time == timeStr).FirstOrDefault();
                        if (loadToolHistory != null)
                        {
                            if (!ToolNos .Contains ("T"+toolNo .ToString ()))
                            {
                                loadToolHistory.specialToolNum++;
                            }
                            else
                            {
                                loadToolHistory.regularToolNum++;
                            }
                        }
                    }
                    else
                    {
                        times.Add(timeStr);
                        LoadToolHistory loadToolHistory = new LoadToolHistory();
                        loadToolHistory.time = timeStr;
                        if (!ToolNos.Contains("T" + toolNo.ToString()))
                        {
                            loadToolHistory.specialToolNum++;
                        }
                        else
                        {
                            loadToolHistory.regularToolNum++;
                        }

                        loadToolHistories.Add(loadToolHistory);
                    }
                }
            }

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var json = serializer.Serialize(loadToolHistories);
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
    public class LoadToolHistory
    {
        public string time;
        public int regularToolNum;
        public int specialToolNum;
    }
}