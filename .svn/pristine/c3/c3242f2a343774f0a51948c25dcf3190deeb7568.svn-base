using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.市场部.关联生产订单
{
    /// <summary>
    /// 读取可关联订单 的摘要说明
    /// </summary>
    public class 读取可关联订单 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            using (JDJS_WMS_DB_USEREntities entities = new JDJS_WMS_DB_USEREntities())
            {
                
                var rows = entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Intention == 2 |  r.Intention == 4);

                List<WebApplication2.Model.生产管理.市场部.OrderRead> order = new List<WebApplication2.Model.生产管理.市场部.OrderRead>();
                foreach (var item in rows)
                {
                    
                    var Order_State = entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == item.Order_ID).FirstOrDefault().Intention;
                    string clientName = "";
                    var clientList = entities.JDJS_WMS_Order_Guide_Schedu_Table.Where(r => r.OrderID == item.Order_ID);
                    if (clientList.Count() > 0)
                    {
                        clientName = clientList.First().ClientName;
                    }
                    {
                        order.Add(new WebApplication2.Model.生产管理.市场部.OrderRead
                        {
                            audit = item.AuditResult == null ? "" : item.AuditResult,
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
                            Order_State = Order_State.ToString(),
                            Engine_Technology_Manager = item.Engine_Technology_Manager,
                            craftPerson = item.craftPerson,
                            projectName = item.ProjectName,
                            Priority = item.Priority.ToString(),
                            clientName = clientName,
                            remark = item.Remark == null ? "" : item.Remark
                        }) ;
                    }

                }
                var json = serializer.Serialize(order);
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