using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Kanban.刀具
{
    /// <summary>
    /// 刀具寿命 的摘要说明
    /// </summary>
    public class 刀具寿命 : IHttpHandler
    {
        System.Web.Script.Serialization.JavaScriptSerializer Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        public void ProcessRequest(HttpContext context)
        {
           using(JDJS_WMS_DB_USEREntities entities=new JDJS_WMS_DB_USEREntities())
            {
                var rows =
                    from toolLife in entities.JDJS_WMS_ToolHolder_ToolLife_History_Table
                    from toolShank in entities.JDJS_WMS_Tool_Shank_Table
                    from tool in entities.JDJS_WMS_ToolHolder_Tool_Table
                    from toolSpec in entities.JDJS_WMS_Tool_Stock_History
                    from cnc in entities.JDJS_WMS_Device_Info
                    from location in entities.JDJS_WMS_Location_Info
                    where toolLife.ToolID == toolShank.ToolID & toolShank.CncID != null & tool.ID == toolShank.ToolID & tool.ToolSpecifications == toolSpec.Id&
                    cnc.ID==toolShank.CncID&cnc.Position==location.id
                    select new
                    {
                        toolShank.CncID,
                        toolShank.ToolNum,
                        toolSpec.KnifeName,
                        toolSpec.KnifeSpecifications,
                        toolLife.ToolCurrLife,
                        toolLife.ToolMaxLife,
                        cnc.MachNum,
                        toolLife.Time,
                        toolLife.ToolID,
                        location.Name
                    };
               var  rowsList = rows.OrderByDescending(r => r.Time).ToList();
                rowsList = rowsList.Where((x, i) => rowsList.FindIndex(p => p.ToolID == x.ToolID) == i).ToList();

                var json = Serializer.Serialize(new { data= rowsList.Take(5) ,count = rowsList .Count});
                context.Response.Write("data:" + json + "\n\n");
                context.Response.ContentType = "text/event-stream";

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