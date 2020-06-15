using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
 
namespace QRST_DI_DS_Metadata.DataImportPlugin
{
    public class SJ9ADataImport:IDataImport
    {
        //定义全局变量
        string srcPath;
        IOrderInterface parentOrder;

        string originalData;
        string relatePath;

        /// <summary>
        /// 设置所需要的参数 
        /// </summary>
        /// <param name="paras"></param>
        public void SetParameter(string []paras)
        {
            if(paras == null || paras.Length <1||!paras[0].ToLower().EndsWith(".tar.gz"))
            {
                throw new Exception("参数格式不正确！");
            }
            srcPath = paras[0];

            //根据文件名定义数据相对路径存储方式   SJ9A_PMS_E123.3_N46.8_20130806_L1A0000064730.tar.gz
            string[] strArr = Path.GetFileName(srcPath).Split("_".ToCharArray());
            if (strArr.Length == 6)
            {
                string satellite = strArr[0];
                string sensor = strArr[1];
                string year = strArr[4].Substring(0, 4);
                string month = strArr[4].Substring(4, 2);
                string day = strArr[4].Substring(6, 2);

                relatePath = string.Format(@"实验验证数据库\\SJ9A\\{0}\\{1}\\{2}\\{3}\\{4}\\{5}\\", satellite, sensor, year, month, day, Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(srcPath)));
            }
            else
            {
                throw new Exception("SetParameter：参数格式不正确！");
            }
        }

        /// <summary>
        /// 设置父订单 
        /// </summary>
        /// <param name="_parentOrder"></param>
        public void SetParentOrder(IOrderInterface _parentOrder)
        {
            if(_parentOrder == null)
            {
                throw new Exception("SetParentOrder：父订单不能为空！");
            }
            parentOrder = _parentOrder;
            originalData = parentOrder.GetOrderWorkspace() + "\\" + "OrignalData";
        }

        /// <summary>
        /// 数据准备，完成数据解压工作
        /// </summary>
        /// <returns></returns>
        public bool DataPrepare()
        {
            if (!File.Exists(srcPath) || !srcPath.ToLower().EndsWith(".tar.gz"))
            {
                parentOrder.Addlog("DataPrepare：源文件不存在！");
                return false;
            }
            //将xml,jpg文件解压到${workspace}/OrignalData下面
            try
            {
                parentOrder.Addlog("DataPrepare：开始解压原始数据");
                DecompressXmlJpgFromTargzFile(srcPath, parentOrder.GetOrderWorkspace() + "\\" + "OrignalData");
                parentOrder.Addlog("DataPrepare：完成解压原始数据");

            }
            catch(Exception ex)
            {
                parentOrder.Addlog("DataPrepare：文件解压失败！" + ex.ToString());
                return false;
            }
           
            return true;
        }

        /// <summary>
        /// 获取原始数据文件路径
        /// </summary>
        /// <returns></returns>
        public string[] GetSourceFilePath()
        {
            return new string[] { srcPath };
        }

        /// <summary>
        /// 提取元数据信息
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetMetadata()
        {
            parentOrder.Addlog("GetMetadata：开始提取元数据文件！");
            Dictionary<string, string> field2value = new Dictionary<string, string>();
            //获取xml文件
            string[] xmlFiles = Directory.GetFiles(originalData,"*.xml");
            string fileNameWithoutExt = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(srcPath))+"-MUX";
            string xmlFile = null;
            for (int i = 0; i < xmlFiles.Length;i++ )
            {
                string tempFilename = Path.GetFileNameWithoutExtension(xmlFiles[i]);
                if (tempFilename.Equals(fileNameWithoutExt))
                {
                    xmlFile = xmlFiles[i];
                    break;
                }
            }
            if(string.IsNullOrEmpty(xmlFile))
            {
                throw new Exception("GetMetadata：没有找到对应的元数据文件");
            }

            XmlDocument root = new XmlDocument();
            try
            {
                root.Load(xmlFile);
            }
            catch (System.Exception ex)
            {
                throw new Exception("GetMetadata：xml文件损坏，请检查！");
            }
            XmlNodeList nodelst = root.GetElementsByTagName("ProductMetaData");
            if(nodelst.Count<1)
            {
                throw new Exception("GetMetadata：没能找到'ProductMetaData'节点！");
            }
            XmlNode node = nodelst[0];
            for (int i = 0; i < node.ChildNodes.Count; i++)
            {
                field2value.Add(node.ChildNodes[i].Name, node.ChildNodes[i].InnerText);
            }
            //添加xml文件中不存在的元数据信息
            field2value.Add("relatePath",relatePath);   //必须添加

