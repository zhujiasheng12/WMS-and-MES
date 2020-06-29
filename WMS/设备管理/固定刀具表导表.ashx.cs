using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApplication2.设备管理
{
    /// <summary>
    /// 固定刀具表导表 的摘要说明
    /// </summary>
    public class 固定刀具表导表 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try {

           
            var file = context.Request.Files;
            var form = context.Request.Form;
            var cncTypeId =int.Parse(form["cncTypeId"]);
            var timeNow = (DateTime.Now.ToString()+"-"+ DateTime.Now.Millisecond.ToString()).Replace ("/","-").Replace(@"\", "-").Replace (":","-");
            PathInfo pathInfo = new PathInfo();
           
            var directoryPath = Path.Combine(pathInfo.upLoadPath(), "固定刀具表", cncTypeId.ToString());
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            string extension = Path.GetExtension(file[0].FileName);
            var filePath = Path.Combine(directoryPath, "固定刀具表"+extension);

            if (File.Exists(Path.Combine(directoryPath, "固定刀具表.xls"))| File.Exists(Path.Combine(directoryPath, "固定刀具表.xlsx"))| File.Exists(Path.Combine(directoryPath, "固定刀具表.csv")))
            {
                var i = 1;
                while(File.Exists(Path.Combine(directoryPath, "固定刀具表_"+i+".xls"))| File.Exists(Path.Combine(directoryPath, "固定刀具表_" + i + ".xlsx"))| File.Exists(Path.Combine(directoryPath, "固定刀具表_" + i + ".csv")))
                {
                    i++;
                }
                if (File.Exists(Path.Combine(directoryPath, "固定刀具表.xls")))
                {
                    File.Move(Path.Combine(directoryPath, "固定刀具表.xls"),Path.Combine(directoryPath, "固定刀具表_" + i + ".xls"));
                }
                if (File.Exists(Path.Combine(directoryPath, "固定刀具表.xlsx")))
                {
                    File.Move(Path.Combine(directoryPath, "固定刀具表.xlsx"), Path.Combine(directoryPath, "固定刀具表_" + i + ".xlsx"));
                }
                if (File.Exists(Path.Combine(directoryPath, "固定刀具表.csv")))
                {
                    File.Move(Path.Combine(directoryPath, "固定刀具表.csv"), Path.Combine(directoryPath, "固定刀具表_" + i + ".csv"));
                }




            }
            file[0].SaveAs(filePath);

            Function function = new Function();
           context.Response.Write( function.ImportExcel(cncTypeId, filePath));
            }
            catch(Exception ex)
            {
                context.Response.Write(ex.Message);
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