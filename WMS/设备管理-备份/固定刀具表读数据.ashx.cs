using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.设备管理
{
    /// <summary>
    /// 固定刀具表读数据 的摘要说明
    /// </summary>
    public class 固定刀具表读数据 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var id = int.Parse(context.Request["id"]);
            using(JDJS_WMS_DB_USEREntities entities=new JDJS_WMS_DB_USEREntities())
            {
                var rows = from r in entities.JDJS_WMS_Tool_Standard_Table.Where(r => r.MachTypeID == id)
                           select new
                           {
                               r.ID,
                               r.ToolID,
                               r.Sort,
                               r.Name,
                               r.Specification,
                               r.ProcessStage,
                               r.RazorDiameter,
                               r.ToolDiameter,
                               r.ToolLength,
                               r.RotatingSpeed,
                               r.Feed,
                               r.Knife,
                               r.Shank,
                               r.MachTypeID


                           };
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                var model = new { code = 0, data = rows};
                var json = serializer.Serialize(model);
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