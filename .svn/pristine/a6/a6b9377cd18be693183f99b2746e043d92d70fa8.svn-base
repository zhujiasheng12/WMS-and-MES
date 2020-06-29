using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model.刀具管理.刀具寿命管理
{
    /// <summary>
    /// stateNumberRead 的摘要说明
    /// </summary>
    public class stateNumberRead : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            using (JDJS_WMS_DB_USEREntities entities = new JDJS_WMS_DB_USEREntities())
            {
                var rows = entities.JDJS_WMS_Tool_Shank_Table.Where(r=>r.CncID!=null&r.ToolID!=null);
                int redNumber=0, yellowNumber=0;
                foreach (var item in rows)
                {
                    var toolId = item.ToolID;
                    if (entities.JDJS_WMS_ToolHolder_ToolLife_History_Table.Where(r => r.ToolID == toolId).Count() > 0)
                    {
                        var now =Convert.ToDouble( entities.JDJS_WMS_ToolHolder_ToolLife_History_Table.Where(r => r.ToolID == toolId).OrderByDescending(r => r.Time).First().ToolCurrLife);
                       var max = Convert.ToDouble(entities.JDJS_WMS_ToolHolder_ToolLife_History_Table.Where(r => r.ToolID == toolId).OrderByDescending(r => r.Time).First().ToolMaxLife);
                        if (now >= max & max > 0)
                        {
                            redNumber++;
                        }
                        else if (now >= max * 0.8 & max > 0)
                        {
                            yellowNumber++;
                        }
                    }
                   
                   

                   
                }
                var model = new { yellowNumber, redNumber };
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                var json = serializer.Serialize(model);
                context.Response.Write(json);
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