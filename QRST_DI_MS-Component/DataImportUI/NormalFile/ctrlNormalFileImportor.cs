using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.IO;
using DotSpatial.Data;
using ICSharpCode.SharpZipLib.Zip;
using log4net;
using QRST_DI_DS_Basis;
using QRST_DI_MS_Basis.UserRole;
using QRST_DI_DS_Metadata.MetaDataDefiner.Dal;
using QRST_DI_DS_Metadata.MetaDataCls;
using QRST_DI_DS_Metadata.Paths;
using QRST_DI_MS_Component.Common;
using QRST_DI_SS_DBInterfaces.IDBEngine;
 
namespace QRST_DI_MS_Component_DataImportorUI.NormalFile
{
    public partial class ctrlNormalFileImportor : UserControl
    {
        public struct DataType
        {
            public string tablecapital;
            public string tablecode;
            public string groupcode;
            public string tablename;

            public override string ToString()
            {
                return tablecapital;
            }
        }

       log4net.ILog log = LogManager.GetLogger(typeof(ctrlNormalFileImportor));
        public static userInfo _currentUser;            //当前用户
        static IDbBaseUtilities _dbUtil = null;


        public ctrlNormalFileImportor()
        {
            InitializeComponent();
            ctrlVirtualDirSetting1.chk_DirImportMode.CheckedChanged += new EventHandler(chk_DirImportMode_CheckedChanged);
            

        }

        void chk_DirImportMode_CheckedChanged(object sender, EventArgs e)
        {
            chk_KeepRelateDir.Checked = ctrlVirtualDirSetting1.chk_DirImportMode.Checked;
        }

