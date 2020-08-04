using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.工程部.处理生产关联订单
{
    /// <summary>
    /// 直接下推到排产 的摘要说明
    /// </summary>
    public class 直接下推到排产 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var orderId = int.Parse(context.Request["orderId"]);

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