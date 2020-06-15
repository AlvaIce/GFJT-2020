using System;
using QRST_DI_TS_Basis.DirectlyAddress;
using System.IO;
using QRST_DI_DS_Metadata.MetaDataCls;
using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetaData;
using QRST_DI_SS_DBInterfaces.IDBEngine;
using QRST_DI_Resources;

namespace QRST_DI_TS_Process.Tasks.InstalledTasks
{
    public class ITStoreGFCorData : TaskClass
    {
        private static IDbOperating _sqLiteOperating = Constant.IdbOperating;
        private IDbBaseUtilities _baseUtilities;
        /// <summary>
        /// 任务名,定义唯一标识，不可动态修改
        /// </summary>
        public override string TaskName
        {
            get { return "ITStoreGFCorData"; }
            set { }
        }

        public override void Process()
        {

            //数据阵列 数据归档
            //  迁移数据至归档目录
            this.ParentOrder.Logs.Add(string.Format("开始源数据归档。"));
            string orderworkspace = ProcessArgu[0];                               //订单工作空间

            try
            {
                MetaDataGF1 metadataGf1 = new MetaDataGF1();
                string[] files = Directory.GetFiles(orderworkspace, "*.tar.gz");
                if (files.Length > 0)
                {
                    ParentOrder.Logs.Add("开始拷贝校正后数据！");
                    metadataGf1.Name = Path.GetFileName(files[0]);
                    string destCorrectedData = metadataGf1.GetCorrectedDataPath();
                    if (Directory.Exists(destCorrectedData))
                    {
                        Directory.Delete(destCorrectedData, true);
                    }
                    CopyFolder(StorageBasePath.SharePath_CorrectedData(orderworkspace), destCorrectedData);
                    ParentOrder.Logs.Add("开始校正后数据信息入库！");
                    //如果纠正归档数据目录存在且里面有文件则1，否则为-1
                    string corDataPath = (Directory.Exists(destCorrectedData) && Directory.GetFiles(destCorrectedData).Length > 1) ? "1" : "-1";
                    string updatesql = string.Format("update prod_gf1 set CorDataFlag = {0} where Name = '{1}'", corDataPath,metadataGf1.Name);
                    _baseUtilities = _sqLiteOperating.GetSubDbUtilities(EnumDBType.EVDB);
                    _baseUtilities.ExecuteSql(updatesql);
                }
                ParentOrder.Logs.Add("完成校正后数据入库！");

                //是否要删除订单文件夹
                //  DeleteFolder(orderworkspace);
            }
            catch (Exception ex)
            {
                ParentOrder.Logs.Add(ex.ToString());
                throw ex;
            }

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


        public void DeleteFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                return;
            }
            else
            {
                string[] files = Directory.GetFiles(path);
                for (int i = 0; i < files.Length;i++ )
                {
                    FileInfo fi = new FileInfo(files[i]);
                    fi.IsReadOnly = false;
                    File.Delete(files[i]);
                }
                string[] folders = Directory.GetDirectories(path);
                for (int j = 0; j < folders.Length;j++ )
                {
                    DeleteFolder(folders[j]);
                }
                Directory.Delete(path,true);
            }
        }
    }
}
