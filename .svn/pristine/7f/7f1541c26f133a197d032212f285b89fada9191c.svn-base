using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.刀具管理.备刀管理
{
    /// <summary>
    /// processRead 的摘要说明
    /// </summary>
    public class processRead : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var orderId = int.Parse(context.Request["orderId"]);
            using(JDJS_WMS_DB_USEREntities entities=new JDJS_WMS_DB_USEREntities())
            {
                
                var processes = entities.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == orderId/* & r.toolPreparation != -1*/);
                
                List<List> lists = new List<List>();
                foreach (var item in processes)
                {
                    var standards = entities.JDJS_WMS_Tool_Standard_Table.Where (r=>r.MachTypeID ==item.DeviceType);
                    var tools = entities.JDJS_WMS_Order_Process_Tool_Info_Table.Where(r =>r.ProcessID== item.ID);
                   

                    foreach (var tool in tools)
                    {
                        var ToolNO = "T" + tool.ToolNO.ToString();
                        var judge = standards.Where(r => r.ToolID == ToolNO);
                        if (judge.Count() == 0)
                        {
                            lists.Add(new List { ProcessID = item.ProcessID.ToString() });
                        }
                    }

                }
               
                

          
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                var last = lists.Where((r, i) => lists.FindIndex(p => p.ProcessID == r.ProcessID) == i);
                var json = serializer.Serialize(last);
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
    class List
    {
        public string ProcessID;
    }
}