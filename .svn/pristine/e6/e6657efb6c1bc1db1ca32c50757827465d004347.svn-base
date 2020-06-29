using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace WebApplication2.Model.生产管理.工程部
{
    /// <summary>
    /// edit 的摘要说明
    /// </summary>
    public class edit : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                context.Response.ContentType = "text/plain";
                var form = context.Request.Form;
                var numberId = int.Parse(form[0]);
                var progId = int.Parse(form[1]);
                var techId = int.Parse(form[2]);
                var progIdOld = int.Parse(form[3]);
                var techIdOld = int.Parse(form[4]);
                List<EditRead> editReads = new List<EditRead>();
                using (JDJS_WMS_DB_USEREntities entities = new JDJS_WMS_DB_USEREntities())
                {
                    if (progId != progIdOld)
                    {
                        var row = entities.JDJS_WMS_Staff_Info.Where(r => r.id == progId).First();
                        if (row.orderNumberId == null)
                        {
                            row.orderNumberId = numberId.ToString();
                        }
                        else
                        {
                            var arr = row.orderNumberId.Split(',');
                            List<List> lists = new List<List>();
                            foreach (var item in arr)
                            {
                                lists.Add(new List { key = item });
                            }
                            var ss = lists.Where(r => r.key == numberId.ToString());
                            if (ss.Count() > 0)
                            {

                            }
                            else
                            {
                                row.orderNumberId += "," + numberId.ToString();
                            }

                        }
                        if (progIdOld != 0)
                        {
                            var rowOld = entities.JDJS_WMS_Staff_Info.Where(r => r.id == progIdOld).First();
                            var arr = rowOld.orderNumberId.Split(',');
                            List<List> lists = new List<List>();
                            foreach (var item in arr)
                            {
                                if (item != "")
                                {
                                    lists.Add(new List { key = item });
                                }
                                
                            }
                            var ss = lists.Where(r => r.key == numberId.ToString());
                            if (ss.Count() > 0)
                            {
                                lists.RemoveAll(r => r.key == numberId.ToString());
                            }
                            string span="";
                            foreach (var item in lists)
                            {
                                span += item.key;
                                span += ",";
                            }
                           var index= span.LastIndexOf(",");
                            if (index == -1)
                            {
                                rowOld.orderNumberId = "";
                            }
                            else
                            {
                                span.Remove(index);
                                rowOld.orderNumberId = span;
                            }
                           

                           
                        }

                    }

                    if (techId != techIdOld)
                    {
                        var row = entities.JDJS_WMS_Staff_Info.Where(r => r.id == techId).First();
                        if (row.orderNumberId == null)
                        {
                            row.orderNumberId = numberId.ToString();
                        }
                        else
                        {
                            var arr = row.orderNumberId.Split(',');
                            List<List> lists = new List<List>();
                            foreach (var item in arr)
                            {
                                if (item != "")
                                {
                                    lists.Add(new List { key = item });
                                }
                            }
                            var ss = lists.Where(r => r.key == numberId.ToString());
                            if (ss.Count() > 0)
                            {

                            }
                            else
                            {
                                row.orderNumberId += "," + numberId.ToString();
                            }

                        }
                        if (techIdOld != 0)
                        {
                            var rowOld = entities.JDJS_WMS_Staff_Info.Where(r => r.id == techIdOld).First();
                            var arr = rowOld.orderNumberId.Split(',');
                            List<List> lists = new List<List>();
                            foreach (var item in arr)
                            {
                                if (item != "")
                                {
                                    lists.Add(new List { key = item });
                                }
                            }
                            var ss = lists.Where(r => r.key == numberId.ToString());
                            if (ss.Count() > 0)
                            {
                                lists.RemoveAll(r => r.key == numberId.ToString());
                            }
                            string span = "";
                            foreach (var item in lists)
                            {
                                span += item.key;
                                span += ",";
                            }
                            var index = span.LastIndexOf(",");
                            if (index == -1)
                            {
                                rowOld.orderNumberId = "";
                            }
                            else
                            {
                                span.Remove(index);
                                rowOld.orderNumberId = span;
                            }
                        }

                    }




                    entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == numberId).First().Engine_Program_Manager = 
                        entities.JDJS_WMS_Staff_Info.Where(r=>r.id==progId).First().staff;
                    entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == numberId).First().Engine_Technology_Manager =
                        entities.JDJS_WMS_Staff_Info.Where(r => r.id == techId).First().staff;
                    entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == numberId).First().Engine_Program_ManagerId = progId;

                    entities.JDJS_WMS_Order_Entry_Table.Where(r => r.Order_ID == numberId).First().Engine_Technology_ManagerId = techId;
                    entities.SaveChanges();
                    context.Response.Write("ok");
                }




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
    class List
    {
        public string key;
    }
}