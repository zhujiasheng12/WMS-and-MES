using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Kanban.资材
{
    /// <summary>
    /// 特殊治具请求 的摘要说明
    /// </summary>
    public class 特殊治具请求 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //特殊治具请求
            List<FixtureRequair> fixtureRequairs = new List<FixtureRequair>();
            using (JDJS_WMS_DB_USEREntities  wms = new  JDJS_WMS_DB_USEREntities ())
            {
                var orders = wms.JDJS_WMS_Order_Entry_Table.Where(r => r.Intention == 2);
                foreach (var item in orders)
                {
                    int orderID = item.Order_ID;
                    string orderNum = item.Order_Number;
                    var processes = wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == orderID && r.sign != 0);
                    foreach (var real in processes)
                    {
                        int processID = real.ID;

                        string fixSpecification = real.JigSpecification;
                        if (fixSpecification != null)
                        {
                            FixtureRequair fixtureRequair = new FixtureRequair();
                            fixtureRequair.OrderNum = orderNum;
                            fixtureRequair.ProcessNum = real.ProcessID.ToString();
                            fixtureRequair.FixtureSpecification = fixSpecification;
                            DateTime StartTime = Convert.ToDateTime(item.Order_Actual_Start_Time);
                            fixtureRequair.FixtureStartTime = StartTime.ToString();
                            var fixtureInfo = wms.JDJS_WMS_Order_Fixture_Manager_Table.Where(r => r.ProcessID == processID).FirstOrDefault();
                            if (fixtureInfo != null)
                            {
                                int RequairNum = Convert.ToInt32(fixtureInfo.FixtureNumber);
                                int Already = Convert.ToInt32(fixtureInfo.FixtureFinishPerpareNumber);
                                int pending = RequairNum - Already;
                                fixtureRequair.FixtureRequairNum = RequairNum.ToString();
                                fixtureRequair.FixtureAlreadyNum = Already.ToString();
                                fixtureRequair.FixturePendingNum = pending.ToString();
                                if (pending > 0)
                                {
                                    fixtureRequair.FixtureState = "待备料";
                                    fixtureRequair.FixtureEndTime = "-";
                                }
                                else
                                {
                                    fixtureRequair.FixtureState = "已完成";
                                    var history = wms.JDJS_WMS_Fixture_Additional_History_Table.Where(r => r.ProcessID == processID).OrderBy(r => r.AddTime).ToList();
                                    if (history.Count > 0)
                                    {
                                        fixtureRequair.FixtureEndTime = history.Last().AddTime.ToString();
                                    }
                                    

                                }

                            }
                        }
                    }
                }
            }

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var model = new { code = 0, data = fixtureRequairs };
            var json = serializer.Serialize(model);
            context.Response.Write("data:"+json+"\n\n");
            context.Response.ContentType = "text/event-stream";


        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
    public class FixtureRequair
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNum;
        /// <summary>
        /// 工序号
        /// </summary>
        public string ProcessNum;
        /// <summary>
        /// 特殊治具规格
        /// </summary>
        public string FixtureSpecification;
        /// <summary>
        /// 特殊治具数量
        /// </summary>
        public string FixtureRequairNum;
        /// <summary>
        /// 特殊治具已准备数
        /// </summary>
        public string FixtureAlreadyNum;
        /// <summary>
        /// 特殊治具待准备数
        /// </summary>
        public string FixturePendingNum;
        /// <summary>
        /// 特殊治具准备状态
        /// </summary>
        public string FixtureState;
        /// <summary>
        /// 需求下发时间
        /// </summary>
        public string FixtureStartTime;
        /// <summary>
        /// 需求完成时间
        /// </summary>
        public string FixtureEndTime;
    }
}