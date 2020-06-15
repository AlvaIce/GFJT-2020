using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using QRST_DI_MS_TOOLS_DataImportorUI.Common;
using QRST_DI_MS_Basis.UserRole;
using QRST_DI_DS_Basis.DBEngine;
using log4net;

namespace QRST_DI_MS_TOOLS_DataImportorUI.Raster
{
    public partial class ctrlRasterProdImportor : UserControl
    {

        ILog log = LogManager.GetLogger(typeof(ctrlRasterProdImportor)); 
        public static userInfo _currentUser;            //当前用户
        MySqlBaseUtilities indbUtil = null;
        public ctrlRasterProdImportor()
        {
            InitializeComponent();
            _ProdDirs = new List<SingleDataImageProd>();
            _dicDrSdip = new Dictionary<DataGridViewRow, SingleDataImageProd>();
            _ProdDirsForImport = new List<SingleDataImageProd>();
        }

        public void Create(MySqlBaseUtilities indb, userInfo currentUser)
        {
            indbUtil = indb;
            _currentUser = currentUser;
        }

        public List<SingleDataImageProd> _ProdDirs;
        public List<SingleDataImageProd> _ProdDirsForImport;
        private void buttonOpenFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            //fbd.ShowDialog();

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                this.textBox1.Text = fbd.SelectedPath;
                _ProdDirs.Clear();

                DirectoryInfo di = new DirectoryInfo(this.textBox1.Text);

                bool findproddir = SingleDataImageProd.IsProdDir(di);
                if (findproddir)
                {
                    SingleDataImageProd sdip = new SingleDataImageProd(di);
                    _ProdDirs.Add(sdip);
                }
                else
                {
                    //检查子文件是否是proddir
                    DirectoryInfo[] cdis = di.GetDirectories();
                    foreach (DirectoryInfo cdi in cdis)
                    {
                        if (SingleDataImageProd.IsProdDir(cdi))
                        {
                            SingleDataImageProd sdip = new SingleDataImageProd(cdi);
                            _ProdDirs.Add(sdip);
                        }
                    }
                }

                UpdateSDIPGridView();
            }
        }
        private Dictionary<DataGridViewRow, SingleDataImageProd> _dicDrSdip;
        private void UpdateSDIPGridView()
        {
            this.dataGridViewX1.Rows.Clear();
            _dicDrSdip.Clear();
            foreach (SingleDataImageProd sdip in _ProdDirs)
            {
                if (!sdip._hasReadMetaData)
                {
                    sdip.ReadMetaData();
                }
                QRST_DI_DS_Metadata.MetaDataCls.MetaDataImageProd md = sdip._MetaData;
                int idx = this.dataGridViewX1.Rows.Add();
                DataGridViewRow dr = this.dataGridViewX1.Rows[idx];
                dr.Cells["colProdDir"].Value = md.Name;
                dr.Cells["colProducor"].Value = md.Produsor;
                dr.Cells["colProduceTime"].Value = md.ProducedDate;
                dr.Cells["colRemark"].Value = md.Remark;
                dr.Cells["colSourceDataName"].Value = md.SourceDataName;
                dr.Cells["colProdType"].Value = md.ProdType;
                dr.Cells["colSize"].Value = md.Size;
                _dicDrSdip.Add(dr, sdip);
            }
        }

        private void SetCustomizedMetaData()
        {
            foreach (DataGridViewRow dr in this.dataGridViewX1.Rows)
            {
                if (dr.Cells["colCheck"].Value!=null&&(bool)dr.Cells["colCheck"].Value == true)
                {
                    SingleDataImageProd sdip = _dicDrSdip[dr];
                    sdip._MetaData.Produsor =(dr.Cells["colProducor"].Value!=null)?dr.Cells["colProducor"].Value.ToString() : "";
                    sdip._MetaData.ProducedDate =(DateTime) dr.Cells["colProduceTime"].Value ;
                    sdip._MetaData.Remark = (dr.Cells["colRemark"].Value != null) ? dr.Cells["colRemark"].Value.ToString() : "";
                    sdip._MetaData.SourceDataName = (dr.Cells["colSourceDataName"].Value != null) ? dr.Cells["colSourceDataName"].Value.ToString() : "";
                    sdip._MetaData.ProdType = (dr.Cells["colProdType"].Value != null) ? dr.Cells["colProdType"].Value.ToString() : "";
                    sdip._MetaData.UploadUser = ctrlRasterProdImportor._currentUser.NAME;
                }
            }
        }

        private void btn_ImportData_Click(object sender, EventArgs e)
        {
            WaitForm wf = new WaitForm();
            wf.datask += importDatas;
            wf.beginShowDialog();
            MessageBox.Show("入库完成！");
        }

        void importDatas()
        {

            GetProdDirsForImport();
            SetCustomizedMetaData();

            foreach (SingleDataImageProd sdip in _ProdDirsForImport)
            {
                try
                {
                    log.Info(string.Format("###########开始导入数据{0}###############", sdip.ToString()));
                    sdip.DataImport(indbUtil);
                    log.Info(string.Format("数据导入成功：{0}！", sdip.ToString()));
                }
                catch (Exception ex)
                {
                    log.Error(string.Format("数据导入失败:{0}！", sdip.ToString()), ex);
                }

            }
        }

        private void GetProdDirsForImport()
        {
            _ProdDirsForImport.Clear();
            foreach (DataGridViewRow dr in this.dataGridViewX1.Rows)
            {
                if (dr.Cells["colCheck"].Value!=null&&(bool)dr.Cells["colCheck"].Value == true)
                {
                    SingleDataImageProd sdip = _dicDrSdip[dr];
                    _ProdDirsForImport.Add(sdip);
                }
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (this.dataGridViewX1.Rows.Count>0)
            {
                this.全不选ToolStripMenuItem.Visible = true;
                this.全选ToolStripMenuItem.Visible = true;
                this.还原初始状态ToolStripMenuItem.Visible = true;
            }
            else
            {
                this.全不选ToolStripMenuItem.Visible = false;
                this.全选ToolStripMenuItem.Visible = false;
                this.还原初始状态ToolStripMenuItem.Visible = false;
            }
        }

        private void 全选ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dr in dataGridViewX1.Rows)
            {
                dr.Cells["colCheck"].Value = true;
            }
        }

        private void 全不选ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dr in dataGridViewX1.Rows)
            {
                dr.Cells["colCheck"].Value = false;
            }
        }

        private void 还原初始状态ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateSDIPGridView();
        }
    }
}
