using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Kanban.刀具
{
    /// <summary>
    /// 备刀请求 的摘要说明
    /// </summary>
    public class 备刀请求 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            List<ToolRead> toolReads = new List<ToolRead>();

            using (JDJS_WMS_DB_USEREntities CIE = new JDJS_WMS_DB_USEREntities())
            {
               // using (System.Data.Entity.DbContextTransaction mytran = CIE.Database.BeginTransaction())
                {
                    //try
                    {
                        var orders = CIE.JDJS_WMS_Order_Process_Info_Table.Where(r => r.toolPreparation == 0);
                        //var orders = CIE.JDJS_WMS_Order_Queue_Table.Where(r => r.isFlag != 0);
                        foreach (var item in orders)
                        {
                            var OrderNum = CIE.JDJS_WMS_Order_Entry_Table.Where(R => R.Order_ID == item.OrderID).First();
                            int orderID = Convert.ToInt32(item.OrderID);
                            var process = CIE.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == orderID && r.sign == 1 /*& r.toolPreparation == 0 | r.toolPreparation == 1*/);
                            foreach (var real in process)
                            {
                                var mach = CIE.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.ProcessID == real.ID).ToList();
                                var tools = CIE.JDJS_WMS_Order_Process_Tool_Info_Table.Where(r => r.ProcessID == real.ID).ToList();
                                List<string> toolsNo = new List<string>();
                                var toolStand = CIE.JDJS_WMS_Tool_Standard_Table.Where(r => r.MachTypeID == real.DeviceType).ToList();
                                foreach (var meal in toolStand)
                                {
                                    toolsNo.Add(meal.ToolID);
                                }
                                List<JDJS_WMS_Order_Process_Tool_Info_Table> tool = new List<JDJS_WMS_Order_Process_Tool_Info_Table>();
                                foreach (var mode in tools)
                                {
                                    if (!toolsNo.Contains("T" + mode.ToolNO.ToString()))
                                    {
                                        tool.Add(mode);
                                    }

                                }

                                List<int> machID = new List<int>();
                                foreach (var demo in mach)
                                {
                                    if (!machID.Contains(Convert.ToInt32(demo.CncID)))
                                    {
                                        machID.Add(Convert.ToInt32(demo.CncID));
                                    }
                                }
                                int machNum = machID.Count();
                                int toolNum = tool.Count();
                                if (toolNum > 0)
                                {
                                    int maxNum = Math.Max(machNum, toolNum);
                                    for (int i = 0; i < maxNum; i++)
                                    {
                                        ToolRead toolRead = new ToolRead();
                                        var times = CIE.JDJS_WMS_Order_DelayTime_Table.Where(r => r.OrderID == real.OrderID);
                                        if (times.Count() > 0)
                                        {
                                            toolRead.time = times.FirstOrDefault().ToolTime.ToString();
                                        }
                                        if (i < machNum && i < toolNum)
                                        {
                                            toolRead.order = OrderNum.Order_Number;
                                            toolRead.process = real.ProcessID.ToString();
                                            toolRead.processId = real.ID.ToString();
                                            toolRead.toolPreparation = real.toolPreparation.ToString();
                                            toolRead.toolNumber = machNum;
                                            int cncID = Convert.ToInt32(machID[i]);
                                            var cnc = CIE.JDJS_WMS_Device_Info.Where(r => r.ID == cncID);
                                            toolRead.mach = cnc.First().MachNum;
                                            toolRead.toolNum = tool[i].ToolNO.ToString();
                                            toolRead.toolName = tool[i].ToolName.ToString();
                                            toolRead.toolLength = tool[i].ToolLength.ToString();
                                            toolRead.toolSpecification = "刃长" + tool[i].ToolAroidance.ToString();
                                            toolRead.hiltSpecification = tool[i].Shank.ToString();
                                            toolRead.accurate = "否";

                                        }
                                        else if (i >= machNum && i < toolNum)//刀多
                                        {
                                            toolRead.order = OrderNum.Order_Number;
                                            toolRead.process = real.ProcessID.ToString();
                                            toolRead.toolNumber = machNum;
                                            toolRead.processId = real.ID.ToString();
                                            toolRead.toolPreparation = real.toolPreparation.ToString();
                                            //var cnc = CIE.JDJS_WMS_Device_Info.Where(r => r.ID == mach[i].CncID);
                                            toolRead.mach = "";
                                            toolRead.toolNum = tool[i].ToolNO.ToString();
                                            toolRead.toolName = tool[i].ToolName.ToString();
                                            toolRead.toolLength = tool[i].ToolLength.ToString();
                                            toolRead.toolSpecification = "刃长" + tool[i].ToolAroidance.ToString();
                                            toolRead.hiltSpecification = tool[i].Shank.ToString();
                                            toolRead.accurate = "否";
                                        }
                                        else if (i >= toolNum && i < machNum)//机床多
                                        {
                                            toolRead.order = OrderNum.Order_Number;
                                            toolRead.process = real.ProcessID.ToString();
                                            toolRead.processId = real.ID.ToString();
                                            toolRead.toolPreparation = real.toolPreparation.ToString();
                                            toolRead.toolNumber = machNum;
                                            int cncID = Convert.ToInt32(machID[i]);
                                            var cnc = CIE.JDJS_WMS_Device_Info.Where(r => r.ID == cncID);
                                            toolRead.mach = cnc.First().MachNum;
                                            toolRead.toolNum = "";
                                            toolRead.toolName = "";
                                            toolRead.toolLength = "";
                                            toolRead.toolSpecification = "";
                                            toolRead.hiltSpecification = "";
                                            toolRead.accurate = "否";
                                        }

                                        toolReads.Add(toolRead);
                                    }
                                }
                            }

                        }
                    }


                }
            }
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var sort = toolReads.OrderBy(r => r.toolPreparation);
            var model = new { code = 0, data = sort.Where(r=>r.toolNumber>0) };
            var json = serializer.Serialize(model);
            context.Response.Write("data:"+json+"\n\n");
            context.Response.ContentType = "text/event-stream";
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
    class ToolRead
    {
        public string order;
        public string process;
        public string toolPreparation;
        public string mach;
        public string toolNum;
        public string toolName;
        public string toolSpecification;
        public string hiltSpecification;
        public string toolLength;
        public int toolNumber;
        public string processId;
        public string time;
        public string accurate;
    }
}