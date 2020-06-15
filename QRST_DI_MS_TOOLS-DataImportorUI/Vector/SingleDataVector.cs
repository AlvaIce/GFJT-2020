using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using QRST_DI_DS_Metadata.Paths;
using log4net;
using QRST_DI_DS_Metadata.MetaDataCls;
using QRST_DI_DS_Basis.DBEngine;
using QRST_DI_MS_TOOLS_DataImportorUI.Common;

namespace QRST_DI_MS_TOOLS_DataImportorUI.Vector
{
    public class SingleDataVector
    {

        public string _filepath;
        public MetaDataVector _metaData;
        public bool _hasReadMetaData;

        public SingleDataVector(string filepath)
        {
            _filepath = filepath;
            _metaData = new MetaDataVector();
            _hasReadMetaData = false;
        }

        public void ReadMetaData()
        {
            _metaData.ReadAttributes(_filepath);
            _hasReadMetaData = true;
        }
        /// <summary>
        /// 执行单文件数据入库
        /// </summary>
        public void DataImport(MySqlBaseUtilities bsdbUtil)
        {
            if (!_hasReadMetaData)
            {
                ReadMetaData();
            }
            if (_metaData.GroupCode==null||_metaData.GroupCode == "")
            {
                _metaData.GroupCode = MetaDataVector.GetDefaultGroupCode(bsdbUtil);
            }
            _metaData.ImportData(bsdbUtil);
            _metaData.GetModel(_metaData.QRST_CODE, bsdbUtil);
            string destPath = GetDestDirPath();
            ImportSHPFiles(destPath);
        }

        private string GetDestDirPath()
        {
            if (_metaData.IsCreated)
            {
                string tableCode = StoragePath.GetTableCodeByQrstCode(_metaData.QRST_CODE);
                StoragePath storePath = new StoragePath(tableCode);
                return storePath.GetDataOldPathForTools(_metaData);
            }
            else
            {
                return "";
            }
        }

        public override string ToString()
        {
            return _filepath;
        }
        private void ImportSHPFiles(string destDir)
        {
            if (Directory.Exists(destDir))
            {
                Directory.Delete(destDir, true);
            }
            Directory.CreateDirectory(destDir);

            string shpfilename=Path.GetFileNameWithoutExtension(_filepath);

            string[] files = Directory.GetFiles(Path.GetDirectoryName(_filepath));

            for (int i = 0; i < files.Length; i++)
            {
                if (Path.GetFileNameWithoutExtension(files[i]) == shpfilename
                    || (Path.GetExtension(files[i]) == ".xml" && Path.GetFileNameWithoutExtension(files[i]).ToLower() == shpfilename + ".shp"))
                {
                    CopyFile(files[i], string.Format(@"{0}\{1}", destDir, Path.GetFileName(files[i])));
                }
            }
        }

        static ILog log = LogManager.GetLogger(typeof(SingleDataVector));
        public static string FileFilter = "矢量数据|*.shp";
        public static string FileSearchPattern = "*.shp";

        public static void CopyFile(string srcPath, string destPath)
        {
            log.Info(string.Format("开始导入文件：{0}", srcPath));
            File.Copy(srcPath, destPath, true);
            log.Info(string.Format("完成导入文件：{0}", srcPath));
        }

    }
}
