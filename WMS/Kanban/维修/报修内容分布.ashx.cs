using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Kanban.维修
{
    /// <summary>
    /// 报修内容分布 的摘要说明
    /// </summary>
    public class 报修内容分布 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //现场看板维修之报修内容分布,
            List<string> content = new List<string>();
            List<RepairContent> repairContents = new List<RepairContent>();
            using (JDJS_WMS_DB_USEREntities  wms = new  JDJS_WMS_DB_USEREntities ())
            {
                var repairInfos = wms.JDJS_WMS_Device_Alarm_Repair;
                foreach (var item in repairInfos)
                {
                    int descID = Convert.ToInt32(item.AlarmDescID);
                    var descInfo = wms.JDJS_WMS_Device_Alarm_Description.Where(r => r.ID == descID).FirstOrDefault();
                    if (descInfo != null)
                    {
                        string desc = descInfo.Description;
                        if (content.Contains(desc))
                        {
                            var repair = repairContents.Where(r => r.Content == desc).FirstOrDefault();
                            if (repair != null)
                            {
                                repair.Num++;
                            }
                        }
                        else
                        {
                            content.Add(desc);
                            RepairContent repairContent = new RepairContent();
                            repairContent.Content = desc;
                            repairContent.Num = 1;
                            repairContents.Add(repairContent);
                        }
                    }
                }
            }
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var json = serializer.Serialize(repairContents);
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
    class RepairContent
    {
        /// <summary>
        /// 维修内容
        /// </summary>
        public string Content;
        /// <summary>
        /// 数量
        /// </summary>
        public int Num;
    }
}