﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model.生产管理.工程部
{
    /// <summary>
    /// orderEntry 的摘要说明
    /// </summary>
    public class orderEntry : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            using(JDJS_WMS_DB_USEREntities entities =new JDJS_WMS_DB_USEREntities())
            {
                var page = int.Parse(context.Request["page"]);
                var limit = int.Parse(context.Request["limit"]);
                var type=context.Request["type"];
              var  rows = entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Intention ==1000);
                if (type == "生产订单") {
                     rows = entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Intention == 2 | r.Intention == 3 );
                } else if(type == "意向订单"){
                     rows = entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Intention == -1 | r.Intention == 0 | r.Intention == 1);
                }
              
               List< OrderRead >order = new List<OrderRead>();
                foreach (var item in rows)
                {
                    string clientName = "";
                    var clientList = entities.JDJS_WMS_Order_Guide_Schedu_Table.Where(r => r.OrderID == item.Order_ID);
                    if (clientList.Count() > 0) {
                        clientName = clientList.First().ClientName;
                    }
                    var virtualProgPers = "";
                    var virtualProgPersRow = entities.JDJS_WMS_Staff_Info.Where(r => r.id == item.virtualProgPersId);
                    if (virtualProgPersRow.Count() > 0)
                    {
                        virtualProgPers = virtualProgPersRow.First().staff;
                    }
                    order.Add(new OrderRead
                    {

                        Engine_Program_Manager = item.Engine_Program_Manager,
                        Order_Number = item.Order_Number.ToString(),
                        Engine_Status = item.Engine_Status,
                        projectName =item.ProjectName ==null?"":item.ProjectName ,
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
                        Engine_Program_ManagerId=Convert.ToInt32(item.Engine_Program_ManagerId),
                        Engine_Technology_ManagerId=Convert.ToInt32(item.Engine_Technology_ManagerId),
                        craftPerson=item.craftPerson,
                        craftPersonId= Convert.ToInt32( item.craftPersonId),
                        clientName = clientName,
                        virtualProgPers = virtualProgPers
                    });
                }
                var key = context.Request["key"];
                if (key != null)
                {
                    var search = order.Where(r => r.Order_Leader == key | r.Order_Number == key | r.Order_State == key | r.Product_Name == key |
                  r.Product_Material == key);

                    var orderKey = search.Skip((page - 1) * limit).Take(limit);
                    var modelKey = new { msg = "", code = 0, count = search.Count(), data = orderKey };
                    var jsonKey = serializer.Serialize(modelKey);
                    context.Response.Write(jsonKey);
                    return;
                }

                var order1 = order.Skip((page - 1) * limit).Take(limit);
                var model = new { msg = "", code = 0, count = rows.Count(), data = order1};
                var json = serializer.Serialize(model);
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
    class OrderRead
    {
        public string Order_ID;
        public string Order_Number;
        public string projectName;
        public string Order_Leader;
        public string Product_Name;
        public string Product_Material;
        public string Product_Drawing;
        public string Product_Output;
        public string Order_Plan_Start_Time;
        public string Order_Plan_End_Time;
        public string Order_State;
        public string Engine_Program_Manager;
        public string Engine_Technology_Manager;
        public string Engine_Status;
        public string craftPerson;
        public int craftPersonId;
        public int Engine_Program_ManagerId;
        public int Engine_Technology_ManagerId;
        public string clientName;
        public string virtualProgPers;
    }
}