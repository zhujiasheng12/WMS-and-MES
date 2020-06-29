using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.工程部
{
    /// <summary>
    /// 判断毛坯信息 的摘要说明
    /// </summary>
    public class 判断毛坯信息 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var orderId =int.Parse ( context.Request["orderId"]);
            using (JDJS_WMS_DB_USEREntities wms = new JDJS_WMS_DB_USEREntities())
            {
                var blankInfo = wms.JDJS_WMS_Order_Blank_Table.Where(r => r.OrderID == orderId).FirstOrDefault ();
                if (blankInfo != null)
                {
                    var str = blankInfo.BlankSpecification;
                    if (str.Contains("#1#"))
                    {
                        context.Response.Write("ok");
                        return;
                    }
                    else
                    {
                        context.Response.Write("no");
                        return;
                    }
                }
                else
                {
                    context.Response.Write("ok");
                    return;
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