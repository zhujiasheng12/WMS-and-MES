using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace WebApplication2.生产管理.工程部
{
    /// <summary>
    /// 审核 的摘要说明
    /// </summary>
    public class 审核 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var form = context.Request.Form;

            var files = context.Request.Files;//审核文件
            var orderId = Convert.ToInt32(form["orderId"]);//审核的订单
            var result = form["result"];//结果
            var text = form["text"];//审核的说明
            var type = form["type"];
            string orderNumber = "";
            PathInfo pathInfo = new PathInfo();
            int auditResult = 0;
            if (result == "true")
            {
                auditResult = 1;
            }
            else
            {
                auditResult = -3;
            }
            using (JDJS_WMS_DB_USEREntities entities = new JDJS_WMS_DB_USEREntities())
            {
                using (System.Data.Entity.DbContextTransaction mytran = entities.Database.BeginTransaction())
                {
                    try
                    {
                        var order = entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == orderId).FirstOrDefault();

                        if (type == "工艺审核")
                        {

                            if (order != null)
                            {
                                orderNumber = order.Order_Number;
                                for (int i = 0; i < files.Count; i++)
                                {
                                    var item = new FileInfo(files[i].FileName);
                                    var pathblank = Path.Combine(pathInfo.upLoadPath(), orderNumber, "审核结果", item.Name);
                                    DirectoryInfo directoryBlank = new DirectoryInfo(Path.Combine(pathInfo.upLoadPath(), orderNumber, "审核结果"));
                                    if (!directoryBlank.Exists)
                                    {
                                        directoryBlank.Create();
                                    }
                                    files[i].SaveAs(pathblank);
                                }
                                string resu = "";
                                if (order.audit_Result != null)
                                {
                                    resu = order.audit_Result;
                                }
                                if (text != null && text != "")
                                {
                                    order.audit_Result = resu + "。" + DateTime.Now.ToString() + ":" + text;
                                }
                                var processes = entities.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == orderId && r.sign == -2);
                                if (processes.Count() < 1)
                                {
                                    context.Response.Write("该订单暂无待审核工序");
                                    return;
                                }
                                foreach (var item in entities.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == orderId))
                                {
                                    item.sign = auditResult;
                                    if (auditResult == 1)
                                    {
                                        string str = item.BlankSpecification;
                                        if (str.Contains("#1#"))
                                        {
                                            str = str.Substring(0, str.IndexOf("#1#"));
                                            item.BlankSpecification = str;
                                        }
                                        str = item.JigSpecification;
                                        if (str.Contains("#1#"))
                                        {
                                            str = str.Substring(0, str.IndexOf("#1#"));
                                            item.JigSpecification = str;
                                            //判断是否为特殊治具，然后进行治具需求创建
                                            var jiaType = item.JigType;
                                            var jiaStr = entities.JDJS_WMS_Device_Status_Table.Where(r => r.ID == jiaType).FirstOrDefault().Status;
                                            if (jiaStr == "其它")
                                            {
                                                string fxnum = "FX" + DateTime.Now.Year.ToString().Substring(2);
                                                string month = DateTime.Now.Month.ToString();
                                                while (month.Length < 2)
                                                {
                                                    month = month.Insert(0, "0");
                                                }
                                                string day = DateTime.Now.Day.ToString();
                                                while (day.Length < 2)
                                                {
                                                    day = day.Insert(0, "0");
                                                }
                                                fxnum += (month + day);
                                                int number = 1;
                                                string numStr = number.ToString();
                                                while (numStr.Length < 3)
                                                {
                                                    numStr = numStr.Insert(0, "0");
                                                }
                                                var isexist = entities.JDJS_WMS_Fixture_Manage_Demand_Table.Where(r => r.FixtureOrderNum == (fxnum + numStr)).FirstOrDefault();
                                                while (isexist != null)
                                                {
                                                    number++;
                                                    numStr = number.ToString();
                                                    while (numStr.Length < 3)
                                                    {
                                                        numStr = numStr.Insert(0, "0");
                                                    }
                                                    isexist = entities.JDJS_WMS_Fixture_Manage_Demand_Table.Where(r => r.FixtureOrderNum == (fxnum + numStr)).FirstOrDefault();
                                                }
                                                fxnum += numStr;
                                                var oldDemand = entities.JDJS_WMS_Fixture_Manage_Demand_Table.Where(r => r.OrderId == order.Order_ID && r.ProcessId == item.ID).FirstOrDefault();
                                                if (oldDemand == null)
                                                {
                                                    JDJS_WMS_Fixture_Manage_Demand_Table jdDemand = new JDJS_WMS_Fixture_Manage_Demand_Table()
                                                    {
                                                        OrderId = order.Order_ID,
                                                        State = "新建",
                                                        FixtureSpecification = str,
                                                        DemandTime = DateTime.Now,
                                                        FixtureOrderNum = fxnum,
                                                        OrderNum = order.Order_Number,
                                                        ProcessId = item.ID,
                                                        ProcessNum = item.ProcessID,
                                                        ProgramPersonName = order.Engine_Program_Manager,
                                                    };
                                                    entities.JDJS_WMS_Fixture_Manage_Demand_Table.Add(jdDemand);
                                                    entities.SaveChanges();
                                                }
                                            }
                                        }

                                    }
                                }
                                if (auditResult == 1)
                                {
                                    var blankInfo = entities.JDJS_WMS_Order_Blank_Table.Where(r => r.OrderID == orderId);
                                    foreach (var item in blankInfo)
                                    {
                                        string str = item.BlankSpecification;
                                        if (str.Contains("#1#"))
                                        {
                                            str = str.Substring(0, str.IndexOf("#1#"));
                                            item.BlankSpecification = str;
                                        }

                                    }
                                }
                                entities.SaveChanges();
                            }
                            else
                            {
                                context.Response.Write("该订单不存在");
                                return;
                            }
                        }
                        else if (type == "编程审核")
                        {
                            if (order != null)
                            {
                                orderNumber = order.Order_Number;
                                for (int i = 0; i < files.Count; i++)
                                {
                                    var item = new FileInfo(files[i].FileName);
                                    var pathblank = Path.Combine(pathInfo.upLoadPath(), orderNumber, "编程审核结果", item.Name);
                                    DirectoryInfo directoryBlank = new DirectoryInfo(Path.Combine(pathInfo.upLoadPath(), orderNumber, "编程审核结果"));
                                    if (!directoryBlank.Exists)
                                    {
                                        directoryBlank.Create();
                                    }
                                    files[i].SaveAs(pathblank);
                                }
                                string resu = "";
                                if (order.program_audit_result != null)
                                {
                                    resu = order.program_audit_result;
                                }
                                if (text != null && text != "")
                                {
                                    order.program_audit_result = resu + "。" + DateTime.Now.ToString() + ":" + text;
                                }
                                var processes = entities.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == orderId && r.program_audit_sign == -2 & r.sign == 1);
                                if (processes.Count() < 1)
                                {
                                    context.Response.Write("该订单暂无待审核工序");
                                    return;
                                }
                                foreach (var item in entities.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == orderId & r.sign == 1))
                                {
                                    item.program_audit_sign = auditResult;
                                    item.ProgramePassTime = DateTime.Now;
                                    //if (auditResult == 1)
                                    //{
                                    //    item.BlankSpecification = item.BlankSpecification.Replace("#1#", "");
                                    //    item.JigSpecification = item.JigSpecification.Replace("#1#", "");
                                    //}
                                }
                                //if (auditResult == 1)
                                //{
                                //    var blankInfo = entities.JDJS_WMS_Order_Blank_Table.Where(r => r.OrderID == orderId);
                                //    foreach (var item in blankInfo)
                                //    {
                                //        item.BlankSpecification = item.BlankSpecification.Replace("#1#", "");
                                //    }
                                //}
                                entities.SaveChanges();
                            }
                            else
                            {
                                context.Response.Write("该订单不存在");
                                return;
                            }
                        }

                        entities.SaveChanges();
                        mytran.Commit();
                        context.Response.Write("ok");
                    }
                    catch (Exception ex)
                    {
                        mytran.Rollback();
                        context.Response.Write(ex.Message);
                    }
                }
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
}