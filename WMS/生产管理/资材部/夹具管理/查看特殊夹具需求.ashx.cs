using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.资材部.夹具管理
{
    /// <summary>
    /// 查看特殊夹具需求 的摘要说明
    /// </summary>
    public class 查看特殊夹具需求 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            List<FixtureInfo> infos = new List<FixtureInfo>();
            try
            {
                using (JDJS_WMS_DB_USEREntities model = new JDJS_WMS_DB_USEREntities())
                {
                    var demands = model.JDJS_WMS_Fixture_Manage_Demand_Table;
                    foreach (var item in demands)
                    {
                        FixtureInfo info = new FixtureInfo()
                        { 
                            ID =item.ID ,
                            DemandTime =item.DemandTime ,
                            DemandTimeStr =item.DemandTime ==null?"":item.DemandTime .ToString (),
                            DesignPersonId =item.DesignPersonId ,
                            DesignPersonName =item.DesignPersonName==null?"":item.DesignPersonName ,
                            AuditRemark =item.AuditRemark ==null?"":item.AuditRemark ,
                            EndTime =item.EndTime ,
                            EndTimeStr =item.EndTime ==null?"":item.EndTime .ToString (),
                            PlanEndTime =item.PlanEndTime ,
                            PlanEndTimeStr =item.PlanEndTime ==null?"":item.PlanEndTime .ToString (),
                            FixtureOrderNum =item.FixtureOrderNum ,
                            FixtureSpecification =item.FixtureSpecification ,
                            Material =item.Material ==null?"":item.Material ,
                            OrderId =item.OrderId ,
                            OrderNum =item.OrderNum ,
                            ProcessId =item.ProcessId ,
                            ProcessNum =item.ProcessNum ,
                            ProgramPersonName =item.ProgramPersonName ,
                            Remark =item.Remark ==null?"":item.Remark ,
                            State =item.State 
                        };
                        infos.Add(info);
                    }
                }
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                var json = serializer.Serialize(infos);
                context.Response.Write(json);
                return;
            }
            catch (Exception ex)
            {
                context.Response.Write(ex.Message);
                return;
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
    public class FixtureInfo
    {
        public int ID { get; set; }
        public Nullable<int> OrderId { get; set; }
        public Nullable<int> ProcessId { get; set; }
        public string OrderNum { get; set; }
        public Nullable<int> ProcessNum { get; set; }
        public string FixtureOrderNum { get; set; }
        public string FixtureSpecification { get; set; }
        public string ProgramPersonName { get; set; }
        public Nullable<System.DateTime> DemandTime { get; set; }
        public string DemandTimeStr { get; set; }
        public Nullable<int> DesignPersonId { get; set; }
        public string DesignPersonName { get; set; }
        public string Material { get; set; }
        public string State { get; set; }
        public string AuditRemark { get; set; }
        public Nullable<System.DateTime> PlanEndTime { get; set; }
        public string PlanEndTimeStr { get; set; }
        public Nullable<System.DateTime> EndTime { get; set; }
        public string EndTimeStr { get; set; }
        public string Remark { get; set; }
    }
}