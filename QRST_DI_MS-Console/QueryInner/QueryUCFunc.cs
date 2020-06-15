using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QRST_DI_MS_Basis.QueryBase;
using System.Windows.Forms;
using QRST_DI_DS_Metadata.MetaDataDefiner;
using System.Xml;
using System.Drawing;
using System.Net;
using System.IO;

namespace QRST_DI_MS_Console.QueryInner
{
    /// <summary>
    /// 此处书写的是有关界面的代码 单独的方法也放在此处
    /// </summary>
    internal class QueryUCFunc
    {
        internal static List<ControlNameClass> listRasterBase = new List<ControlNameClass>();
        internal static List<ControlNameClass> listTilesBase = new List<ControlNameClass>();
        internal static List<ControlNameClass> listVectorBase = new List<ControlNameClass>();
        internal static List<ControlNameClass> listDocumentBase = new List<ControlNameClass>();
        internal static List<ControlNameClass> listSheetBase = new List<ControlNameClass>();

        #region 描述界面中每种数据类型固有基本检索控件的数组
        //要去除的查询条件保存的数组//此数组中内容要跟setBaseQueryControls方法中内容相关联。
        internal static string[] strArrRastersBase = new string[] { "左上经度", "左上纬度", "右上经度", "右上纬度", "左下经度", "左下纬度", "右下经度", "右下纬度", "日期" };
        internal static string[] strArrVectorsBase = new string[] { "左上经度", "左上纬度", "右上经度", "右上纬度", "左下经度", "左下纬度", "右下经度", "右下纬度" };
        internal static string[] strArrSheetsBase = new string[] { "关键字" };
        internal static string[] strArrDocumentsBase = new string[] { "文档类型", "关键字" };
        internal static string[] strArrTilesBase = new string[] { "左上经度", "左上纬度", "右上经度", "右上纬度", "左下经度", "左下纬度", "右下经度", "右下纬度", "切片等级" };

        internal static string[] strArrOperators = new string[] { " = ", " != ", " > ", " >= ", " < ", " <= ", " Like ", " NOT Like " };
        internal static string[] strArrLogicalOperators = new string[] { " and ", " or "};
        #endregion

        /// <summary>
        /// 创建并填充数据类型选择树。数据库中无内容时无一级节点。
        /// </summary>
        /// <param name="outTree">待填充得TreeView控件</param>
        internal static void CreatTree(TreeView outTree)
        {
            outTree.Nodes.Clear();
            //首先添加根节点
            TreeNode rootNode=new TreeNode("综合数据库");
            outTree.Nodes.Add(rootNode);

            //如果数据库中有子库，也即根节点有子节点继续，没有则退出
            if (TheUniversal.subDbLst.Count == 0)
            {
                rootNode.Text = "综合数据库中暂无数据类型可选";
                return;
            }

            //找到HostCode为root的DataTypeItem，填充数据库名称列表listsubDBName
            //QueryInner.listsubDBName.Clear();

            foreach(SiteDb siteDb in TheUniversal.subDbLst)
            {
                TreeNode dbNode = siteDb.GetDbNode();
                rootNode.Nodes.Add(dbNode);
            }
            

            //foreach(DataTypeClass i in listDataType)
            //{
            //    if (i.HostDBCode.Equals("root"))
            //    {
            //        QueryInner.listsubDBName.Add(i.CatagoryName);
            //    }
            //}
            ////数据库名称列表listsubDBName中有值时
            //if (QueryInner.listsubDBName.Count>0)
            //{
            //    TreeNode[] subDBNodes = new TreeNode[QueryInner.listsubDBName.Count];
            //    rootNode.Nodes.AddRange(subDBNodes);
            //}

        }

        /// <summary>
        /// 创建并填充数据类型选择树。数据库中无内容时无一级节点。
        /// </summary>
        /// <param name="outTree">待填充得TreeView控件</param>
        internal static void CreatTree(XmlNode inXml,TreeView outTree)
        {
            outTree.Nodes.Clear();
            //首先添加根节点
            TreeNode rootNode = new TreeNode("综合数据库");
            outTree.Nodes.Add(rootNode);

            XmlNode rootXmlNode = inXml;
            for (int i = 0; i < rootXmlNode.ChildNodes.Count;i++ )
            {
                //outTree.Nodes.Add(rootXmlNode.ChildNodes[i].Attributes["Sescription"].Value);
                TreeNode NodeDB = new TreeNode(rootXmlNode.ChildNodes[i].Attributes["Name"].Value);
                NodeDB.Tag = new string[] { rootXmlNode.ChildNodes[i].Attributes["CatalogCode"].Value,
                          rootXmlNode.ChildNodes[i].Attributes["DataType"].Value,
                          rootXmlNode.ChildNodes[i].Attributes["DataCode"].Value}; 
                XmlNodeList xnlist = rootXmlNode.ChildNodes[i].ChildNodes;
                InitTreeNode(NodeDB,xnlist);
                //for (int j = 0; j < rootXmlNode.ChildNodes[i].ChildNodes.Count;j++ )
                //{

                //}
                rootNode.Nodes.Add(NodeDB);
            }
        }

