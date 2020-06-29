﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Kanban.缓冲区看板
{
    /// <summary>
    /// 上下料今日概况 的摘要说明
    /// </summary>
    public class 上下料今日概况 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            double inNumAll = 0;
            int outNumAll = 0;
            DateTime startTime = DateTime.Now.Date;
            DateTime endTime = DateTime.Now;
            using (JDJS_WMS_DB_USEREntities wms = new JDJS_WMS_DB_USEREntities())
            {
                var inHistory = wms.JDJS_WMS_Warehouse_OutBlank_History_Table.Where(r => r.Time > startTime && r.Time < endTime);
                foreach (var item in inHistory)
                {
                    int processId = Convert.ToInt32(item.Process);
                    var process = wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == processId).FirstOrDefault();
                    if (process != null)
                    {
                        int inNum = Convert.ToInt32(item.OutNum);
                        var order = wms.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == process.OrderID).FirstOrDefault();
                        if (order != null)
                        {
                            outNumAll += inNum;
                        }
                    }
                }
            }

            
            using (JDJS_WMS_DB_USEREntities wms = new JDJS_WMS_DB_USEREntities())
            {
                var inHistory = wms.JDJS_WMS_Warehouse_InBlank_History_Table.Where(r => r.Time > startTime && r.Time < endTime);
                foreach (var item in inHistory)
                {
                    int processId = Convert.ToInt32(item.Process);
                    var process = wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == processId).FirstOrDefault();
                    if (process != null)
                    {
                        int inNum = Convert.ToInt32(item.InNum);
                        var order = wms.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == process.OrderID).FirstOrDefault();
                        if (order != null)
                        {
                            inNumAll += inNum;
                        }
                    }
                }
            }

            var model = new { inNum = inNumAll, outNum = outNumAll };
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var json = serializer.Serialize(model);
            context.Response.ContentType = "text/event-stream";
            context.Response.Write("data:"+json+"\n\n");
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