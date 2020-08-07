using DocumentFormat.OpenXml.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace WebApplication2.生产管理.品质.送检模块ashx
{
    /// <summary>
    /// 送检预约申请 的摘要说明
    /// </summary>
    public class 送检预约申请 : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                DateTime inspectTime = Convert.ToDateTime(context.Request["inspectTime"]);//送检时间
                string workpieceName = context.Request["workpieceName"];//工件名称
                string measureContent= context.Request["measureContent"];//测量内容
                string measureGoal= context.Request["measureGoal"];//测量目的
                string departmentName= context.Request["departmentName"];//送检部门
                string applyPersonName= context.Request["applyPersonName"];//送检人员名称
                int workpieceNum = int.Parse(context.Request["applyPersonName"]);//送检数量

                string applyPersonTel = context.Request["applyPersonTel"] == null ? "" : context.Request["applyPersonTel"];//送检人员联系方式,可以不给
                var loginID = Convert.ToInt32(context.Session["id"]==null?"0": context.Session["id"]);
                DateTime applyTime = DateTime.Now;//申请时间
                string path = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() + "-" + DateTime.Now.Hour.ToString() + "-" + DateTime.Now.Minute.ToString() + "-" + DateTime.Now.Second.ToString() + "-" + DateTime.Now.Millisecond.ToString();
                PathInfo pathInfo = new PathInfo();
                path = System.IO.Path.Combine(pathInfo.upLoadPath(), @"送检模块", path);
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                using (JDJS_WMS_DB_USEREntities model = new JDJS_WMS_DB_USEREntities())
                {
                    using (System.Data.Entity.DbContextTransaction mytran = model.Database.BeginTransaction())
                    {
                        try
                        {
                            JDJS_WMS_Quality_Apply_Measure_Table jd = new JDJS_WMS_Quality_Apply_Measure_Table()
                            {
                                ApplyPersonId = loginID,
                                ApplyPersonName = applyPersonName,
                                ApplyPersonTel = applyPersonTel,
                                ApplyTime = applyTime,
                                DepartmentName = departmentName,
                                InspectTime = inspectTime,
                                MeasureContent = measureContent,
                                MeasureGoal = measureGoal,
                                WorkPieceName = workpieceName,
                                WorkPieceNum = workpieceNum,
                                State = Enum.GetName(typeof(InspectStateType), 0),
                                AcceptPersonName = "",
                                Remark = "",
                                SavePath =path
                            };
                            model.JDJS_WMS_Quality_Apply_Measure_Table.Add(jd);
                            model.SaveChanges();
                            mytran.Commit();
                            bool z = true;
                            SendTextToWechat sendTextToWechat = new CompanyWeChatRobotRemind();
                            sendTextToWechat.SendText(departmentName + "：" + applyPersonName + "新建送检申请，工件名称：" + workpieceName + "。工件数量：" + workpieceNum.ToString() + "。预计送检时间：" + inspectTime.ToShortDateString() + "。请尽快处理！", pathInfo.GetQualituInspectApplyRobot(), new List<string>(), out z);
                            context.Response.Write("ok");
                        }
                        catch (Exception ex)
                        {
                            mytran.Rollback();
                            context.Response.Write(ex.Message);
                            return;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                context.Response.Write(ex.Message );
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