        private static void InitTreeNode(TreeNode NodeDB, XmlNodeList xnlist)
        {
            if (xnlist.Count==0)
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
                for (int i = 0; i < xnlist.Count;i++ )
                {
                    if (xnlist[i].Attributes["PreCatalogCode"].Value==NodeCatalogCode)
                    {
                        TreeNode treeNode = new TreeNode(xnlist[i].Attributes["Name"].Value);
                        treeNode.Tag = new string[]{xnlist[i].Attributes["CatalogCode"].Value,
                          xnlist[i].Attributes["DataType"].Value,
                          xnlist[i].Attributes["DataCode"].Value};
                        InitTreeNode(treeNode,xnlist);
                        NodeDB.Nodes.Add(treeNode);
                    }
                }
            }
        }
        /// <summary>
        /// 根据groupcode获取FileType
        /// </summary>
        /// <param name="strGroupCode">数据库中读取的GroupCode</param>
        internal static FileTypeCategory getFileType(string strGroupCode)
        {
            FileTypeCategory fileTypeout;
            switch(strGroupCode)
            {
                case "System_Vector":
                    fileTypeout = FileTypeCategory.Vectors;
                    break;
                case "System_Tile":
                    fileTypeout = FileTypeCategory.Tiles;
                    break;
                case "System_Raster":
                    fileTypeout = FileTypeCategory.Rasters;
                    break;
                case "System_Document":
                    fileTypeout = FileTypeCategory.Documents;
                    break;
                case "System_Table":
                    fileTypeout = FileTypeCategory.Sheets;
                    break;
                default:
                    fileTypeout = FileTypeCategory.NotDefine;
                    break;
            }
            return fileTypeout;
        }
        /// <summary>
        /// 把界面中得到的字符串转化为字段内操作符的枚举值
        /// </summary>
        /// <param name="strfieldOperator"></param>
        /// <returns></returns>
        //internal static WS_QDB_Searcher_MySQL.FieldOperator FieldOperatorToEnum(string strfieldOperator)
        //{
        //    WS_QDB_Searcher_MySQL.FieldOperator retFOper = new WS_QDB_Searcher_MySQL.FieldOperator();

        //    switch(strfieldOperator.Trim())
        //    {
        //        case "=":
        //            retFOper = WS_QDB_Searcher_MySQL.FieldOperator.Eto;
        //            break;
        //        case "!=":
        //            retFOper = WS_QDB_Searcher_MySQL.FieldOperator.NEto;
        //            break;
        //        case ">":
        //            retFOper = WS_QDB_Searcher_MySQL.FieldOperator.Gto;
        //            break;
        //        case ">=":
        //            retFOper = WS_QDB_Searcher_MySQL.FieldOperator.GEto;
        //            break;
        //        case "<":
        //            retFOper = WS_QDB_Searcher_MySQL.FieldOperator.Lto;
        //            break;
        //        case "<=":
        //            retFOper = WS_QDB_Searcher_MySQL.FieldOperator.LEto;
        //            break;
        //        case "Like":
        //            retFOper = WS_QDB_Searcher_MySQL.FieldOperator.Liketo;
        //            break;
        //        case "NOT Like":
        //            retFOper = WS_QDB_Searcher_MySQL.FieldOperator.NLiketo;
        //            break;
        //        default:
        //            retFOper = WS_QDB_Searcher_MySQL.FieldOperator.NotDefine;
        //            break;
        //    }
        //    return retFOper;
        //}
        /// <summary>
        ///  把界面中输入的逻辑操作符转化为enum类型
        /// </summary>
        /// <param name="strLogicalOperator"></param>
        /// <returns></returns>
        //internal static WS_QDB_Searcher_MySQL.LogicalOperator LogicalOperatorToEnum(string strLogicalOperator)
        //{
        //    WS_QDB_Searcher_MySQL.LogicalOperator retlogicalOper = new WS_QDB_Searcher_MySQL.LogicalOperator();
        //    switch (strLogicalOperator.Trim())
        //    {
        //        case "and":
        //            retlogicalOper = WS_QDB_Searcher_MySQL.LogicalOperator.ANDto;
        //            break;
        //        case "or":
        //            retlogicalOper = WS_QDB_Searcher_MySQL.LogicalOperator.ORto;
        //            break;
        //        default:
        //            retlogicalOper = WS_QDB_Searcher_MySQL.LogicalOperator.NotDefine;
        //            break;
        //    }
        //    return retlogicalOper;
        //}

