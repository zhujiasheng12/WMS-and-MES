using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Kanban.现场
{
    /// <summary>
    /// 机台状态 的摘要说明
    /// </summary>
    public class 机台状态 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var workId = int.Parse(context.Request["workId"]);
            //机台图标状态
            List<PngInfo> pngInfos = new List<PngInfo>();
            using (JDJS_WMS_DB_USEREntities  wms = new JDJS_WMS_DB_USEREntities ())
            {
                var works = wms.JDJS_WMS_Location_Info.ToList();
                var devices = wms.JDJS_WMS_Device_Info.ToList();
                List<CncRead> objs = new List<CncRead>();
                List<int> workIds = new List<int>();
                var MachS = fun(workId, works, devices, objs, workIds);



              
               // var MachS = wms.JDJS_WMS_Device_Info.Where(r=>r.Position==workId).ToList();
                var Status11Code = wms.JDJS_WMS_Device_Maintenance_Status.Where(r => r.DescID == 11).First();
                var Status14Code = wms.JDJS_WMS_Device_Maintenance_Status.Where(r => r.DescID == 14).First();
                var Status16Code = wms.JDJS_WMS_Device_Maintenance_Status.Where(r => r.DescID == 16).First();
                var Status19Code = wms.JDJS_WMS_Device_Maintenance_Status.Where(r => r.DescID == 19).First();
                var Status1Code = wms.JDJS_WMS_Device_Maintenance_Status.Where(r => r.DescID == 1).First();
                var Status0Code = wms.JDJS_WMS_Device_Maintenance_Status.Where(r => r.DescID == 0).First();
                foreach (var item in MachS)
                {
                    int cncID =Convert.ToInt32( item.ID);
                    PngInfo pngInfo = new PngInfo();
                    pngInfo.flag = -2;
                    pngInfo.state = -1;
                    pngInfo.MachIP = item.IP;
                    pngInfo.MachID = item.ID.ToString();
                    pngInfo.MachNum = item.MachNum;
                    var states = wms.JDJS_WMS_Device_RealTime_Data.Where(r => r.CncID == cncID).FirstOrDefault();
                    if (states != null)
                    {
                        pngInfo.state = Convert.ToInt32(states.ProgState);
                    }
                    
                    var alarm = wms.JDJS_WMS_Device_Alarm_Repair.Where(r => r.CncID == cncID && r.AlarmStateID != Status0Code.ID && r.AlarmStateID != Status19Code.ID);
                    var alarm19 = wms.JDJS_WMS_Device_Alarm_Repair.Where(r => r.CncID == cncID && r.AlarmStateID == Status19Code.ID);
                    if (alarm.Count() > 0)
                    {
                        pngInfo.flag = -1;
                    }
                    else if (alarm19.Count() > 0)
                    {
                        pngInfo.flag = 0;
                    }
                    else
                    {
                        pngInfo.flag =1;
                    }
                    pngInfos.Add(pngInfo);
                }
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                var json = serializer.Serialize(pngInfos);
                context.Response.Write("data:"+json+"\n\n");
                context.Response.ContentType = "text/event-stream";
            }

        }
     public   List<CncRead> fun(int workId, List<JDJS_WMS_Location_Info> works, List<JDJS_WMS_Device_Info> devices, List<CncRead> obj, List<int> workIds)
        {

            if (workIds.Count == 0)
            {
                workIds.Add(workId);
                var rows = devices.Where(r => r.Position == workId);
                foreach (var item in rows)
                {
                    obj.Add(new CncRead { ID = item.ID.ToString(), MachNum = item.MachNum,IP=item.IP });
                }
                var childrens = works.Where(r => r.parentId == workId);
                foreach (var item in childrens)
                {
                    fun(item.id, works, devices, obj, workIds);
                }
                return obj;
            }
            else
            {

                var rows = devices.Where(r => r.Position == workId);
                foreach (var item in rows)
                {
                    obj.Add(new CncRead { ID = item.ID.ToString(), MachNum = item.MachNum ,IP=item.IP});
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
    public class PngInfo
    {
        /// <summary>
        /// 机台编号
        /// </summary>
        public string MachNum;
        /// <summary>
        /// 机台主键ID
        /// </summary>
        public string MachID;
        /// <summary>
        /// 机床IP
        /// </summary>
        public string MachIP;
        /// <summary>
        /// 机床状态，-1关机，0停止，1运行，2暂停，3复位，4报警
        /// </summary>
        public int state;
        /// <summary>
        /// 维修标志位。-1不能用，0维修完成待确认，1可以使用
        /// </summary>
        public int flag;
    }

  public  class CncRead
    {
        public string ID;
        public string IP;
        public string MachNum;
    }


}