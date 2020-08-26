using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.资材部.夹具管理.特殊治具管理Access数据库
{
    /// <summary>
    /// 治具种类表
    /// </summary>
    public class JDJS_WMS_Fixture_Type_Table
    {
        [Key]
        public int Id { get; set; }

        public string Type { get; set; }

    }
}