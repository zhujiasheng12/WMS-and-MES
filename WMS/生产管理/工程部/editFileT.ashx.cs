﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApplication2.Model.生产管理.工程部
{
    /// <summary>
    /// editFileT 的摘要说明
    /// </summary>
    public class editFileT : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
             var file = context.Request.Files;
            var processId = int.Parse(context.Request.Form["processId"]);
            var time = int.Parse(context.Request.Form["number"]);//探测点个数
            var nonCuttingTime =Convert .ToDouble ( context.Request.Form["nonCuttingTime"]);//辅助时间
            var timeProportionalityCoefficient =Convert .ToDouble ( context.Request.Form["timeProportionalityCoefficient"]);//比例系数
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
                        var exten = Path.GetExtension(file[0].FileName);
                        var fileName = "T-" + orderNumber + "-P" + row.ProcessID + exten;
                        PathInfo pathInfo = new PathInfo();
                        paths = Path.Combine(pathInfo.upLoadPath(), orderNumber, "刀具表", fileName);
                        file[0].SaveAs(paths);
                        
                    
                }
            }
            if (WebApplication2.Function.ParsingTExcel(paths, processId, time,nonCuttingTime ,timeProportionalityCoefficient,  ref err))
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