using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Kanban.品质ashx
{
    /// <summary>
    /// 圆饼 的摘要说明
    /// </summary>
    public class 圆饼 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //品质表当前品质状态
            int weijiancepin = 0;
            int hegepin = 0;
            int buhegepin = 0;
            using (JDJS_WMS_DB_USEREntities  wms = new JDJS_WMS_DB_USEREntities ())
            {
                var orders = wms.JDJS_WMS_Order_Entry_Table.Where(r => r.Intention == 2);
                {
                    foreach (var item in orders)
                    {
                        var Process = wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == item.Order_ID);
                        var lastProcess = Process.OrderByDescending(r => r.ProcessID).First().ProcessID;
                        var lastProcessId = wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == item.Order_ID & r.ProcessID == lastProcess).First().ID;
                        //当前成品数
                        var CurrFinishedProductNumber = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.ProcessID == lastProcessId & r.isFlag == 3).Count();
                        var confirm = wms.JDJS_WMS_Quality_Confirmation_Table.Where(r => r.OrderID == item.Order_ID);
                        if (confirm.Count() > 0)
                        {
                            int goood = Convert.ToInt32(confirm.FirstOrDefault().QualifiedProductNumber);
                            int pool = Convert.ToInt32(confirm.FirstOrDefault().PefectiveProductNumber);
                            hegepin += goood;
                            buhegepin += pool;
                            weijiancepin += (CurrFinishedProductNumber - (goood + pool));
                        }
                    }
                }

            }

            var model = new { weijiancepin = weijiancepin, hegepin = hegepin, buhegepin = buhegepin };
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var json = serializer.Serialize(model);
            context.Response.Write(json);
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