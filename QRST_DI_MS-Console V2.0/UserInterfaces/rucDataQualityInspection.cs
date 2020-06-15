using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using QRST_DI_DS_Metadata.MetaDataDefiner.Mdl;
using DevExpress.XtraEditors;
using System.IO;
 
namespace QRST_DI_MS_Desktop.UserInterfaces
{
    public partial class rucDataQualityInspection : RibbonPageBaseUC
    {
        public mucDataQualityInspection mucDQI;
        private metadatacatalognode_Mdl nodeMdl;
        private string qrstCode;
        private string[] fileNames;
        private string[] nodeName = {"高分一号卫星",
                                    "高分二号卫星",
                                    "高分三号卫星",
                                    "高分四号卫星"};
        public rucDataQualityInspection()
            : base()
        {
            InitializeComponent();
            InitialDBTree();
        }
        public rucDataQualityInspection(object objMUC)
            : base(objMUC)
        {
            InitializeComponent();
            mucDQI = objMUC as mucDataQualityInspection;
            InitialDBTree();
        }

        private void treeViewInspectDataType_AfterSelect(object sender, TreeViewEventArgs e)
        {
            string qrstcode = e.Node.Text;
            //repositoryItemDataType
        }

        void InitialDBTree()
        {
            //TreeNode tn=new TreeNode(){Text = "QRST",Tag = "root",Name = "root"};
            //获取所有子库列表，初始化元数据树结构
            //for (int i = 0; i < TheUniversal.subDbLst.Count; i++)
            //{
            //    TreeNode tn = TheUniversal.subDbLst[i].GetDbNode();
            //    if (tn != null)
            //    {
            //        treeViewInspectDataType.Nodes.Add(tn);
            //    }

            //}
            for (int i = 0; i < nodeName.Length; i++)
            {
                TreeNode tn = new TreeNode ();
                tn.Text = nodeName[i];
                tn.Name = nodeName[i];
                if (tn != null)
                {
                    treeViewInspectDataType.Nodes.Add(tn);
                }

            }
            treeViewInspectDataType.ExpandAll();
        }

        private void rucDataQualityInspection_VisibleChanged(object sender, EventArgs e)
        {
            InitialDBTree();
        }
        /// <summary>
        /// 双击选择数据类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeViewInspectDataType_DoubleClick(object sender, EventArgs e)
        {
            //nodeMdl = (metadatacatalognode_Mdl)this.treeViewInspectDataType.SelectedNode.Tag;
            //if (nodeMdl.DATA_CODE == null || nodeMdl.DATA_CODE.Equals(""))
            //{
            //    XtraMessageBox.Show("您选择的不是有效的数据，请重新选择！！");
            //}
            //else
            //{
            //    qrstCode = nodeMdl.DATA_CODE;
            //    ClosePopup();
            //}
           String  nodeMdl =this.treeViewInspectDataType.SelectedNode.Text;
           if (nodeMdl == null || nodeMdl.Equals(""))
            {
                XtraMessageBox.Show("您选择的不是有效的数据，请重新选择！！");
            }
            else
            {
                //qrstCode = nodeMdl.DATA_CODE;
                ClosePopup();
            }
        }
        /// <summary>
        /// 获取选择的树节点,根据节点内容判断要显示的卫星检测指标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void repositoryItemPopupContainerDataType_QueryResultValue(object sender, DevExpress.XtraEditors.Controls.QueryResultValueEventArgs e)
        {
            e.Value = this.treeViewInspectDataType.SelectedNode.Text;
            mucDQI.DisplayZB(e.Value.ToString());
        }

        /// <summary>
        /// 关闭popup控件方法
        /// </summary>
        void ClosePopup()
        {
            if (popupContainerControlInspectDataType.OwnerEdit != null)
                popupContainerControlInspectDataType.OwnerEdit.ClosePopup();
        }

        /// <summary>
        /// 选择检验数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void repositoryItemSelectFile_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            //using (OpenFileDialog dlg = new OpenFileDialog())
            //{
            //    if (dlg.ShowDialog() == DialogResult.OK)
            //    {
            //        barEditItemSelectFile.EditValue = dlg.FileName;
            //    }
            //}

