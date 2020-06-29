using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Kanban.刀具
{
    /// <summary>
    /// 异常换刀 的摘要说明
    /// </summary>
    public class 异常换刀 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            List<ToolReplaceInfo> toolReplaceInfos = new List<ToolReplaceInfo>();
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            using (JDJS_WMS_DB_USEREntities wms = new JDJS_WMS_DB_USEREntities())
            {
                var toolReplace = wms.JDJS_WMS_Tool_Machine_Replaace_History_Table.Where(r => r.Flag == 0);
                foreach (var item in toolReplace)
                {
                    string re = "";
                    var reson = wms.JDJS_WMS_Tool_Replace_Reason_Table.Where(r => r.ID == item.ReasonID).FirstOrDefault();
                    if (reson != null)
                    {
                        re = reson.Reason;
                    }
                    var cncID = item.CncID;
                    var poison = from cnc in wms.JDJS_WMS_Device_Info
                                 from pos in wms.JDJS_WMS_Location_Info
                                 where cnc.Position == pos.id && cnc.ID == cncID
                                 select new
                                 {
                                     cnc.MachNum,
                                     pos.Name
                                 };
                    if (poison.Count() > 0)
                    {
                        ToolReplaceInfo toolReplaceInfo = new ToolReplaceInfo();
                        toolReplaceInfo.cncNum = poison.First().MachNum;
                        toolReplaceInfo.dateTime = Convert.ToDateTime(item.Time);
                        toolReplaceInfo.Id = item.ID.ToString();
                        toolReplaceInfo.position = poison.First().Name;
                        toolReplaceInfo.time = Convert.ToDateTime(item.Time).ToString();
                        toolReplaceInfo.reason = re;
                        toolReplaceInfo.toolNum = item.ToolNum.ToString();
                        toolReplaceInfo.totalNumber = toolReplace.Count().ToString();
                        toolReplaceInfos.Add(toolReplaceInfo);
                    }

                }
            }
          
            var json =serializer.Serialize( toolReplaceInfos.OrderBy(r => r.dateTime).Take(5));
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
    class ToolReplaceInfo
    {
        public string position;
        public string cncNum;
        public string toolNum;
        public string time;
        public string reason;
        public DateTime dateTime;
        public string Id;
        public string totalNumber;

    }
}