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
    
    public partial class JDJS_WMS_Quality_Confirmation_History_Table
    {
        public int ID { get; set; }
        public Nullable<int> OrderID { get; set; }
        public Nullable<int> GoodNum { get; set; }
        public Nullable<int> PoolNum { get; set; }
        public Nullable<System.DateTime> OperateTime { get; set; }
        public Nullable<int> StaffID { get; set; }
    
        public virtual JDJS_WMS_Order_Entry_Table JDJS_WMS_Order_Entry_Table { get; set; }
        public virtual JDJS_WMS_Staff_Info JDJS_WMS_Staff_Info { get; set; }
    }
}