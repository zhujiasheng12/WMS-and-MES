using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.工程部
{
    /// <summary>
    /// upload 的摘要说明
    /// </summary>
    public class uploadToolChartFile : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var file = context.Request.Files;
            string paths = "";
            string err = "";
            var processId = int.Parse(context.Request.Form[1]);
            var time = int.Parse(context.Request.Form["number"]);//探测点个数
            var nonCuttingTime = Convert.ToDouble(context.Request.Form["nonCuttingTime"]);//辅助时间
            var timeProportionalityCoefficient = Convert.ToDouble(context.Request.Form["timeProportionalityCoefficient"]);//比例系数
            using (JDJS_WMS_DB_USEREntities entities = new JDJS_WMS_DB_USEREntities())
            {
                using (System.Data.Entity.DbContextTransaction mytran = entities.Database.BeginTransaction())
                {
                    try
                    {
                        var row = entities.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == processId).First();
                        var orderNumber = entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == row.OrderID).First().Order_Number;

                        var exten = Path.GetExtension(file[0].FileName);
                        var fileName = "T-" + orderNumber + "-P" + row.ProcessID + exten;
                        PathInfo pathInfo = new PathInfo();
                        paths = Path.Combine(pathInfo.upLoadPath(), orderNumber, "刀具表", fileName);
                        var directoryPath= Path.Combine(pathInfo.upLoadPath(), orderNumber, "刀具表");
                        DirectoryInfo directory = new DirectoryInfo(directoryPath);
                        FileInfo[] files = directory.GetFiles();
                        foreach (var item in files)
                        {
                            item.Delete();
                        }

                        file[0].SaveAs(paths);
                        row.toolChartName = fileName;
                        entities.SaveChanges();



                        string FilePath = paths;
                        string path = Path.GetFileNameWithoutExtension(FilePath);
                        int ProcessID = 0;
                        string[] str = path.Split('-');
                        string OrderNum = str[1];
                        string Process = str[2].Substring(1);
                        List<int> ToolNum = new List<int>();
                        List<ToolInfo> toolInfos = new List<ToolInfo>();
                        string fileSuffix = System.IO.Path.GetExtension(FilePath);

                      

                        
                        entities.SaveChanges();
                        mytran.Commit();
                    }
                    catch (Exception ex)
                    {
                        mytran.Rollback();
                        context.Response.Write(ex.Message);
                        return;
                    }
                    if (WebApplication2.Function.ParsingTExcel(paths, processId, time, nonCuttingTime, timeProportionalityCoefficient, ref err))
                    {
                        context.Response.Write("ok");
                    }
                    else
                    {
                        context.Response.Write(err);
                    }
                    context.Response.Write("ok");
                }
            }
        }
        protected static bool isNumberic(string message, out double result)
        {
            //判断是否为整数字符串
            //是的话则将其转换为数字并将其设为out类型的输出值、返回true, 否则为false
            result = -1;   //result 定义为out 用来输出值
            try
            {
                //当数字字符串的为是少于4时，以下三种都可以转换，任选一种
                //如果位数超过4的话，请选用Convert.ToInt32() 和int.Parse()

                //result = int.Parse(message);
                //result = Convert.ToInt16(message);
                result = Convert.ToDouble(message);
                return true;
            }
            catch
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
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
    public class ToolInfo
    {
        public string PathName;
        public int ToolNO;
        public string ToolName;
        public string Shank;
        public double ToolLength;
        public double ToolDiameter;
        public double ToolAroidance;
        public string Time;
    }
}