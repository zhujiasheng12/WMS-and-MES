using NPOI.HSSF.Record;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.品质.品质检测导表
{
    public class DataInPutSql
    {
        public static bool InSql(int orderId, int processNum, string workPieceNum,ExcelType type, List<QualityDataInfo> infos,ref string errMsg)
        {
            try
            {
                using (JDJS_WMS_DB_USEREntities model = new JDJS_WMS_DB_USEREntities())
                {
                    using (System.Data.Entity.DbContextTransaction mytran = model.Database.BeginTransaction())
                    {
                        try
                        {
                            foreach (var info in infos)
                            {
                                JDJS_WMS_Quality_Detection_Measurement_Table jd = new JDJS_WMS_Quality_Detection_Measurement_Table()
                                { 
                                OrderID =orderId ,
                                Measurements =info.Measurements ,
                                ToleranceRangeMax =info.ToleranceRangeMax ,
                                ToleranceRangeMin =info.ToleranceRangeMin ,
                                Type =type .ToString(),
                                ProcessNum =processNum ,
                                SizeName =info.SizeName ,
                                StandardValue =info.StandardValue ,
                                WorkpieceNumber =workPieceNum ,
                                OutOfTolerance =info.OutOfTolerance 
                                };
                                model.JDJS_WMS_Quality_Detection_Measurement_Table.Add(jd);
                            }
                            model.SaveChanges();
                            mytran.Commit();
                            errMsg = "ok";
                            return true;
                        }
                        catch (Exception ex)
                        {
                            mytran.Rollback();
                            errMsg = ex.Message;
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return false;
            }
        }
    }

    public struct QualityDataInfo
    { 
        public string SizeName { get; set; }
        public float StandardValue { get; set; }
        public float ToleranceRangeMin { get; set; }
        public float ToleranceRangeMax { get; set; }
        public float Measurements { get; set; }
        public float OutOfTolerance { get; set; }
    }
}