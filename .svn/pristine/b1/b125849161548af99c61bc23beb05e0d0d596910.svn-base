using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model.人员管理
{
    /// <summary>
    /// JDJS_WMS_Staff_InfoRead 的摘要说明
    /// </summary>
    public class JDJS_WMS_Staff_InfoRead : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            List<JDJS_WMS_Staff_InfoReadClass> JDJS_WMS_Staff_InfoReads = new List<JDJS_WMS_Staff_InfoReadClass>();
           using (JDJS_WMS_DB_USEREntities entities1=new JDJS_WMS_DB_USEREntities())
            {
                addData(JDJS_WMS_Staff_InfoReads, entities1.JDJS_WMS_Staff_Info.ToList());
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                var json = serializer.Serialize(JDJS_WMS_Staff_InfoReads);
                context.Response.Write(json);
            }

        }

        
      void addData(List<JDJS_WMS_Staff_InfoReadClass> jsons,List<JDJS_WMS_Staff_Info> JDJS_WMS_Staff_Infos)
        {
            if (jsons.Count() == 0)
            {
                var parents = JDJS_WMS_Staff_Infos.Where(r => r.parentId == 0);
                foreach (var item in parents)
                {
                    jsons.Add(new JDJS_WMS_Staff_InfoReadClass
                    {
                        id = item.id,
                        staff = item.staff,
                        position = item.position,
                        tel = item.tel,
                        //iconCls = "icon-ok"
                    });
                    
                    
                }
                addData(jsons, JDJS_WMS_Staff_Infos);
            }
            else
            {
                foreach (var item in jsons)
                {
                    var sons = JDJS_WMS_Staff_Infos.Where(r => r.parentId == item.id);
                    if (sons.Count() > 0)
                    {
                        foreach (var i in sons)
                        {
                            item.children.Add(new JDJS_WMS_Staff_InfoReadClass
                            {
                                id = i.id,
                                staff = i.staff,
                                position = i.position,
                                tel = i.tel,
                                remark=i.remark,
                                mailbox=i.mailbox,
                                //iconCls = "icon-ok"
                            });

                        }
                        addData(item.children, JDJS_WMS_Staff_Infos);
                    }

                }
            }
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
    class JDJS_WMS_Staff_InfoReadClass
    {
        public int id;
        public string staff;
        public string position;
        public string tel;
        public string iconCls;
        public string remark;
        public string mailbox;
        public List<JDJS_WMS_Staff_InfoReadClass> children = new List<JDJS_WMS_Staff_InfoReadClass>();
    }
}