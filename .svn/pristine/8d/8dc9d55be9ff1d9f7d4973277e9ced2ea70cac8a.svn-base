using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model.生产管理.市场部
{
    /// <summary>
    /// editRead 的摘要说明
    /// </summary>
    public class editRead : IHttpHandler
    {
        

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            var id = int.Parse(context.Request["id"]);
            using(JDJS_WMS_DB_USEREntities entities=new JDJS_WMS_DB_USEREntities())
            {
                var row = entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == id).First();
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                var ClientName = "";
                var guide = entities.JDJS_WMS_Order_Guide_Schedu_Table.Where(r => r.OrderID == id).FirstOrDefault();
                if (guide != null) {
                    ClientName = guide.ClientName;
                }
                List<EditRead> reads = new List<EditRead>();
                reads.Add(new EditRead {Order_Number=row.Order_Number,
                Order_Leader=row.Order_Leader,Product_Name=row.Product_Name,
                Product_Material=row.Product_Material,
                Product_Output=row.Product_Output.ToString (),
                Order_Plan_End_Time=row.Order_Plan_End_Time.ToString(),
                Order_State=row.Order_State,id=row.Order_ID,
                pattern=row.ProofingORProduct.ToString(),
                ProjectName=row.ProjectName,
                  ClientName = ClientName,
                  Remark =row.Remark ==null?"":row.Remark ,
                    IntentionPlanEndTime=row.IntentionPlanEndTime ==null?DateTime .Now .ToString ("yyyy-MM-dd HH:mm:ss:fff") :Convert .ToDateTime(row.IntentionPlanEndTime).ToString ("yyyy-MM-dd HH:mm:ss:fff")

                });

                var json = serializer.Serialize(reads);
                context.Response.Write(json);
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
    class EditRead
    {
       public  string Order_Number;
        public string Order_Leader;
        public string Product_Name;
        public string Product_Material;
        public string Product_Output;
        public string Order_Plan_End_Time;
        public string Order_State;
        public string pattern;
        public int id;
        public string ProjectName;
        public string ClientName;
        public string Remark;
        public string IntentionPlanEndTime;
    }
}