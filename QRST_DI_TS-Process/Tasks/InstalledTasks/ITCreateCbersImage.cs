using System;
using System.IO;

namespace QRST_DI_TS_Process.Tasks.InstalledTasks
{
    public class ITCreateCbersImage : TaskClass
    {
        /// <summary>
        /// 任务名,定义唯一标识，不可动态修改
        /// </summary>
        public override string TaskName
        {
            get { return "ITCreateCbersImage"; }
            set { }
        }

        public override void Process()
        {
            string sourceFilePath = this.ProcessArgu[0];
            string destDir = this.ProcessArgu[2];
            string workspace = this.ProcessArgu[1];
            
            try
            {
                DecompressTarGzFile(sourceFilePath, workspace);
                string imagePath = workspace + Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(sourceFilePath)) + ".jpg";
                string thumbPath = workspace + Path.GetFileNameWithoutExtension(imagePath) + "-THUMB.jpg";
                if (!Directory.Exists(destDir))
                {
                    Directory.CreateDirectory(destDir);
                }
                this.ParentOrder.Logs.Add(string.Format("缩略图数据入库中..."));
                if (File.Exists(imagePath))
                {
                    File.Copy(imagePath, destDir + Path.GetFileName(imagePath));
                }
                if (File.Exists(thumbPath))
                {
                    File.Copy(thumbPath, destDir + Path.GetFileName(thumbPath));
                }
                Directory.Delete(workspace, true);
                this.ParentOrder.Logs.Add(string.Format("缩略图数据入库完成..."));
            }
            catch (System.Exception ex)
            {
            	this.ParentOrder.Logs.Add(ex.Message);
            }
            
        }


        /// <summary>
        /// 解压缩.tar.gz文件
        /// </summary>
        /// <param name="filePath">要解压的文件</param>
        /// <param name="OutputTmpPath">解压到的目录</param>
        private void DecompressTarGzFile(string filePath, string OutputTmpPath)
        {

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
                        p.StartInfo.Arguments = string.Format("e {0} -o{1} * -aoa ", tarPath, OutputTmpPath);

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
