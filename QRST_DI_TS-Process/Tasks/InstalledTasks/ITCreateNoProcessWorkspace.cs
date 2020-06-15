using System.IO;

namespace QRST_DI_TS_Process.Tasks.InstalledTasks
{
    public class ITCreateNoProcessWorkspace : TaskClass
    {

        /// <summary>
        /// 任务名,定义唯一标识，不可动态修改
        /// </summary>
        public override string TaskName
        {
            get { return "ITCreateNoProcessWorkspace"; }
            set { }
        }

        public override void Process()
        {
            this.ParentOrder.Logs.Add("建立工作空间。");
            if (!Directory.Exists(this.ParentOrder.OrderWorkspace))
            {
                Directory.CreateDirectory(this.ParentOrder.OrderWorkspace);
            }
        }
    }
}
