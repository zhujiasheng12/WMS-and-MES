using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using Microsoft.Office.Interop.Excel;

namespace 读取刀具文件
{
    class Program
    {
        /// <summary>
        /// 解析固定刀具表
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {

            //解析excel文件
            int MachTypeID = 1;
            string path = @"C:\Users\82144\Desktop\常规刀具表-机床型号绑定.xlsx";
            string fileSuffix = Path.GetExtension(path);
            if (fileSuffix != ".xls" && fileSuffix != ".xlsx")
            {
                Console.WriteLine("请输入xls或xlsx格式文件！");
                return;
            }
            var data = ReadXls(path, 1);
            var x = Convert.ToInt32(data.GetLongLength(0));
            var y = Convert.ToInt32(data.GetLongLength(1));
            int arrayx = 0;
            int arrayy = 0;
            var dataArray = new string[x, y];
            foreach (var item in data)
            {
                if (item != null)
                {
                    dataArray[arrayx, arrayy] = item.ToString();
                }
                arrayy++;
                if (arrayy >= y)
                {
                    arrayx++;
                    arrayy = 0;
                }
            }
            using (WmsAutoEntities wms =new WmsAutoEntities ())
            {
                using (System.Data.Entity.DbContextTransaction mytran = wms.Database.BeginTransaction())
                {
                    try
                    {
                        List<ToolInfo> toolInfos = new List<ToolInfo>();
                        for (int i = 2; i < x; i++)
                        {
                            ToolInfo toolInfo = new ToolInfo();
                            double a = 0;
                            if (dataArray[i, 6]!=null&&isNumberic(dataArray[i, 6],out a))
                            {
                                toolInfo.Feed = Convert.ToDouble(dataArray[i, 6]);
                            }
                            toolInfo.Knife = 0;
                            toolInfo.MachTypeID = MachTypeID;
                            toolInfo.ProcessStage = "精";
                            if (dataArray[i, 5]!=null&&isNumberic(dataArray[i, 5], out a))
                            {
                                toolInfo.RotatingSpeed = Convert.ToDouble(dataArray[i, 5]);
                            }
                            toolInfo.Shank = dataArray[i, 1];
                            toolInfo.Sort = "常规";
                            toolInfo.Specification = dataArray[i, 4];
                            toolInfo.ToolID = "T" + dataArray[i, 2];
                            if (dataArray[i, 3]!=null&&isNumberic(dataArray[i, 3],out a))
                            {
                                toolInfo.ToolLength =Convert.ToDouble ( dataArray[i, 3]);
                            }
                            toolInfo.ToolName = dataArray[i, 0];
                            toolInfos.Add(toolInfo);
                        }
                        foreach (var item in toolInfos)
                        {
                            var tool = wms.JDJS_WMS_Tool_Standard_Table.Where(r => r.MachTypeID == item.MachTypeID && r.ToolID == item.ToolID);
                            foreach (var real in tool)
                            {
                                wms.JDJS_WMS_Tool_Standard_Table.Remove(real);
                            }
                            JDJS_WMS_Tool_Standard_Table jDJS_WMS_Tool_Standard_Table = new JDJS_WMS_Tool_Standard_Table()
                            {
                                Feed =item.Feed ,
                                Knife =item.Knife ,
                                MachTypeID =item.MachTypeID ,
                                Name =item.ToolName ,
                                ProcessStage =item.ProcessStage ,
                                RazorDiameter =item.RazorDiameter ,
                                RotatingSpeed =item.RotatingSpeed ,
                                Shank =item.Shank ,
                                Sort =item.Sort ,
                                Specification =item.Specification ,
                                ToolDiameter =item.ToolDiameter ,
                                ToolID =item.ToolID ,
                                ToolLength =item.ToolLength 
                            };
                            wms.JDJS_WMS_Tool_Standard_Table.Add(jDJS_WMS_Tool_Standard_Table);
                        }
                        wms.SaveChanges();
                        mytran.Commit();
                    }
                    catch(Exception ex)
                    {
                        mytran.Rollback();
                        Console.WriteLine(ex.Message );
                        return;

                    }
                }

            }
           
        }


