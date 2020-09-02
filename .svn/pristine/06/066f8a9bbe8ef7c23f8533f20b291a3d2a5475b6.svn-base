using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Data.OleDb;
using System.Data;

namespace WebApplication2.Model.生产管理.工程部
{
    /// <summary>
    /// createWorkNumber 的摘要说明
    /// </summary>
    public class createWorkNumber : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var form = context.Request.Form;
            var files = context.Request.Files;
            var orderNumberId = int.Parse(form["orderId"]);//订单id
            var workNumber = int.Parse(form["workNumber"]);//工序号
            var number = int.Parse(form["number"]);//系数
            var blankType = form["blankType"];//毛坯种类  判断null
            var blankSpecification = form["blankSpecification"];//毛坯类型   判断null
            var blankNumber = form["blankNumber"];//毛坯数量  判断null
            var fixtureType = form["fixtureType"];//治具种类
            var fixtureSpecification = form["fixtureSpecification"];//治具规格
            var cncType = form["cncType"];//机床型号

            
            //var fileBlank = context.Request.Files["fileBlank"];//毛坯图纸 判断null
            //var fileFixture = context.Request.Files["fileFixture"];//治具图纸 判断null
            var allFiles = context.Request.Files;
            if (cncType == null)
            {
                context.Response.Write("请输入机床类型");
                return;
            }
            double ToolTime = 0.1666666666;//换刀时间
            double ProcessTime = 0;//工序时间
            double OnMachMea = 0.033333333333;//探测时间
            int toolNum = 0;//换刀次数
            string ToolNo = "0";
            int toolFlag = 1;
            PathInfo pathInfo = new PathInfo();
           
            using (JDJS_WMS_DB_USEREntities entities = new JDJS_WMS_DB_USEREntities())
            {

                using (System.Data.Entity.DbContextTransaction mytran = entities.Database.BeginTransaction())
                {
                    try
                    {




                        if (entities.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == orderNumberId & r.ProcessID == workNumber & r.sign == -1).Count() > 0)
                        {
                            context.Response.Write("该订单下工序已存在");
                            return;
                        }
                        foreach (var item in entities.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == orderNumberId&&r.sign !=0 ))
                        {
                            item.sign = -1;
                            item.program_audit_sign = -1;
                        }
                        var orderNumber = entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == orderNumberId).First().Order_Number;
                        int BlankNumber = 0;
                        int BlankType = 0;
                        string BlankSpecification = "";
                        string JigSpecification = "";
                        int FixtureType = 0;
                        FixtureType = Convert.ToInt32(fixtureType);
                        for (int i = 0; i < allFiles.Count; i++)
                        {
                            if (context.Request.Files["fileBlank"+i] != null)
                            {
                                var pathblank = Path.Combine(pathInfo.upLoadPath(), orderNumber, "工序" + workNumber.ToString(), "毛坯", context.Request.Files["fileBlank" + i].FileName);
                                DirectoryInfo directoryBlank = new DirectoryInfo(Path.Combine(pathInfo.upLoadPath(), orderNumber, "工序" + workNumber.ToString(), "毛坯"));
                                if (!directoryBlank.Exists)
                                {
                                    directoryBlank.Create();
                                }
                                context.Request.Files["fileBlank" + i].SaveAs(pathblank);
                            }
                            if (context.Request.Files["fileFixture" + i] != null)
                            {
                                var path = Path.Combine(pathInfo.upLoadPath(), orderNumber, "工序" + workNumber.ToString(), "治具", context.Request.Files["fileFixture" + i].FileName);
                                DirectoryInfo directoryFixture = new DirectoryInfo(Path.Combine(pathInfo.upLoadPath(), orderNumber, "工序" + workNumber.ToString(), "治具"));
                                if (!directoryFixture.Exists)
                                {
                                    directoryFixture.Create();
                                }
                                context.Request.Files["fileFixture" + i].SaveAs(path);
                            }
                            if (context.Request.Files["technologyFile" + i] != null)
                            {
                                var path = Path.Combine(pathInfo.upLoadPath(), orderNumber, "工序" + workNumber.ToString(), "工艺文件", context.Request.Files["technologyFile" + i].FileName);
                                DirectoryInfo directoryFixture = new DirectoryInfo(Path.Combine(pathInfo.upLoadPath(), orderNumber, "工序" + workNumber.ToString(), "工艺文件"));
                                if (!directoryFixture.Exists)
                                {
                                    directoryFixture.Create();
                                }
                                context.Request.Files["technologyFile" + i].SaveAs(path);
                            }//


                        }
                        //if (fileFixture != null)
                        //{
                        //    FileInfo fileInfo = new FileInfo(fileFixture.FileName);
                        //    var pathblank = Path.Combine(pathInfo.upLoadPath(), orderNumber, "工序" + workNumber.ToString(), "治具", fileInfo.Name);
                        //    DirectoryInfo directoryBlank = new DirectoryInfo(Path.Combine(pathInfo.upLoadPath(), orderNumber, "工序" + workNumber.ToString(),"治具"));
                        //    if (!directoryBlank.Exists)
                        //    {
                        //        directoryBlank.Create();
                        //    }
                        //    fileFixture.SaveAs(pathblank);
                        //}
                        //if (fileBlank != null)
                        //{
                        //    FileInfo fileInfo = new FileInfo(fileBlank.FileName);
                        //    var pathblank = Path.Combine(pathInfo.upLoadPath(), orderNumber, "工序" + workNumber.ToString(), "毛坯", fileInfo.Name);
                        //    DirectoryInfo directoryBlank = new DirectoryInfo(Path.Combine(pathInfo.upLoadPath(), orderNumber, "工序" + workNumber.ToString(),"毛坯"));
                        //    if (!directoryBlank.Exists)
                        //    {
                        //        directoryBlank.Create();
                        //    }
                        //    fileBlank.SaveAs(pathblank);
                        //}
                        if (fixtureSpecification == null)
                        {

                        }
                        else
                        {
                            JigSpecification = fixtureSpecification;
                        }
                        if (workNumber == 1)
                        {
                            if (blankNumber == null)
                            {
                                context.Response.Write("请输入毛坯数量");
                                return;
                            }
                            BlankNumber = Convert.ToInt32(blankNumber);
                            if (blankType == null)
                            {
                                context.Response.Write("请输入毛坯种类");
                                return;
                            }
                            BlankType = Convert.ToInt32(blankType);
                            if (BlankSpecification == null)
                            {
                               
                            }
                            else
                            {
                                BlankSpecification =blankSpecification+"#1#";
                            }
                        }
                        else
                        {
                            var process1 = entities.JDJS_WMS_Order_Blank_Table .Where (r=>r.OrderID ==orderNumberId).FirstOrDefault ();
                            if (process1 != null)
                            {
                                BlankNumber = Convert.ToInt32(process1.BlackNumber);
                                BlankType = Convert.ToInt32(process1.BlankType);
                                BlankSpecification = process1.BlankSpecification;
                            }
                            else
                            {
                                context.Response.Write("请先输入毛坯信息");
                                return;
                            }
                        }
                         
