using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.市场部
{
    /// <summary>
    /// 获取产品名称材料 的摘要说明
    /// </summary>
    public class 获取产品名称材料 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
         
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            {
                string OrderNum = context.Request["order"];
                List<string> Info = new List<string>();
                if (OrderNum != null && OrderNum != "")
                {
                    using (JDJS_WMS_DB_USEREntities  wms = new JDJS_WMS_DB_USEREntities ())
                    {
                        var order = wms.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_Number == OrderNum);
                        if (order.Count() > 0)
                        {
                            string name = order.First().Product_Name;
                            string material = order.First().Product_Material;
                            Info.Add(name);
                            Info.Add(material);
                        }
                    }
                }
                var json = serializer.Serialize(Info);
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
}