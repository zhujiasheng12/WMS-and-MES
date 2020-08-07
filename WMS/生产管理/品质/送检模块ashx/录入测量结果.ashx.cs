using DocumentFormat.OpenXml.Drawing.Diagrams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace WebApplication2.生产管理.品质.送检模块ashx
{
    /// <summary>
    /// 录入测量结果 的摘要说明
    /// </summary>
    public class 录入测量结果 : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                int id = int.Parse(context.Request["id"]);//处理的申请主键ID
                string measureDevice = context.Request["measureDevice"];//测量仪器
                string measureResult= context.Request["measureResult"];//测量结果
                int isHaveReport = int.Parse(context.Request["isHaveReport"]);//是否提供检测报告 "1"|‘0’
                string remark = context.Request["remark"];//备注
                var files = context.Request.Files;//检测报告
                int loginID = Convert.ToInt32(context.Session["id"]);
                string personName = "";

                using (JDJS_WMS_DB_USEREntities model = new JDJS_WMS_DB_USEREntities())
                {
                    var person = model.JDJS_WMS_Staff_Info.Where(r => r.id == loginID).FirstOrDefault();
                    if (person != null)
                    {
                        personName = person.staff;
                    }
                    var inspect = model.JDJS_WMS_Quality_Apply_Measure_Table.Where(r => r.ID == id).FirstOrDefault();
                    if (inspect == null)
                    {
                        context.Response.Write("该送检申请不存在，请确认后再试！");
                        return;
                    }
                    if (inspect.State != Enum.GetName(typeof(InspectStateType), 1))
                    {
                        context.Response.Write("该测量记录状态不符，请确认后再试！");
                        return;
                    }
                    using (System.Data.Entity.DbContextTransaction mytran = model.Database.BeginTransaction())
                    {
                        try
                        {
                            inspect.IsHaveReport = isHaveReport;
                            inspect.MeasureDevice = measureDevice;
                            inspect.MeasurePersonId = loginID;
                            inspect.MeasurePersonName = personName;
                            inspect.MeasureResult = measureResult;
                            inspect.MeasureTime = DateTime.Now;
                            string str = inspect.Remark == null ? "" : inspect.Remark;
                            inspect.Remark = str + "<br/>" + personName + "：" + DateTime.Now.ToString() + "<br/>" + remark;
                            model.SaveChanges();
                            mytran.Commit();
                            if (isHaveReport == 1 && files != null)
                            {
                                var path = inspect.SavePath;
                                if (!System.IO.Directory.Exists(path))
                                {
                                    System.IO.Directory.CreateDirectory(path);
                                }
                                for (int i = 0; i < files.Count; i++)
                                {
                                    files[i].SaveAs(System.IO.Path.Combine(path, files[i].FileName));
                                }

                            }
                            context.Response.Write("ok");
                            return;
                        }
                        catch (Exception ex)
                        {
                            context.Response.Write(ex.Message);
                            return;
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                context.Response.Write(ex.Message);
                return;
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