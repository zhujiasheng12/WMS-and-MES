using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.工程部
{
    /// <summary>
    /// 手动排产读工序 的摘要说明
    /// </summary>
    public class 手动排产读工序 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            using(JDJS_WMS_DB_USEREntities entities=new JDJS_WMS_DB_USEREntities())
            {
                int orderId = int.Parse(context.Request["orderId"]);
                var process = entities.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == orderId&&r.sign ==1);
                List<CncType> types = new List<CncType>();
                foreach (var item in process)
                {
                    var cncTypeId =Convert.ToInt32( item.DeviceType);
                    var cncType = entities.JDJS_WMS_Device_Type_Info .Where(r => r.ID == cncTypeId).FirstOrDefault().Type;
                    types.Add(new CncType { process = item.ProcessID.ToString(), cncType = cncType });

                }
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                var json = serializer.Serialize(types);
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
    class CncType
    {
        public string process;
        public string cncType;
    }
}