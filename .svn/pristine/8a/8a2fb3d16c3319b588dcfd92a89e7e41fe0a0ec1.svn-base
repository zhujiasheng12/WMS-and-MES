using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Kanban.资材
{
    /// <summary>
    /// 物料概况 的摘要说明
    /// </summary>
    public class 物料概况 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //物料概况
            MaterialOverview materialOverview = new MaterialOverview();
            List<string> blankTypes = new List<string>();
            List<string> jiaTypes = new List<string>();
            using (JDJS_WMS_DB_USEREntities  wms = new  JDJS_WMS_DB_USEREntities ())
            {
                var orders = wms.JDJS_WMS_Order_Entry_Table.Where(r => r.Intention == 2).ToList();
                foreach (var item in orders)
                {
                    int orderID = item.Order_ID;
                    var processInfos = wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == orderID && r.ProcessID == 1 && r.sign != 0).FirstOrDefault();
                    if (processInfos != null)
                    {
                        int blankNum = Convert.ToInt32(processInfos.BlankNumber);
                        materialOverview.blankRequairNum += blankNum;
                        string blankType = processInfos.BlankSpecification;

                        if (!blankTypes.Contains(blankType))
                        {
                            blankTypes.Add(blankType);
                            materialOverview.blankFormatNum++;
                        }
                        var blankInfo = wms.JDJS_WMS_Order_Blank_Table.Where(r => r.OrderID == orderID).FirstOrDefault();
                        if (blankInfo != null)
                        {
                            int blankPre = Convert.ToInt32(blankInfo.BlanktotalPreparedNumber);
                            materialOverview.blankAlready += blankPre;
                            materialOverview.blankPending += (blankNum - blankPre);
                        }

                    }

                    var processInfospro = wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == orderID && r.sign != 0);
                    foreach (var real in processInfospro)
                    {
                        int processID = real.ID;
                        string jiaType = real.JigSpecification;
                        if (jiaType != null)
                        {
                            if (jiaTypes.Contains(jiaType))
                            {
                                jiaTypes.Add(jiaType);
                                materialOverview.specialFixtureFormatNum++;
                            }
                            var fixtureInfo = wms.JDJS_WMS_Order_Fixture_Manager_Table.Where(r => r.ProcessID == processID).FirstOrDefault();
                            if (fixtureInfo != null)
                            {
                                int fixAllNum = Convert.ToInt32(fixtureInfo.FixtureNumber);
                                materialOverview.specialFixtureRequairNum += fixAllNum;
                                int fixpre = Convert.ToInt32(fixtureInfo.FixtureFinishPerpareNumber);
                                materialOverview.specialFixtureAlready += fixpre;
                                materialOverview.specialFixturePending += (fixAllNum - fixpre);
                            }

                        }
                    }

                }
            }
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var json = serializer.Serialize(materialOverview);
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
    public class MaterialOverview
    {
        /// <summary>
        /// 毛坯请求总数
        /// </summary>
        public int blankRequairNum;
        /// <summary>
        /// 毛坯规格总数
        /// </summary>
        public int blankFormatNum;
        /// <summary>
        /// 毛坯已准备数
        /// </summary>
        public int blankAlready;
        /// <summary>
        /// 毛坯待准备数
        /// </summary>
        public int blankPending;
        /// <summary>
        /// 特殊治具请求总数
        /// </summary>
        public int specialFixtureRequairNum;
        /// <summary>
        /// 特殊治具规格数
        /// </summary>
        public int specialFixtureFormatNum;
        /// <summary>
        /// 特殊治具已准备数
        /// </summary>
        public int specialFixtureAlready;
        /// <summary>
        /// 特殊治具待准备数
        /// </summary>
        public int specialFixturePending;
    }
}