        public static Array ReadXls(string filename, int index)//读取第index个sheet的数据   
        {            //启动Excel应用程序    
            Microsoft.Office.Interop.Excel.Application xls = new Microsoft.Office.Interop.Excel.Application();
            //打开filename表          
            _Workbook book = xls.Workbooks.Open(filename, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value);
            _Worksheet sheet;//定义sheet变量         
            xls.Visible = false;//设置Excel后台运行        
            xls.DisplayAlerts = false;//设置不显示确认修改提示    
            try
            {
                sheet = (_Worksheet)book.Worksheets.get_Item(index);//获得第index个sheet，准备读取          
            }
            catch (Exception ex)//不存在就退出     
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            Console.WriteLine(sheet.Name);
            int row = sheet.UsedRange.Rows.Count;//获取不为空的行数     
            int col = sheet.UsedRange.Columns.Count;//获取不为空的列数      
                                                    // Array value = (Array)sheet.get_Range(sheet.Cells[1, 1], sheet.Cells[row, col]).Cells.Value2;//获得区域数据赋值给Array数组，方便读取                      
            Microsoft.Office.Interop.Excel.Range range = sheet.Range[sheet.Cells[1, 1], sheet.Cells[row, col]];
            Array value = (Array)range.Value2;
            book.Save();//保存           
            book.Close(false, System.Reflection.Missing.Value, System.Reflection.Missing.Value);//关闭打开的表   
            xls.Quit();//Excel程序退出        
                       //sheet,book,xls设置为null，防止内存泄露     
            sheet = null;
            book = null;
            xls = null;
            GC.Collect();//系统回收资源      
            return value;
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

                    System.Data.DataTable table = conn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, null);
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


        /// <summary>
        /// 根据机床型号读取固定刀具信息
        /// </summary>
        /// <param name="MachTpyeID">机床型号主键ID</param>
        /// <returns></returns>
        private string ReadTool(int MachTpyeID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<ReadToolInfo> toolInfos = new List<ReadToolInfo>();
            using (WmsAutoEntities wms = new WmsAutoEntities())
            {
                var tools = wms.JDJS_WMS_Tool_Standard_Table.Where(r => r.MachTypeID == MachTpyeID);
                foreach (var item in tools)
                {
                    ReadToolInfo toolInfo = new ReadToolInfo();
                    toolInfo.id = item.ID;
                    toolInfo.Feed = item.Feed.ToString ();
                    toolInfo.Knife = item.Knife.ToString ();
                    toolInfo.MachTypeID = MachTpyeID;
                    toolInfo.ProcessStage = item.ProcessStage.ToString();
                    toolInfo.RazorDiameter = item.RazorDiameter.ToString();
                    toolInfo.RotatingSpeed = item.RotatingSpeed.ToString();
                    toolInfo.Shank = item.Shank.ToString();
                    toolInfo.Sort = item.Sort.ToString();
                    toolInfo.Specification = item.Specification.ToString();
                    toolInfo.ToolDiameter = item.ToolDiameter.ToString();
                    toolInfo.ToolID = item.ToolID.ToString();
                    toolInfo.ToolLength = item.ToolLength.ToString();
                    toolInfo.ToolName = item.Name;
                    toolInfos.Add(toolInfo);
                }
            }
            var json = serializer.Serialize(toolInfos);
            return json;
        }

