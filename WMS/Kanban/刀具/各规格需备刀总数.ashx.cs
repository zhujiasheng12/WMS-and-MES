using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Kanban.刀具
{
    /// <summary>
    /// 各规格需备刀总数 的摘要说明
    /// </summary>
    public class 各规格需备刀总数 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/event-stream";
           using(JDJS_WMS_DB_USEREntities CIE=new JDJS_WMS_DB_USEREntities())
            {
                List<ToolRead1> toolReads = new List<ToolRead1>();
                System.Web.Script.Serialization.JavaScriptSerializer serializer1 = new System.Web.Script.Serialization.JavaScriptSerializer();

                try
                {
                    var orders = CIE.JDJS_WMS_Order_Queue_Table.Where(r => r.isFlag != 0);

                    foreach (var item in orders)
                    {
                        var OrderNum = CIE.JDJS_WMS_Order_Entry_Table.Where(R => R.Order_ID == item.OrderID).First();
                        int orderID = Convert.ToInt32(item.OrderID);
                        var process = CIE.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == orderID && r.sign == 1 & r.toolPreparation == 0);
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

                                foreach (var machId in machID)
                                {
                                    foreach (var too in tool)
                                    {
                                        ToolRead1 toolRead = new ToolRead1();
                                        var times = CIE.JDJS_WMS_Order_DelayTime_Table.Where(r => r.OrderID == real.OrderID);
                                        if (times.Count() > 0)
                                        {
                                            toolRead.time = times.FirstOrDefault().ToolTime.ToString();

                                        }
                                        toolRead.order = OrderNum.Order_Number;
                                        toolRead.process = real.ProcessID.ToString();
                                        toolRead.processId = real.ID.ToString();
                                        toolRead.toolPreparation = real.toolPreparation.ToString();
                                        toolRead.toolNumber = machNum.ToString();

                                        var cnc = CIE.JDJS_WMS_Device_Info.Where(r => r.ID == machId);
                                        toolRead.mach = cnc.First().MachNum;
                                        toolRead.toolNum = too.ToolNO.ToString();
                                        toolRead.toolName = too.ToolName.ToString();
                                        toolRead.toolLength = too.ToolLength.ToString();
                                        toolRead.toolSpecification = "刃长" + too.ToolAroidance.ToString();
                                        toolRead.hiltSpecification = too.Shank.ToString();
                                        toolReads.Add(toolRead);
                                    }

                                }

                            }
                        }

                    }

                    List<string> vs = new List<string>();
                    foreach (var item in toolReads)
                    {
                        if (!vs.Contains(item.toolName))
                        {
                            vs.Add(item.toolName);
                        }
                    }
                    List<Reads> reads = new List<Reads>();
                   
                    for (int i = 0; i < vs.Count; i++)
                    {
                        var count = toolReads.Where(r => r.toolName == vs[i]).Count();
                            reads.Add(new Reads { toolName = vs[i], count = count }); 
                    }
                    reads = reads.OrderByDescending(r => r.count).ToList();
                    if (reads.Count > 8)
                    {
                        var number = 0;
                        for (int i = 7; i < reads.Count; i++)
                        {
                            number += reads[i].count;

                        }
                      reads= reads.Take(7).ToList();
                        reads.Add(new Reads { toolName = "其他", count = number });
                    }
                    var json = serializer1.Serialize(reads);
                    context.Response.Write("data:" + json + "\n\n");
                    context.Response.ContentType = "text/event-stream";

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                   
                }
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
    class ToolRead1
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
        public string toolNumber;
        public string processId;
        public string time;
       
    }
    class Reads
    {
       public string toolName;
      public int count;
    }
}