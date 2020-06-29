using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.测试.订单流程状态图
{
    /// <summary>
    /// Handler1 的摘要说明
    /// </summary>
    public class Handler1 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var orderId = 281;
            List<OrderProcessRing> orderProcessRings = new List<OrderProcessRing>();
            using (JDJS_WMS_DB_USEREntities  wms = new JDJS_WMS_DB_USEREntities ())
            {
                var orders = wms.JDJS_WMS_Order_Entry_Table.Where(r => r.Intention != 0&r.Order_ID==orderId);
                foreach (var item in orders)
                {
                    OrderProcessRing orderProcessRing = new OrderProcessRing();
                    orderProcessRing.OrderID = item.Order_ID;
                    orderProcessRing.OrderNum = item.Order_Number;
                    orderProcessRing.ProductDemand = item.Product_Output.ToString();
                    if (item.Intention == 1 || item.Intention == 5 || item.Intention == 6 || item.Intention == 7 || item.Intention == 8)
                    {
                        orderProcessRing.CompletedNum = "0";
                        orderProcessRing.EngineeringTreatment = "0";
                        orderProcessRing.MaterialPreparation = "0";
                        orderProcessRing.OnSiteProduction = "0";
                        orderProcessRing.QqualityInspection = "0";
                        orderProcessRing.GoodNum = "0";
                        orderProcessRing.OrderProcess = "进行中";
                    }
                    else if (item.Intention == 3)
                    {
                        orderProcessRing.CompletedNum = "0";
                        orderProcessRing.EngineeringTreatment = "进行中";
                        orderProcessRing.MaterialPreparation = "0";
                        orderProcessRing.OnSiteProduction = "0";
                        orderProcessRing.QqualityInspection = "0";
                        orderProcessRing.GoodNum = "0";
                        orderProcessRing.OrderProcess = "完成";
                    }
                    else if (item.Intention == 4)
                    {
                        orderProcessRing.CompletedNum = "完成";
                        orderProcessRing.EngineeringTreatment = "完成";
                        orderProcessRing.MaterialPreparation = "0";
                        orderProcessRing.OnSiteProduction = "0";
                        orderProcessRing.QqualityInspection = "0";
                        orderProcessRing.GoodNum = "0";
                        orderProcessRing.OrderProcess = "完成";
                    }
                    else if (item.Intention == 2)
                    {
                        orderProcessRing.OrderProcess = "完成";
                        orderProcessRing.EngineeringTreatment = "完成";
                        orderProcessRing.MaterialPreparation = "0";
                        orderProcessRing.OnSiteProduction = "0";
                        orderProcessRing.CompletedNum = "0";
                        orderProcessRing.QqualityInspection = "0";
                        orderProcessRing.GoodNum = "0";

                        var orderBlanks = wms.JDJS_WMS_Blank_Table.Where(r => r.OrderID == item.Order_ID);
                        int BlankCount = orderBlanks.Count();
                        int count = (item.Product_Output - BlankCount);
                        if (count < 0)
                        {
                            count = 0;
                        }
                        orderProcessRing.MaterialPreparation = count.ToString();
                        var info = wms.JDJS_WMS_Quality_Confirmation_Table.Where(r => r.OrderID == item.Order_ID).FirstOrDefault();
                        if (info != null)
                        {
                            orderProcessRing.CompletedNum = info.DetectionNumber.ToString();
                            orderProcessRing.GoodNum = info.QualifiedProductNumber.ToString();
                        }
                        var processes = wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == item.Order_ID && r.sign != 0);
                        int MaxProcessID = 0;
                        int MaxProcess = 0;
                        foreach (var process in processes)
                        {
                            int processID = Convert.ToInt32(process.ProcessID);
                            if (processID > MaxProcess)
                            {
                                MaxProcessID = Convert.ToInt32(process.ID);
                            }
                        }
                        var overInfo = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.ProcessID == MaxProcessID && r.isFlag == 3);
                        int overCount = overInfo.Count();
                        orderProcessRing.QqualityInspection = (overCount - Convert.ToInt32(orderProcessRing.CompletedNum)).ToString();
                        orderProcessRing.OnSiteProduction = (BlankCount - overCount).ToString();
                    }


                    orderProcessRings.Add(orderProcessRing);
                }
                    var serlizer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var json = serlizer.Serialize(orderProcessRings);
                    context.Response.Write(json);
            }

        









    }

    public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
    public class OrderProcessRing
    {
        /// <summary>
        /// 订单主键ID
        /// </summary>
        public int OrderID;
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNum;
        /// <summary>
        /// 订单处理
        /// </summary>
        public string OrderProcess;
        /// <summary>
        /// 工程处理
        /// </summary>
        public string EngineeringTreatment;
        /// <summary>
        /// 资材准备
        /// </summary>
        public string MaterialPreparation;
        /// <summary>
        /// 现场生产
        /// </summary>
        public string OnSiteProduction;
        /// <summary>
        /// 品质检测
        /// </summary>
        public string QqualityInspection;
        /// <summary>
        /// 产品需求
        /// </summary>
        public string ProductDemand;
        /// <summary>
        /// 已完成数目
        /// </summary>
        public string CompletedNum;
        /// <summary>
        /// 良品数
        /// </summary>
        public string GoodNum;
    }
}