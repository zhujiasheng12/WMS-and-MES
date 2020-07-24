using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.生产部.订单跟踪.Model
{
    public class Order_Trace_Work_Sche
    {
        public string Name { get; set; }
        public int waitNum { get; set; }
        public int Finish { get; set; }
        public int Good { get; set; }
        public int Storage { get; set; }
    }
}