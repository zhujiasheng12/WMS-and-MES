using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.工程部
{
    /// <summary>
    /// uploadPPT 的摘要说明
    /// </summary>
    public class uploadPPT : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try {
            
           
            var form = context.Request.Form;
            var orderId = int.Parse(form["orderId"]);
          
    
            using (JDJS_WMS_DB_USEREntities entities=new JDJS_WMS_DB_USEREntities())
            {


                var process =int.Parse( form["process"]);
                var  allTime = form["allTime"];
                //var cncType = form["cncType"];
                //var cncNumber = form["cncNumber"];

                var cncType = "1";
                var cncNumber = "1" ;
                    if (entities.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == orderId & r.sign == 0 & r.ProcessID== process).Count() > 0)
                    {
                        context.Response.Write("该工序已存在");
                        return;
                    }
                    var row = new JDJS_WMS_Order_Process_Info_Table
                    {
                        OrderID = orderId,
                        ProcessID = process,
                        ProcessTime = int.Parse(allTime),
                        DeviceType = int.Parse(cncType),
                        MachNumber=int.Parse(cncNumber),
                        sign=0
                    };
                    entities.JDJS_WMS_Order_Process_Info_Table.Add(row);
                

                //var orderNumberRow = entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == orderId);
                //var orderNumber = "";
                //if (orderNumberRow.Count() > 0)
                //{
                //    orderNumber = orderNumberRow.First().Order_Number;
                //}
       
                entities.SaveChanges();
                context.Response.Write("ok");
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
}