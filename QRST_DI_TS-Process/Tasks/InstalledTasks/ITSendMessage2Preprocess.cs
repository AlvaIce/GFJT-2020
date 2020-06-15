using QRST_DI_MS_Basis;

namespace QRST_DI_TS_Process.Tasks.InstalledTasks
{
    public class ITSendMessage2Preprocess : TaskClass
    {
        /// <summary>
        /// 任务名,定义唯一标识，不可动态修改
        /// </summary>
        public override string TaskName
        {
            get { return "ITSendMessage2Preprocess"; }
            set { }
        }

        public override void Process()
        {
            string remoteIP = this.ProcessArgu[0];
            string msg = this.ProcessArgu[1];

            //发消息给预处理
            if (ReferenceTheInstances.MessageNoticer == null)
            {
                ReferenceTheInstances.MessageNoticer = new NoticeMessager();
            }

            ReferenceTheInstances.MessageNoticer.SendMessage(remoteIP, msg);     //给本机发是否有问题？

            this.ParentOrder.Logs.Add(string.Format("已消息告知{0}。", remoteIP));

        }


    }
}
