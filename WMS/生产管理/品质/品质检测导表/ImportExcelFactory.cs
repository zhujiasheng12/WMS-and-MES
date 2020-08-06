using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.品质.品质检测导表
{
    public class ImportExcelFactory
    {
        public static IDetectionResult CreateInterence(ExcelType excelType)
        {
            IDetectionResult result = null;
            switch (excelType)
            {
                case ExcelType.二次元:
                    result = new ImportErCiYuan();
                    break;
                case ExcelType.三坐标:
                    result = new ImportSanZuoBiao();
                    break;
                default:
                    break;
            }
            return result;
        }
    }
}