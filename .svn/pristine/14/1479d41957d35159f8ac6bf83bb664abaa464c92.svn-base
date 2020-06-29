using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.刀具管理.刀具历史
{
    /// <summary>
    /// 查看弹窗读数据 的摘要说明
    /// </summary>
    public class 查看弹窗读数据 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var primaryKey = int.Parse(context.Request["primaryKey"]);
            using(JDJS_WMS_DB_USEREntities entities=new JDJS_WMS_DB_USEREntities())
            {
                var rows = entities.JDJS_WMS_Scan_Operate_History_Table.Where(r => r.ToolID == primaryKey).OrderByDescending(r=>r.Time);
                List<Read> reads = new List<Read>();
                foreach (var item in rows)
                {
                    var personId = item.PersonID;
                    var persons= entities.JDJS_WMS_Staff_Info.Where(r => r.id == personId);
                    var person = "";
                    var cnc = "";
                    var toolShank = "";
                    if (persons.Count() > 0)
                    {
                        person = persons.First().staff;
                    }
                    if (entities.JDJS_WMS_Device_Info.Where(r => r.ID == item.machID).Count() > 0)
                    {
                        cnc = entities.JDJS_WMS_Device_Info.Where(r => r.ID == item.machID).First().MachNum;
                    }
                    if (entities.JDJS_WMS_Tool_Shank_Table.Where(r => r.ID == item.ShankID).Count() > 0)
                    {
                       var ShankSpecificationID = entities.JDJS_WMS_Tool_Shank_Table.Where(r => r.ID == item.ShankID).First().ShankSpecificationID;
                        var ShankSpecification = entities.JDJS_WMS_Tool_Shanks_Specification_Table.Where(r => r.ID == ShankSpecificationID).First().ToolShankSpecificationNum.ToString();
                        var ShankNum= entities.JDJS_WMS_Tool_Shank_Table.Where(r => r.ID == item.ShankID).First().ShankNum.ToString();
                        while (ShankSpecification.Length < 3)
                        {
                            ShankSpecification = "0" + ShankSpecification;
                        }
                        while (ShankNum.Length < 4)
                        {
                            ShankNum = "0" + ShankNum;
                        }
                        toolShank ="TS-" +ShankSpecification + ShankNum;
                    }
                    reads.Add(new Read { Operate = item.Operate, remarks = item.remarks, time = item.Time.ToString(),Person=person ,cnc=cnc,toolShank=toolShank});
                }
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                var json = serializer.Serialize(reads);
                context.Response.Write(json);
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
    public class Read
    {
        public string time;
        public string Person;
        public string Operate;
        public string remarks;
        public string cnc;
        public string toolShank;
    }
}