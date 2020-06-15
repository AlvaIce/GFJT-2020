using System.IO;

namespace QRST_DI_TS_Process.Tasks.InstalledTasks
{
    /// <summary>
    /// 将打包好的瓦片数据移动到指定目录，供集成共享访问
    /// @zhangfeilong
    /// </summary>
    public class ITMoveZipToDestDir : TaskClass
    {
        /// <summary>
        /// 任务名,定义唯一标识，不可动态修改
        /// </summary>
        public override string TaskName
        {
            get { return "ITMoveZipToDestDir"; }
            set { }
        }

        public override void Process()
        {
            this.ParentOrder.Logs.Add(string.Format("移动瓦片数据压缩包到指定目录"));
            string zipFilePath = this.ProcessArgu[0];
            string resultTilesPath = this.ProcessArgu[1];
            moveFileToNewDir(zipFilePath, resultTilesPath);
            this.ParentOrder.Logs.Add(string.Format("完成移动瓦片数据压缩包"));
        }

        /// <summary>
        /// 移动目标文件到新的目录下
        /// </summary>
        /// <param name="sourceFile">源文件地址</param>
        /// <param name="destFilePath">目标目录</param>
        public void moveFileToNewDir(string sourceFile, string destFilePath)
        {
            if (!Directory.Exists(destFilePath))
            {
                Directory.CreateDirectory(destFilePath);
            }

            if (File.Exists(sourceFile))
            {
                FileInfo info = new FileInfo(sourceFile);
                File.Move(sourceFile, string.Format(@"{0}\{1}", destFilePath, info.Name));
            }
        }
    }
}
