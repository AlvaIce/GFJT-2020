using System;
using System.IO;

namespace QRST_DI_TS_Process.Tasks.InstalledTasks
{
    /// <summary>
    /// 用于解压tar包
    /// </summary>
    public class ITUnzipTarFile : TaskClass
    {
        /// <summary>
        /// 任务名,定义唯一标识，不可动态修改
        /// </summary>
        public override string TaskName
        {
            get { return "ITUnzipTarFile"; }
            set { }
        }

        public override void Process()
        {
            string sourceFile = this.ProcessArgu[0];
            string destDir = sourceFile;
            Exception exResult = null;    //用于记录任务是否发生异常

            try
            {
                this.ParentOrder.Logs.Add(string.Format("解压原始数据中..."));
                //检查解压的文件是否存在,
                this.ParentOrder.Logs.Add("开始解析压缩文件！");
                string[] files = Directory.GetFiles(sourceFile, "*.tar");
                if (files.Length == 0)
                {
                    throw new Exception("没有找到文件！");
                }
                //解压
                DecompressTarFile(files[0], destDir);
                this.ParentOrder.Logs.Add(string.Format("完成数据解压."));
            }
            catch (Exception ex)
            {
                exResult = ex;
                throw ex;
            }
            finally
            {
                //是否发送消息
                //if(exResult == null)
                //{
                //}
            }
        }

        /// <summary>
        /// 解压缩.tar文件
        /// </summary>
        /// <param name="filePath">要解压的文件</param>
        /// <param name="OutputTmpPath">解压到的目录</param>
        public static void DecompressTarFile(string filePath, string OutputTmpPath)
        {

            //检查输入文件类型合法性
            if (filePath.EndsWith(".tar") || filePath.EndsWith(".TAR"))
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
                        throw new Exception("数据解压缩文件目录创建失败！");
                    }
                }
                try
                {
                    System.Diagnostics.Process p = new System.Diagnostics.Process();
                    p.StartInfo.FileName = System.AppDomain.CurrentDomain.BaseDirectory + "7za.exe";
                    //.tar文件所在目录，作为第一次解压的输出目录
                    string dir = System.IO.Path.GetDirectoryName(filePath);
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
    }
}
