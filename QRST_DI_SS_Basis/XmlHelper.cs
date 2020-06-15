using System;
using System.Collections;
using System.Xml;

namespace QRST_DI_SS_Basis
{
    public class XmlHelper
    {
        protected string strXmlFile;
        protected XmlDocument objXmlDoc = new XmlDocument();
        private Object _syncRoot =new object();
        //private Object _syncInsert = new object();

        public XmlHelper(string XmlFile)
        {
            try
            {
                objXmlDoc.Load(XmlFile);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            strXmlFile = XmlFile;
        }

        public XmlHelper()
        { }

        public void CreateXml(string xmlPath)
        {
            lock (_syncRoot)
            {
                //创建类型声明节点  
                XmlNode node = objXmlDoc.CreateXmlDeclaration("1.0", "utf-8", "");
                objXmlDoc.AppendChild(node);

                strXmlFile = xmlPath;
            }


        }

        public XmlNode SetRootNode(string rootNode)
        {
            lock (_syncRoot)
            {
                XmlNode _rootNode = objXmlDoc.CreateElement(rootNode);
                objXmlDoc.AppendChild(_rootNode);
                return _rootNode;
            }

        }

        /// <summary>    
        /// 创建节点    
        /// </summary>    
        /// <param name="xmldoc"></param>  xml文档  
        /// <param name="parentnode"></param>父节点    
        /// <param name="name"></param>  节点名  
        /// <param name="value"></param>  节点值  
        ///   
        public void InsertNode(XmlNode parentNode, string name, string value)
        {
            XmlNode node = objXmlDoc.CreateNode(XmlNodeType.Element, name, null);
            node.InnerText = value;
            parentNode.AppendChild(node);
        }  

        /// <summary>
        /// 插入一节点和此节点的子节点。 
        /// </summary>
        /// <param name="MainNode">主节点</param>
        /// <param name="ChildNode">子节点</param>
        /// <param name="Element">元素</param>
        /// <param name="Content">内容</param>
        public void InsertNode(string MainNode, string ChildNode, Hashtable ht)
        {
            //插入一节点和此节点的一子节点。 
            XmlNode objRootNode = objXmlDoc.SelectSingleNode(MainNode);

            XmlElement objChildNode = objXmlDoc.CreateElement(ChildNode);
            objRootNode.AppendChild(objChildNode);

            // 遍历哈希表
            foreach (DictionaryEntry de in ht)
            {
                XmlElement objElement = objXmlDoc.CreateElement((string)de.Key);
                objElement.InnerText = (string)de.Value;
                objChildNode.AppendChild(objElement);
            }
        }

        public XmlNode GetNode(string pName)
        {
  
            XmlNodeList childNodes = objXmlDoc.DocumentElement.ChildNodes;
            foreach (XmlNode childNode in childNodes)
            {
                XmlElement childElement = (XmlElement) childNode;
                if (childElement.GetAttribute("attrName") == pName)
                {
                return childNode;
                }
            }

            return null;
        }

        public XmlNode GetSameNode(string pName)
        {
            XmlNodeList childNodes = objXmlDoc.DocumentElement.ChildNodes;
            foreach (XmlNode childNode in childNodes)
            {
                XmlElement childElement = (XmlElement)childNode;
                if (childElement.GetAttribute("attrName") == pName && childNode.InnerText.Equals(pName))
                {                 
                    return childNode;
                }
            }

            return null;
        }

        public void DeleteNode(XmlNode node)
        {
            lock (_syncRoot)
            {
                objXmlDoc.DocumentElement.RemoveChild(node);
            }
        }
        /// <summary>
                /// 对xml文件做插入，更新，删除后需做Save()操作，以保存修改
                /// </summary>
            public void Save()
        {
            //保存文檔。 
            lock (_syncRoot)
            {
                try
                {
                    objXmlDoc.Save(strXmlFile);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                objXmlDoc = null;
            }

        }


        /// <summary>
        /// 新节点内容。
        /// 示例：xmlTool.Replace("Book/Authors[ISBN=\"0002\"]/Content","ppppppp"); 
        /// </summary>
        /// <param name="XmlPathNode"></param>
        /// <param name="Content"></param>
        public void Replace(string XmlPathNode, string Content)
        {
            //更新节点内容。 
            objXmlDoc.SelectSingleNode(XmlPathNode).InnerText = Content;
        }

        /// <summary>
        /// 删除一个指定节点的子节点。 
        /// 示例： xmlTool.DeleteChild("Book/Authors[ISBN=\"0003\"]"); 
        /// </summary>
        /// <param name="Node"></param>
        public void DeleteChild(string Node)
        {
            //删除一个节点。 
            string mainNode = Node.Substring(0, Node.LastIndexOf("/"));
            objXmlDoc.SelectSingleNode(mainNode).RemoveChild(objXmlDoc.SelectSingleNode(Node));
        }



        /// <summary>

        ///  * 使用示列:
        ///  示例： XmlHelper.Delete( "/Node", "")
        ///  XmlHelper.Delete( "/Node", "Attribute")
        /// </summary>
        /// <param name="node">节点</param>
        /// <param name="attribute">属性名，非空时删除该节点属性值，否则删除节点值</param>
        public void Delete(string node, string attribute)
        {
            try
            {

                XmlNode xn = objXmlDoc.SelectSingleNode(node);
                XmlElement xe = (XmlElement)xn;
                if (attribute.Equals(""))
                    xn.ParentNode.RemoveChild(xn);
                else
                    xe.RemoveAttribute(attribute);

            }
            catch { }
        }




        /// <summary>
        /// 插入一个节点，带一属性。
        /// 示例： xmlTool.InsertElement("Book/Author[ISBN=\"0004\"]","Title","Sex","man","iiiiiiii"); 
        /// </summary>
        /// <param name="MainNode">主节点</param>
        /// <param name="Element">元素</param>
        /// <param name="Attrib">属性</param>
        /// <param name="AttribContent">属性内容</param>
        /// <param name="Content">元素内容</param>
        public void InsertElement(string MainNode, string Element, string Attrib, string AttribContent, string Content)
        {
            lock (_syncRoot)
            {
                //插入一个节点，带一属性。 
                XmlNode objNode = objXmlDoc.SelectSingleNode(MainNode);
                XmlElement objElement = objXmlDoc.CreateElement(Element);
                objElement.SetAttribute(Attrib, AttribContent);
                objElement.InnerText = Content;
                objNode.AppendChild(objElement);
            }
        }

        /// <summary>
        /// 插入一个节点，不带属性。
        /// 示例：xmlTool.InsertElement("Book/Author[ISBN=\"0004\"]","Content","aaaaaaaaa"); 
        /// </summary>
        /// <param name="MainNode">主节点</param>
        /// <param name="Element">元素</param>
        /// <param name="Content">元素内容</param>
        public void InsertElement(string MainNode, string Element, string Content)
        {
            //插入一个节点，不带属性。 
            XmlNode objNode = objXmlDoc.SelectSingleNode(MainNode);
            XmlElement objElement = objXmlDoc.CreateElement(Element);
            objElement.InnerText = Content;
            objNode.AppendChild(objElement);
        }


    }
}
