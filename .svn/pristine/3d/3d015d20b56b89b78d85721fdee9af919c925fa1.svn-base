using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.生产部.换产
{
    /// <summary>
    /// 读取可换产机床 的摘要说明
    /// </summary>
    public class 读取可换产机床 : IHttpHandler
    {
        int flag = -1;
        public void ProcessRequest(HttpContext context)
        {
            int schId = int.Parse(context.Request["schId"]);

            context.Response.ContentType = "text/plain";
            List<NodeJson> jsons = new List<NodeJson>();
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            using (JDJS_WMS_DB_USEREntities JDJS_WMS_Device_Info = new JDJS_WMS_DB_USEREntities())
            {
                var sch = JDJS_WMS_Device_Info.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.ID == schId).FirstOrDefault().ProcessID;
                var deviceType = JDJS_WMS_Device_Info.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == sch).FirstOrDefault().DeviceType;
                AddNodes(jsons, JDJS_WMS_Device_Info.JDJS_WMS_Location_Info.ToList(), JDJS_WMS_Device_Info.JDJS_WMS_Device_Info.Where (r=>r.MachType ==deviceType).ToList());

            }
            //NodeState(jsons);
            string json = serializer.Serialize(jsons);
            context.Response.Write(json);
        }
        public static void NodeState(List<NodeJson> jsons)
        {
            foreach (var item in jsons)
            {
                if (item.children.Count > 0)
                {
                    NodeState(item.children);
                    foreach (var real in item.children)
                    {
                        if (real.flag == 1)
                        {
                            item.flag = 1;
                        }
                        else if (real.flag == 2)
                        {
                            item.flag = 2;
                            //break;
                        }
                        else
                        {
                            item.flag = 0;
                        }
                    }
                    //NodeState(item.children);
                }
            }
        }
        public void AddNodes(List<NodeJson> jsons, List<JDJS_WMS_Location_Info> nodes, List<JDJS_WMS_Device_Info> deviceNodes)
        {
            try
            {
                if (jsons.Count == 0)
                {
                    var firstNode = nodes.Where(r => r.parentId == 0);
                    foreach (var item in firstNode)
                    {
                        jsons.Add(new NodeJson() { id = item.id, name = item.Name, team = item.EndDate, hierarchy = item.size, iconCls = "icon-ok" });
                    }
                    AddNodes(jsons, nodes, deviceNodes);
                }
                else
                {
                    foreach (var json in jsons)
                    {
                        var subNodes = nodes.Where(r => r.parentId == json.id);
                        var DevicesubNodes = deviceNodes.Where(r => r.Position == json.id);
                        if (subNodes.Count() > 0 || DevicesubNodes.Count() > 0)
                        {
                            foreach (var item in subNodes)
                            {
                                json.children.Add(new NodeJson() { id = item.id, name = item.Name, team = item.EndDate, hierarchy = item.size, flag = -1 });
                            }
                            foreach (var item in DevicesubNodes)
                            {
                                int error = 0;
                                
                                json.children.Add(new NodeJson() { id = flag, CncID = item.ID, name = item.MachNum, team = item.IP, hierarchy = item.MachState, flag = error });
                                flag--;
                            }
                            AddNodes(json.children, nodes, deviceNodes);
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
    public class NodeJson
    {
        public int id;
        public int CncID;
        public string name;
        public string hierarchy;
        public string team;
        public string iconCls;
        public int flag;
        public List<NodeJson> children = new List<NodeJson>();
    }
}