using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApplication2.测试
{
    /// <summary>
    /// 读svg 的摘要说明
    /// </summary>
    public class 读svg : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            PathInfo path = new PathInfo();
            var cncLayoutPath = path.cncLayoutPath();
            var workId = int.Parse(context.Request["workId"]);
           if( File.Exists(cncLayoutPath + workId + ".svg"))
            {
                string dd = "";
                StreamReader sr = new StreamReader(cncLayoutPath + workId+".svg", System.Text.Encoding.UTF8);

                while (!sr.EndOfStream)
                {

                    dd += sr.ReadLine();
                }
                sr.Close();
                context.Response.Write(dd);
                return;
            }
            else
            {
                using(JDJS_WMS_DB_USEREntities entities=new JDJS_WMS_DB_USEREntities())
                {
                    var works=entities.JDJS_WMS_Location_Info.ToList();
                    var devices=entities.JDJS_WMS_Device_Info.ToList();
                    List<CncRead> objs=new List<CncRead>();
                    List<int> workIds=new List<int>();
                 var  obj=  fun(workId, works, devices, objs, workIds);
                    //var obj = from cnc in entities.JDJS_WMS_Device_Info.Where(r => r.Position == workId)
                    //         select new
                    //         {
                    //             cnc.ID,
                    //             cnc.MachNum
                    //         };
                    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var json = serializer.Serialize(obj);
                    context.Response.Write(json);

                }
            }

           
           
        }
        List<CncRead> fun(int workId, List<JDJS_WMS_Location_Info> works, List<JDJS_WMS_Device_Info> devices, List<CncRead> obj, List<int> workIds)
        {

            if (workIds.Count == 0)
            {
                workIds.Add(workId);
                var rows = devices.Where(r => r.Position == workId);
                foreach (var item in rows)
                {
                    obj.Add(new CncRead { ID = item.ID.ToString(), MachNum = item.MachNum });
                }
                var childrens = works.Where(r => r.parentId == workId);
                foreach (var item in childrens)
                {
                    fun(item.id, works, devices, obj, workIds);
                }
                return obj;
            }
            else {

                var rows = devices.Where(r => r.Position == workId);
                foreach (var item in rows)
                {
                    obj.Add(new CncRead { ID = item.ID.ToString(), MachNum = item.MachNum });
                }
                var childrens = works.Where(r => r.parentId == workId);
                foreach (var item in childrens)
                {
                    fun(item.id, works, devices, obj, workIds);
                }
                return obj;
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
    class CncRead {
        public string ID;
        public string MachNum;
    }
}