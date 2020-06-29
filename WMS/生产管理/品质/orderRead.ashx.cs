﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model.生产管理.品质
{
    /// <summary>
    /// orderRead 的摘要说明
    /// </summary>
    public class orderRead : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            List<OrderRead> reads = new List<OrderRead>();
            using (JDJS_WMS_DB_USEREntities entities=new JDJS_WMS_DB_USEREntities())
            {
                var orderid = entities.JDJS_WMS_Order_Entry_Table.ToArray();
               
                foreach (var item in orderid)
                {
                    int a = item.Order_ID;
                    string b = item.Order_Number;
                    reads.Add(new OrderRead { id = a, orderNumber = b ,orderName =item.Product_Name ,projectName =item .ProjectName ==null?"":item.ProjectName });
                }
            }
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var json = serializer.Serialize(reads);
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
    public class OrderRead
    {
        public int id;
        public string orderNumber;
        public string orderName;
        public string projectName;
    }
}