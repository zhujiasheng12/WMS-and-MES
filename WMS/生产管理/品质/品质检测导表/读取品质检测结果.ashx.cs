using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.品质.品质检测导表
{
    /// <summary>
    /// 读取品质检测结果 的摘要说明
    /// </summary>
    public class 读取品质检测结果 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                var orderId = int.Parse(context.Request["orderId"]);
                OrderQualityInfo infos = new OrderQualityInfo();
                infos.WorkPieceQualityInfoList = new List<WorkPieceQualityInfo>();
                using (JDJS_WMS_DB_USEREntities model = new JDJS_WMS_DB_USEREntities())
                {
                    var qualitys = model.JDJS_WMS_Quality_Detection_Measurement_Table.Where(r => r.OrderID == orderId);
                    foreach (var item in qualitys)
                    {
                        string workPieceNum = item.WorkpieceNumber;
                        var oldWork = infos.WorkPieceQualityInfoList.Where(r => r.WorkPieceNum == "工件：" + workPieceNum).FirstOrDefault();
                        if (oldWork == null)
                        {
                            WorkPieceQualityInfo info = new WorkPieceQualityInfo();
                            info.WorkPieceNum = "工件：" + workPieceNum;
                            info.ProcessQualityInfoList = new List<ProcessQualityInfo>();
                            infos.WorkPieceQualityInfoList.Add(info);
                            oldWork = infos.WorkPieceQualityInfoList.Where(r => r.WorkPieceNum == "工件：" + workPieceNum).FirstOrDefault();
                        }
                        string processNum = item.ProcessNum.ToString ();
                        var oldProcess = oldWork.ProcessQualityInfoList.Where(r => r.ProcessNum == processNum).FirstOrDefault();
                        if (oldProcess == null)
                        {
                            ProcessQualityInfo info = new ProcessQualityInfo();
                            info.ProcessNum = processNum;
                            info.ErCiYuanInfoList = new ErCiYuanInfo();
                            info.ErCiYuanInfoList.QualityDataInfoList = new List<QualityDataInfo>();
                            info.SanZuoBiaoList = new SanZuoBiao();
                            info.SanZuoBiaoList.QualityDataInfoList = new List<QualityDataInfo>();
                            info.DefaultList = new Default();
                            info.DefaultList.QualityDataInfoList = new List<QualityDataInfo>();
                            oldWork.ProcessQualityInfoList.Add(info);
                            oldProcess = oldWork.ProcessQualityInfoList.Where(r => r.ProcessNum == processNum).FirstOrDefault();
                        }
                        QualityDataInfo dataInfo = new QualityDataInfo();
                        dataInfo.Id = item.ID;
                        float value = 0;
                        dataInfo.Measurements = float.TryParse(item.Measurements.ToString(), out value) ? value : 0 ;
                        dataInfo.OutOfTolerance = float.TryParse(item.OutOfTolerance.ToString(), out value) ? value : 0;
                        dataInfo.SizeName = item.SizeName;
                        dataInfo.StandardValue = float.TryParse(item.StandardValue.ToString(), out value) ? value : 0;
                        dataInfo.ToleranceRangeMax = float.TryParse(item.ToleranceRangeMax.ToString(), out value) ? value : 0;
                        dataInfo.ToleranceRangeMin = float.TryParse(item.ToleranceRangeMin.ToString(), out value) ? value : 0;
                        switch (item.Type)
                        {
                            case "二次元":
                                oldProcess.ErCiYuanInfoList.QualityDataInfoList.Add(dataInfo);
                                break;
                            case "三坐标":
                                oldProcess.SanZuoBiaoList.QualityDataInfoList.Add(dataInfo);
                                break;
                            default:
                                oldProcess.DefaultList.QualityDataInfoList.Add(dataInfo);
                                break;
                        }
                    }
                }
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                var json = serializer.Serialize(infos);
                context.Response.Write( json);

            }
            catch (Exception ex)
            {
                context.Response.Write(ex.Message);
                return;
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

    public class OrderQualityInfo
    {
        public int OrderId { get; set; }
        public List<WorkPieceQualityInfo> WorkPieceQualityInfoList { get; set; }
    }
    public class WorkPieceQualityInfo
    { 
        public string WorkPieceNum { get; set; }
        public List<ProcessQualityInfo> ProcessQualityInfoList { get; set; }
    }

    public class ProcessQualityInfo
    {
        public string ProcessNum { get; set; }
        public ErCiYuanInfo ErCiYuanInfoList { get; set; }
        public SanZuoBiao SanZuoBiaoList { get; set; }
        public Default DefaultList { get; set; }
    }
    public class ErCiYuanInfo
    { 
        public string Name { get { return "二次元"; } }
        public List<QualityDataInfo> QualityDataInfoList { get; set; }
    }
    public class SanZuoBiao
    {
        public string Name { get { return "三坐标"; } }
        public List<QualityDataInfo> QualityDataInfoList { get; set; }
    }
    public class Default
    {
        public string Name { get { return "手动录入"; } }
        public List<QualityDataInfo> QualityDataInfoList { get; set; }
    }
}