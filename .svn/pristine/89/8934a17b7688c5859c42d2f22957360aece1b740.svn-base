using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Kanban.品质ashx
{
    /// <summary>
    /// 机外测量 的摘要说明
    /// </summary>
    public class 机外测量 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //机外测量异常
            List<string> infos = new List<string>();
            using (JDJS_WMS_DB_USEREntities  wms = new  JDJS_WMS_DB_USEREntities ())
            {
                var measures = wms.JDJS_WMS_Quality_ManualInput_Measurement_Table;
                foreach (var item in measures)
                {
                    double min = Convert.ToDouble(item.StandardValue + item.ToleranceRangeMin);
                    double max = Convert.ToDouble(item.StandardValue + item.ToleranceRangeMax);
                    double mea = Convert.ToDouble(item.Measurements);
                    if (mea > max || mea < min)
                    {
                        int orderID = Convert.ToInt32(item.OrderID);
                        var orderInfo = wms.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == orderID);
                        string orderNum = orderInfo.FirstOrDefault().Order_Number;
                        string sizeName = item.SizeName.ToString();
                        string sizeNum = item.SizeNumber.ToString();
                        string str = orderNum + " " + "尺寸" + sizeNum + "-" + sizeName + "超差，公差（" + item.ToleranceRangeMin.ToString() + "," + item.ToleranceRangeMax.ToString() + "），实际" + (item.Measurements - item.StandardValue).ToString();
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