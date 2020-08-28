using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.生产管理.资材部.夹具管理.特殊治具管理Access数据库;

namespace WebApplication2.生产管理.资材部.夹具管理.录入系统治具库
{
    /// <summary>
    /// 读取临时治具库 的摘要说明
    /// </summary>
    public class 读取临时治具库 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            List<TemporaryFixtureInfo> infos = new List<TemporaryFixtureInfo>();
            try
            {
                using (FixtureModel model = new FixtureModel())
                {
                    var fixes = model.JDJS_WMS_Fixture_Temporary_Table;
                    foreach (var item in fixes)
                    {
                        TemporaryFixtureInfo info = new TemporaryFixtureInfo()
                        {
                            Id =item.Id ,
                            FixtureOrderNum =item.FixtureOrderNum ,
                            FixtureSpecification =item.FixtureSpecification ,
                            InTime =item.InTime ,
                            Name =item.Name ,
                            Remark =item.Remark==null?"":item.Remark,
                        };
                        infos.Add(info);
                    }
                }
            }
            catch (Exception ex)
            { 
            
            }
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
    public class TemporaryFixtureInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FixtureOrderNum { get; set; }
        public string FixtureSpecification { get; set; }
        public string Remark { get; set; }
        public DateTime? InTime { get; set; }
    }
}