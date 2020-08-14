using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace WebApplication2.Model.生产管理.市场部
{
    /// <summary>
    /// create 的摘要说明
    /// </summary>
    public class createIntention : IHttpHandler,IRequiresSessionState
    {

       
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            var form = context.Request.Form;
            var file = context.Request.Files;
            var projectName = form["projectName"];
            var intentionEndTime = Convert.ToDateTime(form["intentionEndTime"]);//预计评估完成时间
            var remark = form["remark"];//备注
            var loginUserId=Convert .ToInt32 ( context.Session["id"]);
            int priority = 1;//优先级
            // var folder = @"D:\服务器文件勿动\"+ form[0];
         
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
                using (System.Data.Entity.DbContextTransaction date = entities.Database.BeginTransaction())
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
                        if(int.Parse(form[0])==0)
                        {
                            var number = strOrderNum + flagStr;
                            var Customer = form[6];
                            var judge = entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_Number == number);
                            if (judge.Count() > 0)
                            {
                                context.Response.Write("该订单已存在");
                                return;
                            }
                            var row = new JDJS_WMS_Order_Entry_Table
                            {
                                Order_Number = strOrderNum + flagStr,
                                Order_Leader = form[2],
                                Product_Name = form[3],
                                Product_Material = form[4],
                                Product_Output = int.Parse(form[5]),
                                ProjectName =projectName ,
                                CtratPersonID =loginUserId  ,
                                Priority =priority ,
                                Intention = 5,//意向未提交
                                CreateTime = DateTime.Now,
                                CreatePersonID = loginUserId  ,
                                IntentionPlanEndTime =intentionEndTime ,
                                Remark =remark 

                            };

                            entities.JDJS_WMS_Order_Entry_Table.Add(row);
                            entities.SaveChanges();
                            var orderId = entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_Number == number).FirstOrDefault().Order_ID;

                            JDJS_WMS_Order_Guide_Schedu_Table guide = new JDJS_WMS_Order_Guide_Schedu_Table()
                            {
                                OrderID = orderId,
                                ClientName = Customer
                            };
                            entities.JDJS_WMS_Order_Guide_Schedu_Table.Add(guide);
                            entities.SaveChanges();

                            var newRow = new JDJS_WMS_Order_Intention_History_Table { OrderID = orderId ,flag =1,CreatPersonID   =loginUserId ,CreatTime =DateTime .Now,LastAlterPersonID =loginUserId ,LastAlterTime =DateTime .Now };
                            entities.JDJS_WMS_Order_Intention_History_Table.Add(newRow);

                            entities.SaveChanges();
                            //var orderNumber = form[0];
                            //var OrderID = entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_Number == orderNumber).First().Order_ID;
                            //var flag = new JDJS_WMS_Order_Queue_Table
                            //{
                            //    OrderID = OrderID,
                            //    isFlag = 0
                            //};
                            //entities.JDJS_WMS_Order_Queue_Table.Add(flag);
                            PathInfo pathInfo = new PathInfo();
                            var folder = Path.Combine(pathInfo.upLoadPath(), number,form[1], @"客供图纸");
                            if (!Directory.Exists(folder))
                            {
                                Directory.CreateDirectory(folder);
                            };

                            for (int i = 0; i < file.Count; i++)
                            {
                                var name = file[i].FileName;
                                var size = file[i].ContentLength;
                                string path = Path.Combine(folder, name);
                                file[i].SaveAs(path);

                            }
                            entities.SaveChanges();
                            context.Response.Write("ok");
                            date.Commit();

                        }
                        else
                        {
                          
                            {
                                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                {
                                    //var form = context.Request.Form;
                                    
                                    //var file = context.Request.Files;
                                    //var folder = @"D:\服务器文件勿动\"+ form[0];
                                  
                                  

                                 
                                    {
                                        {
                                            
                                            {
                                                PathInfo pathInfo = new PathInfo();
                                             
                                                var NewOrderNumber = strOrderNum + flagStr;
                                                string Owner = form[2];
                                                string OldOrderNumber = form[3];
                                                string ProductName = form[4];
                                                string ProductMater =form[5];
                                                int OutPut = int.Parse(form[6]);
                                                var Customer = form[7];
                                                var NewFolder = Path.Combine(pathInfo.upLoadPath(), NewOrderNumber);
                                                try
                                                {
                                                    var number = NewOrderNumber;

                                                    var judge = entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_Number == number);
                                                    if (judge.Count() > 0)
                                                    {
                                                        Console.WriteLine("该订单已存在");
                                                        return;
                                                    }
                                                    var oldVirPerson = entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_Number == OldOrderNumber).FirstOrDefault().virtualProgPersId;
                                                    var row = new JDJS_WMS_Order_Entry_Table
                                                    {
                                                        Order_Number = NewOrderNumber,
                                                        Order_Leader = Owner,
                                                        Product_Name = ProductName,
                                                        Product_Material = ProductMater,
                                                        Product_Output = OutPut,
                                                        virtualProgPersId=oldVirPerson,
                                                        ProjectName = projectName,
                                                        Intention = 6,
                                                        CtratPersonID = loginUserId,
                                                        Priority = priority,
                                                        CreateTime = DateTime.Now,
                                                        CreatePersonID = loginUserId ,
                                                        IntentionPlanEndTime = intentionEndTime,
                                                        Remark = remark
                                                    };

                                                    entities.JDJS_WMS_Order_Entry_Table.Add(row);
                                                    entities.SaveChanges();
                                                    var orderId = entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_Number == number).FirstOrDefault().Order_ID;

                                                    var newRow = new JDJS_WMS_Order_Intention_History_Table { OrderID = orderId };
                                                    entities.JDJS_WMS_Order_Intention_History_Table.Add(newRow);
                                                    var newOrder = entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_Number == NewOrderNumber && r.Intention == 6).FirstOrDefault().Order_ID;
                                                    JDJS_WMS_Order_Guide_Schedu_Table guide = new JDJS_WMS_Order_Guide_Schedu_Table()
                                                    {
                                                        OrderID = newOrder,
                                                        ClientName = Customer
                                                    };
                                                    entities.JDJS_WMS_Order_Guide_Schedu_Table.Add(guide);
                                                    entities.SaveChanges();
                                                    var oldOrder = entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_Number == OldOrderNumber).FirstOrDefault();
                                                    int oldID = oldOrder.Order_ID;
                                                    var oldprocess = entities.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == oldID && r.sign == 0);
                                                    foreach (var item in oldprocess)
                                                    {
                                                        JDJS_WMS_Order_Process_Info_Table info = new JDJS_WMS_Order_Process_Info_Table()
                                                        {
                                                            OrderID =newOrder ,
                                                            ProcessID =item.ProcessID ,
                                                            programName =item.programName ,
                                                            ProcessTime =item.ProcessTime ,
                                                            BlankNumber =item.BlankNumber ,
                                                            BlankSpecification =item.BlankSpecification ,
                                                            BlankType =item.BlankType ,
                                                            DeviceType =item.DeviceType ,
                                                            JigSpecification =item.JigSpecification ,
                                                            sign =0,
                                                            toolChartName =item.toolChartName ,
                                                            toolPreparation =item.toolPreparation 
                                                        };
                                                        entities.JDJS_WMS_Order_Process_Info_Table.Add(info);
                                                    }
                                                    entities.SaveChanges();


                                                    if (!Directory.Exists(NewFolder))
                                                    {
                                                        Directory.CreateDirectory(NewFolder);
                                                    };
                                                    List<string> srcPath = new List<string>();
                                                    srcPath.Add(pathInfo.upLoadPath() + @"\" + OldOrderNumber + @"\" + @"客供图纸\");
                                                    srcPath.Add(pathInfo.upLoadPath() + @"\" + OldOrderNumber + @"\" + @"虚拟加工方案文档\");
                                                    foreach (var real in srcPath)
                                                    {
                                                        copyDir(real, NewFolder);


                                                    }
                                                    context.Response.Write("ok");
                                                    date.Commit();

                                                }
                                                catch (Exception ex)
                                                {
                                                    date.Rollback();
                                                    context.Response.Write(ex.Message);
                                                }
                                            }
                                        }
                                    }
                                }
                                {
                                    //string OrderNum = "1";
                                    //List<string> Info = new List<string>();
                                    //if (OrderNum != null && OrderNum != "")
                                    //{
                                    //    using (WmsAutoEntities wms = new WmsAutoEntities())
                                    //    {
                                    //        var order = wms.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_Number == OrderNum);
                                    //        if (order.Count() > 0)
                                    //        {
                                    //            string name = order.First().Product_Name;
                                    //            string material = order.First().Product_Material;
                                    //            Info.Add(name);
                                    //            Info.Add(material);
                                    //        }
                                    //    }
                                    //}
                                    //var json = serializer.Serialize(Info);
                                }


                                {
                                    //List<string> OrderNum = new List<string>();
                                    //using (WmsAutoEntities wms = new WmsAutoEntities())
                                    //{
                                    //    var Intention = wms.JDJS_WMS_Order_Intention_History_Table.ToList();
                                    //    foreach (var item in Intention)
                                    //    {
                                    //        int id = Convert.ToInt32(item.OrderID);
                                    //        var order = wms.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == id);
                                    //        var str = order.First().Order_Number;
                                    //        OrderNum.Add(str);
                                    //    }
                                    //}
                                    //var json = serializer.Serialize(OrderNum);
                                }

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        date.Rollback();
                        context.Response.Write(ex.Message);
                    }
                }
            }
        }


        public static bool copyDir(string srcPath, string aimPath)
        {
            try
            {
                if (Directory.Exists(aimPath))
                {
                    Directory.CreateDirectory(aimPath);
                }
                //目标路径
                string srcdir = Path.Combine(aimPath, Path.GetFileName(srcPath));
                if (Directory.Exists(srcPath))
                {
                    DirectoryInfo di = new DirectoryInfo(srcPath);

                    srcdir = Path.Combine(srcdir, di.Name);
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
                        copyDir(element, srcdir);
                    }
                    else
                    {
                        File.Copy(element, srcdir + @"\" + Path.GetFileName(element), true);
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