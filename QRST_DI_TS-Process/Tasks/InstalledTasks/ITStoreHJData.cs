using System;
using System.IO;
using QRST_DI_TS_Basis.DirectlyAddress;

namespace QRST_DI_TS_Process.Tasks.InstalledTasks
{
    public class ITStoreHJData : TaskClass
    {

        /// <summary>
        /// 任务名,定义唯一标识，不可动态修改
        /// </summary>
        public override string TaskName
        {
            get { return "ITStoreHJData"; }
            set { }
        }

        public override void Process()
        {

            //数据阵列 数据归档
            //  迁移数据至归档目录
            this.ParentOrder.Logs.Add(string.Format("开始源数据产品归档。"));
            string orderworkspace = ProcessArgu[0];
            string sourceFilePath = ProcessArgu[1];
            

            try
            {
                //数据阵列 数据归档

                //  迁移数据至归档目录

                try
                { CopyFolder(orderworkspace, StorageBasePath.StorePath_SourceProject(sourceFilePath)); }
                catch (Exception ex) {
                    ParentOrder.Logs.Add(ex.ToString());
                }

                try
                { Directory.Delete(StorageBasePath.SharePath_OrignalData(orderworkspace), true); }
                catch (Exception ex)
                {
                    ParentOrder.Logs.Add(ex.ToString());
                }

                try
                { Directory.Delete(orderworkspace, true); }
                catch (Exception ex) {
                    ParentOrder.Logs.Add(ex.ToString());
                }

            }
            catch (Exception ex)
            {
                ParentOrder.Logs.Add(ex.ToString());
            }

            this.ParentOrder.Logs.Add(string.Format("完成源数据产品归档。"));

        }

        /// <summary>
        /// Copy文件夹
        /// </summary>
        /// <param name="sPath">源文件夹路径</param>
        /// <param name="dPath">目的文件夹路径</param>
        /// <returns>完成状态：success-完成；其他-报错</returns>
        public string CopyFolder(string sPath, string dPath)
        {
            string flag = "success";
            try
            {
                // 创建目的文件夹
                if (!Directory.Exists(dPath))
                {
                    Directory.CreateDirectory(dPath);
                }

                // 拷贝文件
                DirectoryInfo sDir = new DirectoryInfo(sPath);
                FileInfo[] fileArray = sDir.GetFiles();
                foreach (FileInfo file in fileArray)
                {
                    file.CopyTo(dPath + "\\" + file.Name, true);
                }

                // 循环子文件夹
                DirectoryInfo dDir = new DirectoryInfo(dPath);
                DirectoryInfo[] subDirArray = sDir.GetDirectories();
                foreach (DirectoryInfo subDir in subDirArray)
                {
                    //原本是“//”，现在改成“\\”，之后有待验证。
                    CopyFolder(subDir.FullName, dPath + "\\" + subDir.Name);
                }
            }
            catch (Exception ex)
            {
                flag = ex.ToString();
            }
            return flag;
        }
    }
}
