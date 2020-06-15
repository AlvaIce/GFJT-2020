using System;
using System.Collections.Generic;
using System.Windows.Forms;
using QRST_DI_DS_Metadata.MetaDataDefiner.Mdl;
using System.IO;
using QRST_DI_DS_Metadata.MetaDataCls;
using QRST_DI_DS_Basis.DataDownLoad;
using QRST_DI_DS_Basis;
using QRST_DI_Resources;
using QRST_DI_TS_Basis.DirectlyAddress;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksums;
 
namespace QRST_DI_MS_Component.Common
{
    public partial class FrmDownLoad : Form
    {
        //public FrmDownLoad()
        //{
        //    InitializeComponent();
        //}
        private bool fullTileFlag;
        private static DirectlyAddressing _da = new DirectlyAddressing(DirectlyAddressingIPMod.IPModDataSet);
        private metadatacatalognode_Mdl selectedQueryObj;
        private List<string> downLoadLst;
        private string taskPath;

        public FrmDownLoad(metadatacatalognode_Mdl _selectedQueryObj,List<string> _downLoadLst)
        {
            InitializeComponent();
            selectedQueryObj = _selectedQueryObj;
            downLoadLst = _downLoadLst;
            if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_raster") && selectedQueryObj.DATA_CODE.ToUpper().Contains("EVDB"))
            {
                //GF数据
                radio_sourcedata.Enabled = true;
                radio_correcteddata.Enabled = true;
            }
            else
            {
                radio_sourcedata.Enabled = false;
                radio_correcteddata.Enabled = false;

            }

            UsingInMSDesktop = true;
        }
        public FrmDownLoad(metadatacatalognode_Mdl _selectedQueryObj, List<string> _downLoadLst,bool fullFlag)
        {
            InitializeComponent();
            fullTileFlag = fullFlag;
            selectedQueryObj = _selectedQueryObj;
            downLoadLst = _downLoadLst;
            if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_raster") && selectedQueryObj.DATA_CODE.ToUpper().Contains("EVDB"))
            {
                //GF数据
                radio_sourcedata.Enabled = true;
                radio_correcteddata.Enabled = true;
            }
            else
            {
                radio_sourcedata.Enabled = false;
                radio_correcteddata.Enabled = false;

            }

            UsingInMSDesktop = true;

        }

        public FrmDownLoad(metadatacatalognode_Mdl _selectedQueryObj, List<string> _downLoadLst, string _taskPath)
        {
            InitializeComponent();
            selectedQueryObj = _selectedQueryObj;
            downLoadLst = _downLoadLst;
            taskPath = _taskPath;
            if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_raster") && selectedQueryObj.DATA_CODE.ToUpper().Contains("EVDB"))
            {
                //GF数据
                radio_sourcedata.Enabled = true;
                radio_correcteddata.Enabled = true;
            }
            else
            {
                radio_sourcedata.Enabled = false;
                radio_correcteddata.Enabled = false;

            }

            UsingInMSDesktop = true;
        }
        public bool UsingInMSDesktop { get; private set; }

        public FrmDownLoad( List<string> _downLoadLst)
        {
            InitializeComponent();
            //selectedQueryObj = _selectedQueryObj;
            downLoadLst = _downLoadLst;
            radio_sourcedata.Enabled = false;
            radio_correcteddata.Enabled = false;
            UsingInMSDesktop = false;
        }

        bool Check()
        {
            if (!Directory.Exists(textBoxDataPath.Text))
            {
                MessageBox.Show("选中的目标路径不存在！");
                return false;
            }
            else
            {
                return true;
            }
        }

