using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.生产管理.生产部.异常报备Method.读取订单工序.model;

namespace WebApplication2.生产管理.生产部.异常报备Method.读取订单工序
{
    /// <summary>
    /// 读取订单 的摘要说明
    /// </summary>
    public class 读取订单 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var info = OrderInfoManage.GetOrderInfo();
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var json = serializer.Serialize(info);
            context.Response.Write(json);
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