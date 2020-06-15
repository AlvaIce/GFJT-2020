/**描述：瓦片数据库操作接口。
 * 作者：jianghua
 * 日期：20170411
 */

using System.Collections.Generic;
using System.IO;
using DotSpatial.Data;
using DotSpatial.Topology;
using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetaData;
using DataSet = System.Data.DataSet;

namespace QRST_DI_SS_DBInterfaces.IDBService
{
    public interface ITCPService
    {

        /// <summary>
        /// 判断是否开机
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// 获取CPU占用率 
        /// </summary>
        double CpuLoad
        {
            get;
        }

        /// <summary>
        /// 获取CPU的产品名称信息
        /// </summary>
        /// <returns></returns>
        string GetCPUVresionInfor();

        /// <summary>
        /// 获取全部物理内存
        /// </summary>
        /// <returns></returns>
        long GetTotalPhysicalMemory();

        /// <summary>
        /// 获取可用物理内存
        /// </summary>
        /// <returns></returns>
        long GetAvailablePhysicalMemory();

        /// <summary>
        /// 获取可用磁盘空间
        /// </summary>
        /// <param name="driveName"></param>
        /// <returns></returns>
        double GetDivAvalableFreeSpace(string driveName);

        /// <summary>
        /// 获取磁盘总存储空间
        /// </summary>
        /// <param name="driveName"></param>
        /// <returns></returns>
        double GetDivTotalsize(string driveName);

        /// <summary>
        /// 获取已用磁盘空间
        /// </summary>
        /// <param name="driveName"></param>
        /// <returns></returns>
        double GetDivLoadSize(string driveName);

        /// <summary>
        /// 获取IP地址
        /// </summary>
        /// <returns></returns>
        string GetIPAddress();

        /// <summary>
        /// 获取主机名
        /// </summary>
        /// <returns></returns>
        string GetComputerName();

        #region 切片索引更新维护 Tile Index

        bool UpdateFailedTilesIndex(TileIndexUpdateType type, List<string> namelist);

        bool UpdateTileIndex(TileIndexUpdateType type, List<string> namelist);

        #endregion

        #region 远程文件管控Fetch tiles

        DirectoryInfo[] GetdirectoryArray(string currentDirectoryValue);

        FileInfo[] GetfileArray(string currentDirectoryValue);

        bool IsExistDir(string currentDirectory);
        #endregion

        #region 切片远程检索 tile Searching
        List<DataSet> GetDataSetCol(string sql, int pageIndex, out int recordNum, string tilepath, int pageSize);

        List<ModIDSearchInfo> GetResultInfo_SiteModRecordCount(string sql, out int recordNum, string tilepath);

        List<ModIDSearchInfo> GetResultInfo_SiteModRecordCount(string type, string sql, out int recordNum,
            string tilepath, List<Coordinate> coordinate);

        /// <summary>
        /// 查询每个配号下得结果信息，以汇总到网站上
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="recordNum"></param>
        /// <param name="tilepath"></param>
        /// <returns></returns>
        List<ModIDSearchInfo> GetResultInfo_SiteModRecordCount(string type, string sql, out int recordNum,
            string tilepath, string region, string category);

        IFeature GetFeatureByRegion(string region, string category);

        /// <summary>
        /// 查询得到每个配号下的DataSet，不分页返回全部结果。适用于结果不多的情况，如查询所有数据库中的  一共所包含的切片等级。
        /// </summary>
        List<DataSet> GetDataSetCol_CoordsFilter(string sql, string coordsStr, string tilepath);

        /// <summary>
        /// 暂时弃用 查询privew记录，删除不存在.png;.pgw;-1.tif;-2.tif;-3.tif;-4.tif文件的privew记录
        /// </summary>
        //DataSet DeletePngRecordWithGFFfileMissing(string tilepath);

        /// <summary>
        /// 将旧式瓦片名变更为新式瓦片名
        /// </summary>
        void UpdateTileName2NewStyle();

        /// <summary>
        /// 将jpg瓦片快视图变更为png格式，包括jgw变为pgw
        /// </summary>
        void ChangeTileJpg2Png();

        /// <summary>
        /// 查询并删除有记录但文件缺失的瓦片记录，遍历每个配号下的DataSet，不分页返回全部结果。
        /// 能将旧瓦片名重命名为新瓦片名
        /// </summary>
        /// <param name="
        /// "></param>
        /// <returns></returns>
        DataSet DeleteTileFileMissingRecord(string commonSharePath);

        /// <summary>
        /// 查询缺少满幅度云量的瓦片（需要1.2.3.4波段）,计算更新瓦片的满幅度云量信息，重命名，更新记录。
        /// 建议调用本方法前执行下DeleteTileFileMissingRecord方法，对缺失数据的记录进行清理
        /// </summary>
        /// <param name="commonSharePath"></param>
        /// <returns></returns>
        List<string> UpdateAvailabilityCloudInfoOfTiles(List<string> commonSharePath);

        List<DataSet> GetDataSetCol(string sql, string tilepath);

        void ExecuteNonQuery(string sql, string tilepath);

        List<DataSet> GetDataSetColPaged2(string sql, out int recordNum, List<ModIDSearchInfo> siteModsList);

        List<DataSet> GetDataSetColPaged2(out int recordNum, List<ModIDSearchInfo> siteModsList);
        #endregion

        #region 站点远程自动更新
        /// <summary>
        /// 获取指定文件夹下的所有文件夹
        /// </summary>
        void UpdateTSServer();
        #endregion

        /// <summary>
        /// 判断远程机是否运行
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        EnumSiteStatus GetServerStatus();

        byte[] GetTileTinyView(string tilename);

        byte[] GetTileMiniView(string tilename);

        int[][] GetTileImageData(string tilename);

        /// <summary>
        /// 暂时弃用 查询privew记录，删除不存在.png;.pgw;-1.tif;-2.tif;-3.tif;-4.tif文件的privew记录
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="tilepath"></param>
        /// <returns></returns>
        DataSet DeletePngRecordWithGFFfileMissing(string tilepath);

        void UpdateDistinctTables(string commonSharePath);

        void UpdateAllTileFileRecord(string commonSharePath);

        void UpdateDateTime8To10(string commonSharePath);

        DataSet UpdateAvailabilityCloudInfoOfTiles(string commonSharePath, bool updateGFF = false);
    }
}
