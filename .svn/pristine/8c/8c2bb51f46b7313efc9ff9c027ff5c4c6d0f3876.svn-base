using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.品质.品质检测导表
{
    public class ImportSanZuoBiao : IDetectionResult
    {
        public List<QualityDataInfo> ImportExcel(string path,  ref string errMsg)
        {
            List<QualityDataInfo> infos = new List<QualityDataInfo>();
            try
            {
                string err = "";
                string[,] data = Function.GetExcelData(path, ref err);
                #region 解析数组
                bool isOk = false;
                for (int i = 0; i < data.GetLength(0); i++)
                {
                    if (isOk)
                    {
                        if (data[i, 0] == "")
                        {
                            continue;
                        }
                        QualityDataInfo info = new QualityDataInfo();
                        float value = 0;
                        info.Measurements = float.TryParse(data[i, 1], out value) ? value : 0;
                        info.SizeName = data[i, 0];
                        info.StandardValue = float.TryParse(data[i, 2], out value) ? value : 0;
                        info.ToleranceRangeMax = float.TryParse(data[i, 3], out value) ? value : 0;
                        info.ToleranceRangeMin = float.TryParse(data[i, 4], out value) ? value : 0;
                        float max = info.StandardValue + info.ToleranceRangeMax;
                        float min = info.StandardValue + info.ToleranceRangeMin;
                        if ((max > info.Measurements) && (min < info.Measurements))
                        {
                            info.OutOfTolerance = 0;
                        }
                        else
                        {
                            if (max < info.Measurements)
                            {
                                info.OutOfTolerance = info.Measurements - max;
                            }
                            else if (min > info.Measurements)
                            {
                                info.OutOfTolerance = info.Measurements - min;
                            }
                        }
                        infos.Add(info);
                    }
                    else
                    {
                        if (data[i, 0] == "Characteristic")
                        {
                            isOk = true;
                        }
                    }
                }
                #endregion
                errMsg = "ok";
                return infos;
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return null;
            }
        }
    }
}