namespace QRST_DI_TS_Process.Tasks.InstalledTasks
{
    public class ITOutputLoginfo : TaskClass
    {

        /// <summary>
        /// 任务名,定义唯一标识，不可动态修改
        /// </summary>
        public override string TaskName { get { return "ITOutputLoginfo"; } set { } }

        public override void Process()
        {
            string loginfo = this.ProcessArgu[0];

            this.ParentOrder.Logs.Add(string.Format("{0}", loginfo));
        }
    }
}
