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
    
    public partial class JDJS_WMS_Quality_Detection_Measurement_Table
    {
        public int ID { get; set; }
        public Nullable<int> OrderID { get; set; }
        public Nullable<int> ProcessNum { get; set; }
        public string WorkpieceNumber { get; set; }
        public string Type { get; set; }
        public string SizeName { get; set; }
        public Nullable<double> StandardValue { get; set; }
        public Nullable<double> ToleranceRangeMin { get; set; }
        public Nullable<double> ToleranceRangeMax { get; set; }
        public Nullable<double> Measurements { get; set; }
        public Nullable<double> OutOfTolerance { get; set; }
    
        public virtual JDJS_WMS_Order_Entry_Table JDJS_WMS_Order_Entry_Table { get; set; }
    }
}