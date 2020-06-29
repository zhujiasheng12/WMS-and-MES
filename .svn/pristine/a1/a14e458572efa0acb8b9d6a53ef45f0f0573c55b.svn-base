using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.工程部
{
    /// <summary>
    /// _VirProcessEdit 的摘要说明
    /// </summary>
    public class _VirProcessEdit : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                var processId = int.Parse(context.Request["processId"]);
                var process = int.Parse(context.Request["process"]);
                var time = context.Request["time"];
                var deviceType = context.Request["deviceType"];
                var cncNumber = context.Request["cncNumber"];
                using (JDJS_WMS_DB_USEREntities entities = new JDJS_WMS_DB_USEREntities())
                {
                    var row = entities.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ProcessID == process & r.sign == 0);
                    if (row.Count() > 0)
                    {
                        //if (row.First().ID != processId)
                        //{
                        //    context.Response.Write("该工序已存在");
                        //    return;
                        //}
                    }
                    var rowEdit = entities.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == processId);
                 
                    rowEdit.First().ProcessID = process;
                    rowEdit.First().ProcessTime = double.Parse(time);
                    rowEdit.First().DeviceType = int.Parse(deviceType);
                    rowEdit.First().MachNumber = int.Parse(cncNumber);
                    entities.SaveChanges();
                    context.Response.Write("ok");
                }
            }
            catch (Exception ex)
            {
                context.Response.Write(ex.Message);
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