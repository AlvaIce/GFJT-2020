using QRST_DI_DS_Basis;

namespace QRST_DI_TS_Process.Tasks.InstalledTasks
{
    public class ITUnzipZipFile : TaskClass
    {
        /// <summary>
        /// 任务名,定义唯一标识，不可动态修改
        /// </summary>
        public override string TaskName
        {
            get { return "ITUnzipZipFile"; }
            set { }
        }

        public override void Process()
        {
            string sourceFile = this.ProcessArgu[0];
            string destDir = this.ParentOrder.OrderWorkspace;
            this.ParentOrder.Logs.Add("开始解压数据文件");
            DataPacking.unZipFile(sourceFile,destDir);
            this.ParentOrder.Logs.Add("完成数据文件解压！");
        }
    }
}
