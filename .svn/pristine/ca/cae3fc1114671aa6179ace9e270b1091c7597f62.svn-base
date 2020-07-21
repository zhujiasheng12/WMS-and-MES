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
                    from toolLife in entities.JDJS_WMS_Tool_LifeTime_Management
                    from cnc in entities.JDJS_WMS_Device_Info
                    from location in entities.JDJS_WMS_Location_Info
                    from shank in entities.JDJS_WMS_Tool_Shank_Table 
                    from toolTable in entities.JDJS_WMS_ToolHolder_Tool_Table
                    from toolS in entities.JDJS_WMS_Tool_Stock_History
                    where toolLife.CncID == cnc.ID&cnc.Position ==location.id&&shank.CncID ==cnc.ID &&toolTable.ID ==shank.ToolID&&toolS.Id ==toolTable .ToolSpecifications 
                    select new
                    {
                        toolLife.CncID,
                        toolLife.ToolID,
                        toolS.KnifeName,
                        toolS.KnifeSpecifications,
                        toolLife.ToolCurrTime,
                        toolLife.ToolMaxTime,
                        cnc.MachNum,
                        //toolLife.Time,
                        //toolLife.ToolID,
                        location.Name
                    };
               var  rowsList = rows.OrderBy(r => (r.ToolMaxTime -r.ToolCurrTime)).ToList();
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