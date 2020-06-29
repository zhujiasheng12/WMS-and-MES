using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace WebApplication2.生产管理.市场部
{
    /// <summary>
    /// 意向订单编辑提交 的摘要说明
    /// </summary>
    public class 意向订单编辑提交 : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            var loginUserID = Convert.ToInt32(context.Session["id"]);
            var loginName = context.Session["UserName"];

            int priorityNew = Convert.ToInt32(context.Request.Form["Priority"]);
            int priorityOld = 1;

            using (JDJS_WMS_DB_USEREntities entities = new JDJS_WMS_DB_USEREntities())
            {
                using (System.Data.Entity.DbContextTransaction db = entities.Database.BeginTransaction())
                {

                    try
                    {
                        var form = context.Request.Form;

                        var intention = form["intention"];
                        var orderId = int.Parse(form["orderId"]);
                        var Order_Number = form["Order_Number"];
                        var Order_Leader = form["Order_Leader"];
                        var Product_Name = form["Product_Name"];
                        var Product_Material = form["Product_Material"];
                        var Product_Output = int.Parse(form["Product_Output"]);
                        var ProjectName = form["ProjectName"];
                        var ClientName = form["ClientName"];
                        var rows = entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_Number == Order_Number);
                        foreach (var item in rows)
                        {
                            if (item.Order_ID != orderId)
                            {
                                context.Response.Write("该订单已存在");
                                return;
                            }
                        }
                        var row1 = entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == orderId);
                        priorityOld =Convert.ToInt32 ( row1.FirstOrDefault().Priority);
                        if (priorityNew != priorityOld && priorityNew != 0)
                        {
                            var person = entities.JDJS_WMS_Staff_Info.Where(r => r.id == loginUserID).FirstOrDefault();
                            if (person != null)
                            {
                                if (!person.limit.Split(',').Contains("优先级修改"))
                                {
                                    row1.FirstOrDefault().Priority = priorityNew;
                                }
                            }

                            if (row1.Count() > 0)
                            {
                                if (row1.FirstOrDefault().Order_Leader == loginName || row1.FirstOrDefault().CtratPersonID == loginUserID)
                                {
                                    //context.Response.Write(String.Format("该订单负责人为{0}，当前登录用户没有修改订单权限！", loginName));
                                    if (intention == "5")
                                    {

                                        row1.FirstOrDefault().Order_Number = Order_Number;
                                        row1.FirstOrDefault().Order_Leader = Order_Leader;
                                        row1.FirstOrDefault().Product_Name = Product_Name;
                                        row1.FirstOrDefault().Product_Material = Product_Material;
                                        row1.FirstOrDefault().Product_Output = Product_Output;
                                        row1.FirstOrDefault().ProjectName = ProjectName;
                                        var guide = entities.JDJS_WMS_Order_Guide_Schedu_Table.Where(r => r.OrderID == orderId).FirstOrDefault();
                                        if (guide != null) {
                                            guide.ClientName = ClientName;
                                        }
                                        entities.SaveChanges();
                                        context.Response.Write("ok");

                                    }
                                    else if (intention == "6")
                                    {

                                        row1.FirstOrDefault().Order_Number = Order_Number;
                                        row1.FirstOrDefault().Order_Leader = Order_Leader;

                                        row1.FirstOrDefault().Product_Output = Product_Output;
                                        row1.FirstOrDefault().Product_Output = Product_Output;
                                        row1.FirstOrDefault().ProjectName = ProjectName;
                                        var guide = entities.JDJS_WMS_Order_Guide_Schedu_Table.Where(r => r.OrderID == orderId).FirstOrDefault();
                                        if (guide != null)
                                        {
                                            guide.ClientName = ClientName;
                                        }
                                        entities.SaveChanges();
                                        context.Response.Write("ok");
                                    }
                                    else if (intention == "1")
                                    {


                                        row1.FirstOrDefault().Product_Output = Product_Output;

                                        entities.SaveChanges();
                                        context.Response.Write("ok");
                                    }
                                    JDJS_WMS_Order_Alter_History_Table jd = new JDJS_WMS_Order_Alter_History_Table()
                                    {
                                        AlterDecs = "修改优先级",
                                        CreatPersonID = loginUserID,
                                        CreatTime = DateTime.Now,
                                        OrderID = orderId
                                    };
                                    entities.JDJS_WMS_Order_Alter_History_Table.Add(jd);
                                }
                            }
                            
                        }
                        else
                        {
                            if (row1.Count() > 0)
                            {
                                if (row1.FirstOrDefault().Order_Leader != loginName && row1.FirstOrDefault().CtratPersonID != loginUserID)
                                {
                                    context.Response.Write(String.Format("该订单负责人为{0}，当前登录用户没有修改订单权限！", row1.FirstOrDefault().Order_Leader));
                                    return;
                                }
                            }
                            if (intention == "5")
                            {
                                var row = entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == orderId);
                                row.FirstOrDefault().Order_Number = Order_Number;
                                row.FirstOrDefault().Order_Leader = Order_Leader;
                                row.FirstOrDefault().Product_Name = Product_Name;
                                row.FirstOrDefault().Product_Material = Product_Material;
                                row.FirstOrDefault().Product_Output = Product_Output;
                                row.FirstOrDefault().Product_Output = Product_Output;
                                row.FirstOrDefault().ProjectName = ProjectName;
                                var guide = entities.JDJS_WMS_Order_Guide_Schedu_Table.Where(r => r.OrderID == orderId).FirstOrDefault();
                                if (guide != null)
                                {
                                    guide.ClientName = ClientName;
                                }
                                entities.SaveChanges();
                                context.Response.Write("ok");

                            }
                            else if (intention == "6")
                            {
                                var row = entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == orderId);
                                row.FirstOrDefault().Order_Number = Order_Number;
                                row.FirstOrDefault().Order_Leader = Order_Leader;

                                row.FirstOrDefault().Product_Output = Product_Output;
                                row.FirstOrDefault().Product_Output = Product_Output;
                                row.FirstOrDefault().ProjectName = ProjectName;
                                var guide = entities.JDJS_WMS_Order_Guide_Schedu_Table.Where(r => r.OrderID == orderId).FirstOrDefault();
                                if (guide != null)
                                {
                                    guide.ClientName = ClientName;
                                }
                                entities.SaveChanges();
                                context.Response.Write("ok");
                            }
                            else if (intention == "1")
                            {
                                var row = entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == orderId);

                                row.FirstOrDefault().Product_Output = Product_Output;

                                entities.SaveChanges();
                                context.Response.Write("ok");
                            }
                            JDJS_WMS_Order_Alter_History_Table jd = new JDJS_WMS_Order_Alter_History_Table()
                            {
                                AlterDecs = "修改订单数量",
                                CreatPersonID = loginUserID,
                                CreatTime = DateTime.Now,
                                OrderID = orderId
                            };
                            entities.JDJS_WMS_Order_Alter_History_Table.Add(jd);
                        }

                        db.Commit();
                    }
                    catch (Exception ex)
                    {
                        db.Rollback();
                        context.Response.Write(ex.Message);
                    }
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