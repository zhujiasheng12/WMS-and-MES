using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.品质
{
    /// <summary>
    /// qualityConfirmationRead 的摘要说明
    /// </summary>
    public class qualityConfirmationRead : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
           using(JDJS_WMS_DB_USEREntities entities=new JDJS_WMS_DB_USEREntities())
            {
                var rows = from qua in entities.JDJS_WMS_Quality_Confirmation_Table
                          
                           select new
                           {
                               //qua.CurrFinishedProductNumber,
                               qua.DetectionNumber,
                               qua.ID,
                               qua.OrderID,
                               qua.PassRate,
                               qua.PefectiveProductNumber,
                               //qua.PendingNumber,
                               qua.QualifiedProductNumber,

                           };
                List<QualityConfirm> qualityConfirms = new List<QualityConfirm>();

                foreach (var item in rows)
                {
                    var order = entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == item.OrderID).First().Order_Number;
                    var orderName = entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == item.OrderID).First().Product_Name ;
                    var projectName = entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == item.OrderID).First().ProjectName ;
                    var orderNumber = entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == item.OrderID).First().Product_Output;
                    var Process = entities.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == item.OrderID);
                    var lastProcess = Process.OrderByDescending(r => r.ProcessID).First().ProcessID;
                    var lastProcessId = entities.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == item.OrderID & r.ProcessID == lastProcess).First().ID;
                    var modulus = entities.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == item.OrderID & r.ProcessID == lastProcess).First().Modulus;
                    var works = entities.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.ProcessID == lastProcessId);
                    int count = 0;
                    foreach (var work in works)
                    {
                        count += (work.WorkCount == null ? 0 : Convert.ToInt32(work.WorkCount));
                    }
                    var CurrFinishedProductNumber = entities.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.ProcessID == lastProcessId&r.isFlag==3).Count()*modulus;
                    if (item.DetectionNumber != 0)
                    {
                        qualityConfirms.Add(new QualityConfirm
                        {
                            id = item.ID.ToString(),
                            order = order,
                            orderNumber = orderNumber.ToString(),
                            //CurrFinishedProductNumber = CurrFinishedProductNumber.ToString(),
                            CurrFinishedProductNumber=count .ToString (),
                            DetectionNumber = item.DetectionNumber.ToString(),
                            PendingNumber = (count - item.DetectionNumber).ToString(),
                            PefectiveProductNumber = item.PefectiveProductNumber.ToString(),
                            QualifiedProductNumber = item.QualifiedProductNumber.ToString(),


                            PassRate = (Math.Round(Convert.ToDouble(item.QualifiedProductNumber) / Convert.ToDouble(item.DetectionNumber), 4) * 100).ToString() + "%"
                        }); ;
                    }
                    else
                    {
                        qualityConfirms.Add(new QualityConfirm
                        {
                            id = item.ID.ToString(),
                            order = order,
                            orderNumber = orderNumber.ToString(),
                            //CurrFinishedProductNumber = CurrFinishedProductNumber.ToString(),
                            CurrFinishedProductNumber=count .ToString (),
                            orderName =orderName ,
                            projectName =projectName ,
                            DetectionNumber = item.DetectionNumber.ToString(),
                            PendingNumber = (count - item.DetectionNumber).ToString(),
                            PefectiveProductNumber = item.PefectiveProductNumber.ToString(),
                            QualifiedProductNumber = item.QualifiedProductNumber.ToString(),


                            PassRate = "0" + "%"
                        }); ;
                    }
                    
                }
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                var page = int.Parse(context.Request["page"]);
                var limit = int.Parse(context.Request["limit"]);
                var layPage = qualityConfirms.Skip((page - 1) * limit).Take(limit);
                var model = new { code = 0, msg = "", count = qualityConfirms.Count, data = layPage };
                var json = serializer.Serialize(model);
                context.Response.Write(json);
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
    class QualityConfirm
    {
        public string order;
        public string orderNumber;
        public string orderName;
        public string projectName;
        public string CurrFinishedProductNumber;
        public string DetectionNumber;
        public string PendingNumber;
        public string QualifiedProductNumber;
        public string PefectiveProductNumber;
        public string PassRate;
        public string id;
            
    }
}