using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.生产部.订单跟踪
{
    /// <summary>
    /// 模糊搜索 的摘要说明
    /// </summary>
    public class 模糊搜索 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<StrList> infos = new List<StrList>();
            using (JDJS_WMS_DB_USEREntities wms = new JDJS_WMS_DB_USEREntities())
            {
                var orders = wms.JDJS_WMS_Order_Entry_Table.Where(r => r.Intention != -1 && r.Intention != 1 && r.Intention != 5 && r.Intention != -2);
                foreach (var item in orders)
                {
                    StrList info = new StrList();
                    info.id = item.Order_ID;
                    info.str = item.Order_Number;
                    infos.Add(info);
                    StrList info1 = new StrList();
                    info1.id = item.Order_ID;
                    info1.str = item.Product_Name;
                    infos.Add(info1);
                }
            }
            var json = serializer.Serialize(infos);
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
    public struct StrList
    {
        public int id { get; set; }
        public string str { get; set; }
    }
}