using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.IO;

namespace QRST_DI_MS_TOOLS_DataImportorUI.TAR.GZ
{
    public class XML_Tree
    {
        /// <summary>
        /// 创建入库数据类型树。非递归创建。
        /// </summary>
        /// <param name="outTree">待填充的TreeView控件</param>
        public static void CreatImportTypeTree( TreeView outTree)
        {
            string xmlPath = Path.Combine(Application.StartupPath, "FaceTree", "TypeTree.xml");
            XmlDocument treeXml = new XmlDocument();
            treeXml.Load(xmlPath);
            outTree.Nodes.Clear();
            //首先添加根节点
            TreeNode rootNode = new TreeNode("数据类型选择");

            XmlNode rootXmlNode;
            outTree.Nodes.Add(rootNode);
            try
            {
                rootXmlNode = treeXml.ChildNodes[0];
            }
            catch(Exception)
            {
                throw new Exception("获取可入库数据类型失败！");
            }
            for (int i = 0; i < rootXmlNode.ChildNodes.Count; i++)
            {
                //outTree.Nodes.Add(rootXmlNode.ChildNodes[i].Attributes["Sescription"].Value);
                TreeNode NodeDB = new TreeNode(rootXmlNode.ChildNodes[i].Attributes["NodeName"].Value);
                NodeDB.Tag = rootXmlNode.ChildNodes[i].Attributes["Tag"].Value;
                rootNode.Nodes.Add(NodeDB);
            }
        }

        /// <summary>
        /// 创建并填充数据类型选择树。数据库中无内容时无一级节点，递归创建。
        /// </summary>
        /// <param name="outTree">待填充得TreeView控件</param>
        public static void CreatTree(XmlNode inXml, TreeView outTree)
        {
            outTree.Nodes.Clear();
            //首先添加根节点
            TreeNode rootNode = new TreeNode("数据类型选择");
            outTree.Nodes.Add(rootNode);

            XmlNode rootXmlNode = inXml;
            for (int i = 0; i < rootXmlNode.ChildNodes.Count; i++)
            {
                //outTree.Nodes.Add(rootXmlNode.ChildNodes[i].Attributes["Sescription"].Value);
                TreeNode NodeDB = new TreeNode(rootXmlNode.ChildNodes[i].Attributes["Name"].Value);
                NodeDB.Tag = new string[] { rootXmlNode.ChildNodes[i].Attributes["CatalogCode"].Value,
                          rootXmlNode.ChildNodes[i].Attributes["DataType"].Value,
                          rootXmlNode.ChildNodes[i].Attributes["DataCode"].Value};
                XmlNodeList xnlist = rootXmlNode.ChildNodes[i].ChildNodes;
                InitTreeNode(NodeDB, xnlist);

                rootNode.Nodes.Add(NodeDB);
            }
        }
        private static void InitTreeNode(TreeNode NodeDB, XmlNodeList xnlist)
        {
            if (xnlist.Count == 0)
            {
                return;
            }
            else
            {
                string[] NodeArr = (string[])NodeDB.Tag;
                string NodeCatalogCode;
                try
                {
                    NodeCatalogCode = NodeArr[0];
                }
                catch (System.Exception ex)
                {
                    return;
                }
                //string NodeCatalog=(string[3])NodeDB.Tag
                for (int i = 0; i < xnlist.Count; i++)
                {
                    if (xnlist[i].Attributes["PreCatalogCode"].Value == NodeCatalogCode)
                    {
                        TreeNode treeNode = new TreeNode(xnlist[i].Attributes["Name"].Value);
                        treeNode.Tag = new string[]{xnlist[i].Attributes["CatalogCode"].Value,
                          xnlist[i].Attributes["DataType"].Value,
                          xnlist[i].Attributes["DataCode"].Value};
                        InitTreeNode(treeNode, xnlist);
                        NodeDB.Nodes.Add(treeNode);
                    }
                }
            }
        }
    }
}
