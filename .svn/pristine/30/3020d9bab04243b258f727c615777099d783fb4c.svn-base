using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.品质
{
    /// <summary>
    /// 读订单编号 的摘要说明
    /// </summary>
    public class 读订单编号 : IHttpHandler
    {
        System.Web.Script.Serialization.JavaScriptSerializer Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        public void ProcessRequest(HttpContext context)
        {
            using(JDJS_WMS_DB_USEREntities entities=new JDJS_WMS_DB_USEREntities())
            {
                List<string> list = new List<string>();
                var rows = entities.JDJS_WMS_Order_Entry_Table;
                if (rows.Count() > 0)
                {
                    foreach (var item in rows)
                    {
                        list.Add(item.Order_Number);

                    }
                    var json = Serializer.Serialize(list);
                    context.Response.Write(json);
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