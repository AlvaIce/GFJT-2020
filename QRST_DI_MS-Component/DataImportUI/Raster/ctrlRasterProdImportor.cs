using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using System.IO;
using QRST_DI_MS_Basis.UserRole;
using log4net;
using QRST_DI_MS_Component.Common;
using QRST_DI_SS_DBInterfaces.IDBEngine;
 
namespace QRST_DI_MS_Component_DataImportorUI.Raster
{
    public partial class ctrlRasterProdImportor : UserControl
    {
        private string str = "";
        ILog log = LogManager.GetLogger(typeof(ctrlRasterProdImportor)); 
        public static userInfo _currentUser;            //当前用户
        IDbBaseUtilities indbUtil = null;
        public ctrlRasterProdImportor()
        {
            InitializeComponent();
            _ProdDirs = new List<SingleDataImageProd>();
            _dicDrSdip = new Dictionary<DataGridViewRow, SingleDataImageProd>();
            _ProdDirsForImport = new List<SingleDataImageProd>();
        }

        public void Create(IDbBaseUtilities indb, userInfo currentUser)
        {
            indbUtil = indb;
            _currentUser = currentUser;
            //ctrlVirtualDirSetting1.Create(currentUser.NAME, currentUser.PASSWORD);

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
            bool couldimport = false;
            foreach (DataGridViewRow dr in this.dataGridViewX1.Rows)
            {
                if (dr.Cells["colCheck"].Value != null && (bool)dr.Cells["colCheck"].Value == true)
                {
                    couldimport = true;
                    break;
                }
            }

            if (couldimport)
            {
                WaitForm wf = new WaitForm();
                wf.datask += importDatas;
                wf.beginShowDialog(null);
                if (errmsg==null)
                {
                    errmsg = new List<string>();
                }
                MessageBox.Show(string.Format("入库完成！总计提交入库任务{0}个,成功{1}个,失败{2}个", taskcount, taskcount - errmsg.Count, errmsg.Count));
                if (errmsg.Count>0)
                {
                    StringBuilder sb_allerrmsg = new StringBuilder("");
                    for (int i = 0; i < errmsg.Count; i++)
                    {
                        sb_allerrmsg.AppendLine(errmsg[i]);
                    }
                    MessageBox.Show(sb_allerrmsg.ToString());
                }

                this.textBox1.Text = "";
            }
        }

        int taskcount = 0;
        List<string> errmsg;
        void importDatas(object[] objs)
        {
            //加一个方法首先判断CheckBox是否选中，如果选中对.dataGridViewX1.Rows[i]的每一条数据进行查找 如果count为0 就不dataGridViewX1.Rows.Remove(dr)，否则进行移除操作
            FilterData();
            GetProdDirsForImport();
            SetCustomizedMetaData();
            errmsg = new List<string>();
            taskcount = _ProdDirsForImport.Count;
            for (int i = this.dataGridViewX1.Rows.Count-1; i >-1 ; i--)
            {
                DataGridViewRow dr = this.dataGridViewX1.Rows[i];
                SingleDataImageProd sdip = _dicDrSdip[dr];
                if (_ProdDirsForImport.Contains(sdip))
                {
                    try
                    {
                        log.Info(string.Format("###########开始导入数据{0}###############", sdip.ToString()));
                        sdip.DataImport(indbUtil);
                        if (ctrlVirtualDirSetting1 != null && ctrlVirtualDirSetting1.UsingVirtualDir)
                        {
                            Add2VirtualDir(sdip._MetaData.QRST_CODE);
                        }

                        if (this.dataGridViewX1.InvokeRequired)
                        {
                            this.dataGridViewX1.Invoke(new EventHandler(delegate
                            {
                                this.dataGridViewX1.Rows.Remove(dr);
                            }));
                        }
                        else
                        {
                            this.dataGridViewX1.Rows.Remove(dr);
                        }
                        log.Info(string.Format("数据导入成功：{0}！", sdip.ToString()));
                    }
                    catch (Exception ex)
                    {
                        log.Error(string.Format("数据导入失败:{0}！", sdip.ToString()), ex);
                        errmsg.Add(string.Format("数据导入失败:[{0}]:{1}！", sdip.ToString(), ex));
                    }
                }

                

            }
            if (this.dataGridViewX1.InvokeRequired)
            {
                this.dataGridViewX1.Invoke(new EventHandler(delegate
                {
                    this.dataGridViewX1.Refresh();
                }));
            }
            else
            {
                this.dataGridViewX1.Refresh();
            }

        }

