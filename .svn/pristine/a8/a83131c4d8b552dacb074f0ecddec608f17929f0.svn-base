using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.品质
{
    /// <summary>
    /// 品质检测读取数据 的摘要说明
    /// </summary>
    public class 品质检测读取数据 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var data = context.Request["id"];
            List<Data> infos = new List<Data>();
            try
            {
                if (data.Contains('-'))
                {
                    int orderId = int.Parse(data.Split('-')[0]);
                    string workPieceNum = data.Split('-')[1];
                    float value = 0;
                    using (JDJS_WMS_DB_USEREntities model = new JDJS_WMS_DB_USEREntities())
                    {
                        var quality = model.JDJS_WMS_Quality_Detection_Measurement_Table.Where(r => r.OrderID == orderId && r.WorkpieceNumber == workPieceNum);
                        foreach (var item in quality)
                        {
                            Data data1 = new Data();
                            data1.Id = item.ID;
                            data1.Type = item.Type;
                            data1.Measurements = float.TryParse(item.Measurements.ToString(), out value) ? value : 0;
                            data1.OutOfTolerance = float.TryParse(item.OutOfTolerance.ToString(), out value) ? value : 0;
                            data1.SizeName = item.SizeName;
                            data1.StandardValue = float.TryParse(item.StandardValue.ToString(), out value) ? value : 0;
                            data1.ToleranceRangeMax = float.TryParse(item.ToleranceRangeMax.ToString(), out value) ? value : 0;
                            data1.ToleranceRangeMin = float.TryParse(item.ToleranceRangeMin.ToString(), out value) ? value : 0;
                            infos.Add(data1);
                        }
                    }
                }
            }
            catch (Exception ex)
            { 
            
            }
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var json = serializer.Serialize(infos);
            context.Response.Write(  json );
            return;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
    public class Data
    {
        public int Id { get; set; }
        public string SizeName { get; set; }
        public float StandardValue { get; set; }
        public float ToleranceRangeMin { get; set; }
        public float ToleranceRangeMax { get; set; }
        public float Measurements { get; set; }
        public float OutOfTolerance { get; set; }
        public string Type { get; set; }
    }
}