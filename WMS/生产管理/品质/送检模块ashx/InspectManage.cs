using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.品质.送检模块ashx
{
    public class InspectManage
    {
        public static List<InspectInfo> GetAllInspectInfo()
        {
            List<InspectInfo> infos = new List<InspectInfo>();
            using (JDJS_WMS_DB_USEREntities model = new JDJS_WMS_DB_USEREntities())
            {
                var inspects = model.JDJS_WMS_Quality_Apply_Measure_Table;
                foreach (var item in inspects)
                {
                    InspectInfo info = new InspectInfo();
                    info.AcceptDate = item.AcceptTime == null ? "" : Convert.ToDateTime(item.AcceptTime).Year.ToString() + "-" + Convert.ToDateTime(item.AcceptTime).Month.ToString() + "-" + Convert.ToDateTime(item.AcceptTime).Day.ToString() + " " + Convert.ToDateTime(item.AcceptTime).Hour.ToString() + "时";
                    info.AcceptPersonId = Convert.ToInt32(item.AcceptPersonId == null ? "0" : item.AcceptPersonId.ToString());
                    info.AcceptTime = item.AcceptTime == null ? DateTime.Now : Convert.ToDateTime(item.AcceptTime);
                    info.AccpetPersonName = item.AcceptPersonName == null ? "" : item.AcceptPersonName;
                    info.ApplyDate = item.ApplyTime == null ? "" : Convert.ToDateTime(item.ApplyTime).Year.ToString() + "-" + Convert.ToDateTime(item.ApplyTime).Month.ToString() + "-" + Convert.ToDateTime(item.ApplyTime).Day.ToString() + " " + Convert.ToDateTime(item.ApplyTime).Hour.ToString() + "时";
                    info.ApplyTime = item.ApplyTime == null ? DateTime.Now : Convert.ToDateTime(item.ApplyTime);
                    info.ApplyPersonId = item.ApplyPersonId == null ? 0 : Convert.ToInt32(item.ApplyPersonId);
                    info.ApplyPersonName = item.ApplyPersonName == null ? "" : item.ApplyPersonName;
                    info.ApplyPersonTel = item.ApplyPersonTel == null ? "" : item.ApplyPersonTel;
                    info.DepartmentName = item.DepartmentName == null ? "" : item.DepartmentName;
                    info.Id = item.ID;
                    info.InspectDate = item.InspectTime == null ? "" : Convert.ToDateTime(item.InspectTime).Year.ToString() + "-" + Convert.ToDateTime(item.InspectTime).Month.ToString() + "-" + Convert.ToDateTime(item.InspectTime).Day.ToString() + " " + Convert.ToDateTime(item.InspectTime).Hour.ToString() + "时";
                    info.InspectNum = item.WorkPieceNum == null ? 0 : Convert.ToInt32(item.WorkPieceNum);
                    info.InspectTime = item.InspectTime == null ? DateTime.Now : Convert.ToDateTime(item.InspectTime);
                    info.IsHaveRepaot = item.IsHaveReport == 1 ? true : false;
                    info.MeasureContent = item.MeasureContent == null ? "" : item.MeasureContent;
                    info.MeasureDate = item.MeasureTime == null ? "" : Convert.ToDateTime(item.MeasureTime).Year.ToString() + "-" + Convert.ToDateTime(item.MeasureTime).Month.ToString() + "-" + Convert.ToDateTime(item.MeasureTime).Day.ToString() + " " + Convert.ToDateTime(item.MeasureTime).Hour.ToString() + "时";
                    info.MeasureDevice = item.MeasureDevice == null ? "" : item.MeasureDevice;
                    info.MeasureGoal = item.MeasureGoal == null ? "" : item.MeasureGoal;
                    info.MeasureResult = item.MeasureResult == null ? "" : item.MeasureResult;
                    info.MeasureTime = item.MeasureTime == null ? DateTime.Now : Convert.ToDateTime(item.MeasureTime);
                    info.Remark = item.Remark == null ? "" : item.Remark;
                    info.State = item.State == null ? "" : item.State;
                    info.WorkPieceName = item.WorkPieceName == null ? "" : item.WorkPieceName;
                    infos.Add(info);
                }
            }
            return infos;
        }
    }
    /// <summary>
    /// 送检信息
    /// </summary>
    public struct InspectInfo
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 申请日期
        /// </summary>
        public string ApplyDate { get; set; }
        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime ApplyTime { get; set; }
        /// <summary>
        /// 送检日期
        /// </summary>
        public string InspectDate { get; set; }
        /// <summary>
        /// 送检时间
        /// </summary>
        public DateTime InspectTime { get; set; }
        /// <summary>
        /// 工件名称
        /// </summary>
        public string WorkPieceName { get; set; }
        /// <summary>
        /// 测量内容
        /// </summary>
        public string MeasureContent { get; set; }
        /// <summary>
        /// 测量目的
        /// </summary>
        public string MeasureGoal { get; set; }
        /// <summary>
        /// 送检部门
        /// </summary>
        public string DepartmentName { get; set; }
        /// <summary>
        /// 送检人员
        /// </summary>
        public string ApplyPersonName { get; set; }
        /// <summary>
        /// 送检人员id
        /// </summary>
        public int ApplyPersonId { get; set; }
        /// <summary>
        /// 送检人员联系方式
        /// </summary>
        public string ApplyPersonTel { get; set; }
        /// <summary>
        /// 送检数量
        /// </summary>
        public int InspectNum { get; set; }
        /// <summary>
        /// 接受日期
        /// </summary>
        public string AcceptDate { get; set; }
        /// <summary>
        /// 接受时间
        /// </summary>
        public DateTime AcceptTime { get; set; }
        /// <summary>
        /// 接受人ID
        /// </summary>
        public int AcceptPersonId { get; set; }
        /// <summary>
        /// 接受人
        /// </summary>
        public string AccpetPersonName { get; set; }
        /// <summary>
        /// 测量设备
        /// </summary>
        public string MeasureDevice { get; set; }
        /// <summary>
        /// 测量日期
        /// </summary>
        public string MeasureDate { get; set; }
        /// <summary>
        /// 测量时间
        /// </summary>
        public DateTime MeasureTime { get; set; }
        /// <summary>
        /// 测量结果
        /// </summary>
        public string MeasureResult { get; set; }
        /// <summary>
        /// 是否有检测报告
        /// </summary>
        public bool IsHaveRepaot { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 送检状态
        /// </summary>
        public string State { get; set; }

    }
}