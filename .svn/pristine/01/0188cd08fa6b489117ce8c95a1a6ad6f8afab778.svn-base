using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using WebApplication2.生产管理.工程部.文件管理;

namespace WebApplication2.生产管理.工程部
{
    /// <summary>
    /// upload 的摘要说明
    /// </summary>
    public class uploadToolChartFile : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            var file = context.Request.Files;
            var fileType = context.Request["fileType"];//"更新";"覆盖"
            bool isUpdate = true;
            if (fileType == "覆盖")
            {
                isUpdate = false;
            }
            int personId = int.Parse(context.Session["id"].ToString());
            string personName = context.Session["UserName"].ToString();
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

                        if (System.IO.File.Exists(paths))
                        {
                            if (!isUpdate)
                            {
                                System.IO.File.Delete(paths);
                            }
                            else
                            {
                                int i = 1;
                                string oldPath = Path.Combine(pathInfo.upLoadPath (), orderNumber, "刀具表", "T-" + orderNumber + "-P" + row.ProcessID + "-" + i.ToString() + exten);
                                while (System.IO.File.Exists(oldPath))
                                {
                                    i++;
                                    oldPath = Path.Combine(pathInfo.upLoadPath(), orderNumber, "刀具表", "T-" + orderNumber + "-P" + row.ProcessID + "-" + i.ToString() + exten);
                                }
                                System.IO.File.Move(paths, oldPath);

                            }
                        }


                        file[0].SaveAs(paths);
                        row.toolChartName = fileName;
                        entities.SaveChanges();

                        string stre = "";
                        FileManage.UpdateFileToDB(Convert.ToInt32(row.OrderID), row.ID, personId, personName, FileType.路径工艺单, isUpdate, ref stre);
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