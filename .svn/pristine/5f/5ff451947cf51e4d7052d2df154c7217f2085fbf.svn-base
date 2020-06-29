using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApplication2.Model.生产管理.工程部
{
    /// <summary>
    /// processDel 的摘要说明
    /// </summary>
    public class processDel : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var id = int.Parse(context.Request["id"]);
            using (JDJS_WMS_DB_USEREntities entities=new JDJS_WMS_DB_USEREntities())
            {
                var row = entities.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == id).First();

                var orderId = row.OrderID;
                var process = row.ProcessID;
                var count = entities.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == orderId&r.sign!=0).Count();
                if (count != process)
                {
                    context.Response.Write("请从最后一道工序开始删除");
                    return;

                }

                var orderNumber = entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == row.OrderID).First().Order_Number;
                entities.JDJS_WMS_Order_Process_Info_Table.Remove(row);
                PathInfo pathInfo = new PathInfo();
                if (row.programName != null)
                {
                    
                    var pathP = Path.Combine(pathInfo.upLoadPath(), orderNumber, "加工文件", row.programName);
                    var pathT = Path.Combine(pathInfo.upLoadPath(), orderNumber, "刀具表", row.toolChartName);
                    FileInfo fileP = new FileInfo(pathP);
                    fileP.Delete();
                    FileInfo fileT = new FileInfo(pathT);
                    fileT.Delete();

                   
                }
                DirectoryInfo di = new DirectoryInfo(Path.Combine(pathInfo.upLoadPath(), orderNumber, "工序" + row.ProcessID));
                if (di.Exists)
                {
                    di.Delete(true);
                }
               
                entities.SaveChanges();
                context.Response.Write("ok");

            };

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