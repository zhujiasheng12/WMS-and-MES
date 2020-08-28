using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.生产管理.资材部.夹具管理.特殊治具管理Access数据库;

namespace WebApplication2.生产管理.资材部.夹具管理.治具种类管理
{
    /// <summary>
    /// 读取治具种类 的摘要说明
    /// </summary>
    public class 读取治具种类 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            List<TypeInfo> infos = new List<TypeInfo>();
            using (FixtureModel model = new FixtureModel())
            {
                var types = model.JDJS_WMS_Fixture_Type_Table;
                foreach (var item in types)
                {
                    TypeInfo info = new TypeInfo()
                    { 
                    Id =item.Id ,
                    Type =item.Type ,
                    };
                    infos.Add(info);
                }
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
    public class TypeInfo
    { 
        public int Id { get; set; }
        public string Type { get; set; }
    }
}