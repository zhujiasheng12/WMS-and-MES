using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.Model.生产管理.生产部;

namespace WebApplication2.生产管理.生产部.历史订单
{
    /// <summary>
    /// 排产历史订单 的摘要说明
    /// </summary>
    public class 排产历史订单 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var limit = int.Parse(context.Request["limit"]);
            var page = int.Parse(context.Request["page"]);
            using (JDJS_WMS_DB_USEREntities entities = new JDJS_WMS_DB_USEREntities())
            {
                var rows = entities.JDJS_WMS_Order_Entry_Table.Where(r => (r.Intention == 3 | r.Intention == 2/*|r.Intention==4*/));
                List<Order> orders = new List<Order>();

                if (rows.Count() > 0)
                {
                    foreach (var item in rows)
                    {
                        var process = entities.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == item.Order_ID && r.sign != 0).FirstOrDefault();
                        if (process == null)
                        {
                            continue;
                        }
                        if (process.sign != 1 || process.program_audit_sign != 1)
                        {
                            continue;
                        }

                        int maxProcessNum = 0;
                        int processId = 0;
                        var processes = entities.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == item.Order_ID);

                        foreach (var real in processes)
                        {
                            if (maxProcessNum < Convert.ToInt32(real.ProcessID))
                            {
                                maxProcessNum = Convert.ToInt32(real.ProcessID);
                                processId = real.ID;
                            }
                        }
                        var shchedu = entities.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.OrderID == item.Order_ID && r.isFlag != 0);
                        var work = shchedu.Where(r => r.ProcessID == processId);
                        int workCount = 0;
                        foreach (var real in work)
                        {
                            workCount += (real.WorkCount == null ? 0 : Convert.ToInt32(real.WorkCount));
                        }
                        if (workCount < item.Product_Output)
                        {
                            continue;
                        }
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
                        string planTime = "";
                        var sche = entities.JDJS_WMS_Order_Machine_Scheduing_Time_Table.Where(r => r.OrderID == item.Order_ID).FirstOrDefault();
                        if (sche != null)
                        {
                            if (sche.PlanEndTime != null)
                            {
                                planTime = sche.PlanEndTime.ToString();
                            }
                        }
                        Order info = new Order { orderId = item.Order_ID, orderNumber = item.Order_Number, flag = 0, time = time, orderName = item.Product_Name, projectName = item.ProjectName == null ? "" : item.ProjectName, planTime = planTime, orderPlanEndTime = item.Order_Plan_End_Time == null ? DateTime.Now : Convert.ToDateTime(item.Order_Plan_End_Time), orderPlanEndTimeStr = item.Order_Plan_End_Time == null ? "" : Convert.ToDateTime(item.Order_Plan_End_Time).ToString(), output = item.Product_Output };
                        if (item.Intention == 3)
                        {
                            info.flag = 1;
                        }
                        orders.Add(info);
                    }
                    var sort = orders.OrderByDescending(r => r.flag).ThenBy(r => r.orderPlanEndTime);
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
}