using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.市场部
{
    /// <summary>
    /// 读生产订单 的摘要说明
    /// </summary>
    public class 读生产订单 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
          using(JDJS_WMS_DB_USEREntities entities=new JDJS_WMS_DB_USEREntities())
            {
                using(System.Data.Entity.DbContextTransaction db = entities.Database.BeginTransaction())
                {
                    List<string> list = new List<string>();
                    var rows = entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Intention == 2 | r.Intention == 4);
                    if (rows.Count() > 0)
                    {
                        foreach (var item in rows)
                        {
                            list.Add(item.Order_Number);
                        }
                        var json = serializer.Serialize(list);
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