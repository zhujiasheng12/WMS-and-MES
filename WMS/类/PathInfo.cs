using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;

namespace WebApplication2
{
    public class PathInfo
    {
        string xmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "类", "path.xml");     
      public  string upLoadPath()//上传路径
        {            
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlPath);
            XmlNode root = doc.SelectSingleNode("/root/upLoadPath");
            return root.InnerText;
        }
 
     public string downLoadPath()//下载路径
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlPath);
            XmlNode root = doc.SelectSingleNode("/root/downLoadPath");
            return root.InnerText;
        }

        public string DaMaMachName()//打印机位置
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlPath);
            XmlNode root = doc.SelectSingleNode("/root/DaMaMachName"); 
            return @root.InnerText;
        }
        public string cncLayoutPath()//机床布局图位置
        {
            var path =Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "工厂管理", "设计布局图");
            return path+"\\";
        }
        public string ftpUrl()//ftpUrl
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlPath);
            XmlNode root = doc.SelectSingleNode("/root/ftpUrl");
            return @root.InnerText;
        }
        public string ftpUser()//ftpUrl
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlPath);
            XmlNode root = doc.SelectSingleNode("/root/ftpUser");
            return @root.InnerText;
        }
        public string ftpPassword()//ftpUrl
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlPath);
            XmlNode root = doc.SelectSingleNode("/root/ftpPassword");
            return @root.InnerText;
        }

        public  string GetQualituInspectApplyRobot()//获取送检新建申请消息机器人url
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlPath);
            XmlNode root = doc.SelectSingleNode("/root/QualituInspectApplyRobot");
            return root.InnerText;
        }

        public string GetQualituInspectAuditRobot()//获取送检审核消息机器人url
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlPath);
            XmlNode root = doc.SelectSingleNode("/root/QualituInspectAuditRobot");
            return root.InnerText;
        }

        public string GetQualituInspectOverRobot()//获取送检完成消息机器人url
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlPath);
            XmlNode root = doc.SelectSingleNode("/root/QualituInspectOverRobot");
            return root.InnerText;
        }

        public string GetFixtrue_SurfMillPath()//获取送检完成消息机器人url
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlPath);
            XmlNode root = doc.SelectSingleNode("/root/Fixtrue_SurfMillPath");
            return root.InnerText;
        }
    }
}