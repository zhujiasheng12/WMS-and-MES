using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model.刀具管理.刀具寿命管理
{
    /// <summary>
    /// knifeRead 的摘要说明
    /// </summary>
    public class knifeRead : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {

            int CncID = int.Parse(context.Request["id"]);
            List<ToolInfo> toolList = new List<ToolInfo>();

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            using (JDJS_WMS_DB_USEREntities JDJS_WMS_Device_Info = new JDJS_WMS_DB_USEREntities())
            {
                //AddNodes(jsons, JDJS_WMS_Device_Info.JDJS_WMS_Location_Info.ToList(), JDJS_WMS_Device_Info.JDJS_WMS_Device_Info.ToList());
                //int CncID = 33;
                if (JDJS_WMS_Device_Info.JDJS_WMS_Device_Info.Where(r => r.ID == CncID).Count() == 0)
                {
                   context.Response.Write("{ \"code\":0,\"data\":[]}");
                    return;

                }
                var cnc = JDJS_WMS_Device_Info.JDJS_WMS_Device_Info.Where(r => r.ID == CncID).First();
                var rows = JDJS_WMS_Device_Info.JDJS_WMS_Tool_LifeTime_Management.Where(r => r.CncID == CncID);
                //List<ToolInfo> toolList = new List<ToolInfo>();
                foreach (var item in rows)
                {
                    ToolInfo tool = new ToolInfo();
                    tool.cncId = CncID.ToString();
                    tool.toolID = Convert.ToInt32(item.ToolID);
                    tool.toolL = Math.Round(Convert.ToDouble(item.ToolL),2);
                    tool.toolH = Math.Round(Convert.ToDouble(item.ToolH),2);
                    tool.toolR = Math.Round(Convert.ToDouble(item.ToolR),2);
                    tool.toolD = Math.Round(Convert.ToDouble(item.ToolD),2);
                    if (Convert.ToDouble(item.ToolMaxTime) < 0)
                    {
                        tool.toolMaxTime = 0;
                    }
                    else
                    {
                        tool.toolMaxTime = Math.Round(Convert.ToDouble(item.ToolMaxTime), 2);
                    }
                    if (Convert.ToDouble(item.ToolCurrTime)<0)
                    {
                        tool.toolCurrTime = 0;
                    }
                    else
                    {
                        tool.toolCurrTime = Math.Round(Convert.ToDouble(item.ToolCurrTime), 2);
                    }
                    if (Convert.ToDouble(item.ToolMaxNum)<0)
                    {
                        tool.toolMaxNum = 0;
                    }
                    else
                    {
                        tool.toolMaxNum = Math.Round(Convert.ToDouble(item.ToolMaxNum), 2);
                    }
                    if (Convert.ToDouble(item.ToolCurrNum)<0)
                    {
                        tool.toolCurrNum = 0;
                    }
                    else
                    {
                        tool.toolCurrNum = Convert.ToDouble(item.ToolCurrNum);
                    }
                    string ToolNum = "T" + item.ToolID.ToString();
                    var toolTable = JDJS_WMS_Device_Info.JDJS_WMS_Tool_Standard_Table.Where(r => r.ToolID == ToolNum&&r.MachTypeID ==cnc.MachType);
                    if (toolTable.Count () > 0)
                    {
                        tool.toolLength = Convert.ToDouble(toolTable.First ().ToolLength);
                    }
                    
                   
                    tool.toolMaxDistance = Convert.ToDouble(item.ToolMaxDistance);
                    tool.toolCurrDistance = Convert.ToDouble(item.ToolCurrDistance);
                    toolList.Add(tool);
                }
            }
            var page = int.Parse(context.Request["page"]);
            var limit = int.Parse(context.Request["limit"]);
            var date = toolList.Skip((page - 1) * limit).Take(limit);
            var model = new { code = 0, msg = "", count = toolList.Count, data = date };
            string json = serializer.Serialize(model);
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
    public class ToolInfo
    {
        public int toolID;
        public double toolL;
        public double toolH;
        public double toolR;
        public double toolD;
        public double toolMaxTime;
        public double toolCurrTime;
        public double toolMaxNum;
        public double toolCurrNum;
        public double toolMaxDistance;
        public double toolCurrDistance;
        public double toolLength;
        public string cncId;
    }
}