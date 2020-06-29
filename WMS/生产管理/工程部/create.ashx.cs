using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApplication2.Model.生产管理.工程部
{
    /// <summary>
    /// create 的摘要说明
    /// </summary>
    public class create : IHttpHandler
    {

       
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            var form = context.Request.Form;
            var file = context.Request.Files;
            PathInfo pathInfo = new PathInfo();
            var folder =pathInfo.upLoadPath()+ form[0];
          
            using (JDJS_WMS_DB_USEREntities entities = new JDJS_WMS_DB_USEREntities())
            {
                using (System.Data.Entity.DbContextTransaction date = entities.Database.BeginTransaction())
                {
                    try
                    {
                        var number = form[0];
                   
                    var judge = entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_Number == number);
                    if (judge.Count() > 0)
                    {
                        context.Response.Write("该订单已存在");
                        return;
                    }
                    var row = new JDJS_WMS_Order_Entry_Table
                    {
                        Order_Number = form[0],
                        Order_Leader = form[1],
                        Product_Name = form[2],
                        Product_Material = form[3],
                        Product_Output = int.Parse(form[4]),
                        Order_Plan_End_Time = DateTime.Parse(form[5]),
                        Order_State = form[6],
                        Order_Plan_Start_Time = DateTime.Now
                    };
                    entities.JDJS_WMS_Order_Entry_Table.Add(row);



                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    };

                    for (int i = 0; i < file.Count; i++)
                    {
                        var name = file[i].FileName;
                        var size = file[i].ContentLength;
                        string path = Path.Combine(folder, name);
                        file[i].SaveAs(path);

                    }
                      entities.SaveChanges();
                    context.Response.Write("ok");
                        date.Commit();

                    }
                    catch(Exception ex)
                    {
                        date.Rollback();
                        context.Response.Write(ex.Message);
                    }
                }
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