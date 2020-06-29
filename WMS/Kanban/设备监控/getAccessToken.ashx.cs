using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace WebApplication2.Kanban.设备监控
{
    /// <summary>
    /// getAccessToken 的摘要说明
    /// </summary>
    public class getAccessToken : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string result = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://open.ys7.com/api/lapp/token/get");
            req.Method = "post";
            req.ContentType = "application/x-www-form-urlencoded";
            // req.Host = "open.ys7.com";
            #region 添加Post 参数
            byte[] data = Encoding.UTF8.GetBytes("appKey=bc6b7a5d58e4445eaff4fac5965705dc&appSecret=4e28780d036383859713c7ae50fb71ad");
            req.ContentLength = data.Length;
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
            }
            #endregion

            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            //获取响应内容
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }

            context.Response.Write(result);

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