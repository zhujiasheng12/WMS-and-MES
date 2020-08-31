using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.生产部.异常报备Method
{
    public interface IProductAbnormalSubmit
    {
        bool AbnormalSubmit(int id,ref string errMsg);
    }
}