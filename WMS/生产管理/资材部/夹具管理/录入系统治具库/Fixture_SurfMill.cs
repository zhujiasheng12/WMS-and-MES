using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace WebApplication2.生产管理.资材部.夹具管理.录入系统治具库
{
    public class Fixture_SurfMill
    {
        public static bool AddChildJIG(string name, string desc, string fileName, string sum, string curnum, ref string strErr,string venderName= "JD", string serialCode = "")
        {
            try
            {
                string path = new PathInfo ().GetFixtrue_SurfMillPath ();
                XmlDocument xml = new XmlDocument();
                xml.Load(path);
                XmlElement root = xml.DocumentElement;
                XmlNode xn = xml.SelectSingleNode("Inf-file");
                XmlElement element = xml.CreateElement("Inffile_Data_Feature");
                element.SetAttribute("Inffile_Data_Feature_Name", name);
                element.SetAttribute("Inffile_Data_Feature_VenderName",venderName);
                element.SetAttribute("Inffile_Data_Feature_SerialCode", serialCode);
                element.SetAttribute("Inffile_Data_Feature_Describe", desc + @"。 " + "当前库存数:" + curnum + @" " + "库存总数:" + sum);
                element.SetAttribute("Inffile_Data_FileName", fileName);

                root.InsertAfter(element, root.LastChild);
                XmlNode countNode = root.GetElementsByTagName("Inffile_Data_Count")[0];
                var a = int.Parse(countNode.Attributes[0].Value);
                countNode.Attributes[0].Value = (a + 1).ToString();
                xml.Save(path);
                strErr = "ok";
                return true;
            }
            catch (Exception ex)
            {
                strErr = ex.Message;
                return false;
            }
        }

        public static bool AlterChildJIG(string name, string desc, string fileName, string sum, string curnum, ref string strErr, string venderName = "JD", string serialCode = "")
        {
            try
            {
                string path = new PathInfo().GetFixtrue_SurfMillPath();
                XmlDocument xml = new XmlDocument();
                xml.Load(path);
                XmlNode xn = xml.SelectSingleNode("Inf-file");
                XmlNodeList xnl = xn.ChildNodes;
                for (int i = 2; i < xnl.Count; i++)
                {
                    XmlElement xe = (XmlElement)xnl[i];
                    string nodeval = xe.Attributes["Inffile_Data_Feature_Name"].Value;
                    string describ = string.Empty;
                    if (nodeval == name)
                    {
                        describ = desc + @"。 " + "当前库存数:" + curnum + @" " + "库存总数:" + sum;
                        xe.Attributes["Inffile_Data_Feature_Describe"].Value = describ;
                        xe.Attributes["Inffile_Data_FileName"].Value = fileName;
                        xe.Attributes["Inffile_Data_Feature_VenderName"].Value= venderName;
                        xe.Attributes["Inffile_Data_Feature_SerialCode"].Value =serialCode;
                        break;
                    }
                }
                xml.Save(path);
                strErr = "ok";
                return true;
            }
            catch (Exception ex)
            {
                strErr = ex.Message;
                return false;
            }
        }


        public static bool DeleteChildJIG(string name, string desc, string sum, string curnum, ref string strErr)
        {
            try
            {
                string path = new PathInfo().GetFixtrue_SurfMillPath();
                XmlDocument xml = new XmlDocument();
                xml.Load(path);
                XmlElement root = xml.DocumentElement;
                XmlNode countNode = root.GetElementsByTagName("Inffile_Data_Count")[0];
                var a = int.Parse(countNode.Attributes[0].Value);
                if (a < 1)
                {
                    strErr = "文件异常！";
                    return false;
                }
                int i = 0;
                XmlNode child = null;
                foreach (XmlNode item in root.ChildNodes)
                {
                    if (i < 2)
                    {
                        i++;
                        continue;
                    }
                    if (item.Attributes["Inffile_Data_Feature_Name"].Value == name && item.Attributes["Inffile_Data_Feature_Describe"].Value == (desc + @"。 " + "当前库存数:" + curnum + @" " + "库存总数:" + sum))
                    {
                        child = item;
                        break;
                    }
                }
                if (child != null)
                {
                    root.RemoveChild(child);
                }




                countNode.Attributes[0].Value = (a - 1).ToString();
                xml.Save(path);
                strErr = "ok";
                return true;
            }
            catch (Exception ex)
            {
                strErr = ex.Message;
                return false;
            }
        }
    }
}