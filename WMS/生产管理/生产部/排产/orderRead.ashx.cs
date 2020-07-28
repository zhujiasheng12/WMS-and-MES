using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model.生产管理.生产部
{
    /// <summary>
    /// orderRead 的摘要说明
    /// </summary>
    public class orderRead : IHttpHandler
    {
        System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

        public void ProcessRequest(HttpContext context)
        {

          
            var limit =int.Parse( context.Request["limit"]);
            var page =int.Parse( context.Request["page"]);
            using(JDJS_WMS_DB_USEREntities entities=new JDJS_WMS_DB_USEREntities())
            {
                var rows = entities.JDJS_WMS_Order_Entry_Table.Where(r =>(r.Intention==3|r.Intention==2|r.Intention==4));
                List<Order> orders = new List<Order>();

                if (rows.Count() > 0)
                {
                    foreach (var item in rows)
                    {
                        int orderID = item.Order_ID;
                        string time = "";
                        var guide = entities.JDJS_WMS_Order_Guide_Schedu_Table.Where(r => r.OrderID == orderID).FirstOrDefault ();
                        if (guide != null)
                        {
                            if (guide.ExpectEndTime != null)
                            {
                                time = guide.ExpectEndTime.ToString();
                            }
                        }
                        string planTime = "";
                        var sche = entities.JDJS_WMS_Order_Machine_Scheduing_Time_Table.Where(r => r.OrderID == item.Order_ID).FirstOrDefault();
                        if (sche != null)
                        {
                            if (sche.PlanEndTime != null)
                            {
                                planTime = sche.PlanEndTime.ToString();
                            }
                        }
                   
                        orders.Add(new Order { orderId = item.Order_ID, orderNumber = item.Order_Number, flag =0, time = time,orderName =item.Product_Name ,projectName =item.ProjectName ==null?"":item.ProjectName, planTime=planTime });

                        

                    }
                    var sort = orders.OrderByDescending(r => r.flag);
                    var layPage = sort.Skip((page - 1) * limit).Take(limit);
                    var model = new { code = 0, msg = "", count = orders.Count(), data = layPage };
                    var json = serializer.Serialize(model);
                    context.Response.Write(json);
                }
                else
                {
                    context.Response.Write("{\"code\":0,\"msg\":\"\",\"count\":1,\"data\":[]}");
                    return;
                }


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
    class Order
    {
        public string orderNumber;
        public int orderId;
        public int flag;
        public string time;
        public string orderName;
        public string projectName;
        public string planTime;
    }
}