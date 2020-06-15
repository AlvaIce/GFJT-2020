using System.Diagnostics;

namespace QRST_DI_TS_Process.Tasks
{
    public class CustomizedTaskClass:TaskClass
    {
        public override string TaskName
        {
            get;
            set;
        }

        public CustomizedTaskClass()
        :base()
        {
            TaskProcess = null;
            SuspendAvailable = false;
        }
        public Process TaskProcess { get; protected set; }
        public override void Process()
        {
            base.Process();
            Status = EnumTaskStatus.Processing;
            TaskProcess = new Process();//实例
            TaskProcess.StartInfo.CreateNoWindow = true;//设定不显示窗口
            TaskProcess.StartInfo.UseShellExecute = false;
            TaskProcess.StartInfo.FileName = ProcessExec; //设定程序名  
            TaskProcess.StartInfo.RedirectStandardInput = true;   //重定向标准输入
            TaskProcess.StartInfo.RedirectStandardOutput = true;  //重定向标准输出
            TaskProcess.StartInfo.RedirectStandardError = true;//重定向错误输出
            TaskProcess.StartInfo.Arguments = ProcessArgus(ProcessArgu);//重定向错误输出
            TaskProcess.Start();
            //TaskProcess.StandardInput.WriteLine("ipconfig");//执行的命令
            //TaskProcess.StandardInput.WriteLine("exit");
            TaskProcess.WaitForExit();
            TaskProcess.Close();

            getTaskResult();
        }

        private void getTaskResult()
        {
            object rst = base.TaskResult;

            string taskresultPath = string.Format(@"{0}\TaskResult.txt", System.IO.Path.GetDirectoryName(TaskWorkspace));
            if (System.IO.File.Exists(taskresultPath))
            {
                rst = System.IO.File.ReadAllText(taskresultPath);
            }

            this.TaskResult = rst;
        }        

        private string ProcessArgus(string[] pArgus)
        {
            string args = "";
            foreach (string obj in pArgus)
            {
                args += string.Format("\"{0}\" ", obj.ToString());
            }
            return args;
        }

        public override void Suspend()
        {
            base.Suspend();
        }

        public override void Cancel()
        {
            TaskProcess.Kill();
            base.Cancel();
        }

        public override int GetProgressInfo()
        {
            int basereturn = base.GetProgressInfo();

            int progressinfo = -1;
            string progressfilepath = string.Format(@"{0}\ProgressInfo.txt", System.IO.Path.GetDirectoryName(ProcessExec));
            if (System.IO.File.Exists(progressfilepath))
            {
                string[] alllines = System.IO.File.ReadAllLines(progressfilepath);
                for (int i = alllines.Length - 1; i < 0; i--)
                {

                    if (int.TryParse(alllines[i].Trim(), out progressinfo))
                    {
                        break;
                    }
                }
            }
            return progressinfo;

        }
    }
}
