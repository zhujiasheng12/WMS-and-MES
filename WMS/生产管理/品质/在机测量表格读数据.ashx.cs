using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.品质
{
    /// <summary>
    /// 在机测量表格读数据 的摘要说明
    /// </summary>
    public class 在机测量表格读数据 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            {
                
                if (!isNum( context.Request["id"]))
                {
                    var json1 = "{\"code\":0,\"data\":[]}";
                    context.Response.Write(json1);
                    return;
                }
                int BlankID = int.Parse(context.Request["id"]);
                List<MeasurementInfo> measurementInfos = new List<MeasurementInfo>();
                using (JDJS_WMS_DB_USEREntities wms = new JDJS_WMS_DB_USEREntities())
                {
                    var info = wms.JDJS_WMS_Quality_Onmachine_Measurement_Data.Where(r => r.blankID == BlankID);
                    foreach (var item in info)
                    {
                        MeasurementInfo measurementInfo = new MeasurementInfo();
                        measurementInfo.Max = (item.StandardValue + item.ToleranceRangeMax).ToString();
                        measurementInfo.Measurements = item.Measurements.ToString();
                        measurementInfo.Min = (item.StandardValue + item.ToleranceRangeMin).ToString();
                        measurementInfo.ProcessNum = "工序" + item.SerialNumber.ToString();
                        measurementInfo.StandardValue = item.StandardValue.ToString();
                        measurementInfo.SizeName = item.SizeName;
                        if (item.Measurements >= (item.StandardValue + item.ToleranceRangeMin) && item.Measurements <= (item.StandardValue + item.ToleranceRangeMax))
                        {
                            measurementInfo.Result = "合格";
                        }
                        else
                        {
                            measurementInfo.Result = "不合格";
                        }
                        measurementInfos.Add(measurementInfo);
                    }
                }
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                var model = new { code = 0, data = measurementInfos };
                var json = serializer.Serialize(model);
                context.Response.Write(json);
            }
        }

        private bool isNum(string str)
        {
            try
            {
                int a = Convert.ToInt32(str);
                return true;
            }
            catch
            {
                return false;
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
    class MeasurementInfo
    {
        public string ProcessNum;
        public string SizeName;
        public string StandardValue;
        public string Measurements;
        public string Max;
        public string Min;
        public string Result;
    }
}