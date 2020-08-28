using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.资材部.夹具管理.特殊治具管理Access数据库
{
    public class JDJS_WMS_Fixture_System_Table
    {
        public int Id { get; set; }
        public int? TypeId { get; set; }
        public int? FXNum { get; set; }
        public string FixtureOrderNum { get; set; }
        public string Name { get; set; }
        public string VenderName { get; set; }
        public string SerialCode { get; set; }
        public string Desc { get; set; }
        public string FileName { get; set; }
        public string Remark { get; set; }
        public int? StockCurrNum { get; set; }
        public int? StockAllNum { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? AlterTime { get; set; }
    }
}