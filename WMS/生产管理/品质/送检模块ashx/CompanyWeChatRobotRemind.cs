using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace WebApplication2.生产管理.品质.送检模块ashx
{
    interface SendTextToWechat
    {
        void SendText(string Text, string URL, List<string> vs, out bool z);
    }
    public class CompanyWeChatRobotRemind:SendTextToWechat
    {
        /// <summary>
        /// 发送文本到企业微信，
        /// </summary>
        /// <param name="Text">文本信息</param>
        /// <param name="myurl">机器人url</param>
        /// <param name="vs">需要发送给的人</param>
        /// <param name="z">是否成功</param>
        public void SendText(string Text, string myurl, List<string> vs, out bool z)
        {
            string myparaUrlCoded = "{\"msgtype\":\"text\",\"text\":{\"content\":\"";
            myparaUrlCoded += Text;
            myparaUrlCoded += "\",\"mentioned_mobile_list\":[\"@all\"]";
            myparaUrlCoded += "}}";
            string y;
            y = Post(myparaUrlCoded, myurl);
            z = string.Equals(y, "{\"errcode\":0,\"errmsg\":\"ok\"}");
            // Console.WriteLine(y);
        }
        private string Post(string paraUrlCoded, string URL)
        {
            string url = URL;
            string strURL = url;
            System.Net.HttpWebRequest request;
            request = (System.Net.HttpWebRequest)WebRequest.Create(strURL);
            request.Method = "POST";
            request.ContentType = "application/json;charset=UTF-8";

            byte[] payload;
            payload = System.Text.Encoding.UTF8.GetBytes(paraUrlCoded);
            request.ContentLength = payload.Length;
            Stream writer = request.GetRequestStream();
            writer.Write(payload, 0, payload.Length);
            writer.Close();
            System.Net.HttpWebResponse response;
            response = (System.Net.HttpWebResponse)request.GetResponse();
            System.IO.Stream s;
            s = response.GetResponseStream();
            string StrDate = "";
            StreamReader Reader = new StreamReader(s, Encoding.UTF8);
            StrDate = Reader.ReadLine();
            return StrDate;

        }
    }
}