using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.品质
{
    /// <summary>
    /// 良品数总数 的摘要说明
    /// </summary>
    public class 良品数总数 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            int orderID = int.Parse(context.Request["id"]);
            int GoodNum = 0;
            int AllNum = 0;
            List<int> AllblankIDs = new List<int>();
            List<int> AllGoodblankIDs = new List<int>();
            List<int> AllPoorblankIDs = new List<int>();
            using (JDJS_WMS_DB_USEREntities  wms = new JDJS_WMS_DB_USEREntities ())
            {
                var info = wms.JDJS_WMS_Quality_Onmachine_Measurement_Data;
                foreach (var item in info)
                {

                    int blankID = Convert.ToInt32(item.blankID);
                    var blank = wms.JDJS_WMS_Blank_Table.Where(r => r.ID == blankID).FirstOrDefault();
                    if (blank != null)
                    {
                        if (blank.OrderID == orderID)
                        {

                            if (!AllblankIDs.Contains(blankID))
                            {
                                AllblankIDs.Add(blankID);
                            }
                            var measure = item.Measurements;
                            var Max = item.StandardValue + item.ToleranceRangeMax;
                            var Min = item.StandardValue + item.ToleranceRangeMin;
                            if (measure >= Min && measure <= Max)
                            {//好的
                                if ((!AllGoodblankIDs.Contains(blankID)) && (!AllPoorblankIDs.Contains(blankID)))
                                {
                                    AllGoodblankIDs.Add(blankID);
                                }
                            }
                            else
                            {//坏的
                                if (AllGoodblankIDs.Contains(blankID))
                                {
                                    AllGoodblankIDs.Remove(blankID);
                                }
                                if (!AllPoorblankIDs.Contains(blankID))
                                {
                                    AllPoorblankIDs.Add(blankID);
                                }
                            }
                        }
                    }
                }
            }
            AllNum = AllblankIDs.Count();
            GoodNum = AllGoodblankIDs.Count();
            var model = new { AllNum = AllNum, GoodNum = GoodNum };
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            context.Response.Write(serializer.Serialize(model));

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