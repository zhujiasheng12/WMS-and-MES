using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.工程部
{
    /// <summary>
    /// 审核结果弹窗读数据 的摘要说明
    /// </summary>
    public class 审核结果弹窗读数据 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var orderId =Convert.ToInt32 ( context.Request.QueryString["orderId"]);
            var type = context.Request["type"];
            Result result = new Result();
            using(JDJS_WMS_DB_USEREntities entities=new JDJS_WMS_DB_USEREntities())
            {
                var order = entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == orderId).FirstOrDefault ();
                if (type == "工艺审核") {
                    if (order != null)
                    {
                        result.OrderNum = order.Order_Number;
                        if (order.audit_Result != null)
                        {
                            result.Audit_Result = order.audit_Result;
                        }
                        var processes = entities.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == orderId && r.sign != 0).FirstOrDefault();
                        if (processes != null)
                        {
                            int re = Convert.ToInt32(processes.sign);
                            switch (re)
                            {
                                case 0:
                                    result.State = "意向订单";
                                    break;
                                case 1:
                                    result.State = "审核已通过";
                                    break;
                                case -1:
                                    result.State = "工艺规划中";
                                    break;
                                case -2:
                                    result.State = "提交审核中";
                                    break;
                                case -3:
                                    result.State = "审核未通过，请尽快确认";
                                    break;
                                default:
                                    result.State = "";
                                    break;
                            }
                        }
                        PathInfo pathInfo = new PathInfo();
                        var path = Path.Combine(pathInfo.upLoadPath(), order.Order_Number, "审核结果");
                        DirectoryInfo directory = new DirectoryInfo(path);
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        FileInfo[] files = directory.GetFiles();
                        List<FileRead> fileReads = new List<FileRead>();
                        foreach (var item in files)
                        {
                            fileReads.Add(new FileRead { fileName = item.Name, fileSize = item.Length.ToString(), filePath = item.FullName, State = result.State, Audit_Result = result.Audit_Result, OrderNum = result.OrderNum });
                        }
                        if (fileReads.Count == 0)
                        {
                            fileReads.Add(new FileRead { State = result.State, Audit_Result = result.Audit_Result, OrderNum = result.OrderNum });
                        }
                        var model = new { code = 0, data = fileReads };

                        System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var json = serializer.Serialize(model);
                        context.Response.Write(json);


                    }
                    else
                    {
                        context.Response.Write("该订单不存在");
                        return;
                    }
                } else if (type == "编程审核")
                {
                    if (order != null)
                    {
                        result.OrderNum = order.Order_Number;
                        if (order.audit_Result != null)
                        {
                            result.Audit_Result = order.program_audit_result;
                        }
                        var processes = entities.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == orderId && r.sign != 0).FirstOrDefault();
                        if (processes != null)
                        {
                            int re = Convert.ToInt32(processes.program_audit_sign);
                            switch (re)
                            {
                                case 0:
                                    result.State = "";
                                    break;
                                case 1:
                                    result.State = "审核已通过";
                                    break;
                                case -1:
                                    result.State = "工程编程中";
                                    break;
                                case -2:
                                    result.State = "提交审核中";
                                    break;
                                case -3:
                                    result.State = "审核未通过，请尽快确认";
                                    break;
                                default:
                                    result.State = "";
                                    break;
                            }
                        }
                        PathInfo pathInfo = new PathInfo();
                        var path = Path.Combine(pathInfo.upLoadPath(), order.Order_Number, "编程审核结果");
                        DirectoryInfo directory = new DirectoryInfo(path);
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        FileInfo[] files = directory.GetFiles();
                        List<FileRead> fileReads = new List<FileRead>();
                        foreach (var item in files)
                        {
                            fileReads.Add(new FileRead { fileName = item.Name, fileSize = item.Length.ToString(), filePath = item.FullName, State = result.State, Audit_Result = result.Audit_Result, OrderNum = result.OrderNum });
                        }
                        if (fileReads.Count == 0)
                        {
                            fileReads.Add(new FileRead { State = result.State, Audit_Result = result.Audit_Result, OrderNum = result.OrderNum });
                        }
                        var model = new { code = 0, data = fileReads };

                        System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var json = serializer.Serialize(model);
                        context.Response.Write(json);


                    }
                    else
                    {
                        context.Response.Write("该订单不存在");
                        return;
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
    class Result
    {
        public string OrderNum;
        public string State;
        public string Audit_Result;
    }
    class FileRead
    {
         public string  fileName;
        public string fileSize;
        public string filePath;
        public string OrderNum;
        public string State;
        public string Audit_Result;
    }
}