using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Management;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;
using DotSpatial.Data;
using DotSpatial.Topology;
using QRST_DI_SS_DBInterfaces.IDBService;
using QRST_DI_SS_Basis;
using QRST_DI_TS_Basis.DBEngine;
using QRST_DI_TS_Basis.DirectlyAddress;
using QRST_DI_TS_Basis.Search;
using QRST_DI_TS_Process.Site;
using QRST_DI_TS_Process.Service;
using DataSet = System.Data.DataSet;
using System.Windows.Forms;
using QRST_DI_DS_DBEngine;
using QRST_DI_Resources;
using QRST_DI_SS_Basis.MetaData;
using QRST_DI_SS_DBInterfaces.IDBEngine;
using QRST_DI_SS_DBServer.DBEngine;
using System.Linq;

namespace QRST_DI_SS_DBServer.DBService
{
    public class TileSiteTCPServiceServer : MarshalByRefObject, ITCPService
    {
        private static TcpServerChannel _chan = null;
        ///  
        /// 构造函数，初始化计数器等 
        ///  
        public TileSiteTCPServiceServer()
        {
            InitializeLifetimeService();
            if (!Constant.ServiceIsConnected)
            {
                Constant.InitializeTcpConnection();
            }

            #region status parameters initialize
            //初始化CPU计数器 
            pcCpuLoad = new PerformanceCounter("Processor", "% Processor Time", "_Total") { MachineName = "." };
            pcCpuLoad.NextValue();

            //CPU个数 
            m_ProcessorCount = Environment.ProcessorCount;
            using (ManagementClass mc = new ManagementClass("Win32_ComputerSystem"))
            {
                using (ManagementObjectCollection moc = mc.GetInstances())
                {
                    foreach (ManagementObject mo in moc)
                    {
                        if (mo["TotalPhysicalMemory"] != null)
                        {
                            m_PhysicalMemory = long.Parse(mo["TotalPhysicalMemory"].ToString());
                        }
                    }
                }
            }
            #endregion

        }

        /// <summary>
        /// 开启TCP服务
        /// </summary>
        public static void StartTCPService(string TcpPort)
        {
            try
            {
                BinaryServerFormatterSinkProvider serverProvider = new BinaryServerFormatterSinkProvider();
                serverProvider.TypeFilterLevel = TypeFilterLevel.Full;
                IDictionary props = new Hashtable();
                //props["port"] = TcpPort;
                props["name"] = "SSTCP";
                props["typeFilterLevel"] = TypeFilterLevel.Full;
                _chan = new TcpServerChannel(
                props, serverProvider);
                ChannelServices.RegisterChannel(_chan);

                RemotingConfiguration.RegisterWellKnownServiceType(typeof(TileSiteTCPServiceServer),
                                "QDB_Storage_TCP",
                                WellKnownObjectMode.Singleton);
            }
            catch(Exception e)
            {
                throw new Exception("注册TCPServiceServer异常",e);
            }
        }

        /// <summary>
        /// 重写远程对象生存周期。默认远程对象一段时间后删除，重写后永久保存。
        /// </summary>
        /// <returns></returns>
        public override object InitializeLifetimeService()
        {
            return null;
        }

        #region 状态监控status

        #region WinAPI
        /// <summary>
        /// 获取计算机状态的
        /// </summary>
        public struct MEMORY_INFO
        {
            public uint dwLength;
            public uint dwMemoryLoad;
            public uint dwTotalPhys;
            public uint dwAvailPhys;
            public uint dwTotalVirtual;
            public uint dwAvailVirtual;
        }
        public string divName;
        public double divTotalFreeSpace, divTotalSize, DivAvalableFreeSpace;
        public class getmemory
        {
            [DllImport("kernel32")]
            public static extern void GlobalMemoryStatus(ref MEMORY_INFO meminfo);
        }
        private readonly int m_ProcessorCount = 0;   //CPU个数 
        private readonly PerformanceCounter pcCpuLoad;   //CPU计数器 
        private readonly long m_PhysicalMemory = 0;
        private const int GW_HWNDFIRST = 0;
        private const int GW_HWNDNEXT = 2;
        private const int GWL_STYLE = (-16);
        private const int WS_VISIBLE = 268435456;
        private const int WS_BORDER = 8388608;
        #endregion

        #region 是否开机运行
        /// <summary>
        /// 判断是否开机
        /// </summary>
        /// <returns></returns>
        public bool IsRunning
        {
            get { return true; }
        }
        #endregion

        #region CPU信息
        /// 获取CPU占用率 
        public double CpuLoad
        {
            get
            {
                return pcCpuLoad.NextValue();
            }
        }
        /// <summary>
        /// 获取CPU的产品名称信息
        /// </summary>
        /// <returns></returns>
        public string GetCPUVresionInfor()
        {
            string CPUVresion = "";
            using (ManagementObjectSearcher driveID = new ManagementObjectSearcher("SELECT * FROM Win32_Processor"))
            {
                foreach (ManagementObject mo in driveID.Get())
                {
                    CPUVresion = mo["Name"].ToString();
                }
            }
            return CPUVresion;
        }
        #endregion

        #region 物理内存
        /// <summary>
        /// 获取全部物理内存
        /// </summary>
        /// <returns></returns>
        public long GetTotalPhysicalMemory()
        {
            try
            {

                long st = 0;
                ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    st = long.Parse(mo["TotalPhysicalMemory"].ToString()) / 1024 / 1024;
                }
                moc = null;
                mc = null;
                return st;
            }
            catch
            {
                return 0;
            }
            finally
            {
            }
        }
        /// <summary>
        /// 获取可用物理内存
        /// </summary>
        /// <returns></returns>
        public long GetAvailablePhysicalMemory()
        {
            long availablebytes = 0;
            using (ManagementClass mos = new ManagementClass("Win32_OperatingSystem"))
            {
                foreach (ManagementObject mo in mos.GetInstances())
                {
                    if (mo["FreePhysicalMemory"] != null)
                    {
                        availablebytes = 1024 * long.Parse(mo["FreePhysicalMemory"].ToString());
                    }
                }
            }
            availablebytes = availablebytes / 1024 / 1024;
            return availablebytes;
        }
        #endregion

        #region 磁盘空间
        /// <summary>
        /// 获取可用磁盘空间
        /// </summary>
        /// <param name="driveName"></param>
        /// <returns></returns>
        public double GetDivAvalableFreeSpace(string driveName)
        {
            double divavalableFreeSpace = 0;
            driveName = driveName.ToUpper().Replace("/", "\\").Trim();
            if (driveName.Length <= 3)
            {
                if (driveName.Length == 2)
                {
                    driveName += "\\";
                }
                else if (driveName.Length == 1)
                {
                    driveName += ":\\";
                }
                DriveInfo[] allDrives = DriveInfo.GetDrives();
                foreach (DriveInfo d in allDrives)
                {
                    divName = d.Name;
                    if (divName == driveName)
                    {

                        if (d.IsReady == true)
                            DivAvalableFreeSpace = d.AvailableFreeSpace / 1024 / 1024;
                        divavalableFreeSpace = DivAvalableFreeSpace;
                        break;
                    }
                }
            }

            return divavalableFreeSpace;
        }
        /// <summary>
        /// 获取磁盘总存储空间
        /// </summary>
        /// <param name="driveName"></param>
        /// <returns></returns>
        public double GetDivTotalsize(string driveName)
        {
            double divtotalsize = 0;
            driveName = driveName.ToUpper().Replace("/", "\\").Trim();
            if (driveName.Length <= 3)
            {
                if (driveName.Length == 2)
                {
                    driveName += "\\";
                }
                else if (driveName.Length == 1)
                {
                    driveName += ":\\";
                }

                DriveInfo[] allDrives = DriveInfo.GetDrives();
                foreach (DriveInfo d in allDrives)
                {

                    divName = d.Name;
                    if (divName == driveName)
                    {

                        if (d.IsReady == true)
                        {
                            divTotalSize = d.TotalSize / 1024 / 1024;
                        }
                        divtotalsize = divTotalSize;
                        break;
                    }
                }
            }

            return divtotalsize;
        }
        /// <summary>
        /// 获取已用磁盘空间
        /// </summary>
        /// <param name="driveName"></param>
        /// <returns></returns>
        public double GetDivLoadSize(string driveName)
        {
            double divLoadSize = GetDivTotalsize(driveName) - GetDivAvalableFreeSpace(driveName);
            return divLoadSize;
        }
        #endregion

