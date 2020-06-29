using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.资材部
{
    /// <summary>
    /// 获取工单信息 的摘要说明
    /// </summary>
    public class 获取工单信息 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var orderId = Convert.ToInt32(context.Request.Form["orderId"]);
            string Product_Name = "";
            string Order_Number = "";
            string Product_Output = "";

            string ProjectName = "";
            string ClientName = "";
            string Order_Leader = "";
            string craftPerson = "";
            string Product_Material = "";
            var BlankSpecification = "";
            try
            {
                using (JDJS_WMS_DB_USEREntities entities = new JDJS_WMS_DB_USEREntities())
                {
                    var row = from order in entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == orderId)
                              from client in entities.JDJS_WMS_Order_Guide_Schedu_Table
                              from blank in entities.JDJS_WMS_Order_Blank_Table
                              where client.OrderID == order.Order_ID & blank.OrderID == order.Order_ID
                              select new
                              {
                                  order.Product_Name,
                                  order.Order_Number,
                                  order.Product_Output,
                                  order.ProjectName,
                                  client.ClientName,
                                  order.Order_Leader,
                                  order.craftPerson,
                                  blank.BlankSpecification,
                                  order.Product_Material
                              };
                    if (row.Count() > 0)
                    {
                        var rows = entities.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == orderId);
                        List<Process> process = new List<Process>();
                        if (rows.Count() > 0)
                        {
                          

                            foreach (var item in rows)
                            {
                                var tool = "无";
                                var deviceType = entities.JDJS_WMS_Device_Type_Info.Where(r => r.ID == item.DeviceType).FirstOrDefault();
                                var jig = entities.JDJS_WMS_Device_Status_Table.Where(r => r.ID == item.JigType).FirstOrDefault();
                                var tools = entities.JDJS_WMS_Order_Process_Tool_Info_Table.Where(r => r.ProcessID == item.ProcessID);
                                foreach (var too in tools)
                                {
                                    var toolNum = "T" + too.ToolNO;
                                    if (entities.JDJS_WMS_Tool_Standard_Table.Where(r => r.MachTypeID == item.DeviceType & r.ToolID == toolNum).Count() == 0)
                                    {
                                        tool = "有";
                                        break;
                                    }
                                }
                                process.Add(new Process
                                {
                                    processNum = item.ProcessID.ToString(),
                                    DeviceType = deviceType == null ? "" : deviceType.Type,
                                    JigSpecification = jig == null ? "" : jig.Status,
                                    tool = tool
                                });
                            }

                        }
                        var model = new
                        {
                            Product_Name = row.First(). Product_Name,
                            Order_Number = row.First().Order_Number,
                            Product_Output = row.First().Product_Output,

                            ProjectName = row.First().ProjectName,
                            ClientName = row.First().ClientName,
                            Order_Leader = row.First().Order_Leader,
                            craftPerson = row.First().craftPerson,
                            Product_Material = row.First().Product_Material,
                            BlankSpecification=row.First().BlankSpecification,
                            process = process
                        };
                        System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var json = serializer.Serialize(model);
                        context.Response.Write(json);
                    }
                }
            }
            catch (Exception ex)
            {
                context.Response.Write(ex.Message);
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
    class Process
    {
        public string processNum;
        public string DeviceType;
        public string JigSpecification;
        public string tool;
    }



}