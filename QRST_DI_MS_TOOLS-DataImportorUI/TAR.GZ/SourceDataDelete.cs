using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using QRST_DI_DS_Metadata.Paths;
using System.Windows.Forms;
using System.Configuration;
using System.Text.RegularExpressions;
using QRST_DI_Resources;
using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetaData;
using QRST_DI_SS_DBInterfaces.IDBEngine;
 
namespace QRST_DI_MS_Component_DataImportorUI.TAR.GZ
{
    class SourceDataDelete
    {
        private String sourceDataPath = null;
        private String unImportedSourceDataPath = @ConfigurationManager.AppSettings["UnImportedData"]; 
        private List<String> needDeleteDataList = new List<String>();
        private static IDbOperating mySQLOperator = Constant.IdbOperating;
        IDbBaseUtilities MySqlBaseUti = mySQLOperator.GetSubDbUtilities(EnumDBType.EVDB);
        private List<String> allDataInDB = new List<string>();
        private FileInfo[] files = null;
        private UCImportTarGZ form;

        public SourceDataDelete(String sourceDataPath, UCImportTarGZ form)
        {
            this.sourceDataPath = sourceDataPath;
            this.form = form;
        }

        public SourceDataDelete()
        {

        }

        public String confirmDelete()
        {
            files = getAllFiles(sourceDataPath);
            if(files != null && files.Length > 0)
            {
                needDeleteDataList = TraverseSourceDataInDB(files);
            }
            DialogResult result = MessageBox.Show(String.Format("当前目录下共有{0}景数据，其中已经入库成功为{1}景数据，未入库或者未入库成功的有{2}景数据，是否继续执行删除操作", files.Length, needDeleteDataList.Count, files.Length-needDeleteDataList.Count), "删除确认", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                return deleteSourceData();
            }
            return null;
                
        }


        public String deleteSourceData()
        {
            foreach (FileInfo file in files)
            {
                if (needDeleteDataList.Contains(file.Name))
                {
                    file.Delete();
                    form.Addmessege1(string.Format("{0} 已经删除", file.Name)+"\n");
                }
                else
                {
                    //if(unImportedSourceDataPath != "")
                    //{
                    //    String destPath = unImportedSourceDataPath + @"\"+getTimeByDataName(file.Name);
                    //    if (!Directory.Exists(destPath))
                    //    {
                    //        Directory.CreateDirectory(destPath);
                    //    }
                    //    if (File.Exists(destPath + @"\" + file.Name))
                    //    {
                    //        File.Delete(destPath + @"\" + file.Name);
                    //    }
                    //    File.Move(file.FullName, destPath+@"\"+file.Name);
                    //    form.Addmessege1(string.Format("{0} 已经移到 {1}", file.Name, destPath + @"\" + file.Name) + "\n");
                    //}
                    form.Addmessege1(string.Format("{0} 已保留", file.Name) + "\n");
                }
            }
            return getReturnResult();
        }

        public String getTimeByDataName(String dataName)
        {
            String time = null;
            Match m = Regex.Match(dataName, @"\d{4}((?:0[13578]|1[02])(?:0[1-9]|[12]\d|3[01])|02(?:0[1-9]|[12]\d)|(?:0[469]|11)(?:0[1-9]|[12]\d|30))");
            if (m.Success)
            {
                time = m.Value;
            }
            return time;
        }

        private String getReturnResult()
        {
            String result = null;
            if (unImportedSourceDataPath != "")
            {
                result = String.Format("总共删除{0}景数据，{1}景数据移到{2}", needDeleteDataList.Count, files.Length - needDeleteDataList.Count, unImportedSourceDataPath)+"\n";
            }
            else
            {
                result = String.Format("总共删除{0}景数据，{1}景数据未入库或者入库没有成功", needDeleteDataList.Count, files.Length - needDeleteDataList.Count)+"\n";
            }
            return result;
        }

        public List<string> TraverseSourceDataInDB(FileInfo[] files)
        {
            List<string> result = new List<string>();
            int i = 0;
            String tableName = null;
            while (i < 7)
            {
                switch (i)
                {
                    case 0:
                        tableName = "prod_gf1";
                        break;
                    case 1:
                        tableName = "prod_hj";
                        break;
                    case 2:
                        tableName = "prod_zy3";
                        break;
                    case 3:
                        tableName = "prod_zy02c";
                        break;
                    case 4:
                        tableName = "prod_sj9a";
                        break;
                    case 5:
                        tableName = "prod_hj1c";
                        break;
                    case 6:
                        tableName = "prod_gf3";
                        break;
                    default:
                        break;
                }
                string SQLString = string.Format("select name from {0} where name in (", tableName);
                foreach(FileInfo file in files)
                {
                    SQLString = string.Format("{0}'{1}',", SQLString, file.Name);
                }
                SQLString = SQLString.TrimEnd().TrimEnd(',');
                SQLString += ")";

                //根据数据库名称获取库访问实例
                MySqlBaseUti = mySQLOperator.GetSubDbUtilities(EnumDBType.EVDB);

                DataSet returnDS = MySqlBaseUti.GetDataSet(SQLString);

                if (returnDS != null && returnDS.Tables.Count > 0)
                {
                    foreach (DataRow dr in returnDS.Tables[0].Rows)
                    {
                        result.Add(dr["name"].ToString());
                    }
                }
                i++;
            }
            return result;
        }

        public Boolean isDataImportCompelete(FileInfo file)
        {
            Boolean result = false;
            String importDataPath = getImportDataPath(getQrstCodeByDataName(file.Name));
            if (Directory.Exists(importDataPath))
            {
                DirectoryInfo dir = new DirectoryInfo(importDataPath);
                FileInfo[] importFiles = dir.GetFiles("*.tar.gz", SearchOption.AllDirectories);
                if (importFiles != null && importFiles.Length > 0 && importFiles.Length < 2)
                {
                    FileInfo importFile = importFiles[0];
                    if(importFile.Length == file.Length)
                    {
                        result = true;
                    }
                }
            }
            return result;
        }

        private String getImportDataPath(String qrstCode)
        {
            StoragePath storePath = new StoragePath(qrstCode);
            return storePath.GetDataPath(qrstCode);
        }

        private String getQrstCodeByDataName(String dataName)
        {
            String result = null;
            string presql = string.Format("select QRST_CODE from prod_gf1 where Name ='{0}'", dataName);
            DataSet ds = MySqlBaseUti.GetDataSet(presql);
            if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count > 0)
            {
                result = ds.Tables[0].Rows[0]["QRST_CODE"].ToString();
            }
            return result;
        }

        public FileInfo[] getAllFiles(String path)
        {
            if (Directory.Exists(path))
            {
                DirectoryInfo dir = new DirectoryInfo(path);
                FileInfo[] files = dir.GetFiles("*.tar.gz",SearchOption.AllDirectories);
                return files;
            }
            else {
                return null;
            }
        }
    }
}
