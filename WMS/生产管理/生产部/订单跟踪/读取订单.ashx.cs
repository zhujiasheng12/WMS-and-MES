using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.生产部.订单跟踪
{
    /// <summary>
    /// 读取订单 的摘要说明
    /// </summary>
    public class 读取订单 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Order_Trace_AllOrderInfo> infos = new List<Order_Trace_AllOrderInfo>();
            using (JDJS_WMS_DB_USEREntities wms = new JDJS_WMS_DB_USEREntities())
            {
                var orders = wms.JDJS_WMS_Order_Entry_Table.Where(r => r.Intention != -1 && r.Intention != 1 && r.Intention != 5 && r.Intention != -2);
                foreach (var item in orders)
                {
                    Order_Trace_AllOrderInfo info = new Order_Trace_AllOrderInfo();
                    info.Id = item.Order_ID;
                    info.EndTime = item.Order_Plan_End_Time==null?"-":item.Order_Plan_End_Time.ToString();
                    if (item.Order_Actual_End_Time != null)
                    {
                        info.EndTime = item.Order_Actual_End_Time.ToString();
                    }
                    info.OrderNum = item.Order_Number;
                    info.ProductName = item.Product_Name;
                    info.ProjectName = item.ProjectName;
                    info.IsOver = false;
                    if (item.Intention == 4)
                    {
                        info.IsOver = true;
                    }
                    infos.Add(info);
                }
            }
            var json = serializer.Serialize(infos);
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

    public struct Order_Trace_AllOrderInfo
    { 
        public int Id { get; set; }
        public string OrderNum { get; set; }
        public string ProductName { get; set; }
        public string ProjectName { get; set; }
        public string EndTime { get; set; }
        public bool IsOver { get; set; }

    }
}