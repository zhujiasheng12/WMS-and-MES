using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.生产部
{
    /// <summary>
    /// 读工序数量 的摘要说明
    /// </summary>
    public class 读工序数量 : IHttpHandler
    {
        string 好的="";
        

        public void ProcessRequest(HttpContext context)
        {
            好的 = "1";

            {
                List<string> ProcessTable = new List<string>();
                List<int> id = new List<int>();
                int max = -1;
                using (JDJS_WMS_DB_USEREntities wms = new JDJS_WMS_DB_USEREntities ())
                {
                    var processs = wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.sign != 0);
                    foreach (var item in processs)
                    {
                        if (!id.Contains(Convert.ToInt32(item.ProcessID)))
                        {
                            id.Add(Convert.ToInt32(item.ProcessID));
                        }
                    }
                    max = id.Max();

                }
                for (int i = 1; i <= max; i++)
                {
                    ProcessTable.Add("工序" + i.ToString());
                }
                for (int i = 0; i < ProcessTable.Count(); i++)
                {
                    ProcessTable[i] = ProcessTable[i].Replace('1', '一').Replace('2', '二').Replace('3', '三').Replace('4', '四').Replace('5', '五').Replace('6', '六').Replace('7', '七').Replace('8', '八').Replace('9', '九').Replace('0', '零');
                }

                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                var json = serializer.Serialize(ProcessTable);
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
}