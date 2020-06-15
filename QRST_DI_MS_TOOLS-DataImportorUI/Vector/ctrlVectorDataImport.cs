using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading.Tasks;
using log4net;
using QRST_DI_MS_TOOLS_DataImportorUI.Common;
using QRST_DI_MS_TOOLS_DataImportorUI.Vector;
using QRST_DI_DS_Metadata.MetaDataCls;
using QRST_DI_DS_Basis.DBEngine;
using QRST_DI_MS_Basis.UserRole;

namespace QRST_DI_MS_TOOLS_DataImportorUI.Vector
{
    public partial class ctrlVectorDataImport : UserControl
    {
        log4net.ILog log = LogManager.GetLogger(typeof(MetaDataVector));

        private ImportVectorData importData;  //数据导入对象

        ctrlVectorBatchImportLst ctrlbatchImportLst ;
        ctrlVectorMetaData ctrlGetMetaData;
        bool[] importEnable = new bool[] { false, false };
        MySqlBaseUtilities bsdbUtil = null;
        public static userInfo _currentUser;            //当前用户

        //数据导入模式
        private bool _IsSingleImport;
        public bool IsSingleImport
        {
            get { return _IsSingleImport; }
            set
            {
                if (value)  //切换到单文件导入
                {
                    textSingleFilePath.Enabled = true;
                    btnChooseSingleFile.Enabled = true;

                    textFolderPath.Enabled = false;
                    btnChooseFolder.Enabled = false;

                    btnFinish.Enabled = importEnable[1];

                    AddContrl2MainPanel((Control)ctrlGetMetaData);
                }
                else       //切换到批量导入
                {
                    textSingleFilePath.Enabled = false;
                    btnChooseSingleFile.Enabled = false;

                    textFolderPath.Enabled = true;
                    btnChooseFolder.Enabled = true;

                    btnFinish.Enabled = importEnable[0];

                    if(ctrlbatchImportLst==null)
                    {
                        ctrlbatchImportLst = new ctrlVectorBatchImportLst();
                        ctrlbatchImportLst.itemStateChangedDel += DisableFinishButton;
                    }
                    AddContrl2MainPanel(ctrlbatchImportLst);
                }
                _IsSingleImport = value;

                ChangeFinishButtonState();
            }
        }

        public ctrlVectorDataImport()
        {
            InitializeComponent();
            importData = new ImportVectorData();
            ctrlGetMetaData = new ctrlVectorMetaData();
        }

        public void Create(MySqlBaseUtilities bsdb, userInfo currentUser)
        {
            bsdbUtil = bsdb;
            _currentUser = currentUser;
            radioSingleImport.Checked = true;
        }

        /// <summary>
        /// 批量导入模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioBatchImport_CheckedChanged(object sender, EventArgs e)
        {
            IsSingleImport = radioSingleImport.Checked;
        }

        //单文件导入模式
        private void radioSingleImport_CheckedChanged(object sender, EventArgs e)
        {
            IsSingleImport = radioSingleImport.Checked;
        }
        /// <summary>
        /// 选择批量导入文件夹
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChooseFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if(fbd.ShowDialog() == DialogResult.OK)
            {
                textFolderPath.Text = fbd.SelectedPath;
                //解析要导入的项
                _shpfiles.Clear();
                GetShpfilesFromDir(fbd.SelectedPath);
                foreach (FileInfo fi in _shpfiles)
                {
                    try
                    {
                        //解析导入项
                        SingleDataVector sigleVD = new SingleDataVector(fi.FullName);

                        importData.Add(sigleVD);
                    }
                    catch (Exception ex)
                    {
                        log.Error("读取元数据失败(" + fi.FullName + ")：" + ex.ToString());
                    }
                }

                ctrlbatchImportLst.AddItems(importData.GetImportDataLst());

            }
        }

        public List<FileInfo> _shpfiles = new List<FileInfo>();

        public void GetShpfilesFromDir(string DirName)
        {
            //文件夹信息
            DirectoryInfo dir = new DirectoryInfo(DirName);
            //如果非根路径且是系统文件夹则跳过
            if (null != dir.Parent && dir.Attributes.ToString().IndexOf("System") > -1)
            {
                return;
            }
            //取得所有文件
            FileInfo[] finfo = dir.GetFiles(SingleDataVector.FileSearchPattern);
            for (int i = 0; i < finfo.Length; i++)
            {
                _shpfiles.Add(finfo[i]);
            }

            //取得所有子文件夹
            DirectoryInfo[] dinfo = dir.GetDirectories();
            for (int i = 0; i < dinfo.Length; i++)
            {
                //查找子文件夹中是否有符合要求的文件
                GetShpfilesFromDir(dinfo[i].FullName);
            }
        }

        /// <summary>
        /// 选择单个文件导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChooseSingleFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = SingleDataVector.FileFilter;
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    textSingleFilePath.Text = ofd.FileName;
                    //解析导入项
                    SingleDataVector sigleVD = new SingleDataVector(ofd.FileName);
                    sigleVD.ReadMetaData();
                    ctrlGetMetaData.Create(sigleVD);

                    SetimportEnable(IsSingleImport, true);
                }
                catch (Exception ex)
                {
                    log.Error("读取元数据失败："+ex.ToString());
                }
               
            }
        }

        /// <summary>
        /// 开始导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFinish_Click(object sender, EventArgs e)
        {

            WaitForm wf = new WaitForm();
            wf.datask += importDatas;
            wf.beginShowDialog();
            MessageBox.Show("入库完成！");
        }

        void importDatas()
        {
            importData.ClearAll();
            //整理要导入的数据项
            if (IsSingleImport)
            {
                if (ctrlGetMetaData.Check())
                {
                    //收集用户信息
                    ctrlGetMetaData.SetCustomizedMetaData();
                    importData.Add(ctrlGetMetaData._singleDataVector);
                    log.Info(string.Format("入库项添加成功：{0}", ctrlGetMetaData._singleDataVector._filepath));
                }
            }
            else   //批量导入
            {
                // importData.AddRange(ctrlbatchImportLst.GetCheckedItems());
                foreach (SingleDataVector temp in ctrlbatchImportLst.GetCheckedItems())
                {
                    try
                    {
                        temp.ReadMetaData();
                        ctrlbatchImportLst.SetCustomizedMetaData(temp);
                        importData.Add(temp);
                        log.Info(string.Format("入库项添加成功：{0}",temp._filepath));
                    }
                    catch(Exception ex)
                    {
                        log.Error(string.Format("入库项添加失败：{0}", temp._filepath));
                    }
                  
                }
            }
            importData.DataImport(bsdbUtil);
        }

        private void DisableFinishButton(int checkItemCount)
        {
            if (0 == checkItemCount)
            {
                SetimportEnable(IsSingleImport, false);
            }
            else
                SetimportEnable(IsSingleImport, true);
        }


        /// <summary>
        /// 取消导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancle_Click(object sender, EventArgs e)
        {

        }

        private void AddContrl2MainPanel(Control ctrl)
        {
            panelMain.Controls.Clear();
            panelMain.Controls.Add(ctrl);
            ctrl.Dock = DockStyle.Fill;

        }

        private void SetimportEnable(bool issingleImport,bool setValue)
        {
            if (issingleImport)
            {
                importEnable[1] = setValue;
            }
            else
                importEnable[0] = setValue;

            ChangeFinishButtonState();
        }


        private void ChangeFinishButtonState()
        {
            if (IsSingleImport)
            {
                btnFinish.Enabled = importEnable[1];
            }
            else
                btnFinish.Enabled = importEnable[0];
        }
    }
}
