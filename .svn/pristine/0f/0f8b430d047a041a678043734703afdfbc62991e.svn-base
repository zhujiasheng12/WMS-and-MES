using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Kanban.订单ashx
{
    /// <summary>
    /// 订单数目 的摘要说明
    /// </summary>
    public class 订单数目 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //订单进度看板读取
            int AllOrderNum = 0;
            int processingOrderNum = 0;
            int pocessWillOrderNum = 0;
            int NowadaysProductNum = 0;
            double PassRate = 0;
            using (JDJS_WMS_DB_USEREntities wms = new  JDJS_WMS_DB_USEREntities ())
            {
                var orders2 = wms.JDJS_WMS_Order_Entry_Table.Where(r => r.Intention == 2);
                var orders3 = wms.JDJS_WMS_Order_Entry_Table.Where(r => r.Intention == 3);
                AllOrderNum = orders2.Count() + orders3.Count();
                pocessWillOrderNum = orders3.Count();
                foreach (var item in orders2)
                {
                    int orderID = item.Order_ID;
                    var num1 = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.OrderID == orderID && r.isFlag == 1);
                    var num2 = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.OrderID == orderID && r.isFlag == 2);
                    var num3 = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.OrderID == orderID && r.isFlag == 3);
                    if (num1.Count() > 0 && num2.Count() < 1 && num3.Count() < 1)
                    {
                        pocessWillOrderNum++;
                    }
                    else if (num2.Count() > 0 || (num1.Count() > 0 && num3.Count() > 0))
                    {
                        processingOrderNum++;
                    }
                }
                var overProcess = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.isFlag == 3);
                DateTime nowTime = DateTime.Now;
                string timeStr = nowTime.Year.ToString() + "-" + nowTime.Month.ToString() + "-" + nowTime.Day.ToString();
                foreach (var item in overProcess)
                {
                    DateTime endTime = Convert.ToDateTime(item.EndTime);
                    string overTimeStr = endTime.Year.ToString() + "-" + endTime.Month.ToString() + "-" + endTime.Day.ToString();
                    if (overTimeStr == timeStr)
                    {
                        NowadaysProductNum++;
                    }
                }
                int GoodNum = 0;
                int PoolNum = 0;
                var qualtity = wms.JDJS_WMS_Quality_Confirmation_History_Table.ToList();
                foreach (var item in qualtity)
                {
                    DateTime endTime = Convert.ToDateTime(item.OperateTime);
                    string overTimeStr = endTime.Year.ToString() + "-" + endTime.Month.ToString() + "-" + endTime.Day.ToString();
                    if (overTimeStr == timeStr)
                    {
                        GoodNum += Convert.ToInt32(item.GoodNum);
                        PoolNum += Convert.ToInt32(item.PoolNum);
                    }
                }
                if(GoodNum + PoolNum == 0)
                {
                    PassRate = 0;
                }
                else
                {
                    PassRate = GoodNum / (GoodNum + PoolNum);
                }
             
            }
            var mode = new { orderAllNum = AllOrderNum, processingOrderNum = processingOrderNum, processWillNum = pocessWillOrderNum, NowadaysProductNum = NowadaysProductNum, PassRate = PassRate };
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var json = serializer.Serialize(mode);
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
}