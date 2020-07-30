using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Kanban.订单状态
{
    /// <summary>
    /// 订单排产提醒 的摘要说明
    /// </summary>
    public class 订单排产提醒 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<CncWorkScheRemind> infos = new List<CncWorkScheRemind>();
            using (JDJS_WMS_DB_USEREntities wms = new JDJS_WMS_DB_USEREntities())
            {
                var orders = wms.JDJS_WMS_Order_Entry_Table.Where(r => r.Intention == 3);
                foreach (var order in orders)
                {
                    CncWorkScheRemind info = new CncWorkScheRemind();
                    info.OrderNum = order.Order_Number;
                    info.OutputNum = order.Product_Output.ToString();
                    info.ProductName = order.Product_Name;
                    info.ProjectName = order.ProjectName;
                    var guide = wms.JDJS_WMS_Order_Guide_Schedu_Table.Where(r => r.OrderID == order.Order_ID).FirstOrDefault();
                    if (guide != null)
                    {
                        if (guide.ExpectEndTime != null)
                        {
                            info.EndTime = "预计"+guide.ExpectEndTime.ToString();
                            info.time = Convert.ToDateTime(guide.ExpectEndTime);
                            info.IsSche = false;
                        }
                    }
                    var process = wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == order.Order_ID&&r.sign ==1).FirstOrDefault ();
                    if (process != null)
                    {
                        if (process.program_audit_sign == 1&&process.ProgramePassTime !=null)
                        {
                            info.EndTime = "实际" + process.ProgramePassTime.ToString();
                            info.time = Convert.ToDateTime(process.ProgramePassTime);
                            info.IsSche = true;
                        }
                    }
                    infos.Add(info);
                }
            }
            infos = infos.OrderBy(r => r.time).ToList();
            var json = serializer.Serialize(infos);
            context.Response.ContentType = "text/event-stream";
            context.Response.Write("data:" + json + "\n\n");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }

    public struct CncWorkScheRemind
    { 
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNum { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 订单数量
        /// </summary>
        public string OutputNum { get; set; }
        /// <summary>
        /// 编程完成时间
        /// </summary>
        public string EndTime { get; set; }
        /// <summary>
        /// 编程完成时间
        /// </summary>
        public DateTime time { get; set; }
        /// <summary>
        /// 可否排产
        /// </summary>
        public bool IsSche { get; set; }
    }
}