        //internal static string SimpleConditionToStr(SimpleCondition simpleCondition)
        //{
        //    string retStr = string.Empty;
        //    return retStr;
        //}
        /// <summary>
        /// 查找含有指定字符串的字段名称
        /// </summary>
        /// <param name="strArrIn"></param>
        /// <param name="likestr"></param>
        /// <returns></returns>
        internal static string FindLikeField(string[] strArrIn,string likestr)
        {
            string likeField=string.Empty;

            for (int i = 0; i < strArrIn.Length;i++ )
            {
                if (strArrIn[i].Contains(likestr))
                {
                    likeField = strArrIn[i];
                    break;
                }
            }
            return likeField;
        }
        /// <summary>
        /// 查找含有指定字符串的字段名称
        /// </summary>
        /// <param name="strArrIn"></param>
        /// <param name="likestr"></param>
        /// <returns></returns>
        internal static List<string> FindLikeField(string[] strArrIn, string[] likestr)
        {
            List<string> likeField = new List<string>();
            likeField.Clear();
            for (int i = 0; i < strArrIn.Length; i++)
            {
                for (int j = 0; j < likestr.Length;j++ )
                {
                    if (strArrIn[i].Contains(likestr[j]))
                    {
                        likeField.Add(strArrIn[i]);
                    }
                }
            }
            return likeField;
        }
        /// <summary>
        /// 根据控件描述得到控件对应的字段名称
        /// </summary>
        /// <param name="fileType"></param>
        /// <param name="inCaption"></param>
        /// <returns></returns>
        internal static string FindFieldByCaption(FileTypeCategory fileType, string inCaption)
        {
            string retField = string.Empty;
            switch (fileType)
            {
                case FileTypeCategory.Rasters:
                    for (int i = 0; i < listRasterBase.Count; i++)
                    {
                        if (listRasterBase[i].ControlText == inCaption)
                        {
                            retField = listRasterBase[i].FieldText;
                            break;
                        }
                    }
                    break;
                case FileTypeCategory.Tiles:
                    for (int i = 0; i < listTilesBase.Count; i++)
                    {
                        if (listTilesBase[i].ControlText == inCaption)
                        {
                            retField = listTilesBase[i].FieldText;
                            break;
                        }
                    }
                    break;
                case FileTypeCategory.Vectors:
                    for (int i = 0; i < listVectorBase.Count; i++)
                    {
                        if (listVectorBase[i].ControlText == inCaption)
                        {
                            retField = listVectorBase[i].FieldText;
                            break;
                        }
                    }
                    break;
                case FileTypeCategory.Documents:
                    for (int i = 0; i < listDocumentBase.Count; i++)
                    {
                        if (listDocumentBase[i].ControlText == inCaption)
                        {
                            retField = listDocumentBase[i].FieldText;
                            break;
                        }
                    }
                    break;
                case FileTypeCategory.Sheets:
                    for (int i = 0; i < listSheetBase.Count; i++)
                    {
                        if (listSheetBase[i].ControlText == inCaption)
                        {
                            retField = listSheetBase[i].FieldText;
                            break;
                        }
                    }
                    break;
                case FileTypeCategory.NotDefine:
                    break;
                default:
                    break;

            }
            return retField;
        }
        /// <summary>
        /// 根据控件描述得到控件对应的字段名称  适用于关键字   有多个字段对应一个控件的
        /// </summary>
        /// <param name="fileType"></param>
        /// <param name="inCaption"></param>
        /// <returns></returns>
        internal static List<string> FindFieldListByCaption(FileTypeCategory fileType, string inCaption)
        {
            List<string> retFieldList = new List<string>();
            switch (fileType)
            {
                case FileTypeCategory.Rasters:
                    for (int i = 0; i < listRasterBase.Count; i++)
                    {
                        if (listRasterBase[i].ControlText == inCaption)
                        {
                            retFieldList.Add(listRasterBase[i].FieldText);
                        }
                    }
                    break;
                case FileTypeCategory.Tiles:
                    for (int i = 0; i < listTilesBase.Count; i++)
                    {
                        if (listTilesBase[i].ControlText == inCaption)
                        {
                            retFieldList.Add(listTilesBase[i].FieldText);
                        }
                    }
                    break;
                case FileTypeCategory.Vectors:
                    for (int i = 0; i < listVectorBase.Count; i++)
                    {
                        if (listVectorBase[i].ControlText == inCaption)
                        {
                            retFieldList.Add(listVectorBase[i].FieldText);
                        }
                    }
                    break;
                case FileTypeCategory.Documents:
                    for (int i = 0; i < listDocumentBase.Count; i++)
                    {
                        if (listDocumentBase[i].ControlText == inCaption)
                        {
                            retFieldList.Add(listDocumentBase[i].FieldText);
                        }
                    }
                    break;
                case FileTypeCategory.Sheets:
                    for (int i = 0; i < listSheetBase.Count; i++)
                    {
                        if (listSheetBase[i].ControlText == inCaption)
                        {
                            retFieldList.Add(listSheetBase[i].FieldText);
                        }
                    }
                    break;
                case FileTypeCategory.NotDefine:
                    break;
                default:
                    break;

            }
            return retFieldList;
        }
        /// <summary>
        /// 把界面中基础检索控件和 数据库中字段名称关联起来，并记录显示的基础检索控件
        /// </summary>
        /// <param name="controlText"></param>
        /// <param name="fieldText"></param>
        internal static void setListBase(string controlText,string fieldText,FileTypeCategory fileType)
        {
            ControlNameClass controlnameClass = new ControlNameClass();
            controlnameClass.ControlText = controlText;
            controlnameClass.FieldText = fieldText;

            switch (fileType)
            {
                case FileTypeCategory.Rasters:
                    listRasterBase.Add(controlnameClass);
                    break;
                case FileTypeCategory.Tiles:
                    listTilesBase.Add(controlnameClass);
                    break;
                case FileTypeCategory.Vectors:
                    listVectorBase.Add(controlnameClass);
                    break;
                case FileTypeCategory.Documents:
                    listDocumentBase.Add(controlnameClass);
                    break;
                case FileTypeCategory.Sheets:
                    listSheetBase.Add(controlnameClass);
                    break;
                case FileTypeCategory.NotDefine:
                    break;
                default:
                    break;

            }

        }