            //FolderBrowserDialog fbd = new FolderBrowserDialog();
            //if(fbd.ShowDialog() == DialogResult.OK)
            //{
            //   // DataCheck dc = new DataCheck(Directory.get);
            //    string[] strArr = Directory.GetFiles(fbd.SelectedPath);
            //    List<string> str = new List<string>();
            //    str.AddRange(strArr);
            //    DataCheck dc = new DataCheck(str);
            //    barEditItemSelectFile.EditValue = fbd.SelectedPath;
            //    mucDQI.dc = dc;
            //    mucDQI.starTime();
                using (FolderBrowserDialog fbd = new FolderBrowserDialog())
                {
                    if (fbd.ShowDialog() == DialogResult.OK)
                    {
                        // DataCheck dc = new DataCheck(Directory.get);
                        SearchTarFiles(fbd.SelectedPath);
                        //string[] strArr = Directory.GetFiles(fbd.SelectedPath);
                        //List<string> str = new List<string>();
                        //str.AddRange(strArr);
                        DataCheck dc = new DataCheck(filename);
                        filename.Clear();
                        barEditItemSelectFile.EditValue = fbd.SelectedPath;
                        mucDQI.dc = dc;
                        mucDQI.starTime();
                    }
                }
        }
        List<string> filename = new List<string>();
        /// <summary>
        /// 循环遍历文件夹和子文件夹下所有待检测的文件
        /// </summary>
        /// <param name="p">目标路径</param>
        private void SearchTarFiles(string p)
        {
           // filename.Clear();
            DirectoryInfo dir = new DirectoryInfo(p);
            if (dir != null)
            {
                 FileSystemInfo[] fs= dir.GetFileSystemInfos();
                 for (int i = 0; i < fs.Length; i++)
                 {
                     FileInfo file = fs[i] as FileInfo;
                     if (file != null)
                     {
                         filename.Add(file.FullName);
                     }
                     else
                     {
                         DirectoryInfo di = fs[i] as DirectoryInfo;
                         if (di != null)
                         {
                             SearchTarFiles(di.FullName);
                         }
                     }
                 }
            }
        }
       
