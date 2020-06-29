using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.资材部
{
    /// <summary>
    /// 治具追加提交 的摘要说明
    /// </summary>
    public class 治具追加提交 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var processId = int.Parse(context.Request["processId"]);
            var number = int.Parse(context.Request["number"]);
            using (JDJS_WMS_DB_USEREntities entities=new JDJS_WMS_DB_USEREntities())
            {
                var row = entities.JDJS_WMS_Order_Fixture_Manager_Table.Where(r => r.ProcessID == processId);
                if (row.Count() > 0)
                {
                    row.FirstOrDefault().FixtureAdditionNumber = number;
                }
                entities.SaveChanges();
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