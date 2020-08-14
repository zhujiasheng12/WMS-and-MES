using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace WebApplication2.Model.生产管理.市场部
{
    /// <summary>
    /// edit 的摘要说明
    /// </summary>
    public class edit : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {


                context.Response.ContentType = "text/plain";
                var form = context.Request.Form;


                var Order_Number = form["Order_Number"];
                var Order_Leader = form["Order_Leader"];
                var Product_Name = form["Product_Name"];
                var Product_Material = form["Product_Material"];
                var Product_Output = int.Parse(form["Product_Output"]);
                var Order_Plan_End_Time = DateTime.Parse(form["Order_Plan_End_Time"]);
                var Order_State = form["Order_State"];
                var id = int.Parse(form["id"]);
                var pattern = int.Parse(form["pattern"]);
                var ProjectName = form["ProjectName"];
                var ClientName = form["ClientName"];
                var remark= form["remark"];

                List<EditRead> editReads = new List<EditRead>();
                using (JDJS_WMS_DB_USEREntities entities = new JDJS_WMS_DB_USEREntities())
                {
                    var row = entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == id).First();
                    //var folder = @"D:\服务器文件勿动\" + row.Order_Number;
                    PathInfo pathInfo = new PathInfo();
                    var folder = Path.Combine(pathInfo.upLoadPath(), row.Order_Number, @"客供图纸");
                    var rows = entities.JDJS_WMS_Order_Entry_Table;
                    foreach (var item in rows)
                    {
                        editReads.Add(new EditRead { Order_Number = item.Order_Number });
                    }
                    editReads.RemoveAll(r => r.Order_Number == row.Order_Number);
                    var judge = editReads.Where(r => r.Order_Number == Order_Number);
                    if (judge.Count() > 0)
                    {
                        context.Response.Write("订单号重复");
                        return;

                    }
                    else
                    {
                        row.Order_Number = Order_Number;
                        row.Order_Leader = Order_Leader;
                        row.Product_Name = Product_Name;
                        row.Product_Material = Product_Material;
                        row.Product_Output = Product_Output;
                        row.Order_Plan_End_Time = Order_Plan_End_Time;
                        row.Order_State = Order_State;
                        row.ProofingORProduct = pattern;
                        row.ProjectName = ProjectName;
                        row.Remark = remark;
                        var guide = entities.JDJS_WMS_Order_Guide_Schedu_Table.Where(r => r.OrderID == id).FirstOrDefault();
                        if (guide != null) {
                            guide.ClientName = ClientName;
                        }
                    }
                    var row1 = entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_Number == Order_Number);
                    if (row1.Count() > 0)
                    {

                    }
                    else
                    {
                       
                        DirectoryInfo directory = new DirectoryInfo(folder);
                        //var path = Path.Combine(@"D:/服务器文件勿动/",Order_Number );
                       
                        var path = Path.Combine(pathInfo.upLoadPath(), Order_Number, @"客供图纸");
                        directory.MoveTo(path);
                    }
                    var process = entities.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == id && r.ProcessID == 1 && r.sign != 0);
                    if (process.Count() > 0)
                    {
                        foreach (var item in process)
                        {
                            item.BlankNumber = Product_Output;
                        }
                    }


                    context.Response.Write("ok");
                    entities.SaveChanges();
                }




            }
            catch(Exception ex)
            {
                context.Response.Write(ex.Message);
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