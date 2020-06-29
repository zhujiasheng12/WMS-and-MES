using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.品质
{
    /// <summary>
    /// 读订单工件 的摘要说明
    /// </summary>
    public class 读订单工件 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //读取订单工件
            List<test> tests = new List<test>();
            using (JDJS_WMS_DB_USEREntities  wms = new JDJS_WMS_DB_USEREntities ())
            {
                var orders = wms.JDJS_WMS_Order_Entry_Table.Where(r => r.Intention == 2);
                foreach (var item in orders)
                {
                    test test = new test();
                    test.children = new List<test>();
                    test.id = item.Order_ID.ToString();
                    test.name = item.Order_Number;
                   
                    var blanks = wms.JDJS_WMS_Blank_Table.Where(r => r.OrderID == item.Order_ID).ToList();
                    if (blanks.Count > 0)
                    {
                        test.state = "closed";
                    }
                    for (int i = 0; i < blanks.Count(); i++)
                    {
                        test test1 = new test();
                        test1.id = blanks[i].ID.ToString();
                        test1.name = "工件" + (i + 1).ToString();
                        test.children.Add(test1);
                    }
                    tests.Add(test);
                }
            }
            System.Web.Script.Serialization.JavaScriptSerializer serlizer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var json = serlizer.Serialize(tests);
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
    class test
    {
        public string id;
        public string state;
        public string name;
      public   List<test> children;
    }

}