using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.工程部
{
    /// <summary>
    /// 手动排产读机床 的摘要说明
    /// </summary>
    public class 手动排产读机床 : IHttpHandler
    {


        int flag = -1;

        public void ProcessRequest(HttpContext context)
        {
            //         "id":11,
            //"name":"Java",
            //"size":"",
            //"date":"01/13/2010",
            var cncType = context.Request["cncType"];
            context.Response.ContentType = "text/plain";
            List<NodeJson> jsons = new List<NodeJson>();
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            using (JDJS_WMS_DB_USEREntities JDJS_WMS_Device_Info = new JDJS_WMS_DB_USEREntities())
            {
                var TypeID = JDJS_WMS_Device_Info.JDJS_WMS_Device_Type_Info.Where(r => r.Type == cncType).FirstOrDefault();
                if (TypeID != null)
                {

                    AddNodes(jsons, JDJS_WMS_Device_Info.JDJS_WMS_Location_Info.ToList(), JDJS_WMS_Device_Info.JDJS_WMS_Device_Info.Where(r => r.MachType == TypeID.ID).ToList(), JDJS_WMS_Device_Info.JDJS_WMS_Tool_LifeTime_Management.ToList(), cncType); ;
                }
            }
            //NodeState(jsons);
            for (int i = 0; i < jsons.Count(); i++)
            {
                if (jsons[i].children.Count() > 1)
                {

                    jsons[i].children = paixu(jsons[i].children);
                }
            }
            string json = serializer.Serialize(jsons);
            context.Response.Write(json);

        }
        public static List<NodeJson> paixu(List<NodeJson> jsons)
        {
            jsons = jsons.OrderBy(r => r.dateTime).ToList();
            for (int i = 0; i < jsons.Count(); i++)
            {
                if (jsons[i].children.Count() > 1)
                {

                    jsons[i].children = paixu(jsons[i].children);
                }
            }
            return jsons;
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
        public void AddNodes(List<NodeJson> jsons, List<JDJS_WMS_Location_Info> nodes, List<JDJS_WMS_Device_Info> deviceNodes, List<JDJS_WMS_Tool_LifeTime_Management> ToolInfo, string cncType)
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
                    AddNodes(jsons, nodes, deviceNodes, ToolInfo, cncType);
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
                                int deviceID = item.ID;
                                DateTime time = DateTime.Now;
                                using (JDJS_WMS_DB_USEREntities  wms = new  JDJS_WMS_DB_USEREntities ())
                                {
                                    var process = wms.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.CncID == deviceID && r.isFlag != 0).ToList();
                                    if (process.Count() > 0)
                                    {
                                        process = process.OrderByDescending(r => r.EndTime).ToList();
                                        time = Convert.ToDateTime(process.First().EndTime);
                                    }

                                }
                                //if (item.MachState == cncType)
                                {
                                    json.children.Add(new NodeJson() { id = flag, CncID = item.ID, name = item.MachNum, team = item.IP, hierarchy = item.MachState, dateTime = time, time = time.Year.ToString()+"/"+time.Month.ToString() + "/" + time.Day.ToString()+"  " + time.Hour.ToString()+"时" }); ;
                                    flag--;
                                }


                            }
                            AddNodes(json.children, nodes, deviceNodes, ToolInfo, cncType);
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
        public DateTime dateTime;
        public string time;
        public List<NodeJson> children = new List<NodeJson>();
    }
}
