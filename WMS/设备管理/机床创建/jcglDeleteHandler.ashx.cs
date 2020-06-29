using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace WebApplication2.Model
{
    /// <summary>
    /// jcglDeleteHandler 的摘要说明
    /// </summary>
    public class jcglDeleteHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int id = int.Parse(context.Request["id"]);
            using (JDJS_WMS_DB_USEREntities pm2 = new JDJS_WMS_DB_USEREntities())
            {
                using (System.Data.Entity.DbContextTransaction mytran = pm2.Database.BeginTransaction())
                {
                    try
                    {
                        System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                        context.Response.ContentType = "text/plain";

                        var data = (from d in pm2.JDJS_WMS_Device_Info
                                    where d.ID == id
                                    select d).Single();
                        var workId = pm2.JDJS_WMS_Device_Info.Where(r => r.ID == id).First().Position;
                        pm2.JDJS_WMS_Device_Info.Remove(data);    //再标记主表数据为删除装填
                     
                        PathInfo path = new PathInfo();
                        var svgPathDir = path.cncLayoutPath();
                      var svgPathFile=  Path.Combine(svgPathDir, workId.ToString()+ ".svg");
                      //  File.Delete(svgPathFile);
                        pm2.SaveChanges();
                        mytran.Commit();
                        context.Response.Write("ok");
                    }
                    catch(Exception ex)
                    {
                        mytran.Rollback();
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