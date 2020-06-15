//Author:JiangHua
//Date:2017-06-03

using System;
using System.Diagnostics;
using System.IO;
using System.Security.AccessControl;
using System.Threading;
using System.Xml;

namespace QRST_DI_SS_Basis
{
    public  class XmlUtility
    {
        static string _xmlPath = @"C:\QRST\AppConfig.xml";


        #region 生成端口插入xml文件
        public static void InsertPidPortNode(string attrName,string port)
        {
            try
            {
                XmlNode root = null;
                XmlHelper xmlHelper;
                if (!File.Exists(_xmlPath))
                {
                    string dircPath = _xmlPath.Substring(0, _xmlPath.Length - 13);
                    Directory.CreateDirectory(dircPath);
                    xmlHelper = new XmlHelper();
                    //创建XML  
                    xmlHelper.CreateXml(_xmlPath);
                    //创建根节点
                    xmlHelper.SetRootNode("ServerProName");

                    //添加节点
                    xmlHelper.InsertElement("ServerProName", "DbServerProcess", "attrName", attrName, port);

                    xmlHelper.Save();
                }
            }
            catch (Exception e)
            {
                //显示错误信息  
                throw new Exception("XML写入端口异常：" + e);
            }

        }

        private static bool PortIsUsed(string pName)
        {
            //lock (_syncRoot)
            //{
                bool isUsed = false;
                if (File.Exists(_xmlPath))
                {

                    //查询指定Name的端口
                    XmlHelper xmlHelper = new XmlHelper(_xmlPath);
                    //string xpath = "PIDPort[@PID=" + pid + "]";
                    XmlNode node = xmlHelper.GetSameNode(pName);
                    if (node != null)
                    {
                            isUsed = true;
                    }
                }


                return isUsed;
            //}

        }

        public static string GetServerUtilityPort(string pName)
        {
            string value = null;
            if (File.Exists(_xmlPath))
            {
                XmlHelper xmlHelper = new XmlHelper(_xmlPath);
                value = xmlHelper.GetNode(pName).InnerText;
            }
            return value;

        }
        #endregion

    }
}
