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
    
    public partial class JDJS_WMS_Location_Info
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public JDJS_WMS_Location_Info()
        {
            this.JDJS_WMS_Device_Info = new HashSet<JDJS_WMS_Device_Info>();
        }
    
        public string Name { get; set; }
        public string size { get; set; }
        public int id { get; set; }
        public string EndDate { get; set; }
        public int parentId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JDJS_WMS_Device_Info> JDJS_WMS_Device_Info { get; set; }
    }
}