using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.工程部.文件管理
{
    public class FileManage
    {
        /// <summary>
        /// 更新文件
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <param name="processId">工序Id</param>
        /// <param name="personId">员工id</param>
        /// <param name="personName">员工姓名</param>
        /// <param name="fileType">文件类型</param>
        /// <param name="isUpdate">是否更新</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public static bool UpdateFileToDB(int orderId, int processId, int personId, string personName, FileType fileType, bool isUpdate, ref string errMsg)
        {
            try
            {
                string fileTypeStr = fileType.ToString();
                using (JDJS_WMS_DB_USEREntities model = new JDJS_WMS_DB_USEREntities())
                {
                    int max = 0;
                    if (model.JDJS_WMS_Order_Files_Manage_Table.Where(r => r.OrderId == orderId && r.ProcessId == processId && r.FileType == fileTypeStr).Count() > 0)
                    {
                        max = int.Parse(model.JDJS_WMS_Order_Files_Manage_Table.Where(r => r.OrderId == orderId && r.ProcessId == processId && r.FileType == fileTypeStr).Max<JDJS_WMS_Order_Files_Manage_Table>(r => r.VersonNum).ToString());
                    }
                    int verson = max;
                    string str = "覆盖";
                    if (isUpdate)
                    {
                        verson++;
                        str = "更新";
                    }
                    JDJS_WMS_Order_Files_Manage_Table jdf = new JDJS_WMS_Order_Files_Manage_Table()
                    {
                        OrderId = orderId,
                        CreateTime = DateTime.Now,
                        FileType = fileTypeStr,
                        PersonId = personId,
                        PersonName = personName,
                        ProcessId = processId,
                        VersonNum = verson,
                        UpdateOrCover =str
                    };
                    using (System.Data.Entity.DbContextTransaction mytran = model.Database.BeginTransaction())
                    {
                        try
                        {
                            model.JDJS_WMS_Order_Files_Manage_Table.Add(jdf);
                            model.SaveChanges();
                            mytran.Commit();
                            errMsg = "ok";
                            return true;
                        }
                        catch (Exception ex)
                        {
                            mytran.Rollback();
                            errMsg = ex.Message;
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return false;
            }
        }

        public static List<StaffFileInfo> GetStaffFileInfo(DateTime startTime, DateTime endTime)
        {
            List<StaffFileInfo> infos = new List<StaffFileInfo>();
            try
            {
                using (JDJS_WMS_DB_USEREntities model = new JDJS_WMS_DB_USEREntities())
                {
                    var files = model.JDJS_WMS_Order_Files_Manage_Table.Where(r => r.CreateTime > startTime && r.CreateTime < endTime);
                    foreach (var item in files)
                    {
                        int personId =Convert.ToInt32 ( item.PersonId);
                        var oldStaff = infos.Where(r => r.StaffId == personId).FirstOrDefault();
                        if (oldStaff == null)
                        {
                            StaffFileInfo info = new StaffFileInfo();
                            info.StaffId = personId;
                            info.StaffName = item.PersonName;
                            info.AllFileNum = 1;
                            info.AllCoverNum = 0;
                            info.AllUpdateNum = 0;
                            info.FileInfo = new List<string>();
                            info.FileInfo.Add(item.OrderId.ToString() + "_" + item.ProcessId.ToString() + "_" + item.FileType);
                            if (item.UpdateOrCover == "更新")
                            {
                                info.AllUpdateNum++;
                            }
                            else if (item.UpdateOrCover == "覆盖")
                            {
                                info.AllCoverNum++;
                            }
                            infos.Add(info);
                        }
                        else
                        {
                            string str = item.OrderId.ToString() + "_" + item.ProcessId.ToString() + "_" + item.FileType;
                            if (oldStaff.FileInfo.Contains(str))
                            {
                                if (item.UpdateOrCover == "更新")
                                {
                                    oldStaff.AllUpdateNum++;
                                }
                                else if (item.UpdateOrCover == "覆盖")
                                {
                                    oldStaff.AllCoverNum++;
                                }
                            }
                            else
                            {
                                oldStaff.FileInfo.Add(str);
                                oldStaff.AllFileNum++;
                                if (item.UpdateOrCover == "更新")
                                {
                                    oldStaff.AllUpdateNum++;
                                }
                                else if (item.UpdateOrCover == "覆盖")
                                {
                                    oldStaff.AllCoverNum++;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
            return infos;
        }
    }

    public class StaffFileInfo
    { 
        public int StaffId { get; set; }
        public string StaffName { get; set; }
        public List<string> FileInfo { get; set; }
        public int AllFileNum { get; set; }
        public int AllUpdateNum { get; set; }
        public int AllCoverNum { get; set; }
    }

    public enum FileType
    {
        其它文件,
        加工文件,
        路径工艺单
    }
}