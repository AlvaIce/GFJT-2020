using System;
using System.IO;

namespace QRST_DI_TS_Process.Tasks.InstalledTasks
{
    public class ITGetDataFromJCGX : TaskClass
    {
        //从集成共享获取数据  zxw 2013/4/15,两种方式
        public override string TaskName
        {
            get { return "ITGetDataFromJCGX"; }
            set { }
        }

        public override void Process()
        {
            string pathID = this.ProcessArgu[0];    //源数据的文件路径，包括文件名
            string destPath = this.ProcessArgu[1];   //数据的目标路径

            this.ParentOrder.Logs.Add("开始获取数据！");
            
        }

        //通过局域网内部拷贝
        void GetDataByCopy(string srcPath,string destPath)
        {
            if (File.Exists(srcPath))
            {
                try
                {
                    string fileName = Path.GetFileName(srcPath);
                    destPath = string.Format(@"{0}\{1}",destPath,fileName);
                    File.Copy(srcPath,destPath,true);
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                throw new Exception("文件不存在！");
            }
        }

    }
}
