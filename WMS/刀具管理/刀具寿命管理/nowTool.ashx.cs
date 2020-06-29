using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model.刀具管理.刀具寿命管理
{
    /// <summary>
    /// nowTool 的摘要说明
    /// </summary>
    public class nowTool : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var cncid = int.Parse(context.Request["cncId"]);
            using (JDJS_WMS_DB_USEREntities jdjs = new JDJS_WMS_DB_USEREntities())
            {
                string cncNum;
                string cncType;
                var cncinfo = jdjs.JDJS_WMS_Device_RealTime_Data.Where(r => r.ID == cncid);
                if(jdjs.JDJS_WMS_Device_Info.Where(r => r.ID == cncid).Count() > 0)
                {
                    cncNum = jdjs.JDJS_WMS_Device_Info.Where(r => r.ID == cncid).First().MachNum;
                    var typeId = jdjs.JDJS_WMS_Device_Info.Where(r => r.ID == cncid).First().MachType;
                    cncType = jdjs.JDJS_WMS_Device_Type_Info.Where(r => r.ID == typeId).First().Type;
                }
                else
                {
                     cncNum = "0";
                    cncType = "";
                }
                
                if (cncinfo.Count() > 0)
                {
                    int toolNo = Convert.ToInt32(cncinfo.First().ToolNo);
                    if (toolNo < 0)
                    {
                        toolNo = 0;
                    }
                    int Feed = Convert.ToInt32(cncinfo.First().SpindleSpeed);
                    if (Feed  < 0)
                    {
                        Feed  = 0;
                    }
                    

                    context.Response.Write(cncNum+","+toolNo + "," + Feed+"," +cncType);
                }
                else
                {
                    context.Response.Write(cncNum+",0"+",0" +","+ cncType);

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
}