using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.刀具管理.备刀管理
{
    /// <summary>
    /// orderRead 的摘要说明
    /// </summary>
    public class orderRead : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

           using(JDJS_WMS_DB_USEREntities entities=new JDJS_WMS_DB_USEREntities())
            {
                //var rows = from orderQueue in entities.JDJS_WMS_Order_Queue_Table
                //           from orderNumber in entities.JDJS_WMS_Order_Entry_Table
                //           from process in entities.JDJS_WMS_Order_Process_Info_Table
                //           where orderQueue.OrderID == orderNumber.Order_ID & process.toolPreparation != -1 & orderNumber.Order_ID == process.OrderID
                //           select new
                //           {
                //               orderNumber.Order_Number,
                //               orderNumber.Order_ID
                //           };

                var rows = 
                           from orderNumber in entities.JDJS_WMS_Order_Entry_Table
                           from process in entities.JDJS_WMS_Order_Process_Info_Table
                           where  orderNumber.Order_ID == process.OrderID
                           select new
                           {
                               orderNumber.Order_Number,
                               orderNumber.Order_ID
                           };

                if (rows.Count() > 0)
                {
                    var json = serializer.Serialize(rows);
                    context.Response.Write(json);
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
    class OrderRead
    {

    }
}