using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.生产部.异常报备Method
{
    public class AbnormalFactory
    {
        public static IProductAbnormalSubmit GetAbnormalMethod(AbnormalType type)
        {
            IProductAbnormalSubmit product;
            switch (type)
            {
                case AbnormalType.NC:
                    product = new ProductAbnormalSubmitByNc();
                    return product;
                case AbnormalType.Jia:
                    product = new ProductAbnormalSubmitByFixture();
                    return product;
                case AbnormalType.Blank :
                    product = new ProductAbnormalSubmitByBlank();
                    return product;
                case AbnormalType.Tool:
                    product = new ProductAbnormalSubmitByTool();
                    return product;
                default:
                    return null;
            }
            
        }
    }
}