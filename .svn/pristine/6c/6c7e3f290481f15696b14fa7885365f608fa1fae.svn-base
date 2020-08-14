using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.市场部
{
    /// <summary>
    /// 意向订单编辑读数据 的摘要说明
    /// </summary>
    public class 意向订单编辑读数据 : IHttpHandler
    {
        System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();


        public void ProcessRequest(HttpContext context)
        {
            var orderId = int.Parse(context.Request["orderId"]);
            using(JDJS_WMS_DB_USEREntities entities=new JDJS_WMS_DB_USEREntities())
            {
                var row = from orders in entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == orderId)
                          from guide in entities.JDJS_WMS_Order_Guide_Schedu_Table
                          where orders.Order_ID==guide.OrderID
                          select new
                          {
                              orders.Engine_Program_Manager,
                              orders.Engine_Program_ManagerId,
                              orders.Engine_Status,
                              orders.Engine_Technology_Manager,
                              orders.Engine_Technology_ManagerId,
                              orders.Intention,
                              orders.Order_Actual_End_Time,
                              orders.Order_Actual_Start_Time,
                              orders.Order_ID,
                              orders.Order_Leader,
                              orders.Order_Number,
                              orders.Order_Plan_End_Time,
                              orders.Order_Plan_Start_Time,
                              orders.Order_State,
                              orders.Product_Material,
                              orders.Product_Name,
                              orders.Product_Output,
                              orders.virtualProgPersId,
                              orders.virtualReturnTime,
                              orders.ProjectName ,
                              orders.Remark ,
                              orders.IntentionPlanEndTime ,
                              orders .Priority ,
                            guide.ClientName


                          };
                var json = serializer.Serialize(row);
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
}