using System;
using QRST_DI_DS_Metadata.Paths;
using QRST_DI_DS_Metadata.MetaDataDefiner.Dal;
using System.IO;
using System.Data;
using QRST_DI_DS_Basis;
using QRST_DI_SS_DBInterfaces.IDBEngine;

namespace QRST_DI_DS_Metadata.MetaDataCls
{
    public abstract class MetaData
    {
        public bool IsCreated
        {
            get;
            set;
        }

        protected EnumMetadataTypes _dataType;
        public EnumMetadataTypes DataType
        {
            get { return _dataType; }
        }

        public string QRST_CODE
        {
            set;
            get;
        }

        public MetaData() { }



        public virtual void ReadAttributes(string fileName) { }   //读取属性数据
        public virtual void ImportData(IDbBaseUtilities sqlBase) { }//存储属性数据
        public virtual void GetModel(string qrst_code, IDbBaseUtilities sqlBase) { IsCreated = true; }      //根据qrst_code从数据库中获取元数据对象实，若没有获取到记录则返回false

        /// <summary>
        /// 获取数据存储的相对路径,若以元数据信息组成路径，则需从写此方法，若按照树结构组织，则返回“”。zxw 20130615
        /// </summary>ru
        /// <returns></returns>
        public virtual string GetRelateDataPath() 
        {
            return "";
        }

        /// <summary>
        /// 获取数据入库前的文件名称 joki 170305 对于重命名为.gnf的一般文件，下载时需要该信息
        /// </summary>ru
        /// <returns></returns>
        public virtual string GetRelateDataPath(out string originalFilename)
        {
            string relPath = GetRelateDataPath();
            originalFilename = "";
            try
            {
                originalFilename = Path.GetFileName(relPath);
            }
            catch (Exception ex)
            {
                originalFilename = "";
            }
            return relPath;
        }

        /// <summary>
        /// 根据数据编码，删除对应的元数据与文件数据,“0001-ISDB-31-5”,返回-1表示没有要找到对应的数据，否则返回成功删除的记录数
        /// </summary>
        /// <param name="dataCode"></param>
        public static int DeleteData(string dataCode)
        {
            try
            {
                //获取数据应该存放的完整路径
                string tableCode = StoragePath.GetTableCodeByQrstCode(dataCode);
                StoragePath storePath = new StoragePath(tableCode);
                string destPath = storePath.GetDataPath(dataCode);

                tablecode_Dal tablecodeDal = new tablecode_Dal(storePath.GetMysqlBaseUtilities());
                string tableName = tablecodeDal.GetTableName(tableCode);
                string deletesql = string.Format(" delete from {0} where QRST_CODE = '{1}'", tableName, dataCode);
                int i = storePath.GetMysqlBaseUtilities().ExecuteSql(deletesql);
                storePath.GetMysqlBaseUtilities().ExecuteSql(deletesql);
                if (Directory.Exists(destPath))
                {
                    //删除文件夹目录以及对应的文件,首先将只读文件属性改为normal
                    DirectoryInfo dr = new DirectoryInfo(destPath);
                    FileInfo[] fileArr = dr.GetFiles();
                    for (int j = 0; j < fileArr.Length;j++ )
                    {
                        if (fileArr[j].Attributes.ToString().IndexOf("ReadOnly")!= -1)
                        {
                            fileArr[j].Attributes = FileAttributes.Normal;
                        }
                    }
                    Directory.Delete(destPath,true);
                }
                if(i==0)
                {
                    return -1;
                }
                else
                {
                    return i;
                }
            }
            catch(Exception ex)
            {
                return 0;
            }
          
        }

