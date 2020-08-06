using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.品质.品质检测导表
{
    public interface IDetectionResult
    {
        /// <summary>
        /// 品质检测结果录入
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        List<QualityDataInfo> ImportExcel(string path,ref string errMsg);
    }
}