        /// <summary>
        /// 从网址上获取图片信息，并保存到指定目录下的指定文件中。实现先在目录下查找，若图像不存在再去网站上下载，返回Image对象。
        /// </summary>
        /// <param name="imgUrl">指定的网址</param>
        /// <param name="path">指定目录</param>
        /// <param name="fileName">指定的</param>
        /// <returns>返回的Image对象</returns>
        internal static Image SaveImageFromWeb(string imgUrl, string fileDirectory, string fileName)
        {
            Image returnIMG = null;

            if (fileDirectory.Equals(""))
                throw new Exception("未指定保存文件的路径");
            if (!Directory.Exists(fileDirectory))
            {
                return returnIMG;
            }
            string fullFilePath = fileDirectory + fileName+".jpg";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(imgUrl);
            request.UserAgent = "Mozilla/6.0 (MSIE 6.0; Windows NT 5.1; Natas.Robot)";
            request.Timeout = 3000;
            //WebResponse response = null;
            //Stream stream=null;
            try
            {
                WebResponse response = request.GetResponse();
                Stream stream = response.GetResponseStream();
                if (response.ContentType.ToLower().StartsWith("image/"))
                {
                    byte[] arrayByte = new byte[1024];
                    int imgLong = (int)response.ContentLength;
                    int l = 0;

                    FileStream fso = new FileStream(fullFilePath, FileMode.Create);
                    while (l < imgLong)
                    {
                        int i = stream.Read(arrayByte, 0, 1024);
                        fso.Write(arrayByte, 0, i);
                        l += i;
                    }

                    fso.Close();
                    stream.Close();
                    response.Close();

                    returnIMG = Image.FromFile(fullFilePath);
                }
            }
            catch
            {
                Console.WriteLine("操作超时，当前图像未获取成功。");
            }
            return returnIMG;
        }  

