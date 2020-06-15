using System;
using System.Linq;
using System.IO;
using QRST_DI_Resources;
using QRST_DI_DS_Metadata.Paths;
 
namespace QRST_DI_TS_Basis.DirectlyAddress
{
    [Serializable]
    public class StorageBasePath
    {
        static public string QRST_DB_Common = "QRST_DB_Prototype";
        static public string QRST_DB_Store = "QRST_DB_Store";
        static public string QRST_DB_Share = "QRST_DB_Share";
        static public string QRST_DB_Basis = "QRST_DB_Basis";
        static public string QRST_DB_Tile = "QRST_DB_Tile";
        static public string OrignalData = "OrignalData";
        static public string CorrectedData = "CorrectedData";
        static public string ClassfiedData = "ClassfiedData";
        static public string TiledData = "TiledData";
        static public string FailedTile = "FailedTile";
        static public string Products = "Products";
        static public string Vector = "Vector";
        static public string Algorithm = "Algorithm";
        static public string UserSharingData = "UserSharingData";
        static public string UserSharingAlgorithm = "UserSharingData";

        /// <summary>
        /// string.Format(@"\\{0}\{1}\{2}\", Constant.ConsoleServerIP,StorageBasePath.QRST_DB_Tile, StorageBasePath.FailedTile);          
        /// </summary>
        static public string FailedTileTempPath
        {
            get
            {
                string path = "";
                switch (Constant.DbStorage)
                {
                    case EnumDbStorage.MULTIPLE:
                        path= string.Format(@"\\{0}\{1}\{2}\", Constant.ConsoleServerIP, StorageBasePath.QRST_DB_Tile,
                 StorageBasePath.FailedTile);
                        break;
                    case EnumDbStorage.SINGLE:
                        path = string.Format(@"{0}\{1}\{2}\", Constant.PcDBRootPath, StorageBasePath.QRST_DB_Tile,
                       StorageBasePath.FailedTile);
                        break;
                }
                return path;
            }
        }

        static public string QRST_DB_StorePath
        {
            get
            {
                return string.Format(@"{0}", Constant.DataStorePath);
            }
        }
        static public string SharePath_OrignalData(string orderWorkspace)
        {
            return string.Format(@"{0}{1}\", orderWorkspace, OrignalData);
        }
        static public string SharePath_CorrectedData(string orderWorkspace)
        {
            return string.Format(@"{0}{1}\", orderWorkspace, CorrectedData);
        }
        static public string SharePath_TiledData(string orderWorkspace)
        {
            return string.Format(@"{0}{1}\", orderWorkspace, TiledData);
        }
        static public string SharePath_Products(string orderWorkspace)
        {
            return string.Format(@"{0}{1}\", orderWorkspace, Products);
        }
        static public string HJDataSourceFilePath(string orderWorkspace,string srcFileName)
        {
            return string.Format(@"{0}{1}", orderWorkspace, srcFileName);
        }
        static public string StorePath_SourceProject(string sourceFilePath)
        {
             string sourceFileNameWithoutExt=(sourceFilePath.Length > 0)?
                Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(sourceFilePath)):"";
            
                //构建存储路径
                string[] namefeature = sourceFileNameWithoutExt.Split("-".ToArray());
                string satellite = "";
                string sensor = "";
                string year = "";
                string month = "";
                string day = "";

                if (namefeature.Length == 6)
                {
                    //HJ1B-CCD1-454-68-20100204-L20000246816.tar.gz
                    namefeature = sourceFileNameWithoutExt.Split("-".ToArray());
                    satellite = namefeature[0];
                    sensor = namefeature[1];
                    year = namefeature[4].Substring(0, 4);
                    month = namefeature[4].Substring(4, 2);
                    day = namefeature[4].Substring(6, 2);
                }
                else if (namefeature.Length == 7)
                {
                    //HJ1A-HSI-1-65-B2-20110816-L20000595562.tar.gz"
                    namefeature = sourceFileNameWithoutExt.Split("-".ToArray());
                    satellite = namefeature[0];
                    sensor = namefeature[1];
                    year = namefeature[5].Substring(0, 4);
                    month = namefeature[5].Substring(4, 2);
                    day = namefeature[5].Substring(6, 2);
                }

                string StorePath_Satellite = string.Format(@"{0}实验验证数据库\环境卫星数据\{1}\", StoragePath.StoreBasePath, satellite);
                string StorePath_Sensor = string.Format(@"{0}{1}\", StorePath_Satellite, sensor);
                string StorePath_Year = string.Format(@"{0}{1}\", StorePath_Sensor, year);
                string StorePath_Month = string.Format(@"{0}{1}\", StorePath_Year, month);
                string StorePath_Day = string.Format(@"{0}{1}\", StorePath_Month, day);
            
                //\\172.16.0.1\综合数据库\实验验证数据库\HJ\hj1a\ccd\2003\12\03\hj1a_hjjjjjcd\
                string StorePath_SourceProject = string.Format(@"{0}{1}\", StorePath_Day, sourceFileNameWithoutExt);
            return StorePath_SourceProject;
        }
        static public string StorePath_CorrectedData(string sourceFilePath)
        {
            string StorePath_CorrectedData = string.Format(@"{0}{1}\", StorePath_SourceProject(sourceFilePath), StorageBasePath.CorrectedData);
            return StorePath_CorrectedData;
        }

        /// <summary>
        /// 打包好的瓦片数据放置的目录，通过读取数据库midb中的appsettings表获得
        /// @zhangfeilong
        /// </summary>
        /// <returns></returns>
        static public string StorePath_ResultTileZipPath()
        {
            if (!Constant.Created)
                Constant.Create();
            return Constant.ResultTileZipPath;
        }
    }
}
