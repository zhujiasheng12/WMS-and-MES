using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.工程部
{
    /// <summary>
    /// 毛坯信息读数据 的摘要说明
    /// </summary>
    public class 毛坯信息读数据 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var orderNumberId = int.Parse(context.Request["orderNumberId"]);
            var id = -1;
            using (JDJS_WMS_DB_USEREntities entities = new JDJS_WMS_DB_USEREntities())
            {
                //var processInfo = entities.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == orderNumberId && r.ProcessID == 1 && r.sign == 1);
                //if (processInfo.Count() < 1)
                //{
                //    context.Response.Write("");
                //    return;
                //}
                //id = Convert.ToInt32(processInfo.FirstOrDefault().ID);
                var row = from process in entities.JDJS_WMS_Order_Blank_Table
                          where process.OrderID  == orderNumberId 
                          select new
                          {
                              
                              
                              process.OrderID,
                              
                              process.BlackNumber,
                              process.BlankSpecification,
                              process.BlankType,
                              
                          };
                if (row.Count() < 1)
                {
                    context.Response.Write("");
                    return;
                }
                string blankS = row.First().BlankSpecification;
                if (blankS.Contains("#1#"))
                {
                   blankS = blankS.Replace("#1#", "");
                }
                var model = new
                {
               
                    BlankSpecification = blankS,
                   
                    BlankType = row.First().BlankType,

                };

                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                var json = serializer.Serialize(model);
                context.Response.Write(json);
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