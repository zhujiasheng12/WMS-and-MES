using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model.工厂管理
{
    /// <summary>
    /// PositionCreateHandler1 的摘要说明
    /// </summary>
    public class PositionCreateHandler1 : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {   
        var name = context.Request["name"];
            int  id = int.Parse(context.Request["id"]);
            var team = "北京精雕";
            context.Response.ContentType = "text/plain";
          
          using (JDJS_WMS_DB_USEREntities data=new JDJS_WMS_DB_USEREntities())
            {
              
              
               
              
                var data1 = new JDJS_WMS_Location_Info {Name=name,EndDate=team,parentId=id,size="1" };
                data.JDJS_WMS_Location_Info.Add(data1);
                data.SaveChanges();
                context.Response.Write("ok");
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