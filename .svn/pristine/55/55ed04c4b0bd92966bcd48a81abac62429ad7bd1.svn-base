using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Data;
using System.Data.OleDb;

namespace WebApplication2.生产管理.品质
{
    /// <summary>
    /// i_mport 的摘要说明
    /// </summary>
    public class import : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {

            var files = context.Request.Files;
            PathInfo pathInfo = new PathInfo();
            var fileName =files[0].FileName;
           var path= Path.Combine(pathInfo.upLoadPath(), fileName);
            if (File.Exists(path))
            {
                File.Delete(path);
            }

           


            files[0].SaveAs(path);
            string FilePath = path;

            List<QualityInfo> qualities = new List<QualityInfo>();

            string fileSuffix = System.IO.Path.GetExtension(FilePath);

            if (fileSuffix == ".xls")
            {
                #region 
                {
                    DataTable dt = getData(FilePath).Tables[0];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        using (JDJS_WMS_DB_USEREntities CIE = new JDJS_WMS_DB_USEREntities())
                        {
                            string a1 = dt.Rows[i]["订单单号"].ToString();
                            var order = CIE.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_Number == a1);
                            if (order.Count() > 0)
                            {
                                QualityInfo quality = new QualityInfo();
                                quality.OrderNum = Convert.ToInt32(order.First().Order_ID);
                                quality.WorkpieceNumber = Convert.ToInt32(dt.Rows[i]["工件序号"].ToString());
                                quality.SizeNumber = Convert.ToInt32(dt.Rows[i]["尺寸序号"].ToString());
                                quality.SizeName = dt.Rows[i]["尺寸名称"].ToString(); ;
                                quality.StandardValue = Convert.ToDouble(dt.Rows[i]["尺寸理论值"].ToString());
                                quality.ToleranceRangeMin = Convert.ToDouble(dt.Rows[i]["合理误差范围下公差"].ToString());
                                quality.ToleranceRangeMax = Convert.ToDouble(dt.Rows[i]["合理误差范围上公差"].ToString());
                                quality.Measurements = Convert.ToDouble(dt.Rows[i]["尺寸实测值"].ToString());
                                if (dt.Rows[i]["是否超差"].ToString() == "是")
                                {
                                    quality.OutOfTolerance = 1;
                                }
                                else if (dt.Rows[i]["是否超差"].ToString() == "否")
                                {
                                    quality.OutOfTolerance = 0;
                                }
                                else
                                {
                                    quality.OutOfTolerance = 2;
                                }
                                qualities.Add(quality);
                            }
                        }
                    }
                }
                #endregion
            }
            else if (fileSuffix == ".csv")
            {


                //List<QualityInfo> qualities = new List<QualityInfo>();
                //using (FileStream file = new FileStream(FilePath, FileMode.Append))
                //{

                    using (StreamReader read = new StreamReader(FilePath, Encoding.Default))
                    {
                        string str = read.ReadLine();
                        str = read.ReadLine();
                        while (str != null)
                        {
                            if (str != null && str != ",,,,,,,")
                            {

                                string[] info = str.Split(',');
                                using (JDJS_WMS_DB_USEREntities CIE = new JDJS_WMS_DB_USEREntities())
                                {
                                    string ordernum = info[0];
                                    var order = CIE.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_Number == ordernum);
                                    if (order.Count() > 0)
                                    {
                                        QualityInfo quality = new QualityInfo();
                                        quality.OrderNum = Convert.ToInt32(order.First().Order_ID);
                                        quality.WorkpieceNumber = Convert.ToInt32(info[1]);
                                        quality.SizeNumber = Convert.ToInt32(info[2]);
                                        quality.SizeName = info[3];
                                        quality.StandardValue = Convert.ToDouble(info[4]);
                                        quality.ToleranceRangeMin = Convert.ToDouble(info[5]);
                                        quality.ToleranceRangeMax = Convert.ToDouble(info[6]);
                                        quality.Measurements = Convert.ToDouble(info[7]);
                                        if (info[8] == "是")
                                        {
                                            quality.OutOfTolerance = 1;
                                        }
                                        else if (info[8] == "否")
                                        {
                                            quality.OutOfTolerance = 0;
                                        }
                                        else
                                        {
                                            quality.OutOfTolerance = 2;
                                        }
                                        qualities.Add(quality);

                                    }
                                }

                            }
                            str = read.ReadLine();
                        }

                    }
                //}
            }
            else
            {
                context.Response.Write("请输入格式为csv或xls的文件");
                return;

            }



            using (JDJS_WMS_DB_USEREntities CIE = new JDJS_WMS_DB_USEREntities())
            {
                using (System.Data.Entity.DbContextTransaction mytran = CIE.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var item in qualities)
                        {


                            JDJS_WMS_Quality_ManualInput_Measurement_Table ma = new JDJS_WMS_Quality_ManualInput_Measurement_Table()
                            {
                                OrderID = item.OrderNum,
                                WorkpieceNumber = item.WorkpieceNumber,
                                SizeNumber = item.SizeNumber,
                                SizeName = item.SizeName,
                                StandardValue = item.StandardValue,
                                ToleranceRangeMin = item.ToleranceRangeMin,
                                ToleranceRangeMax = item.ToleranceRangeMax,
                                Measurements = item.Measurements,
                                OutOfTolerance = item.OutOfTolerance
                            };
                            CIE.JDJS_WMS_Quality_ManualInput_Measurement_Table.Add(ma);

                        }
                        CIE.SaveChanges();
                        mytran.Commit();

                    }
                    catch
                    {
                          mytran.Rollback();
                    }

                }
            }


        
        context.Response.Write("ok");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }


        public static DataSet getData(string path)

        {

            string fileSuffix = System.IO.Path.GetExtension(path);

            if (string.IsNullOrEmpty(fileSuffix))

                return null;

            using (DataSet ds = new DataSet())

            {

                //判断Excel文件是2003版本还是2007版本

                string connString = "";

                if (fileSuffix == ".xls")

                    connString = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + path + ";" + ";Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1\"";

                else
                    // connString = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + path + ";" + ";Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1\"";
                    connString = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + path + ";" + ";Extended Properties=\"Excel 12.0;HDR=YES;IMEX=1\"";

                //读取文件

                //string sql_select = "select * from [sheet1$]";
                string tableName;
                using (OleDbConnection conn = new OleDbConnection(connString))
                {

                    //DataTable table = conn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, null);
                    //string tableName = table.Rows[0]["Table_Name"].ToString();
                    //string sql_select = "select * from " + "[" + tableName + "]";
                    //string sql_select = "select * from [sheet1$]";



                    conn.Open();

                    DataTable table = conn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, null);
                    tableName = table.Rows[0]["Table_Name"].ToString();
                    string strExcel = "select * from " + "[" + tableName + "]";
                    OleDbDataAdapter adapter = new OleDbDataAdapter(strExcel, connString);

                    adapter.Fill(ds, tableName);
                    conn.Close();


                }

                if (ds == null)
                {

                }
                return ds;

            }

        }

    }
    /// <summary>
    /// 品质录入表信息
    /// </summary>
    public class QualityInfo
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public int OrderNum;
        /// <summary>
        /// 工件序号
        /// </summary>
        public int WorkpieceNumber;
        /// <summary>
        /// 尺寸编号
        /// </summary>
        public int SizeNumber;
        /// <summary>
        /// 尺寸名称
        /// </summary>
        public string SizeName;
        /// <summary>
        /// 理论值
        /// </summary>
        public double StandardValue;
        /// <summary>
        /// 下公差
        /// </summary>
        public double ToleranceRangeMin;
        /// <summary>
        /// 上公差
        /// </summary>
        public double ToleranceRangeMax;
        /// <summary>
        /// 实测值
        /// </summary>
        public double Measurements;
        /// <summary>
        /// 是否超差
        /// </summary>
        public int OutOfTolerance;
    }
}