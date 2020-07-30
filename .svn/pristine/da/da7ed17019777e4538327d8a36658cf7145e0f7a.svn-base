using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Kanban.订单状态
{
    /// <summary>
    /// 加工任务提醒 的摘要说明
    /// </summary>
    public class 加工任务提醒 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<WorkTaskRemind> infos = new List<WorkTaskRemind>();
            using (JDJS_WMS_DB_USEREntities model = new JDJS_WMS_DB_USEREntities())
            {
                var works = model.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.isFlag == 1).OrderBy(r => r.StartTime);
                foreach (var item in works)
                {
                    if (infos.Where(r => r.CncId == item.CncID).Count() > 0)
                    {
                        continue;//如果该机床已经包含则退出
                    }
                    WorkTaskRemind info = new WorkTaskRemind()
                    {
                        CncId = 0,
                        CncNum = "-",
                        BlankPrepareFlag = false,
                        IsWork = false,
                        JiaPrepareFlag = true,
                        Location = "-",
                        ProductName = "-",
                        ProjectName = "-",
                        TaskNum = "-",
                        time = DateTime.Now,
                        ToolPrepareFlag = true
                    };
                    info.CncId = Convert.ToInt32(item.CncID);
                    var cnc = model.JDJS_WMS_Device_Info.Where(r => r.ID == item.CncID).FirstOrDefault();
                    if (cnc != null)
                    {
                        info.CncNum = cnc.MachNum;
                        var location = model.JDJS_WMS_Location_Info.Where(r => r.id == cnc.Position).FirstOrDefault();
                        if (location != null)
                        {
                            info.Location = location.Name;
                        }
                    }
                    var order = model.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == item.OrderID).FirstOrDefault();
                    var process = model.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == item.ProcessID).FirstOrDefault();
                    info.TaskNum = $"{order.Order_Number }-P{process.ProcessID}";
                    info.ProductName = order.Product_Name;
                    info.ProjectName = order.ProjectName;
                    info.time =Convert.ToDateTime ( item.StartTime);
                    var row = model.JDJS_WMS_Order_Blank_Table.Where(r => r.OrderID == order.Order_ID).FirstOrDefault();
                    if (row != null)
                    {
                        if (row.BlankState == "已完成")
                        {
                            info.BlankPrepareFlag = true;
                        }
                    }

                    var fixtureInfo =model.JDJS_WMS_Order_Fixture_Manager_Table.Where(r => r.ProcessID == item.ProcessID);
                    if (fixtureInfo.Count() > 0)
                    {
                        int DeviceNum = 0;
                        int preparenum = 0;
                        if (fixtureInfo.First().FixtureNumber == null)
                        {
                            var cncs = model.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.ProcessID == item.ID && r.isFlag != 0);
                            List<int> cncid = new List<int>();
                            foreach (var real in cncs)
                            {
                                if (!cncid.Contains(Convert.ToInt32(real.CncID)))
                                {
                                    cncid.Add(Convert.ToInt32(real.CncID));
                                }
                            }
                            DeviceNum = cncid.Count();
                        }
                        else
                        {
                            DeviceNum = Convert.ToInt32(fixtureInfo.First().FixtureNumber);
                        }
                        var over = model.JDJS_WMS_Fixture_Additional_History_Table.Where(r => r.ProcessID == item.ID);
                        foreach (var real in over)
                        {
                            preparenum += Convert.ToInt32(real.AddNum);
                        }
                        if (DeviceNum > preparenum)
                        {
                            info.JiaPrepareFlag = false;
                        }


                    }

                    #region 刀具
                    int cncID =Convert.ToInt32 ( item.CncID);
                    #region 是否完成
                    List<int> shankToolNums = new List<int>();
                    List<int> ToolStandInfos = new List<int>();
                    List<int> ProcessToolInfos = new List<int>();
                    Dictionary<int, string> shanlToolInfo = new Dictionary<int, string>();
                    Dictionary<int, string> ProcessNeedToolInfo = new Dictionary<int, string>();
                    {
                        var cncs = model.JDJS_WMS_Device_Info.Where(r => r.ID == cncID);
                        var shangs = model.JDJS_WMS_Tool_Shank_Table.Where(r => r.CncID == cncID);
                        foreach (var real in shangs)
                        {
                            var toolID = real.ToolID;
                            var sp = model.JDJS_WMS_ToolHolder_Tool_Table.Where(r => r.ID == toolID).FirstOrDefault();
                            if (sp != null)
                            {
                                var spID = sp.ToolSpecifications;
                                var spinfo = model.JDJS_WMS_Tool_Stock_History.Where(r => r.Id == spID).FirstOrDefault();
                                if (spinfo != null)
                                {
                                    if (!shanlToolInfo.ContainsKey(Convert.ToInt32(real.ToolNum)))
                                    {
                                        shanlToolInfo.Add(Convert.ToInt32(real.ToolNum), spinfo.KnifeName);
                                    }
                                }
                            }
                            shankToolNums.Add(Convert.ToInt32(real.ToolNum));

                        }
                       
                        {
                            int processID = Convert.ToInt32(item.ProcessID);
                            var cncTypeID = cncs.FirstOrDefault().MachType;
                            var standTool = model.JDJS_WMS_Tool_Standard_Table.Where(r => r.MachTypeID == cncTypeID);
                            foreach (var real in standTool)
                            {
                                var toolnumstr = real.ToolID;
                                if (toolnumstr.Length > 1)
                                {
                                    int toolNum = Convert.ToInt32(toolnumstr.Substring(1));

                                    ToolStandInfos.Add(toolNum);
                                }
                            }
                            var processTools = model.JDJS_WMS_Order_Process_Tool_Info_Table.Where(r => r.ProcessID == processID);
                            foreach (var real in processTools)
                            {
                                if (!ToolStandInfos.Contains(Convert.ToInt32(real.ToolNO)))
                                {
                                    ProcessToolInfos.Add(Convert.ToInt32(real.ToolNO));
                                    var ToolSTR = real.ToolName;
                                    int index0 = ToolSTR.IndexOf("[");
                                    int index1 = ToolSTR.IndexOf("]");
                                    if (index1 > index0)
                                    {
                                        ToolSTR = ToolSTR.Substring(index0 + 1, index1 - index0 - 1);
                                        if (!ProcessNeedToolInfo.ContainsKey(Convert.ToInt32(real.ToolNO)))
                                        {
                                            ProcessNeedToolInfo.Add(Convert.ToInt32(real.ToolNO), ToolSTR);
                                        }
                                    }
                                }
                            }
                            foreach (var real in ProcessToolInfos)
                            {
                                if (!shankToolNums.Contains(real))
                                {
                                    info.ToolPrepareFlag = false;
                                }
                            }

                        }
                    }
                    if (info.ToolPrepareFlag)
                    {
                        foreach (var real in ProcessNeedToolInfo)
                        {
                            if (shanlToolInfo.ContainsKey(real.Key))
                            {
                                if (!(shanlToolInfo[real.Key].Contains(real.Value) || real.Value.Contains(shanlToolInfo[real.Key])))
                                {
                                    info.ToolPrepareFlag = false;
                                    break;
                                }
                            }
                            else
                            {
                                info.ToolPrepareFlag = false;
                                break;
                            }
                        }
                    }
                    #endregion
                    #endregion


                    if (info.JiaPrepareFlag && info.ToolPrepareFlag && info.BlankPrepareFlag)
                    {
                        info.IsWork = true;
                    }




                    infos.Add(info);
                }
            }
            infos = infos.OrderBy(r => r.time).ToList();
            var json = serializer.Serialize(infos);
            context.Response.ContentType = "text/event-stream";
            context.Response.Write("data:" + json + "\n\n");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
    public struct WorkTaskRemind
    {
        /// <summary>
        /// 设备主键ID
        /// </summary>
        public int CncId { get; set; }
        /// <summary>
        /// 车间
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// 机台编号
        /// </summary>
        public string CncNum { get; set; }
        /// <summary>
        /// 任务号  2020202020-P1
        /// </summary>
        public string TaskNum { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 上刀情况
        /// </summary>
        public bool ToolPrepareFlag { get; set; }
        /// <summary>
        /// 毛坯备料情况
        /// </summary>
        public bool BlankPrepareFlag { get; set; }
        /// <summary>
        /// 治具准备情况
        /// </summary>
        public bool JiaPrepareFlag { get; set; }
        /// <summary>
        /// 可否加工
        /// </summary>
        public bool IsWork { get; set; }
        /// <summary>
        /// 预计开始加工时间
        /// </summary>
        public DateTime time { get; set; }
    }
}