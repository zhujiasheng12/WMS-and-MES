using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Kanban.品质ashx
{
    /// <summary>
    /// 在机测量 的摘要说明
    /// </summary>
    public class 在机测量 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //在机测量异常
            List<string> infos = new List<string>();
            using (JDJS_WMS_DB_USEREntities wms = new  JDJS_WMS_DB_USEREntities ())
            {
                var measures = wms.JDJS_WMS_Quality_Onmachine_Measurement_Data;
                foreach (var item in measures)
                {
                    double min = Convert.ToDouble(item.StandardValue + item.ToleranceRangeMin);
                    double max = Convert.ToDouble(item.StandardValue + item.ToleranceRangeMax);
                    double mea = Convert.ToDouble(item.Measurements);
                    if (mea > max || mea < min)
                    {
                        string processNum = item.SerialNumber.ToString(); ;
                        int orderID = Convert.ToInt32(item.OrderID);
                        var orderInfo = wms.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == orderID);
                        string orderNum = orderInfo.FirstOrDefault().Order_Number;
                        int cncID = Convert.ToInt32(item.CncID);
                        var cncInfo = wms.JDJS_WMS_Device_Info.Where(r => r.ID == cncID);
                        string cncNum = cncInfo.FirstOrDefault().MachNum;
                        string sizeName = item.SizeName;



                        string str = cncNum + "-" + orderNum + "-P" + processNum + " " + sizeName + "超差，公差（" + item.ToleranceRangeMin.ToString() + "," + item.ToleranceRangeMax.ToString() + "），实际" + (item.Measurements - item.StandardValue).ToString();
                        infos.Add(str);
                    }
                }
            }
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var json = serializer.Serialize(infos);
            context.Response.Write(json);

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