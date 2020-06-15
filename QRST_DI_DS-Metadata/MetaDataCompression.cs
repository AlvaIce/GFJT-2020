using System;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

namespace QRST_DI_DS_Metadata
{
    internal class MetaDataCompression
    {
        //解压文件临时目录
        public static string OutputTmpPath = Path.GetTempPath();

        public MetaDataCompression()
        { }

        /// <summary>
        /// 从数据压缩文件里解压元数据描述文件（.xml .odl等）
        /// </summary>
        /// <param name="dataType">数据类型</param>
        /// <param name="filePath">数据压缩文件路径</param>
        /// <returns>解压出来的元数据描述文件路径</returns>
        public string Decompression(EnumMetadataTypes dataType, string filePath)
        {
            string metaInfoFilePath = "";

            switch (dataType)
            {
                case EnumMetadataTypes.Unknown:
                    break;
                case EnumMetadataTypes.MODIS:
                    break;
                case EnumMetadataTypes.CBERS:
                    metaInfoFilePath = DecompressCBERS(filePath);
                    break;
                case EnumMetadataTypes.HJ:
                    metaInfoFilePath = DecompressHJ(filePath);
                    break;
                default:
                    break;
            }

            return metaInfoFilePath;
        }

        /// <summary>
        /// 解压缩CBERS的zip格式的压缩文件，返回文件中的xml或odl文件
        /// </summary>
        /// <param name="filePath">zip文件路径</param>
        /// <returns>xml或odl文件路径</returns>
        public string DecompressCBERS(string filePath)
        {
            string metaInfoFilePath = "";
            //检查输入文件类型合法性
            string extension = Path.GetExtension(filePath).ToLower();
            if (Path.GetExtension(filePath).ToLower() == ".zip" || Path.GetExtension(filePath).ToLower() == ".ZIP")
            {
                metaInfoFilePath = DecompressZipFile(filePath);
            }
            else if (Path.GetExtension(filePath).ToLower() == ".tar.gz" || Path.GetExtension(filePath).ToLower() == ".gz" || Path.GetExtension(filePath).ToLower() == ".TAR.GZ")
            {
                metaInfoFilePath = DecompressTarGzFile(filePath);
            }
            else
            {
                throw new Exception("MetaDataCompression:CBERS数据压缩文件输入异常。");
            }
            return metaInfoFilePath;
        }
        /// <summary>
        /// 解压缩zip文件内的odl文件
        /// </summary>
        /// <param name="filePath">解压缩文件路径</param>
        /// <returns>对应的odl路径</returns>
        public string DecompressZipFile(string filePath)
        {
            string metaInfoFilePath = "";
            ZipFile zipfile = null;
            string name = null;
            int index;
            string suffix = null;
            //检查输入文件类型合法性
            if (Path.GetExtension(filePath).ToLower() == ".zip")
            {
                //如果压缩包里存在.xml文件则解压.xml文件到目录OutputTmpPath；
                zipfile = new ZipFile(filePath);
                foreach (ZipEntry ze in zipfile)
                {
                    name = ze.Name;
                    index = name.IndexOf(".");
                    suffix = name.Substring(index + 1).ToLower();
                    if (suffix == "odl")
                    {
                        if (ze.Name.Contains("/"))
                        {
                            name = ze.Name.Substring(ze.Name.LastIndexOf("/") + 1);
                        }
                        Stream inPut = zipfile.GetInputStream(ze);
                        metaInfoFilePath = OutputTmpPath + "\\" + name;
                        if (!File.Exists(metaInfoFilePath))
                        {
                            Stream outPut = new FileStream(metaInfoFilePath, FileMode.Create);
                            byte[] buf = new byte[1024];
                            int line = 0;
                            while ((line = inPut.Read(buf, 0, 1024)) > 0)
                            {
                                outPut.Write(buf, 0, line);
                            }
                            inPut.Close();
                            outPut.Close();
                        }
                    }
                    else
                    {
                        continue;
                    }
                    zipfile.Close();
                }

                if (metaInfoFilePath == "")
                {
                    throw new Exception("MetaDataCompression:CBERS数据压缩文件内容异常，不存在元数据信息文件。");
                }
            }
            else
            {
                throw new Exception("MetaDataCompression:CBERS数据压缩文件输入异常。");
            }
            return metaInfoFilePath;

        }
        /// <summary>
        /// 解压.tar.gz文件内的XML文件到指定目录
        /// </summary>
        /// <param name="inDir">解压缩文件路径</param>
        /// <returns>xml文件路径</returns>
        public string DecompressHJ(string filePath)
        {
            return DecompressTarGzFile(filePath);
        }

        /// <summary>
        /// 将.tar.gz文件中的XML文件解压到指定目录
        /// </summary>
        /// <param name="filePath">.tar.gz文件</param>
        /// <returns>XML文件输出目录</returns>
        private string DecompressTarGzFile(string filePath)
        {
            string metaInfoFilePath = "";
            //检查输入文件类型合法性
            if (filePath.EndsWith(".tar.gz") || filePath.EndsWith(".TAR.GZ") || filePath.EndsWith(".TAR.gz"))
            {
                filePath = filePath.TrimEnd('\\');
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
                    string tempFile = OutputTmpPath + "\\tempMid";
                    if (!System.IO.Directory.Exists(tempFile))
                    {
                        try
                        {
                            System.IO.Directory.CreateDirectory(tempFile);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("MetaDataCompression:" + ex.Message);
                        }
                    }
                    if (tarPath != null)
                    {
                        p.StartInfo.Arguments = string.Format("e {0} -r -o{1} *.xml -aoa ", tarPath, tempFile);
                        p.Start();
                        p.WaitForExit();
                        string[] allFiles = Directory.GetFiles(tempFile, "*.xml", SearchOption.AllDirectories);
                        string fileName = Path.GetFileNameWithoutExtension(filePath).Split('.')[0];
                        //fileName = fileName.Split('-')[fileName.Split('-').Length-1]; 不知这里为什么还要分，难道文件1A和1B的XML命名不一样？
                        foreach (string s in allFiles)
                        {
                            if (fileName.Contains(Path.GetFileNameWithoutExtension(s)))
                            {
                                metaInfoFilePath = OutputTmpPath + s.Substring(s.LastIndexOf("\\"));
                                if (!File.Exists(metaInfoFilePath))
                                {
                                    File.Move(s, metaInfoFilePath);
                                    break;
                                }
                                break;
                            }
                        }
                        //删除第一次解压的*.tar文件
                        System.IO.File.Delete(tarPath);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("MetaDataCompression:" + ex.Message);
                }

                if (metaInfoFilePath == "")
                {
                    throw new Exception("MetaDataCompression:压缩文件内容异常，不存在XML文件。");
                }
            }
            else
            {
                throw new Exception("MetaDataCompression:压缩文件输入异常。");
            }
            return metaInfoFilePath;
        }


    }
}
