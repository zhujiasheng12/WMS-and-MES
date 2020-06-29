using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.工程部.Controller
{
    /// <summary>
    /// 下发备刀请求 的摘要说明
    /// </summary>
    public class 下发备刀请求 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var Id =int.Parse(context.Request["id"]);
            JDJS_WMS_DB_USEREntities entities = new JDJS_WMS_DB_USEREntities();
            var processRow = entities.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == Id);
            if (processRow.Count() > 0)
            {
                if (processRow.First().toolPreparation == -1)
                {
                    processRow.First().toolPreparation = 0;
                    entities.SaveChanges();
                    context.Response.Write("ok");
                }
                else
                {
                    context.Response.Write("无需下发备刀请求");
                }
               
            }
            else
            {
                context.Response.Write("无此工序");
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