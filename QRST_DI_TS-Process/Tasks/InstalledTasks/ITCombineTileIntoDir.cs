using System;
using System.IO;

namespace QRST_DI_TS_Process.Tasks.InstalledTasks
{
    /// <summary>
    /// 将查询到的瓦片数据移动到一个目录下，供打包使用
    /// @zhangfeilong
    /// </summary>
    public class ITCombineTileIntoDir : TaskClass
    {
        /// <summary>
        /// 任务名,定义唯一标识，不可动态修改
        /// </summary>
        public override string TaskName
        {
            get { return "ITCombineTileIntoDir"; }
            set { }
        }

        public override void Process()
        {
            this.ParentOrder.Logs.Add(string.Format("开始移动瓦片数据到一个文件夹里。"));
            string tileSorcePath = this.ProcessArgu[0];
            string tileDataDir = this.ProcessArgu[1];
            moveFileToNewDir(tileSorcePath, tileDataDir);
            this.ParentOrder.Logs.Add(string.Format("完成移动"));
        }

        /// <summary>
        /// 将原始目录下的所有文件都移动到目标目录下
        /// </summary>
        /// <param name="sourceDir"></param>
        /// <param name="destDir"></param>
        private void moveFileToNewDir(string sourceDir, string destDir)
        {
            if (!Directory.Exists(destDir))
            {
                try
                {
                    Directory.CreateDirectory(destDir);
                }
                catch
                {
                    throw new Exception("CombineTileIntoDir:数据合并目录创建失败！");
                }
            }
            if (Directory.Exists(sourceDir))
            {
                DirectoryInfo dInfo = new DirectoryInfo(sourceDir);
                FileInfo[] fInfos = dInfo.GetFiles();
                foreach (FileInfo info in fInfos)
                {
                    if (File.Exists(info.FullName))
                    {
                        try
                        {
                            File.Move(info.FullName, string.Format(@"{0}\{1}", destDir, info.Name));
                        }
                        catch
                        {
                            throw new Exception("CombineTileIntoDir:数据移动到目标目录失败！");
                        }
                    }
                }
            }
        }

    }
}
