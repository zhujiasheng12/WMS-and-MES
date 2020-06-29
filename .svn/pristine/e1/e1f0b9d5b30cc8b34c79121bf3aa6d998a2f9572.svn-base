using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model.设备管理
{
    /// <summary>
    /// machOption 的摘要说明
    /// </summary>
    public class machOption : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
           using (JDJS_WMS_DB_USEREntities entities=new JDJS_WMS_DB_USEREntities())
            {
                var rows = entities.JDJS_WMS_Device_Info;
                List<Option> options = new List<Option>();
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                foreach (var item in rows)
                {
                    Option option = new Option();
                    option.number = item.MachNum;
                    options.Add(option);
                };
                var rowsWork = entities.JDJS_WMS_Location_Info;
                foreach (var item in rowsWork)
                {
                    Option option = new Option();
                    option.number = item.Name;
                    options.Add(option);
                }
                var data = options.Where((r, i) => options.FindIndex(p=>p.number==r.number) == i);
                var sort = data.OrderBy(r => r.number);
                var json = serializer.Serialize(sort);
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
    class Option
    {
        public string number;
    }
}