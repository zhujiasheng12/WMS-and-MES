using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.工程部
{
    /// <summary>
    /// 新建工序读数据 的摘要说明
    /// </summary>
    public class 新建工序读数据 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            using (JDJS_WMS_DB_USEREntities entities=new JDJS_WMS_DB_USEREntities())
            {
                var orderId = int.Parse(context.Request["orderNumberId"]);
                List<string> vs = new List<string>();
                var rows = entities.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == orderId&r.sign!=0);
                vs.Add(rows.Count().ToString());
                if (rows.Count() > 0)
                {
                    var BlankType = rows.Where(r => r.ProcessID == 1).FirstOrDefault().BlankType;
                    var BlankSpecification = rows.Where(r => r.ProcessID == 1).FirstOrDefault().BlankSpecification;
                    vs.Add(BlankType.ToString());
                    vs.Add(BlankSpecification);
                }
                
                var json = serializer.Serialize(vs);
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