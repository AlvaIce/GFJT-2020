using System;
using System.Collections.Generic;
using System.Windows.Forms;
using QRST_DI_DataImportTool.DataImport.MetaData;
using QRST_DI_DataImportTool.DataImport.FileData;

namespace QRST_DI_DataImportTool.ctrlSingleImport
{
    public partial class ctrlUserToolkitMetaData : UserControl,IGetMetaData
    {
        MetaDataUserToolKit metaDataToolkit;

        FileData filedata;

        public ctrlUserToolkitMetaData()
        {
            InitializeComponent();
        }

        //收集用户输入的元数据信息
        public MetaData GetMetaData(string groupCode)
        {
            metaDataToolkit.toolname = textToolName.Text;
            metaDataToolkit.type = textToolType.Text;
            metaDataToolkit.author= textAuthor.Text ;
           metaDataToolkit.unitName = textToolDep.Text ;
            metaDataToolkit.keywords =textKeyWord.Text ;
            metaDataToolkit.toolDescribe = textDes.Text;
            metaDataToolkit.toolVersion = textversion.Text;

            metaDataToolkit.publicInfo.UpLoadTime = DateTime.Now;
            metaDataToolkit.publicInfo.UserName =textUpload.Text ;
            metaDataToolkit.publicInfo.Department =textUploader.Text ;

           

            return metaDataToolkit;
        }

        public void DisplayMetaData(MetaData metadata)
        {
            metaDataToolkit = (MetaDataUserToolKit)metadata;
            textToolName.Text = metaDataToolkit.toolname;
            textToolType.Text = metaDataToolkit.type;
            textAuthor.Text = metaDataToolkit.author;
            textToolDep.Text = metaDataToolkit.unitName;
            textKeyWord.Text = metaDataToolkit.keywords;
            textDes.Text = metaDataToolkit.toolDescribe;
            textversion.Text = metaDataToolkit.toolVersion;
            if (metaDataToolkit.publicInfo == null)
            {
                metaDataToolkit.publicInfo = new MetaDataPublicInfo();
            }
            textUpload.Text = metaDataToolkit.publicInfo.UserName;
            textUploader.Text = metaDataToolkit.publicInfo.Department;


            List<string> lstFiles = filedata.GetAttacheMents();
            listBox1.Items.Clear();
            listBox1.Items.AddRange(lstFiles.ToArray());
            //StringBuilder sb = new StringBuilder();
            //foreach (var file in lstFiles)
            //{
            //    sb.AppendLine(file);
            //}
            //textFiles.Text = sb.ToString();
            
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
