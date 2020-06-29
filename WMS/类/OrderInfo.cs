using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication2
{
    public class OrderInfo
    {
        public int OrderID;
        public string OrderNum;
        public int OrderOutPut;
        public int isFlag;
        public List<ProcessInfo> OrderProcessInfos = new List<ProcessInfo>();
        public List<Result> OrderResults = new List<Result>();
    }
    public class DeviceInfo
    {
        public int CncID;
        public string Type;
        public int order;
        public int process;
        public DateTime startTime;
        public DateTime endTime;

    }

    public class ProcessInfo
    {
        public int id;
        public int processID;
        public int deviceNum;
        public double processTime;
        public int deviceType;
        /// <summary>
        /// 工序系数
        /// </summary>
        public int modulus;
        /// <summary>
        /// 排产数量
        /// </summary>
        public int ScheduNum;
        public double allTime;
    }
    public class Result
    {
        public int Num;
        public double Time;
        public DateTime OverTime;
        public List<DeviceInfo> deviceInfos;
    }
}
