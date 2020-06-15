﻿using System;
using System.IO;

namespace QRST_DI_TS_Process.Tasks.InstalledTasks
{
    /// <summary>
    /// Tar包解压任务类
    /// </summary>
    public class ITUnzipTargz4XmlJpg : TaskClass
    {

        /// <summary>
        /// Tar包解压任务名称
        /// </summary>
        public override string TaskName
        {
            get { return "ITUnzipTargz4XmlJpg"; }
            set { }
        }

        /// <summary>
        /// Tar包解压任务名称
        /// </summary>
        public override void Process()
        {

            string sourceFile = this.ProcessArgu[0];
            if (!Directory.Exists(sourceFile))
            {
                this.ParentOrder.Logs.Add(string.Format("没有找到订单工作空间"));
                throw new Exception("没有找到订单工作空间");
            }
            string destDir = this.ProcessArgu[1];
            string[] fileNames = Directory.GetFiles(sourceFile, "*.tar.*");
            if (fileNames.Length <= 0)
            {
                this.ParentOrder.Logs.Add(string.Format("没有找到BCD卫星源文件"));
                throw new Exception("没有找到卫星源文件");
            }
            sourceFile = fileNames[0];
            this.ParentOrder.Logs.Add(string.Format("解压原始数据中..."));

            //解压缩
            DecompressXmlJpgFromTarGzFile(sourceFile, destDir);
            this.ParentOrder.Logs.Add(string.Format("完成数据解压."));

        }

        /// <summary>
        /// 解压缩.tar.gz文件,提取Xml、Jpg文件
        /// </summary>
        /// <param name="filePath">待解压的文件</param>
        /// <param name="OutputTmpPath">解压到的目的文件夹</param>
        private void DecompressXmlJpgFromTarGzFile(string filePath, string OutputTmpPath)
        {

            //检查输入文件类型合法性
            if ((filePath.EndsWith(".tar.gz") || filePath.EndsWith(".TAR.GZ") || filePath.EndsWith(".TAR.gz")) &&
                !filePath.Contains("VIMS"))
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
                        p.StartInfo.Arguments = string.Format("e {0} -o{1} *.xml -aoa -r", tarPath, OutputTmpPath);

                        //p.StartInfo.Arguments = string.Format("x {0} -y  -aoa  -o{1} * ", tarPath, OutputTmpPath);                      
                        p.Start();
                        p.WaitForExit();
                        p.StartInfo.Arguments = string.Format("e {0} -o{1} *.jpg -aoa -r", tarPath, OutputTmpPath);

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
            else if (filePath.Contains("AHSI") || filePath.Contains("VIMS") || filePath.Contains("EMI"))
            {
                filePath = filePath.TrimEnd('\\');
                OutputTmpPath = OutputTmpPath.TrimEnd('\\');
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
                    //解压
                    p.StartInfo.Arguments = string.Format("e {0} -o{1} * -aoa", filePath, OutputTmpPath);
                    p.StartInfo.CreateNoWindow = true;
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    p.Start();
                    p.WaitForExit();
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