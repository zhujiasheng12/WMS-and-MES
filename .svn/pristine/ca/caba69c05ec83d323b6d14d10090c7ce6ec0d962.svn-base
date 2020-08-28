using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.生产管理.资材部.夹具管理.特殊治具管理Access数据库;

namespace WebApplication2.生产管理.资材部.夹具管理.录入系统治具库
{
    /// <summary>
    /// 修改系统治具 的摘要说明
    /// </summary>
    public class 修改系统治具 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                int id = int.Parse(context.Request["id"]);//治具种类id
                int typeId = int.Parse(context.Request["typeId"]);//治具种类id
                string name = context.Request["name"];//治具名称//唯一
                string venderName = context.Request["venderName"];//厂家名称，默认给"JD"
                string desc = context.Request["desc"];//描述
                string remark = context.Request["remark"];//备注
                int currCount = int.Parse(context.Request["currCount"]);//当前库存
                int allCount = int.Parse(context.Request["allCount"]);//库存总量
                var file = context.Request.Files;//文件
                using (FixtureModel model = new FixtureModel())
                {
                    var fx = model.JDJS_WMS_Fixture_System_Table.Where(r => r.Name == name&&r.Id !=id).FirstOrDefault();
                    if (fx != null)
                    {
                        context.Response.Write("该治具名称已存在！");
                        return;
                    }
                    fx = model.JDJS_WMS_Fixture_System_Table.Where(r => r.Id == id).FirstOrDefault();
                    using (System.Data.Entity.DbContextTransaction mytran = model.Database.BeginTransaction())
                    {
                        try
                        {
                            fx.AlterTime = DateTime.Now;
                            fx.Desc = desc;
                            fx.Name = name;
                            fx.Remark = remark;
                            fx.StockAllNum = allCount;
                            fx.StockCurrNum = currCount;
                            fx.TypeId = typeId;
                            fx.VenderName = venderName;
                            string fileName = fx.FileName;
                            if (file != null && file.Count > 0)
                            {
                                
                                PathInfo info1 = new PathInfo();
                                if (System.IO.File.Exists(System.IO.Path.Combine(info1.GetFixtrue_SurfMillFilePath(), fileName)))
                                {
                                    System.IO.File.Delete(System.IO.Path.Combine(info1.GetFixtrue_SurfMillFilePath(), fileName));
                                }
                                for (int i = 0; i < file.Count; i++)
                                {
                                    file[i].SaveAs(System.IO.Path.Combine(info1.GetFixtrue_SurfMillFilePath(), file[i].FileName));
                                    fileName = file[i].FileName;
                                }
                            }
                            model.SaveChanges();
                            mytran.Commit();
                            using (JDJS_WMS_DB_USEREntities wms = new JDJS_WMS_DB_USEREntities())
                            {
                                var status = wms.JDJS_WMS_Device_Status_Table.Where(r => r.SystemId == id).FirstOrDefault();
                                if (status != null)
                                {
                                    status.Status = name;
                                    status.explain = desc;
                                }
                                wms.SaveChanges();
                            }
                            PathInfo info = new PathInfo();
                            for (int i = 0; i < file.Count; i++)
                            {
                                file[i].SaveAs(System.IO.Path.Combine(info.GetFixtrue_SurfMillFilePath(), file[i].FileName));
                            }
                            string str = "";
                            Fixture_SurfMill.AlterChildJIG(name, desc, fileName, allCount.ToString(), currCount.ToString(), ref str, venderName, fx.SerialCode.ToString());
                            context.Response.Write(str);
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