        /// <summary>
        /// 修改单独一条固定刀具信息
        /// </summary>
        /// <param name="ToolID">该固定刀具主键ID</param>
        /// <param name="toolInfo">刀具信息</param>
        /// <param name="ErrStr">错误信息</param>
        /// <returns></returns>
        private bool AlterTool(int ToolID, ToolInfo toolInfo ,ref string ErrStr)
        {
            using (WmsAutoEntities wms = new WmsAutoEntities())
            {
                using (System.Data.Entity.DbContextTransaction mytran=wms.Database .BeginTransaction ())
                {
                    try
                    {
                        var tool = wms.JDJS_WMS_Tool_Standard_Table.Where(r => r.ID == ToolID).FirstOrDefault();
                        if (tool == null)
                        {
                            mytran.Rollback();
                            ErrStr = "该固定刀具不存在！";
                            return false;
                        }
                        tool.Feed = toolInfo.Feed;
                        tool.Knife = toolInfo.Knife;
                        tool.Name = toolInfo.ToolName;
                        tool.ProcessStage = toolInfo.ProcessStage;
                        tool.RazorDiameter = toolInfo.RazorDiameter;
                        tool.RotatingSpeed = toolInfo.RotatingSpeed;
                        tool.Shank = toolInfo.Shank;
                        tool.Sort = toolInfo.Sort;
                        tool.Specification = toolInfo.Specification;
                        tool.ToolDiameter = toolInfo.ToolDiameter;
                        tool.ToolID = toolInfo.ToolID;
                        tool.ToolLength = toolInfo.ToolLength;
                        
                        wms.SaveChanges();
                        mytran.Commit();
                        return true;
                    }
                    catch(Exception ex)
                    {
                        mytran.Rollback();
                        ErrStr = ex.Message;
                        return false;

                    }

                }
            }
        }

    }

    public class ToolInfo
    {
        /// <summary>
        /// 刀具编号
        /// </summary>
        public string ToolID;
        /// <summary>
        /// 刀具常规
        /// </summary>
        public string Sort;
        /// <summary>
        /// 刀具名称
        /// </summary>
        public string ToolName;
        /// <summary>
        /// 刀具规格
        /// </summary>
        public string Specification;
        /// <summary>
        /// 粗精？
        /// </summary>
        public string  ProcessStage;
        /// <summary>
        /// 刀杆直径
        /// </summary>
        public double RazorDiameter;
        /// <summary>
        /// 刀具直径
        /// </summary>
        public double ToolDiameter;
        /// <summary>
        /// 装刀长度
        /// </summary>
        public double ToolLength;
        /// <summary>
        /// 转速
        /// </summary>
        public double RotatingSpeed;
        /// <summary>
        /// 进给
        /// </summary>
        public double Feed;
        /// <summary>
        /// 吃刀量
        /// </summary>
        public double Knife;
        /// <summary>
        /// 刀柄
        /// </summary>
        public string Shank;
        /// <summary>
        /// 机床型号主键
        /// </summary>
        public int MachTypeID;
    }

    public class ReadToolInfo
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int id;
        /// <summary>
        /// 刀具编号
        /// </summary>
        public string ToolID;
        /// <summary>
        /// 刀具常规
        /// </summary>
        public string Sort;
        /// <summary>
        /// 刀具名称
        /// </summary>
        public string ToolName;
        /// <summary>
        /// 刀具规格
        /// </summary>
        public string Specification;
        /// <summary>
        /// 粗精？
        /// </summary>
        public string ProcessStage;
        /// <summary>
        /// 刀杆直径
        /// </summary>
        public string RazorDiameter;
        /// <summary>
        /// 刀具直径
        /// </summary>
        public string ToolDiameter;
        /// <summary>
        /// 装刀长度
        /// </summary>
        public string ToolLength;
        /// <summary>
        /// 转速
        /// </summary>
        public string RotatingSpeed;
        /// <summary>
        /// 进给
        /// </summary>
        public string Feed;
        /// <summary>
        /// 吃刀量
        /// </summary>
        public string Knife;
        /// <summary>
        /// 刀柄
        /// </summary>
        public string Shank;
        /// <summary>
        /// 机床型号主键
        /// </summary>
        public int MachTypeID;
    }
}

