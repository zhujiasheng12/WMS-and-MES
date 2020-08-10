﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.SessionState;
using System.Web.UI.WebControls;

namespace WebApplication2.生产管理.品质.送检模块ashx
{
    /// <summary>
    /// 处理送检申请 的摘要说明
    /// </summary>
    public class 处理送检申请 : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                var loginID = Convert.ToInt32(context.Session["id"]);
                int id =int.Parse ( context.Request["id"]);//处理的申请主键ID
                string result = context.Request["result"];//‘接受’|’未接受‘
                string remark = context.Request["remark"];//备注
                string personName = "";
                string state = Enum.GetName(typeof(InspectStateType), 1);
                if (result == "未接受")
                {
                    state = Enum.GetName(typeof(InspectStateType), 3);
                }

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
                    if (inspect.State != Enum.GetName(typeof(InspectStateType), 0))
                    {
                        context.Response.Write("该测量记录状态不符，请确认后再试！");
                        return;
                    }
                    using (System.Data.Entity.DbContextTransaction mytran = model.Database.BeginTransaction())
                    {
                        try
                        {
                            inspect.AcceptPersonId = loginID;
                            inspect.AcceptPersonName = personName;
                            inspect.AcceptTime = DateTime.Now;
                            string str = inspect.Remark == null ? "" : inspect.Remark;
                            inspect.Remark = str + "<br/>" + personName + "：" + DateTime.Now.ToString() + "<br/>" + remark;
                            inspect.State = state;
                            model.SaveChanges();
                            mytran.Commit();
                            PathInfo pathInfo = new PathInfo();
                            bool z = true;
                            SendTextToWechat sendTextToWechat = new CompanyWeChatRobotRemind();
                            sendTextToWechat.SendText(personName+ result +inspect .ApplyPersonName + "提交的工件："+inspect .WorkPieceName+"的送检申请。备注为"+remark+ "。点击http://3x196w2589.qicp.vip/M/appointList.html", pathInfo.GetQualituInspectAuditRobot(), new List<string>(), out z);
                            context.Response.Write("ok");
                            return;
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