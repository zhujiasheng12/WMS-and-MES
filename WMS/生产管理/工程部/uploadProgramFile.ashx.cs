﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using WebApplication2.生产管理.工程部.文件管理;

namespace WebApplication2.生产管理.工程部
{
    /// <summary>
    /// upload 的摘要说明
    /// </summary>
    public class uploadProgramFile : IHttpHandler,IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            var fileType = context.Request["fileType"];//"更新";"覆盖"
            bool isUpdate = true;

            if (fileType == "覆盖")
            {
                isUpdate = false;
            }
            int personId = int.Parse(context.Session["id"].ToString ());
            string personName = context.Session["UserName"].ToString ();
            var number = context.Request.Form[0];
            var processId = int.Parse(context.Request.Form[1]);
            var file = context.Request.Files;
            using (JDJS_WMS_DB_USEREntities entities = new JDJS_WMS_DB_USEREntities())
            {
                var row = entities.JDJS_WMS_Order_Process_Info_Table.Where(r => r.ID == processId).FirstOrDefault();

                var programName = row.programName;
               
                PathInfo pathInfo = new PathInfo();
                var root = pathInfo.upLoadPath();

                    var fileName = Path.GetFileNameWithoutExtension(programName)+ Path.GetExtension(file[0].FileName);
                var path = Path.Combine(root, number, "加工文件", fileName);
                var Folder = Path.Combine(root, number, "加工文件");

                if (System.IO.File.Exists(path))
                {
                    if (!isUpdate)
                    {
                        System.IO.File.Delete(path);
                    }
                    else
                    {
                        int i = 1;
                    string oldPath= Path.Combine(root, number, "加工文件", Path.GetFileNameWithoutExtension(programName) + "-" + i.ToString ()+ Path.GetExtension(file[0].FileName));
                        while (System.IO.File.Exists(oldPath))
                        {
                            i++;
                            oldPath = Path.Combine(root, number, "加工文件", Path.GetFileNameWithoutExtension(programName) + "-" + i.ToString() + Path.GetExtension(file[0].FileName));
                        }
                        System.IO.File.Move(path, oldPath);

                    }
                }
                string str = "";
                FileManage.UpdateFileToDB(Convert.ToInt32(row.OrderID), row.ID, personId, personName, FileType.加工文件, isUpdate, ref str);
                file[0].SaveAs(path);
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