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
    
    public partial class JDJS_WMS_Tool_LifeTime_Management
    {
        public int ID { get; set; }
        public Nullable<int> CncID { get; set; }
        public Nullable<int> ToolID { get; set; }
        public Nullable<double> ToolL { get; set; }
        public Nullable<double> ToolH { get; set; }
        public Nullable<double> ToolR { get; set; }
        public Nullable<double> ToolD { get; set; }
        public Nullable<double> ToolMaxTime { get; set; }
        public Nullable<double> ToolCurrTime { get; set; }
        public Nullable<double> ToolMaxNum { get; set; }
        public Nullable<double> ToolCurrNum { get; set; }
        public Nullable<double> ToolMaxDistance { get; set; }
        public Nullable<double> ToolCurrDistance { get; set; }
    
        public virtual JDJS_WMS_Device_Info JDJS_WMS_Device_Info { get; set; }
    }
}