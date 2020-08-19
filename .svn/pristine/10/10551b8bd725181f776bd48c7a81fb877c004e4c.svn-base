using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.维修部.精度校验
{
    /// <summary>
    /// 确认提交 的摘要说明
    /// </summary>
    public class 确认提交 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            int PlanID = int.Parse(context.Request["MaintenacneID"]);//计划表中的主键ID
            int staffID = int.Parse(context.Request["staffId"]);//员工中的主键ID
            var data = Convert.ToDouble(context.Request["data"]);
            using (JDJS_WMS_DB_USEREntities  wms = new JDJS_WMS_DB_USEREntities ())
            {
                using (System.Data.Entity.DbContextTransaction mytran = wms.Database.BeginTransaction())
                {
                    try
                    {
                        var plan = wms.JDJS_WMS_Device_Accuracy_Verification_Plan.Where(r => r.ID == PlanID && r.isFlag == 1);
                        if (plan.Count() > 0)
                        {
                            foreach (var item in plan)
                            {
                                item.AccuracyVerificationTime = DateTime.Now;
                            }

                            wms.SaveChanges();
                            JDJS_WMS_Device_Accuracy_Verification_Data confirm = new JDJS_WMS_Device_Accuracy_Verification_Data()
                            {
                                RecordTime = DateTime.Now,
                                PlanID = PlanID,
                                StaffID = staffID,
                                Data=data
                            };
                            wms.JDJS_WMS_Device_Accuracy_Verification_Data.Add(confirm);
                            wms.SaveChanges();
                            
                        }
                        mytran.Commit();
                    }
                    catch
                    {
                        mytran.Rollback();
                    }
                }
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