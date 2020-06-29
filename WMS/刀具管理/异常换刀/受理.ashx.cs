using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace WebApplication2.刀具管理.异常换刀
{
    /// <summary>
    /// 受理 的摘要说明
    /// </summary>
    public class 受理 : IHttpHandler,IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            var Id=int.Parse( context.Request.Form["Id"]);
            string str = "";
            try

            {
                using (JDJS_WMS_DB_USEREntities entities = new JDJS_WMS_DB_USEREntities())
                {
                    var rows = entities.JDJS_WMS_Tool_Machine_Replaace_History_Table.Where(r => r.ID == Id);
                    if (rows.Count() > 0)
                    {
                        var userId =int.Parse( context.Session["id"].ToString());
                        rows.First().Flag = 1;
                        rows.First().AcceptPersonID = userId;
                        rows.First().AcceptPersonTime = DateTime.Now;
                        entities.SaveChanges();
                        str = "ok";
                    }
                    else
                    {
                        str = "该记录不存在";

                    }
                }
            }
            catch (Exception ex)
            {
                str = ex.Message;
            }
            finally
            {
                context.Response.Write(str);
               
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