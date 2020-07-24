using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.Model.生产管理.工程部;
using WebApplication2.生产管理.生产部.订单跟踪.Model;

namespace WebApplication2.生产管理.生产部.订单跟踪
{
    /// <summary>
    /// 生产进度 的摘要说明
    /// </summary>
    public class 生产进度 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var orderId = int.Parse(context.Request.Form["orderId"]);
            Order_Trace_Work_Sche info = new Order_Trace_Work_Sche();
            using (JDJS_WMS_DB_USEREntities wms = new JDJS_WMS_DB_USEREntities())
            {
                info.Name = "生产进度";
                int allBlankNum = 0;
                var blank = wms.JDJS_WMS_Order_Blank_Table.Where(r => r.OrderID == orderId).FirstOrDefault();
                if (blank != null)
                {
                     allBlankNum = blank.BlanktotalPreparedNumber == null ? 0 : Convert.ToInt32(blank.BlanktotalPreparedNumber);
                }
                #region 已加工数
                int OverNum = 0;
                List<int> processID = new List<int>();
                var orderprocess = wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == orderId && r.sign != 0);
                if (orderprocess.Count() > 0)
                {
                    foreach (var item in orderprocess)
                    {
                        allBlankNum = allBlankNum *Convert.ToInt32 ( item.Modulus);
                        int id = Convert.ToInt32(item.ProcessID);
                        if (!processID.Contains(id))
                        {
                            processID.Add(id);
                        }
                    }
                    int max = processID.Max();
                    var MaxProcess = wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == orderId && r.sign != 0 && r.ProcessID == max).First();
                    var over = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.ProcessID == MaxProcess.ID && r.isFlag == 3);
                    var doing = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.ProcessID == MaxProcess.ID && r.isFlag == 2);
                    var not = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.ProcessID == MaxProcess.ID && r.isFlag == 1);
                    int doCount = 0;
                    foreach (var item in over)
                    {
                        doCount += (item.WorkCount == null ? 0 : Convert.ToInt32(item.WorkCount));

                    }
                    foreach (var item in doing)
                    {
                        doCount += (item.WorkCount == null ? 0 : Convert.ToInt32(item.WorkCount));

                    }
                    foreach (var item in not)
                    {
                        doCount += (item.WorkCount == null ? 0 : Convert.ToInt32(item.WorkCount));

                    }


                    OverNum = doCount;
                }
                info.Finish = OverNum;
                #endregion
                #region 待加工数
                info.waitNum = allBlankNum - info.Finish;
                if (info.waitNum < 0)
                {
                    info.waitNum = 0;
                }
                #endregion
                #region 良品数
                var confirm = wms.JDJS_WMS_Quality_Confirmation_Table.Where(r => r.OrderID == orderId);
                if (confirm.Count() > 0)
                {

                    if (OverNum < 1)
                    {
                        info.Good = 0;
                    }
                    else
                    {
                        info.Good = confirm.First().QualifiedProductNumber==null?0:Convert.ToInt32 ( confirm.First().QualifiedProductNumber);
                    }

                }
                else
                {
                    info.Good = 0;
                }
                #endregion
                #region 入库数
                var product = wms.JDJS_WMS_Finished_Product_Manager.Where(r => r.OrderID == orderId).FirstOrDefault ();
                if (product != null)
                {
                    info.Storage =  product.warehousingNumber==null?0:Convert.ToInt32 (product.warehousingNumber);
                }
                #endregion 
            }
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var json = serializer.Serialize(info);
            context.Response.Write(json);
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