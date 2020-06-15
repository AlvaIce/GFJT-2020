using System;
using QRST_DI_DS_Metadata.MetaDataCls;
using System.IO;

namespace QRST_DI_TS_Process.Tasks.InstalledTasks
{
    /// <summary>
    /// 根据数据的QRST_CODE找到对应的源文件，拷贝到工作空间下
    /// </summary>
    public class ITPerareGF1Data : TaskClass
    {
        /// <summary>
        /// 任务名,定义唯一标识，不可动态修改
        /// </summary>
        public override string TaskName
        {
            get { return "ITPerareGF1Data"; }
            set { }
        }

        public override void Process()
        {
            string qrst_code = this.ProcessArgu[0];
            string destPath = this.ProcessArgu[1];
            this.ParentOrder.Logs.Add("正在查找源数据！");
            string srcPath =MetaData.GetDataAddress(qrst_code); 
            if(srcPath == "-1")
            {
                throw new Exception("没有找到源文件目录！");
            }
            string[] files = Directory.GetFiles(srcPath,"*.tar.gz");
            if(files.Length == 0)
            {
                 throw new Exception("没有找到源文件！");
            }
            //zsm 20161022 我个人认为下面四行代码应该改成注释的 因为数据不一定是一条
            //foreach (string scr in files)
            //{
            //    destPath = string.Format(@"{0}\{1}", destPath, Path.GetFileName(scr));
            //    File.Copy(scr, destPath);
            //    this.ParentOrder.Logs.Add("源数据拷贝完成！");

            //}
            string srcGFFile = files[0];
            destPath = string.Format(@"{0}\{1}",destPath,Path.GetFileName(files[0]));
            File.Copy(files[0],destPath);
            this.ParentOrder.Logs.Add("源数据拷贝完成！");

           
        }
    }
    
}