            field2value.Add("NAME",Path.GetFileName(srcPath));

            return field2value;
        }

        /// <summary>
        /// 获取SJ9A数据存储的相对路径
        /// </summary>
        /// <returns></returns>
        public string GetSourceRelatePath()
        {
            return relatePath;
        }

        //指定缩略图文件，系统将制定的缩略图文件列表入库
        public string[] GetThnmbnail()
        {
            string[] jpgFiles = Directory.GetFiles(originalData, "*.jpg");
            return jpgFiles;
        }

        /// <summary>
        /// 指定键字段，此字段将作为判定一条记录是否存在的依据
        /// </summary>
        /// <returns></returns>
        public string GetKeyField()
        {
            return "NAME";
        }

        /// <summary>
        /// 解压缩.tar.gz文件,提取Xml、Jpg文件
        /// </summary>
        /// <param name="filePath">要解压的文件</param>
        /// <param name="OutputTmpPath">解压到的目录</param>
        private void DecompressXmlJpgFromTargzFile(string filePath, string OutputTmpPath)
        {

            //检查输入文件类型合法性
            if (filePath.ToLower().EndsWith(".tar.gz"))
            {
                OutputTmpPath = OutputTmpPath.TrimEnd('\\');
                string tarPath = null;
                if (!System.IO.Directory.Exists(OutputTmpPath))
                {
                    try
                    {
                        System.IO.Directory.CreateDirectory(OutputTmpPath);
                    }
                    catch
                    {
                        throw new Exception("MetaDataCompression:数据解压缩文件目录创建失败！");
                    }
                }
                try
                {
                    System.Diagnostics.Process p = new System.Diagnostics.Process();
                    p.StartInfo.FileName = System.AppDomain.CurrentDomain.BaseDirectory + "7za.exe";
                    //.tar.gz文件所在目录，作为第一次解压的输出目录
                    string dir = System.IO.Path.GetDirectoryName(filePath);
                    //解压
                    p.StartInfo.Arguments = string.Format("e {0} -o{1} * -aoa", filePath, dir);
                    p.StartInfo.CreateNoWindow = true;
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    p.Start();
                    p.WaitForExit();
                    //解压后的 *.tar文件，作为第二次解压的输入文件名
                    tarPath = System.IO.Path.GetDirectoryName(filePath) + "\\" + System.IO.Path.GetFileNameWithoutExtension(filePath);
                    //string tempFile = OutputTmpPath + "\\tempMid";
                    if (!System.IO.Directory.Exists(OutputTmpPath))
                    {
                        try
                        {
                            System.IO.Directory.CreateDirectory(OutputTmpPath);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("MetaDataCompression:" + ex.Message);
                        }
                    }
                    if (tarPath != null)
                    {
                        p.StartInfo.Arguments = string.Format("e {0} -o{1} *.xml -aoa ", tarPath, OutputTmpPath);

                        //p.StartInfo.Arguments = string.Format("x {0} -y  -aoa  -o{1} * ", tarPath, OutputTmpPath);                      
                        p.Start();
                        p.WaitForExit();
                        p.StartInfo.Arguments = string.Format("e {0} -o{1} *.jpg -aoa ", tarPath, OutputTmpPath);

                        //p.StartInfo.Arguments = string.Format("x {0} -y  -aoa  -o{1} * ", tarPath, OutputTmpPath);                      
                        p.Start();
                        p.WaitForExit();
                        DeleteDir(OutputTmpPath);
                        System.IO.File.Delete(tarPath);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("MetaDataCompression:" + ex.Message);
                }
            }
            else
            {
                throw new Exception("MetaDataCompression:压缩文件输入异常。");
            }
        }

        /// <summary>
        /// 删除文件夹内子目录
        /// </summary>
        /// <param name="dirRoot"></param>
        private void DeleteDir(string dirRoot)
        {
            DirectoryInfo dir = new DirectoryInfo(dirRoot);
            if (dir.Exists)
            {
                DirectoryInfo[] childs = dir.GetDirectories();
                foreach (DirectoryInfo child in childs)
                {
                    child.Delete(true);
                }
            }
        }


    }
}
