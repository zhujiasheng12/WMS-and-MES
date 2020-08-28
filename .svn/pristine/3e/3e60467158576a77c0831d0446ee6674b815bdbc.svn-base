using DocumentFormat.OpenXml.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.生产管理.资材部.夹具管理.特殊治具管理Access数据库;

namespace WebApplication2.生产管理.资材部.夹具管理.录入系统治具库
{
    /// <summary>
    /// 添加系统治具 的摘要说明
    /// </summary>
    public class 添加系统治具 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                int typeId = int.Parse(context.Request["typeId"]);//治具种类id
                string fixOrderNum = context.Request["fixOrderNum"];//治具订单编号
                string name = context.Request["name"];//治具名称//唯一
                string venderName = context.Request["venderName"];//厂家名称，默认给"JD"
                string desc = context.Request["desc"];//描述
                string remark = context.Request["remark"];//备注
                int currCount = int.Parse(context.Request["currCount"]);//当前库存
                int allCount = int.Parse(context.Request["allCount"]);//库存总量
                var file = context.Request.Files;//文件
                using (FixtureModel model = new FixtureModel())
                {
                    var fx = model.JDJS_WMS_Fixture_System_Table.Where(r => r.Name == name).FirstOrDefault();
                    if (fx != null)
                    {
                        context.Response.Write("该治具名称已存在！");
                        return;
                    }
                    if (file.Count < 1)
                    {
                        context.Response.Write("请输入文件！");
                        return;
                    }
                    int fxNum = 1;
                    fx = model.JDJS_WMS_Fixture_System_Table.Where(r => r.FXNum == fxNum).FirstOrDefault();
                    while (fx != null)
                    {
                        fxNum++;
                        fx = model.JDJS_WMS_Fixture_System_Table.Where(r => r.FXNum == fxNum).FirstOrDefault();
                    }
                    using (System.Data.Entity.DbContextTransaction mytran = model.Database.BeginTransaction())
                    {
                        try
                        {
                            JDJS_WMS_Fixture_System_Table jd = new JDJS_WMS_Fixture_System_Table()
                            {
                                AlterTime = DateTime.Now,
                                CreateTime = DateTime.Now,
                                Desc = desc,
                                FileName = file[0].FileName,
                                FixtureOrderNum = fixOrderNum,
                                FXNum = fxNum,
                                Name = name,
                                Remark = remark,
                                SerialCode = fxNum.ToString(),
                                StockAllNum = allCount,
                                StockCurrNum = currCount,
                                TypeId = typeId,
                                VenderName = venderName
                            };
                            model.JDJS_WMS_Fixture_System_Table.Add(jd);
                            model.SaveChanges();
                            mytran.Commit();
                            PathInfo info = new PathInfo();
                            for (int i = 0; i < file.Count; i++)
                            {
                                file[i].SaveAs(System.IO.Path.Combine(info.GetFixtrue_SurfMillFilePath(), file[i].FileName));
                            }
                            string str = "";
                            Fixture_SurfMill.AddChildJIG(name, desc, file[0].FileName, allCount.ToString(), currCount.ToString(), ref str, venderName, fxNum.ToString());

                            using (JDJS_WMS_DB_USEREntities wms = new JDJS_WMS_DB_USEREntities())
                            {
                                JDJS_WMS_Device_Status_Table ststus = new JDJS_WMS_Device_Status_Table()
                                {
                                    explain = desc,
                                    Status = name,
                                    SystemId = jd.Id
                                };
                                wms.JDJS_WMS_Device_Status_Table.Add(ststus);
                                wms.SaveChanges();
                            }
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