        private void Add2VirtualDir(string code)
        {
            if (ctrlVirtualDirSetting1.CheckValue())
            {
                ctrlVirtualDirSetting1.AddFileLink(code);
            }
        }
        /// <summary>
        /// 过滤数据20170410
        /// </summary>
        private void FilterData()
        {
            if (this.checkBox1.CheckState == CheckState.Checked)
            {
                for (int i = this.dataGridViewX1.Rows.Count - 1; i > -1; i--)
                {
                    DataGridViewRow dr = this.dataGridViewX1.Rows[i];
                    string sql = string.Format("select * from imageprod where Name = '{0}'  ", dr.Cells["colProdDir"].Value);
                    System.Data.DataSet ds = indbUtil.GetDataSet(sql);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        if (this.dataGridViewX1.InvokeRequired)
                        {
                            this.dataGridViewX1.Invoke(new EventHandler(delegate
                            {
                                this.dataGridViewX1.Rows.Remove(dr);
                            }));
                        }
                        else
                        {
                            this.dataGridViewX1.Rows.Remove(dr);
                        }
                    }
                }
            }
            else
            { }
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
                this.复制ToolStripMenuItem.Visible = true;
                this.粘贴ToolStripMenuItem.Visible = true;
            }
            else
            {
                this.全不选ToolStripMenuItem.Visible = false;
                this.全选ToolStripMenuItem.Visible = false;
                this.还原初始状态ToolStripMenuItem.Visible = false;
                this.复制ToolStripMenuItem.Visible = false;
                this.粘贴ToolStripMenuItem.Visible = false;
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
       
        private void 复制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            str = CopyDataGridView(dataGridViewX1);

        }
       
        private void 粘贴ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGirdViewCellPaste(dataGridViewX1);
        }

        /// <summary>
        /// 通过剪贴板复制DataGridView控件中所选中的内容.
        /// </summary>
        /// <param DGView="DataGridView">DataGridView类</param>
        /// <return>字符串</return>
        public string CopyDataGridView(DataGridView DGView)
        {
            string tem_str = "";
            if (DGView.GetCellCount(DataGridViewElementStates.Selected) > 0)
            {
                try
                {
                    //将数据添加到剪贴板中
                    Clipboard.SetDataObject(DGView.GetClipboardContent());
                    //从剪贴板中获取信息
                    tem_str = Clipboard.GetText();
                }
                catch (System.Runtime.InteropServices.ExternalException)
                {
                    return "";
                }
            }
            return tem_str;
        }

        /// <summary>
        /// 从剪贴板环境中获取已复制内容,然后粘贴
        /// </summary>
        /// <param name="DGView"></param>
        public void DataGirdViewCellPaste(DataGridView DGView)
        {
            try
            {
                // 获取剪切板的内容
                string pasteText = Clipboard.GetText();
                if (string.IsNullOrEmpty(pasteText))
                    return;

                if (DGView.GetCellCount(DataGridViewElementStates.Selected) > 0)
                {
                    //this.dataGridViewX1.CurrentCell.Value = pasteText;
                    foreach (DataGridViewCell c in dataGridViewX1.SelectedCells)
                    {
                        c.Value = pasteText;
                    }
                }
            }
            catch
            {
                // 不处理
            }
        }

        
    }
}