        private void btnStartDownLoad_Click(object sender, EventArgs e)
        {
            if (!Check())
            {
                return;
            }
            if (UsingInMSDesktop && fullTileFlag == false)  //非全覆盖的数据h和点击下载选中数据的全覆盖数据
            {
                GetFilePathListByQueryObject();
            }
            else if (UsingInMSDesktop && fullTileFlag == true) //单时相全覆盖的数据且点击下载gff数据的数据
            {
                GetFilePathFullPath();
            }
            else
            {
                List<string> tileFiles = new List<string>();
                List<string> correctedData = new List<string>();
                List<string> normalData = new List<string>();
                for (int i = 0; i < downLoadLst.Count; i++)
                {
                    string filecode = downLoadLst[i];
                    enumdatatype datatype = getdatatypebycode(filecode);
                    switch (datatype)
                    {
                        case enumdatatype.tile:
                            tileFiles.Add(filecode);
                            break;
                        case enumdatatype.corrected:
                            correctedData.Add(filecode);
                            break;
                        case enumdatatype.normal:
                            normalData.Add(filecode);
                            break;
                        default:
                            break;
                    }
                }

                if (tileFiles.Count > 0)  //下载切片
                {
                    try
                    {
                        List<string> srcPath = _da.GetTilesList(tileFiles);
                        List<string> tilesPath = new List<string>();
                        tilesPath.AddRange(srcPath);
                        for (int i = 0; i < srcPath.Count; i++)
                        {
                            string destPath = string.Format(@"{0}\{1}", textBoxDataPath.Text, Path.GetFileName(srcPath[i]));
                            if (File.Exists(srcPath[i]))
                            {
                                FileInfo fi = new FileInfo(srcPath[i]);
                                if (fi.Name.ToUpper().EndsWith(".JPG"))
                                {
                                    string jgwSourcePath = Path.Combine(fi.DirectoryName, Path.GetFileNameWithoutExtension(srcPath[i]) + ".jgw");
                                    if (File.Exists(jgwSourcePath))
                                    {
                                        tilesPath.Add(jgwSourcePath);
                                    }
                                }
                                if (fi.Name.ToUpper().EndsWith(".PNG"))
                                {
                                    string jgwSourcePath = Path.Combine(fi.DirectoryName, Path.GetFileNameWithoutExtension(srcPath[i]) + ".pgw");
                                    if (File.Exists(jgwSourcePath))
                                    {
                                        tilesPath.Add(jgwSourcePath);
                                    }
                                }
                            }

                        }
                        List<DownLoadDataObj> downloadDataLst = new List<DownLoadDataObj>();
                        downloadDataLst.Add(new DownLoadDataObj(tilesPath, textBoxDataPath.Text));
                        FrmDownLoadLst.GetInstance().Show();
                        FrmDownLoadLst.GetInstance().AddDownLoadTask(downloadDataLst);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("无法获取切片地址查询服务！");
                    }
                }

                if (normalData.Count > 0)  //下载原始数据的修改，使得一边取得路径一边下载数据，@张飞龙，@20150126
                {
                    string destDir = textBoxDataPath.Text;
                    FrmDownLoadLst.GetInstance().Show();
                    FrmDownLoadLst.GetInstance().AddDownLoadTaskThreads(downLoadLst, destDir);
                }

                if (correctedData.Count > 0)  //下载校正数据,目前只支持GF1号数据  
                {
                    MetaDataGF1 metaData = new MetaDataGF1();
                    for (int i = 0; i < downLoadLst.Count; i++)
                    {
                        metaData.GetModel(downLoadLst[i], Constant.IdbServerUtilities.GetSubDBUtil("EVDB"));
                        if (metaData.CorDataFlag != "-1" && metaData.CorDataFlag != "")
                        {
                            string srcPath = metaData.GetCorrectedDataPath();

                            string[] files = Directory.GetFiles(srcPath);
                            DirectoryInfo info = new DirectoryInfo(srcPath);
                            string destDir = string.Format(@"{0}\{1}", textBoxDataPath.Text, info.Name);
                            if (!Directory.Exists(destDir))
                            {
                                Directory.CreateDirectory(destDir);
                            }
                            for (int k = 0; k < files.Length; k++)
                            {
                                string destPath = string.Format(@"{0}\{1}", destDir, Path.GetFileName(files[k]));
                                File.Copy(files[k], destPath, true);
                            }
                        }
                        else
                        {
                            MessageBox.Show(metaData.Name + "数据未纠正!");
                        }
                    }
                }
            }
            
            this.Close();
        }
        
