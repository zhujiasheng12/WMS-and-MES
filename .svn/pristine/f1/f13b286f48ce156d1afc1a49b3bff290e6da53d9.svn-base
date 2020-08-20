using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.市场部.意向订单已评估确认
{
    /// <summary>
    /// 意向订单以评估确认 的摘要说明
    /// </summary>
    public class 意向订单已评估确认 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                var idListStr = context.Request["idList"];
                var idListArray = idListStr.Split(',');
                List<int> idlist = new List<int>();
                int val = 0;
                string result = "";
                foreach (var item in idListArray)
                {
                    if (int.TryParse(item, out val))
                    {
                        if (val > 0)
                        {
                            idlist.Add(val);
                        }
                    }
                }
                foreach (var item in idlist)
                {
                    using (JDJS_WMS_DB_USEREntities model = new JDJS_WMS_DB_USEREntities())
                    {
                        var order = model.JDJS_WMS_Order_Entry_Table.Find(item);
                        if (order.Intention != 1)
                        {
                            result += ("订单："+order.Order_Number+"未评估完成！</br>");
                            continue;
                        }
                        order.IntentionOverConfirm = "已确认";
                        model.SaveChanges();
                    }
                }
                if (result == "")
                {
                    result = "确认完成!";
                }
                context.Response.Write(result);
                return;
            }
            catch (Exception ex)
            {
                context.Response.Write(ex.Message);
                return;
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