namespace QRST_DI_TS_Process.Tasks.InstalledTasks
{
    public class ITSendMessage:TaskClass
    {
        /// <summary>
        /// 任务名,定义唯一标识，不可动态修改
        /// </summary>
        public override string TaskName
        {
            get { return "ITSendMessage"; }
            set { }
        }

        public override void Process()
        {
            //王栋消息中心负责消息传递，原与预处理消息暂停 joki 131129
            //System.Threading.Tasks.Task task = new System.Threading.Tasks.Task(new Action<object>(sendmessge),(object)ProcessArgu);
            //task.Start();

            ////发消息给预处理
            //if (ReferenceTheInstances.MessageNoticer == null)
            //{
            //    ReferenceTheInstances.MessageNoticer = new NoticeMessager();
            //}

            //ReferenceTheInstances.MessageNoticer.SendMessage(remoteIP, msg);     //给本机发是否有问题？


            string remoteIP = this.ProcessArgu[0];
            this.ParentOrder.Logs.Add(string.Format("已消息告知{0}。", remoteIP));
            this.ParentOrder.Status = Orders.EnumOrderStatusType.Suspended;
        }

        private void sendmessge(object obj)
        {
            //已无独立预处理机器，都是本机响应。待王栋完成消息中心后，更新为给王栋中心发消息。
            string[] ProcessArgu = obj as string[];
            string remoteIP = ProcessArgu[0];
            string msg = ProcessArgu[1];

            //"P201221212?\\192.179.12.12\adsf\adfa\adsf\?@NoticePreprocess"
            string[] strs = msg.Split("?".ToCharArray());

            string orderCoder = strs[0];
            string sourceDir = strs[1];

            System.Windows.Forms.MessageBox.Show(string.Format("消息通知：收到新任务{0}！", orderCoder));

            if (System.IO.Directory.Exists(sourceDir))
            {
                System.Diagnostics.Process.Start("explorer.exe", sourceDir);
            }

        }


    }
}
