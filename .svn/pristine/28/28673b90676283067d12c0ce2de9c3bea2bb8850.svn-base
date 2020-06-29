using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Kanban.工程中心Method
{
    /// <summary>
    /// TOP评估量 的摘要说明
    /// </summary>
    public class TOP评估量 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                List<TopAssessNum> topAssessNums = new List<TopAssessNum>();
                DateTime time = DateTime .Now.AddDays (-7);
                using (JDJS_WMS_DB_USEREntities wms = new JDJS_WMS_DB_USEREntities())
                {
                    var orders = wms.JDJS_WMS_Order_Intention_History_Table.Where(r => r.SubmitTime > time);
                    foreach (var item in orders)
                    {
                        var staff  =wms.JDJS_WMS_Staff_Info .Where (r=>r.id ==item.CraftPersonID ).FirstOrDefault ();
                        if(staff!=null)
                        {
                            string name = staff.staff;
                            var assess = topAssessNums.Where(r => r.name == name).FirstOrDefault ();
                            if (assess!=null)
                            {
                                assess.num = assess.num + 1;
                            }
                            else
                            {
                                TopAssessNum top = new TopAssessNum();
                                top.name = name;
                                top.num = 1;
                                topAssessNums.Add(top);
                            }
                        }
                    }
                    for (int i = 0; i < topAssessNums.Count () ; i++)
                    {
                        topAssessNums[i].numStr = topAssessNums[i].num.ToString();
                    }
                    topAssessNums = topAssessNums.OrderByDescending(r => r.num).ToList ();
                    while (topAssessNums.Count() < 5)
                    {
                        topAssessNums .Add (new TopAssessNum (){name ="",num =0,numStr =""});

                    }
                    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var json = serializer.Serialize(topAssessNums);
                    context.Response.Write("data:"+json+"\n\n");
                    context.Response.ContentType = "text/event-stream";
                }
            }
            catch (Exception ex)
            {
                context.Response.Write(ex.Message);
            }
            //context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }

    public class TopAssessNum
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string name;
        /// <summary>
        /// 用于排序的，不用显示
        /// </summary>
        public int num;
        /// <summary>
        /// 显示的数量
        /// </summary>
        public string numStr;
    }
}