        enum enumdatatype {tile=1,corrected=2,normal=3}
        static Dictionary<string, string> table;
        private void GetFilePathListByQrstCode()
        {
            List<string> tileFiles = new List<string>();
            List<string> correctedData = new List<string>();
            List<string> normalData = new List<string>();
            for (int i = 0; i < downLoadLst.Count; i++)
            {
                string filecode = downLoadLst[i];
                enumdatatype datatype = getdatatypebycode(filecode);
                switch (datatype)
                {
                    case enumdatatype.tile:
                        tileFiles.Add(filecode);
                        break;
                    case enumdatatype.corrected:
                        correctedData.Add(filecode);
                        break;
                    case enumdatatype.normal:
                        normalData.Add(filecode);
                        break;
                    default:
                        break;
                }
            }

            if (tileFiles.Count > 0)  //下载切片
            {
                try
                {
                    List<string> srcPath = _da.GetTilesList(tileFiles);
                    List<string> tilesPath = new List<string>();
                    tilesPath.AddRange(srcPath);
                    for (int i = 0; i < srcPath.Count; i++)
                    {
                        string destPath = string.Format(@"{0}\{1}", textBoxDataPath.Text, Path.GetFileName(srcPath[i]));
                        if (File.Exists(srcPath[i]))
                        {
                            FileInfo fi = new FileInfo(srcPath[i]);
                            if (fi.Name.ToUpper().EndsWith(".JPG"))
                            {
                                string jgwSourcePath = Path.Combine(fi.DirectoryName, Path.GetFileNameWithoutExtension(srcPath[i]) + ".jgw");
                                if (File.Exists(jgwSourcePath))
                                {
                                    tilesPath.Add(jgwSourcePath);
                                }
                            }
                            if (fi.Name.ToUpper().EndsWith(".PNG"))
                            {
                                string jgwSourcePath = Path.Combine(fi.DirectoryName, Path.GetFileNameWithoutExtension(srcPath[i]) + ".pgw");
                                if (File.Exists(jgwSourcePath))
                                {
                                    tilesPath.Add(jgwSourcePath);
                                }
                            }
                        }

                    }
                    List<DownLoadDataObj> downloadDataLst = new List<DownLoadDataObj>();
                    downloadDataLst.Add(new DownLoadDataObj(tilesPath, textBoxDataPath.Text));
                    FrmDownLoadLst.GetInstance().Show();
                    FrmDownLoadLst.GetInstance().AddDownLoadTask(downloadDataLst);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("无法获取切片地址查询服务！");
                }
            }

            if (normalData.Count > 0)  //下载原始数据的修改，使得一边取得路径一边下载数据，@张飞龙，@20150126
            {
                string destDir = textBoxDataPath.Text;
                FrmDownLoadLst.GetInstance().Show();
                FrmDownLoadLst.GetInstance().AddDownLoadTaskThreads(downLoadLst, destDir);
            }

            if (correctedData.Count > 0)  //下载校正数据,目前只支持GF1号数据  
            {
                MetaDataGF1 metaData = new MetaDataGF1();
                for (int i = 0; i < downLoadLst.Count; i++)
                {
                    metaData.GetModel(downLoadLst[i], Constant.IdbServerUtilities.GetSubDBUtil("EVDB"));
                    if (metaData.CorDataFlag != "-1" && metaData.CorDataFlag != "")
                    {
                        string srcPath = metaData.GetCorrectedDataPath();

                        string[] files = Directory.GetFiles(srcPath);
                        DirectoryInfo info = new DirectoryInfo(srcPath);
                        string destDir = string.Format(@"{0}\{1}", textBoxDataPath.Text, info.Name);
                        if (!Directory.Exists(destDir))
                        {
                            Directory.CreateDirectory(destDir);
                        }
                        for (int k = 0; k < files.Length; k++)
                        {
                            string destPath = string.Format(@"{0}\{1}", destDir, Path.GetFileName(files[k]));
                            File.Copy(files[k], destPath, true);
                        }
                    }
                    else
                    {
                        MessageBox.Show(metaData.Name + "数据未纠正!");
                    }
                }
            }
        }


