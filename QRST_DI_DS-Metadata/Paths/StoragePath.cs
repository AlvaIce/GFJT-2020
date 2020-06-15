using System;
using System.Collections.Generic;
using QRST_DI_Resources;
using QRST_DI_DS_Metadata.MetaDataDefiner.Dal;
using System.Data;
using System.Text;
using System.IO;
using QRST_DI_DS_Metadata.MetaDataCls;
using System.Security.Cryptography;
using System.Threading.Tasks;
using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetaData;
using QRST_DI_SS_DBInterfaces.IDBEngine;
 
namespace QRST_DI_DS_Metadata.Paths
{
    public class StoragePath
    {

        //public static string StoreBasePath = string.Format(@"\\{0}\{2}\{1}\", Constant.DeployedHadoopIP, "综合数据库","test");
        public static string StoreBasePath
        {
            get
            {
                string path = "";
                switch (Constant.DbStorage)
                {
                    case EnumDbStorage.MULTIPLE:
                        path= string.Format(@"\\{0}\{1}\", QRST_DI_Resources.Constant.DeployedHadoopIP, StaticStrings.RootDir);
                        break;
                    case EnumDbStorage.SINGLE:
                        path = string.Format(@"{0}\{1}\", Constant.PcDBRootPath, StaticStrings.RootDir);
                        break;
                }
                return path;
            }
        }

        

        static List<string> _storeHistoryPath;
        public static List<string> StoreHistoryPath
        {
            get
            {
                _storeHistoryPath = (_storeHistoryPath == null) ? new List<string>() : _storeHistoryPath;
                string[] hadoopHistory = QRST_DI_Resources.Constant.DeployedHadoopIP_History.Split(';');
                foreach (var item in hadoopHistory)
                {
                    string hadoopHis = string.Format(@"\\{0}\{1}\", item, StaticStrings.RootDir);
                    lock (_storeHistoryPath)
                    {
                        if (!_storeHistoryPath.Contains(hadoopHis))
                        {
                            _storeHistoryPath.Add(hadoopHis);
                        }
                    }
                }
                return _storeHistoryPath;
            }
        }

        //private DBMySqlOperating mySqlOperater = new DBMySqlOperating();
        //private MySqlBaseUtilities mySqlUtilities = new MySqlBaseUtilities();
        private IDbBaseUtilities mySqlUtilities = Constant.IdbServerUtilities;
        private IDbOperating mySqlOperater = Constant.IdbOperating;
        private string _dataCode;
        private DataSet Ds;
        int maxNum;
        /// <summary>
        /// 根据订单号获取临时存放文件夹
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string TempStoragePath(string ipAddress, string code)
        {
            return string.Format(@"\\{0}\{1}\{2}", ipAddress, StaticStrings.QRST_DB_Share, code);
        }

        public StoragePath(string datacode)
        {
            //MySqlBaseUtilities mysqlconn = new MySqlBaseUtilities();
            //Ds = mysqlconn.GetDataSet("select * from metadatainfo");
            Ds = mySqlUtilities.GetDataSet("select * from metadatainfo");
            maxNum = GetMaxCode();
            _dataCode = datacode;
            if (datacode != null && datacode.Length >= 4)
            {
                string db = datacode.Substring(0, 4);

                switch (db)
                {
                    case "BSDB":
                        mySqlUtilities = mySqlOperater.GetSubDbUtilities(EnumDBType.BSDB);
                        break;
                    case "EVDB":
                        mySqlUtilities = mySqlOperater.GetSubDbUtilities(EnumDBType.EVDB);
                        break;
                    case "RCDB":
                        mySqlUtilities = mySqlOperater.GetSubDbUtilities(EnumDBType.RCDB);
                        break;
                    case "ISDB":
                        mySqlUtilities = mySqlOperater.GetSubDbUtilities(EnumDBType.ISDB);
                        break;
                    case "MADB":
                        mySqlUtilities = mySqlOperater.GetSubDbUtilities(EnumDBType.MADB);
                        break;
                    case "IPDB":
                        mySqlUtilities = mySqlOperater.GetSubDbUtilities(EnumDBType.IPDB);
                        break;
                    case "INDB":
                        mySqlUtilities = mySqlOperater.GetSubDbUtilities(EnumDBType.INDB);
                        break;
                    default:
                        break;
                }
            }
            else
            {

            }
        }

        public string OriginalFilename = "";

        public IDbBaseUtilities GetMysqlBaseUtilities()
        {
            return mySqlUtilities;
        }

        public string[] DataStorePath()
        {
            string[] dataPath = null;
            string tablename = GetTableNameBydataCode();
            table_view_Dal tableview = new table_view_Dal(mySqlUtilities);
            switch (tablename.ToUpper())
            {
                case "PROD_HJ": dataPath = new string[] { tableview.GetField(tablename, "SATELLITE"), tableview.GetField(tablename, "Sensor"), tableview.GetField(tablename, "SceneDate"), tableview.GetField(tablename, "NAME") }; break;
                case "PROD_NOAA": dataPath = new string[] { tableview.GetField(tablename, "SATELLITE"), tableview.GetField(tablename, "Sensor"), tableview.GetField(tablename, "StartDate") }; break;
                case "PROD_CBERS": dataPath = new string[] { tableview.GetField(tablename, "SATELLITE"), tableview.GetField(tablename, "Sensor"), tableview.GetField(tablename, "SceneDate") }; break;
                case "PROD_MODIS": dataPath = new string[] { tableview.GetField(tablename, "SATELLITE"), tableview.GetField(tablename, "Sensor"), tableview.GetField(tablename, "StartDate") }; break;
                //case "PRODS_VECTOR": dataPath = new string[] { tableview.GetField(tablename, "DataName"), };
                case "PRODS_VECTOR":
                    dataPath = new string[] { tableview.GetField(tablename, "resTitle"), };
                    break;
            }
            return dataPath;
        }

        /// <summary>
        /// 获取非常规路径的相对路径部分
        /// </summary>
        /// <returns></returns>
        //public string DataStorePath()
        //{
        //    MetaData metadata = null;
        //    string tablename = GetTableNameBydataCode();
        //    table_view_Dal tableview = new table_view_Dal(mySqlUtilities);
        //    switch (tablename.ToUpper())
        //    {
        //        case "PROD_HJ": metadata = new MetaDataHj(); break;
        //        case "PROD_NOAA": metadata = new MetaDataNOAA(); break;
        //        case "PROD_CBERS": metadata = new MetaDataCbers(); break;
        //        case "PROD_MODIS": metadata = new MetaDataModis(); break;
        //    }

        //    if (metadata == null)
        //    {
        //        return null;
        //    }
        //    else
        //    {
        //        return metadata.GetRelateDataPath();
        //    }
        //}

        private string GetTableNameBydataCode()
        {
            string tablename = "";
            tablename = (new tablecode_Dal(mySqlUtilities)).GetTableName(_dataCode);
            return tablename;
        }
        /// <summary>
        /// 不包含表的具体相对路径
        /// </summary>
        /// <returns></returns>
        public string getGroupAddress()
        {
            StringBuilder ralativeAddress = new StringBuilder();
            ralativeAddress.Append(StoragePath.StoreBasePath);
            List<string> listGroupName = new List<string>();

            string groupCode = _dataCode;
            getAddressParts(listGroupName, groupCode);

            if (listGroupName.Count > 0)
            {
                for (int i = listGroupName.Count - 1; i >= 0; i--)
                {
                    listGroupName[i] += @"\";
                    ralativeAddress.Append(listGroupName[i]);
                }
            }
            //1:25万什么的路径会出问题，用中文的：来替换。
            ralativeAddress = ralativeAddress.Replace(":", "：");
            return ralativeAddress.ToString();
        }

        /// <summary>
        /// 获取完整相对路径//基础空间数据添加
        /// </summary>
        /// <returns></returns>
        public string getRalativeAddress(string groupCode)
        {
            StringBuilder ralativeAddress = new StringBuilder();
            ralativeAddress.Append(StoragePath.StoreBasePath);
            //string AddressPart2 
            //StringBuilder strbuild = new StringBuilder();
            //strbuild.Append()
            List<string> listGroupName = new List<string>();

            //string groupCode = GetGroupCodeByDataCode(_dataCode);
            getAddressParts(listGroupName, groupCode);

            if (listGroupName.Count > 0)
            {
                for (int i = listGroupName.Count - 1; i >= 0; i--)
                {
                    listGroupName[i] += @"\";
                    ralativeAddress.Append(listGroupName[i]);
                }
            }
            ralativeAddress = ralativeAddress.Replace(":", "：");
            return ralativeAddress.ToString();
        }

        /// <summary>
        /// 获取完整相对路径
        /// </summary>
        /// <returns></returns>
        public string getRalativeAddress()
        {
            StringBuilder ralativeAddress = new StringBuilder();
            ralativeAddress.Append(StoragePath.StoreBasePath);
            //string AddressPart2 
            //StringBuilder strbuild = new StringBuilder();
            //strbuild.Append()
            List<string> listGroupName = new List<string>();

            string groupCode = GetGroupCodeByDataCode(_dataCode);
            getAddressParts(listGroupName, groupCode);

            if (listGroupName.Count > 0)
            {
                for (int i = listGroupName.Count - 1; i >= 0; i--)
                {
                    listGroupName[i] += @"\";
                    ralativeAddress.Append(listGroupName[i]);
                }
            }
            ralativeAddress = ralativeAddress.Replace(":", "：");
            return ralativeAddress.ToString();
        }

        /// <summary>
        /// 递归调用的方法每次从树结构中获取相对路径的一级
        /// </summary>
        /// <param name="listName"></param>
        /// <param name="groupCode"></param>
        public void getAddressParts(List<string> listName, string groupCode)
        {
            string groupName = GetGroupNameByCode(groupCode);
            listName.Add(groupName);

            if (GetGroupTypeByCode(groupCode).Trim() != "root;")
            {
                string newGroupCode = GetPreGroupCode(groupCode);
                getAddressParts(listName, newGroupCode);
            }

        }
        ///// <summary>
        ///// 得到配号-节点数据集
        ///// </summary>
        //public DataSet GetDataSet()
        //{
        //    DataSet ds = new DataSet();
        //    MySqlConnection conn = new MySqlConnection("DataSource=192.168.10.104;Database=midb;UserID=HJDATABASE_ADMIN;Password=dbadmin_2011");
        //    MySqlCommand cmd;
        //    try
        //    {
        //        conn.Open();
        //        if (conn.State == ConnectionState.Open)
        //        {
        //            Console.WriteLine("连接成功！");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //    cmd = new MySqlCommand("select*from metadatainfo", conn);
        //    MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
        //    sda.Fill(ds);
        //    return ds;
        //}
        /// <summary>
        /// 哈希码获取
        /// </summary>
        /// <returns></returns>
        public int GetHSCode(string qrst_code)
        {
            //通过qrst_code读取数据库中对应原始图像的文件名
            IDbOperating mySQLOperator = Constant.IdbOperating;
            IDbBaseUtilities MySqlBaseUti;
            //mySQLOperator = new DBMySqlOperating();
            MySqlBaseUti = mySQLOperator.GetSubDbUtilities(EnumDBType.EVDB);
            DataSet ds = MySqlBaseUti.GetDataSet(string.Format("select Name from prod_gf1 where qrst_code = '{0}'", qrst_code));
            String fileName = ds.Tables[0].Rows[0][0].ToString();
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.Default.GetBytes(fileName);//将字符编码为一个字节序列 
            byte[] md5data = md5.ComputeHash(data);//计算data字节数组的哈希值 
            string Xstr = "";
            for (int i = 0; i < md5data.Length; i++)
            {
                Xstr += md5data[i].ToString("x").PadLeft(2, '0');//将字节数组的每个值转换成16进制，长度为2，左填充0
            }
            string Dstr = Xstr.Substring(0, 2);
            return Convert.ToInt32(Dstr, 16);
        }
        /// <summary>
        /// VDS配号最大值获取
        /// </summary>
        /// <returns></returns>
        public int GetMaxCode()
        {
            int temp = 0;
            for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
            {
                for (int j = 0; j < Ds.Tables[0].Rows[i][2].ToString().Split(',').Length; j++)
                {
                    if (Convert.ToInt32(Ds.Tables[0].Rows[i][2].ToString().Split(',')[j]) > temp)
                    {
                        temp = Convert.ToInt32(Ds.Tables[0].Rows[i][2].ToString().Split(',')[j]);
                    }
                }
            }
            return temp;//int数组中有一个方法，可以直接获取最大值
        }
        /// <summary>
        /// 分散存储路径获取
        /// </summary>
        /// <returns></returns>
        public string GetNewDataPath(string qrst_code)
        {
            //获取数据的根路径
            int Row = 0;
            int hsCode = 0;
            bool bl = false;
            hsCode = GetHSCode(qrst_code);
            int code = hsCode % maxNum;
            // int code = GetHSCode(qrst_code) % maxNum;
            for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
            {
                for (int j = 0; j < Ds.Tables[0].Rows[i][2].ToString().Split(',').Length; j++)
                {
                    if (Convert.ToInt32(Ds.Tables[0].Rows[i][2].ToString().Split(',')[j]) == code)
                    {
                        Row = i;
                        bl = true;
                    }
                }
                if (bl)
                {
                    break;
                }
            }
            string rootPath = string.Format(@"\\{0}\{1}\{2}\", Ds.Tables[0].Rows[Row][1].ToString(), StaticStrings.RootDir, code);
            //获取数据的相对路径
            MetaData metaData = MetaDataFactory.CreateMetaDataInstance(qrst_code);
            if (metaData.IsCreated)
            {
                string relatePath = metaData.GetRelateDataPath(out OriginalFilename);  //若为按照元数据信息组织路径，则返回路径，若为树结构组织的常规寻址，则返回“”

                if (string.IsNullOrEmpty(relatePath))  //常规寻址
                {
                    string tableCode = GetTableCodeByQrstCode(qrst_code);
                    string groupCode = GetGroupCodeByDataCode(tableCode);
                    if (groupCode == string.Empty)
                    {
                        return "";
                    }
                    StringBuilder ralativeAddress = new StringBuilder();
                    List<string> listGroupName = new List<string>();
                    GetAddressRelateParts(listGroupName, groupCode);
                    if (listGroupName.Count > 0)
                    {
                        for (int i = listGroupName.Count - 1; i >= 0; i--)
                        {
                            listGroupName[i] += @"\";
                            ralativeAddress.Append(listGroupName[i]);
                        }
                    }
                    if (!ralativeAddress.ToString().EndsWith(@"\"))
                    {
                        ralativeAddress.AppendFormat(@"\{0}", qrst_code);
                    }
                    else
                    {
                        ralativeAddress.AppendFormat(@"{0}", qrst_code);
                    }
                    relatePath = ralativeAddress.ToString();
                }
                string path = string.Format("{0}{1}", rootPath, relatePath);
                return path;
            }
            //此步无需遍历根目录原因是，根目录是生成目录，获得路径即为真实路径
            else  //qrst_code无效，没能在库中找到对应的元数据记录表
            {
                return "";
            }
        }
        /// <summary>
        /// 根据数据的qrst_code获取数据的完整路径  zxw 2013/03/22 
        /// 这里分两种情况：常规寻址、非常规寻址
        /// 常规寻址：是根据该数据的目录结构构建路径，找到数据库中对应的数据存放位置
        /// 非常规寻址：如环境星数据是根据其元数据中部分属性值构成路径，因此需要查找元数据信息构建数据路径
        /// </summary>
        /// <param name="qrst_code"></param>
        /// <returns></returns>
        public string GetOldDataPath(string qrst_code)
        {
            if (!qrst_code.Contains("#"))                               //不含有Name的qrst_code
            {
                //获取数据的相对路径
                MetaData metaData = MetaDataFactory.CreateMetaDataInstance(qrst_code);
                return GetOldDataPath(metaData);
            }
            else
            {
                //jcgx传入的数据qrst_code带#数据名称
                return GetOldDataPathByJCGXcode(qrst_code);
            }
        }

        private string GetOldDataPathByJCGXcode(string jcgxQrstcode)
        {
            string dataID = jcgxQrstcode.Split('#')[0];                   //jcgx传入的数据qrst_code
            string dataName = jcgxQrstcode.Split('#')[1];             // jcgx传入的数据名称
            //获取数据的相对路径
            MetaData metaData = MetaDataFactory.CreateMetaDataInstance(dataID);

            if (metaData != null && metaData.IsCreated)
            {
                string rootPath = StoragePath.StoreBasePath;      //数据的根路径
                //应JCGX要求当传入四个参数时,对Name参数拼接出来的路径与Qrst_code查询出的进行比对
                string relatePath = metaData.GetRelateDataPath(out OriginalFilename);  //若为按照元数据信息组织路径，则返回路径，若为树结构组织的常规寻址，则返回“”
                //判断集成共享传入的数据名称是否与库表中数据名称一致
                string[] strArr = dataName.Split("_".ToCharArray());
                string satellite = strArr[0];
                string sensor = strArr[1];
                string year = strArr[4].Substring(0, 4);
                string month = strArr[4].Substring(4, 2);
                string day = strArr[4].Substring(6, 2);
                string relatePathGCGX = string.Format("实验验证数据库\\GF1卫星数据\\{0}\\{1}\\{2}\\{3}\\{4}\\{5}\\", satellite, sensor, year, month, day, Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(dataName)));
                if (relatePath == relatePathGCGX)
                {
                    if (string.IsNullOrEmpty(relatePath))  //常规寻址
                    {
                        string tableCode = GetTableCodeByQrstCode(dataID);
                        string groupCode = GetGroupCodeByDataCode(tableCode);
                        if (groupCode == string.Empty)
                        {
                            return "";
                        }
                        StringBuilder ralativeAddress = new StringBuilder();
                        List<string> listGroupName = new List<string>();
                        GetAddressRelateParts(listGroupName, groupCode);
                        if (listGroupName.Count > 0)
                        {
                            for (int i = listGroupName.Count - 1; i >= 0; i--)
                            {
                                listGroupName[i] += @"\";
                                ralativeAddress.Append(listGroupName[i]);
                            }
                        }
                        if (!ralativeAddress.ToString().EndsWith(@"\"))
                        {
                            ralativeAddress.AppendFormat(@"\{0}", dataID);
                        }
                        else
                        {
                            ralativeAddress.AppendFormat(@"{0}", dataID);
                        }
                        relatePath = ralativeAddress.ToString();
                    }

                    string path = GetExistPath(rootPath, relatePath);
                    return path;
                }
                else
                {
                    //JCGX提供数据名称与库表中数据名称不一致,选择提供集成共享名称数据
                    if (string.IsNullOrEmpty(relatePathGCGX))  //常规寻址
                    {
                        string tableCode = GetTableCodeByQrstCode(dataID);
                        string groupCode = GetGroupCodeByDataCode(tableCode);
                        if (groupCode == string.Empty)
                        {
                            return "";
                        }
                        StringBuilder ralativeAddress = new StringBuilder();
                        List<string> listGroupName = new List<string>();
                        GetAddressRelateParts(listGroupName, groupCode);
                        if (listGroupName.Count > 0)
                        {
                            for (int i = listGroupName.Count - 1; i >= 0; i--)
                            {
                                listGroupName[i] += @"\";
                                ralativeAddress.Append(listGroupName[i]);
                            }
                        }
                        if (!ralativeAddress.ToString().EndsWith(@"\"))
                        {
                            ralativeAddress.AppendFormat(@"\{0}", dataID);
                        }
                        else
                        {
                            ralativeAddress.AppendFormat(@"{0}", dataID);
                        }
                        relatePathGCGX = ralativeAddress.ToString();
                    }

                    string path = GetExistPath(rootPath, relatePath);
                    return path;
                }
            }
            else  //qrst_code无效，没能在库中找到对应的元数据记录表
            {
                return "";
            }
        }


        /// <summary>
        /// 获取新入库数据的目标保存路径，区别与已有存档数据，无需判断是否存在
        /// </summary>
        /// <param name="metaData"></param>
        /// <returns></returns>
        public string GetNewDataPath(MetaData metaData)
        {
            if (metaData != null && metaData.IsCreated)
            {
                string rootPath = StoragePath.StoreBasePath;      //数据的根路径

                string relatePath = metaData.GetRelateDataPath(out OriginalFilename);  //若为按照元数据信息组织路径，则返回路径，若为树结构组织的常规寻址，则返回“”
                string path = string.Format("{0}{1}", rootPath, relatePath);
                return path;
            }
            else  //qrst_code无效，没能在库中找到对应的元数据记录表
            {
                return "";
            }
        }


        public string GetOldDataPath(MetaData metaData)
        {
            string rootPath = "";
            if (metaData != null && metaData.IsCreated)
            {
                        rootPath = StoragePath.StoreBasePath;      //数据的根路径
                string relatePath = metaData.GetRelateDataPath(out OriginalFilename);  //若为按照元数据信息组织路径，则返回路径，若为树结构组织的常规寻址，则返回“”
                if (string.IsNullOrEmpty(relatePath))  //常规寻址
                {
                    string tableCode = GetTableCodeByQrstCode(metaData.QRST_CODE);
                    string groupCode = GetGroupCodeByDataCode(tableCode);
                    if (groupCode == string.Empty)
                    {
                        return "";
                    }
                    StringBuilder ralativeAddress = new StringBuilder();
                    List<string> listGroupName = new List<string>();
                    GetAddressRelateParts(listGroupName, groupCode);
                    if (listGroupName.Count > 0)
                    {
                        for (int i = listGroupName.Count - 1; i >= 0; i--)
                        {
                            listGroupName[i] += @"\";
                            ralativeAddress.Append(listGroupName[i]);
                        }
                    }
                    if (!ralativeAddress.ToString().EndsWith(@"\"))
                    {
                        ralativeAddress.AppendFormat(@"\{0}", metaData.QRST_CODE);
                    }
                    else
                    {
                        ralativeAddress.AppendFormat(@"{0}", metaData.QRST_CODE);
                    }
                    relatePath = ralativeAddress.ToString();
                }
                string path = GetExistPath(rootPath, relatePath);
                return path;
            }
            else  //qrst_code无效，没能在库中找到对应的元数据记录表
            {
                return "";
            }
        }


        static public string GetGFDataExistPathByName(string filename, bool importUsing)
        {
            string relatefilePath = MetaDataGF1.GetRelateDataPath(filename);

            return GetExistPathByRelatePath(relatefilePath, importUsing);
        }


        static public string GetExistPathByRelatePath(string relatefilePath, bool importUsing)
        {
            if (importUsing)
            {
                return GetExistPath(StoragePath.StoreBasePath, relatefilePath);
            }
            else
            {
                string path = GetExistPath(StoragePath.StoreBasePath, relatefilePath);
                if (File.Exists(path))
                    return path;
                else
                    return "-1";
            }
        }

        public static string GetExistPath(string rootPath, string relatePath)
        {
            string path = string.Format("{0}{1}", rootPath, relatePath);
            //20140324 ksk添加，用于多个磁盘阵列检索。默认先检索当前磁盘阵列，若找到，则返回地址，若未找到，则检索历史磁盘阵列。
            //\\192.168.10.190\zhsjk\实验验证数据库\GF1卫星数据\GF1\WFV2\2016\01\10\GF1_WFV2_E116.1_N39.3_20160110_L1A0001324006\
            if (Directory.Exists(path))
                return path;
            else if (File.Exists(path))
                return path;
            else
            {
                string taskpath = "";//为空代表在该磁盘阵列中没有检索到该路径path
                List<System.Threading.Tasks.Task<string>> tasklist = new List<Task<string>>();
                foreach (var item in StoragePath.StoreHistoryPath)//循环遍历历史磁盘
                {
                    Task<string> task = System.Threading.Tasks.Task.Factory.StartNew<string>((o) =>
                    {
                        string pathHistory = string.Format("{0}{1}", o, relatePath);
                        if (Directory.Exists(pathHistory))
                            return pathHistory;
                        else if (File.Exists(pathHistory))
                            return pathHistory;
                        return "";
                    }, item);
                    tasklist.Add(task);
                }

                bool alltaskdone = false;//false代表多线程没有执行完毕
                while (taskpath == "" && !alltaskdone)
                {
                    alltaskdone = true;
                    foreach (Task<string> tk in tasklist)//循环遍历四个进程
                    {
                        if (tk.IsCompleted)//判断一个进程有没有完全执行完毕
                        {
                            taskpath = tk.Result;//返回进程结果
                            if (taskpath != "")//如果第一个磁盘没有存在该path，taskpath为空，然后执行continue，继续循环下一个磁盘tk;如果存在path；把该路径\\历史磁盘IP地址\zhsjk\信息服务库\文档成果\0001-ISDB-10-57“，break跳出；
                            {
                                break;
                            }
                            else
                            {
                                continue;
                            }
                        }
                        if (!(tk.IsCanceled || tk.IsCompleted || tk.IsFaulted))
                        {
                            alltaskdone = false;
                            break;
                        }
                    }
                    System.Threading.Thread.Sleep(50);
                }

                path = (taskpath != "") ? taskpath : path;
                return path;
            }
        }


        /// <summary>
        /// 获取最新路径
        /// </summary>
        /// <param name="qrst_code"></param>
        /// <returns></returns>
        public string GetDataPath(string qrst_code)
        {
            string strPath2 = "";
            strPath2 = GetOldDataPath(qrst_code);
            return strPath2;
        }

        /// <summary>
        /// 获取最新路径
        /// </summary>
        /// <param name="qrst_code"></param>
        /// <returns></returns>
        public string GetDataPath(MetaData metadata)
        {
            string strPath2 = "";
            strPath2 = GetOldDataPath(metadata);
            return strPath2;
        }

        #region 数据缺失检查时路径查找方法：数据不存在时，不返回生成路径，返回""值
        public string GetOldDataPathByCheck(string qrst_code)
        {
            string rootPath = StoragePath.StoreBasePath;      //数据的根路径

            //获取数据的相对路径
            MetaData metaData = MetaDataFactory.CreateMetaDataInstance(qrst_code);
            if (metaData.IsCreated)
            {
                string relatePath = metaData.GetRelateDataPath(out OriginalFilename);  //若为按照元数据信息组织路径，则返回路径，若为树结构组织的常规寻址，则返回“”
                if (string.IsNullOrEmpty(relatePath))  //常规寻址
                {
                    string tableCode = GetTableCodeByQrstCode(qrst_code);
                    string groupCode = GetGroupCodeByDataCode(tableCode);
                    if (groupCode == string.Empty)
                    {
                        return "";
                    }
                    StringBuilder ralativeAddress = new StringBuilder();
                    List<string> listGroupName = new List<string>();
                    GetAddressRelateParts(listGroupName, groupCode);
                    if (listGroupName.Count > 0)
                    {
                        for (int i = listGroupName.Count - 1; i >= 0; i--)
                        {
                            listGroupName[i] += @"\";
                            ralativeAddress.Append(listGroupName[i]);
                        }
                    }
                    if (!ralativeAddress.ToString().EndsWith(@"\"))
                    {
                        ralativeAddress.AppendFormat(@"\{0}", qrst_code);
                    }
                    else
                    {
                        ralativeAddress.AppendFormat(@"{0}", qrst_code);
                    }
                    relatePath = ralativeAddress.ToString();
                }
                string path = string.Format("{0}{1}", rootPath, relatePath);
                //20140324 ksk添加，用于多个磁盘阵列检索。默认先检索当前磁盘阵列，若找到，则返回地址，若未找到，则检索历史磁盘阵列。
                if (Directory.Exists(path))
                    return path;
                else
                {
                    foreach (var item in StoragePath.StoreHistoryPath)
                    {
                        string pathHistory = string.Format("{0}{1}", item, relatePath);
                        if (Directory.Exists(pathHistory))
                            return pathHistory;
                    }
                }
                return "";//路径不存在返回空 @jianghua
            }
            else  //qrst_code无效，没能在库中找到对应的元数据记录表
            {
                return "";
            }
        }

        public string GetDataPathByCheck(string qrst_code)
        {
            string strPath2 = "";
            strPath2 = GetOldDataPathByCheck(qrst_code);
            return strPath2;


        }
        #endregion

        /// <summary>
        /// 获取路径的相对组成部分  zxw 2013/3/22
        /// </summary>
        /// <param name="listName"></param>
        /// <param name="groupCode"></param>
        public void GetAddressRelateParts(List<string> listName, string groupCode)
        {
            string groupName = GetGroupNameByCode(groupCode);
            listName.Add(groupCode);

            if (GetGroupTypeByCode(groupCode).Trim() != "root;")
            {
                string newGroupCode = GetPreGroupCode(groupCode);
                GetAddressRelateParts(listName, newGroupCode);
            }

        }

        /// <summary>
        /// 根据数据qrst_code获取相对路径部分 zxw 2013/06/15
        /// </summary>
        /// <param name="qrst_code"></param>
        /// <returns></returns>
        public string GetAddressRelateParts(string qrst_code)
        {
            //获取存储该数据的表编码
            string tableCode = GetTableCodeByQrstCode(qrst_code);
            string groupCode = GetGroupCodeByDataCode(tableCode);
            if (groupCode == string.Empty)
            {
                return "";
            }
            List<string> listGroupName = new List<string>();
            GetAddressRelateParts(listGroupName, groupCode);
            StringBuilder ralativeAddress = new StringBuilder();
            if (listGroupName.Count > 0)
            {
                for (int i = listGroupName.Count - 1; i >= 0; i--)
                {
                    listGroupName[i] += @"\";
                    ralativeAddress.Append(listGroupName[i]);
                }
            }
            //if (!ralativeAddress.ToString().EndsWith(@"\"))
            //{
            //    ralativeAddress.AppendFormat(@"\{0}", qrst_code);
            //}
            //else
            //{
            //    ralativeAddress.AppendFormat(@"{0}", qrst_code);
            //}
            return ralativeAddress.ToString();
        }

        /// <summary>
        /// 根据新定义的数据qrst_code获取存储表的qrst_code
        /// </summary>示例：“0001-ISDB-31-5”，前面四位代表行业编号，中间ISDB-31代表存储表qrst_code,5代表数据ID
        /// <param name="qrst_code"></param>
        /// <returns>ISDB-31</returns>
        public static string GetTableCodeByQrstCode(string qrst_code)
        {
            int startIndex = qrst_code.IndexOf('-');
            int endIndex = qrst_code.LastIndexOf('-');

            return qrst_code.Substring(startIndex + 1, endIndex - startIndex - 1);
        }

        /// <summary>
        /// 根据数据编码获取数据库编码
        /// </summary>示例：“0001-ISDB-31-5”
        /// <param name="qrst_code"></param>
        /// <returns>ISDB</returns>
        public static string GetDBCode(string qrst_code)
        {
            int startIndex = qrst_code.IndexOf('-');
            return qrst_code.Substring(startIndex + 1, 4);
        }

        /// <summary>
        /// 根据Data_CODE获得GROUP_CODE
        /// </summary>
        /// <param name="dataCode"></param>
        /// <returns></returns>
        private string GetGroupNameByCode(string groupCode)
        {
            string groupName = string.Empty;

            string sqlStr = string.Format("select NAME from metadatacatalognode where GROUP_CODE = '{0}'", groupCode);
            DataSet ds = mySqlUtilities.GetDataSet(sqlStr);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                groupName = ds.Tables[0].Rows[0][0].ToString();
            }

            return groupName;
        }


        /// <summary>
        /// 根据Data_CODE获得GROUP_CODE
        /// </summary>
        /// <param name="dataCode"></param>
        /// <returns></returns>
        private string GetGroupCodeByDataCode(string dataCode)
        {
            string groupCode = string.Empty;

            string sqlStr = string.Format("select GROUP_CODE from metadatacatalognode where DATA_CODE = '{0}'", dataCode);
            DataSet ds = this.mySqlUtilities.GetDataSet(sqlStr);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                groupCode = ds.Tables[0].Rows[0][0].ToString();
            }
            return groupCode;
        }
        /// <summary>
        /// 根据GROUP_CODE获得父节点的GROUP_CODE
        /// </summary>
        /// <param name="dataCode"></param>
        /// <returns></returns>
        private string GetPreGroupCode(string groupCode)
        {
            string preGroupCode = string.Empty;

            string sqlStr = string.Format("select GROUP_CODE from metadatacatalognode_r where CHILD_CODE = '{0}'", groupCode);
            DataSet ds = this.mySqlUtilities.GetDataSet(sqlStr);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                preGroupCode = ds.Tables[0].Rows[0][0].ToString();
            }

            return preGroupCode;
        }
        /// <summary>
        /// 根据GROUP_CODE获得GROUP_TYPE
        /// </summary>
        /// <param name="dataCode"></param>
        /// <returns></returns>
        private string GetGroupTypeByCode(string groupCode)
        {
            string groupType = string.Empty;

            string sqlStr = string.Format("select GROUP_TYPE from metadatacatalognode where GROUP_CODE = '{0}'", groupCode);
            DataSet ds = this.mySqlUtilities.GetDataSet(sqlStr);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                groupType = ds.Tables[0].Rows[0][0].ToString();
            }

            return groupType;
        }

        /// <summary>
        /// 根据插件名获取插件的公共存放路径    zxw 20130924
        /// </summary>
        /// <param name="exeName"></param>
        /// <returns></returns>
        public static string GetPluginPath(string exeName)
        {
            string pluginPath = string.Format(@"{0}Plugin\{1}\{2}", StoreBasePath, Path.GetFileNameWithoutExtension(exeName), exeName);
            return pluginPath;
        }

        public static string GetPluginDir(string exeName)
        {
            string pluginPath = string.Format(@"{0}Plugin\{1}", StoreBasePath, Path.GetFileNameWithoutExtension(exeName));
            return pluginPath;
        }

        /// <summary>
        /// 获取本地插件路径
        /// </summary>
        /// <param name="exeName"></param>
        /// <returns></returns>
        public static string GetLocalPluginPath(string exeName)
        {
            return string.Format(@"{0}Plugin\{1}\{2}", AppDomain.CurrentDomain.BaseDirectory, Path.GetFileNameWithoutExtension(exeName), exeName);
        }

        /// <summary>
        /// 获取没有进行散列的路径
        /// </summary>
        /// <param name="qrst_code"></param>
        /// <returns></returns>
        public string GetDataOldPathForTools(string qrst_code)
        {
            string strPath1 = "";
            Task[] tasks = new Task[1];
            tasks[0] = new Task(() =>
            {
                strPath1 = GetOldDataPath(qrst_code);
            });
            tasks[0].Start();
           
            try
            {
                Task.WaitAll(tasks);
            }
            catch (System.AggregateException ex)
            {
                throw (ex.InnerException);
            }
            return strPath1;

            /*//分布式存储模式，再考量，暂时回退到老版本，JOKI 2014/11/03 成都部署前夕
            string strPath1 = "";
            strPath1 = GetOldDataPath(qrst_code);
            return strPath1;              
             */
        }


        /// <summary>
        /// 获取没有进行散列的路径
        /// </summary>
        /// <param name="qrst_code"></param>
        /// <returns></returns>
        public string GetDataOldPathForTools(MetaData metadata)
        {
            string strPath1 = "";
            Task[] tasks = new Task[1];
            tasks[0] = new Task(() =>
            {
                strPath1 = GetOldDataPath(metadata);
            });
            tasks[0].Start();
            try
            {
                Task.WaitAll(tasks);
            }
            catch (System.AggregateException ex)
            {
                throw (ex.InnerException);
            }
            return strPath1;

            /*//分布式存储模式，再考量，暂时回退到老版本，JOKI 2014/11/03 成都部署前夕
            string strPath1 = "";
            strPath1 = GetOldDataPath(qrst_code);
            return strPath1;              
             */
        }

        /// <summary>
        /// 根据快视图文件名获取相对路径
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string GetParentDirByQRSTThumbName(string filename)
        {
            int code = -1;
            string[] elems = Path.GetFileNameWithoutExtension(filename).Split('-');

            int.TryParse(elems[3], out code);
            string parentdir = string.Format(@"{0}", Convert.ToString(code / 1000));
            return parentdir;
        }

        /// <summary>
        /// 根据快视图文件名获取路径,不含文件名
        /// </summary>
        /// <param name="thumbBaseDir">快视图根目录路径</param>
        /// <param name="thumbfilename">快视图文件名</param>
        /// <returns></returns>
        public static string GetThumbPathByFileName(string thumbBaseDir, string thumbfilename)
        {
            string parentdir = GetParentDirByQRSTThumbName(thumbfilename);
            return Path.Combine(thumbBaseDir, parentdir);
        }

        /// <summary>
        /// 根据QRSTCODE获取快视图路径
        /// </summary>
        /// <param name="thumbBaseDir">快视图根目录路径</param>
        /// <param name="qrstcode">如0001-EVDB-32-32323</param>
        /// <returns>""代表没找到</returns>
        public static string GetThumbPathByQRSTCODE(string thumbBaseDir, string qrstcode)
        {
            //拼凑快视图文件名
            string thumbfilename = qrstcode + ".jpg";
            string thumbpath = GetThumbPathByFileName(thumbBaseDir, thumbfilename);
            string thumbfilepath = string.Format(@"{0}\{1}", thumbpath, thumbfilename);
            if (File.Exists(thumbfilepath))
            {
                return thumbfilepath;
            }
            else
            {
                thumbfilename = qrstcode + "-1.jpg";
                thumbpath = GetThumbPathByFileName(thumbBaseDir, thumbfilename);
                thumbfilepath = string.Format(@"{0}\{1}", thumbpath, thumbfilename);
                if (File.Exists(thumbfilepath))
                {
                    return thumbfilepath;
                }
            }
            return "";
        }
    }
}
