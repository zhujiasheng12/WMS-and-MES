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
    
    public partial class JDJS_WMS_Finished_Product_OutPut_History_Manager
    {
        public int ID { get; set; }
        public Nullable<int> OrderID { get; set; }
        public Nullable<int> OutPutNum { get; set; }
        public Nullable<System.DateTime> Time { get; set; }
    
        public virtual JDJS_WMS_Order_Entry_Table JDJS_WMS_Order_Entry_Table { get; set; }
    }
}
