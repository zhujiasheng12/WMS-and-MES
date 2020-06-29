using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Kanban.设备监控
{
    /// <summary>
    /// 机床稼动率TOP 的摘要说明
    /// </summary>
    public class 机床稼动率TOP : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            if (context.Request["workId"] == "全部")
            {
                {
                    //稼动率top5
                    //int LocationID = workId;
                    List<CropMobility> cropMobilitiesUse = new List<CropMobility>();

                    List<CropMobility> cropMobilities = new List<CropMobility>();
                    using (JDJS_WMS_DB_USEREntities wms = new JDJS_WMS_DB_USEREntities())
                    {
                        var devices = wms.JDJS_WMS_Device_Info;
                        foreach (var item in devices)
                        {
                            CropMobility crops = new CropMobility();
                            int cncID = Convert.ToInt32(item.ID);
                            string cncNum = item.MachNum;
                            crops.cncNum = cncNum;
                            DateTime CancleStartTime = DateTime.Now.AddDays(-1);//计算稼动率的开始时间
                            DateTime CancleEndTime = DateTime.Now;//计算稼动率的结束时间

                            double runTime = 0;
                            double openTime = 0;
                            var states = wms.JDJS_WMS_Device_ProgState_Info.Where(r => r.CncID == cncID && ((r.StartTime >= CancleStartTime && r.StartTime <= CancleEndTime) || (r.EndTime >= CancleStartTime && r.EndTime <= CancleEndTime) || (r.StartTime <= CancleStartTime && r.EndTime >= CancleEndTime)));
                            List<progState> progStates = new List<progState>();
                            foreach (var real in states)
                            {
                                progState progState = new progState();
                                progState.ProgState = Convert.ToInt32(real.ProgState);
                                progState.startTime = Convert.ToDateTime(real.StartTime);
                                if (Convert.ToDateTime(real.StartTime) < CancleStartTime)
                                {
                                    progState.startTime = CancleStartTime;
                                }
                                if (real.EndTime == null)
                                {
                                    progState.EndTime = DateTime.Now;
                                }
                                else
                                {
                                    if (real.EndTime > CancleEndTime)
                                    {
                                        progState.EndTime = Convert.ToDateTime(CancleEndTime);
                                    }
                                    else
                                    {
                                        progState.EndTime = Convert.ToDateTime(real.EndTime);
                                    }
                                }
                                progStates.Add(progState);
                            }

                            foreach (var real in progStates)
                            {
                                if (real.ProgState != -1)
                                {
                                    openTime += Convert.ToDouble((real.EndTime - real.startTime).TotalMinutes);
                                }
                                if (real.ProgState == 1)
                                {
                                    runTime += Convert.ToDouble((real.EndTime - real.startTime).TotalMinutes);
                                }
                            }
                            if (openTime != 0)
                            {
                                crops.progress = runTime / openTime;
                            }
                            else
                            {
                                crops.progress = 0;
                            }
                            cropMobilities.Add(crops);
                        }


                    }

                    cropMobilities = cropMobilities.OrderByDescending(r => r.progress).ToList();

                    for (int i = 0; i < 5; i++)
                    {
                        if (cropMobilities.Count() > i)
                        {
                            CropMobility crop = new CropMobility();
                            crop.cncNum = cropMobilities[i].cncNum;
                            crop.progress = Math.Round(cropMobilities[i].progress, 4);
                            cropMobilitiesUse.Add(crop);
                        }
                        else
                        {
                            CropMobility crop = new CropMobility();
                            crop.cncNum = "";
                            crop.progress = 0;
                            cropMobilitiesUse.Add(crop);
                        }
                    }

                    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var model = new { code = 0, data = cropMobilitiesUse };
                    var json = serializer.Serialize(model);
                 context.Response.Write("data:" + json + "\n\n");
                    context.Response.ContentType = "text/event-stream";

                }
            }
            else if (context.Request["workId"] == "34台")
            {
                {
                    List<int> cncIDs = new List<int>();
                    for (int i = 95; i < 112; i++)
                    {
                        cncIDs.Add(i);
                    }
                    for (int i = 122; i < 139; i++)
                    {
                        cncIDs.Add(i);
                    }
                    //稼动率top5
                    //int LocationID = workId;
                    List<CropMobility> cropMobilitiesUse = new List<CropMobility>();

                    List<CropMobility> cropMobilities = new List<CropMobility>();
                    using (JDJS_WMS_DB_USEREntities wms = new JDJS_WMS_DB_USEREntities())
                    {
                        var devices = wms.JDJS_WMS_Device_Info;
                        foreach (var item in devices)
                        {
                            CropMobility crops = new CropMobility();
                            int cncID = Convert.ToInt32(item.ID);
                            if (cncIDs.Contains(cncID))
                            {
                                string cncNum = item.MachNum;
                                crops.cncNum = cncNum;
                                DateTime CancleStartTime = DateTime.Now.AddDays(-1);//计算稼动率的开始时间
                                DateTime CancleEndTime = DateTime.Now;//计算稼动率的结束时间

                                double runTime = 0;
                                double openTime = 0;
                                var states = wms.JDJS_WMS_Device_ProgState_Info.Where(r => r.CncID == cncID && ((r.StartTime >= CancleStartTime && r.StartTime <= CancleEndTime) || (r.EndTime >= CancleStartTime && r.EndTime <= CancleEndTime) || (r.StartTime <= CancleStartTime && r.EndTime >= CancleEndTime)));
                                List<progState> progStates = new List<progState>();
                                foreach (var real in states)
                                {
                                    progState progState = new progState();
                                    progState.ProgState = Convert.ToInt32(real.ProgState);
                                    progState.startTime = Convert.ToDateTime(real.StartTime);
                                    if (Convert.ToDateTime(real.StartTime) < CancleStartTime)
                                    {
                                        progState.startTime = CancleStartTime;
                                    }
                                    if (real.EndTime == null)
                                    {
                                        progState.EndTime = DateTime.Now;
                                    }
                                    else
                                    {
                                        if (real.EndTime > CancleEndTime)
                                        {
                                            progState.EndTime = Convert.ToDateTime(CancleEndTime);
                                        }
                                        else
                                        {
                                            progState.EndTime = Convert.ToDateTime(real.EndTime);
                                        }
                                    }
                                    progStates.Add(progState);
                                }

                                foreach (var real in progStates)
                                {
                                    if (real.ProgState != -1)
                                    {
                                        openTime += Convert.ToDouble((real.EndTime - real.startTime).TotalMinutes);
                                    }
                                    if (real.ProgState == 1)
                                    {
                                        runTime += Convert.ToDouble((real.EndTime - real.startTime).TotalMinutes);
                                    }
                                }
                                if (openTime != 0)
                                {
                                    crops.progress = runTime / openTime;
                                }
                                else
                                {
                                    crops.progress = 0;
                                }
                                cropMobilities.Add(crops);
                            }
                        }


                    }

                    cropMobilities = cropMobilities.OrderByDescending(r => r.progress).ToList();

                    for (int i = 0; i < 5; i++)
                    {
                        if (cropMobilities.Count() > i)
                        {
                            CropMobility crop = new CropMobility();
                            crop.cncNum = cropMobilities[i].cncNum;
                            crop.progress = Math.Round(cropMobilities[i].progress, 4);
                            cropMobilitiesUse.Add(crop);
                        }
                        else
                        {
                            CropMobility crop = new CropMobility();
                            crop.cncNum = "";
                            crop.progress = 0;
                            cropMobilitiesUse.Add(crop);
                        }
                    }

                    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var model = new { code = 0, data = cropMobilitiesUse };
                    var json = serializer.Serialize(model);
                 context.Response.Write("data:" + json + "\n\n"); 
                    context.Response.ContentType = "text/event-stream";

                }
            }
            else
            {
                var workId = int.Parse(context.Request["workId"]);
                {
                    //稼动率top5
                    int LocationID = workId;
                    List<CropMobility> cropMobilitiesUse = new List<CropMobility>();

                    List<CropMobility> cropMobilities = new List<CropMobility>();
                    using (JDJS_WMS_DB_USEREntities wms = new JDJS_WMS_DB_USEREntities())
                    {

                        var works = wms.JDJS_WMS_Location_Info.ToList();
                        var device = wms.JDJS_WMS_Device_Info.ToList();
                        List<WebApplication2.Kanban.现场. CncRead> objs = new List<WebApplication2.Kanban.现场.CncRead>();
                        List<int> workIds = new List<int>();
                        WebApplication2.Kanban.现场.机台状态 funs = new WebApplication2.Kanban.现场.机台状态();
                        var devices = funs.fun(workId, works, device, objs, workIds);


                        //var devices = wms.JDJS_WMS_Device_Info.Where(r => r.Position == LocationID);

                        foreach (var item in devices)
                        {
                            CropMobility crops = new CropMobility();
                            int cncID = Convert.ToInt32(item.ID);
                            string cncNum = item.MachNum;
                            crops.cncNum = cncNum;
                            DateTime CancleStartTime = DateTime.Now.AddDays(-1);//计算稼动率的开始时间
                            DateTime CancleEndTime = DateTime.Now;//计算稼动率的结束时间

                            double runTime = 0;
                            double openTime = 0;
                            var states = wms.JDJS_WMS_Device_ProgState_Info.Where(r => r.CncID == cncID && ((r.StartTime >= CancleStartTime && r.StartTime <= CancleEndTime) || (r.EndTime >= CancleStartTime && r.EndTime <= CancleEndTime) || (r.StartTime <= CancleStartTime && r.EndTime >= CancleEndTime)));
                            List<progState> progStates = new List<progState>();
                            foreach (var real in states)
                            {
                                progState progState = new progState();
                                progState.ProgState = Convert.ToInt32(real.ProgState);
                                progState.startTime = Convert.ToDateTime(real.StartTime);
                                if (Convert.ToDateTime(real.StartTime) < CancleStartTime)
                                {
                                    progState.startTime = CancleStartTime;
                                }
                                if (real.EndTime == null)
                                {
                                    progState.EndTime = DateTime.Now;
                                }
                                else
                                {
                                    if (real.EndTime > CancleEndTime)
                                    {
                                        progState.EndTime = Convert.ToDateTime(CancleEndTime);
                                    }
                                    else
                                    {
                                        progState.EndTime = Convert.ToDateTime(real.EndTime);
                                    }
                                }
                                progStates.Add(progState);
                            }

                            foreach (var real in progStates)
                            {
                                if (real.ProgState != -1)
                                {
                                    openTime += Convert.ToDouble((real.EndTime - real.startTime).TotalMinutes);
                                }
                                if (real.ProgState == 1)
                                {
                                    runTime += Convert.ToDouble((real.EndTime - real.startTime).TotalMinutes);
                                }
                            }
                            if (openTime != 0)
                            {
                                crops.progress = runTime / openTime;
                            }
                            else
                            {
                                crops.progress = 0;
                            }
                            cropMobilities.Add(crops);
                        }


                    }

                    cropMobilities = cropMobilities.OrderByDescending(r => r.progress).ToList();

                    for (int i = 0; i < 5; i++)
                    {
                        if (cropMobilities.Count() > i)
                        {
                            CropMobility crop = new CropMobility();
                            crop.cncNum = cropMobilities[i].cncNum;
                            crop.progress = Math.Round(cropMobilities[i].progress, 4);
                            cropMobilitiesUse.Add(crop);
                        }
                        else
                        {
                            CropMobility crop = new CropMobility();
                            crop.cncNum = "";
                            crop.progress = 0;
                            cropMobilitiesUse.Add(crop);
                        }
                    }

                    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var model = new { code = 0, data = cropMobilitiesUse };
                    var json = serializer.Serialize(model);
                 context.Response.Write("data:" + json + "\n\n"); 
                    context.Response.ContentType = "text/event-stream";

                }
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
    class CropMobility
    {
        public string cncNum;
        /// <summary>
        /// 稼动率
        /// </summary>
        public double progress;
       
    }
}