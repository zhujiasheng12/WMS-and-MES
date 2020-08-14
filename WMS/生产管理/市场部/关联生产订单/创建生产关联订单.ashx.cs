using DocumentFormat.OpenXml.Drawing.Charts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace WebApplication2.生产管理.市场部.关联生产订单
{
    /// <summary>
    /// 创建生产关联订单 的摘要说明
    /// </summary>
    public class 创建生产关联订单 : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            int orderId = int.Parse(context.Request["orderId"]);//关联的订单主键ID
            int output = int.Parse(context.Request["output"]);//订单需求量
            string Order_Leader = context.Request["Order_Leader"];//订单负责人
            string Product_Name = context.Request["Product_Name"];//产品名称
            string Project_Name = context.Request["Project_Name"];//项目名称
            string Order_State = context.Request["Order_State"];//订单状态
            var patternStr = (context.Request["pattern"] == null ? "-100" : context.Request["pattern"]);
            int pattern = int.Parse(patternStr);//生产模式
            int loginID = Convert.ToInt32(context.Session["id"]);
            DateTime Order_Plan_End_Time = Convert.ToDateTime(context.Request["Order_Plan_End_Time"]);//计划结束时间
            var Order_Plan_Start_Time = DateTime.Now;
            var Customer = context.Request["Customer"];//客户名称
            var remark = context.Request["remark"];
            #region 订单号
            string year = DateTime.Now.Year.ToString().Substring(2, 2);
            string month = DateTime.Now.Month.ToString();
            while (month.Length < 2)
            {
                month = month.Insert(0, "0");
            }
            string day = DateTime.Now.Day.ToString();
            while (day.Length < 2)
            {
                day = day.Insert(0, "0");
            }
            string strOrderNum = year + month + day;
            int flag = 1;
            string flagStr = "";
            using (JDJS_WMS_DB_USEREntities entities = new JDJS_WMS_DB_USEREntities())
            {


                try
                {
                    while (true)
                    {
                        flagStr = flag.ToString();
                        while (flagStr.Length < 5)
                        {
                            flagStr = flagStr.Insert(0, "0");
                        }

                        var order = entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_Number == strOrderNum + flagStr);
                        if (order.Count() < 1)
                        {
                            break;
                        }
                        flag++;
                    }
                }
                catch(Exception ex)
                {
                    context.Response.Write(ex.Message);
                    return;
                }
                var OldOrder = entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == orderId).FirstOrDefault();
                if (OldOrder == null)
                {
                    context.Response.Write("被关联订单不存在，请确认后再试！");
                    return;
                }
                var clientInfo = entities.JDJS_WMS_Order_Guide_Schedu_Table.Where(r => r.OrderID == orderId).FirstOrDefault();
                Order_Leader = Order_Leader == null ? OldOrder.Order_Leader : Order_Leader;
                Product_Name= Product_Name==null? OldOrder.Product_Name : Product_Name;
                Project_Name= Project_Name==null? OldOrder.ProjectName : Project_Name;
                Order_State = Order_State == null ? OldOrder.Order_State : Order_State;
                Customer = Customer == null ?(clientInfo==null?"无": clientInfo.ClientName) : Customer;
                pattern = pattern == -100 ? ((int)(OldOrder.ProofingORProduct==null?-1: OldOrder.ProofingORProduct)) : pattern;
                using (System.Data.Entity.DbContextTransaction date = entities.Database.BeginTransaction())
                {
                    try
                    {
                        JDJS_WMS_Order_Entry_Table orderentry = new JDJS_WMS_Order_Entry_Table()
                        {
                            Order_Number = strOrderNum + flagStr,
                            Order_Leader = Order_Leader,
                            Product_Name = Product_Name,
                            Product_Material = OldOrder.Product_Material,
                            Product_Output = output,
                            Order_Plan_Start_Time = Order_Plan_Start_Time,
                            Order_Actual_Start_Time = null,
                            Order_Actual_End_Time = null,
                            Order_Plan_End_Time = Order_Plan_End_Time,
                            Engine_Program_Manager = OldOrder.Engine_Program_Manager,
                            Engine_Technology_Manager = OldOrder.Engine_Technology_Manager,
                            Engine_Program_ManagerId = OldOrder.Engine_Program_ManagerId,
                            Engine_Technology_ManagerId = OldOrder.Engine_Technology_ManagerId,
                            Engine_Status = "未进行",
                            Intention = 6,
                            ProjectName = Project_Name,
                            CreateTime = DateTime.Now,
                            CreatePersonID = loginID,
                            Order_State = Order_State,
                            virtualProgPersId = OldOrder.virtualProgPersId,
                            virtualReturnTime = OldOrder.virtualReturnTime,
                            AuditResult ="待审核",
                            CtratPersonID = loginID,
                            //audit_Result = OldOrder.audit_Result,
                            craftPerson = OldOrder.craftPerson,
                            Priority = 1,
                            craftPersonId = OldOrder.craftPersonId,
                            ProofingORProduct =pattern ,
                            program_audit_result = null,
                            ParentId =orderId,
                            Remark =remark 
                        };
                        entities.JDJS_WMS_Order_Entry_Table.Add(orderentry);
                        entities.SaveChanges();
                        int orderIdNew = orderentry.Order_ID;
                        JDJS_WMS_Order_Guide_Schedu_Table guide = new JDJS_WMS_Order_Guide_Schedu_Table()
                        {
                            OrderID = orderIdNew,
                            ClientName = Customer
                        };

                        entities.JDJS_WMS_Order_Guide_Schedu_Table.Add(guide);
                        entities.SaveChanges();
                        var queue = new JDJS_WMS_Order_Queue_Table { OrderID = orderIdNew, isFlag = 3 };
                        entities.JDJS_WMS_Order_Queue_Table.Add(queue);
                        entities.SaveChanges();
                        date.Commit();

                        PathInfo pathInfo = new PathInfo();
                        var NewFolder = Path.Combine(pathInfo.upLoadPath(), strOrderNum + flagStr);
                        var OldFolder = Path.Combine(pathInfo.upLoadPath(), OldOrder.Order_Number);


                        List<string> srcPath = new List<string>();

                        string[] src = Directory.GetFileSystemEntries(OldFolder);
                        foreach (var item in src)
                        {
                            copyDir(item, NewFolder, strOrderNum + flagStr);
                        }

                        context.Response.Write("ok");
                        return;

                    }
                    catch (Exception ex)
                    {
                        date.Rollback();
                        context.Response.Write(ex.Message);
                        return;
                    }
                }


                }
            #endregion


        }

        /// <summary>
        /// 复制文件夹
        /// </summary>
        /// <param name="srcPath">源路径</param>
        /// <param name="aimPath">目标路径</param>
        /// <returns></returns>
        public static bool copyDir(string srcPath, string aimPath, string NewOrderNum)
        {
            try
            {
                if (!Directory.Exists(aimPath))
                {
                    Directory.CreateDirectory(aimPath);
                }
                //目标路径
                string srcdir = Path.Combine(aimPath, Path.GetFileName(srcPath));
                if (Directory.Exists(srcPath))
                {
                    DirectoryInfo di = new DirectoryInfo(srcPath);

                    //srcdir = Path.Combine(srcdir, di.Name);
                }
                if (!Directory.Exists(srcdir))
                {
                    Directory.CreateDirectory(srcdir);
                }
                string[] files = Directory.GetFileSystemEntries(srcPath);
                foreach (var element in files)
                {
                    if (Directory.Exists(element))
                    {
                        copyDir(element, srcdir, NewOrderNum);
                    }
                    else
                    {
                        string str = Path.GetFileName(element);
                        if (str.Contains("-P"))
                        {
                            string[] strs = str.Split('-');
                            if (strs.Length == 2)
                            {
                                str = NewOrderNum + "-" + strs[1];
                                System.IO. File.Copy(element, srcdir + @"\" + str, true);
                            }
                            else if (strs.Length == 3)
                            {
                                str = "T" + "-" + NewOrderNum + "-" + strs[2];
                                System.IO.File.Copy(element, srcdir + @"\" + str, true);
                            }
                        }
                        else
                        {
                            System.IO.File.Copy(element, srcdir + @"\" + Path.GetFileName(element), true);
                        }
                    }
                }
                return true;
            }
            catch
            {
                return false;
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