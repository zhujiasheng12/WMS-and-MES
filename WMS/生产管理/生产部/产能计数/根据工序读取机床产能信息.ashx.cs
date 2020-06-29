﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.生产部.产能计数
{
    /// <summary>
    /// 根据工序读取机床产能信息 的摘要说明
    /// </summary>
    public class 根据工序读取机床产能信息 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            int all = 0;
            int syatemAll = 0;
            var processId = int.Parse(context.Request["processId"]);
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<MachineProductivityInformation> machInfos = new List<MachineProductivityInformation>();
            using (JDJS_WMS_DB_USEREntities wms = new JDJS_WMS_DB_USEREntities())
            {
                var works = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.ProcessID == processId&&r.isFlag !=0);
                foreach (var item in works)
                {
                    MachineProductivityInformation mach = new MachineProductivityInformation();
                    mach.cncId =Convert.ToInt32 ( item.CncID);
                    mach.cncNum = wms.JDJS_WMS_Device_Info.Where(r => r.ID == item.CncID).First().MachNum;
                    mach.endTime =Convert .ToDateTime ( item.EndTime);
                    mach.endTimeStr = item.EndTime.ToString();
                    mach.id = item.ID;
                    mach.orderId =Convert.ToInt32 ( item.OrderID);
                    mach.processId =Convert.ToInt32 ( item.ProcessID);
                    mach.startTime =Convert .ToDateTime ( item.StartTime);
                    mach.startTimeStr = item.StartTime.ToString();
                    all+=(item.WorkCount ==null?0: Convert .ToInt32 ( item.WorkCount));
                    syatemAll += (item.SystemCount == null ? 0 : Convert.ToInt32(item.SystemCount));
                    mach.workCount =item.WorkCount ==null?0: Convert .ToInt32 ( item.WorkCount);
                    mach.workCountStr = mach.workCount.ToString();
                    mach.systemCount = mach.systemCount == null ? 0 : Convert.ToInt32(item.SystemCount);
                    machInfos.Add(mach);
                }
            }
            var model = new { code = 0, msg = "", count = machInfos.Count, data = machInfos ,all=all,systemAll=syatemAll  };
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
    public struct MachineProductivityInformation
    {
        /// <summary>
        /// 加工主键ID
        /// </summary>
        public int id;
        /// <summary>
        /// 机床主键Id
        /// </summary>
        public int cncId;
        /// <summary>
        /// 订单主键
        /// </summary>
        public int orderId;
        /// <summary>
        /// 工序主键
        /// </summary>
        public int processId;
        /// <summary>
        /// 机床编号
        /// </summary>
        public string cncNum;
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime startTime;
        /// <summary>
        /// 开始时间字符串
        /// </summary>
        public string startTimeStr;
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime endTime;
        /// <summary>
        /// 结束时间字符串
        /// </summary>
        public string endTimeStr;
        /// <summary>
        /// 工件计数
        /// </summary>
        public int workCount;
        /// <summary>
        /// 工件计数字符串
        /// </summary>
        public string workCountStr;
        /// <summary>
        /// 系统工件计数
        /// </summary>
        public int systemCount;
    }
}