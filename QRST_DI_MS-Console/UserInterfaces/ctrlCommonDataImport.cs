using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using QRST_DI_DataImportTool.DataImport;
using System.IO;
using QRST_DI_DataImportTool.DataImport.FileData;
using QRST_DI_DataImportTool.DataImport.MetaData;
using QRST_DI_DataImportTool.ctrlSingleImport;
using System.Threading.Tasks;
using log4net;
using QRST_DI_DataImportTool;
using QRST_DI_DS_Metadata.MetaDataDefiner.Mdl;
using DevExpress.XtraEditors;
using QRST_DI_DS_Metadata.MetaDataDefiner;
using QRST_DI_DS_Metadata;

namespace QRST_DI_MS_Console.UserInterfaces
{
    public partial class ctrlCommonDataImport : DevExpress.XtraEditors.XtraUserControl
    {
        log4net.ILog log = LogManager.GetLogger(typeof(MetaDataVector));

        private DataType _dataType;    //数据导入类型
        private ImportData importData;  //数据导入对象
        public TreeView treeView1;
        ctrlBatchImportLst ctrlbatchImportLst;
        IGetMetaData ctrlGetMetaData;
        public LeftButtonUserControlMetadata left;

        public static string _selectDataType;
        public static metadatacatalognode_Mdl parentCatalogMdl;

        public static string groupCode = null;

        bool[] importEnable = new bool[] { false, false };

        public DataType dataType
        {
            get { return _dataType; }
            set
            {
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

                    if (ctrlbatchImportLst == null)
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
            //ctrlDisplayInfo1.Visible = false;
            left = new LeftButtonUserControlMetadata();
            this.groupControl1.Controls.Add(left);
            left.Dock = DockStyle.Fill;
            foreach (TreeView item in left.treeList)
            {
                item.AfterSelect += new TreeViewEventHandler(treeView1_AfterSelect);
            }
            treeView1 = left.treeView;
            foreach (var item in left.treeList)
            {
                item.NodeMouseClick += new TreeNodeMouseClickEventHandler(item_NodeMouseClick);
            }
        }

        void item_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            parentCatalogMdl = (metadatacatalognode_Mdl)e.Node.Tag;
            _selectDataType = e.Node.Text;
            groupCode = parentCatalogMdl.GROUP_CODE;
            if (e.Button == MouseButtons.Right)
            {

                //确定右键的位置  
                Point clickPoint = new Point(e.X, e.Y);
                //在确定后的位置上面定义一个节点  
                TreeNode treeNode = treeView1.GetNodeAt(clickPoint);
                treeNode.ContextMenuStrip = contextMenuStrip1;
                treeView1.SelectedNode = treeNode;
            }
            else
            {

            }

            //this.Cursor = Cursors.WaitCursor;
            //TreeSelete();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //获取方案名
            treeView1 = left.treeView;
            string schema = treeView1.SelectedNode.Name.Substring(0, 4);

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
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                textFolderPath.Text = fbd.SelectedPath;
                //解析要导入的项
                string[] dirArr = Directory.GetDirectories(textFolderPath.Text);
                for (int i = 0; i < dirArr.Length; i++)
                {
                    FileData fileData = importData.dataImportFactory.CreateFileData();
                    fileData.SetfilePath(dirArr[i]);
                    if (fileData.FileCheck()) //检核数据文件的完整性
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
            if (ofd.ShowDialog() == DialogResult.OK)
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
                    AddContrl2MainPanel((Control)ctrlGetMetaData);
                }
                catch (Exception ex)
                {
                    log.Error("读取元数据失败：" + ex.ToString());
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
                        log.Info(string.Format("入库项添加成功：{0}", temp.fileData.GetFilePath()));
                    }
                    catch (Exception ex)
                    {
                        log.Error(string.Format("入库项添加失败：{0}", temp.fileData.GetFilePath()));
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

        private void SetimportEnable(bool issingleImport, bool setValue)
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

        private void 添加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode selectNode = treeView1.SelectedNode;
            if (this.left.treeView.SelectedNode != null && this.left.treeView.SelectedNode.Nodes.Count > 0)
            {
                InputMessageDialog inputDialog = new InputMessageDialog();
                if (inputDialog.ShowDialog() == DialogResult.OK)
                {
                    this.left.treeView.SelectedNode.Nodes.Add(inputDialog.inputContent);
                    this.Refresh();
                    addNewNodeToMata(inputDialog.inputContent);
                }
            }
        }
        public void addNewNodeToMata(string addMetaDataName)
        {
            //找到子库名称
            SiteDb sitedb = TheUniversal.GetsubDbByCODE(parentCatalogMdl.GROUP_CODE.Substring(0, 4));

            metadatacatalognode_Mdl child = new metadatacatalognode_Mdl() { NAME = addMetaDataName, GROUP_TYPE = parentCatalogMdl.GROUP_TYPE };
            //执行元数据表创建脚本
            try
            {
                if (parentCatalogMdl.GROUP_TYPE == EnumDataKind.System_Vector.ToString())
                {
                    child.DATA_CODE = sitedb.tablecode.GetTableCode("prods_vector");
                }
            }
            catch (Exception ex)
            {

                XtraMessageBox.Show("元数据表创建失败" + ex.ToString());
                return;
            }

            // 维护数据类型树

            sitedb.AddMetadata(child, parentCatalogMdl);

            //刷新treeview
            TreeNode root = treeView1.SelectedNode;
            while (root.Parent != null)
            {
                root = root.Parent;
            }

            TreeNode tn1 = sitedb.GetDbNode();
            int nodeindex = treeView1.Nodes.IndexOf(root);
            treeView1.Nodes.RemoveAt(nodeindex);
            treeView1.Nodes.Insert(nodeindex, tn1);
            treeView1.ExpandAll();
            treeView1.SelectedNode = tn1;
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                SiteDb sitedb = TheUniversal.GetsubDbByCODE(parentCatalogMdl.GROUP_CODE.Substring(0, 4));
                metadatacatalognode_Mdl node = (metadatacatalognode_Mdl)treeView1.SelectedNode.Tag;

                System.Windows.Forms.DialogResult result = DevExpress.XtraEditors.XtraMessageBox.Show(this, "删除该节点，将失去该节点及其子节点所有数据，确定要删除" + node.NAME + "及其子节点？", "提示", System.Windows.Forms.MessageBoxButtons.YesNo);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    treeView1.Nodes.Remove(treeView1.SelectedNode);
                    sitedb.DeleteNode(node);
                }
            }
        }
    }
}
