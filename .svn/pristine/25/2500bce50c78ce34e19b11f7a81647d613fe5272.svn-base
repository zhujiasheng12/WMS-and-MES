using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.刀具管理.备刀管理
{
    /// <summary>
    /// 刀具室弹窗提醒 的摘要说明
    /// </summary>
    public class 刀具室弹窗提醒 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
          
                string str = "";
            using (JDJS_WMS_DB_USEREntities wms = new JDJS_WMS_DB_USEREntities ())
            {
                var processes = wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.sign != 0 & r.toolPreparation != -1);
                foreach (var item in processes)
                {
                    int orderID = Convert.ToInt32(item.OrderID);
                    int processNum = Convert.ToInt32(item.ProcessID);

                    if (item.toolPreparation != 1)
                    {
                        var order = wms.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == orderID).FirstOrDefault();
                        if (order != null)
                        {
                            str += order.Order_Number + "号订单" + processNum.ToString() + "序" + "待备刀"+"/r/n";
                        }
                    }
                }
            }
            context.Response.Write(str);
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