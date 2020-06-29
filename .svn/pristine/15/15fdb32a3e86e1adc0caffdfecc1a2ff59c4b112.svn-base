using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using org.in2bits.MyXls;
namespace WebApplication2.生产管理.市场部
{
    /// <summary>
    /// 导出表 的摘要说明
    /// </summary>
    public class 导出表 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //导表智能中心订单进度表
            List<OrderInfoSchedu> orderInfoSchedus = new List<OrderInfoSchedu>();
            int mounth = DateTime.Now.Month;
            int year = DateTime.Now.Year;
            using (JDJS_WMS_DB_USEREntities  wms = new  JDJS_WMS_DB_USEREntities ())
            {
                var orders = wms.JDJS_WMS_Order_Entry_Table.Where(r => r.Intention == 2 || r.Intention == 3 || r.Intention == 4);
                foreach (var item in orders)
                {
                    int orderID = Convert.ToInt32(item.Order_ID);
                    if (item.Intention == 4)
                    {
                        var overProcess = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.OrderID == orderID && r.isFlag != 0);
                        var endTime = overProcess.OrderByDescending(r => r.EndTime).FirstOrDefault();
                        if (endTime != null)
                        {
                            var time = Convert.ToDateTime(endTime.EndTime).Month;
                            var yeartime = Convert.ToDateTime(endTime.EndTime).Year;
                            if (time != mounth &&year !=yeartime)
                            {
                                break;
                            }
                        }
                    }
                    OrderInfoSchedu orderInfoSchedu = new OrderInfoSchedu();
                    var orderInf0 = wms.JDJS_WMS_Order_Guide_Schedu_Table.Where(r => r.OrderID == orderID).FirstOrDefault();
                    if (orderInf0 != null)
                    {
                        orderInfoSchedu.Client = orderInf0.ClientName;
                        if (orderInf0.EndTime != null)
                        {
                            orderInfoSchedu.EndTime = Convert.ToDateTime(orderInf0.EndTime).Date.ToShortDateString();
                        }
                        else
                        {
                            orderInfoSchedu.EndTime = "/";
                        }
                        if (orderInf0.ExpectEndTime != null)
                        {
                            orderInfoSchedu.ExpectEndTime = Convert.ToDateTime(orderInf0.ExpectEndTime).Date.ToShortDateString();
                        }
                        else
                        {
                            orderInfoSchedu.ExpectEndTime = "/";
                        }
                        if (orderInf0.FileDownTime != null)
                        {
                            orderInfoSchedu.FileDownTime = Convert.ToDateTime(orderInf0.FileDownTime).Date.ToShortDateString();
                        }
                        else
                        {
                            orderInfoSchedu.FileDownTime = "/";
                        }

                    }
                    orderInfoSchedu.Name = item.Product_Name;
                    orderInfoSchedu.OrderNum = item.Order_Number;
                    orderInfoSchedu.EngineerName = item.Engine_Program_Manager;
                    var processes = wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == orderID && r.sign != 0);

