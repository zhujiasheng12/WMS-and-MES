using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model.工厂管理
{
    /// <summary>
    /// PositionDeleteHandler1 的摘要说明
    /// </summary>
    public class PositionDeleteHandler1 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int id = int.Parse(context.Request["id"]);
            bool a = false;
            using(JDJS_WMS_DB_USEREntities data=new JDJS_WMS_DB_USEREntities())
            {
                var data2 = data.JDJS_WMS_Location_Info.Where(r => r.id == id);
                if (IfHaveMach(data2.ToList()))
                {
                    context.Response.Write("请先删除该车间或子车间机床");
                }
                else
                {
                    data.JDJS_WMS_Location_Info.Remove(data2.First());
                    data.SaveChanges();
                    context.Response.Write("ok");
                }
              
               
               
            }

           
        }
        
         bool IfHaveMach(List<JDJS_WMS_Location_Info> JDJS_WMS_Location_Infos)
                {
                     JDJS_WMS_DB_USEREntities data=new JDJS_WMS_DB_USEREntities();
                         bool a = false;
                    
                    foreach (var item in JDJS_WMS_Location_Infos)
                    {
                       
                        var mach = data.JDJS_WMS_Device_Info.Where(r => r.Position == item.id);
                        if (mach.Count() > 0)
                        {
                            a = true;
                            return a;
                           
                        }
                        else
                        {
                            var data3 = data.JDJS_WMS_Location_Info.Where(r => r.parentId == item.id);
                            if (data3.Count() > 0)
                            {
                                a = true;
                                return a;
                            }
                            else
                            {
                                continue;
                            }
                        }
                        
                    }

                    return a;
                }
                //void remove(List<JDJS_WMS_Location_Info> JDJS_WMS_Location_Infos)
                //{
                //    JDJS_WMS_DB_USEREntities data = new JDJS_WMS_DB_USEREntities();
                   
                //    foreach (var item in JDJS_WMS_Location_Infos)
                //    {
                        
                //        data.JDJS_WMS_Location_Info.Remove(item);
                //        var data3 = data.JDJS_WMS_Location_Info.Where(r => r.parentId == item.id);
                //        remove(data3.ToList());
                //    }
                  
                //}
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}