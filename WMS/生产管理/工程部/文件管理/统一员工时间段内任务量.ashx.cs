using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.工程部.文件管理
{
    /// <summary>
    /// 统一员工时间段内任务量 的摘要说明
    /// </summary>
    public class 统一员工时间段内任务量 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                DateTime startTime = DateTime.Parse(context.Request["startTime"]);
                DateTime endTime = DateTime.Parse(context.Request["endTime"]);
                var infos = FileManage.GetStaffFileInfo(startTime, endTime);
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                string json = serializer.Serialize(infos);
                context.Response.Write(json);
            }
            catch (Exception ex)
            {
                context.Response.Write(ex.Message);
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