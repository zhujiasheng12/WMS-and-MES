using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
namespace WebApplication2.生产管理.市场部
{
    /// <summary>
    /// pushDown 的摘要说明
    /// </summary>
    public class pushDown : IHttpHandler,IRequiresSessionState
    {
        System.Web.Script.Serialization.JavaScriptSerializer Serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        public void ProcessRequest(HttpContext context)
        {
            var loginUserID=Convert.ToInt32 ( context.Session["id"]);
            var loginName = context.Session["UserName"];
          
            string str = context.Request.Form["flag"];//下推or撤销
            using (JDJS_WMS_DB_USEREntities entities = new JDJS_WMS_DB_USEREntities())
            {
                var form = context.Request.Form;
                for (int i = 0; i < form.Count-1; i++)
                {
                    var orderId = int.Parse(form[i]);
                    if (str == "下推")
                    {
                        var output = Convert.ToInt32(context.Request.Form["Product_Output"]);
                        var pattern = int.Parse(context.Request.Form["pattern"]);
                        var row = entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == orderId);
                        if (row.FirstOrDefault().Order_Leader != loginName && row.FirstOrDefault().CtratPersonID != loginUserID)
                        {
                            context.Response.Write(String.Format("该订单负责人为{0}，当前登录用户没有修改订单权限！", row.FirstOrDefault().Order_Leader));
                            return;

                        }
                        int outputOld = row.First().Product_Output;
                        string pattonOld =(Convert.ToInt32 ( row.First().ProofingORProduct))==-1?"大批量":"小批量";
                        string pattonNew = (pattern) == -1 ? "大批量" : "小批量";
                        int rowID = row.FirstOrDefault().Order_ID;
                        if (row.First().Intention == 3)
                        {
                            context.Response.Write("已下推");
                            return;
                        }
                        row.First().AuditResult = "待审核";
                        row.First().Intention = 3;
                        row.First().Product_Output = output;
                        row.First().ProofingORProduct = pattern;
                        row.First().Engine_Program_ManagerId = null;
                        var virtualProgPersId = row.First().virtualProgPersId;
                        if (entities.JDJS_WMS_Staff_Info.Where(r => r.id == virtualProgPersId).Count() == 0)
                        {
                            context.Response.Write("虚拟方案未完成不可下推");
                            return;
                        }
                        var name = entities.JDJS_WMS_Staff_Info.Where(r => r.id == virtualProgPersId).First().staff;
                        row.First().Engine_Program_Manager = null;

                        var his = entities.JDJS_WMS_Order_Intention_History_Table.Where(r => r.OrderID == rowID).FirstOrDefault();
                        if (his != null)
                        {
                            his.CraftPersonID = virtualProgPersId;
                            his.LastAlterPersonID = loginUserID;
                            his.LastAlterTime = DateTime.Now;
                        }
                        JDJS_WMS_Order_Alter_History_Table jd = new JDJS_WMS_Order_Alter_History_Table()
                        {
                            AlterDecs =String .Format ( "下推订单,订单数量由{0}修改为{}，{2}修改为{3}",outputOld ,output ,pattonOld ,pattonNew ),
                            CreatPersonID = loginUserID,
                            CreatTime = DateTime.Now,
                            OrderID = orderId
                        };
                        entities.JDJS_WMS_Order_Alter_History_Table.Add(jd);
                    }
                    else
                    {
                        var row = entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == orderId);
                        int rowID = row.FirstOrDefault().Order_ID;
                        if (row.First().Intention == -2)
                        {
                            context.Response.Write("已撤销");
                            return;
                        }
                        row.First().Intention = -2;
                        var virtualProgPersId = row.First().virtualProgPersId;
                        
                        var name = entities.JDJS_WMS_Staff_Info.Where(r => r.id == virtualProgPersId).First().staff;
                        JDJS_WMS_Order_Alter_History_Table jd = new JDJS_WMS_Order_Alter_History_Table()
                        {
                            AlterDecs = "撤销订单",
                            CreatPersonID = loginUserID,
                            CreatTime = DateTime.Now,
                            OrderID = orderId
                        };
                        entities.JDJS_WMS_Order_Alter_History_Table.Add(jd);
                    }
                }
                entities.SaveChanges();
            }

            context.Response.Write("ok");
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