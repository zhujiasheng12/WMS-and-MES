using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Kanban.工程中心Method
{
    /// <summary>
    /// 订单评估航班表 的摘要说明
    /// </summary>
    public class 订单评估航班表 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            List<OrderEvaluationInfo> orderInfos = new List<OrderEvaluationInfo>();
            try
            {
                
                using (JDJS_WMS_DB_USEREntities wms = new JDJS_WMS_DB_USEREntities())
                {
                    var orders = wms.JDJS_WMS_Order_Entry_Table.Where(r => r.Intention == -3 || r.Intention == -1 || r.Intention == 0 || r.Intention == 1);
                    foreach (var order in orders)
                    {
                        if (order.Intention == 1)
                        {
                            //if (order.virtualProgPersId != null)
                            //{
                            //    var inter = wms.JDJS_WMS_Order_Intention_History_Table.Where(r => r.OrderID == order.Order_ID).FirstOrDefault();
                            //    if (inter != null && inter.SubmitTime != null && Convert.ToDateTime(inter.SubmitTime) > DateTime.Now.AddDays(-7))
                            //    {
                            //        OrderEvaluationInfo orderInfo1 = new OrderEvaluationInfo();
                            //        orderInfo1.createTime = Convert.ToDateTime(order.CreateTime);
                            //        orderInfo1.orderLeader = order.Order_Leader;
                            //        orderInfo1.orderName = order.Product_Name;
                            //        orderInfo1.orderNum = order.Order_Number;
                            //        orderInfo1.orderNumber = order.Product_Output;
                            //        orderInfo1.priority = order.Priority == null ? 1 : Convert.ToInt32(order.Priority);
                            //        orderInfo1.projectName = order.ProjectName;
                            //        orderInfo1.virtualPerson = "";
                            //        if (order.virtualProgPersId != null)
                            //        {
                            //            var staff = wms.JDJS_WMS_Staff_Info.Where(r => r.id == order.virtualProgPersId).FirstOrDefault();
                            //            if (staff != null)
                            //            {
                            //                orderInfo1.virtualPerson = staff.staff;
                            //            }
                            //        }

                            //        orderInfos.Add(orderInfo1);
                                    
                            //    }
                            //}
                            continue;
                        }
                        OrderEvaluationInfo orderInfo = new OrderEvaluationInfo();
                        orderInfo.createTime =Convert.ToDateTime ( order.CreateTime);
                        orderInfo.orderLeader = order.Order_Leader;
                        orderInfo.orderName = order.Product_Name;
                        orderInfo.orderNum = order.Order_Number;
                        orderInfo.orderNumber = order.Product_Output;
                        orderInfo.priority =order.Priority==null? 1: Convert.ToInt32 ( order.Priority);
                        orderInfo.projectName = order.ProjectName;
                        orderInfo.virtualPerson = "";
                        if (order.virtualProgPersId != null)
                        {
                            var staff = wms.JDJS_WMS_Staff_Info.Where(r => r.id == order.virtualProgPersId).FirstOrDefault();
                            if (staff != null)
                            {
                                orderInfo.virtualPerson = staff.staff;
                            }
                        }
                        orderInfo.state = "";
                        switch (Convert .ToInt32 ( order.Intention))
                        { 
                            case -1:
                                orderInfo.state = "评估中";
                                break;
                            case -3:
                                orderInfo.state = "未评估";
                                break;
                            case 0:
                                orderInfo.state = "评估中";
                                break;
                                
                        }
                        orderInfos.Add(orderInfo);
                    }
                }
                orderInfos = orderInfos.OrderByDescending(r => r.priority).ThenBy(r => r.createTime).ToList(); ;
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                var json = serializer.Serialize(orderInfos);
                context.Response.Write("data:"+json+"\n\n");
                context.Response.ContentType = "text/event-stream";

            }
            catch (Exception ex)
            {
                context.Response.Write(ex.Message );
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
    public struct OrderEvaluationInfo
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string orderNum;
        /// <summary>
        /// 项目名
        /// </summary>
        public string projectName;
        /// <summary>
        /// 产品名称
        /// </summary>
        public string orderName;
        /// <summary>
        /// 订单负责人
        /// </summary>
        public string orderLeader;
        /// <summary>
        /// 评估负责人
        /// </summary>
        public string virtualPerson;
        /// <summary>
        /// 优先级
        /// </summary>
        public int priority;
        /// <summary>
        /// 订单数量
        /// </summary>
        public int orderNumber;
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime createTime;
        /// <summary>
        /// 订单状态
        /// </summary>
        public string state;
    }
}