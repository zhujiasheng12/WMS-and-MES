using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Kanban.刀具
{
    /// <summary>
    /// 备刀历史 的摘要说明
    /// </summary>
    public class 备刀历史 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //备刀历史
            List<string> times = new List<string>();
            List<PerpareToolHistory> perpareToolHistories = new List<PerpareToolHistory>();
            using (JDJS_WMS_DB_USEREntities  wms = new JDJS_WMS_DB_USEREntities ())
            {
                try
                {
                    List<string> ToolNos = new List<string>();

                   
                    var toolHistorys = wms.JDJS_WMS_Order_Tool_Prepare_History_table;
                    if (toolHistorys.Count() > 0)
                    {
                        foreach (var item in toolHistorys)
                        {
                            ToolNos.Clear();
                            DateTime time = Convert.ToDateTime(item.PrepareTime);
                            string timestr = time.Year.ToString() + "-" + time.Month.ToString() + "-" + time.Day.ToString();
                            int cncNUm = Convert.ToInt32(item.Num);
                            int processID = Convert.ToInt32(item.ProcessID);
                            var process = wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == processID).First();
                            var standTool = wms.JDJS_WMS_Tool_Standard_Table.Where (r=>r.MachTypeID ==process.DeviceType );
                            foreach (var real in standTool)
                            {
                                ToolNos.Add(real.ToolID);
                            }
                            int toolAllNum = 0;
                            var toos = wms.JDJS_WMS_Order_Process_Tool_Info_Table.Where(r => r.ProcessID == processID);
                            foreach (var tool in toos)
                            {
                                int tooNo = Convert.ToInt32(tool.ToolNO);
                                if (!ToolNos.Contains ("T"+tooNo.ToString ()))
                                {
                                    toolAllNum++;
                                }
                            }
                            int allToolNum = toolAllNum * cncNUm;
                            if (times.Contains(timestr))
                            {
                                var history = perpareToolHistories.Where(r => r.time == timestr).FirstOrDefault();
                                history.num += allToolNum;
                            }
                            else
                            {
                                times.Add(timestr);
                                PerpareToolHistory perpareToolHistory = new PerpareToolHistory();
                                perpareToolHistory.time = timestr;
                                perpareToolHistory.num = allToolNum;
                                perpareToolHistories.Add(perpareToolHistory);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    context.Response.Write(ex.Message);
                    return;
                }
            }
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var json = serializer.Serialize(perpareToolHistories);
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



    public class PerpareToolHistory
    {
        public string time;
        public int num;
    }
}