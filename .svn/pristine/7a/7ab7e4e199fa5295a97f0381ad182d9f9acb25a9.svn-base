using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace WebApplication2.Model.生产管理.市场部
{
    /// <summary>
    /// orderDelete 的摘要说明
    /// </summary>
    public class orderDelete : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
        
                using (JDJS_WMS_DB_USEREntities entities = new JDJS_WMS_DB_USEREntities())
                {
                    using (System.Data.Entity.DbContextTransaction db = entities.Database.BeginTransaction())
                    {
                    try
                    {
                        var form = context.Request.Form;
                        for (int i = 0; i < form.Count; i++)
                        {
                            var orderId = int.Parse(form[i]);
                            var id = orderId;

                            {
                                var row = entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == id);
                                var number = row.First().Order_Number;
                                entities.JDJS_WMS_Order_Entry_Table.Remove(row.First());
                                entities.SaveChanges();
                                PathInfo pathInfo = new PathInfo();
                                var root = pathInfo.upLoadPath();
                                var path = Path.Combine(root, number);

                                DirectoryInfo directory = new DirectoryInfo(path);
                                if (!directory.Exists)
                                {
                                    directory.Create();
                                }
                                FileInfo[] file = directory.GetFiles();
                                FileSystemInfo[] fileinfo = directory.GetFileSystemInfos();



                                foreach (var item in fileinfo)
                                {
                                    if (item is DirectoryInfo)
                                    {
                                        DirectoryInfo subdir = new DirectoryInfo(item.FullName);
                                        subdir.Delete(true);
                                    }
                                    else
                                    {
                                        File.Delete(item.FullName);
                                    }
                                }
                                directory.Delete();


                            }

                        }
                        entities.SaveChanges();
                        db.Commit();
                    }
                    catch (Exception ex)
                    {
                        db.Rollback();
                        context.Response.Write(ex.Message);
                        return;
                    }
                }


            }
            context.Response.Write("ok");
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