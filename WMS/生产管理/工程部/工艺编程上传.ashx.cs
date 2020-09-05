using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web.SessionState;
using System.Web;
using WebApplication2.生产管理.工程部.文件管理;

namespace WebApplication2.生产管理
{
    /// <summary>
    /// 工艺编程上传 的摘要说明
    /// </summary>
    public class 工艺编程上传 : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            var form = context.Request.Form;
            var files = context.Request.Files;
            var ncFile = files["ncFile"];
            var toolChartFile = files["toolChartFile"];
            var ClampingFile = files["Clamping"];
            var processId = int.Parse(context.Request.Form["processId"]);
            var fileType = context.Request["fileType"];//"更新";"覆盖"
            bool isUpdate = true;

            if (fileType == "覆盖")
            {
                isUpdate = false;
            }
            int personId = int.Parse(context.Session["id"].ToString());
            string personName = context.Session["UserName"].ToString();

            using (JDJS_WMS_DB_USEREntities entities = new JDJS_WMS_DB_USEREntities())
            {
                using(System.Data.Entity.DbContextTransaction db = entities.Database.BeginTransaction())
                {
                    try
                    {
                        var orderId = entities.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == processId).First().OrderID;
                        var orderNum = entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == orderId).First().Order_Number;
                        var processNum = entities.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == processId).First().ProcessID;
                        PathInfo pathInfo1 = new PathInfo();
                        var directoryPath = Path.Combine(pathInfo1.upLoadPath(), orderNum, "工序" + processNum, "编程文件");
                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }
                        var count = 0;
                        while (files["programmingFile" + count] != null)
                        {
                            files["programmingFile" + count].SaveAs(Path.Combine(directoryPath, files["programmingFile" + count].FileName));
                            count++;
                        }
                        var row = entities.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == processId).First();
                        var orderNumber = entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == row.OrderID).First().Order_Number;
                       
                        var exten = Path.GetExtension(ncFile.FileName);
                        var fileName = orderNumber + "-P" + row.ProcessID + exten;
                        var oldFileName = row.programName;

                        PathInfo pathInfo = new PathInfo();

                        DirectoryInfo directoryP = new DirectoryInfo(Path.Combine(pathInfo.upLoadPath(), orderNumber, "加工文件"));
                        if (!directoryP.Exists)
                        {
                            directoryP.Create();
                        }

                        DirectoryInfo directoryT = new DirectoryInfo(Path.Combine(pathInfo.upLoadPath(), orderNumber, "刀具表"));
                        if (!directoryT.Exists)
                        {
                            directoryT.Create();
                        }
                        var path = Path.Combine(pathInfo.upLoadPath(), orderNumber, "加工文件", fileName);

                        if (System.IO.File.Exists(path))
                        {
                            if (!isUpdate)
                            {
                                System.IO.File.Delete(path);
                            }
                            else
                            {
                                int i = 1;
                                string oldPath = Path.Combine(pathInfo.upLoadPath(), orderNumber, "加工文件", orderNumber + "-P" + row.ProcessID + "-" + i.ToString() + Path.GetExtension(exten));
                                while (System.IO.File.Exists(oldPath))
                                {
                                    i++;
                                    oldPath = Path.Combine(pathInfo.upLoadPath(), orderNumber, "加工文件", orderNumber + "-P" + row.ProcessID + "-" + i.ToString() + Path.GetExtension(exten));
                                }
                                System.IO.File.Move(path, oldPath);

                            }
                        }

                        ncFile.SaveAs(path);
                        string str = "";
                        FileManage.UpdateFileToDB(Convert.ToInt32(row.OrderID), row.ID, personId, personName, FileType.加工文件, isUpdate, ref str);
                        row.programName = fileName;
                        entities.SaveChanges();

                        var ClampingFileDir = Path.Combine(pathInfo1.upLoadPath(), orderNum, "工序" + processNum, "装夹示意图");
                        if (Directory.Exists(ClampingFileDir)) {
                            Directory.Delete(ClampingFileDir,true);
                          
                        }
                        Directory.CreateDirectory(ClampingFileDir);
                        var ClampingFilePath = Path.Combine(ClampingFileDir, ClampingFile.FileName);
                        ClampingFile.SaveAs(ClampingFilePath);
                        db.Commit();
                    }
                    catch(Exception ex)
                    {
                        context.Response.Write(ex.Message);
                        db.Rollback();
                        return;
                    }
                }
              
   
              


            }






        
            var time = int.Parse(context.Request.Form["number"]);//探测点个数
            var nonCuttingTime = Convert.ToDouble(context.Request.Form["nonCuttingTime"]);//辅助时间
           // var timeProportionalityCoefficient = Convert.ToDouble(context.Request.Form["timeProportionalityCoefficient"]);//比例系数
            double timeProportionalityCoefficient = 1;
            double ToolTime = 0.1666666666;//换刀时间
            double ProcessTime = 0;//工序时间
            double OnMachMea = 0.033333333333;//探测时间
            int toolNum = 0;//换刀次数
            string ToolNo = "0";
            int toolFlag = 1;
            string paths = "";
            string err = "";
            using (JDJS_WMS_DB_USEREntities entities = new JDJS_WMS_DB_USEREntities())
            {
                using (System.Data.Entity.DbContextTransaction mytran = entities.Database.BeginTransaction())
                {
                    var row = entities.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == processId).First();
                    var orderNumber = entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == row.OrderID).First().Order_Number;
                    int cncType = Convert.ToInt32(row.DeviceType);
                    var exten = Path.GetExtension(toolChartFile.FileName);
                    var fileName = "T-" + orderNumber + "-P" + row.ProcessID + exten;
                    PathInfo pathInfo = new PathInfo();
                    paths = Path.Combine(pathInfo.upLoadPath(), orderNumber, "刀具表", fileName);
                    if (System.IO.File.Exists(paths))
                    {
                        if (!isUpdate)
                        {
                            System.IO.File.Delete(paths);
                        }
                        else
                        {
                            int i = 1;
                            string oldPath = Path.Combine(pathInfo.upLoadPath(), orderNumber, "刀具表", "T-" + orderNumber + "-P" + row.ProcessID + "-" + i.ToString() + exten);
                            while (System.IO.File.Exists(oldPath))
                            {
                                i++;
                                oldPath = Path.Combine(pathInfo.upLoadPath(), orderNumber, "刀具表", "T-" + orderNumber + "-P" + row.ProcessID + "-" + i.ToString() + exten);
                            }
                            System.IO.File.Move(paths, oldPath);

                        }
                    }
                    toolChartFile.SaveAs(paths);
                    string stre = "";
                    FileManage.UpdateFileToDB(Convert.ToInt32(row.OrderID), row.ID, personId, personName, FileType.路径工艺单, isUpdate, ref stre);

                }
            }
            if (WebApplication2.Function.ParsingTExcel(paths, processId, time, nonCuttingTime, timeProportionalityCoefficient, ref err))
            {
                context.Response.Write("ok");
            }
            else
            {
                context.Response.Write(err);
            }
            



        }

        public bool IsReusable
        {
            get
            {
                return false;
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

    }
}