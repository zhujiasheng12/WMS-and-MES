//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication2
{
    using System;
    using System.Collections.Generic;
    
    public partial class JDJS_WMS_Fixture_Manage_Demand_Table
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
        public Nullable<int> DesignPersonId { get; set; }
        public string DesignPersonName { get; set; }
        public string Material { get; set; }
        public string State { get; set; }
        public string AuditRemark { get; set; }
        public Nullable<System.DateTime> PlanEndTime { get; set; }
        public Nullable<System.DateTime> EndTime { get; set; }
        public string Remark { get; set; }
    }
}