                    orderInfoSchedu.jiawei = processes.Count().ToString();
                    var schedu1 = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.OrderID == orderID && r.isFlag == 1);
                    var schedu2 = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.OrderID == orderID && r.isFlag == 2);
                    var schedu3 = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.OrderID == orderID && r.isFlag == 3);
                    if (schedu1.Count() > 0 && schedu2.Count() < 1 && schedu3.Count() < 1)
                    {

                        orderInfoSchedu.State = "等待生产中";
                        var blankInfo = wms.JDJS_WMS_Blank_Table.Where(r => r.OrderID == orderID);
                        if (blankInfo.Count() < 1)
                        {
                            orderInfoSchedu.State = "待料";
                        }
                        var toolInfo = wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == orderID && r.sign != 0 && r.toolPreparation == 1);
                        if (toolInfo.Count() < 1)
                        {
                            orderInfoSchedu.State = "待备刀装刀";
                        }
                    }
                    if (schedu1.Count() < 1 && schedu2.Count() < 1 && schedu3.Count() < 1)
                    {

                        orderInfoSchedu.State = "编程中";
                    }
                    if ((schedu2.Count() > 0))
                    {

                        orderInfoSchedu.State = "生产中";
                    }
                    if (schedu1.Count() > 0 && schedu3.Count() > 0)
                    {

                        orderInfoSchedu.State = "生产中";
                    }
                    if (schedu1.Count() < 1 && schedu2.Count() < 1 && schedu3.Count() > 0)
                    {

                        orderInfoSchedu.State = "已完成";
                    }
                    orderInfoSchedus.Add(orderInfoSchedu);
                }
            }

            XlsDocument doc = new XlsDocument();
            doc.FileName = DateTime.Now.ToString("yyyyMMddhhmmssms")+"订单进度" + ".xls";
            Worksheet sheet = doc.Workbook.Worksheets.Add("sheet1");
            Cells cells = sheet.Cells;

            ColumnInfo col = new ColumnInfo(doc, sheet); //创建列样式对象
            col.ColumnIndexStart = 2;  //起始列，索引从0开始
            col.ColumnIndexEnd = 2;    //结束列，索引从0开始，这样为第1列、第2列使用此样式
            col.Width = 8888;         //宽度，字节长度，ushort类型 0~65535
            sheet.AddColumnInfo(col);  //将列样式作用于此工作表

            ColumnInfo col2 = new ColumnInfo(doc, sheet); //创建列样式对象
            col2.ColumnIndexStart = 3;  //起始列，索引从0开始
            col2.ColumnIndexEnd = 3;    //结束列，索引从0开始，这样为第1列、第2列使用此样式
            col2.Width = 3333;         //宽度，字节长度，ushort类型 0~65535
            sheet.AddColumnInfo(col2);  //将列样式作用于此工作表

            ColumnInfo col6 = new ColumnInfo(doc, sheet); //创建列样式对象
            col6.ColumnIndexStart = 6;  //起始列，索引从0开始
            col6.ColumnIndexEnd = 6;    //结束列，索引从0开始，这样为第1列、第2列使用此样式
            col6.Width = 5555;         //宽度，字节长度，ushort类型 0~65535
            sheet.AddColumnInfo(col6);  //将列样式作用于此工作表

            ColumnInfo col7 = new ColumnInfo(doc, sheet); //创建列样式对象
            col7.ColumnIndexStart = 7;  //起始列，索引从0开始
            col7.ColumnIndexEnd = 7;    //结束列，索引从0开始，这样为第1列、第2列使用此样式
            col7.Width = 5555;         //宽度，字节长度，ushort类型 0~65535
            sheet.AddColumnInfo(col7);  //将列样式作用于此工作表

            ColumnInfo col8 = new ColumnInfo(doc, sheet); //创建列样式对象
            col8.ColumnIndexStart = 8;  //起始列，索引从0开始
            col8.ColumnIndexEnd = 8;    //结束列，索引从0开始，这样为第1列、第2列使用此样式
            col8.Width = 5555;         //宽度，字节长度，ushort类型 0~65535
            sheet.AddColumnInfo(col8);  //将列样式作用于此工作表



            MergeArea ma1 = new MergeArea(1, 1, 1, 10); //合并单元格，第2行第5列 到 第3行第7列
            sheet.AddMergeArea(ma1); //添加合并单元格到工作表

            MergeArea ma2 = new MergeArea(2, 2, 2, 9); //合并单元格，第2行第5列 到 第3行第7列
            sheet.AddMergeArea(ma2); //添加合并单元格到工作表

            XF xf = doc.NewXF(); //单元格样式对象
            xf.VerticalAlignment = VerticalAlignments.Centered; //垂直居中
            xf.HorizontalAlignment = HorizontalAlignments.Centered;  //水平居中

            xf.Pattern = 0; //填充风格，0为无色填充，1为没有间隙的纯色填充
            xf.PatternColor = Colors.Green; //填充背景底色

            xf.Font.ColorIndex = 0; //字体前景色颜色，未知值
            xf.Font.FontName = "黑体"; //字体 
            xf.Font.Height = 15 * 15; //字体大小 
                                      //xf.UseBorder = false ; //使用边框 
                                      //xf.BottomLineStyle = 1; //边框样式 
                                      //xf.BottomLineColor = Colors.Black; //边框颜色 

            XF xf1 = doc.NewXF(); //单元格样式对象
            xf1.VerticalAlignment = VerticalAlignments.Centered; //垂直居中
            xf1.HorizontalAlignment = HorizontalAlignments.Right;  //水平居中

            xf1.Pattern = 0; //填充风格，0为无色填充，1为没有间隙的纯色填充
            xf1.PatternColor = Colors.Green; //填充背景底色

            xf1.Font.ColorIndex = 0; //字体前景色颜色，未知值
            xf1.Font.FontName = "宋体"; //字体 
            xf1.Font.Height = 13 * 13; //字体大小 
                                       //xf1.UseBorder = false ; //使用边框 
                                       //xf1.BottomLineStyle = 1; //边框样式 
                                       //xf1.BottomLineColor = Colors.Black; //边框颜色

            XF xf2 = doc.NewXF(); //单元格样式对象
            xf2.VerticalAlignment = VerticalAlignments.Centered; //垂直居中
            xf2.HorizontalAlignment = HorizontalAlignments.Centered;  //水平居中

            xf2.Pattern = 0; //填充风格，0为无色填充，1为没有间隙的纯色填充
            xf2.PatternColor = Colors.Green; //填充背景底色

            xf2.Font.ColorIndex = 0; //字体前景色颜色，未知值
            xf2.Font.FontName = "宋体"; //字体 
            xf2.Font.Height = 13 * 13; //字体大小 
                                       //xf2.UseBorder = false; //使用边框 
                                       //xf2.BottomLineStyle = 1; //边框样式 
                                       //xf2.BottomLineColor = Colors.Black; //边框颜色

            XF xf3 = doc.NewXF(); //单元格样式对象
            xf3.VerticalAlignment = VerticalAlignments.Centered; //垂直居中
            xf3.HorizontalAlignment = HorizontalAlignments.Centered;  //水平居中

            xf3.Pattern = 1; //填充风格，0为无色填充，1为没有间隙的纯色填充
            xf3.PatternColor = Colors.Green; //填充背景底色

            xf3.Font.ColorIndex = 0; //字体前景色颜色，未知值
            xf3.Font.FontName = "宋体"; //字体 
            xf3.Font.Height = 13 * 13; //字体大小 
                                       //xf2.UseBorder = false; //使用边框 
                                       //xf2.BottomLineStyle = 1; //边框样式 
                                       //xf2.BottomLineColor = Colors.Black; //边框颜色

            var DATA = DateTime.Now.Year.ToString() + "年" + DateTime.Now.Month.ToString() + "月";
            cells.Add(1, 1, DATA + "智能中心项目进度表", xf); //添加单元格内容，第2行，第5列，内容，索引从1开始 
            cells.Add(2, 2, "日期：" + DateTime.Now.Date.ToShortDateString(), xf1); //添加单元格内容，第2行，第5列，内容，索引从1开始 
            cells.Add(3, 1, "序号", xf2);
            cells.Add(3, 2, "客户名称", xf2);
            cells.Add(3, 3, "客户零件编号", xf2);
            cells.Add(3, 4, "内部零件编号", xf2);
            cells.Add(3, 5, "工程师", xf2);
            cells.Add(3, 6, "夹位", xf2);
            cells.Add(3, 7, "文件下发时间", xf2);
            cells.Add(3, 8, "预计编程完成时间", xf2);
            cells.Add(3, 9, "完成时间", xf2);
            cells.Add(3, 10, "备注", xf2);
            int index = 4;
            for (int i = 0; i < orderInfoSchedus.Count(); i++)
            {
                cells.Add(i + 4, 1, (i + 1).ToString(), xf2);
                cells.Add(i + 4, 2, orderInfoSchedus[i].Client, xf2);

                cells.Add(i + 4, 3, orderInfoSchedus[i].Name, xf2);
                cells.Add(i + 4, 4, orderInfoSchedus[i].OrderNum, xf2);
                cells.Add(i + 4, 5, orderInfoSchedus[i].EngineerName, xf2);


                cells.Add(i + 4, 6, orderInfoSchedus[i].jiawei, xf2);
                cells.Add(i + 4, 7, orderInfoSchedus[i].FileDownTime, xf2);
                cells.Add(i + 4, 8, orderInfoSchedus[i].ExpectEndTime, xf2);
                cells.Add(i + 4, 9, orderInfoSchedus[i].EndTime, xf2);
                cells.Add(i + 4, 10, orderInfoSchedus[i].State, xf2);
                index++;
            }

            PathInfo pathInfo = new PathInfo();
            string path = pathInfo.upLoadPath() + @"ExcelFile\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }


            doc.Save(path);
            context.Response.Write(pathInfo.downLoadPath() + @"ExcelFile\" + doc.FileName);

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
    class OrderInfoSchedu
    {
        /// <summary>
        /// 客户
        /// </summary>
        public string Client;
        /// <summary>
        /// 客户零件编号
        /// </summary>
        public string Name;
        /// <summary>
        /// 内部零件编号
        /// </summary>
        public string OrderNum;
        /// <summary>
        /// 工程师
        /// </summary>
        public string EngineerName;
        /// <summary>
        /// 夹位
        /// </summary>
        public string jiawei;
        /// <summary>
        /// 文件下发时间
        /// </summary>
        public string FileDownTime;
        /// <summary>
        /// 预计完成时间
        /// </summary>
        public string ExpectEndTime;
        /// <summary>
        /// 完成时间
        /// </summary>
        public string EndTime;
        /// <summary>
        /// 备注，订单状态
        /// </summary>
        public string State;


    }
}