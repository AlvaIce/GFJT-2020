using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using QRST_DI_DS_Metadata.MetaDataDefiner.Mdl;
using DevExpress;
using DevExpress.XtraEditors;
using System.IO;
using QRST_DI_DS_Metadata;
using QRST_DI_DS_Metadata.MetaDataCls;

namespace QRST_DI_MS_Console.UserInterfaces
{
    public partial class rucDataQualityInspection : RibbonPageBaseUC
    {
        public mucDataQualityInspection mucDQI;
        private metadatacatalognode_Mdl nodeMdl;
        private string qrstCode;
        private string[] fileNames;

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
            for (int i = 0; i < TheUniversal.subDbLst.Count; i++)
            {
                TreeNode tn = TheUniversal.subDbLst[i].GetDbNode();
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
            nodeMdl = (metadatacatalognode_Mdl)this.treeViewInspectDataType.SelectedNode.Tag;
            if (nodeMdl.DATA_CODE == null || nodeMdl.DATA_CODE.Equals(""))
            {
                XtraMessageBox.Show("您选择的不是有效的数据，请重新选择！！");
            }
            else
            {
                qrstCode = nodeMdl.DATA_CODE;
                ClosePopup();
            }
        }
        /// <summary>
        /// 获取选择的树节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void repositoryItemPopupContainerDataType_QueryResultValue(object sender, DevExpress.XtraEditors.Controls.QueryResultValueEventArgs e)
        {
            e.Value = this.treeViewInspectDataType.SelectedNode.Text;
            if (e.Value.ToString() == "高分系列卫星数据")
            {
                mucDQI.DisplayZB(true);
            }
            else
            {
                mucDQI.DisplayZB(false);
            }
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
        /// 选择文件
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
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if(fbd.ShowDialog() == DialogResult.OK)
            {
               // DataCheck dc = new DataCheck(Directory.get);
                string[] strArr = Directory.GetFiles(fbd.SelectedPath);
                List<string> str = new List<string>();
                str.AddRange(strArr);
                DataCheck dc = new DataCheck(str);
                barEditItemSelectFile.EditValue = fbd.SelectedPath;
                mucDQI.dc = dc;
                mucDQI.starTime();
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
            mucDQI.StartCheck();
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
