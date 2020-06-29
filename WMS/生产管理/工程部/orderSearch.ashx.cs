using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model.生产管理.工程部
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
               var type=context.Request["type"];
               var rows = entities.JDJS_WMS_Order_Entry_Table.Where(r=>r.Intention==1000);
               if (type == "生产订单") {
                    rows = entities.JDJS_WMS_Order_Entry_Table.Where(r=>(r.Intention==2|r.Intention==3)&&r.AuditResult =="审核通过");
               } else if(type == "意向订单"){
                   rows = entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Intention == -1 | r.Intention == 0 | r.Intention == 1);
               }
              
                List<OrderSearch> searches = new List<OrderSearch>();
                foreach (var item in rows)
                {
                    searches.Add(new OrderSearch { key = item.Order_Number });
                    searches.Add(new OrderSearch { key = item.Order_Leader });
                    searches.Add(new OrderSearch { key = item.Product_Name });
                    searches.Add(new OrderSearch { key = item.Product_Material });
                    //searches.Add(new OrderSearch { key = entities.JDJS_WMS_Staff_Info.Where(r => r.id == item.Engine_Program_Manager).First().staff });
                    //searches.Add(new OrderSearch { key = entities.JDJS_WMS_Staff_Info.Where(r => r.id == item.Engine_Technology_Manager).First().staff });
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