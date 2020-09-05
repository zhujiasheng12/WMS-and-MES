﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using WebApplication2.生产管理.工程部.文件管理;

namespace WebApplication2.Model.生产管理.工程部
{
    /// <summary>
    /// editFileP 的摘要说明
    /// </summary>
    public class editFileP : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            var file = context.Request.Files;
            var processId = int.Parse(context.Request.Form["processId"]);
            var fileType = context.Request["fileType"];//"更新";"覆盖"
            bool isUpdate = true;
            if (fileType == "覆盖")
            {
                isUpdate = false;
            }
            int personId = int.Parse(context.Session["id"].ToString());
            string personName = context.Session["UserName"].ToString();
            using (JDJS_WMS_DB_USEREntities entities=new JDJS_WMS_DB_USEREntities())
            {
                var row = entities.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == processId).First();
                var orderNumber = entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == row.OrderID).First().Order_Number;

               var exten= Path.GetExtension(file[0].FileName);
                var fileName = orderNumber + "-P" + row.ProcessID + exten;
                var oldFileName = row.programName;

                PathInfo pathInfo = new PathInfo();
               
                DirectoryInfo directoryP = new DirectoryInfo(Path.Combine(pathInfo.upLoadPath(), orderNumber, "加工文件"));
                if (!directoryP.Exists)
                {
                    directoryP.Create();
                }

                DirectoryInfo directoryT = new DirectoryInfo(Path.Combine(pathInfo.upLoadPath(), orderNumber, "刀具表"));
                if (!directoryT.Exists)
                {
                    directoryT.Create();
                }
                var path = Path.Combine(pathInfo.upLoadPath(), orderNumber,"加工文件", fileName);

                if (System.IO.File.Exists(path))
                {
                    if (!isUpdate)
                    {
                        System.IO.File.Delete(path);
                    }
                    else
                    {
                        int i = 1;
                        string oldPath = Path.Combine(pathInfo.upLoadPath(), orderNumber, "加工文件", Path.GetFileNameWithoutExtension(row.programName) +"-" +i.ToString() + Path.GetExtension(file[0].FileName));
                        while (System.IO.File.Exists(oldPath))
                        {
                            i++;
                            oldPath = Path.Combine(pathInfo.upLoadPath(), orderNumber, "加工文件", Path.GetFileNameWithoutExtension(row.programName) + "-" + i.ToString() + Path.GetExtension(file[0].FileName));
                        }
                        System.IO.File.Move(path, oldPath);

                    }
                }

                file[0].SaveAs(path);
                string str = "";
                FileManage.UpdateFileToDB(Convert.ToInt32(row.OrderID), row.ID, personId, personName, FileType.加工文件, isUpdate, ref str);
                row.programName = fileName;
                entities.SaveChanges();
                context.Response.Write(str);
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