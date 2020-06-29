using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace WebApplication2.Model.生产管理.工程部
{
    /// <summary>
    /// orderDelete 的摘要说明
    /// </summary>
    public class orderDelete : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            var id = int.Parse(context.Request["id"]);
            using (JDJS_WMS_DB_USEREntities entities = new JDJS_WMS_DB_USEREntities())
            {
                using (System.Data.Entity.DbContextTransaction db = entities.Database.BeginTransaction())
                {
                    try
                    {
                        var row = entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == id);
                        var number = row.First().Order_Number;
                        entities.JDJS_WMS_Order_Entry_Table.Remove(row.First());
                        entities.SaveChanges();
                        PathInfo pathInfo = new PathInfo();
                        var root = pathInfo.upLoadPath();
                        var path = Path.Combine(root, number);

                        DirectoryInfo directory = new DirectoryInfo(path);
                        FileInfo[] file = directory.GetFiles();
                        foreach (var item in file)
                        {
                            item.Delete();
                        }
                        directory.Delete();
                        db.Commit();
                        context.Response.Write("ok");
                    }
                    catch(Exception ex)
                    {
                        db.Rollback();
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