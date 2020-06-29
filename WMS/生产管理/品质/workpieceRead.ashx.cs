using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.品质
{
    /// <summary>
    /// workpieceRead 的摘要说明
    /// </summary>
    public class workpieceRead : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var orderId = int.Parse(context.Request["orderId"]);
            using (JDJS_WMS_DB_USEREntities entities=new JDJS_WMS_DB_USEREntities())
            {
                var rows = entities.JDJS_WMS_Quality_ManualInput_Measurement_Table.Where(r => r.OrderID == orderId);
                List<Workpiece> workpieces = new List<Workpiece>();
                foreach (var item in rows)
                {
                    workpieces.Add(new Workpiece { workpieceNumber = item.WorkpieceNumber.ToString() });
                }
                var single = workpieces.Where((r, i) => workpieces.FindIndex(p => p.workpieceNumber == r.workpieceNumber) == i);
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                var json = serializer.Serialize(single);
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
    class Workpiece
    {
        public string workpieceNumber;
    }
}