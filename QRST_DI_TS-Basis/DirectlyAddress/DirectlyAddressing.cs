
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using QRST_DI_Basis;
 
namespace QRST_DI_TS_Basis.DirectlyAddress
{
    public class DirectlyAddressing
    {

        public DataSet _dsIPMod;
        public int _MaxMod;//VDS的总数
        public DirectlyAddressing(DataSet dsIPMod)
        {
            _dsIPMod = dsIPMod;
            _MaxMod = getMaxMode();
        }

        
        /// <summary>
        /// 获取所有站点中最大的ModID
        /// </summary>
        /// <returns></returns>
        private int getMaxMode()
        {
            try
            {
                List<int> mods = new List<int>();
                int maxmod = -1;
                for (int i = 0; i < _dsIPMod.Tables[0].Rows.Count; i++)
                {
                    if (_dsIPMod.Tables[0].Rows[i][1].ToString().Length > 0)
                    {
                        string[] strmods = _dsIPMod.Tables[0].Rows[i][1].ToString().Split(",".ToCharArray());
                        foreach (string strmod in strmods)
                        {
                            int curMod = int.Parse(strmod);
                            mods.Add(curMod);
                            if (curMod > maxmod)
                            {
                                maxmod = curMod;
                            }
                        }
                    }
                }
                _MaxMod = maxmod + 1;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return _MaxMod;
        }

     

        /// <summary>
        /// 根据文件名提取切片的属性参数
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public TileNameArgs GetTileNameArgs(string filename)
        {
            TileNameArgs tnargs = new CorrectedTileNameArgs(filename);
            if (!tnargs.Created && !tnargs.IsThumbnail)
            {
                tnargs = new ProdTileNameArgs(filename);
            }
            else if (!tnargs.Created && !tnargs.IsThumbnail)
            {
                tnargs = new ClassifySampleTileNameArgs(filename);
            }
            return tnargs;
        }

        /// <summary>
        /// 根据文件名获取站点ip余号
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public int GetStorageIPMod(string fileName)
        {
            int row = -1;
            int column = -1;
            TileNameArgs tnargs = GetTileNameArgs(fileName);
            row = int.Parse(tnargs.Row);
            column = int.Parse(tnargs.Col);

            return GetStorageIPMod( row,column);
        }

        /// <summary>
        /// 根据行列号，获取切片的应该入库的VDS序号
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public int GetStorageIPMod( int row,int column)
        {
            if (row<0||column<0)
            {
                return -1;
            }
            int ipmod = ((row + column) % _MaxMod);
            return ipmod;
        }

        /// <summary>
        ///根据文件名获取IP
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GetStorageIP(string fileName)
        {

            int ipmod=GetStorageIPMod(fileName);

            string ip = GetIPbyMod(ipmod.ToString());

            return ip;

        }

        /// <summary>
        ///根据行列获取IP
        public string GetStorageIP(int row, int column)
        {

            int ipmod = GetStorageIPMod(row, column);

            string ip = GetIPbyMod(ipmod.ToString());

            return ip;

        }


        Dictionary<string, string[]> _dicIpMods;
        /// <summary>
        /// 根据ModID获取站点IP,针对单备份
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public string GetIPbyMod(string mod)
        {
            try
            {
                if (_dicIpMods == null)
                {
                    _dicIpMods = new Dictionary<string, string[]>();

                    for (int i = 0; i < _dsIPMod.Tables[0].Rows.Count; i++)
                    {
                        string ip = _dsIPMod.Tables[0].Rows[i][0].ToString();
                        string[] strmods = _dsIPMod.Tables[0].Rows[i][1].ToString().Split(",".ToCharArray());

                        _dicIpMods.Add(ip, strmods);
                    }
                }

                foreach (KeyValuePair<string,string[]> kvp in _dicIpMods)
                {

                    foreach (string strmod in kvp.Value)
                    {
                        if (mod.Trim().ToUpper().Equals(strmod.Trim().ToUpper()))
                        {
                            return kvp.Key;
                        }
                    }
                }
                return "-1"; 
            }
            catch { return "-1"; }
        }
        //    List<string> ipLst = GetIPArrByMod(mod);
        //    TServerSiteManager.UpdateOptimalStorageSiteList();
        //    return "";
        //}

        /// <summary>
        /// 根据ModID获取所有包含该Mod的站点IP
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public List<string> GetIPArrByMod(string mod)
        {
            List<string> ipLst = new List<string>();
            try
            {
                for (int i = 0; i < _dsIPMod.Tables[0].Rows.Count; i++)
                {
                    string[] strmods = _dsIPMod.Tables[0].Rows[i][1].ToString().Split(",".ToCharArray());
                    foreach (string strmod in strmods)
                    {
                        if (mod.Trim().ToUpper().Equals(strmod.Trim().ToUpper()))
                        {
                            ipLst.Add(_dsIPMod.Tables[0].Rows[i][0].ToString());
                        }
                    }
                }
                return ipLst;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据文件名获取文件存储路径
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GetPathByFileName(string fileName, out string storeip,out string ipmod)
        {         //增加了Availability和Cloud字段，需要更新
            string filePath = "-1";
            storeip = "-1";
            ipmod = "-1";
            TileNameArgs tnargs = GetTileNameArgs(fileName);

            filePath = GetPathByTileNameArgs(tnargs, out storeip, out ipmod);

            return filePath;
        }

        public string GetPathByTileNameArgs(TileNameArgs tnargs, out string storeip, out string ipmod)
        {
            string filePath = "-1";
            storeip = "-1";
            ipmod = "-1";
            if (tnargs == null || (!tnargs.Created && !tnargs.IsThumbnail))
            {
                return "-1";
            }
            try
            {
                if (tnargs.IsOldNameStyle)
                {
                    tnargs.Filename = tnargs.GetNewStyleFilename();
                }

                ipmod = GetStorageIPMod(int.Parse(tnargs.Row), int.Parse(tnargs.Col)).ToString();
                storeip = GetIPbyMod(ipmod);
                filePath = tnargs.GetFilePath(storeip, ipmod);
            }
            catch
            {

                return "-1";
            }

            return filePath;
        }



        public string GetPathByFileName(string fileName)
        {      //增加了Availability和Cloud字段，需要更新
            string storeip;
            string ipmod;
            return GetPathByFileName(fileName, out storeip, out ipmod);
        }

        public List<string> GetTilesList(List<string> tileNames)
        {
            List<string> tilesPath = new List<string>();
            foreach (string tilename in tileNames)
            {
                string tilepath = GetPathByFileName(tilename);
                tilesPath.Add(tilepath);
            }
            return tilesPath;
        }

        /// <summary>
        /// 根据文件名获取存储路径 zxw
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="storeip">获取存储站点</param>
        /// <returns></returns>
        public string GetPathByFileName(string fileName,out string storeip)
        {   //增加了Availability和Cloud字段，需要更新
            string ipmod;
            return GetPathByFileName(fileName, out storeip, out ipmod);
        }

        /// <summary>
        /// 根据文件名获取存储路径, zxw 2013/3/7
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="storeips">需要存储该文件的所有站点IP</param>
        /// <returns>返回的是文件存储相对路径</returns>
        public string GetPathByFileName(string fileName,out string[]storeips)
        {
            string ipmod;
            string relatePath = GetRelatePathByFileName(fileName,out ipmod);
            storeips = GetIPArrByMod(ipmod).ToArray();
            return relatePath;
        }

        /// <summary>
        /// 根据文件名获取文件存储的相对路径，即前面没有加IP地址
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GetRelatePathByFileName(string fileName,out string ipmod)
        {
            string filePath = "-1";
            ipmod = "-1";
            TileNameArgs tnargs = GetTileNameArgs(fileName);
            if (!tnargs.Created && !tnargs.IsThumbnail)
            {
                return "-1";
            }
            try
            {
                ipmod = GetStorageIPMod(int.Parse(tnargs.Row), int.Parse(tnargs.Col)).ToString();
                filePath = tnargs.GetRelatePath(ipmod);
            }
            catch
            {

                return "-1";
            }

            return filePath;
        }
        /// <summary>
        /// 输入层级行列号，返回切片码对应的路径
        /// _8_322_1333 8是层级，322是列，1333是行
        /// </summary>
        /// <param name="lv">层级</param>
        /// <param name="col">列</param>
        /// <param name="row">行</param>
        /// <returns>切片码对应的路径，例如“\\192.168.10.102\11\8\322\1333\”,无效路径返回“-1”</returns>
        public string GetDirByLvColRow(string lv, string col, string row)
        {
            try
            {
                string ipmod = GetStorageIPMod(int.Parse(row), int.Parse(col)).ToString();
                string storeip = GetIPbyMod(ipmod);
                string dir = string.Format(@"\\{0}\{1}\{2}\{3}\{4}\{5}\", storeip, StorageBasePath.QRST_DB_Tile, ipmod, lv, col, row);
                return dir;
            }
            catch
            {

                return "-1";
            }     
        }

        public string GetIpByRowCol(string row, string col)
        {
            try
            {
                string ipmod = GetStorageIPMod(int.Parse(row), int.Parse(col)).ToString();
                string storeip = GetIPbyMod(ipmod);
                return storeip;
            }
            catch
            {
                return "-1";
            }     
        }

        /// <summary>
        /// 根据经纬度获取行列号 迁移到QRST_DI_Basis 
        /// 最小行，最小列，最大行，最大列  
        /// </summary>
        /// <param name="LatLon">最小纬度、最小经度、最大纬度、最大经度</param>
        /// <returns></returns>
        public static int[] GetRowAndColum(string[] LatLon, string lv)
        {
            return RowColumnTransform.GetRowAndColum(LatLon,lv);
        }

        /// <summary> 
        /// 最小纬度，最小经度，最大纬度，最大经度 
        /// </summary>
        /// <param name="rowAndColum"></param>
        /// <param name="lv"></param>
        /// <returns></returns>
        public static double[] GetLatAndLong(string[] rowAndColum, string lv)
        {
            double a = RowColumnTransform.getLevelRate(lv);
            return GetLatAndLong(double.Parse(rowAndColum[0]), double.Parse(rowAndColum[1]), a);
        }

        /// <summary> 
        /// 最小纬度，最小经度，最大纬度，最大经度 
        /// </summary>
        /// <param name="rowAndColum"></param>
        /// <param name="a">lvrate 通过getLevelRate(lv)获得</param>
        /// <returns></returns>
        public static double[] GetLatAndLong(double row,double col, double a)
        {
            double[] latAndlong = new double[4];
            latAndlong[0] = row / a - 90;
            latAndlong[1] = col / a - 180;
            latAndlong[2] = (row + 1) / a - 90;
            latAndlong[3] = (col + 1) / a - 180;
            return latAndlong;
        }

        /// <summary>
        /// 获取层级字母  迁移到QRST_DI_Basis 
        /// </summary>
        /// <param name="lv"></param>
        /// <returns></returns>
        public static double getLevelRate(string lv)
        {
            return RowColumnTransform.getLevelRate(lv);

        }


        public static string getLevel(string Rate)
        { 
            return GetStrLvByResolution(Rate);
        }

        public static string GetStrLvByResolution(string res)
        {
            string strlv = "7";
            switch (res)
            {
                case "100公里":
                    strlv = "J";
                    break;
                case "50公里":
                    strlv = "I";
                    break;
                case "25公里":
                    strlv = "H";
                    break;
                case "10公里":
                    strlv = "G";
                    break;
                case "5公里":
                case "5000米":
                    strlv = "F";
                    break;
                case "2.5公里":
                case "2500米":
                    strlv = "E";
                    break;
                case "1公里":
                case "1千米":
                case "1000米":
                    strlv = "D";
                    break;
                case "500米":
                    strlv = "C";
                    break;
                case "250米":
                    strlv = "B";
                    break;
                case "100米":
                    strlv = "A";
                    break;
               case "50米":
                    strlv = "9";
                    break;
                case "25米":
                    strlv = "8";
                    break;
                case "10米":
                    strlv = "7";
                    break;
                case "5米":
                    strlv = "6";
                    break;
                case "2.5米":
                    strlv = "5";
                    break;
                case "1米":
                    strlv = "4";
                    break;
                case "0.5米":
                    strlv = "3";
                    break;
                case "0.25米":
                    strlv = "2";
                    break;
                case "0.1米":
                    strlv = "1";
                    break;
                default:
                    break;
            }
            return strlv;
        }


        public static string GetResolutionByStrLv(string strLv)
        {
            return GetStrResolutionByLevelChar(strLv);
        }

        public static string GetStrResolutionByLevelChar(string res)
        {
            string strRes = "10米";
            switch (res)
            {
                case  "J":
                case "19":
                    strRes ="100公里";
                    break;
                case "I":
                case "18":
                    strRes = "50公里";
                    break;
                case "H":
                case "17":
                    strRes = "25公里";
                    break;
                case "G":
                case "16":
                    strRes = "10公里";
                    break;
                case "F":
                case "15":
                    strRes = "5公里";
                    break;
                case "E":
                case "14":
                    strRes = "2.5公里";
                    break;
                case "D":
                case "13":
                    strRes = "1公里";
                    break;
                case "C":
                case "12":
                    strRes = "500米";
                    break;
                case "B":
                case "11":
                    strRes = "250米";
                    break;
                case "A":
                case "10":
                    strRes = "100米";
                    break;
                case "9":
                    strRes = "50米";
                    break;
                case "8":
                    strRes = "25米";
                    break;
                case "7":
                    strRes = "10米";
                    break;
                case "6":
                    strRes = "5米";
                    break;
                case "5":
                    strRes = "2.5米";
                    break;
                case "4":
                    strRes = "1米";
                    break;
                case "3":
                    strRes = "0.5米";
                    break;
                case "2":
                    strRes = "0.25米";
                    break;
                case "1":
                    strRes = "0.1米";
                    break;
                default:
                    break;
            }
            return strRes;
        }

        
        /// <summary>
        /// 层级获取间隔度数
        /// </summary>
        /// <param name="strLv"></param>
        /// <returns></returns>
        public static string GetDegreeByStrLv(string strLv)
        {
            string deg = "8";
            switch (strLv)
            {
                case "F":
                    deg = "100";
                    break;
                case "E":
                    deg = "50";
                    break;
                case "D":
                    deg = "25";
                    break;
                case "C":
                    deg = "5";
                    break;
                case "B":
                    deg = "2.5";
                    break;
                case "A":
                    deg = "1";
                    break;
                case "9":
                    deg = "0.5";
                    break;
                case "8":
                    deg = "0.25";
                    break;
                case "7":
                    deg = "0.1";
                    break;
                case "6":
                    deg = "0.05";
                    break;
                case "5":
                    deg = "0.025";
                    break;
                case "4":
                    deg = "0.01";
                    break;
                case "3":
                    deg = "0.005";
                    break;
                case "2":
                    deg = "0.0025";
                    break;
                case "1":
                    deg = "0.001";
                    break;
                default:
                    break;
            }
            return deg;
        }

        //将带单位的分辨率字符串转成数字，如1米=1,1千米=1000,5公里=5000
        public static double getRateDoule(string Rate)
        {
            string rate_cut = Rate;
            int r = 1;
            if (Rate.Contains("千米"))
            {
                rate_cut = Rate.TrimEnd("千米".ToCharArray());
                r = 1000;
            }
            else if (Rate.Contains("米"))
            {
                rate_cut = Rate.TrimEnd("米".ToCharArray());
            }
            else if (Rate.Contains("公里"))
            {
                rate_cut = Rate.TrimEnd("公里".ToCharArray());
                r = 1000;
            }
            double out1 = double.Parse(rate_cut.Trim()) * r;
            return out1;
        }

        public static string NewGetResolutionByStrLv(string strLv)
        {
            string res = "7";
            switch (strLv)
            {
                //case "9":
                //    res = "50米";
                //    break;
                case "8":
                    res = "16米";
                    break;
                case "7":
                    res = "8米";
                    break;
                case "6":
                    res = "2米";
                    break;
                case "5":
                    res = "1米";
                    break;
                case "4":
                    res = "1米";
                    break;
                //case "3":
                //    res = "0.5米";
                //    break;
                //case "2":
                //    res = "0.25米";
                //    break;
                default:
                    break;
            }
            return res;
        }
        public static string getClassifyName(string num)
        {
            string ClassifyName = "";
            switch (num)
            { 
                case "编号":
                    ClassifyName = "类型";
                    break;
                case "1":
                    //ClassifyName = "水体";
                    ClassifyName = "大棚";
                    break;
                case "2":
                    //ClassifyName = "裸地";
                    ClassifyName = "农田";
                    break;
                case "3":
                    //ClassifyName = "植被";
                    ClassifyName = "裸地";
                    break;
                case "4":
                    ClassifyName = "建筑地";
                    break;
                case "5":
                    ClassifyName = "其他";
                    break;
                case "总计":
                    ClassifyName = "总计";
                    break;
                default: break;
            }
            return ClassifyName;
        }


        public List<string> GetModArrByIP(string ip)
        {
            //_dsIPMod
            //string sql = "select addressip,modid,ISCENTER from midb.tileserversitesinfo";

            List<string> modLst = new List<string>();

            for (int i = 0; i < _dsIPMod.Tables[0].Rows.Count; i++)
            {
                if (_dsIPMod.Tables[0].Rows[i][0].ToString() == ip)
                {
                    string[] strmods = _dsIPMod.Tables[0].Rows[i][1].ToString().Split(",".ToCharArray());
                    modLst.AddRange(strmods);
                    if (_dsIPMod.Tables[0].Rows[i][2].ToString() == "1")
                    {
                        modLst.Add("Failed");
                    }
                    break;
                }

            }
            return modLst;
        }


        /// <summary>
        /// 1000*1000可视化影像
        /// </summary>
        /// <param name="tilename"></param>
        /// <returns></returns>
        public static string getPreViewFilename(string tilename)
        {

            TileNameArgs tna = TileNameArgs.GetTileNameArgs(tilename);
            if (tna.Created || tna.IsThumbnail)
            {
                if (tna.IsOldNameStyle)
                {
                    string nm = Path.GetFileNameWithoutExtension(tilename);
                    int idx = nm.LastIndexOf('-');
                    nm = (idx != -1) ? nm.Substring(0, idx) : nm;
                    return nm + ".png";
                }
                else
                {
                    string nm = Path.GetFileNameWithoutExtension(tilename);
                    string ext = Path.GetExtension(nm);
                    nm = Path.GetFileNameWithoutExtension(nm);
                    int idx = nm.LastIndexOf('-');
                    nm = (idx != -1) ? nm.Substring(0, idx) : nm;
                    nm = nm + ext + ".png";
                    return nm;
                }
            }
            return "-1";
        }


        /// <summary>
        /// 100*100快视图
        /// </summary>
        /// <param name="tilename"></param>
        /// <returns></returns>
        public static string getMiniViewFilename(string tilename)
        {
            TileNameArgs tna = TileNameArgs.GetTileNameArgs(tilename);
            if (tna.Created || tna.IsThumbnail)
            {
                if (tna.IsOldNameStyle)
                {
                    string nm = Path.GetFileNameWithoutExtension(tilename);
                    int idx = nm.LastIndexOf('-');
                    nm = (idx != -1) ? nm.Substring(0, idx) : nm;
                    return nm + "-mini.jpg";
                }
                else
                {
                    string nm = Path.GetFileNameWithoutExtension(tilename);
                    string ext=Path.GetExtension(nm);
                    nm = Path.GetFileNameWithoutExtension(nm);
                    int idx = nm.LastIndexOf('-');
                    nm = (idx != -1) ? nm.Substring(0, idx) : nm;
                    nm = nm + "-mini" + ext + ".jpg";
                    return nm;
                }
            }
            return "-1";
        }

        /// <summary>
        /// 20*20拇指图
        /// </summary>
        /// <param name="tilename"></param>
        /// <returns></returns>
        public static string getTinyViewFilename(string tilename)
        {
            TileNameArgs tna = TileNameArgs.GetTileNameArgs(tilename);
            if (tna.Created || tna.IsThumbnail)
            {
                if (tna.IsOldNameStyle)
                {
                    string nm = Path.GetFileNameWithoutExtension(tilename);
                    int idx = nm.LastIndexOf('-');
                    nm = (idx != -1) ? nm.Substring(0, idx) : nm;
                    return nm + "-tiny.jpg";
                }
                else
                {
                    string nm = Path.GetFileNameWithoutExtension(tilename);
                    string ext = Path.GetExtension(nm);
                    nm = Path.GetFileNameWithoutExtension(nm);
                    int idx = nm.LastIndexOf('-');
                    nm = (idx != -1) ? nm.Substring(0, idx) : nm;
                    nm = nm + "-tiny" + ext + ".jpg";
                    return nm;
                }
            }
            return "-1";
        }

        //public List<string> GetIPmodCollectionByPosition(List<string> position)
        //{
        //    List<string> IPmodCollection = new List<string>();
        //    for (int i = Convert.ToInt32(position[0]); i <=  Convert.ToInt32(position[2]); i++)
        //    {
        //        for (int j = Convert.ToInt32(position[1]); j <=   Convert.ToInt32(position[3]); j++)
        //        {
        //            int ipmod;
        //            GetIPmod(i, j, out ipmod);
        //            if(!IPmodCollection.Contains(ipmod.ToString()))
        //            {
        //                IPmodCollection.Add(ipmod.ToString());
        //            }
        //        }
        //    }
        //    return IPmodCollection;
 
        //}
        //public void TranferSearchCondition(out List<string> position,
        //    out List<int> datetime,
        //    out List<string> satellite,
        //    out List<string> sensor,
        //    out List<string> datatype)
        //{

        //}
    }
}
