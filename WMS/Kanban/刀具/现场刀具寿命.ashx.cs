using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Kanban.刀具
{
    /// <summary>
    /// 现场刀具寿命 的摘要说明
    /// </summary>
    public class 现场刀具寿命 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            {
                //刀具寿命
                int Location = int.Parse(context.Request["workId"]);
                List<ToolLife> toolLives = new List<ToolLife>();
                using (JDJS_WMS_DB_USEREntities  wms = new JDJS_WMS_DB_USEREntities ())
                {
                   
                    var ToolLifes = wms.JDJS_WMS_Tool_LifeTime_Management;
                    foreach (var item in ToolLifes)
                    {

                        int cncID = Convert.ToInt32(item.CncID);
                        var cnc = wms.JDJS_WMS_Device_Info.Where(r => r.ID == cncID).FirstOrDefault();

                        if (cnc != null && cnc.Position == Location)
                        {

                            List<string> ToolNos = new List<string>();
                            var standTool = wms.JDJS_WMS_Tool_Standard_Table.Where (r=>r.MachTypeID ==cnc.MachType);
                            foreach (var mo in standTool)
                            {
                                ToolNos.Add(mo.ToolID);
                            }
                            var cetou = wms.JDJS_WMS_Tool_Standard_Table.Where(r =>( r.Name .Contains ( "测头")|| r.Name.Contains ( "探针")) && r.MachTypeID == cnc.MachType).FirstOrDefault();
                            int cetouID = 31;
                            if (cetou != null)
                            {
                                cetouID = Convert.ToInt32(cetou.ToolID.Substring(1));
                            }
                            int toolNum =Convert .ToInt32 ( item.ToolID);
                            var shankInfo = wms.JDJS_WMS_Tool_Shank_Table.Where(r => r.CncID == cncID && r.ToolNum == toolNum).FirstOrDefault ();
                            if (shankInfo != null)
                            {
                                if (item.ToolID != cetouID)
                                {
                                    var max = item.ToolCurrTime;
                                    var curr = item.ToolMaxTime;
                                    var time = max - curr;
                                    ToolLife toolLife = new ToolLife();
                                    toolLife.cncNum = cnc.MachNum;
                                    toolLife.ToolNo = "T" + item.ToolID.ToString();
                                    toolLife.Time = Convert.ToDouble(time / 60);
                                    if (time >= 0)
                                    {
                                        toolLife.PendTime = DateTime.Now.AddMinutes(Convert.ToDouble(time)).ToString();
                                    }
                                    else
                                    {
                                        var toolHistory = wms.JDJS_WMS_Tool_LifeTimeOver_History_Table.Where(r => r.CncID == cncID && r.ToolID == item.ToolID).OrderByDescending(r => r.Time).ToList();
                                        if (toolHistory.Count() > 0)
                                        {
                                            toolLife.PendTime = toolHistory.FirstOrDefault().Time.ToString();
                                        }
                                        else
                                        {
                                            toolLife.PendTime = DateTime.Now.AddMinutes(Convert.ToDouble(time)).ToString();
                                        }
                                    }
                                    toolLives.Add(toolLife);
                                }
                            }
                        }
                    }
                }
                toolLives = toolLives.OrderBy(r => r.Time).ToList();
                var model = new { code = 0, data = toolLives };
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                var json = serializer.Serialize(model);
                //System.Threading.Thread.Sleep(10000); //毫秒
                context.Response.Write("data:"+json+"\n\n");
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
    public class ToolLife
    {
        public string cncNum;
        public string ToolNo;
        public double Time;
        public string PendTime;
    }
}