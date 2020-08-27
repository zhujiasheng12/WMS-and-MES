using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model.生产管理.工程部
{
    /// <summary>
    /// ncRead 的摘要说明
    /// </summary>
    public class ncRead : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            var id = int.Parse(context.Request["id"]);
            List<Nc> ncs = new List<Nc>();
            using(JDJS_WMS_DB_USEREntities entities=new JDJS_WMS_DB_USEREntities())
            {
                var rows = entities.JDJS_WMS_Order_Process_Info_Table.Where(r=>r.OrderID==id&r.sign!=0);
                if (rows.Count() > 0)
                {
                    foreach (var item in rows)
                    {
                        var processId = item.ID;
                        int JigNumber = 0;
                        if (entities.JDJS_WMS_Order_Fixture_Manager_Table.Where(r => r.ProcessID == processId).FirstOrDefault() != null)
                        {
                            JigNumber =Convert.ToInt32( entities.JDJS_WMS_Order_Fixture_Manager_Table.Where(r => r.ProcessID == processId).First().FixtureNumber);
                        }
                        int flag = 0;
                        var schedu = entities.JDJS_WMS_Order_Process_Scheduling_Table.Where(r => r.ProcessID == processId && r.isFlag != 0);
                        if (schedu.Count() > 0)
                        {
                            flag = 1;
                        }
                        var BlankType = "";
                        if (item.BlankType.ToString() == "1")
                        {
                            BlankType = "板料";
                        }
                        if (item.BlankType.ToString() == "2")
                        {
                            BlankType = "块料";
                        }
                        if (item.BlankType.ToString() == "3")
                        {
                            BlankType = "其他";
                        }
                        //var fixtureType = "";
                        //if (item.JigType.ToString() == "1")
                        //{
                        //    fixtureType = "真空吸盘";
                        //}
                        //else if (item.JigType.ToString() == "2")
                        //{
                        //    fixtureType = "零点快换";
                        //}else if (item.JigType.ToString() == "3")
                        //{
                        //    fixtureType = "台钳";
                        //}
                        //else
                        //{
                        //    fixtureType = "其他";
                        //}
                        int fixID = Convert.ToInt32(item.JigType);
                        string fixName = "";
                        var fixTable = entities.JDJS_WMS_Device_Status_Table.Where(r => r.ID == fixID).FirstOrDefault();
                        if (fixTable != null)
                        {
                            fixName = fixTable.Status + "(" + fixTable.explain + ")";
                        }
                        var orderNumber = entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == id).First().Order_Number;
                        string deviceType = "";
                        if (entities.JDJS_WMS_Device_Type_Info .Where(r => r.ID == item.DeviceType).FirstOrDefault() != null)
                        {
                             deviceType = entities.JDJS_WMS_Device_Type_Info .Where(r => r.ID == item.DeviceType).First().Type;
                        }

                        ncs.Add(new Nc
                        {
                            workNumber =(item.ProcessID)==null ? "" : item.ProcessID.ToString (),
                            programName = item.programName == null ? "" : item.programName.ToString(),
                            toolChartName = item.toolChartName == null ? "" : item.toolChartName.ToString(),
                            programTime = item.ProcessTime == null ? "" : item.ProcessTime.ToString(),
                            deviceType = deviceType,
                            fixName=fixName ,
                            BlankSpecification =item.BlankSpecification==null? "": item.BlankSpecification.Replace("#1#", ""),
                            BlankNumber =item.BlankNumber==null? "": item.BlankNumber.ToString(),
                            fixtureType = fixName,
                            JigSpecification =item.JigSpecification==null? "": item.JigSpecification.Replace ("#1#",""),
                            id = item.ID.ToString(),
                            flag = flag.ToString () ,
                            BlankType = BlankType,
                            JigNumber = JigNumber.ToString(),
                            coefficient = item.Modulus==null?"":item.Modulus.ToString(),
                            toolPrepare = item.toolPreparation == null ? "" : item.toolPreparation.ToString(),
                            nonCuttingTime=item.NonCuttingTime.ToString()

                        }); ;
                    };
                    var model = new { code = 0, msg = "", count = ncs.Count, data = ncs };
                    var json = serializer.Serialize(model);
                    context.Response.Write(json);
                }
                else
                {
                   
                
                    context.Response.Write("{\"code\":0,\"msg\":\"\",\"count\":1,\"data\":[]}");
                  
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
    class Nc
    {
        public string workNumber;
        public string programName;
        public string fixName;
        public string programTime;
        public string deviceType;
        public string  BlankType;
        public string BlankSpecification;
        public string  BlankNumber;
        public string JigSpecification;
        public string toolChartName;
        public string id;
        public string JigNumber;
        public string flag;
        public string fixtureType;
        public string coefficient;//
        public string toolPrepare;
        public string nonCuttingTime;
    }
}