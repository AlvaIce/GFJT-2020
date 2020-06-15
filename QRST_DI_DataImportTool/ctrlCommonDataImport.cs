using System;
using System.Windows.Forms;
using QRST_DI_DataImportTool.DataImport;
using System.IO;
using QRST_DI_DataImportTool.DataImport.FileData;
using QRST_DI_DataImportTool.DataImport.MetaData;
using QRST_DI_DataImportTool.ctrlSingleImport;
using log4net;

namespace QRST_DI_DataImportTool
{
    public partial class ctrlCommonDataImport : UserControl
    {
        log4net.ILog log = LogManager.GetLogger(typeof(MetaDataVector));

        private DataType _dataType;    //数据导入类型
        private ImportData importData;  //数据导入对象

        ctrlBatchImportLst ctrlbatchImportLst ;
        IGetMetaData ctrlGetMetaData;
        public static string groupCode = null;
        bool[] importEnable = new bool[] { false, false }; 
        
        public DataType dataType
        {
            get { return _dataType; }
            set {
                _dataType = value;
                importData = new ImportData(_dataType);
                fileFilter = importData.dataImportFactory.CreateFileData().GetFileFilter();
                ctrlGetMetaData = GetMetaDataFactory.createCtrlGetMetadata(_dataType);
            }
        }

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
                        ctrlbatchImportLst = new ctrlBatchImportLst();
                        ctrlbatchImportLst.itemStateChangedDel += DisableFinishButton;
                    }
                    AddContrl2MainPanel(ctrlbatchImportLst);
                }
                _IsSingleImport = value;

                ChangeFinishButtonState();
            }
        }

        private string fileFilter = "所有文件|*.*";

        public ctrlCommonDataImport()
        {
            InitializeComponent();
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
                string[] dirArr = Directory.GetDirectories(textFolderPath.Text);
                for (int i = 0; i < dirArr.Length;i++ )
                {
                    FileData fileData = importData.dataImportFactory.CreateFileData();
                    fileData.SetfilePath(dirArr[i]);
                    if(fileData.FileCheck()) //检核数据文件的完整性
                    {
                        importData.Add(new SingleData(fileData, importData.dataImportFactory.CreateMetaData()));//将检核通过的数据加入待入库列表
                    }
                }
                ctrlbatchImportLst.AddItems(importData.GetImportDataLst());

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
            ofd.Filter = fileFilter;
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    textSingleFilePath.Text = ofd.FileName;
                    //解析导入项
                    MetaData metaData = importData.dataImportFactory.CreateMetaData();
                    FileData filedata = importData.dataImportFactory.CreateFileData();
                    filedata.SetfilePath(textSingleFilePath.Text);
                    metaData.ReadAttributes(filedata.GetMetaDataFile());
                    ctrlGetMetaData.GetFileObj(filedata);
                    ctrlGetMetaData.DisplayMetaData(metaData);

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
        }

        void importDatas()
        {
            importData.ClearAll();
            //整理要导入的数据项
            if (IsSingleImport)
            {
                if (ctrlGetMetaData.Check())
                {
                    MetaData metadata = ctrlGetMetaData.GetMetaData(groupCode);
                    FileData filedata = importData.dataImportFactory.CreateFileData();
                    filedata.SetfilePath(Directory.GetParent(textSingleFilePath.Text).FullName);
                    importData.Add(new SingleData(filedata, metadata));
                }
            }
            else   //批量导入
            {
                // importData.AddRange(ctrlbatchImportLst.GetCheckedItems());
                foreach (SingleData temp in ctrlbatchImportLst.GetCheckedItems())
                {
                    try
                    {
                        temp.metaData.ReadAttributes(temp.fileData.GetMetaDataFile());
                        importData.Add(temp);
                        log.Info(string.Format("入库项添加成功：{0}",temp.fileData.GetFilePath()));
                    }
                    catch(Exception ex)
                    {
                        log.Error(string.Format("入库项添加失败：{0}",temp.fileData.GetFilePath()));
                    }
                  
                }
            }
            importData.DataImport();
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
            ctrl.Dock = DockStyle.Fill;
            panelMain.Controls.Add(ctrl);

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
