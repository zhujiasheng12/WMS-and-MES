using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Kanban.刀具
{
    /// <summary>
    /// 备刀需求 的摘要说明
    /// </summary>
    public class 备刀需求 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //备刀需求
            int alreadyToolNum = 0;
            int pendingToolNum = 0;
            using (JDJS_WMS_DB_USEREntities wms = new JDJS_WMS_DB_USEREntities())
            {
                List<string> ToolNos = new List<string>();

               
                var orders = wms.JDJS_WMS_Order_Entry_Table.Where(r => r.Intention == 2);
                if (orders.Count() > 0)
                {
                    foreach (var item in orders)
                    {
                        int orderID = item.Order_ID;
                        var processes = wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == orderID && r.sign != 0);
                        foreach (var real in processes)
                        {
                            ToolNos.Clear();
                            var standTool = wms.JDJS_WMS_Tool_Standard_Table.Where (r=>r.MachTypeID ==real.DeviceType );
                            foreach (var mode in standTool)
                            {
                                ToolNos.Add(mode.ToolID);
                            }
                            int toolAllNum = 0;
                            int processID = real.ID;
                            var toos = wms.JDJS_WMS_Order_Process_Tool_Info_Table.Where(r => r.ProcessID == processID);
                            foreach (var tool in toos)
                            {
                                int tooNo = Convert.ToInt32(tool.ToolNO);
                                if (!ToolNos.Contains ("T"+tooNo.ToString ()))
                                {
                                    toolAllNum++;
                                }
                            }
                            if (real.toolPreparation == null)
                            {
                                List<int> deviceID = new List<int>();
                                var devices = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.ProcessID == processID);
                                foreach (var device in devices)
                                {
                                    int cncID = Convert.ToInt32(device.CncID);
                                    if (!deviceID.Contains(cncID))
                                    {
                                        deviceID.Add(cncID);
                                    }
                                }
                                int cncNum = deviceID.Count();
                                pendingToolNum += (cncNum * toolAllNum);
                            }
                            else
                            {
                                int toolstate = Convert.ToInt32(real.toolPreparation);
                                var alreadyTool = wms.JDJS_WMS_Order_Tool_Prepare_History_table.Where(r => r.ProcessID == processID).FirstOrDefault();
                                if (alreadyTool != null)
                                {
                                    int num = Convert.ToInt32(alreadyTool.Num);
                                    alreadyToolNum += (num * toolAllNum);
                                }
                            }
                        }
                    }
                }
            }
            var model = new { alreadyToolNum = alreadyToolNum, pendingToolNum = pendingToolNum };
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var json = serializer.Serialize(model);
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
}