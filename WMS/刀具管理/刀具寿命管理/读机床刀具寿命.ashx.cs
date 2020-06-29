using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.刀具管理.刀具寿命管理
{
    /// <summary>
    /// 读机床刀具寿命 的摘要说明
    /// </summary>
    public class 读机床刀具寿命 : IHttpHandler
    {
        System.Web.Script.Serialization.JavaScriptSerializer Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

        public void ProcessRequest(HttpContext context)
        {
            {
                List<deviceToolInfo> deviceToolInfos = new List<deviceToolInfo>();
                using (JDJS_WMS_DB_USEREntities  wms = new JDJS_WMS_DB_USEREntities ())
                {
                    var devices = wms.JDJS_WMS_Device_Info;
                    foreach (var item in devices)
                    {
                        deviceToolInfo deviceTool = new deviceToolInfo();
                        deviceTool.CncID = item.ID.ToString();
                        deviceTool.flag = "0";
                        var toollist = wms.JDJS_WMS_Tool_LifeTime_Management.Where(r => r.CncID == item.ID);
                        int error = 0;
                        foreach (var real in toollist)
                        {

                            if (real.ToolCurrTime > real.ToolMaxTime * 0.8 && real.ToolCurrTime < real.ToolMaxTime && real.ToolMaxTime > 0)
                            {
                                error = 1;
                                deviceTool.flag = "1";
                            }
                            if (real.ToolCurrTime >= real.ToolMaxTime && real.ToolMaxTime > 0)
                            {
                                error = 2;
                                deviceTool.flag = "2";
                                break;
                            }
                        }
                        deviceToolInfos.Add(deviceTool);

                    }
                }
                var json = Serializer.Serialize(deviceToolInfos);
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
    public class deviceToolInfo
    {
        public string CncID;
        public string flag;
    }
}