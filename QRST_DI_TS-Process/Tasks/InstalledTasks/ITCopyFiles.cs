using System;
using System.IO;

namespace QRST_DI_TS_Process.Tasks.InstalledTasks
{
    public class ITCopyFiles:TaskClass
    {
        /// <summary>
        /// 任务名,定义唯一标识，不可动态修改
        /// </summary>
        public override string TaskName
        {
            get { return "ITCopyFiles"; }
            set { }
        }

        public override void Process()
        {
            //Copy Source Data
            string srcFilePath= this.ProcessArgu[0];
            string destFilePath = this.ProcessArgu[1];

            try
            {
                if (System.IO.File.Exists(srcFilePath))
                {

                    if (destFilePath != srcFilePath)
                    {
                        this.ParentOrder.Logs.Add(string.Format("正在拷贝数据..."));
                        string destDir = Path.GetDirectoryName(destFilePath);
                        if (!Directory.Exists(destDir))
                        {
                            Directory.CreateDirectory(destDir);
                        }
                        System.IO.File.Copy(srcFilePath, destFilePath,true);
                        this.ParentOrder.Logs.Add(string.Format("完成数据拷贝。"));
                    }
                }
            }
            catch (Exception ex)
            {
                this.ParentOrder.Logs.Add(string.Format("拷贝数据出现异常{0}", ex.Message));
            }
        }

    }
}
