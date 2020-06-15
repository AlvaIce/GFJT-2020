using System;
using System.IO;

namespace QRST_DI_TS_Process.Tasks.InstalledTasks
{
    public class ITImportRasterFiles : TaskClass
    {
        /// <summary>
        /// 任务名,定义唯一标识，不可动态修改
        /// </summary>
        public override string TaskName
        {
            get { return "ITImportRasterFiles"; }
            set { }
        }

        public override void Process()
        {
            //Copy Source Data
            string srcFileDir = this.ProcessArgu[0];
            string destFileDir = this.ProcessArgu[1];

            try
            {
                if (System.IO.Directory.Exists(srcFileDir))
                {
                    if (!Directory.Exists(destFileDir))
                    {
                        Directory.CreateDirectory(destFileDir);
                    }
                    this.ParentOrder.Logs.Add(string.Format("正在入库数据..."));
                    CopyDirectory(srcFileDir, destFileDir);
                    this.ParentOrder.Logs.Add(string.Format("完成数据入库。"));
                }
            }
            catch (Exception ex)
            {
                this.ParentOrder.Logs.Add(string.Format("数据入库出现异常{0}", ex.Message));
            }
        }

        private void CopyDirectory(string srcdir, string desdir)
        {
            string folderName = srcdir.Substring(srcdir.LastIndexOf("\\") + 1);

            string desfolderdir = desdir + "\\" + folderName;

            if (desdir.LastIndexOf("\\") == (desdir.Length - 1))
            {
                desfolderdir = desdir + folderName;
            }
            string[] filenames = Directory.GetFileSystemEntries(srcdir);

            foreach (string file in filenames)// 遍历所有的文件和目录
            {
                if (Directory.Exists(file))// 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
                {

                    string currentdir = desfolderdir + "\\" + file.Substring(file.LastIndexOf("\\") + 1);
                    if (!Directory.Exists(currentdir))
                    {
                        Directory.CreateDirectory(currentdir);
                    }

                    CopyDirectory(file, desfolderdir);
                }

                else // 否则直接copy文件
                {
                    string srcfileName = file.Substring(file.LastIndexOf("\\") + 1);

                    srcfileName = desfolderdir + "\\" + srcfileName;


                    if (!Directory.Exists(desfolderdir))
                    {
                        Directory.CreateDirectory(desfolderdir);
                    }


                    File.Copy(file, srcfileName);
                }
            }//foreach
        }//function end
    }
}
