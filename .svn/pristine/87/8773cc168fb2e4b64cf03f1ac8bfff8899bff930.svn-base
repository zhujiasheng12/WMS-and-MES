using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Kanban.资材
{
    /// <summary>
    /// 毛坯请求 的摘要说明
    /// </summary>
    public class 毛坯请求 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //毛坯请求
            List<BlankRequair> blankRequairs = new List<BlankRequair>();
            using (JDJS_WMS_DB_USEREntities  wms = new JDJS_WMS_DB_USEREntities ())
            {
                var orders = wms.JDJS_WMS_Order_Entry_Table.Where(r => r.Intention == 2);
                foreach (var item in orders)
                {
                    BlankRequair blankRequair = new BlankRequair();
                    blankRequair.OrderNum = item.Order_Number;
                    DateTime time = Convert.ToDateTime(item.Order_Actual_Start_Time);
                    blankRequair.StartTime = time.ToString();
                    int orderID = item.Order_ID;
                    var process1 = wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == orderID && r.ProcessID == 1 && r.sign != 0).FirstOrDefault();
                    if (process1 != null)
                    {
                        int blankType = Convert.ToInt32(process1.BlankType);
                        if (blankType == 1)
                        {
                            blankRequair.BlankType = "Ⅰ型";
                        }
                        else if (blankType == 2)
                        {
                            blankRequair.BlankType = "Ⅱ型";
                        }
                        else
                        {
                            blankRequair.BlankType = "其他";
                        }
                        blankRequair.BlankSpecification = process1.BlankSpecification;
                        blankRequair.BlankRequairNum = Convert.ToInt32(process1.BlankNumber).ToString();
                        var blankInfo = wms.JDJS_WMS_Order_Blank_Table.Where(r => r.OrderID == orderID).FirstOrDefault();
                        if (blankInfo != null)
                        {
                            int already = Convert.ToInt32(blankInfo.BlanktotalPreparedNumber);
                            blankRequair.BlankAlreadyNum = already.ToString();
                            int pending = Convert.ToInt32(blankRequair.BlankRequairNum) - already;
                            blankRequair.BlankPendingNum = pending.ToString();
                            if (pending > 0)
                            {
                                blankRequair.BlankState = "待备料";
                                blankRequair.EndTime = "-";
                            }
                            else
                            {
                                blankRequair.BlankState = "已完成";
                                var history = wms.JDJS_WMS_Blank_Additional_History_Table.Where(r => r.OrderID == orderID).OrderBy(r => r.AddTime).ToList();
                                if (history.Count > 0)
                                {
                                    blankRequair.EndTime = history.Last().AddTime.ToString();
                                }
                                
                            }
                        }
                    }
                    blankRequairs.Add(blankRequair);

                }

            }
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var model = new { code = 0, data = blankRequairs };
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
    public class BlankRequair
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNum;
        /// <summary>
        /// 毛坯类型
        /// </summary>
        public string BlankType;
        /// <summary>
        /// 毛坯规格
        /// </summary>
        public string BlankSpecification;
        /// <summary>
        /// 毛坯需求总数
        /// </summary>
        public string BlankRequairNum;
        /// <summary>
        /// 毛坯已准备数
        /// </summary>
        public string BlankAlreadyNum;
        /// <summary>
        /// 毛坯待准备数
        /// </summary>
        public string BlankPendingNum;
        /// <summary>
        /// 毛坯状态
        /// </summary>
        public string BlankState;
        /// <summary>
        /// 需求下达时间
        /// </summary>
        public string StartTime;
        /// <summary>
        /// 需求完成时间
        /// </summary>
        public string EndTime;
    }
}