using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model.生产管理.市场部
{
    /// <summary>
    /// orderEntry 的摘要说明
    /// </summary>
    public class orderIntentionRead : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            using(JDJS_WMS_DB_USEREntities entities =new JDJS_WMS_DB_USEREntities())
            {
                var page = int.Parse(context.Request["page"]);
                var limit = int.Parse(context.Request["limit"]);
                var rows = from orders in entities.JDJS_WMS_Order_Entry_Table
                           from history in entities.JDJS_WMS_Order_Intention_History_Table
                           from client in entities.JDJS_WMS_Order_Guide_Schedu_Table
                           where orders.Order_ID == history.OrderID&client.OrderID==orders.Order_ID
                           
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
                                orders .Priority ,
                                
                                client.ClientName
                           };



               List< OrderRead >order = new List<OrderRead>();
                foreach (var item in rows)
                {
                    
                    var virtualProgPers = "";
                    var virtualProgPersRow = entities.JDJS_WMS_Staff_Info.Where(r => r.id == item.virtualProgPersId);
                    if (virtualProgPersRow.Count() > 0)
                    {
                        virtualProgPers = virtualProgPersRow.First().staff;
                    }
                    //var virtualReturnTime = "";
                    //var virtualReturnTimeList = entities.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.OrderID == item.Order_ID);
                    //if (virtualReturnTimeList.Count() > 0)
                    //{
                    //    virtualReturnTime = virtualReturnTimeList.OrderByDescending(r => r.EndTime).First().EndTime.ToString();
                    //}

                    var virtualReturnTime = "";
                    double time = 0;
                    var virtualReturnTimeList = entities.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == item.Order_ID && r.sign == 0);
                    foreach (var process in virtualReturnTimeList)
                    {
                        if (process.ProcessTime != null)
                        {
                            time += Convert.ToDouble(process.ProcessTime);
                        }
                    }

                    virtualReturnTime = MinuteToHour(time);
                    //virtualReturnTime = (time *( item.Product_Output==null?1:Convert.ToInt32 (item.Product_Output ))).ToString ();
                    order.Add(new OrderRead
                    {
                        Engine_Program_Manager = item.Engine_Program_Manager,
                        Order_Number = item.Order_Number.ToString(),
                        Engine_Status = item.Engine_Status,
                        Order_Leader = item.Order_Leader,
                        Order_ID = item.Order_ID.ToString(),
                        Product_Drawing = "110",
                        Product_Material = item.Product_Material,
                        Product_Name = item.Product_Name,
                        Product_Output = item.Product_Output.ToString(),
                        Order_Plan_End_Time = item.Order_Plan_End_Time.ToString(),
                        Order_Plan_Start_Time = item.Order_Plan_Start_Time.ToString(),
                        Order_State = item.Intention.ToString(),
                        Engine_Technology_Manager = item.Engine_Technology_Manager,
                        virtualProgPers = virtualProgPers,
                        virtualReturnTime=virtualReturnTime,
                        projectName =item.ProjectName ,
                        Priority = item.Priority.ToString(),
                        clientName=item.ClientName
                    }); ;
                    
      
                }
                var key = context.Request["key"];
                if (key != null)
                {
                    var search = order.Where(r => r.Order_Leader == key | r.Order_Number == key | r.Product_Name == key | r.virtualProgPers == key | r.Engine_Program_Manager == key | r.Engine_Technology_Manager == key);

                    var orderKey = search.Skip((page - 1) * limit).Take(limit);
                    var modelKey = new { msg = "", code = 0, count = search.Count(), data = orderKey };
                    var jsonKey = serializer.Serialize(modelKey);
                    context.Response.Write(jsonKey);
                    return;
                }
                var order1 = order.Skip((page - 1) * limit).Take(limit);
                //var order2 = order1.ToList();
                //var number = limit - order1.Count();
                //if (number > 0)
                //{
                //    for (int i = 0; i < number; i++)
                //    {
                //        order2.Add(new OrderRead { Order_Number=" "});
                //    }
                //}
             

                var model = new { msg = "", code = 0, count = order.Count(), data = order1};
                var json = serializer.Serialize(model);
                context.Response.Write(json);
            }
        }



        #region 分钟转换小时 SecondToHour
        /// <summary>
        ///分钟转换小时
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string MinuteToHour(double time)
        {
            int hour = 0;
            int minute = 0;

            minute = Convert.ToInt32(time);

            
            if (minute > 60)
            {
                hour = minute / 60;
                minute = minute % 60;
            }
            return (hour + "小时" + minute + "分钟"
                );
        }
        #endregion

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
   
}