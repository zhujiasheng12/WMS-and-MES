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
    
    public partial class JDJS_WMS_Device_Alarm_Repair
    {
        public int ID { get; set; }
        public Nullable<int> CncID { get; set; }
        public Nullable<System.DateTime> RepairTime { get; set; }
        public Nullable<System.DateTime> Receptiontime { get; set; }
        public Nullable<System.DateTime> StartTime { get; set; }
        public Nullable<System.DateTime> EndTime { get; set; }
        public string CncNum { get; set; }
        public Nullable<int> AlarmDescID { get; set; }
        public Nullable<int> AlarmStateID { get; set; }
        public string MaintenanceStaff { get; set; }
        public string MaintenanceDesc { get; set; }
        public Nullable<long> OrderNumber { get; set; }
        public string ReportStaff { get; set; }
    
        public virtual JDJS_WMS_Device_Alarm_Description JDJS_WMS_Device_Alarm_Description { get; set; }
        public virtual JDJS_WMS_Device_Info JDJS_WMS_Device_Info { get; set; }
        public virtual JDJS_WMS_Device_Maintenance_Status JDJS_WMS_Device_Maintenance_Status { get; set; }
    }
}
