using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.生产管理.生产部.异常报备Method
{
    public class ProductAbnormalSubmitByNc : IProductAbnormalSubmit
    {
        /// <summary>
        /// nc文件异常报备
        /// </summary>
        /// <param name="id">订单Id</param>
        /// <returns></returns>
        public bool AbnormalSubmit(int id,ref string errMsg)
        {
            try
            {
                using (JDJS_WMS_DB_USEREntities wms = new JDJS_WMS_DB_USEREntities())
                {
                    var processes = wms.JDJS_WMS_Order_Process_Info_Table.Where(r => r.OrderID == id && r.sign != 0);
                    using (System.Data.Entity.DbContextTransaction mytran = wms.Database.BeginTransaction())
                    {
                        try
                        {
                            foreach (var item in processes)
                            {
                                item.program_audit_sign = -1;
                                item.ProgramePassTime = null;
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