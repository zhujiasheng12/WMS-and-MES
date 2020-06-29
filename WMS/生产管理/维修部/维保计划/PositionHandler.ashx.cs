using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model.生产管理.维保
{
    /// <summary>
    /// PositionHandler 的摘要说明
    /// </summary>
    public class PositionHandler1 : IHttpHandler
    {
        static  int flag = -1;
        static int num = -1;
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
                AddNodes(jsons, JDJS_WMS_Device_Info.JDJS_WMS_Location_Info.ToList(), JDJS_WMS_Device_Info.JDJS_WMS_Device_Info.ToList(),JDJS_WMS_Device_Info.JDJS_WMS_Device_Alarm_Repair .ToList ()); 

            }

            string json = serializer.Serialize(jsons);
            context.Response.Write(json);
          
        }


        public static void AddNodes(List<NodeJson> jsons, List<JDJS_WMS_Location_Info> nodes, List<JDJS_WMS_Device_Info> deviceNodes, List<JDJS_WMS_Device_Alarm_Repair> ToolInfo)
        {
            List<int> intlist = new List<int>();
            foreach (var item in nodes)
            {
                if (!intlist.Contains(item.parentId))
                {
                    intlist.Add(item.parentId);
                }
            }
            foreach (var item in deviceNodes)
            {
                if (!intlist.Contains(Convert.ToInt32(item.Position)))
                {
                    intlist.Add(Convert.ToInt32(item.Position));
                }
            }
            try
            {
                if (jsons.Count == 0)
                {
                    var firstNode = nodes.Where(r => r.parentId == 0);
                    foreach (var item in firstNode)
                    {
                        jsons.Add(new NodeJson() { id = item.id, name = item.Name, team = item.EndDate, hierarchy = item.size, });
                    }
                    AddNodes(jsons, nodes, deviceNodes, ToolInfo);
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

                                //如何是最低节点，state为null
                                if (intlist.Contains(item.id))
                                {
                                    json.children.Add(new NodeJson() { id = item.id, name = item.Name, team = item.EndDate, hierarchy = item.size, flag = -1, CncID = num, state = "closed" });
                                    num--;
                                }
                                else
                                {
                                    json.children.Add(new NodeJson() { id = item.id, name = item.Name, team = item.EndDate, hierarchy = item.size, flag = -1, CncID = num });
                                    num--;
                                }
                            }
                            foreach (var item in DevicesubNodes)
                            {
                                var toollist = ToolInfo.Where(r => r.CncID == item.ID);
                                int error = 0;
                                foreach (var real in toollist)
                                {
                                    switch (real.AlarmStateID)
                                    {
                                        case 4:
                                            error = 4;
                                            break;
                                        case 3:
                                            error = 3;
                                            break;
                                        case 2:
                                            error = 2;
                                            break;
                                        case 5:
                                            error = 5;
                                            break;
                                        case 1:
                                            error = 1;
                                            break;
                                        default:
                                            error = 4;
                                            break;
                                    }
                                }
                                json.children.Add(new NodeJson() { id = flag, CncID = item.ID, name = item.MachNum, team = item.IP, hierarchy = item.MachState, flag = error,  });
                                flag--;
                            }
                            AddNodes(json.children, nodes, deviceNodes, ToolInfo);
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

    //public class NodeJson
    //{
    //    public int id;
    //    public int CncID;
    //    public string name;
    //    public string hierarchy;
    //    public string team;
    //    public string iconCls;
    //    public int flag;
    //    public string state;
    //    public List<NodeJson> children=new List<NodeJson>();
    //}
}