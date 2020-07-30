using DocumentFormat.OpenXml.Drawing.Charts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.生产管理.生产部.订单跟踪.Model;

namespace WebApplication2.生产管理.生产部.订单跟踪
{
    /// <summary>
    /// 读取订单 的摘要说明
    /// </summary>
    public class 读取订单 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var str = context.Request.Form["str"];
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Order_Trace_AllOrderInfo> infos = new List<Order_Trace_AllOrderInfo>();
            using (JDJS_WMS_DB_USEREntities wms = new JDJS_WMS_DB_USEREntities())
            {
                var orders = wms.JDJS_WMS_Order_Entry_Table.Where(r => r.Intention != -1 && r.Intention != 1 && r.Intention != 5 && r.Intention != -2 && r.Intention != -3&& (r.Order_Leader .Contains (str)||r.Order_Number .Contains (str)||r.Product_Name .Contains (str)||r.ProjectName .Contains (str)));
                foreach (var item in orders)
                {
                    Order_Trace_AllOrderInfo info = new Order_Trace_AllOrderInfo();
                    info.Leader = item.Order_Leader;
                    info.Output = item.Product_Output;
                    info.Id = item.Order_ID;
                    info.State = "生产中";
                    info.orderState = OrderState.生产中;
                    info.PlanEndTime = item.Order_Plan_End_Time==null?"-":item.Order_Plan_End_Time.ToString();
                    if (item.Order_Actual_End_Time != null)
                    {
                        info.EndTime = item.Order_Actual_End_Time.ToString();
                    }
                    else
                    {
                        info.EndTime = "-";
                    }
                    info.OrderNum = item.Order_Number;
                    info.ProductName = item.Product_Name;
                    info.ProjectName = item.ProjectName;
                    info.IsOver = false;
                    if (item.Intention == 4)
                    {
                        info.IsOver = true;
                    }
                    var info2 = DataManage.GetWorkInfo(item.Order_ID);
                    if (info2.waitNum > 0)
                    {
                        info.State = "生产中";
                        info.orderState = OrderState.生产中;
                    }
                    else if (info2.waitNum == 0 && info2.Finish == 0)
                    {
                        info.State = "生产中";
                        info.orderState = OrderState.生产中;
                    }
                    var info1 = DataManage.GetInfo(item.Order_ID);
                    if (info1[0].IsOver == false)
                    {
                        info.State = "下单中";
                        info.orderState = OrderState.下单中;
                    }
                    else if (info1[0].IsOver == true && info1[1].IsOver == false)
                    {
                        info.State = "编程中";
                        info.orderState = OrderState.编程中;
                    }
                    else if (info1[0].IsOver == true && info1[1].IsOver == true && info1[2].IsOver == false)
                    {
                        info.State = "待生产";
                        info.orderState = OrderState.待生产;
                    }
                    if (item.Intention == 4)
                    {
                        info.State = "已完成";
                        info.orderState = OrderState.已完成;
                    }
                    


                    infos.Add(info);
                }
            }
            infos = infos.OrderBy(r => r.orderState).ToList(); ;
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
        public int Output { get; set; }
        public string EndTime { get; set; }
        public string PlanEndTime { get; set; }
        public bool IsOver { get; set; }
        public string State { get; set; }
        public OrderState orderState { get; set; }

        private string _leader;
        public string Leader 
        { 
            get 
            { 
                return _leader; 
            } 
            set 
            { 
                _leader = value;
                if (value.Length > 0) 
                {
                    Xing = _leader[0]; 
                } 
            } 
        }
        public char Xing { get; set; }

    }

    public enum OrderState
    { 
        下单中,
        编程中,
        待生产,
        生产中,
        已完成
    }
}