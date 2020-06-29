using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.刀具管理.刀具历史
{
    /// <summary>
    /// 刀具历史读数据 的摘要说明
    /// </summary>
    public class 刀具历史读数据 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
           using(JDJS_WMS_DB_USEREntities entities=new JDJS_WMS_DB_USEREntities())
            {
                var rows = entities.JDJS_WMS_ToolHolder_Tool_Table;
                List<ToolHistory> tools = new List<ToolHistory>();
                var Verdor = "";
                var Batch = "-";
                var ToolSpecification = "-";
                var ToolNum = "-";
                var toolIdentity = "-";
                foreach (var item in rows)
                {
                   


                    var VerdorID = item.VerdorID;
                    Batch = item.Batch;
                    var ToolSpecificationId = item.ToolSpecifications;
                    ToolNum = item.ToolNum.ToString();
                    while (ToolNum.Length < 5)
                    {
                        ToolNum = "0" + ToolNum;
                    }
                    if (entities.JDJS_WMS_Tool_Verdor_Table.Where(r => r.ID == VerdorID).Count() > 0)
                    {
                        Verdor = entities.JDJS_WMS_Tool_Verdor_Table.Where(r => r.ID == VerdorID).First().VerdorNum;
                        if (entities.JDJS_WMS_Tool_Stock_History.Where(r => r.Id == ToolSpecificationId).Count() > 0)
                        {
                            ToolSpecification = entities.JDJS_WMS_Tool_Stock_History.Where(r => r.Id == ToolSpecificationId).First().num.ToString();
                            while (ToolSpecification.Length < 4)
                            {
                                ToolSpecification = "0" + ToolSpecification;
                            }
                            toolIdentity = Verdor + Batch + ToolSpecification + ToolNum;
                        }
                    }





                 
                    var id = item.ID;
                    var state = "未使用";
                    var toolShankTable = entities.JDJS_WMS_Tool_Shank_Table.Where(r => r.ToolID == id);
                    if (toolShankTable.Count()>0)
                    {
                        if (toolShankTable.First().CncID == null)
                        {
                            state = "待使用";
                        }
                        else
                        {
                            state = "使用中";
                        }

                    }
                    var nowLife="";
                    var maxLife = "";
                    if(entities.JDJS_WMS_ToolHolder_ToolLife_History_Table.Where(r => r.ToolID == id).Count() > 0)
                    {
                        nowLife = entities.JDJS_WMS_ToolHolder_ToolLife_History_Table.Where(r => r.ToolID == id).First().ToolCurrLife.ToString();
                        maxLife= entities.JDJS_WMS_ToolHolder_ToolLife_History_Table.Where(r => r.ToolID == id).First().ToolMaxLife.ToString();
                    }
                    var KnifeSpecifications = entities.JDJS_WMS_Tool_Stock_History.Where(r => r.Id == item.ToolSpecifications).First().KnifeSpecifications.ToString();
                    tools.Add(new ToolHistory
                    {
                        toolId = toolIdentity,
                        specification = KnifeSpecifications,

                        nowLife = nowLife,
                        maxLife = maxLife,
                        state=state,
                        primaryKey= id.ToString()
                    });
                }

                if (context.Request["key"] != null)
                {
                    var key = context.Request["key"];
                  tools=  tools.Where(r => r.toolId==key|r.specification==key|r.state==key).ToList();
                }
                var limit = int.Parse(context.Request["limit"]);
                var page = int.Parse(context.Request["page"]);
               var  layPage = tools.Skip((page - 1) * limit).Take(limit).ToList();
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                var model = new { code = 0, data = layPage,count=tools.Count() };
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
    class ToolHistory
    {

        public string toolId;
        public string specification;
        public string nowLife;
        public string maxLife;
        public string state;
        public string primaryKey;
    }
}