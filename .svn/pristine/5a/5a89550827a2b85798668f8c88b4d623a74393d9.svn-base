using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2
{
    /// <summary>
    /// PositionHandler 的摘要说明
    /// </summary>
    public class PositionHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
   //         "id":11,
			//"name":"Java",
			//"size":"",
			//"date":"01/13/2010",
            context.Response.ContentType = "text/plain";
            List<NodeJson> jsons = new List<NodeJson>();
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            using (JDJS_WMS_DB_USEREntities JDJS_WMS_Device_Info = new JDJS_WMS_DB_USEREntities())
            {
                AddNodes(jsons, JDJS_WMS_Device_Info.JDJS_WMS_Location_Info.ToList());

            }
            string json = serializer.Serialize(jsons);
            context.Response.Write(json);
          
        }

        public void AddNodes(List<NodeJson> jsons, List<JDJS_WMS_Location_Info> nodes)
        {
            try
            {
                if (jsons.Count == 0)
                {
                    var firstNode = nodes.Where(r => r.parentId == 0);
                    foreach (var item in firstNode)
                    {
                        jsons.Add(new NodeJson() { id = item.id, name = item.Name, team = item.EndDate, hierarchy = item.size , });
                    }
                    AddNodes(jsons, nodes);
                }
                else
                {
                    foreach (var json in jsons)
                    {
                        var subNodes = nodes.Where(r => r.parentId == json.id);
                        if (subNodes.Count() > 0)
                        {
                            foreach (var item in subNodes)
                            {
                                json.children.Add(new NodeJson() { id = item.id, name = item.Name, team  = item.EndDate, hierarchy = item.size, });
                            }
                            AddNodes(json.children, nodes);
                        }
                        else
                        {

                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
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
    //public class Node
    //{
    //    public int id;
    //    public string Name;
    //    public string  size;
    //    public string  EndDate;
    //    public int parentId;
    //}

    public class NodeJson
    {
        public int id;
        public string name;
        public string hierarchy;
        public string team;
        public string iconCls;
        public List<NodeJson> children=new List<NodeJson>();
    }
}