        /// <summary>
        /// 获取缩略图的数据来源
        /// </summary>
        /// <param name="ThumbCellContant"></param>
        /// <returns></returns>
        internal static ThumbNailSource GetThumbNailSource(string ThumbCellContant)
        {
            if (string.IsNullOrEmpty(ThumbCellContant))
            {
                return ThumbNailSource.Others;
            }
            if (ThumbCellContant.Length>=4)
            {
                string StartString2 = ThumbCellContant.Substring(0, 2);
                string StartString4 = ThumbCellContant.Substring(0, 4);

                if (StartString2.Equals(@"\\")) 
                {
                    return ThumbNailSource.TelePath;
                }
                else if (StartString4.Equals("http"))
                {
                    return ThumbNailSource.HttpOnNet;
                }
                else
                {
                    return ThumbNailSource.DirectlyDB;
                }
            }
            else
            {
                return ThumbNailSource.Others;
            }
        }
        /// <summary>
        /// 拷贝文件夹
        /// </summary>
        /// <param name="from">源目录</param>
        /// <param name="to">目标目录</param>
        internal static void CopyFolder(string from, string to)
        {
            if (!Directory.Exists(to))
                Directory.CreateDirectory(to);

            // 子文件夹
            foreach (string sub in Directory.GetDirectories(from))
                CopyFolder(sub + "\\", to + Path.GetFileName(sub) + "\\");

            // 文件
            foreach (string file in Directory.GetFiles(from))
                File.Copy(file, to + Path.GetFileName(file), true);
        }

        /// <summary>
        /// 单条数据下载
        /// </summary>
        /// <param name="RalativePath">源地址的绝对路径</param>
        /// <param name="AbsolutePath">源地址的相对路径</param>
        /// <param name="DestPath">目标地址</param>
        /// <param name="currentDataCatalogName">当前数据类型</param>
        /// <returns></returns>
        internal static bool LoadOneData(string RalativePath, string AbsolutePath, string DestPath, string currentDataCatalogName)
        {
            bool retBool = false;

            string strDataMarkSubPath = string.Empty;
            RalativePath = RalativePath.TrimEnd(@"\".ToCharArray()) + @"\";
            DestPath = DestPath.Trim(@"\".ToCharArray()) + @"\";
            AbsolutePath = AbsolutePath.Trim(@"\".ToCharArray());
            strDataMarkSubPath = AbsolutePath.Substring(AbsolutePath.LastIndexOf(@"\")+1);

            string sourcePath = RalativePath + AbsolutePath + @"\";
            string destPath = DestPath + strDataMarkSubPath + @"\";

            if (Directory.Exists(destPath))
            {
                retBool = true;
            }
            else
            {
                if (Directory.Exists(sourcePath))
                {
                    QueryUCFunc.CopyFolder(sourcePath, destPath);
                    retBool = true;
                }
                else
                {
                    retBool = false;
                }
            }
            return retBool;
        }
    }

    internal class ControlNameClass
    {
        internal string ControlText;
        internal string FieldText;
    }

    public enum ThumbNailSource
    {
        /// <summary>
        /// 直接存储于数据库中
        /// </summary>
        DirectlyDB,
        /// <summary>
        /// 远程路径，如部署的hadoop路径。数据库中存放的是数据的路径
        /// </summary>
        TelePath,
        /// <summary>
        /// 通过网络获取，数据库中存放图像的网址
        /// </summary>
        HttpOnNet,
        /// <summary>
        /// 其他
        /// </summary>
        Others
    }

    public class GetImgFromNet
    {
        public string imgUrl;
        public string fileDirectory;
        public string fileName;

        public GetImgFromNet(string url, string fileDic, string filename)
        {
            imgUrl = url;
            fileDirectory = fileDic;
            fileName = filename;
        }

        public void GetImgFromWeb()
        {
            if (fileDirectory.Equals(""))
                throw new Exception("未指定保存文件的路径");
            if (!Directory.Exists(fileDirectory))
            {
                return ;
            }
            string fullFilePath = fileDirectory + fileName + ".jpg";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(imgUrl);
            request.UserAgent = "Mozilla/6.0 (MSIE 6.0; Windows NT 5.1; Natas.Robot)";
            request.Timeout = 3000;
            //WebResponse response = null;
            //Stream stream=null;
            try
            {
                WebResponse response = request.GetResponse();
                Stream stream = response.GetResponseStream();
                if (response.ContentType.ToLower().StartsWith("image/"))
                {
                    byte[] arrayByte = new byte[1024];
                    int imgLong = (int)response.ContentLength;
                    int l = 0;

                    FileStream fso = new FileStream(fullFilePath, FileMode.Create);
                    while (l < imgLong)
                    {
                        int i = stream.Read(arrayByte, 0, 1024);
                        fso.Write(arrayByte, 0, i);
                        l += i;
                    }

                    fso.Close();
                    stream.Close();
                    response.Close();
                }
            }
            catch
            {
                Console.WriteLine("操作超时，当前图像未获取成功。");
                return;
            }
        }
    }
}
