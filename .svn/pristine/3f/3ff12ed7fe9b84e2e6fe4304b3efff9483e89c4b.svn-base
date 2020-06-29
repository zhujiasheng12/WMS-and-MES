using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.刀具管理.备刀管理
{
    /// <summary>
    /// completeToolPreparation 的摘要说明
    /// </summary>
    public class completeToolPreparation : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var processId = int.Parse(context.Request["processId"]);
            var toolNumber = int.Parse(context.Request["toolNumber"]);
            using (JDJS_WMS_DB_USEREntities entities=new JDJS_WMS_DB_USEREntities())
            {
                JDJS_WMS_Order_Tool_Prepare_History_table tool = new JDJS_WMS_Order_Tool_Prepare_History_table()
                {
                    ProcessID =processId ,
                    Num =toolNumber ,
                    PrepareTime =DateTime.Now,
                };
                entities.JDJS_WMS_Order_Tool_Prepare_History_table.Add(tool);
                entities.SaveChanges();
                var row = entities.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == processId);
                if (row.Count() > 0)
                {
                    row.First().toolPreparation = 1;
                }
                entities.SaveChanges();
                context.Response.Write("ok");
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