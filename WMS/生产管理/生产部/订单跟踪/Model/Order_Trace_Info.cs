using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.生产部.订单跟踪.Model
{
    public class Order_Trace_Info
    {
        public string Name { get; set; }
        public List<Order_Trace_Content_Info> ContentList { get; set; }
        public bool IsOver { get; set; }
    }

    public struct Order_Trace_Content_Info
    {
        public string Content { get; set; }
        public string Person { get; set; }
        public string PlanEndTime { get; set; }
        public string EndTime { get; set; }
        public bool IsOver { get; set; }
    }
}