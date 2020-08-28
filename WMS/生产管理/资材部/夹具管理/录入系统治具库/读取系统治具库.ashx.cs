using DocumentFormat.OpenXml.Office2010.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.生产管理.资材部.夹具管理.特殊治具管理Access数据库;

namespace WebApplication2.生产管理.资材部.夹具管理.录入系统治具库
{
    /// <summary>
    /// 读取系统治具库 的摘要说明
    /// </summary>
    public class 读取系统治具库 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            List<SystemFixInfo> infos = new List<SystemFixInfo>();
            try
            {
                using (FixtureModel model = new FixtureModel())
                {
                    var fixes = model.JDJS_WMS_Fixture_System_Table;
                    foreach (var item in fixes)
                    {
                        SystemFixInfo info = new SystemFixInfo()
                        {
                            AlterTime = item.AlterTime == null ? DateTime.Now : Convert.ToDateTime(item.AlterTime),
                            AlterTimeStr = item.AlterTime == null ? "" : item.AlterTime.ToString(),
                            CreateTime = item.CreateTime == null ? DateTime.Now : Convert.ToDateTime(item.CreateTime),
                            CreateTimeStr = item.CreateTime == null ? "" : item.CreateTime.ToString(),
                            Desc = item.Desc == null ? "" : item.Desc,
                            FileName = item.FileName == null ? "" : item.FileName,
                            FixtureOrderNum = item.FixtureOrderNum == null ? "" : item.FixtureOrderNum,
                            FXNum = item.FXNum == null ? 0 : Convert.ToInt32(item.FXNum),
                            Id = item.Id,
                            Name = item.Name == null ? "" : item.Name,
                            Remark = item.Remark == null ? "" : item.Remark,
                            SerialCode = item.SerialCode == null ? "" : item.SerialCode,
                            StockAllNum = item.StockAllNum == null ? 0 : Convert.ToInt32(item.StockAllNum),
                            StockCurrNum = item.StockCurrNum == null ? 0 : Convert.ToInt32(item.StockCurrNum),
                            TypeId = item.TypeId == null ? 0 : Convert.ToInt32(item.TypeId),
                            VenderName = item.VenderName == null ? "" : item.VenderName ,
                            Type=model.JDJS_WMS_Fixture_Type_Table .Where (r=>r.Id ==item.TypeId).FirstOrDefault ()==null?"": model.JDJS_WMS_Fixture_Type_Table.Where(r => r.Id == item.TypeId).FirstOrDefault().Type

                        };
                        infos.Add(info);
                    }
                }
            }
            catch (Exception ex)
            { }
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var json = serializer.Serialize(infos);
            context.Response.Write(json);
            return;

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }

    public class SystemFixInfo
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public string Type { get; set; }
        public int FXNum { get; set; }
        public string FixtureOrderNum { get; set; }
        public string Name { get; set; }
        public string VenderName { get; set; }
        public string SerialCode { get; set; }
        public string Desc { get; set; }
        public string FileName { get; set; }
        public string Remark { get; set; }
        public int StockCurrNum { get; set; }
        public int StockAllNum { get; set; }
        public DateTime CreateTime { get; set; }
        public string CreateTimeStr { get; set; }
        public DateTime AlterTime { get; set; }
        public string AlterTimeStr { get; set; }
    }
}