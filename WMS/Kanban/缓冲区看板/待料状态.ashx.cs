﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Kanban.缓冲区看板
{
    /// <summary>
    /// 待料状态 的摘要说明
    /// </summary>
    public class 待料状态 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            List<WorkStatus> workStatuses = new List<WorkStatus>();
            using (JDJS_WMS_DB_USEREntities wms = new JDJS_WMS_DB_USEREntities())
            {
                var orders = wms.JDJS_WMS_Order_Entry_Table.Where(r => (r.Intention == 2 || r.Intention == 3) && r.ProofingORProduct == -1);
                foreach (var order in orders)
                {
                    int blankNum = 0;
                    blankNum = Convert.ToInt32(order.Product_Output);
                    bool isFirst = true;
                    var processes = wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == order.Order_ID && r.sign != 0).OrderBy(r => r.ProcessID);
                    foreach (var process in processes)
                    {
                        var scheduing = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.ProcessID == process.ID && r.isFlag == 2);
                        if (scheduing.Count() > 0)
                        {
                            //这个工序有正在干的
                            int outNum = 0;
                            var blank = wms.JDJS_WMS_Warehouse_InOut_History_Table.Where(r => r.ProcessId == process.ID).FirstOrDefault();
                            if (blank != null)
                            {
                                outNum = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(blank.OutNum)));
                            }
                            if (outNum < blankNum)
                            {
                                WorkStatus status = new WorkStatus();
                                var fix = wms.JDJS_WMS_Device_Status_Table.Where(r => r.ID == process.JigType).FirstOrDefault();
                                if (fix != null)
                                {
                                    status.fixType = fix.Status + "(" + fix.explain + ")";
                                }
                                status.orderNum = order.Order_Number;
                                status.productName = order.Product_Name;
                                status.projectName = order.ProjectName;
                                status.requiredNum = blankNum - outNum;
                                if (isFirst)
                                {
                                    status.materialType = "毛坯料";

                                }
                                else
                                {
                                    switch (process.ProcessID)
                                    {
                                        case 1:
                                            status.materialType = "毛坯料";
                                            break;
                                        case 2:
                                            status.materialType = "一序完成料";
                                            break;
                                        case 3:
                                            status.materialType = "二序完成料";
                                            break;
                                        case 4:
                                            status.materialType = "三序完成料";
                                            break;
                                        case 5:
                                            status.materialType = "四序完成料";
                                            break;
                                        case 6:
                                            status.materialType = "五序完成料";
                                            break;
                                    }
                                }
                                workStatuses.Add(status);
                            }
                        }
                        else
                        {
                            bool isWork = false;
                            var devices = wms.JDJS_WMS_Device_Info;
                            foreach (var device in devices)
                            {
                                var work = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.CncID == device.ID && r.isFlag == 1).OrderBy(r => r.StartTime).FirstOrDefault();
                                if (work != null)
                                {
                                    if (work.ProcessID == process.ID)
                                    {
                                        isWork = true;
                                        break;
                                    }
                                }
                            }
                            if (isWork)
                            {
                                int outNum = 0;
                                var outBlank = wms.JDJS_WMS_Warehouse_InOut_History_Table.Where(r => r.ProcessId == process.ID).FirstOrDefault();
                                if (outBlank != null)
                                {
                                    outNum = Convert.ToInt32(outBlank.OutNum);
                                }
                                if (outNum == 0)
                                {
                                    WorkStatus status = new WorkStatus();
                                    var fix = wms.JDJS_WMS_Device_Status_Table.Where(r => r.ID == process.JigType).FirstOrDefault();
                                    if (fix != null)
                                    {
                                        status.fixType = fix.Status + "(" + fix.explain + ")";
                                    }
                                    status.orderNum = order.Order_Number;
                                    status.productName = order.Product_Name;
                                    status.projectName = order.ProjectName;
                                    status.requiredNum = blankNum;
                                    if (isFirst)
                                    {
                                        status.materialType = "毛坯料";

                                    }
                                    else
                                    {
                                        switch (process.ProcessID)
                                        {
                                            case 1:
                                                status.materialType = "毛坯料";
                                                break;
                                            case 2:
                                                status.materialType = "一序完成料";
                                                break;
                                            case 3:
                                                status.materialType = "二序完成料";
                                                break;
                                            case 4:
                                                status.materialType = "三序完成料";
                                                break;
                                            case 5:
                                                status.materialType = "四序完成料";
                                                break;
                                            case 6:
                                                status.materialType = "五序完成料";
                                                break;
                                        }
                                    }
                                    workStatuses.Add(status);
                                }
                            }
                        }

                        blankNum = blankNum * Convert.ToInt32(process.Modulus);
                        isFirst = false;
                    }
                }
            }
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var json = serializer.Serialize(workStatuses);
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
    /// <summary>
    /// 待料状态
    /// </summary>
    public class WorkStatus
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        public string projectName;
        /// <summary>
        /// 产品名称
        /// </summary>
        public string productName;
        /// <summary>
        /// 订单号
        /// </summary>
        public string orderNum;
        /// <summary>
        /// 待料状态
        /// </summary>
        public string materialType;
        /// <summary>
        /// 治具类型
        /// </summary>
        public string fixType;
        /// <summary>
        /// 需求数量
        /// </summary>
        public double requiredNum;

    }
}