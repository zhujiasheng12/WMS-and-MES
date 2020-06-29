using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model.刀具寿命管理
{
    /// <summary>
    /// PositionHandler 的摘要说明
    /// </summary>
    public class PositionHandler1 : IHttpHandler
    {
        int flag = -1;

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
                AddNodes(jsons, JDJS_WMS_Device_Info.JDJS_WMS_Location_Info.ToList(), JDJS_WMS_Device_Info.JDJS_WMS_Device_Info.ToList(), JDJS_WMS_Device_Info.JDJS_WMS_Tool_LifeTime_Management.ToList()); ;

            }
            NodeState(jsons);
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
        public void AddNodes(List<NodeJson> jsons, List<JDJS_WMS_Location_Info> nodes, List<JDJS_WMS_Device_Info> deviceNodes, List<JDJS_WMS_Tool_LifeTime_Management> ToolInfo)
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
                                json.children.Add(new NodeJson() { id = item.id, name = item.Name, team = item.EndDate, hierarchy = item.size, flag = -1 });
                            }
                            foreach (var item in DevicesubNodes)
                            {
                                var toollist = ToolInfo.Where(r => r.CncID == item.ID);
                                int error = 0;
                                foreach (var real in toollist)
                                {

                                    if (real.ToolCurrTime > real.ToolMaxTime * 0.8 && real.ToolCurrTime < real.ToolMaxTime && real.ToolMaxTime > 0)
                                    {
                                        error = 1;
                                        json.flag = 1;

                                    }
                                    if (real.ToolCurrTime >= real.ToolMaxTime && real.ToolMaxTime > 0)
                                    {
                                        error = 2;
                                        json.flag = 2;
                                        break;
                                    }
                                }
                                json.children.Add(new NodeJson() { id = flag, CncID = item.ID, name = item.MachNum, team = item.IP, hierarchy = item.MachState, flag = error });
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
}
    //public class Node
    //{
    //    public int id;
    //    public string Name;
    //    public string  size;
    //    public string  EndDate;
    //    public int parentId;
    //}

    