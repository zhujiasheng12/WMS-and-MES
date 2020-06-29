using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using org.in2bits.MyXls;

namespace WebApplication2.生产管理.生产部.区域显示
{
    /// <summary>
    /// 打印 的摘要说明
    /// </summary>
    public class 打印 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {

            List<DataLists> dataLists = new List<DataLists>();
            {
                int LocationID = 1;
                //List<DataList> dataLists = new List<DataList>();
                int RunMachNum = 0;
                int AllMachNum;
                using (JDJS_WMS_DB_USEREntities  wms = new  JDJS_WMS_DB_USEREntities ())
                {

                    var devices = wms.JDJS_WMS_Device_Info;
                    AllMachNum = devices.Count();
                    foreach (var device in devices)
                    {
                        int cncids = device.ID;
                        var state = wms.JDJS_WMS_Device_RealTime_Data.Where(r => r.CncID == device.ID).FirstOrDefault();
                        if (state != null)
                        {
                            if (state.ProgState == 1)
                            {
                                int cncid = device.ID;
                                var platecnc = wms.JDJS_WMS_Quickchangbaseplate_Table.Where(r => r.CncID == cncid);
                                if (platecnc.Count() > 0)
                                {
                                    //RunMachNum++;
                                }
                            }
                        }

                        DataLists data = new DataLists();
                        data.cncNum = device.MachNum;
                        data.cncType = device.MachState;
                        data.State = "/";
                        data.Time = DateTime.Now.Date.ToString();
                        var state1 = wms.JDJS_WMS_Device_RealTime_Data.Where(r => r.CncID == device.ID).FirstOrDefault();
                        if (state1 != null)
                        {
                            if (state1.ProgState != -1)
                            {
                                data.State = "开机";
                                RunMachNum++;
                            }

                        }
                        var processing = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.CncID == device.ID && r.isFlag == 2);
                        if (processing.Count() > 0)
                        {
                            int id = Convert.ToInt32(processing.FirstOrDefault().ProcessID);
                            int processNum = Convert.ToInt32(wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == id).FirstOrDefault().ProcessID);
                            string OrderNum = wms.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == processing.FirstOrDefault().OrderID).FirstOrDefault().Order_Number.ToString();
                            data.doingFileName = OrderNum + "-P" + processNum.ToString();
                            data.JiaWei = processNum.ToString() + "夹";
                            data.doingProcess = OrderNum + "-" + processNum.ToString() + "序";
                            var willdo = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.CncID == device.ID && r.ProcessID == processing.FirstOrDefault().ProcessID && r.isFlag == 1);
                            data.surplusNumber = willdo.Count().ToString();
                            {
                                var cncqu = wms.JDJS_WMS_Quickchangbaseplate_Table.Where(r => r.CncID == device.ID);
                                if (cncqu.Count() < 1)
                                {
                                    data.progress = "0.000000";
                                }
                                else
                                {
                                    var cncstate = wms.JDJS_WMS_Device_RealTime_Data.Where(r => r.CncID == device.ID).FirstOrDefault();
                                    if (cncstate != null)
                                    {
                                        int states = Convert.ToInt32(cncstate.ProgState);

                                        if (states == 1)
                                        {
                                            var timestate = wms.JDJS_WMS_Device_Times_Data.Where(r => r.ID == device.ID).FirstOrDefault();
                                            if (timestate != null)
                                            {
                                                double timeMin = Convert.ToDouble(timestate.NowRunTime);
                                                double alltime = Convert.ToDouble(wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == processing.FirstOrDefault().ProcessID).FirstOrDefault().ProcessTime);
                                                double rate = timeMin / alltime;
                                                if (rate < 0)
                                                {
                                                    data.progress = "0.000000";
                                                }
                                                else if (rate >= 1)
                                                {
                                                    data.progress = "0.999999";
                                                }
                                                else
                                                {
                                                    data.progress = rate.ToString("0.000000");
                                                }

                                            }
                                            else
                                            {
                                                data.progress = "0.000000";
                                            }
                                        }
                                        else
                                        {
                                            DateTime time = Convert.ToDateTime(cncqu.FirstOrDefault().time);
                                            double nowtime = (DateTime.Now - time).TotalMinutes;
                                            var info = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.CncID == device.ID && r.isFlag == 3).ToList();
                                            if (info.Count() < 1)
                                            {
                                                data.progress = "0.000000";
                                            }
                                            else
                                            {
                                                var timeInfo = info.OrderByDescending(r => r.EndTime);
                                                DateTime oldTime = Convert.ToDateTime(timeInfo.FirstOrDefault().EndTime);
                                                if (oldTime > time)
                                                {
                                                    data.progress = "1.000000";
                                                }
                                                else
                                                {
                                                    data.progress = "0.000000";
                                                }
                                            }


                                        }

                                    }
                                    else
                                    {
                                        data.progress = "0.000000";
                                    }
                                }
                            }
                            var next = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.CncID == device.ID && r.isFlag == 1).OrderBy(r => r.StartTime);
                            if (next.Count() > 0)
                            {
                                int ids = Convert.ToInt32(next.FirstOrDefault().ProcessID);
                                int processNums = Convert.ToInt32(wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == ids).FirstOrDefault().ProcessID);
                                string OrderNums = wms.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == next.FirstOrDefault().OrderID).FirstOrDefault().Order_Number.ToString();
                                data.waitingFileName = OrderNums + "-P" + processNums.ToString();
                            }
                            var nextTask = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.CncID == device.ID && r.isFlag == 1 && r.ProcessID != processing.FirstOrDefault().ProcessID).OrderBy(r => r.StartTime);
                            if (nextTask.Count() > 0)
                            {
                                int ids = Convert.ToInt32(nextTask.FirstOrDefault().ProcessID);
                                int processNums = Convert.ToInt32(wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == ids).FirstOrDefault().ProcessID);
                                string OrderNums = wms.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == nextTask.FirstOrDefault().OrderID).FirstOrDefault().Order_Number.ToString();
                                data.waitingProcess = OrderNums + "-" + processNums.ToString() + "序";
                            }
                        }
                        else
                        {
                            processing = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.CncID == device.ID && r.isFlag == 1).OrderBy(r => r.StartTime);
                            if (processing.Count() > 0)
                            {
                                if (processing.Count() > 1)
                                {
                                    var pro = processing.ToList();
                                    int id = Convert.ToInt32(processing.FirstOrDefault().ProcessID);
                                    int processNum = Convert.ToInt32(wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == id).FirstOrDefault().ProcessID);
                                    string OrderNum = wms.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == processing.FirstOrDefault().OrderID).FirstOrDefault().Order_Number.ToString();
                                    data.doingFileName = OrderNum + "-P" + processNum.ToString();

                                    data.JiaWei = processNum.ToString() + "夹";
                                    data.doingProcess = OrderNum + "-" + processNum.ToString() + "序";
                                    var willdo = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.CncID == device.ID && r.ProcessID == processing.FirstOrDefault().ProcessID && r.isFlag == 1);
                                    data.surplusNumber = willdo.Count().ToString();
                                    {
                                        var cncqu = wms.JDJS_WMS_Quickchangbaseplate_Table.Where(r => r.CncID == device.ID);
                                        if (cncqu.Count() < 1)
                                        {
                                            data.progress = "0.000000";
                                        }
                                        else
                                        {
                                            var cncstate = wms.JDJS_WMS_Device_RealTime_Data.Where(r => r.CncID == device.ID).FirstOrDefault();
                                            if (cncstate != null)
                                            {
                                                int states = Convert.ToInt32(cncstate.ProgState);

                                                if (states == 1)
                                                {
                                                    var timestate = wms.JDJS_WMS_Device_Times_Data.Where(r => r.ID == device.ID).FirstOrDefault();
                                                    if (timestate != null)
                                                    {
                                                        double timeMin = Convert.ToDouble(timestate.NowRunTime);
                                                        double alltime = Convert.ToDouble(wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == processing.FirstOrDefault().ProcessID).FirstOrDefault().ProcessTime);
                                                        double rate = timeMin / alltime;
                                                        if (rate < 0)
                                                        {
                                                            data.progress = "0.000000";
                                                        }
                                                        else if (rate >= 1)
                                                        {
                                                            data.progress = "0.999999";
                                                        }
                                                        else
                                                        {
                                                            data.progress = rate.ToString("0.000000");
                                                        }

                                                    }
                                                    else
                                                    {
                                                        data.progress = "0.000000";
                                                    }
                                                }
                                                else
                                                {
                                                    DateTime time = Convert.ToDateTime(cncqu.FirstOrDefault().time);
                                                    double nowtime = (DateTime.Now - time).TotalMinutes;
                                                    var info = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.CncID == device.ID && r.isFlag == 3).ToList();
                                                    if (info.Count() < 1)
                                                    {
                                                        data.progress = "0.000000";
                                                    }
                                                    else
                                                    {
                                                        var timeInfo = info.OrderByDescending(r => r.EndTime);
                                                        DateTime oldTime = Convert.ToDateTime(timeInfo.FirstOrDefault().EndTime);
                                                        if (oldTime > time)
                                                        {
                                                            data.progress = "1.000000";
                                                        }
                                                        else
                                                        {
                                                            data.progress = "0.000000";
                                                        }
                                                    }


                                                }

                                            }
                                            else
                                            {
                                                data.progress = "0.000000";
                                            }
                                        }
                                    }

                                    int ids = Convert.ToInt32(pro[1].ProcessID);
                                    int processNums = Convert.ToInt32(wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == ids).FirstOrDefault().ProcessID);
                                    int idsss = Convert.ToInt32(pro[1].OrderID);
                                    string OrderNums = wms.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == idsss).FirstOrDefault().Order_Number.ToString();
                                    data.waitingFileName = OrderNums + "-P" + processNums.ToString();

                                    var nextTask = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.CncID == device.ID && r.isFlag == 1 && r.ProcessID != ids).OrderBy(r => r.StartTime);
                                    if (nextTask.Count() > 0)
                                    {
                                        int idss = Convert.ToInt32(nextTask.FirstOrDefault().ProcessID);
                                        int processNumss = Convert.ToInt32(wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == idss).FirstOrDefault().ProcessID);
                                        string OrderNumss = wms.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == nextTask.FirstOrDefault().OrderID).FirstOrDefault().Order_Number.ToString();
                                        data.waitingProcess = OrderNumss + "-" + processNumss.ToString() + "序";
                                    }
                                }
                                else
                                {
                                    int id = Convert.ToInt32(processing.FirstOrDefault().ProcessID);
                                    int processNum = Convert.ToInt32(wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == id).FirstOrDefault().ProcessID);
                                    string OrderNum = wms.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == processing.FirstOrDefault().OrderID).FirstOrDefault().Order_Number.ToString();
                                    data.doingFileName = OrderNum + "-P" + processNum.ToString();
                                    data.JiaWei = processNum.ToString() + "夹";
                                    data.doingProcess = OrderNum + "-" + processNum.ToString() + "序";
                                    var willdo = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.CncID == device.ID && r.ProcessID == processing.FirstOrDefault().ProcessID && r.isFlag == 1);
                                    data.surplusNumber = willdo.Count().ToString();
                                    {
                                        var cncqu = wms.JDJS_WMS_Quickchangbaseplate_Table.Where(r => r.CncID == device.ID);
                                        if (cncqu.Count() < 1)
                                        {
                                            data.progress = "0.000000";
                                        }
                                        else
                                        {
                                            var cncstate = wms.JDJS_WMS_Device_RealTime_Data.Where(r => r.CncID == device.ID).FirstOrDefault();
                                            if (cncstate != null)
                                            {
                                                int states = Convert.ToInt32(cncstate.ProgState);

                                                if (states == 1)
                                                {
                                                    var timestate = wms.JDJS_WMS_Device_Times_Data.Where(r => r.ID == device.ID).FirstOrDefault();
                                                    if (timestate != null)
                                                    {
                                                        double timeMin = Convert.ToDouble(timestate.NowRunTime);
                                                        double alltime = Convert.ToDouble(wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == processing.FirstOrDefault().ProcessID).FirstOrDefault().ProcessTime);
                                                        double rate = timeMin / alltime;
                                                        if (rate < 0)
                                                        {
                                                            data.progress = "0.000000";
                                                        }
                                                        else if (rate >= 1)
                                                        {
                                                            data.progress = "0.999999";
                                                        }
                                                        else
                                                        {
                                                            data.progress = rate.ToString("0.000000");
                                                        }

                                                    }
                                                    else
                                                    {
                                                        data.progress = "0.000000";
                                                    }
                                                }
                                                else
                                                {
                                                    DateTime time = Convert.ToDateTime(cncqu.FirstOrDefault().time);
                                                    double nowtime = (DateTime.Now - time).TotalMinutes;
                                                    var info = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.CncID == device.ID && r.isFlag == 3).ToList();
                                                    if (info.Count() < 1)
                                                    {
                                                        data.progress = "0.000000";
                                                    }
                                                    else
                                                    {
                                                        var timeInfo = info.OrderByDescending(r => r.EndTime);
                                                        DateTime oldTime = Convert.ToDateTime(timeInfo.FirstOrDefault().EndTime);
                                                        if (oldTime > time)
                                                        {
                                                            data.progress = "1.000000";
                                                        }
                                                        else
                                                        {
                                                            data.progress = "0.000000";
                                                        }
                                                    }


                                                }

                                            }
                                            else
                                            {
                                                data.progress = "0.000000";
                                            }
                                        }
                                    }

                                }
                            }
                            else
                            {
                                data.doingFileName = "无排配";
                                data.JiaWei = "/";
                            }
                        }
                        var endProcess = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.CncID == cncids && r.isFlag != 0);
                        var end = endProcess.OrderByDescending(r => r.EndTime).FirstOrDefault();
                        if (end != null)
                        {
                            data.StopTime = end.EndTime.ToString();
                        }

                        dataLists.Add(data);
                    }
                    foreach (var item in dataLists)
                    {
                        item.RunMachNum = RunMachNum.ToString();
                        item.AllMachNum = AllMachNum.ToString();
                        item.OtherMachNum = (AllMachNum - RunMachNum).ToString();
                    }
                    var lists = dataLists.OrderByDescending(r => r.progress);
                    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var model = new { code = 0, data = lists };
                    var json = serializer.Serialize(model);

                }
            }

            XlsDocument doc = new XlsDocument();
            doc.FileName = DateTime.Now.ToString("yyyyMMddhhmmssms") + "样机开机状况.xls";
            Worksheet sheet = doc.Workbook.Worksheets.Add("sheet1");
            Cells cells = sheet.Cells;

            ColumnInfo col = new ColumnInfo(doc, sheet); //创建列样式对象
            col.ColumnIndexStart = 5;  //起始列，索引从0开始
            col.ColumnIndexEnd = 5;    //结束列，索引从0开始，这样为第1列、第2列使用此样式
            col.Width = 8888;         //宽度，字节长度，ushort类型 0~65535
            sheet.AddColumnInfo(col);  //将列样式作用于此工作表

            ColumnInfo col2 = new ColumnInfo(doc, sheet); //创建列样式对象
            col2.ColumnIndexStart = 2;  //起始列，索引从0开始
            col2.ColumnIndexEnd = 2;    //结束列，索引从0开始，这样为第1列、第2列使用此样式
            col2.Width = 4444;         //宽度，字节长度，ushort类型 0~65535
            sheet.AddColumnInfo(col2);  //将列样式作用于此工作表

            ColumnInfo col4 = new ColumnInfo(doc, sheet); //创建列样式对象
            col4.ColumnIndexStart = 4;  //起始列，索引从0开始
            col4.ColumnIndexEnd = 4;    //结束列，索引从0开始，这样为第1列、第2列使用此样式
            col4.Width = 4444;         //宽度，字节长度，ushort类型 0~65535
            sheet.AddColumnInfo(col4);  //将列样式作用于此工作表


            MergeArea ma1 = new MergeArea(1, 1, 1, 7); //合并单元格，第2行第5列 到 第3行第7列
            sheet.AddMergeArea(ma1); //添加合并单元格到工作表

            MergeArea ma2 = new MergeArea(2, 2, 2, 6); //合并单元格，第2行第5列 到 第3行第7列
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


            cells.Add(1, 1, "智能中心样机开机状况", xf); //添加单元格内容，第2行，第5列，内容，索引从1开始 
            cells.Add(2, 2, "日期：" + DateTime.Now.Date.ToShortDateString(), xf1); //添加单元格内容，第2行，第5列，内容，索引从1开始 
            cells.Add(3, 1, "类别", xf2);
            cells.Add(3, 2, "机台号", xf2);
            cells.Add(3, 3, "文件名", xf2);
            cells.Add(3, 4, "夹位", xf2);
            cells.Add(3, 5, "开机状态", xf2);
            cells.Add(3, 6, "CNC预计结束时间", xf2);
            cells.Add(3, 7, "备注", xf2);
            int index = 4;
            for (int i = 0; i < dataLists.Count(); i++)
            {
                cells.Add(i + 4, 1, dataLists[i].cncType, xf2);
                cells.Add(i + 4, 2, dataLists[i].cncNum, xf2);
                if (dataLists[i].doingFileName == "无排配")
                {
                    cells.Add(i + 4, 3, dataLists[i].doingFileName, xf2);
                    cells.Add(i + 4, 4, dataLists[i].JiaWei, xf2);
                    cells.Add(i + 4, 5, dataLists[i].State, xf2);
                }
                else
                {
                    cells.Add(i + 4, 3, dataLists[i].doingFileName, xf3);
                    cells.Add(i + 4, 4, dataLists[i].JiaWei, xf3);
                    cells.Add(i + 4, 5, dataLists[i].State, xf3);
                }
                cells.Add(i + 4, 6, dataLists[i].StopTime, xf2);
                cells.Add(i + 4, 7, "", xf2);
                index++;
            }
            cells.Add(index, 1, "机台总数", xf);
            //cells.Add(index, 2, dataLists[lastIndex].AllMachNum, xf2);
            cells.Add(index, 3, "实际开机数量", xf);
            //cells.Add(index, 4, dataLists[lastIndex].RunMachNum, xf2);
            cells.Add(index, 5, "未开机数量", xf);
            //cells.Add(index, 6, dataLists[lastIndex].OtherMachNum, xf2);
            if (dataLists.Count() > 0)
            {
                int lastIndex = dataLists.Count() - 1;
                //cells.Add(index, 1, "机台总数", xf);
                cells.Add(index, 2, dataLists[lastIndex].AllMachNum, xf2);
                //cells.Add(index, 3, "实际开机数量", xf);
                cells.Add(index, 4, dataLists[lastIndex].RunMachNum, xf2);
                //cells.Add(index, 5, "未开机数量", xf);
                cells.Add(index, 6, dataLists[lastIndex].OtherMachNum, xf2);
            }

            PathInfo pathInfo = new PathInfo();
            string path = pathInfo.upLoadPath() +@"ExcelFile\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }


            doc.Save(path);
            context.Response.Write(pathInfo.downLoadPath()+ @"ExcelFile\" + doc.FileName);

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
    class DataLists
    {
        /// <summary>
        /// 机床编号
        /// </summary>
        public string cncNum;
        /// <summary>
        /// 机床种类
        /// </summary>
        public string cncType;
        /// <summary>
        /// 正在进行订单及工序
        /// </summary>
        public string doingProcess;
        /// <summary>
        /// 等待加工订单及工序
        /// </summary>
        public string waitingProcess;
        /// <summary>
        /// 机台当前订单剩余量
        /// </summary>
        public string surplusNumber;
        /// <summary>
        /// 正在加工文件名
        /// </summary>
        public string doingFileName;
        /// <summary>
        /// 等待加工文件名
        /// </summary>
        public string waitingFileName;
        /// <summary>
        /// cnc预计结束时间
        /// </summary>
        public string StopTime;
        /// <summary>
        /// 夹位
        /// </summary>
        public string JiaWei;
        /// <summary>
        /// 时间
        /// </summary>
        public string Time;
        /// <summary>
        /// 当前任务完成进度
        /// </summary>
        /// 
        public string progress;
        /// <summary>
        /// 开机状态
        /// </summary>
        public string State;
        public string RunMachNum;
        public string AllMachNum;
        public string OtherMachNum;
    }
}