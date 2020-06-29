using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model.生产管理.市场部
{
    /// <summary>
    /// orderSearch 的摘要说明
    /// </summary>
    public class orderSearch : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
           using(JDJS_WMS_DB_USEREntities entities=new JDJS_WMS_DB_USEREntities())
            {
                var rows = entities.JDJS_WMS_Order_Entry_Table;
                List<OrderSearch> searches = new List<OrderSearch>();
                foreach (var item in rows)
                {
                    searches.Add(new OrderSearch { key = item.Order_Number });
                    searches.Add(new OrderSearch { key = item.Order_Leader });
                    searches.Add(new OrderSearch { key = item.Product_Name });
                    searches.Add(new OrderSearch { key = item.Product_Material });
                    searches.Add(new OrderSearch { key = item.Engine_Program_Manager });
                    searches.Add(new OrderSearch { key = item.Engine_Technology_Manager });
                    searches.Add(new OrderSearch { key = item.Order_State });
                }
               var select= searches.Where((x, i) => searches.FindIndex(z => z.key == x.key) == i);
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                var json = serializer.Serialize(select);
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
    class OrderSearch
    {
       public string key;
    }
}