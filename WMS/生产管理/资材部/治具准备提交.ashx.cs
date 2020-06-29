using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.资材部
{
    /// <summary>
    /// 治具准备提交 的摘要说明
    /// </summary>
    public class 治具准备提交 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var processId = int.Parse(context.Request["processId"]);
            var number = int.Parse(context.Request["number"]);
            using(JDJS_WMS_DB_USEREntities entities=new JDJS_WMS_DB_USEREntities())
            {
                using (System.Data.Entity.DbContextTransaction mytran = entities.Database.BeginTransaction())
                {
                    try
                    {
                        var process = entities.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == processId).FirstOrDefault();
                        if (process != null)
                        {

                            int jigtype = Convert.ToInt32(process.JigType);
                            if (jigtype == 4)
                            {
                                for (int i = 0; i < number; i++)
                                {


                                    JDJS_WMS_Quickchangbaseplate_Table quickchangbaseplate_Table = new JDJS_WMS_Quickchangbaseplate_Table()
                                    {
                                        time = DateTime.Now,
                                        flag = 0,
                                        Type = 4,

                                    };
                                    entities.JDJS_WMS_Quickchangbaseplate_Table.Add(quickchangbaseplate_Table);
                                    string numstr = "";
                                    var jigs = entities.JDJS_WMS_Quickchangbaseplate_Table.ToList ().LastOrDefault() ;
                                    if (jigs != null)
                                    {
                                        numstr =(Convert.ToInt32( jigs.ID)+1).ToString();
                                        while (numstr.Length < 4)
                                        {
                                            numstr = numstr.Insert(0, "0");
                                        }
                                        //Show the DLL version
                                        PathInfo path = new PathInfo();
                                        string damaMachName = @path.DaMaMachName();
                                        damaMachName = damaMachName.Replace("\\\\", "++++");
                                        damaMachName = damaMachName.Replace("\\", @"\");
                                        damaMachName = damaMachName.Replace("++++", "\\");
                                        TSCLIB_DLL.openport(damaMachName); //注意修改打印机名称，需要和资材部电脑连接的打码机一致                                          //Open specified printer driver
                                        TSCLIB_DLL.setup("40", "10", "4", "8", "0", "2", "0");        //设置标签大小格式                   //Setup the media size and sensor type info
                                        TSCLIB_DLL.clearbuffer();                                                           //Clear image buffer
                                        TSCLIB_DLL.windowsfont(15, 5, 20, 0, 0, 0, "宋体", "治具:JD-04-" + numstr);  //Draw windows font
                                        TSCLIB_DLL.barcode("20", "30", "128", "45", "1", "0", "2", "2", "JD-04-" + numstr); //Drawing barcode
                                        TSCLIB_DLL.printlabel("1", "1");                                                    //Print labels
                                        TSCLIB_DLL.closeport();
                                    }
                                }

                            }
                            var row = entities.JDJS_WMS_Order_Fixture_Manager_Table.Where(r => r.ProcessID == processId);
                            if (row.Count() > 0)
                            {
                                row.FirstOrDefault().FixtureFinishPerpareNumber = number;

                            }

                            JDJS_WMS_Fixture_Additional_History_Table fix = new JDJS_WMS_Fixture_Additional_History_Table()
                            {
                                ProcessID = processId,
                                AddNum = number,
                                AddTime = DateTime.Now
                            };
                            entities.JDJS_WMS_Fixture_Additional_History_Table.Add(fix);
                            entities.SaveChanges();
                            mytran.Commit();
                            context.Response.Write("ok");
                        }
                        else
                        {
                            context.Response.Write("该工序不存在");
                        }
                    }
                    catch(Exception ex)
                    {
                        mytran.Rollback();
                        context.Response.Write(ex.Message);
                        return;
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