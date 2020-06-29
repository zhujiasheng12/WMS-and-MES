using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Kanban.缓冲区看板
{
    /// <summary>
    /// 今日来料 的摘要说明
    /// </summary>
    public class 今日来料 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            List<TodayTheIncoming> comings = new List<TodayTheIncoming>();
            DateTime startTime = DateTime.Now.Date;
            DateTime endTime = DateTime.Now;
            using (JDJS_WMS_DB_USEREntities wms = new JDJS_WMS_DB_USEREntities())
            {
                var inHistory = wms.JDJS_WMS_Warehouse_InBlank_History_Table.Where(r => r.Time > startTime && r.Time < endTime);
                foreach (var item in inHistory)
                {
                    int processId =Convert.ToInt32 ( item.Process);
                    var process = wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == processId).FirstOrDefault();
                    if (process != null)
                    {
                        int inNum = Convert.ToInt32(item.InNum);
                        var order = wms.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == process.OrderID).FirstOrDefault();
                        if (order != null)
                        {
                            var come = comings.Where(r => r.processId == process.ID).FirstOrDefault();
                            if (come == null)
                            {
                                TodayTheIncoming comeing = new TodayTheIncoming();
                                comeing.count = inNum;
                                comeing.orderNum = order.Order_Number;
                                comeing.processId = process.ID;
                                comeing.productName = order.Product_Name;
                                comeing.projectName = order.ProjectName;
                                comeing.time = Convert.ToDateTime(item.Time);
                                comeing.materialType = "";
                                var work = wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == process.OrderID && r.ProcessID > process.ProcessID);
                                if (work.Count() < 1)
                                {
                                    comeing.materialType = "成品料";
                                }
                                else
                                {
                                    switch (process.ProcessID)
                                    {
                                        case 1:
                                            comeing.materialType = "一序完成料";
                                            break;
                                        case 2:
                                            comeing.materialType = "二序完成料";
                                            break;
                                        case 3:
                                            comeing.materialType = "三序完成料";
                                            break;
                                        case 4:
                                            comeing.materialType = "四序完成料";
                                            break;
                                        case 5:
                                            comeing.materialType = "五序完成料";
                                            break;
                                        case 6:
                                            comeing.materialType = "六序完成料";
                                            break;

                                    }
                                }
                                comings.Add(comeing);
                            }
                            else
                            {
                                come.count += inNum;
                                var time= Convert.ToDateTime(item.Time);
                                if (time > come.time)
                                {
                                    come.time = time;
                                }
                            }
                        }
                    }
                }
            }
            comings = comings.OrderByDescending(r => r.time).ToList ();
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var json = serializer.Serialize(comings);
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

    public class TodayTheIncoming
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
        /// 工序Id
        /// </summary>
        public int processId;
        /// <summary>
        /// 来料类型
        /// </summary>
        public string materialType;
        /// <summary>
        /// 来料数量
        /// </summary>
        public int count;
        /// <summary>
        /// 来料时间
        /// </summary>
        public DateTime time;
    }
}