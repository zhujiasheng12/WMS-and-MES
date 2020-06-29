using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.市场部
{
    /// <summary>
    /// 生产订单关联读数据 的摘要说明
    /// </summary>
    public class 生产订单关联读数据 : IHttpHandler
    {
        System.Web.Script.Serialization.JavaScriptSerializer Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        public void ProcessRequest(HttpContext context)
        {
           using(JDJS_WMS_DB_USEREntities entities=new JDJS_WMS_DB_USEREntities())
            {
                using (System.Data.Entity.DbContextTransaction db = entities.Database.BeginTransaction())
                {
                    var orderNumber = context.Request["orderNumber"];
                    var row = entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_Number == orderNumber);
                    if (row.Count() > 0)
                    {
                        var model = new { Product_Name = row.FirstOrDefault().Product_Name, Product_Material = row.FirstOrDefault().Product_Material };
                        var json = Serializer.Serialize(model);
                        context.Response.Write(json);
                    }

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