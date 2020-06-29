using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace WebApplication2.Model.生产管理.工程部
{
    /// <summary>
    /// edit 的摘要说明
    /// </summary>
    public class edit1 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                context.Response.ContentType = "text/plain";
                var form = context.Request.Form;
                var type=form["type"];
                var numberId = int.Parse(form["id"]);
                var progId = int.Parse(form["prog"]);
                var progIdOld = int.Parse(form["progOld"]);
                var techIdOld = int.Parse(form["techOld"]);
             
                using (JDJS_WMS_DB_USEREntities entities = new JDJS_WMS_DB_USEREntities())
                {
                    if (type == "生产订单") {
                        entities.JDJS_WMS_Order_Guide_Schedu_Table.Where(r => r.OrderID == numberId).FirstOrDefault().FileDownTime = DateTime.Now;
                        entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == numberId).First().Engine_Program_Manager =
                            entities.JDJS_WMS_Staff_Info.Where(r => r.id == progId).First().staff;

                        entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == numberId).First().craftPerson =
                            entities.JDJS_WMS_Staff_Info.Where(r => r.id == progId).First().staff;

                        entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == numberId).First().Engine_Program_ManagerId = progId;

                        entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == numberId).First().virtualProgPersId = progId;
                        entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == numberId).First().craftPersonId = progId;

                        entities.SaveChanges();
                        context.Response.Write("ok");
                    
                    } else {


                        entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == numberId).First().virtualProgPersId = progId;
                        entities.SaveChanges();
                        context.Response.Write("ok");
                    }
                   
                }




            }
            catch(Exception ex)
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