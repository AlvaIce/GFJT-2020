using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using QRST_DI_DS_Basis.DBEngine;
using log4net;
using QRST_DI_MS_Basis.UserRole;
using QRST_DI_DS_Metadata.MetaDataDefiner.Dal;
using QRST_DI_DS_Metadata.MetaDataCls;
using QRST_DI_MS_TOOLS_DataImportorUI.Common;
using QRST_DI_DS_Metadata.Paths;

namespace QRST_DI_MS_TOOLS_DataImportorUI.NormalFile
{
    public partial class ctrlNormalFileImportor : UserControl
    {
        ILog log = LogManager.GetLogger(typeof(ctrlNormalFileImportor));
        public static userInfo _currentUser;            //当前用户
        static MySqlBaseUtilities isdbUtil = null;

        public ctrlNormalFileImportor()
        {
            InitializeComponent();
        }


        public void Create(MySqlBaseUtilities isdb, userInfo currentUser)
        {
            isdbUtil = isdb;
            _currentUser = currentUser;
        }

        private DirectoryInfo sourceDi;
        private void buttonOpenFolder_Click(object sender, EventArgs e)
        {
            if (chk_DirImportMode.Checked)
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

        private void btn_ImportData_Click(object sender, EventArgs e)
        {
            WaitForm wf = new WaitForm();
            wf.datask += ImportData;
            wf.beginShowDialog();
            MessageBox.Show("入库完成！");

        }

        private StoragePath storePath = null;
        
        private void ImportData()
        {
            foreach (object obj in cbImportDataLst.CheckedItems)
            {
                try
                {
                    string filename = obj.ToString();
                    log.Info(string.Format("###########开始导入数据{0}###############", filename.ToString()));

                    MetaDataNormalFile mdnf = new MetaDataNormalFile();
                    mdnf.ReadAttributes(filename);
                    SetCustomizedMetaData(mdnf);
                    mdnf.ImportData(isdbUtil);
                    mdnf.GetModel(mdnf.QRST_CODE, isdbUtil);

                    if (storePath == null)
                    {
                        string tableCode = StoragePath.GetTableCodeByQrstCode(mdnf.QRST_CODE);
                        storePath = new StoragePath(tableCode);
                    }
                    string destpath = storePath.GetDataOldPathForTools(mdnf);

                    string dir = Path.GetDirectoryName(destpath);
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    File.Copy(filename, destpath, true);


                    if (chk_DirImportMode.Checked)
                    {
                        //执行虚拟文件夹操作
                        Add2VirtualDir();
                    }
                    log.Info(string.Format("数据导入成功：{0}！", filename.ToString()));
                }
                catch (Exception ex)
                {
                    log.Error(string.Format("数据导入异常：{0}！\r\n{1}", obj.ToString(), ex.Message));
                }

            }
        }

        private void Add2VirtualDir()
        {
            throw new NotImplementedException();
        }

        private void SetCustomizedMetaData(MetaDataNormalFile mdnf)
        {
            this.Invoke(new EventHandler(delegate
               {
                   mdnf.uploaduser = _currentUser.NAME;
                   mdnf.uploaddate = DateTime.Now;
                   mdnf.remark = rtextRemark.Text;
                   mdnf.groupcode = MetaDataNormalFile.GetDefaultGroupCode(isdbUtil);
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
