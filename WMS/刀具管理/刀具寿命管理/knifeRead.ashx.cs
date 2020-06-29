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
                if (JDJS_WMS_Device_Info.JDJS_WMS_Device_Info.Where(r => r.ID == CncID).Count() == 0)
                {
                   context.Response.Write("{ \"code\":0,\"data\":[]}");
                    return;

                }


                var cncTypeId = JDJS_WMS_Device_Info.JDJS_WMS_Device_Info.Where(r => r.ID == CncID).First().MachType;

                var standTable = JDJS_WMS_Device_Info.JDJS_WMS_Tool_Standard_Table.Where(r => r.MachTypeID == cncTypeId);
                if (standTable.Count() > 0) {
                    foreach (var item in standTable)
                    {
                        int toolNum = 0;
                        if (!int.TryParse((item.ToolID.Replace("T", "")), out toolNum))
                        {
                            continue;
                        } 
                          
                      
                        var lifeTable = JDJS_WMS_Device_Info.JDJS_WMS_Tool_LifeTime_Management.Where(r => r.CncID == CncID & r.ToolID == toolNum).FirstOrDefault();

                        if (lifeTable != null)
                        {
                            toolList.Add(new ToolInfo
                            {
                                toolID = toolNum.ToString(),
                                toolNum =toolNum,
                                toolLength =(60+ item.ToolLength).ToString(),
                                cncId = CncID.ToString(),
                                toolCurrTime =Math.Round(Convert.ToDouble(lifeTable.ToolCurrTime),0).ToString(),
                                toolMaxTime = Math.Round(Convert.ToDouble(lifeTable.ToolMaxTime), 0).ToString(),
                                toolAttribute ="常规",
                                toolIdentity = "-",
                                existence = "有",

                                toolShank = "-"
                            });
                        }
                        else {
                            toolList.Add(new ToolInfo
                            {
                                toolID = toolNum.ToString(),
                                toolNum = toolNum,
                                toolLength = "-",
                                cncId = CncID.ToString(),
                                toolCurrTime = "-",
                                toolMaxTime = "-",
                                toolAttribute = "-",
                                toolIdentity = "-",
                                existence = "无",

                                toolShank = "-"
                            });
                        }
                    }
                }



                var cncType = JDJS_WMS_Device_Info.JDJS_WMS_Device_Info.Where(r => r.ID == CncID).First().MachType; ;
                var tool_Location_Number = JDJS_WMS_Device_Info.JDJS_WMS_Device_Type_Info.Where(r => r.ID == cncType).First().tool_Location_Number;
                for (int i = 1; i <= tool_Location_Number; i++)
                {
                    var toolNum="T"+i.ToString();
                    if (JDJS_WMS_Device_Info.JDJS_WMS_Tool_Standard_Table.Where(r => r.MachTypeID == cncTypeId & r.ToolID == toolNum).Count() > 0)
                    {
                        continue;
                    }
                    var rows = JDJS_WMS_Device_Info.JDJS_WMS_Tool_Shank_Table.Where(r => r.CncID == CncID&r.ToolNum==i);
                    if (rows.Count() > 0)
                    {
                        var toolId = rows.First().ToolID;
                        var Verdor = "";
                        var Batch = "-";
                        var ToolSpecification = "-";
                        var ToolNum = "-";
                        var toolIdentity = "-";
                        var toolCurrTime = "-";
                        var toolMaxTime = "-";
                        var toolAttribute = "-";
                        var ShankSpecificationID = rows.First().ShankSpecificationID;
                        var ShankSpecificationNum = JDJS_WMS_Device_Info.JDJS_WMS_Tool_Shanks_Specification_Table.Where(r => r.ID == ShankSpecificationID).First().ToolShankSpecificationNum.ToString();
                        while (ShankSpecificationNum.Length < 3)
                        {
                            ShankSpecificationNum = "0" + ShankSpecificationNum;
                        }
                        var shankNum = rows.First().ShankNum.ToString();
                        while (shankNum.Length < 4)
                        {
                            shankNum = "0" + shankNum;
                        }
                        var toolShank = ShankSpecificationNum + shankNum;
                        var JDJS_WMS_ToolHolder_Tool_Table = JDJS_WMS_Device_Info.JDJS_WMS_ToolHolder_Tool_Table.Where(r => r.ID == toolId);
                        if (JDJS_WMS_ToolHolder_Tool_Table.Count() > 0)
                        {
                            var VerdorID = JDJS_WMS_ToolHolder_Tool_Table.First().VerdorID;
                             Batch = JDJS_WMS_ToolHolder_Tool_Table.First().Batch;
                            var ToolSpecificationId = JDJS_WMS_ToolHolder_Tool_Table.First().ToolSpecifications;
                             ToolNum = JDJS_WMS_ToolHolder_Tool_Table.First().ToolNum.ToString();
                            while (ToolNum.Length < 5)
                            {
                                ToolNum = "0" + ToolNum;
                            }
                            if (JDJS_WMS_Device_Info.JDJS_WMS_Tool_Verdor_Table.Where(r => r.ID == VerdorID).Count() > 0)
                            {
                                Verdor = JDJS_WMS_Device_Info.JDJS_WMS_Tool_Verdor_Table.Where(r => r.ID == VerdorID).First().VerdorNum;
                                if (JDJS_WMS_Device_Info.JDJS_WMS_Tool_Stock_History.Where(r => r.Id == ToolSpecificationId).Count() > 0)
                                {
                                    ToolSpecification = JDJS_WMS_Device_Info.JDJS_WMS_Tool_Stock_History.Where(r => r.Id == ToolSpecificationId).First().num.ToString();
                                    while (ToolSpecification.Length < 4)
                                    {
                                        ToolSpecification = "0" + ToolSpecification;
                                    }
                                    toolIdentity = Verdor + Batch + ToolSpecification + ToolNum;
                                }
                            }
                           
                          
                           

                            

                        }
                        var toolLifes = JDJS_WMS_Device_Info.JDJS_WMS_ToolHolder_ToolLife_History_Table.Where(r => r.ToolID == toolId);
                        if (toolLifes.Count() > 0)
                        {
                            toolCurrTime = toolLifes.OrderByDescending(r => r.Time).First().ToolCurrLife.ToString();
                            toolMaxTime = toolLifes.OrderByDescending(r => r.Time).First().ToolMaxLife.ToString();
                        }
                        var toolNu = "T" + i.ToString();
                        var JDJS_WMS_Tool_Standard_Table = JDJS_WMS_Device_Info.JDJS_WMS_Tool_Standard_Table.Where(r => r.MachTypeID == cncType & r.ToolID == toolNu);
                        if (JDJS_WMS_Tool_Standard_Table.Count() > 0)
                        {
                            toolAttribute = "常规";
                        }
                        else
                        {
                            toolAttribute = "特殊";
                        }
                        toolList.Add(new ToolInfo
                        {
                            toolID = i.ToString(),
                            toolNum=i,
                            toolLength = rows.First().ToolLength.ToString(),
                            cncId = CncID.ToString(),
                            toolCurrTime = toolCurrTime,
                            toolMaxTime = toolMaxTime,
                            toolIdentity= toolIdentity,
                            toolAttribute = toolAttribute,
                            existence="有",
                            
                            toolShank="TS-"+toolShank
                        });
                    }
                    else
                    {
                        toolList.Add(new ToolInfo
                        {
                            toolID = i.ToString(),
                            toolNum = i,
                            toolLength = "-",
                            cncId = CncID.ToString(),
                            toolCurrTime = "-",
                            toolMaxTime = "-",
                            toolAttribute = "-",
                            toolIdentity = "-",
                            existence = "无",
                          
                            toolShank = "-"
                        });
                    }
                }
               




            }
            var page = int.Parse(context.Request["page"]);
            var limit = int.Parse(context.Request["limit"]);
            toolList = toolList.OrderBy(r => r.toolNum).ToList();
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
        public string toolID;
        public int toolNum;
        public string toolMaxTime;
        public string toolCurrTime;
      public string  toolShank;
      
        public string toolLength;
        public string cncId;
        public string toolAttribute;
        public string toolIdentity;
        public string existence;
    }
}