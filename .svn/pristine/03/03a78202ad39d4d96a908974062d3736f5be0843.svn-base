using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.工程部
{
    /// <summary>
    /// 读取治具信息 的摘要说明
    /// </summary>
    public class 读取治具信息 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                List<FixInfo> fixInfos = new List<FixInfo>();
                using (JDJS_WMS_DB_USEREntities wms = new JDJS_WMS_DB_USEREntities())
                {
                    var fixs = wms.JDJS_WMS_Device_Status_Table.ToList();
                    foreach (var item in fixs)
                    {
                        FixInfo fix = new FixInfo();
                        fix.id = item.ID;
                        fix.fixName = item.Status + "(" + item.explain + ")";
                        fixInfos.Add(fix);
                    }
                }
                var json = serializer.Serialize(fixInfos);
                context.Response.Write(json);
            }
            catch (Exception ex)
            {
                context.Response.Write(ex.Message );
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

    public struct FixInfo
    {
        public int id;
        public string fixName;
    }
}