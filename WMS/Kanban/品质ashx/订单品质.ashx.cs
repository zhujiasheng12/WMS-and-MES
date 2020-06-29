using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Kanban.品质ashx
{
    /// <summary>
    /// 订单品质 的摘要说明
    /// </summary>
    public class 订单品质 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //订单品质
            List<OrderQualtity> orderQualtities = new List<OrderQualtity>();
            using (JDJS_WMS_DB_USEREntities  wms = new  JDJS_WMS_DB_USEREntities ())
            {
                var qualtitys = wms.JDJS_WMS_Quality_Confirmation_Table;
                foreach (var item in qualtitys)
                {
                    OrderQualtity orderQualtity = new OrderQualtity();
                    int orderID = Convert.ToInt32(item.OrderID);
                    orderQualtity.OrderNum = wms.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == orderID).FirstOrDefault().Order_Number;
                    int good = Convert.ToInt32(item.QualifiedProductNumber);
                    int pool = Convert.ToInt32(item.PefectiveProductNumber);
                    var Process = wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == orderID);
                    var lastProcess = Process.OrderByDescending(r => r.ProcessID).First().ProcessID;
                    var lastProcessId = wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == item.OrderID && r.ProcessID == lastProcess).First().ID;
                    //当前成品数
                    var CurrFinishedProductNumber = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.ProcessID == lastProcessId & r.isFlag == 3).Count();
                    orderQualtity.good = good;
                    orderQualtity.pool = pool;
                    orderQualtity.notCe = CurrFinishedProductNumber - (good + pool);
                    orderQualtities.Insert(0, orderQualtity);
                }

            }
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var json = serializer.Serialize(orderQualtities);
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
    public class OrderQualtity
    {
        public string OrderNum;
        public int good;
        public int pool;
        public int notCe;
    }
}