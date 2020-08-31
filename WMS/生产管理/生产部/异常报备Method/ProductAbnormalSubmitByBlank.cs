using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.生产部.异常报备Method
{
    public class ProductAbnormalSubmitByBlank : IProductAbnormalSubmit
    {
        /// <summary>
        /// 毛坯异常报备
        /// </summary>
        /// <param name="id">订单id</param>
        /// <returns></returns>
        public bool AbnormalSubmit(int id, ref string errMsg)
        {
            try
            {
                using (JDJS_WMS_DB_USEREntities wms = new JDJS_WMS_DB_USEREntities())
                {
                    var processes = wms.JDJS_WMS_Order_Blank_Table.Where(r => r.OrderID == id);
                    using (System.Data.Entity.DbContextTransaction mytran = wms.Database.BeginTransaction())
                    {
                        try
                        {
                            foreach (var item in processes)
                            {
                                item.BlanktotalPreparedNumber  = 0;
                                item.BlankState = "待备料";
                            }
                            wms.SaveChanges();
                            errMsg = "ok";
                            mytran.Commit();
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
    }
}