        /// <summary>
        /// 默认是isdb的prod_normalfiles
        /// </summary>
        /// <param name="dbutil">isdb</param>
        /// <param name="currentUser"></param>
        public void Create(IDbBaseUtilities dbutil, userInfo currentUser)
        {
            
            _dbUtil = dbutil;
            _currentUser = currentUser;
            ctrlVirtualDirSetting1.Create(currentUser.NAME, currentUser.PASSWORD);


            //初始化一般文件类型列表
            cmbFileCatalog.Items.Clear();
           
            string sql = "select * from metadatacatalognode where GROUP_TYPE = 'System_NormalFile';";
            System.Data.DataSet ds = _dbUtil.GetDataSet(sql);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                tablecode_Dal tablecode_dal = new tablecode_Dal(_dbUtil);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    try
                    {
                        DataType dtype = new DataType();
                        dtype.tablecapital = dr["NAME"].ToString();
                        dtype.tablecode = dr["DATA_CODE"].ToString();
                        dtype.groupcode = dr["GROUP_CODE"].ToString();
                        dtype.tablename = tablecode_dal.GetTableName(dtype.tablecode);
                        if (dtype.tablecapital.Trim() == "一般文件")
                        {
                            cmbFileCatalog.Items.Insert(0, dtype);
                        }
                        else
                        {
                            cmbFileCatalog.Items.Add(dtype);
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            try
            {
                cmbFileCatalog.SelectedIndex = 0;
                cmbFileCatalog.Refresh();
            }
            catch (Exception)
            {
                
              
            }

           
        }


        public void Create(IDbBaseUtilities dbutil, userInfo currentUser, List<DataType> datatypeItems)
        {
            _dbUtil = dbutil;
            _currentUser = currentUser;
            ctrlVirtualDirSetting1.Create(currentUser.NAME, currentUser.PASSWORD);


            //初始化一般文件类型列表
            cmbFileCatalog.Items.Clear();
            if (datatypeItems != null && datatypeItems.Count > 0)
            {
                foreach (DataType dt in datatypeItems)
                {
                    cmbFileCatalog.Items.Add(dt);
                }
                cmbFileCatalog.SelectedIndex = 0;
            }

            cmbFileCatalog.Refresh();
            /*
            string sql = "select * from metadatacatalognode where GROUP_TYPE = 'System_NormalFile';";
            System.Data.DataSet ds = isdbUtil.GetDataSet(sql);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                tablecode_Dal tablecode_dal = new tablecode_Dal(isdbUtil);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    try
                    {
                        DataType dtype = new DataType();
                        dtype.tablecapital = dr["NAME"].ToString();
                        dtype.tablecode = dr["DATA_CODE"].ToString();
                        dtype.groupcode = dr["GROUP_CODE"].ToString();
                        dtype.tablename = tablecode_dal.GetTableName(dtype.tablecode);
                        if (dtype.tablecapital.Trim() == "一般文件")
                        {
                            cmbFileCatalog.Items.Insert(0, dtype);
                        }
                        else
                        {
                            cmbFileCatalog.Items.Add(dtype);
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }

            cmbFileCatalog.SelectedIndex = 0;
            cmbFileCatalog.Refresh();
            */
        }

        private DirectoryInfo sourceDi;
        private void buttonOpenFolder_Click(object sender, EventArgs e)
        {
            if (chk_KeepRelateDir.Checked)
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                DialogResult dr = fbd.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    sourceDi = new DirectoryInfo(fbd.SelectedPath);
                    DataListAddDir(sourceDi);
                }
            }
            else
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Multiselect = true;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string[] selectedFiles = ofd.FileNames;
                    foreach (string fi in selectedFiles)
                    {
                        cbImportDataLst.Items.Add(fi, true);
                    }
                }
            }
        }

        private void DataListAddDir(DirectoryInfo sourceDi)
        {
            FileInfo[] fis = sourceDi.GetFiles();
            foreach (FileInfo fi in fis)
            {
                cbImportDataLst.Items.Add(fi.FullName, true);
            }
            DirectoryInfo[] dis = sourceDi.GetDirectories();

            foreach (DirectoryInfo di in dis)
            {
                DataListAddDir(di);
            }
        }


        private string tablename = "";
        private void btn_ImportData_Click(object sender, EventArgs e)
        {
            string tablename = "prod_NormalFiles";
            try
            {
                tablename = ((DataType)cmbFileCatalog.SelectedItem).tablename;
            }
            catch (Exception ex)
            {
            }

            WaitForm wf = new WaitForm();
            wf.datask += ImportData;
            wf.beginShowDialog(new object[] { tablename });
            MessageBox.Show("入库完成！");
            //
            cbImportDataLst.Items.Clear();
        
        }
        

        private StoragePath storePath = null;

        private void ImportData(object[] objs)
        {
            foreach (object obj in cbImportDataLst.CheckedItems)
            {
                try
                {
                    string filename = obj.ToString();//C:\Users\JS\Desktop\入库文件\新建文本文档 (2).zip
                    log.Info(string.Format("###########开始导入数据{0}###############", filename.ToString()));
                    tablename = objs[0].ToString();
                    MetaDataNormalFile mdnf = new MetaDataNormalFile(tablename);
                    mdnf.ReadAttributes(filename);
                    SetCustomizedMetaData(mdnf);
                    mdnf.ImportData(_dbUtil);
                    mdnf.GetModel(mdnf.QRST_CODE, _dbUtil);

                    if (storePath == null)
                    {
                        //tableCode==ISDB-39
                        string tableCode = StoragePath.GetTableCodeByQrstCode(mdnf.QRST_CODE);//0001-ISDB-39-32
                        storePath = new StoragePath(tableCode);
                    }
                    //\\192.168.10.202\zhsjk\信息服务数据库\一般文件\新建文本文档 (2)#0001-ISDB-39-32.gnf
                    string destpath = storePath.GetDataOldPathForTools(mdnf);
                    string sharedir = StoragePath.StoreBasePath;

                    string dir = Path.GetDirectoryName(destpath);
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    File.Copy(filename, destpath, true);

                    //判断是否是压缩包 如果是，判断里面有木有thumbnail.jpg,如果有，放到缓存里。
                    //zsm 20170410
                    if (filename.EndsWith(".zip"))
                    {
                        bool isexit=isExitJPG(filename);
                        if (isexit)
                        {
                            //解压压缩包路径为缓存路径 下面注释1的为任意路径string.Format(@"Thumb\文档成果\{0}", QRST_CODE);    
                            string sourcedir = string.Format(@"{0}\{1}\{2}", Application.StartupPath, @"Cache\Thumbnail", mdnf.QRST_CODE);
                            if (Directory.Exists(sourcedir))
                            {
                                DataPacking.unZipFile(filename, sourcedir);
                                string thumbnailjpgpath = string.Format(@"{0}\{1}", sourcedir, "thumbnail.jpg");
                                if (File.Exists(thumbnailjpgpath))
                                {
                                    string chushu = GetChushuByCode(mdnf.QRST_CODE);
                                    string shangnumber = Convert.ToString((Convert.ToInt32(chushu) / 1000));
                                    string sharepath = string.Format(@"{0}\Thumb",sharedir);
                                    string newfilepath = Path.Combine(sharepath, shangnumber);
                                    if (newfilepath != "" && Directory.Exists(newfilepath))
                                    {
                                        string destdir = string.Format(@"{0}\{1}.jpg", newfilepath, mdnf.QRST_CODE);
                                        File.Copy(thumbnailjpgpath, destdir);
                                    }
                                    else
                                    {
                                        Directory.CreateDirectory(newfilepath);
                                        string destdir = string.Format(@"{0}\{1}.jpg", newfilepath, mdnf.QRST_CODE);
                                        File.Copy(thumbnailjpgpath, destdir);
                                    }
                                }
                                else
                                { }

                            }
                            else
                            {
                                //Directory.CreateDirectory(sourcedir);
                                //DataPacking.unZipFile(filename, sourcedir);
                                //string thumbnailjpgpath = string.Format(@"{0}\{1}", sourcedir, "thumbnail.jpg");
                                //if (File.Exists(thumbnailjpgpath))
                                //{
                                //    string destdir = string.Format(@"{0}\{1}.jpg", sourcedir, mdnf.QRST_CODE);
                                //    File.Move(thumbnailjpgpath, destdir);//剪切比较好
                                //}
                                //else
                                //{ }
                                Directory.CreateDirectory(sourcedir);
                                DataPacking.unZipFile(filename, sourcedir);
                                string thumbnailjpgpath = string.Format(@"{0}\{1}", sourcedir, "thumbnail.jpg");
                                if (File.Exists(thumbnailjpgpath))
                                {
                                    string chushu = GetChushuByCode(mdnf.QRST_CODE);
                                    string shangnumber = Convert.ToString((Convert.ToInt32(chushu) / 1000));
                                    string sharepath = string.Format(@"{0}\Thumb", sharedir);//\\192.168.10.202\zhsjk\\Thumb
                                    string newfilepath = Path.Combine(sharepath, shangnumber);
                                    if (newfilepath != "" && Directory.Exists(newfilepath))
                                    {
                                        string destdir = string.Format(@"{0}\{1}.jpg", newfilepath, mdnf.QRST_CODE);
                                        File.Copy(thumbnailjpgpath, destdir);
                                    }
                                    else
                                    {
                                        Directory.CreateDirectory(newfilepath);
                                        string destdir = string.Format(@"{0}\{1}.jpg", newfilepath, mdnf.QRST_CODE);
                                        File.Copy(thumbnailjpgpath, destdir);
                                    }
                                }
                                else
                                { }
                            }
                            #region 注释1 的为 解压压缩包路径为任意指定位置
                            //Directory.CreateDirectory(destdir);
                            //DataPacking.unZipFile(filename, destdir);//问题 解压到的目的路径是随意设置一个还是直接解压到缓存中？？  现在思路是解压到临时目录然后把thumbnail.jpg复制到了缓存（以code命名的）中
                            //string thumbnailjpgpath = string.Format(@"{0}\{1}", destdir, "thumbnail.jpg");
                            //if (File.Exists(thumbnailjpgpath))
                            //{
                            //    //缓存位置
                            //    string cachePath = string.Format(@"{0}\{1}", Application.StartupPath, @"Cache\Thumbnail");
                            //    string newpath = string.Format(@"{0}\{1}", cachePath, mdnf.QRST_CODE);
                            //    if (Directory.Exists(newpath))
                            //    {
                            //        string newfile = string.Format(@"{0}\{1}.jpg", newpath, mdnf.QRST_CODE);
                            //        File.Copy(thumbnailjpgpath, newfile, true);
                            //    }
                            //    else
                            //    {
                                   
                            //        Directory.CreateDirectory(newpath);
                            //        string newfile = string.Format(@"{0}\{1}.jpg", newpath, mdnf.QRST_CODE);
                            //        File.Copy(thumbnailjpgpath, newfile, true);
                            //    }
                            //}
                            //else { }
                            #endregion
                        }
                        else
                        { 
                        }
                    }
                    else
                    { 
                    }

                    if (ctrlVirtualDirSetting1.UsingVirtualDir)
                    {
                        //执行虚拟文件夹操作
                        Add2VirtualDir(mdnf.QRST_CODE);
                    }
                    log.Info(string.Format("数据导入成功：{0}！", filename.ToString()));
                   
                }
                catch (Exception ex)
                {
                    log.Error(string.Format("数据导入异常：{0}！\r\n{1}", obj.ToString(), ex.Message));
                    
                }

            }
        }
        /// <summary>
        /// 判断压缩包里是否含有thumbnail.jpg
        /// </summary>
        /// <param name="zippath"></param>
        /// <returns></returns>
        private bool isExitJPG(string zippath)
        { 
             ZipInputStream zis = new ZipInputStream(File.OpenRead(zippath.Trim()));
             ZipEntry ze = zis.GetNextEntry();            
             while (ze != null)
             {
                 int position = ze.Name.LastIndexOf("/");
                 string str = ze.Name.ToString();
                 if (position > 0)
                 {
                     str = ze.Name.Substring(0, position);           
                 }
                 int position1 = str.IndexOf("thumbnail.jpg");
                 if (position1 == 0)
                 {
                     return true;
                     break;
                 }               
                 ze = zis.GetNextEntry();            
             }
             return false;      
        }
        private string GetChushuByCode(string code)//0001-EVDB-XJ02-8
        {
            int startIndex = code.LastIndexOf('-');
            string chushu = code.Substring(startIndex+1, code.Length - startIndex-1);
            return chushu;           
        }
      

        private void Add2VirtualDir(string code)
       {
            if (ctrlVirtualDirSetting1.CheckValue())
            {
                ctrlVirtualDirSetting1.AddFileLink(code);
            }
        }

        private void SetCustomizedMetaData(MetaDataNormalFile mdnf)
        {
            this.Invoke(new EventHandler(delegate
               {
                   mdnf.uploaduser = _currentUser.NAME;
                   mdnf.uploaddate = DateTime.Now;
                   mdnf.remark = rtextRemark.Text;
                   try
                   {
                       mdnf.groupcode = ((DataType)cmbFileCatalog.SelectedItem).groupcode;
                       //MetaDataNormalFile.GetDefaultGroupCode(isdbUtil,();
                   }
                   catch (Exception ex)
                   {
                       mdnf.groupcode = "";
                   }
               }));
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < cbImportDataLst.Items.Count; i++)
            {
                cbImportDataLst.SetItemChecked(i, true);
            }
        }

        private void btnRemoveAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < cbImportDataLst.Items.Count; i++)
            {
                cbImportDataLst.SetItemChecked(i, false);
            }
        }

        private void btn_ClearList_Click(object sender, EventArgs e)
        {

            cbImportDataLst.Items.Clear();
        }
    }
}
