using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.EMMA;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.品质.品质检测导表
{
    /// <summary>
    /// 解析表格 的摘要说明
    /// </summary>
    public class 解析表格 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                var orderId = int.Parse(context.Request["orderId"]);
                string type = context.Request["type"];//表格类型‘二次元’|‘三坐标’
                string num =context.Request["num"];//工件编号
                string orderNum= context.Request["orderNum"];//订单编号
                var file = context.Request.Files[0];
                int processNum= int.Parse((context.Request["processNum"]==null?"0": context.Request["processNum"]));

                PathInfo pathInfo = new PathInfo();
                string formatPath = pathInfo.upLoadPath();

                string filePath = "";
                filePath= System.IO.Path.Combine(formatPath, orderNum,"品质检测-工件"+ num, type);
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                filePath = System.IO.Path.Combine(formatPath, orderNum, "品质检测-工件" + num,type,file.FileName);
                file.SaveAs(filePath);
                string errMsg = "";
                ExcelType excelType;
                switch (type)
                {
                    case "二次元":
                        excelType = ExcelType.二次元;
                        break;
                    case "三坐标":
                        excelType = ExcelType.三坐标;
                        break;
                    default:
                        context.Response.Write("请输入正确的文件模板类型！");
                        return;
                }
                IDetectionResult result = ImportExcelFactory.CreateInterence(excelType);
                List<QualityDataInfo> infos=result.ImportExcel(filePath, ref errMsg);
                if(infos!=null)
                {
                    if (DataInPutSql.InSql(orderId, processNum, num, excelType, infos, ref errMsg))
                    {
                        context.Response.Write("ok");
                        return;
                    }
                    else
                    {
                        context.Response.Write(errMsg);
                        return;
                    }
                        

                    
                }
                else
                {
                    context.Response.Write(errMsg);
                    return;
                }
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
}