                        var judge = entities.JDJS_WMS_Order_Blank_Table.Where(r => r.OrderID == orderNumberId);
                        if (judge.Count() == 0)
                        {
                            var newRow = new JDJS_WMS_Order_Blank_Table
                            {
                                OrderID = orderNumberId,
                                BlankType = BlankType,
                                BlankSpecification = BlankSpecification,
                                BlackNumber = BlankNumber,
                                BlankState = "待备料",
                                BlanktotalPreparedNumber = 0,
                                BlankAddition = 0
                            };
                            entities.JDJS_WMS_Order_Blank_Table.Add(newRow);
                        }
                        else
                        {
                            BlankSpecification  = judge.First().BlankSpecification;
                            BlankType = Convert.ToInt32(judge.First().BlankType);

                        }

                        //var name0 = files[0].FileName;
                        //var name1 = files[1].FileName;
                        //string last0 = Path.GetExtension(name0);
                        //string last1 = Path.GetExtension(name1);
                        //var program = orderNumber + "-P" + workNumber + last0;
                        //var toolChartName = "T-" + orderNumber + "-P" + workNumber + last1;

                        
                        //var path0 = Path.Combine(pathInfo.upLoadPath(), orderNumber, "加工文件", program);
                        //DirectoryInfo directoryP = new DirectoryInfo(Path.Combine(pathInfo.upLoadPath(), orderNumber, "加工文件"));
                        //if (!directoryP.Exists)
                        //{
                        //    directoryP.Create();
                        //}
                        //files[0].SaveAs(path0);
                        //var path1 = Path.Combine(pathInfo.upLoadPath(), orderNumber, "刀具表", toolChartName);
                        //DirectoryInfo directoryT = new DirectoryInfo(Path.Combine(pathInfo.upLoadPath(), orderNumber, "刀具表"));
                        //if (!directoryT.Exists)
                        //{
                        //    directoryT.Create();
                        //}
                        //files[1].SaveAs(path1);


