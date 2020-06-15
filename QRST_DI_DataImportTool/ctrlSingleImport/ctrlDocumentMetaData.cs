using System;
using System.Collections.Generic;
using System.Windows.Forms;
using QRST_DI_DataImportTool.DataImport.FileData;
using QRST_DI_DataImportTool.DataImport.MetaData;

namespace QRST_DI_DataImportTool.ctrlSingleImport
{
    public partial class ctrlDocumentMetaData : UserControl, IGetMetaData
    {
        MetaDataUserDocument metaDataDocument;

        FileData filedata;

        public ctrlDocumentMetaData()
        {
            InitializeComponent();
        }


        //收集用户输入的元数据信息
        public MetaData GetMetaData(string groupCode)
        {
            metaDataDocument.documentname = textToolName.Text;
            metaDataDocument.author = textAuthor.Text;
            metaDataDocument.keywords = textKeyWord.Text;
            metaDataDocument.fileabstract = textDes.Text;
            metaDataDocument.filetime = dateTimePicker1.Value;

            metaDataDocument.publicInfo.UpLoadTime = DateTime.Now;
            metaDataDocument.publicInfo.UserName = textUpload.Text;
            metaDataDocument.publicInfo.Department = textUploader.Text;
            
            return metaDataDocument;
        }

        public void DisplayMetaData(MetaData metadata)
        {
            metaDataDocument = (MetaDataUserDocument)metadata;
            textToolName.Text = metaDataDocument.documentname;
            textAuthor.Text = metaDataDocument.author;
            textKeyWord.Text = metaDataDocument.keywords;
            textDes.Text = metaDataDocument.fileabstract;
            if (metaDataDocument.publicInfo == null)
            {
                metaDataDocument.publicInfo = new MetaDataPublicInfo();
            }
            textUpload.Text = metaDataDocument.publicInfo.UserName;
            textUploader.Text = metaDataDocument.publicInfo.Department;


            List<string> lstFiles = filedata.GetAttacheMents();
            listBox1.Items.Clear();
            listBox1.Items.AddRange(lstFiles.ToArray());


        }

        public bool Check()
        {
            if (!string.IsNullOrEmpty(textToolName.Text))
                return true;
            else
            {
                MessageBox.Show("请填写工具名称！");
                return false;
            }

        }


        public void GetFileObj(FileData filedata)
        {
            this.filedata = filedata;
        }
    }
}
