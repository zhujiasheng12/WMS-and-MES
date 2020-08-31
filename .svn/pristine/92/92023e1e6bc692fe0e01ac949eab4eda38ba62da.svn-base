using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.生产部.异常报备Method
{
    /// <summary>
    /// 异常报备 的摘要说明
    /// </summary>
    public class 异常报备 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                int id = int.Parse(context.Request["id"]);
                string type = context.Request["type"];//NC,毛坯，治具，刀具
                AbnormalType abnormalType=AbnormalType.Other;
                string str = "请输入正确的报备类型！";
                switch (type)
                {
                    case "NC":
                        abnormalType = AbnormalType.NC;
                        break;
                    case "治具":
                        abnormalType = AbnormalType.Jia;
                        break;
                    case "毛坯":
                        abnormalType = AbnormalType.Blank;
                        break;
                    case "刀具":
                        abnormalType = AbnormalType.Tool;
                        break;
                }
                IProductAbnormalSubmit product = AbnormalFactory.GetAbnormalMethod(abnormalType);
                if (product != null)
                {
                    product.AbnormalSubmit(id, ref str);
                }
                context.Response.Write(str);
            }
            catch (Exception ex)
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