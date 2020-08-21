using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model.生产管理.工程部
{
    /// <summary>
    /// orderRead 的摘要说明
    /// </summary>
    public class orderRead : IHttpHandler
    {
        System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                var id = int.Parse(context.Request["id"]);
                var limit = int.Parse(context.Request["limit"]);
                var page = int.Parse(context.Request["page"]);
                using (JDJS_WMS_DB_USEREntities entities = new JDJS_WMS_DB_USEREntities())
                {
                    var rows = entities.JDJS_WMS_Order_Entry_Table.Where(r => (r.Intention == 3 | r.Intention == 2 | r.Intention == 4));
                    List<Order> orders = new List<Order>();

                    if (rows.Count() > 0)
                    {
                        foreach (var item in rows)
                        {
                            int orderID = item.Order_ID;
                            string time = "";
                            var guide = entities.JDJS_WMS_Order_Guide_Schedu_Table.Where(r => r.OrderID == orderID).FirstOrDefault();
                            if (guide != null)
                            {
                                if (guide.ExpectEndTime != null)
                                {
                                    time = guide.ExpectEndTime.ToString();
                                }
                            }

                            bool isOut = true;
                            var processes = entities.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == item.Order_ID && r.sign != 0);
                            if (processes.Count() <= 0)
                            {
                                isOut = false;
                            }
                            foreach (var process in processes)
                            {
                                if (process.program_audit_sign != 1)
                                {
                                    isOut = false;
                                    break;
                                }
                            }
                            if (isOut)
                            {
                                continue;
                            }
                            if (item.Engine_Program_ManagerId == id | item.craftPersonId == id)
                            {
                                orders.Add(new Order
                                {
                                    orderId = item.Order_ID,
                                    orderNumber = item.Order_Number,
                                    flag = 1,
                                    time = time,
                                    creatTime = Convert.ToDateTime(item.CreateTime),
                                    orderName = item.Product_Name,
                                    projectName = item.ProjectName == null ? "" : item.ProjectName,
                                    Remark = item.Remark == null ? "" : item.Remark,
                                    IntentionPlanEndTime = item.IntentionPlanEndTime == null ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") : Convert.ToDateTime(item.IntentionPlanEndTime).ToString("yyyy-MM-dd HH:mm:ss:fff")
                                });
                            }
                            else
                            {
                                orders.Add(new Order
                                {
                                    orderId = item.Order_ID,
                                    orderNumber = item.Order_Number,
                                    flag = 0,
                                    time = time,
                                    creatTime = Convert.ToDateTime(item.CreateTime),
                                    orderName = item.Product_Name,
                                    projectName = item.ProjectName == null ? "" : item.ProjectName,
                                    Remark = item.Remark == null ? "" : item.Remark,
                                    IntentionPlanEndTime = item.IntentionPlanEndTime == null ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") : Convert.ToDateTime(item.IntentionPlanEndTime).ToString("yyyy-MM-dd HH:mm:ss:fff"),
                                    processPerson = item.Engine_Program_Manager
                                });

                            }

                        }
                        var sort = orders.OrderByDescending(r => r.flag).ThenByDescending(r => r.creatTime);
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
            catch (Exception ex)
            {
                context.Response.Write(ex.Message);
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
        public string orderName;
        public string projectName;
        public int orderId;
        public int flag;
        public string time;
        public DateTime creatTime;
        public string processPerson;
        public string Remark;
        public string IntentionPlanEndTime;
    }
}