        /// <summary>
        /// 根据数据编码，返回该编码对应的元数据记录
        /// </summary>
        /// <param name="dataCode">“0001-ISDB-31-5”</param>
        /// <returns></returns>
        public static DataSet GetDataInfo(string dataCode)
        {
            try
            {
                //获取数据应该存放的完整路径
                string tableCode = StoragePath.GetTableCodeByQrstCode(dataCode);
                StoragePath storePath = new StoragePath(tableCode);
                tablecode_Dal tablecodeDal = new tablecode_Dal(storePath.GetMysqlBaseUtilities());
                string tableName = tablecodeDal.GetTableName(tableCode);
                string viewName = "";
                //信息服务库“ISDB”中因为要返回关联公共的publicinfo表信息，因此建立的是关联视图
                if (tableCode.StartsWith("ISDB"))
                {
                    viewName = string.Format("{0}_isdb_publicinfo_view", tableName);
                }
                else
                {
                    viewName = string.Format("{0}_view",tableName);
                }
                string searchSql = string.Format(" select * from {0} where QRST_CODE = '{1}'", viewName, dataCode);
                return storePath.GetMysqlBaseUtilities().GetDataSet(searchSql);
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        /// <summary>
        /// 根据数据编码，返回该数据的存放地址，若该数据不存在，则返回“-1”，否则返回路径地址
        /// </summary>
        /// <param name="dataCode"></param>
        /// <returns></returns>
        public static string GetDataAddress(string dataCode)
        {
            try
            {
                string tableCode = StoragePath.GetTableCodeByQrstCode(dataCode);
                StoragePath storePath = new StoragePath(tableCode);
                string destPath = storePath.GetDataPath(dataCode);
                if (Directory.Exists(destPath))
                {
                    return destPath;
                }
                else if (File.Exists(destPath))
                {
                    return destPath;
                }
                else
                {
                    return "-1";
                }
            }
            catch (Exception ex)
            {
                return "0";
            }
        }

        /// <summary>
        /// 根据数据编码，返回该数据的存放地址，若该数据不存在，则返回“-1”，否则返回路径地址
        /// </summary>
        /// <param name="dataCode"></param>
        /// <returns></returns>
        public static string GetDataAddress(string dataCode,out string originalfilename)
        {
            originalfilename = "";

            try
            {
                string tableCode = StoragePath.GetTableCodeByQrstCode(dataCode);
                StoragePath storePath = new StoragePath(tableCode);
                string destPath = storePath.GetDataPath(dataCode);
                originalfilename = storePath.OriginalFilename;
                if (Directory.Exists(destPath))
                {
                    return destPath;
                }
                else if (File.Exists(destPath))
                {
                    return destPath;
                }
                else
                {
                    return "-1";
                }
            }
            catch (Exception ex)
            {
                return "0";
            }
        }

        /// <summary>
        /// 数据下载
        /// </summary>
        /// <param name="dataCode">需要下载的数据编码</param>
        /// <param name="remotePath">下载的目标路径</param>
        /// <param name="transferType">数据是否打包下载</param>
        /// <returns>若下载的数据存在，则返回true,否则返回false</returns>
        public static bool DownLoadData(string dataCode,string remotePath,bool isPacking)
        {
            string srcPath = GetDataAddress(dataCode);
            if(Directory.Exists(srcPath))
            {
                string[] files = Directory.GetFileSystemEntries(srcPath);
                if (files.Length > 1)   //将数据打包
                {
                    if (isPacking)
                    {
                        DirectoryInfo dirInfo = new DirectoryInfo(srcPath);
                        string zipname = string.Format("{0}{1}.zip", remotePath, dirInfo.Name);
                        DataPacking.ZipFile(srcPath, zipname);
                    }
                    else
                    {
                        remotePath = remotePath.TrimEnd("\\".ToCharArray());
                        string result = CopyFolder(srcPath,remotePath);
                        if (!result.Equals("success"))
                        {
                            return false;
                        }
                    }
                 
                    return true;
                }
                else if (files.Length <= 0)
                {
                    return false;
                }
                else
                {
                    string destPath = string.Format(@"{0}{1}",remotePath,Path.GetFileName(files[0]));
                    File.Copy(files[0],destPath,true);
                    return true;
                }
            }
            return false;
            
        }

        /// <summary>
        /// Copy文件夹
        /// </summary>
        /// <param name="sPath">源文件夹路径</param>
        /// <param name="dPath">目的文件夹路径</param>
        /// <returns>完成状态：success-完成；其他-报错</returns>
        public static string CopyFolder(string sPath, string dPath)
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