        #region IP地址
        /// <summary>
        /// 获取IP地址
        /// </summary>
        /// <returns></returns>
        public string GetIPAddress()
        {
            try
            {
                string st = "";
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        //st=mo["IpAddress"].ToString(); 
                        System.Array ar;
                        ar = (System.Array)(mo.Properties["IpAddress"].Value);
                        st = ar.GetValue(0).ToString();
                        break;
                    }
                }
                moc = null;
                mc = null;
                return st;
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }

        }
        #endregion

        #region 主机名
        /// <summary>
        /// 获取主机名
        /// </summary>
        /// <returns></returns>
        public string GetComputerName()
        {
            try
            {
                return System.Environment.GetEnvironmentVariable("ComputerName");
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }
        }
        #endregion

        #endregion

        #region 远程应用application

        #region 切片索引更新维护 Tile Index
        public bool UpdateFailedTilesIndex(TileIndexUpdateType type, List<string> namelist)
        {
            try
            {
                string dbfile_failedTiles = null;
                switch (Constant.DbStorage)
                {
                    case EnumDbStorage.MULTIPLE:
                        dbfile_failedTiles = string.Format("{0}QDB_IDX_Failed.db",
string.Format(@"\\{0}\{1}\{2}\", TServerSiteManager.GetCenterSiteIP(), StorageBasePath.QRST_DB_Tile,
    StorageBasePath.FailedTile));
                        break;
                    case EnumDbStorage.SINGLE:
                        dbfile_failedTiles = string.Format("{0}QDB_IDX_Failed.db",
           string.Format(@"{0}\{1}\{2}\", Constant.PcDBRootPath, StorageBasePath.QRST_DB_Tile,
               StorageBasePath.FailedTile));
                        break;
                  
                }
                TileIndexUpdateUtilities tiuu = new TileIndexUpdateUtilities();
                tiuu.TileIndexUpdate(type, namelist, dbfile_failedTiles);
            }
            catch (Exception)
            {

                return false;
            }

            return true;

        }

        public bool UpdateTileIndex(TileIndexUpdateType type, List<string> namelist)
        {
            try
            {
                TileIndexUpdateUtilities tiuu = new TileIndexUpdateUtilities();
                tiuu.TileIndexUpdate(type, namelist);

                //利用多线程，将切片同步到相同的VDS中 2013/2/27
                //包括切片同步以及索引的建立
                //第一步：更新活动的站点列表
                //第二步：在活动站点
            }
            catch (Exception ex)
            {
                throw ex;
                return false;
            }

            return true;
        }
        #endregion

        #region 远程文件管控Fetch tiles

        /// 获取指定文件夹下的所有文件夹
        /// </summary>
        /// <returns></returns>
        public DirectoryInfo[] GetdirectoryArray(string currentDirectoryValue)
        {

            string currentDirectory = currentDirectoryValue;
            DirectoryInfo newcurrentDirectory = new DirectoryInfo(currentDirectory);
            DirectoryInfo[] direcytoryArray = newcurrentDirectory.GetDirectories();
            return direcytoryArray;

        }
        public FileInfo[] GetfileArray(string currentDirectoryValue)
        {

            string currentDirectory = currentDirectoryValue;
            DirectoryInfo newcurrentDirectory = new DirectoryInfo(currentDirectory);
            FileInfo[] fileArray = newcurrentDirectory.GetFiles();
            return fileArray;
        }
        public bool IsExistDir(string currentDirectory)
        {
            bool s = false;
            if (Directory.Exists(currentDirectory))
                s = true;
            return s;

        }

        #endregion

        #region 切片远程检索 tile Searching
        /// <summary>
        /// 查询得到每个配号下的DataSet，返回列表
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pageIndex"></param>
        /// <param name="recordNum"></param>
        /// <param name="tilepath"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<DataSet> GetDataSetCol(string sql, int pageIndex, out int recordNum, string tilepath, int pageSize)
        {
            TileSearchUtil tileSearchUtil = new TileSearchUtil();
            return tileSearchUtil.GetDataSetCol(sql, pageIndex, out recordNum, tilepath, pageSize);
        }
        /// <summary>
        /// 查询每个配号下得结果信息，以汇总到网站上
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="recordNum"></param>
        /// <param name="tilepath"></param>
        /// <returns></returns>
        public List<ModIDSearchInfo> GetResultInfo_SiteModRecordCount(string sql, out int recordNum, string tilepath)
        {
            TileSearchUtil tileSearchUtil = new TileSearchUtil();
            return tileSearchUtil.GetResultInfo_SiteModRecordCount(sql, out recordNum, tilepath);
        }
        /// <summary>
        /// 查询每个配号下得结果信息，以汇总到网站上
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="recordNum"></param>
        /// <param name="tilepath"></param>
        /// <returns></returns>
        public List<ModIDSearchInfo> GetResultInfo_SiteModRecordCount(string type, string sql, out int recordNum, string tilepath, List<Coordinate> coordinate)
        {
            DotSpatial.Data.IFeature iFeature = null;
             DotSpatial.Data.Shape shp = new DotSpatial.Data.Shape(FeatureType.Polygon);
            shp.AddPart(coordinate, DotSpatial.Data.CoordinateType.Regular);
            iFeature = new DotSpatial.Data.Feature(shp);
            TileSearchUtil tileSearchUtil = new TileSearchUtil();
            return tileSearchUtil.GetResultInfo_SiteModRecordCount(type, sql, out recordNum, tilepath, iFeature);
        }
        /// <summary>
        /// 查询每个配号下得结果信息，以汇总到网站上
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="recordNum"></param>
        /// <param name="tilepath"></param>
        /// <returns></returns>
        public List<ModIDSearchInfo> GetResultInfo_SiteModRecordCount(string type, string sql, out int recordNum, string tilepath, string region, string category)
        {

            DotSpatial.Data.IFeature iFeature = GetFeatureByRegion(region, category);
            TileSearchUtil tileSearchUtil = new TileSearchUtil();
            return tileSearchUtil.GetResultInfo_SiteModRecordCount(type, sql, out recordNum, tilepath, iFeature);
        }

        public IFeature GetFeatureByRegion(string region,string category)
        {
            DotSpatial.Data.IFeatureSet ProVftset, Cityftset, Countyftset;
            string shpPath =Environment.CurrentDirectory+(@"\map\provincialBoundary.shp");
            ProVftset = DotSpatial.Data.Shapefile.Open(shpPath);
            string cityShpPath = Environment.CurrentDirectory + (@"\map\cityBoundary.shp");
            Cityftset = DotSpatial.Data.Shapefile.Open(cityShpPath);
            string countryShpPath = Environment.CurrentDirectory + (@"\map\countyBoundary.shp");
            Countyftset = DotSpatial.Data.Shapefile.Open(countryShpPath);
            IFeature iFeature = null;
            if (category == "省")
            {
                foreach (DotSpatial.Data.Feature f in ProVftset.Features)
                {
                    if (f.DataRow["Name"].ToString() == region)
                    {
                        iFeature = f;
                        break;
                    }
                }
            }
            else if (category == "市")
            {
                foreach (DotSpatial.Data.Feature f in Cityftset.Features)
                {
                    if (f.DataRow["Name"].ToString() == region)
                    {
                        iFeature = f;
                        break;
                    }
                }
            }
            else
            {
                foreach (DotSpatial.Data.Feature f in Countyftset.Features)
                {
                    if (f.DataRow["Name"].ToString() == region)
                    {
                        iFeature = f;
                        break;
                    }
                }
            }
            return iFeature;
        }

        /// <summary>
        /// 查询得到每个配号下的DataSet，不分页返回全部结果。适用于结果不多的情况，如查询所有数据库中的  一共所包含的切片等级。
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pageIndex"></param>
        /// <param name="recordNum"></param>
        /// <param name="tilepath"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<DataSet> GetDataSetCol_CoordsFilter(string sql, string coordsStr, string tilepath)
        {
            TileSearchUtil tileSearchUtil = new TileSearchUtil();
            return tileSearchUtil.GetDataSetCol_CoordsFilter(sql, coordsStr, tilepath);
        }

        /// <summary>
        /// 暂时弃用 查询privew记录，删除不存在.png;.pgw;-1.tif;-2.tif;-3.tif;-4.tif文件的privew记录
        /// </summary>
        /// <param name = "sql" ></ param >
        /// < param name="tilepath"></param>
        /// <returns></returns>
        public DataSet DeletePngRecordWithGFFfileMissing(string tilepath)
        {
            string sql = "Select * from correctedTiles where type='Preview'";

            TileSearchUtil tileSearchUtil = new TileSearchUtil();
            List<DataSet> dss = tileSearchUtil.GetDataSetCol(sql, tilepath);

            DataSet DS = new DataSet();
            if (dss != null && dss.Count != 0)
            {
                foreach (DataSet ds in dss)
                {
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        if (DS.Tables.Count == 0)
                        {
                            DS.Tables.Add(ds.Tables[0].Copy());
                        }
                        else
                        {
                            DS.Merge(ds, false, MissingSchemaAction.Add);
                        }
                    }

                }
            }
            if (DS != null && DS.Tables.Count > 0)
            {

                SQLBaseTool sqlbt = new SQLBaseTool();
                DirectlyAddressing dautil = new DirectlyAddressing(DirectlyAddressingIPMod.IPModDataSet);

                sqlbt.AddData_TileNameAddress(DS);
                DataTable table = DS.Tables[0];
                for (int i = table.Rows.Count - 1; i > -1; i--)
                {
                    string destPath = dautil.GetPathByFileName(table.Rows[i]["TileFileName"].ToString());
                    table.Rows[i]["TileFileName"] = destPath;
                    if (File.Exists(destPath) && File.Exists(destPath.Replace(".png", ".pgw")) && File.Exists(destPath.Replace(".png", "-1.tif")) && File.Exists(destPath.Replace(".png", "-2.tif")) && File.Exists(destPath.Replace(".png", "-3.tif")) && File.Exists(destPath.Replace(".png", "-4.tif")))
                    {
                        table.Rows.RemoveAt(i);
                        continue;
                    }
                }
                DS.AcceptChanges();

                List<Task> tasks = new List<Task>();
                Dictionary<string, List<string>> dbsql = new Dictionary<string, List<string>>();
                for (int i = table.Rows.Count - 1; i > -1; i--)
                {

                    sql = string.Format("Delete from correctedTiles where DataSourceID='{0}' and Satellite='{1}' and Sensor='{2}' and Date='{3}' and Level = '{4}' and Row = '{5}' and Col ='{6}' and type='{7}'", table.Rows[i]["DataSourceID"], table.Rows[i]["Satellite"], table.Rows[i]["Sensor"], table.Rows[i]["Date"], table.Rows[i]["Level"], table.Rows[i]["Row"], table.Rows[i]["Col"], table.Rows[i]["type"]);
                    string curtilepath = string.Format(@"{0}QRST_DB_Tile\{1}\QDB_IDX_{1}.db", tilepath, dautil.GetStorageIPMod(Convert.ToInt16(table.Rows[i]["Row"]), Convert.ToInt16(table.Rows[i]["Col"])));

                    if (dbsql.ContainsKey(curtilepath))
                    {
                        dbsql[curtilepath].Add(sql);
                    }
                    else
                    {
                        dbsql.Add(curtilepath, new List<string>());
                        dbsql[curtilepath].Add(sql);
                    }

                }
                foreach (KeyValuePair<string, List<string>> kvp in dbsql)
                {
                    object[] objs = new object[] { kvp.Key, kvp.Value };
                    Task t = Task.Factory.StartNew(o =>
                    {
                        object[] objkvp = o as object[];
                        string filepath = objkvp[0] as string;
                        List<string> sqls = objkvp[1] as List<string>;
                        foreach (string s in sqls)
                        {
                            ExecuteNonQuery(s, filepath);
                        }
                    }, objs);
                    tasks.Add(t);
                }

                foreach (Task task in tasks)
                {
                    task.Wait();
                }
            }

            return DS;
        }


        /// <summary>
        /// 将旧式瓦片名变更为新式瓦片名
        /// </summary>
        public void UpdateTileName2NewStyle()
        {
            string tilePath = null;
            switch (Constant.DbStorage)
            {
                case EnumDbStorage.MULTIPLE:
                    tilePath = string.Format(@"\\localhost\{0}\", QRST_DI_Resources.StaticStrings.QRST_DB_Tile);
                    break;
                case EnumDbStorage.SINGLE:
                    tilePath = string.Format(@"{0}\{1}\", Constant.PcDBRootPath, QRST_DI_Resources.StaticStrings.QRST_DB_Tile);
                    break;
                case EnumDbStorage.CLUSTER:
                    break;
            }
            List<string> ctnnl = new List<string>();
            ChangeTileName2NewStyleObj ctn2nsobj = new ChangeTileName2NewStyleObj(tilePath, ctnnl);
            ChangeTileName2NewStyle(ctn2nsobj);

            //时间变了需要更新sqlite索引
            TileIndexUpdateUtilities tiuu = new TileIndexUpdateUtilities();
            tiuu.TileIndexUpdate(TileIndexUpdateType.InsertUpdate, ctnnl);
        }

        class ChangeTileName2NewStyleObj 
        {
            public ChangeTileName2NewStyleObj(string dir,List<string> ctnnl)
            {
                Dir = dir;
                if (ctnnl == null)
                {
                    ctnnl = new List<string>();
                }
                ChangedTileNewNameList = ctnnl;
            }

            public string Dir { get; set; }
            public List<string> ChangedTileNewNameList;
        }

        private void ChangeTileName2NewStyle(object changeTileName2NewStyleObj)
        {
            ChangeTileName2NewStyleObj ctn2nsobj = changeTileName2NewStyleObj as ChangeTileName2NewStyleObj;
            string rootdir = ctn2nsobj.Dir;

            string[] childDirs = Directory.GetDirectories(rootdir);
            List<Task> tasks = new List<Task>();
            foreach (string dir in childDirs)
            {
                ChangeTileName2NewStyleObj ctn2nsobj_child = new ChangeTileName2NewStyleObj(dir, ctn2nsobj.ChangedTileNewNameList);
                tasks.Add(Task.Factory.StartNew(ChangeTileName2NewStyle, ctn2nsobj_child));
                //alltasks++;
            }
            string[] files = Directory.GetFiles(rootdir, "*.*");
            foreach (string file in files)
            {
                TileNameArgs tna = TileNameArgs.GetTileNameArgs(Path.GetFileName(file));
                if (tna.Created && tna.IsOldNameStyle)
                {
                    //是瓦片,而且是旧式命名
                    string newfile = file.Replace(Path.GetFileName(file), tna.GetNewStyleFilename());
                    //已存在，判断是否一致，如果不一致，删除新文件，再重命名，如果一致，删除旧文件
                    if (File.Exists(newfile))
                    {
                        FileInfo newfi = new FileInfo(newfile);
                        FileInfo oldfi = new FileInfo(file);
                        if (newfi.Length != oldfi.Length)
                        {
                            try
                            {
                                File.Delete(newfile);
                                System.Threading.Thread.Sleep(100);
                                File.Move(file, newfile);
                            }
                            catch (Exception ex) { }
                        }
                        else
                        {
                            try
                            {
                                File.Delete(file);
                            }
                            catch (Exception ex) { }
                        }
                    }
                    else
                    {
                        File.Move(file, newfile);
                    }

                    lock (ctn2nsobj.ChangedTileNewNameList)
                    {
                        ctn2nsobj.ChangedTileNewNameList.Add(Path.GetFileName(newfile));
                    }
                }
            }

            Task.WaitAll(tasks.ToArray());
            //donetasks += tasks.Count + 1;
            //Console.WriteLine(string.Format("All Tasks:{0} DoneTasks:{1}",alltasks,donetasks));
        }
        /// <summary>
        /// 将jpg瓦片快视图变更为png格式，包括jgw变为pgw
        /// </summary>
        public void ChangeTileJpg2Png()
        {
            string tilePath = null;
            switch (Constant.DbStorage)
            {
                case EnumDbStorage.CLUSTER:
                    break;
                case EnumDbStorage.MULTIPLE:
                    tilePath = string.Format(@"\\localhost\{0}\", QRST_DI_Resources.StaticStrings.QRST_DB_Tile);
                    break;
                case EnumDbStorage.SINGLE:
                    tilePath = string.Format(@"{0}\{1}\", Constant.PcDBRootPath, QRST_DI_Resources.StaticStrings.QRST_DB_Tile);
                    break;
            }
            FilesRename_JPG2PNG(tilePath);
        }

        private void FilesRename_JPG2PNG(object objrootdir)
        {
            string rootdir = objrootdir as string;

            string[] childDirs = Directory.GetDirectories(rootdir);
            List<Task> tasks = new List<Task>();
            foreach (string dir in childDirs)
            {
                tasks.Add(Task.Factory.StartNew(FilesRename_JPG2PNG, dir));
                //alltasks++;
            }
            string[] jpgs = Directory.GetFiles(rootdir, "*.jpg");
            foreach (string jpgf in jpgs)
            {
                File.Move(jpgf, jpgf.Replace(".jpg", ".png"));
            }
            string[] jgws = Directory.GetFiles(rootdir, "*.jgw");
            foreach (string jgwf in jgws)
            {
                File.Move(jgwf, jgwf.Replace(".jgw", ".pgw"));
            }

            Task.WaitAll(tasks.ToArray());
            //donetasks += tasks.Count + 1;
            //Console.WriteLine(string.Format("All Tasks:{0} DoneTasks:{1}",alltasks,donetasks));
        }

        /// <summary>
        /// 查询并删除有记录但文件缺失的瓦片记录，遍历每个配号下的DataSet，不分页返回全部结果。
        /// 能将旧瓦片名重命名为新瓦片名
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="CommonSharePath"></param>
        /// <returns></returns>
        public DataSet DeleteTileFileMissingRecord(string commonSharePath)
        {
            List<string> missFileTiles = new List<string>();
            DataSet DS = new DataSet();

            TileSearchUtil tileSearchUtil = new TileSearchUtil();
            string sql = "Select * from correctedTiles";
            List<DataSet> dss = tileSearchUtil.GetDataSetCol(sql, commonSharePath);
            sql = "Select * from productTiles";
            dss.AddRange(tileSearchUtil.GetDataSetCol(sql, commonSharePath));
            sql = "Select * from classifySampleTiles";
            dss.AddRange(tileSearchUtil.GetDataSetCol(sql, commonSharePath));

            if (dss != null && dss.Count != 0)
            {
                List<Task> tasks = new List<Task>();
                foreach (DataSet ds in dss)
                {
                    tasks.Add(Task.Factory.StartNew(o =>
                    {
                        DataSet in_ds = (o as object[])[0] as DataSet;
                        List<string> in_missFileTiles = (o as object[])[1] as List<string>;

                        if (in_ds != null && in_ds.Tables.Count > 0)
                        {

                            SQLBaseTool sqlbt = new SQLBaseTool();
                            DirectlyAddressing dautil = new DirectlyAddressing(DirectlyAddressingIPMod.IPModDataSet);

                            sqlbt.AddData_TileNameAddress(in_ds);
                            DataTable table = in_ds.Tables[0];
                            for (int i = table.Rows.Count - 1; i > -1; i--)
                            {
                                string destPath = dautil.GetPathByFileName(table.Rows[i]["TileFileName"].ToString());
                                if (File.Exists(destPath))
                                {
                                    //文件存在
                                    table.Rows.RemoveAt(i);
                                    continue;
                                }
                                else
                                {
                                    string oldstylename = TileNameArgs.GetTileNameArgs(table.Rows[i]["TileFileName"].ToString()).GetOldStyleFilename();
                                    string oldstyledestPath = destPath.Replace(table.Rows[i]["TileFileName"].ToString(), oldstylename);
                                    if (File.Exists(oldstyledestPath))
                                    {
                                        //文件存在,但是是旧式命名方法
                                        try
                                        {
                                            //重命名，将旧式命为新式
                                            File.Move(oldstyledestPath, destPath);
                                        }
                                        catch { }
                                        table.Rows.RemoveAt(i);
                                        continue;
                                    }
                                    else
                                    {
                                        //文件丢失
                                        lock (in_missFileTiles)     //调试missFileTiles是否跨线程共用
                                        {
                                            in_missFileTiles.Add(table.Rows[i]["TileFileName"].ToString());
                                        }
                                    }
                                }
                            }
                            in_ds.AcceptChanges();
                        }

                    }, new object[] { ds, missFileTiles }));

                }
                foreach (Task task in tasks)
                {
                    task.Wait(-1);
                }

                foreach (DataSet ds in dss)
                {
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        if (DS.Tables.Count == 0)
                        {
                            DS.Tables.Add(ds.Tables[0].Copy());
                        }
                        else
                        {
                            //DataRow[] drs=new DataRow[ds.Tables[0].Rows.Count];
                            //ds.Tables[0].Rows.CopyTo(drs, 0);
                            //DS.Tables[0].LoadDataRow(drs,  LoadOption.OverwriteChanges);

                            DS.Merge(ds, false, MissingSchemaAction.Add);
                        }
                    }
                }
            }

            try
            {
                QRST_DI_Resources.MyConsole.WriteLine("丢失文件瓦片记录数：" + missFileTiles.Count.ToString());
            }
            catch
            {
                QRST_DI_Resources.MyConsole.WriteLine("丢失文件瓦片记录数：0");
            }

            TileIndexUpdateUtilities tiuu = new TileIndexUpdateUtilities();
            tiuu.TileIndexUpdate(TileIndexUpdateType.Delete, missFileTiles);

            return DS;
        }


        /// <summary>
        /// 将传入的新瓦片名称列表（查找*_FFFF_*.png 存在4波段的未计算过云量满幅度的瓦片快视图），计算满幅度和云量、重命名，并更新索引
        /// </summary>
        /// <param name="tilenames">已入库的新瓦片名称列表</param>
        /// <returns>更新过后的瓦片名称列表，行4波段和快视图，可用于更新索引</returns>
        public List<string> UpdateAvailabilityCloudInfoOfTiles(List<string> tilenames)
        {
            List<string> updatedFileTiles = new List<string>();
            DirectlyAddressing dautil = new DirectlyAddressing(DirectlyAddressingIPMod.IPModDataSet);

            for (int i = tilenames.Count - 1; i > -1; i--)
            {
                //查找*_FFFF_*.png 存在4波段的未计算过云量满幅度的瓦片快视图
                if (!tilenames[i].ToLower().EndsWith(".png") || tilenames[i].ToUpper().IndexOf("_FFFF_") == -1)
                {
                    tilenames.RemoveAt(i);
                    continue;
                }
            }

            QRST_DI_Resources.MyConsole.WriteLine("待更新云量满幅度瓦片记录数：" + tilenames.Count.ToString());
            for (int i = tilenames.Count - 1; i > -1; i--)
            {
                QRST_DI_Resources.MyConsole.WriteLine(string.Format("正在计算云量满幅度瓦片：{0}({1}/{2})", tilenames[i], tilenames.Count - i, tilenames.Count));


                //需要1.2.3.4波段）,
                bool exist4bands = true;
                string filename = tilenames[i];
                string filepath = dautil.GetPathByFileName(filename);
                string destDir = Path.GetDirectoryName(filepath);
                CorrectedTileNameArgs tna = TileNameArgs.GetTileNameArgs(filename) as CorrectedTileNameArgs;

                if (!File.Exists(filepath) || tna == null || !tna.Created)
                {
                    QRST_DI_Resources.MyConsole.WriteLine(string.Format("目标文件不存在,且瓦片名称不可识别，跳过！ {0}", filepath));

                    //如果名称有误，则跳过，开始计算下一行记录
                    continue;
                }

                string tileWithoutExt = Path.GetFileNameWithoutExtension(filename);
                string[] tilefiles = new string[4];

                string tileWithoutSubtype = Path.GetFileNameWithoutExtension(tileWithoutExt);       //去掉.c
                tilefiles[0] = string.Format(@"{0}\{1}-1.c.tif", destDir, tileWithoutSubtype);
                tilefiles[1] = string.Format(@"{0}\{1}-2.c.tif", destDir, tileWithoutSubtype);
                tilefiles[2] = string.Format(@"{0}\{1}-3.c.tif", destDir, tileWithoutSubtype);
                tilefiles[3] = string.Format(@"{0}\{1}-4.c.tif", destDir, tileWithoutSubtype);
                string tileinfo = string.Format(@"{0}\{1}.info", destDir, tileWithoutExt);

                //检查源四波段数据存在性
                foreach (string tilepath in tilefiles)
                {
                    if (!File.Exists(tilepath))
                    {
                        QRST_DI_Resources.MyConsole.WriteLine(string.Format("{0}找不到文件，无法计算，跳过", tilepath));
                        exist4bands = false;
                        break;

                    }
                }

                if (!exist4bands)
                {
                    //如果四波段数据不完整，则跳过，开始计算下一行记录
                    continue;
                }

                try
                {
                    QRST_DI_Resources.MyConsole.WriteLine(string.Format("启动云量满幅度计算进程：{0}({1}/{2})", tilenames[i], tilenames.Count - i, tilenames.Count));
                    //计算更新瓦片的满幅度云量信息
                    ProcessStartInfo psi = new ProcessStartInfo(Path.GetDirectoryName(Application.ExecutablePath) + @"\plugin\CloudMask\CloudMask.exe");
                    psi.UseShellExecute = false;
                    psi.CreateNoWindow = true;
                    psi.Arguments = string.Format("2 \"{0}\" \"{1}\" \"{2}\" \"{3}\" \"{4}\" 64 128", tilefiles[0], tilefiles[1], tilefiles[2], tilefiles[3], tileinfo);

                    using (Process p = Process.Start(psi))
                    {
                        p.WaitForExit();
                        int iii = 0;
                        while (!File.Exists(tileinfo))
                        {
                            if (iii > 15)
                            {

                                QRST_DI_Resources.MyConsole.WriteLine(string.Format("等待云量满幅度计算进程退出超时：{0}({1}/{2})", tilenames[i], tilenames.Count - i, tilenames.Count));
                                throw new ApplicationException(string.Format("CloudMask.exe of file '{0}' failed.", tilefiles[0]));
                            }
                            System.Threading.Thread.Sleep(200);
                            iii++;
                        }


                    }

                    QRST_DI_Resources.MyConsole.WriteLine(string.Format("完成云量满幅度计算：{0}({1}/{2})", tilenames[i], tilenames.Count - i, tilenames.Count));
                    string[] info = File.ReadAllLines(tileinfo);
                    string[] info_x2 = new string[2];
                    info_x2[0] = Convert.ToInt16(info[0].Trim()).ToString("X2");       //云量
                    info_x2[1] = Convert.ToInt16(info[1].Trim()).ToString("X2");       //满幅度
                    //info[0] = (info[0].Length == 1) ? "0" + info[0] : info[0];
                    //info[1] = (info[1].Length == 1) ? "0" + info[1] : info[1];


                    //重命名文件
                    // tileWithoutExt=HJ1A_CCD1_20161212_L20000521090_4_241_599
                    //newTileWithoutExt=HJ1A_CCD1_20161212_L20000521090#6400_4_241_599
                    tna.Availability = Convert.ToInt16(info[1].Trim());
                    tna.Cloud = Convert.ToInt16(info[0].Trim());
                    string newTileWithoutExt = tna.GetNewStyleFilename();
                    newTileWithoutExt = newTileWithoutExt.Substring(0, newTileWithoutExt.Length - ".c.png".Length);

                    FileInfo[] files = new DirectoryInfo(destDir).GetFiles();
                    foreach (FileInfo fi in files)
                    {
                        if (fi.Extension == ".info")
                        {
                            try
                            {
                                File.Delete(fi.FullName);
                            }
                            catch (Exception ex)
                            {

                                QRST_DI_Resources.MyConsole.WriteLine(string.Format("删除info文件异常，继续：{0}({1}/{2}) {3}，{4} ", tilenames[i], tilenames.Count - i, tilenames.Count, fi.FullName, ex.Message));
                            }
                            continue;
                        }

                        if (fi.Name.StartsWith(tileWithoutSubtype))
                        {
                            string newpath = fi.FullName.Replace(tileWithoutSubtype, newTileWithoutExt);
                            if (!fi.Name.StartsWith(newTileWithoutExt))
                            {
                                try
                                {

                                    fi.MoveTo(newpath);
                                }
                                catch (Exception ex)
                                {
                                    QRST_DI_Resources.MyConsole.WriteLine(string.Format("文件重命名异常，继续：{0}({1}/{2}) {3} {4} ", tilenames[i], tilenames.Count - i, tilenames.Count, newpath, ex.Message));
                                }
                            }

                            if (fi.Name.ToUpper().EndsWith(".TIF") || fi.Name.ToUpper().EndsWith(".JPG") || fi.Name.ToUpper().EndsWith(".PNG"))
                            {
                                updatedFileTiles.Add(Path.GetFileName(newpath));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    QRST_DI_Resources.MyConsole.WriteLine(string.Format("计算云量满幅度异常，继续：{0}({1}/{2}) {3} ", tilenames[i], tilenames.Count - i, tilenames.Count, ex.Message));
                }

            }


            try
            {
                if (updatedFileTiles.Count > 0)
                {
                    TileIndexUpdateUtilities tiuu = new TileIndexUpdateUtilities();
                    tiuu.TileIndexUpdate(TileIndexUpdateType.InsertUpdate, updatedFileTiles);
                }
            }
            catch (Exception ex)
            {
                QRST_DI_Resources.MyConsole.WriteLine(string.Format("更新云量满幅度索引异常：{0}", ex.Message));
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < updatedFileTiles.Count; i++)
                {
                    sb.AppendLine(updatedFileTiles[i]);
                }

                QRST_DI_Resources.MyConsole.WriteLine(sb.ToString());
            }


            return updatedFileTiles;
        }

        /// <summary>
        /// 还原目录下已添加云量满幅度信息的瓦片名称到没有云量满幅度信息的瓦片名称
        /// </summary>
        /// <param name="destDir">指定瓦片所在目录</param>
        private void rollbackTileName(string destDir, string tilePrefix="")
        {
            FileInfo[] cfiles = new DirectoryInfo(destDir).GetFiles();
            foreach (FileInfo fi in cfiles)
            {
                if (tilePrefix == "")
                {
                    bool rst=TileNameArgs.GetTilePrefixWithoutAvailabilityCloud(fi.Name, out tilePrefix);
                    if (!rst)
                    {
                        continue;
                    }
                }

                try
                {
                    string newname = "";
                    if (fi.Name.EndsWith("-1.tif"))
                    {
                        newname = tilePrefix + "-1.tif";
                        if (fi.Name != newname)
                        {
                            fi.MoveTo(Path.Combine(Path.GetDirectoryName(fi.FullName), newname));
                        }
                    }
                    else if (fi.Name.EndsWith("-2.tif"))
                    {
                        newname = tilePrefix + "-2.tif";
                        if (fi.Name != newname)
                        {
                            fi.MoveTo(Path.Combine(Path.GetDirectoryName(fi.FullName), newname));
                        }
                    }
                    else if (fi.Name.EndsWith("-3.tif"))
                    {
                        newname = tilePrefix + "-3.tif";
                        if (fi.Name != newname)
                        {
                            fi.MoveTo(Path.Combine(Path.GetDirectoryName(fi.FullName), newname));
                        }
                    }
                    else if (fi.Name.EndsWith("-4.tif"))
                    {
                        newname = tilePrefix + "-4.tif";
                        if (fi.Name != newname)
                        {
                            fi.MoveTo(Path.Combine(Path.GetDirectoryName(fi.FullName), newname));
                        }
                    }
                    else if (fi.Name.EndsWith(".png"))
                    {
                        newname = tilePrefix + ".png";
                        if (fi.Name != newname)
                        {
                            fi.MoveTo(Path.Combine(Path.GetDirectoryName(fi.FullName), newname));
                        }
                    }
                    else if (fi.Name.EndsWith(".pgw"))
                    {
                        newname = tilePrefix + ".pgw";
                        if (fi.Name != newname)
                        {
                            fi.MoveTo(Path.Combine(Path.GetDirectoryName(fi.FullName), newname));
                        }
                    }
                    else if (fi.Name.EndsWith("-Azimuth.tif"))
                    {
                        newname = tilePrefix + "-Azimuth.tif";
                        if (fi.Name != newname)
                        {
                            fi.MoveTo(Path.Combine(Path.GetDirectoryName(fi.FullName), newname));
                        }
                    }
                    else if (fi.Name.EndsWith("-Zenith.tif"))
                    {
                        newname = tilePrefix + "-Zenith.tif";
                        if (fi.Name != newname)
                        {
                            fi.MoveTo(Path.Combine(Path.GetDirectoryName(fi.FullName), newname));
                        }
                    }
                    else if (fi.Name.EndsWith(".jpg"))
                    {
                        newname = tilePrefix + ".png";
                        if (fi.Name != newname)
                        {
                            fi.MoveTo(Path.Combine(Path.GetDirectoryName(fi.FullName), newname));
                        }
                    }
                    else if (fi.Name.EndsWith(".jgw"))
                    {
                        newname = tilePrefix + ".pgw";
                        if (fi.Name != newname)
                        {
                            fi.MoveTo(Path.Combine(Path.GetDirectoryName(fi.FullName), newname));
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="tilepath"></param>
        /// <returns></returns>
        public List<DataSet> GetDataSetCol(string sql, string tilepath)
        {
            TileSearchUtil tileSearchUtil = new TileSearchUtil();
            return tileSearchUtil.GetDataSetCol(sql, tilepath);
        }
        public void ExecuteNonQuery(string sql, string tilepath)
        {
            TileSearchUtil tileSearchUtil = new TileSearchUtil();
            tileSearchUtil.ExecuteNonQuery(sql, tilepath);
        }
        /// <summary>
        /// 根据分发的分页信息，查询结果
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="recordNum"></param>
        /// <param name="siteModsList"></param>
        /// <returns></returns>
        public List<DataSet> GetDataSetColPaged2(string sql, out int recordNum, List<ModIDSearchInfo> siteModsList)
        {
            TileSearchUtil tileSearchUtil = new TileSearchUtil();
            return tileSearchUtil.GetDataSetColPaged2(sql, out recordNum, siteModsList);
        }
        /// <summary>
        /// 根据分发的分页信息，查询结果
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="recordNum"></param>
        /// <param name="siteModsList"></param>
        /// <returns></returns>
        public List<DataSet> GetDataSetColPaged2( out int recordNum, List<ModIDSearchInfo> siteModsList)
        {
            TileSearchUtil tileSearchUtil = new TileSearchUtil();
            return tileSearchUtil.GetDataSetColPaged2(out recordNum, siteModsList);
        }
#endregion

        #endregion


        #region 站点远程自动更新
        /// 获取指定文件夹下的所有文件夹
        /// </summary>
        /// <returns></returns>
        public void UpdateTSServer()
        {
            string curDir = AppDomain.CurrentDomain.BaseDirectory;
            string updaterExeDir = Path.Combine(curDir.Substring(0, curDir.TrimEnd('\\').LastIndexOf("\\")), "QDB_TS_Updater");
            string updaterExePath = Path.Combine(updaterExeDir, "QRST_DI_TS-ServerUpdater.exe");
            if (File.Exists(updaterExePath))
            {
                Process.Start(updaterExePath);
            }
        }
        #endregion

        /// <summary>
        /// 判断远程机是否运行
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public EnumSiteStatus GetServerStatus()
        {
            try
            {
                DbOperatingTCPServer slOperatingTcp = new DbOperatingTCPServer();

                IDbOperating dbOperating = null;
                switch (Constant.QrstDbEngine)
                {
                        case EnumDbEngine.SQLITE:
                        dbOperating = new DBSQLiteOperating();
                        break;
                        case EnumDbEngine.MYSQL:
                        dbOperating = new DBMySqlOperating();
                        break;
                }
               
                string sql = string.Format("select RunningState from tileserversitesinfo where ADDRESSIP = '{0}'", this.GetIPAddress());
                IDbBaseUtilities sqLiteBaseUtilities = dbOperating.GetSubDbUtilities(EnumDBType.MIDB);
                DataSet ds = sqLiteBaseUtilities.GetDataSet(sql);

                EnumSiteStatus sitestatustype = EnumSiteStatus.Stopped;
                Enum.TryParse(ds.Tables[0].Rows[0][0].ToString(), out sitestatustype);
                return sitestatustype;
            }
            catch
            {
            }
            return EnumSiteStatus.Stopped;
        }

        private static DirectlyAddressing _tileDA = new DirectlyAddressing(DirectlyAddressingIPMod.IPModDataSet);
        public int[][] GetTileImageData(string tilename)
        {
            System.Drawing.Bitmap img = null;
            string tilepath = _tileDA.GetPathByFileName(tilename);
            if (System.IO.File.Exists(tilepath))
            {
                img =new Bitmap(tilepath);
            }
            int[][] imgdata = new int[img.Width][];
            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    if (j == 0)
                    {
                        imgdata[i] = new int[img.Height];
                    }
                    imgdata[i][j]=img.GetPixel(i, j).ToArgb();
                }
            }
            return imgdata;
        }

        public byte[] GetTileMiniView(string tilename)
        {
            return TileThumbnailViewCreator.GetTileMiniView(_tileDA, tilename);
        }


        public byte[] GetTileTinyView(string tilename)
        {
            return TileThumbnailViewCreator.GetTileTinyView(_tileDA,tilename);
        }

        public void UpdateDistinctTables(string commonSharePath)
        {
            TileSearchUtil tileSearchUtil = new TileSearchUtil();
            tileSearchUtil.UpdateDistinctTables(commonSharePath);
        }

        /// <summary>
        /// 查询并删除有记录但文件缺失的瓦片记录，遍历每个配号下的DataSet，不分页返回全部结果。
        /// 能将旧瓦片名重命名为新瓦片名
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="CommonSharePath"></param>
        /// <returns></returns>
        public void UpdateAllTileFileRecord(string commonSharePath)
        {
            DirectoryInfo rootDi = new DirectoryInfo(commonSharePath.Replace("QRST_DB_Prototype", "QRST_DB_Tile"));
            DirectoryInfo[] modDis = rootDi.GetDirectories();

            List<Task> tasks = new List<Task>();

            foreach (DirectoryInfo modDi in modDis)
            {
                tasks.Add(Task.Factory.StartNew(o =>
                {
                    List<string> files = Directory.GetFiles(o.ToString(), "*.*", SearchOption.AllDirectories).Where(s => s.ToLower().EndsWith(".jpg") || s.ToLower().EndsWith(".png") || s.ToLower().EndsWith(".tiff") || s.ToLower().EndsWith(".tif")).ToList();
                    List<string> tilename = new List<string>();

                    for (int i = files.Count - 1; i > -1; i--)
                    {
                        string filename = Path.GetFileName(files[i]);
                        TileNameArgs tna = TileNameArgs.GetTileNameArgs(filename);
                        if (tna.Created)
                        {
                            tilename.Add(filename);
                        }
                    }

                    if (tilename.Count > 0)
                    {
                        QRST_DI_Resources.MyConsole.WriteLine(string.Format("正在更新{0}个瓦片记录...", tilename.Count));
                        TileIndexUpdateUtilities tiuu = new TileIndexUpdateUtilities();
                        tiuu.TileIndexUpdate(TileIndexUpdateType.InsertUpdate, tilename);
                    }
                }, modDi.FullName));
            }

            foreach (Task task in tasks)
            {
                task.Wait(-1);
            }
            QRST_DI_Resources.MyConsole.WriteLine("UpdateAllTileFileRecord 更新完成！");
        }

        /// <summary>
        /// 查询并删除有记录但文件缺失的瓦片记录，遍历每个配号下的DataSet，不分页返回全部结果。
        /// 能将旧瓦片名重命名为新瓦片名
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="CommonSharePath"></param>
        /// <returns></returns>
        public void UpdateDateTime8To10(string commonSharePath)
        {
            TileSearchUtil tileSearchUtil = new TileSearchUtil();
            string sql = "update correctedTiles Set[Date] =[Date]*100+24 WHERE [Date]<100000000;";
            tileSearchUtil.ExecuteNonQuery(sql, commonSharePath);
            sql = "update productTiles Set[Date] =[Date]*100+24 WHERE [Date]<100000000;";
            tileSearchUtil.ExecuteNonQuery(sql, commonSharePath);
            tileSearchUtil.UpdateGFFTable(commonSharePath);

            QRST_DI_Resources.MyConsole.WriteLine("UpdateDateTime8To10 更新完成！");
        }

        /// <summary>
        /// 查询缺少满幅度云量的瓦片（需要1.2.3.4波段）,计算更新瓦片的满幅度云量信息，重命名，更新记录。
        /// 建议调用本方法前执行下DeleteTileFileMissingRecord方法，对缺失数据的记录进行清理
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="commonSharePath"></param>
        /// <returns></returns>
        public DataSet UpdateAvailabilityCloudInfoOfTiles(string commonSharePath, bool updateGFF = false)
        {
            //查询有1.2.3.4四波段数据的瓦片
            //string sql = "Select * from gff";     //重建满幅度云量
            TileSearchUtil tileSearchUtil = new TileSearchUtil();

            if (updateGFF)
            {
                tileSearchUtil.UpdateGFFTable(commonSharePath);
            }

            string sql = "Select * from gff where Availability = 255 or Cloud = 255";       //常规 只针对缺失的数据

            List<DataSet> dss = tileSearchUtil.GetDataSetCol(sql, commonSharePath);
            DataSet DS = new DataSet();
            if (dss != null && dss.Count != 0)
            {
                List<Task> tasks = new List<Task>();
                foreach (DataSet ds in dss)
                {

                    //每一配号返回一个ds，每个配号启动一个线程执行任务。
                    tasks.Add(Task.Factory.StartNew(o =>
                    {
                        DataSet in_ds = o as DataSet;
                        List<string> updatedFileTiles = new List<string>();
                        List<string> updatedOldFileTiles = new List<string>();

                        if (in_ds != null && in_ds.Tables.Count > 0)
                        {

                            SQLBaseTool sqlbt = new SQLBaseTool();
                            DirectlyAddressing dautil = new DirectlyAddressing(DirectlyAddressingIPMod.IPModDataSet);

                            sqlbt.AddData_TileNameAddress(in_ds);   //gff结果集获得的文件名为快视图文件名
                            DataTable table = in_ds.Tables[0];
                            QRST_DI_Resources.MyConsole.WriteLine("待更新云量满幅度瓦片记录数：" + table.Rows.Count.ToString());
                            for (int i = table.Rows.Count - 1; i > -1; i--)
                            {
                                QRST_DI_Resources.MyConsole.WriteLine(string.Format("正在计算云量满幅度瓦片：{0}({1}/{2})", table.Rows[i]["TileFileName"].ToString(), table.Rows.Count - i, table.Rows.Count));


                                //跳过不需要更新的记录
                                if (Convert.ToInt16(table.Rows[i]["Availability"]) != 255 || Convert.ToInt16(table.Rows[i]["Cloud"]) != 255)
                                {
                                    QRST_DI_Resources.MyConsole.WriteLine(string.Format("{0}非255 255，不需要更新", table.Rows[i]["TileFileName"].ToString()));

                                    continue;
                                }

                                //需要1.2.3.4波段）,
                                bool exist4bands = true;
                                string filename = table.Rows[i]["TileFileName"].ToString();
                                string filepath = dautil.GetPathByFileName(filename);
                                string destDir = Path.GetDirectoryName(filepath);
                                CorrectedTileNameArgs tna = TileNameArgs.GetTileNameArgs(filename) as CorrectedTileNameArgs;

                                if (!File.Exists(filepath))
                                {
                                    if (tna == null || !tna.Created)
                                    {
                                        QRST_DI_Resources.MyConsole.WriteLine(string.Format("{0}目标文件不存在,且名称有误，不可识别，跳过", table.Rows[i]["TileFileName"].ToString()));

                                        //如果名称有误，则跳过，开始计算下一行记录
                                        continue;
                                    }

                                    filename = tna.GetOldStyleFilename();
                                    filepath = Path.Combine(destDir, filename);
                                    tna.Filename = filename;
                                    tna.IsOldNameStyle = true;
                                    if (!File.Exists(filepath))
                                    {
                                        QRST_DI_Resources.MyConsole.WriteLine(string.Format("{0}目标文件不存在！！{1}", table.Rows[i]["TileFileName"].ToString(), filepath));

                                        continue;
                                    }
                                }
                                string tileWithoutExt = Path.GetFileNameWithoutExtension(filename);
                                string[] tilefiles = new string[4];

                                if (tna.IsOldNameStyle)
                                {
                                    tilefiles[0] = string.Format(@"{0}\{1}-1.tif", destDir, tileWithoutExt);
                                    tilefiles[1] = string.Format(@"{0}\{1}-2.tif", destDir, tileWithoutExt);
                                    tilefiles[2] = string.Format(@"{0}\{1}-3.tif", destDir, tileWithoutExt);
                                    tilefiles[3] = string.Format(@"{0}\{1}-4.tif", destDir, tileWithoutExt);
                                }
                                else
                                {
                                    tileWithoutExt = Path.GetFileNameWithoutExtension(tileWithoutExt);       //去掉.c
                                    tilefiles[0] = string.Format(@"{0}\{1}-1.c.tif", destDir, tileWithoutExt);
                                    tilefiles[1] = string.Format(@"{0}\{1}-2.c.tif", destDir, tileWithoutExt);
                                    tilefiles[2] = string.Format(@"{0}\{1}-3.c.tif", destDir, tileWithoutExt);
                                    tilefiles[3] = string.Format(@"{0}\{1}-4.c.tif", destDir, tileWithoutExt);
                                }
                                string tileinfo = string.Format(@"{0}\{1}.info", destDir, tileWithoutExt);


                                #region 还原已修改的瓦片名称
                                //rollbackTileName(destDir);
                                #endregion


                                //检查源四波段数据存在性
                                foreach (string tilepath in tilefiles)
                                {
                                    if (!File.Exists(tilepath))
                                    {
                                        QRST_DI_Resources.MyConsole.WriteLine(string.Format("{0}找不到文件，无法计算，跳过", tilepath));
                                        exist4bands = false;
                                        break;

                                    }
                                }

                                if (!exist4bands)
                                {
                                    //如果四波段数据不完整，则跳过，开始计算下一行记录
                                    continue;
                                }

                                try
                                {
                                    QRST_DI_Resources.MyConsole.WriteLine(string.Format("启动云量满幅度计算进程：{0}({1}/{2})", table.Rows[i]["TileFileName"].ToString(), table.Rows.Count - i, table.Rows.Count));
                                    //计算更新瓦片的满幅度云量信息
                                    ProcessStartInfo psi = new ProcessStartInfo(Path.GetDirectoryName(Application.ExecutablePath) + @"\plugin\CloudMask\CloudMask.exe");
                                    psi.UseShellExecute = false;
                                    psi.CreateNoWindow = true;
                                    psi.Arguments = string.Format("2 \"{0}\" \"{1}\" \"{2}\" \"{3}\" \"{4}\" 64 128", tilefiles[0], tilefiles[1], tilefiles[2], tilefiles[3], tileinfo);

                                    using (Process p = Process.Start(psi))
                                    {
                                        p.WaitForExit();
                                        int iii = 0;
                                        while (!File.Exists(tileinfo))
                                        {
                                            if (iii > 15)
                                            {

                                                QRST_DI_Resources.MyConsole.WriteLine(string.Format("等待云量满幅度计算进程退出超时：{0}({1}/{2})", table.Rows[i]["TileFileName"].ToString(), table.Rows.Count - i, table.Rows.Count));
                                                throw new ApplicationException(string.Format("CloudMask.exe of file '{0}' failed.", tilefiles[0]));
                                            }
                                            System.Threading.Thread.Sleep(200);
                                            iii++;
                                        }


                                    }

                                    QRST_DI_Resources.MyConsole.WriteLine(string.Format("完成云量满幅度计算：{0}({1}/{2})", table.Rows[i]["TileFileName"].ToString(), table.Rows.Count - i, table.Rows.Count));

                                    string[] info = File.ReadAllLines(tileinfo);
                                    string[] info_x2 = new string[2];
                                    info_x2[0] = Convert.ToInt16(info[0].Trim()).ToString("X2");       //云量
                                    info_x2[1] = Convert.ToInt16(info[1].Trim()).ToString("X2");       //满幅度
                                    //info[0] = (info[0].Length == 1) ? "0" + info[0] : info[0];
                                    //info[1] = (info[1].Length == 1) ? "0" + info[1] : info[1];


                                    //重命名文件
                                    // tileWithoutExt=HJ1A_CCD1_20161212_L20000521090_4_241_599
                                    //newTileWithoutExt=HJ1A_CCD1_20161212_L20000521090#6400_4_241_599
                                    tna.Availability = Convert.ToInt16(info[1].Trim());
                                    tna.Cloud = Convert.ToInt16(info[0].Trim());
                                    string newTileWithoutExt = tna.GetNewStyleFilename();
                                    newTileWithoutExt = newTileWithoutExt.Substring(0, newTileWithoutExt.Length - ".c.png".Length);

                                    FileInfo[] files = new DirectoryInfo(destDir).GetFiles();
                                    foreach (FileInfo fi in files)
                                    {
                                        if (fi.Extension == ".info")
                                        {
                                            try
                                            {
                                                File.Delete(fi.FullName);
                                            }
                                            catch (Exception ex)
                                            {

                                                QRST_DI_Resources.MyConsole.WriteLine(string.Format("删除info文件异常，继续：{0}({1}/{2}) {3}，{4} ", table.Rows[i]["TileFileName"].ToString(), table.Rows.Count - i, table.Rows.Count, fi.FullName, ex.Message));
                                            }
                                            continue;
                                        }

                                        if (fi.Name.StartsWith(tileWithoutExt))
                                        {
                                            string oldname = Path.GetFileName(fi.FullName);
                                            string newpath = fi.FullName.Replace(tileWithoutExt, newTileWithoutExt);
                                            if (!fi.Name.StartsWith(newTileWithoutExt))
                                            {
                                                try
                                                {

                                                    fi.MoveTo(newpath);
                                                }
                                                catch (Exception ex)
                                                {
                                                    QRST_DI_Resources.MyConsole.WriteLine(string.Format("文件重命名异常，继续：{0}({1}/{2}) {3} {4} ", table.Rows[i]["TileFileName"].ToString(), table.Rows.Count - i, table.Rows.Count, newpath, ex.Message));
                                                }
                                            }

                                            if (fi.Name.ToUpper().EndsWith(".TIF") || fi.Name.ToUpper().EndsWith(".JPG") || fi.Name.ToUpper().EndsWith(".PNG"))
                                            {
                                                updatedFileTiles.Add(Path.GetFileName(newpath));
                                                updatedOldFileTiles.Add(oldname);
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    QRST_DI_Resources.MyConsole.WriteLine(string.Format("计算云量满幅度异常，继续：{0}({1}/{2}) {3} ", table.Rows[i]["TileFileName"].ToString(), table.Rows.Count - i, table.Rows.Count, ex.Message));
                                }

                            }
                            try
                            {
                                if (updatedFileTiles.Count > 0)
                                {
                                    TileIndexUpdateUtilities tiuu = new TileIndexUpdateUtilities();
                                    tiuu.TileIndexUpdate(TileIndexUpdateType.Delete, updatedOldFileTiles);
                                    tiuu.TileIndexUpdate(TileIndexUpdateType.InsertUpdate, updatedFileTiles);
                                }
                            }
                            catch (Exception ex)
                            {
                                QRST_DI_Resources.MyConsole.WriteLine(string.Format("更新云量满幅度索引异常：{0}", ex.Message));
                                StringBuilder sb = new StringBuilder();
                                for (int i = 0; i < updatedFileTiles.Count; i++)
                                {
                                    sb.AppendLine(updatedFileTiles[i]);
                                }

                                QRST_DI_Resources.MyConsole.WriteLine(sb.ToString());
                            }
                        }
                    }, ds));
                }

                foreach (Task task in tasks)
                {
                    task.Wait(-1);
                }
            }

            foreach (DataSet ds in dss)
            {
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (DS.Tables.Count == 0)
                    {
                        DS.Tables.Add(ds.Tables[0].Copy());
                    }
                    else
                    {
                        //DataRow[] drs=new DataRow[ds.Tables[0].Rows.Count];
                        //ds.Tables[0].Rows.CopyTo(drs, 0);
                        //DS.Tables[0].LoadDataRow(drs,  LoadOption.OverwriteChanges);

                        DS.Merge(ds, false, MissingSchemaAction.Add);
                    }
                }
            }

            try
            {
                QRST_DI_Resources.MyConsole.WriteLine("更新完成，更新文件瓦片记录数：" + DS.Tables[0].Rows.Count.ToString());
            }
            catch
            {
                QRST_DI_Resources.MyConsole.WriteLine("更新完成，更新文件瓦片记录数：0");
            }

            return DS;
        }

    }
}