        private enumdatatype getdatatypebycode(string filecode)
        {
            if (!filecode.StartsWith(QRST_DI_Resources.Constant.INDUSTRYCODE))
            {
                return enumdatatype.tile;
            }
            else if (radio_sourcedata.Checked) //下载原始数据
            {
                return enumdatatype.normal;
            }
            else
            {
                return enumdatatype.corrected;
            }
        }

        private void GetFilePathListByQueryObject()
        {
            if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_tile"))  //下载切片
            {
                try
                {
                    DirectlyAddressing da = new DirectlyAddressing(DirectlyAddressingIPMod.IPModDataSet);
                    List<string> srcPath = da.GetTilesList(downLoadLst);

                    List<string> tilesPath = new List<string>();
                    tilesPath.AddRange(srcPath);
                    for (int i = 0; i < srcPath.Count; i++)
                    {
                        string destPath = string.Format(@"{0}\{1}", textBoxDataPath.Text, Path.GetFileName(srcPath[i]));
                        if (File.Exists(srcPath[i]))
                        {
                            FileInfo fi = new FileInfo(srcPath[i]);
                            if (fi.Name.ToUpper().EndsWith(".JPG"))
                            {
                                string jgwSourcePath = Path.Combine(fi.DirectoryName, Path.GetFileNameWithoutExtension(srcPath[i]) + ".jgw");
                                if (File.Exists(jgwSourcePath))
                                {
                                    tilesPath.Add(jgwSourcePath);
                                }
                            }
                            if (fi.Name.ToUpper().EndsWith(".PNG"))
                            {
                                string jgwSourcePath = Path.Combine(fi.DirectoryName, Path.GetFileNameWithoutExtension(srcPath[i]) + ".pgw");
                                if (File.Exists(jgwSourcePath))
                                {
                                    tilesPath.Add(jgwSourcePath);
                                }
                            }
                        }

                    }
                    List<DownLoadDataObj> downloadDataLst = new List<DownLoadDataObj>();
                    downloadDataLst.Add(new DownLoadDataObj(tilesPath, textBoxDataPath.Text));
                    FrmDownLoadLst.GetInstance().Show();
                    FrmDownLoadLst.GetInstance().AddDownLoadTask(downloadDataLst);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("无法获取切片地址查询服务！");
                }


            }
            else  //下载非切片数据
            {
                //下载原始数据的修改，使得一边取得路径一边下载数据，@张飞龙，@20150126
                if (radio_sourcedata.Checked) //下载原始数据
                {
                    string destDir = textBoxDataPath.Text;
                    bool isRCdata = false;
                    switch (selectedQueryObj.NAME)
                    {
                        case "土壤":
                        case "南方植被":
                        case "北方植被":
                        case "城市目标":
                        case "地表大气":
                        case "水体":
                        case "岩矿":
                            isRCdata = true;
                            break;
                    }
                    if (isRCdata)
                    {
                        List<string> newdownLoadLst = new List<string>();
                        foreach (string dlf in downLoadLst)
                        {
                            MetaDataRcBopu mdrb = new MetaDataRcBopu(selectedQueryObj.NAME);
                            mdrb.GetModel(dlf, null);
                            string rootPath = QRST_DI_DS_Metadata.Paths.StoragePath.StoreBasePath;      //数据的根路径
                            string relatePath = mdrb.GetRelateDataPath();
                            newdownLoadLst.Add(QRST_DI_DS_Metadata.Paths.StoragePath.GetExistPath(rootPath, relatePath));
                        }

                        FrmDownLoadLst.GetInstance().Show();
                        FrmDownLoadLst.GetInstance().AddDownLoadTaskThreads(newdownLoadLst, destDir);

                    }
                    else
                    {
                        FrmDownLoadLst.GetInstance().Show();
                        FrmDownLoadLst.GetInstance().AddDownLoadTaskThreads(downLoadLst, destDir);
                    }
                }
                else    //下载校正数据,目前只支持GF1号数据  
                {
                    MetaDataGF1 metaData = new MetaDataGF1();
                    for (int i = 0; i < downLoadLst.Count; i++)
                    {
                        metaData.GetModel(downLoadLst[i], Constant.IdbServerUtilities.GetSubDBUtil("EVDB"));
                        if (metaData.CorDataFlag != "-1" && metaData.CorDataFlag != "")
                        {
                            string srcPath = metaData.GetCorrectedDataPath();

                            string[] files = Directory.GetFiles(srcPath);
                            DirectoryInfo info = new DirectoryInfo(srcPath);
                            string destDir = string.Format(@"{0}\{1}", textBoxDataPath.Text, info.Name);
                            if (!Directory.Exists(destDir))
                            {
                                Directory.CreateDirectory(destDir);
                            }
                            for (int k = 0; k < files.Length; k++)
                            {
                                string destPath = string.Format(@"{0}\{1}", destDir, Path.GetFileName(files[k]));
                                File.Copy(files[k], destPath, true);
                            }
                        }
                        else
                        {
                            MessageBox.Show(metaData.Name + "数据未纠正!");
                        }
                    }
                }
            }
        }
        /// <summary>
        /// xmh 20170518
        /// 用于瓦片的单时相全覆盖下载时，下载含有1.2.3.4.Preview类型的gff的压缩包及非瓦片数据的单时相全覆盖
        /// </summary>
        private void GetFilePathFullPath()
        {
            string tempzipPath = null;
            List<string> tilesPath = new List<string>();
            if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_tile")&& fullTileFlag == true)  //下载单时相的切片
            {
               DirectlyAddressing da = new DirectlyAddressing(DirectlyAddressingIPMod.IPModDataSet);
               List<string> srcPath = da.GetTilesList(downLoadLst);
               //循环遍历每个srcPath下的切片数据，把1.2.3.4.Preview类型的切片放入.gff的压缩包中
               for (int i = 0; i < srcPath.Count; i++)
                {
                        //string destPathLocal = string.Format(@"{0}\", textBoxDataPath.Text);
                        tempzipPath = getgff(srcPath[i]);
                        tilesPath.Add(tempzipPath);
                        //string destPath = string.Format(@"{0}\{1}", textBoxDataPath.Text, Path.GetFileNameWithoutExtension(srcPath[i]));
                        //if (File.Exists(srcPath[i]))
                        //{
                        //    FileInfo fi = new FileInfo(srcPath[i]);
                        //    if (fi.Name.ToUpper().EndsWith(".JPG"))
                        //    {
                        //        string jgwSourcePath = Path.Combine(fi.DirectoryName, Path.GetFileNameWithoutExtension(srcPath[i]) + ".jgw");
                        //        if (File.Exists(jgwSourcePath))
                        //        {
                        //            tilesPath.Add(jgwSourcePath);
                        //        }
                        //    }
                        //    if (fi.Name.ToUpper().EndsWith(".PNG"))
                        //    {
                        //        string jgwSourcePath = Path.Combine(fi.DirectoryName, Path.GetFileNameWithoutExtension(srcPath[i]) + ".pgw");
                        //        if (File.Exists(jgwSourcePath))
                        //        {
                        //            tilesPath.Add(jgwSourcePath);
                        //        }
                        //    }
                        //}
                }
                   
                    List<DownLoadDataObj> downloadDataLst = new List<DownLoadDataObj>();
                    downloadDataLst.Add(new DownLoadDataObj(tilesPath, textBoxDataPath.Text));
                    FrmDownLoadLst.GetInstance().Show();
                    FrmDownLoadLst.GetInstance().AddDownLoadTask(downloadDataLst);
                    
            }
            else  //下载非切片数据
            {
                //下载原始数据的修改，使得一边取得路径一边下载数据，@张飞龙，@20150126
                if (radio_sourcedata.Checked) //下载原始数据
                {
                    string destDir = textBoxDataPath.Text;
                    bool isRCdata = false;
                    switch (selectedQueryObj.NAME)
                    {
                        case "土壤":
                        case "南方植被":
                        case "北方植被":
                        case "城市目标":
                        case "地表大气":
                        case "水体":
                        case "岩矿":
                            isRCdata = true;
                            break;
                    }
                    if (isRCdata)
                    {
                        List<string> newdownLoadLst = new List<string>();
                        foreach (string dlf in downLoadLst)
                        {
                            MetaDataRcBopu mdrb = new MetaDataRcBopu(selectedQueryObj.NAME);
                            mdrb.GetModel(dlf, null);
                            string rootPath = QRST_DI_DS_Metadata.Paths.StoragePath.StoreBasePath;      //数据的根路径
                            string relatePath = mdrb.GetRelateDataPath();
                            newdownLoadLst.Add(QRST_DI_DS_Metadata.Paths.StoragePath.GetExistPath(rootPath, relatePath));
                        }

                        FrmDownLoadLst.GetInstance().Show();
                        FrmDownLoadLst.GetInstance().AddDownLoadTaskThreads(newdownLoadLst, destDir);

                    }
                    else
                    {
                        FrmDownLoadLst.GetInstance().Show();
                        FrmDownLoadLst.GetInstance().AddDownLoadTaskThreads(downLoadLst, destDir);
                    }
                }
                else    //下载校正数据,目前只支持GF1号数据  
                {
                    MetaDataGF1 metaData = new MetaDataGF1();
                    for (int i = 0; i < downLoadLst.Count; i++)
                    {
                        metaData.GetModel(downLoadLst[i], Constant.IdbServerUtilities.GetSubDBUtil("EVDB"));
                        if (metaData.CorDataFlag != "-1" && metaData.CorDataFlag != "")
                        {
                            string srcPath = metaData.GetCorrectedDataPath();

                            string[] files = Directory.GetFiles(srcPath);
                            DirectoryInfo info = new DirectoryInfo(srcPath);
                            string destDir = string.Format(@"{0}\{1}", textBoxDataPath.Text, info.Name);
                            if (!Directory.Exists(destDir))
                            {
                                Directory.CreateDirectory(destDir);
                            }
                            for (int k = 0; k < files.Length; k++)
                            {
                                string destPath = string.Format(@"{0}\{1}", destDir, Path.GetFileName(files[k]));
                                File.Copy(files[k], destPath, true);
                            }
                        }
                        else
                        {
                            MessageBox.Show(metaData.Name + "数据未纠正!");
                        }
                    }
                }
            }

        }
        /// <summary>
        /// xmh 
        /// 根据瓦片名，获取其1.2.3.4.preview类型的是所有数据
        /// </summary>
        /// <param name="filePath"></param>
        public string  getgff(string filePath) 
        {
            DirectlyAddressing da = new DirectlyAddressing(DirectlyAddressingIPMod.IPModDataSet);
            string tile = Path.GetFileNameWithoutExtension(filePath);  //"CP3_GRC_2014000024_L2A201510201000031_2700_7_1310_2966.c"	
            TileNameArgs tileArgs = da.GetTileNameArgs(tile + ".png");
            List<string> tilefiles = new List<string>();
            switch (tileArgs.Type)
            {
                case TileNameArgs.TileType.ProdTile:
                    ProdTileNameArgs ptileArgs = tileArgs as ProdTileNameArgs;
                    //LST_2014022024_L1A0000169144_6400_8_474_1176.p.tif 
                    tilefiles.AddRange(new string[] { da.GetPathByFileName(tile + ".tif") });
                    break;
                case TileNameArgs.TileType.CorrectedTile:

                    CorrectedTileNameArgs ctileArgs = tileArgs as CorrectedTileNameArgs;
                    tile = Path.GetFileNameWithoutExtension(tile);
                    if (tile.ToUpper().Contains("_MOC"))  //"CP3_GRC_2014000024_L2A201510201000031_2700_7_1310_2966"

                    {
                        //GF1_MOC2_2015011024_L1A0000580463_0C00_8_518_1179-Alpha.c.tif
                        tilefiles.AddRange(new string[]{
                                            da.GetPathByFileName(tile + ".c.png"),
                                            da.GetPathByFileName(tile + ".c.pgw"),
                                            da.GetPathByFileName(string.Format("{0}-1.c.tif", tile)),
                                            da.GetPathByFileName(string.Format("{0}-2.c.tif", tile)),
                                            da.GetPathByFileName(string.Format("{0}-3.c.tif", tile)),
                                            da.GetPathByFileName(string.Format("{0}-Alpha.c.tif", tile))});
                    }
                    else if (tile.ToUpper().Contains("_GRC_"))
                    {
                        //GF1_WFV3_2016082724_L1A0001788435_0C00_7_1300_2957.c
                        //CP3_GRC_20150110_L1A0000580463_8_518_1179-Alpha.tif old
                        tilefiles.AddRange(new string[]{
                                            da.GetPathByFileName(tile + ".c.png"),
                                            da.GetPathByFileName(tile + ".c.pgw"),
                                            da.GetPathByFileName(string.Format("{0}-1.c.tif", tile)),
                                            da.GetPathByFileName(string.Format("{0}-2.c.tif", tile)),
                                            da.GetPathByFileName(string.Format("{0}-3.c.tif", tile)),
                                            da.GetPathByFileName(string.Format("{0}-4.c.tif", tile)),
                                            da.GetPathByFileName(string.Format("{0}-Alpha.c.tif", tile))});
                    }
                    else
                    {
                        tilefiles.AddRange(new string[]{
                                            da.GetPathByFileName(tile + ".c.png"),
                                            da.GetPathByFileName(tile + ".c.pgw"),
                                            da.GetPathByFileName(string.Format("{0}-1.c.tif", tile)),
                                            da.GetPathByFileName(string.Format("{0}-2.c.tif", tile)),
                                            da.GetPathByFileName(string.Format("{0}-3.c.tif", tile)),
                                            da.GetPathByFileName(string.Format("{0}-4.c.tif", tile))
                                            //,da.GetPathByFileName(string.Format("{0}-Azimuth.c.tif", tile))
                                            //,da.GetPathByFileName(string.Format("{0}-Zenith.c.tif", tile))
                                            });
                    }
                    break;
                default:
                    throw new Exception("未支持该类型数据！");

            }

            string destDir = filePath.Substring(0, filePath.LastIndexOf("\\"));  //192.168.10.63\\QRST_DB_Tile\\2\\7\\1139\\2938\\PMS2\\20160516"
            string strZipPath = Path.Combine(destDir, tile + ".gff");//zip  //\\192.168.10.63\\QRST_DB_Tile\\2\\7\\1139\\2938\\PMS2\\20160516\\GF1_PMS2_2016051624_L1A0001585776_2E00_7_1139_2938.gff"	

            //string destDir = string.Format(@"{0}\", textBoxDataPath.Text);
            //string strZipPath = Path.Combine(destDir, tile + ".gff");

            ZipOutputStream s = new ZipOutputStream(File.Create(strZipPath));
            foreach (string tilepath in tilefiles)
            {
               zip(tilepath, s, strZipPath);
            }
            s.Close();

            //FileInfo gfftmpfile = new FileInfo(strZipPath);
            //gfftmpfile.MoveTo(destPath);
           return strZipPath;
            
        }
        /// <summary>
        /// 把瓦片文件打成gff包
        /// </summary>
        /// <param name="strFile"></param>
        /// <param name="s"></param>
        /// <param name="staticFile"></param>
        private void zip(string strFile, ZipOutputStream s, string staticFile)
        {
            if (File.Exists(strFile))
            {
                Crc32 crc = new Crc32();
                //string[] filenames = Directory.GetFileSystemEntries(strFile);
                FileStream fs = File.OpenRead(strFile);

                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                string tempfile = strFile.Substring(strFile.LastIndexOf("\\") + 1);
                ZipEntry entry = new ZipEntry(tempfile);

                entry.DateTime = DateTime.Now;
                entry.Size = fs.Length;
                fs.Close();
                crc.Reset();
                crc.Update(buffer);
                entry.Crc = crc.Value;
                s.PutNextEntry(entry);
                s.Write(buffer, 0, buffer.Length);
            }
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnChooseBtn_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fb = new FolderBrowserDialog();
          //  fb.SelectedPath = @"D:\xiazai1";
            if(fb.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBoxDataPath.Text = fb.SelectedPath;
                long availableSpace = DirectoryUtil.GetDirectoryDriverSize(textBoxDataPath.Text);
                lblAvailableSpace.Text = string.Format("可用空间大小：{0} GB", availableSpace/(1024*1024*1024));
            }
        }
    }
}
//this.radioGroup1.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
//new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "原始数据"),
//new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "校正数据")});