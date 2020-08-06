using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.生产管理.品质.品质检测导表;

namespace WebApplication2.生产管理.品质.品质检测手动处理
{
    /// <summary>
    /// 添加尺寸 的摘要说明
    /// </summary>
    public class 添加尺寸 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                var orderId = int.Parse(context.Request["orderId"]);//订单主键ID
                string num = context.Request["num"];//工件编号
                string SizeName = context.Request["sizeName"];//尺寸名称
                float StandardValue = float.Parse(context.Request["standardValue"]);//理论值
                float Measurements = float.Parse(context.Request["measurements"]);//实测值
                float ToleranceRangeMin = float.Parse(context.Request["toleranceRangeMin"]);//下偏差
                float ToleranceRangeMax = float.Parse(context.Request["toleranceRangeMax"]);//上偏差


                float max = StandardValue + ToleranceRangeMax;
                float min = StandardValue + ToleranceRangeMin;
                float OutOfTolerance = 0;
                if ((max > Measurements) && (min < Measurements))
                {
                    
                }
                else
                {
                    if (max < Measurements)
                    {
                        OutOfTolerance = Measurements - max;
                    }
                    else if (min > Measurements)
                    {
                        OutOfTolerance = Measurements - min;
                    }
                }

                int processNum = int.Parse((context.Request["processNum"] == null ? "0" : context.Request["processNum"]));
                string errMsg = "";
                List<QualityDataInfo> infos = new List<QualityDataInfo>();
                infos.Add(new QualityDataInfo()
                {
                    ToleranceRangeMax = ToleranceRangeMax,
                    Measurements = Measurements,
                    ToleranceRangeMin = ToleranceRangeMin,
                    SizeName = SizeName,
                    StandardValue = StandardValue,
                    OutOfTolerance = OutOfTolerance
                });

                if (DataInPutSql.InSql(orderId, processNum, num, ExcelType.手动录入, infos, ref errMsg))
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