                        {
                            //string FilePath = path1;
                            //string path = Path.GetFileNameWithoutExtension(FilePath);
                            int ProcessID = 0;
                            //string[] str = path.Split('-');
                            string OrderNum = orderNumber;
                            string Process = workNumber.ToString ();
                            //List<int> ToolNum = new List<int>();
                            //List<ToolInfo> toolInfos = new List<ToolInfo>();
                            //string fileSuffix = System.IO.Path.GetExtension(FilePath);

                            //if (fileSuffix == ".xls")
                            //{
                            //    #region 
                            //    {
                            //        DataTable dt = getData(FilePath).Tables[0];
                            //        dt.Columns[0].ColumnName = "序号";
                            //        dt.Columns[1].ColumnName = "路径名";
                            //        dt.Columns[2].ColumnName = "刀号";
                            //        dt.Columns[3].ColumnName = "刀具";
                            //        dt.Columns[4].ColumnName = "刀柄";
                            //        dt.Columns[5].ColumnName = "刀具伸出长度";
                            //        dt.Columns[6].ColumnName = "直径";
                            //        dt.Columns[7].ColumnName = "加工时间";
                            //        for (int i = 0; i < dt.Rows.Count; i++)
                            //        {
                            //            if (i == 0 || i == 1 || i == 2 || i == 3 || i == 4 || i == 5 || i == 6 || i == 8)
                            //            {
                            //                //dt.Rows.Remove(dt.Rows[0]);
                            //            }
                            //            else if (i == 7)
                            //            {

                            //                ProcessTime = 0;
                            //            }
                            //            else
                            //            {


                            //                string toolnum = dt.Rows[i]["刀号"].ToString();
                            //                double _toolnum = 0;
                            //                if (isNumberic(toolnum, out _toolnum))
                            //                {
                            //                    if (toolnum != ToolNo)
                            //                    {
                            //                        toolNum++;
                            //                        ToolNo = toolnum;
                            //                    }

                            //                    if (dt.Rows[i]["加工时间"].ToString() != "" && dt.Rows[i]["加工时间"] != null)
                            //                    {
                            //                        if (dt.Rows[i]["加工时间"].ToString().Contains(':'))
                            //                        {
                            //                            var timeInfo = dt.Rows[i]["加工时间"].ToString().Split(':');
                            //                            int hT = Convert.ToInt32(timeInfo[0]);
                            //                            int minT = Convert.ToInt32(timeInfo[1]);
                            //                            int secT = Convert.ToInt32(timeInfo[2]);
                            //                            ProcessTime += (hT * 60 + minT + ((float)secT / 60));
                            //                        }
                            //                        else
                            //                        {
                            //                            ProcessTime += (Convert.ToDouble(dt.Rows[i]["加工时间"].ToString()) * 1440);
                            //                        }
                            //                    }

                            //                    if (!ToolNum.Contains(Convert.ToInt32(dt.Rows[i]["刀号"].ToString())))
                            //                    {
                            //                        ToolNum.Add(Convert.ToInt32(dt.Rows[i]["刀号"].ToString()));
                            //                        ToolInfo tool = new ToolInfo();
                            //                        tool.PathName = dt.Rows[i]["路径名"].ToString();
                            //                        string toolno = dt.Rows[i]["刀号"].ToString();
                            //                        double _toolno = 0;
                            //                        if (isNumberic(toolno, out _toolno))
                            //                        {
                            //                            tool.ToolNO = Convert.ToInt32(dt.Rows[i]["刀号"].ToString());
                            //                        }
                            //                        tool.ToolName = dt.Rows[i]["刀具"].ToString();
                            //                        string tooll = dt.Rows[i]["刀具伸出长度"].ToString();
                            //                        double _tooll = 0;
                            //                        if (isNumberic(tooll, out _tooll))
                            //                        {
                            //                            tool.ToolLength = Convert.ToDouble(dt.Rows[i]["刀具伸出长度"].ToString());
                            //                        }
                            //                        string ToolD = dt.Rows[i]["直径"].ToString();
                            //                        double _ToolD = 0;
                            //                        if (isNumberic(ToolD, out _ToolD))
                            //                        {
                            //                            tool.ToolDiameter = Convert.ToDouble(dt.Rows[i]["直径"].ToString());
                            //                        }

                            //                        tool.Shank = dt.Rows[i]["刀柄"].ToString();
                            //                        toolInfos.Add(tool);
                            //                    }
                            //                }
                            //            }

                            //        }
                            //    }
                            //    #endregion
                            //}
                            //else
                            //{
                            //    mytran.Rollback();
                            //    context.Response.Write("刀具表请输入xls格式文件");
                            //    return;

                            //}
                            //var toolStand = entities.JDJS_WMS_Tool_Standard_Table.ToList();
                            //var CETOU = toolStand.Where(r => r.Name == "测头");
                            //int cetouID = -1;
                            //if (CETOU.Count() > 0)
                            //{
                            //    cetouID = Convert.ToInt32(CETOU.First().ToolID.Substring(1));
                            //}
                            //List<string> toolStandNo = new List<string>();
                            //foreach (var real in toolStand)
                            //{
                            //    string strT = real.ToolID;
                            //    toolStandNo.Add(strT);
                            //}
                            //foreach (var item in toolInfos)
                            //{
                            //    //判断是否有特殊刀具，判断刀具是否在刀具标准表是否存在

                            //    if (!toolStandNo.Contains("T" + item.ToolNO.ToString()))
                            //    {
                            //        toolFlag = 0;
                            //        break;
                            //    }
                            //}
                            //double times = Math.Ceiling(OnMachMea * time + ToolTime * toolNum + ProcessTime);
                            var row = new JDJS_WMS_Order_Process_Info_Table
                            {
                                OrderID = orderNumberId,
                                ProcessID = workNumber,
                                //ProcessTime = times,
                                DeviceType = Convert.ToInt32 ( cncType),
                                BlankType = BlankType,
                                BlankSpecification = BlankSpecification,
                                JigSpecification = JigSpecification+"#1#",
                                //programName = program,
                                //toolChartName = toolChartName,
                                sign = -1,
                                BlankNumber = BlankNumber,
                                toolPreparation = toolFlag,
                                JigType = FixtureType,
                                Modulus = number


                            };
                            entities.JDJS_WMS_Order_Process_Info_Table.Add(row);
                            entities.SaveChanges();

                            var order = entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_Number == OrderNum);
                            if (order.Count() > 0)
                            {
                                int orderid = order.First().Order_ID;
                                double pros = 0;
                                if (isNumberic(Process, out pros))
                                {
                                    int prose = Convert.ToInt32(Process);
                                    var pro = entities.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == orderid && r.ProcessID == prose & r.sign == -1);
                                    if (pro.Count() > 0)
                                    {
                                        ProcessID = pro.First().ID;
                                    }
                                }
                            }
                            //foreach (var item in toolInfos)
                            //{

                            //    if (item.ToolNO != cetouID)
                            //    {
                            //        JDJS_WMS_Order_Process_Tool_Info_Table tool = new JDJS_WMS_Order_Process_Tool_Info_Table()
                            //        {
                            //            ProcessID = ProcessID,
                            //            PathName = item.PathName,
                            //            ToolNO = item.ToolNO,
                            //            ToolName = item.ToolName,
                            //            ToolLength = item.ToolLength,
                            //            ToolDiameter = item.ToolDiameter,
                            //            //ToolAroidance = item.ToolAroidance,
                            //            Shank = item.Shank
                            //        };
                            //        entities.JDJS_WMS_Order_Process_Tool_Info_Table.Add(tool);
                            //    }
                            //}
                            var processinfo = entities.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ProcessID == ProcessID & r.OrderID == orderNumberId & r.sign != 0);
                            int id = ProcessID;
                            var orderNum = entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == orderNumberId);
                            int output = orderNum.FirstOrDefault().Product_Output;
                            JDJS_WMS_Order_Fixture_Manager_Table fix = new JDJS_WMS_Order_Fixture_Manager_Table()
                            {
                                ProcessID = id,
                                FixtureNumber = 0,
                                FixtureFinishPerpareNumber = 0,
                                FixtureAdditionNumber = 0

                            };

                            JDJS_WMS_Warehouse_InOut_History_Table jd = new JDJS_WMS_Warehouse_InOut_History_Table()
                            {
                                InNum =0,
                                OutNum =0,
                                ProcessId =id
                            };
                            entities.JDJS_WMS_Warehouse_InOut_History_Table.Add(jd);
                            entities.JDJS_WMS_Order_Fixture_Manager_Table.Add(fix);
                            entities.SaveChanges();
                        }


                        //var path0 = Path.Combine(@"D:\服务器文件勿动", orderNumber, program);

                        entities.SaveChanges();
                        mytran.Commit();

                    }
                    catch (Exception ex)
                    {
                        mytran.Rollback();
                        context.Response.Write(ex.Message);
                        return;
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
    public class ToolInfo
    {
        public string PathName;
        public int ToolNO;
        public string ToolName;
        public string Shank;
        public double ToolLength;
        public double ToolDiameter;
        public double ToolAroidance;
    }
}