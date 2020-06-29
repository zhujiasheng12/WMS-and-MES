using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model.生产管理.维保
{
    /// <summary>
    /// firstCnc 的摘要说明
    /// </summary>
    public class firstCnc : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
           using(JDJS_WMS_DB_USEREntities entities=new JDJS_WMS_DB_USEREntities())
            {
                var rows = entities.JDJS_WMS_Device_Info;
                if (rows.Count() > 0)
                {
                    context.Response.Write(rows.First().ID);

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