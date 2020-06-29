using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.刀具管理.刀具历史
{
    /// <summary>
    /// 下拉框读数据 的摘要说明
    /// </summary>
    public class 下拉框读数据 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            using (JDJS_WMS_DB_USEREntities entities = new JDJS_WMS_DB_USEREntities())
            {
                var rows = entities.JDJS_WMS_ToolHolder_Tool_Table;
                List<ToolHistory> tools = new List<ToolHistory>();
                List<string> vs = new List<string>();
                foreach (var item in rows)
                {


                    var VerdorID = item.VerdorID;
                   var Batch = item.Batch;
                    var ToolSpecificationId = item.ToolSpecifications;
                   var ToolNum = item.ToolNum.ToString();
                    string Verdor = "";
                    string ToolSpecification = "";
                    string toolIdentity = "-";
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







                   
                    vs.Add(toolIdentity);

                }

                var specifications = entities.JDJS_WMS_Tool_Stock_History;
                foreach (var item in specifications)
                {
                    vs.Add(item.KnifeSpecifications);
                }
                vs.Add("未使用");
                vs.Add("待使用");
                vs.Add("使用中");
                vs = vs.Distinct().ToList();
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

                var json = serializer.Serialize(vs);
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