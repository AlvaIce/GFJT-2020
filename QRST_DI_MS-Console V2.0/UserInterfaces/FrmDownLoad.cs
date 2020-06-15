using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using QRST_DI_DS_Metadata.MetaDataDefiner.Mdl;
using System.IO;
using QRST_DI_DS_Metadata.MetaDataCls;
using QRST_DI_DS_Basis.DataDownLoad;
using QRST_DI_DS_Basis;

namespace QRST_DI_MS_Console.UserInterfaces
{
    public partial class FrmDownLoad : DevExpress.XtraEditors.XtraForm
    {
        //public FrmDownLoad()
        //{
        //    InitializeComponent();
        //}

        private metadatacatalognode_Mdl selectedQueryObj;
        private List<string> downLoadLst;

        public FrmDownLoad(metadatacatalognode_Mdl _selectedQueryObj,List<string> _downLoadLst)
        {
            InitializeComponent();
            selectedQueryObj = _selectedQueryObj;
            downLoadLst = _downLoadLst;

            if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_raster") && selectedQueryObj.DATA_CODE.ToUpper().Contains("EVDB"))
            {
                //GF数据
                radioGroup1.Enabled = true;
            }
            else
            {
                radioGroup1.Enabled = false;
            }
        }


        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {

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
            if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_tile"))  //下载切片
            {
                try
                {
                    WS_QDB_GetDataService.Service client = new WS_QDB_GetDataService.Service();
                    string[] srcPath = client.GetTilesList(downLoadLst.ToArray());
                    List<string> tilesPath = new List<string>();
                    tilesPath.AddRange(srcPath);
                    for (int i = 0; i < srcPath.Length; i++)
                    {
                        string destPath = string.Format(@"{0}\{1}", textBoxDataPath.Text, Path.GetFileName(srcPath[i]));
                        if (File.Exists(srcPath[i]))
                        {
                            //段龙方20130823添加  下载jpg的同时，下载jgw数据。
                            string jgwSourcePath = Path.Combine(Path.GetDirectoryName(srcPath[i]), Path.GetFileNameWithoutExtension(srcPath[i]) + ".jgw");
                            if (File.Exists(jgwSourcePath))
                            {
                                tilesPath.Add(jgwSourcePath);
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
                if (radioGroup1.SelectedIndex == 0) //下载原始数据
                {
                    string destDir = textBoxDataPath.Text;
                    FrmDownLoadLst.GetInstance().Show();
                    FrmDownLoadLst.GetInstance().AddDownLoadTaskThreads(downLoadLst, destDir);

                }
                else    //下载校正数据,目前只支持GF1号数据  
                {
                    MetaDataGF1 metaData = new MetaDataGF1();
                    for (int i = 0; i < downLoadLst.Count; i++)
                    {
                        metaData.GetModel(downLoadLst[i], TheUniversal.EVDB.sqlUtilities);
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

        private void radioGroup1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }
    }
}