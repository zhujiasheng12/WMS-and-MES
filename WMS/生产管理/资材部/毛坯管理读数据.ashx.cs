using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.资材部
{
    /// <summary>
    /// 毛坯管理读数据 的摘要说明
    /// </summary>
    public class 毛坯管理读数据 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var page = int.Parse(context.Request["page"]);
            var limit = int.Parse(context.Request["limit"]);
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            {
                List<BlankRead> blankReads = new List<BlankRead>();
                using (JDJS_WMS_DB_USEREntities wms = new JDJS_WMS_DB_USEREntities())
                {
                    var orders = wms.JDJS_WMS_Order_Entry_Table.Where(r =>  r.Intention != 1&& r.Intention != 4 && r.Intention != 5 && r.Intention != 6);
                    foreach (var order in orders)
                    {
                        BlankRead blankRead = new BlankRead();
                        blankRead.projectName = order.ProjectName == null ? "" : order.ProjectName;
                        blankRead.orderName = order.Product_Name;
                        blankRead.orderId = order.Order_ID.ToString();
                        blankRead.orderId1 = order.Order_ID.ToString();
                        blankRead.order = order.Order_Number;
                        var times = wms.JDJS_WMS_Order_DelayTime_Table.Where(r => r.OrderID == order.Order_ID);
                        if (times.Count() > 0)
                        {
                            blankRead.time = times.FirstOrDefault().BlankTime.ToString();
                        }
                        var blankInfo = 
                                        from blank in wms.JDJS_WMS_Order_Blank_Table
                                        where   blank.OrderID == order.Order_ID
                                        select new
                                        {
                                            blank.BlankType,
                                            blank.BlankSpecification,
                                            blank.BlackNumber,
                                            blank.BlankState,
                                            blank.BlanktotalPreparedNumber,
                                            blank.BlankAddition,
                                            blank.Expected_Completion_Time

                                        };
                        if (blankInfo.Count() > 0)
                        {

                            switch (Convert.ToInt32(blankInfo.First().BlankType))
                            {
                                case 1:
                                    blankRead.blankType = "板料";
                                    break;
                                case 2:
                                    blankRead.blankType = "块料";
                                    break;
                                default:
                                    blankRead.blankType = "未知";
                                    break;
                            }
                            blankRead.blankSpecification = blankInfo.First().BlankSpecification;
                            blankRead.blankDemandNumber = blankInfo.First().BlackNumber.ToString();
                            blankRead.Expected_Completion_Time = blankInfo.First().Expected_Completion_Time.ToString();
                            int num = 0;
                            var overProcess = wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == order.Order_ID && r.ProcessID == 1 && r.sign == 1);
                            if (overProcess.Count() > 0)
                            {
                                var over = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.ProcessID == overProcess.FirstOrDefault().ID && (r.isFlag == 2 || r.isFlag == 3));
                                num = over.Count();

                            }
                            blankRead.orderUsedNumber = num.ToString();
                            int OverNum = 0;
                            List<int> processID = new List<int>();
                            var orderprocess = wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == order.Order_ID && r.sign != 0);
                            if (orderprocess.Count() > 0)
                            {
                                foreach (var item in orderprocess)
                                {
                                    int id = Convert.ToInt32(item.ProcessID);
                                    if (!processID.Contains(id))
                                    {
                                        processID.Add(id);
                                    }
                                }
                                int max = processID.Max();
                                var MaxProcess = wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == order.Order_ID && r.sign != 0 && r.ProcessID == max).First();
                                var over = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.ProcessID == MaxProcess.ID && r.isFlag == 3);
                                OverNum = over.Count();
                            }
                            blankRead.orderFinishedProductNumber = OverNum.ToString();
                            int PefectiveProductNumber = 0;
                            double passRate = 0;
                            var pp = wms.JDJS_WMS_Quality_Confirmation_Table.Where(r => r.OrderID == order.Order_ID);
                            if (pp.Count() > 0)
                            {
                                PefectiveProductNumber = Convert.ToInt32(pp.First().PefectiveProductNumber);
                                passRate = Convert.ToDouble(pp.First().PassRate);
                            }
                            blankRead.orderWasteNumber = PefectiveProductNumber.ToString();
                            blankRead.orderGoodProductRate = (passRate*100).ToString()+"%";
                            blankRead.blankSparePartsState = blankInfo.First().BlankState;
                            blankRead.blankPreparedNumber = blankInfo.First().BlanktotalPreparedNumber.ToString();
                            blankRead.blankBolusNumber = blankInfo.First().BlankAddition.ToString();


                        }
                        if (blankRead.blankSpecification !=null&& !blankRead.blankSpecification.Contains("#1#"))
                        {
                            blankReads.Add(blankRead);
                        }


                    }

                   
                }
                var layPage = blankReads.Skip((page - 1) * limit).Take(limit);
                var model = new { code = 0, data = layPage,count=blankReads.Count };
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
    class BlankRead
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string order;
        /// <summary>
        /// 项目名称
        /// </summary>
        public string projectName;
        /// <summary>
        /// 产品名称
        /// </summary>
        public string orderName;
        /// <summary>
        /// 毛坯种类
        /// </summary>
        public string blankType;
        /// <summary>
        /// 毛坯规格
        /// </summary>
        public string blankSpecification;
        /// <summary>
        /// 毛坯需求数量
        /// </summary>
        public string blankDemandNumber;
        /// <summary>
        /// 订单已使用量
        /// </summary>
        public string orderUsedNumber;
        /// <summary>
        /// 订单成品量
        /// </summary>
        public string orderFinishedProductNumber;
        /// <summary>
        /// 订单废品量
        /// </summary>
        public string orderWasteNumber;
        /// <summary>
        /// 订单良品率
        /// </summary>
        public string orderGoodProductRate;
        /// <summary>
        /// 毛坯备料状态
        /// </summary>
        public string blankSparePartsState;
        /// <summary>
        /// 毛坯已准备总量
        /// </summary>
        public string blankPreparedNumber;
        /// <summary>
        /// 毛坯追加量
        /// </summary>
        public string blankBolusNumber;

        /// <summary>
        /// 订单ID（主键）
        /// </summary>
        public string orderId;
        public string orderId1;
        public string time;//延期日期
        public string Expected_Completion_Time;//预计完成日期

    }
}