        /// <summary>
        /// 选择文件夹
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void repositoryItemSelectDir_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {

            using (FolderBrowserDialog dlg = new FolderBrowserDialog { ShowNewFolderButton = false })
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                  //  barEditItemSelectDir.EditValue = dlg.SelectedPath;
                   // fileNames = Directory.GetFiles(dlg.SelectedPath, "*.*");
                }
            }
        }
        /// <summary>
        /// 单个文件检查
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFileInspection_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //mucDQI.StartCheck(); 
            if (barEditItemSelectFile.EditValue == null)
            {
                MessageBox.Show("请选择待检测文件!");
            }
            else { mucDQI.StartCheck(); }
        }
        /// <summary>
        /// 批量检查
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void btnBatchInspection_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    if (barEditItemSelectDir.EditValue == null )
        //    {
        //        XtraMessageBox.Show("请选择检验数据！");
        //    }
        //    else if (barEditItemDataType.EditValue == null)
        //    {
        //        XtraMessageBox.Show("请选择需要检验的数据类型！");
        //    }
        //    else
        //    {
        //        InspectFiles();
        //    }
        //}

        public void InspectFile()
        {
            //string filename = barEditItemSelectFile.EditValue.ToString();
            //switch (qrstCode)
            //{
            //    case "EVDB-16": InspectHJData(filename); break;
            //    case "EVDB-18": InspectCbersData(filename); break;
            //    case "EVDB-20": InspectModisData(filename); break;
            //    case "EVDB-21": InspectNoaaData(filename); break;
            //    default: XtraMessageBox.Show("对当前数据类型不提供检验方法！"); break;
            //}
        }
        /// <summary>
        /// 导出检测结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.AddExtension = true;
            sfd.Filter = "Text Document(*.txt)|*.txt";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string filename = sfd.FileName;
                FileStream fs = File.Open(filename, FileMode.Create, FileAccess.Write, FileShare.None);
                string text = mucDQI.memoEditLog.Text;
                Byte[] info = new UTF8Encoding(true).GetBytes(text);
                fs.Write(info, 0, info.Length);

            }
        }
        //选择检验数据
        private void barEditItemSelectFile_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            //{
            //    if (fbd.ShowDialog() == DialogResult.OK)
            //    {
                   // SearchTarFiles(fbd.SelectedPath);
            //        DataCheck dc = new DataCheck(filename);
            //        filename.Clear();
            //        barEditItemSelectFile.EditValue = fbd.SelectedPath;
            //        mucDQI.dc = dc;
            //        mucDQI.starTime();
            //    }
            //}
            using (OpenFileDialog openFileDialog = new OpenFileDialog()) {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "原始影像|*.tar.gz";
                openFileDialog.Multiselect = true;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string[] file_name = openFileDialog.FileNames;
                        for( int i =0;i<file_name.Length;i++){
                            filename.Add(file_name[i]);
                        }
                    DataCheck dc = new DataCheck(filename);
                    filename.Clear();
                    barEditItemSelectFile.EditValue = openFileDialog.FileName;
                    mucDQI.dc = dc;
                    mucDQI.starTime();
                }
            }
        }

        //public void InspectFiles()
        //{
        //    switch (qrstCode)
        //    {
        //        case "EVDB-16":
        //            foreach (string filename in fileNames)
        //            {
        //                InspectHJData(filename);
        //            }
        //            break;
        //        case "EVDB-18": 
        //            foreach (string filename in fileNames)
        //            {
        //                InspectCbersData(filename);
        //            }
        //            break;
        //        case "EVDB-20": 
        //            foreach (string filename in fileNames)
        //            {
        //                InspectModisData(filename);
        //            } 
        //            break;
        //        case "EVDB-21":
        //            foreach (string filename in fileNames)
        //            {
        //                InspectNoaaData(filename); 
        //            } 
        //            break;
        //        default: XtraMessageBox.Show("对当前数据类型不提供检验方法！"); break;
        //    }
        //}

        /// <summary>
        /// 检验环境星数据
        /// </summary>
        /// <param name="filename"></param>
        //public void InspectHJData(string filename)
        //{
        //    MetaDataReader read = new MetaDataReader();
        //    try
        //    {
        //        read.ReadMetaDataHj(filename);
        //        AddTable(filename, "环境星数据检查完成，文件质量良好！");
        //    }
        //    catch (System.Exception ex)
        //    {
        //        //将异常内容加入表格
        //        AddTable(filename, "此数据有异常：" + ex);
        //    }
        //}
        /// <summary>
        /// 检验Cbers数据
        /// </summary>
        /// <param name="filename"></param>
        //public void InspectCbersData(string filename)
        //{
        //    MetaDataReader read = new MetaDataReader();
        //    try
        //    {
        //        read.ReadMetaDataCbers(filename);
        //        AddTable(filename, "Cbers数据检查完成，文件质量良好！");
        //    }
        //    catch (System.Exception ex)
        //    {
        //        //将异常内容加入表格
        //        AddTable(filename, "此数据有异常：" + ex);
        //    }
        //}
        /// <summary>
        /// 检验Modis数据
        /// </summary>
        /// <param name="filename"></param>
        //public void InspectModisData(string filename)
        //{
        //    MetaDataReader read = new MetaDataReader();
        //    try
        //    {
        //        read.ReadMetaDataModis(filename);
        //        AddTable(filename, "Modis数据检查完成，文件质量良好！");
        //    }
        //    catch (System.Exception ex)
        //    {
        //        //将异常内容加入表格
        //        AddTable(filename, "此数据有异常：" + ex);
        //    }
        //}
        /// <summary>
        /// 检验Noaa数据
        /// </summary>
        /// <param name="filename"></param>
        //public void InspectNoaaData(string filename)
        //{
        //    MetaDataReader read = new MetaDataReader();
        //    try
        //    {
        //        read.ReadMetaDataNOAA(filename);
        //        AddTable(filename, "NOAA数据检查完成，文件质量良好！");
        //    }
        //    catch (System.Exception ex)
        //    {
        //        //将异常内容加入表格
        //        AddTable(filename, "此数据有异常：" + ex);
        //    }
        //}

        /// <summary>
        /// 将检验结果信息加入到表格中。
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="exMsg"></param>
        //private void AddTable(string fileName, string exMsg)
        //{
        //    if (fileName == null || fileName == "")
        //    {
        //        return;
        //    }

        //    DataTable dt;

        //    if (mucDQI.gridControlInspectionList.DataSource == null)
        //    {
        //        dt = new DataTable();

        //        DataColumn dc = new DataColumn { ColumnName = "检验数据名称", DataType = Type.GetType("System.String") };
        //        dt.Columns.Add(dc);

        //        dc = new DataColumn { ColumnName = "检验状态", DataType = Type.GetType("System.String") };
        //        dt.Columns.Add(dc);
        //    }
        //    else
        //    {
        //        dt = mucDQI.gridControlInspectionList.DataSource as DataTable;
        //        //dt.Rows.Clear();
        //    }

        //    DataRow dr = dt.NewRow();

        //    dr[0] = Path.GetFileName(fileName);
        //    dr[1] = exMsg;

        //    dt.Rows.Add(dr);

        //    mucDQI.gridControlInspectionList.DataSource = dt;
        //}
    }
}
