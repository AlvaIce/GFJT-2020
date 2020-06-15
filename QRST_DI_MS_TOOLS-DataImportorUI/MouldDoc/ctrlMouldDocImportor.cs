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

namespace QRST_DI_MS_TOOLS_DataImportorUI.MouldDoc
{
    public partial class ctrlMouldDocImportor : UserControl
    {
        ILog log = LogManager.GetLogger(typeof(ctrlMouldDocImportor));
        public static userInfo _currentUser;            //当前用户
        static MySqlBaseUtilities isdbUtil = null;

        public ctrlMouldDocImportor()
        {
            InitializeComponent();
        }


        public void Create(MySqlBaseUtilities isdb, userInfo currentUser)
        {
            isdbUtil = isdb;
            _currentUser = currentUser;
        }

        public string _SourceFile { get; set; }
        public MetaDataMouldDoc _MetaDataMDoc { get; set; }
        private void buttonOpenFolder_Click(object sender, EventArgs e)
        {
                OpenFileDialog ofd = new OpenFileDialog();
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    textBox1.Text = ofd.FileName;
                    _SourceFile = ofd.FileName;
                    ReadFile();
                }
        }

        private void ReadFile()
        {
            _MetaDataMDoc = new MetaDataMouldDoc();
            _MetaDataMDoc.ReadAttributes(_SourceFile);

            textTitle.Text = _MetaDataMDoc.TITLE;
            datetimepicker_DocDate.Value = _MetaDataMDoc.DOCDATE;
            cmbDocType.SelectedIndex = 3;
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
                try
                {
                    log.Info(string.Format("###########开始导入数据{0}###############",_SourceFile));

                    SetCustomizedMetaData();
                   _MetaDataMDoc.ImportData(isdbUtil);
                   _MetaDataMDoc.GetModel(_MetaDataMDoc.QRST_CODE, isdbUtil);

                    if (storePath == null)
                    {
                        string tableCode = StoragePath.GetTableCodeByQrstCode(_MetaDataMDoc.QRST_CODE);
                        storePath = new StoragePath(tableCode);
                    }
                    string destpath = storePath.GetDataOldPathForTools(_MetaDataMDoc);

                    if (!Directory.Exists(destpath))
                    {
                        Directory.CreateDirectory(destpath);
                    }
                    File.Copy(_SourceFile, string.Format(@"{0}\{1}", destpath, Path.GetFileName(_SourceFile), true));


                    if (chk_DirImportMode.Checked)
                    {
                        //执行虚拟文件夹操作
                        Add2VirtualDir();
                    }
                    log.Info(string.Format("数据导入成功：{0}！", _SourceFile));
                }
                catch (Exception ex)
                {
                    log.Error(string.Format("数据导入异常：{0}！\r\n{1}", _SourceFile, ex.Message));
                }

        }

        private void Add2VirtualDir()
        {
            throw new NotImplementedException();
        }

        private void SetCustomizedMetaData()
        {
            this.Invoke(new EventHandler(delegate
               {
                   _MetaDataMDoc.TITLE = textTitle.Text;
                   _MetaDataMDoc.KEYWORD = textKEYWORDs.Text;
                   _MetaDataMDoc.DOCTYPE = cmbDocType.SelectedItem.ToString();
                   _MetaDataMDoc.ABSTRACT = rtxtDESC.Text;
                   _MetaDataMDoc.AUTHOR = textAUTHORS.Text;
                   _MetaDataMDoc.DESCRIPTION = rtextRemark.Text;
                   _MetaDataMDoc.DOCDATE = datetimepicker_DocDate.Value;
                   _MetaDataMDoc.UPLOADER = _currentUser.NAME;
                   _MetaDataMDoc.UPLOADTIME = DateTime.Now;
               }));
        }

    }
}
