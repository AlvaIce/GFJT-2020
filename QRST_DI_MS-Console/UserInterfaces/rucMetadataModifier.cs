using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraEditors;
using QRST_DI_DS_Metadata.MetaDataDefiner;
using QRST_DI_DS_Metadata.MetaDataDefiner.Mdl;
using QRST_DI_DS_Metadata;
using QRST_DI_DS_Basis.DBEngine;

namespace QRST_DI_MS_Console.UserInterfaces
{
    class rucMetadataModifier : RibbonPageBaseUC
    {
        private DevExpress.XtraBars.BarEditItem barDataType;
        private DevExpress.XtraBars.BarEditItem barEditItemDataName;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
        private DevExpress.XtraBars.BarButtonItem barButtonItemCreateField;
        private DevExpress.XtraBars.BarButtonItem barButtonItemDeleteField;
        private DevExpress.XtraBars.BarButtonItem barButtonItemKey;
        private DevExpress.XtraBars.BarButtonItem barButtonItemSave;
        private DevExpress.XtraBars.BarButtonItem barButtonItemCreateDataType;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup3;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup4;
        private DevExpress.XtraBars.BarButtonItem barButtonItemMoveNode;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox1;
        private DevExpress.XtraBars.BarButtonItem barButtonItemDeleteNode;
        private DevExpress.XtraBars.BarEditItem barEditItem1;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEditType;
        private DevExpress.XtraBars.BarButtonItem barButtonItemSaveType;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup5;


        private SiteDb subDb;  //当前子库


        public rucMetadataModifier()
            : base()
        {
            InitializeComponent();


        }

        public rucMetadataModifier(object objMUC)
            : base(objMUC)
        {
            InitializeComponent();
            ((mucMetadataModifier)ObjMainUC).DataTypeChanged += TreeSelectedIndexChanged;

            //string sbDbCode = ((mucMetadataModifier) ObjMainUC).treeView1.Nodes[0].Name.Substring(0, 4);
            //subDb =TheUniversal.GetsubDbByCODE(sbDbCode);
        }

        private void InitializeComponent()
        {
            this.barDataType = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.barEditItemDataName = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.barButtonItemCreateField = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemDeleteField = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemKey = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPageGroup3 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.barButtonItemSave = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPageGroup4 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.barButtonItemCreateDataType = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemMoveNode = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemDeleteNode = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPageGroup5 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.barEditItem1 = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemTextEditType = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.barButtonItemSaveType = new DevExpress.XtraBars.BarButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEditType)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonControl1
            // 
            // 
            // 
            // 
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.ExpandCollapseItem.Name = "";
            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barDataType,
            this.barEditItemDataName,
            this.barButtonItemCreateField,
            this.barButtonItemDeleteField,
            this.barButtonItemKey,
            this.barButtonItemSave,
            this.barButtonItemCreateDataType,
            this.barButtonItemMoveNode,
            this.barButtonItemDeleteNode,
            this.barEditItem1,
            this.barButtonItemSaveType});
            this.ribbonControl1.MaxItemId = 18;
            this.ribbonControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemComboBox1,
            this.repositoryItemTextEdit1,
            this.repositoryItemTextEditType});
            this.ribbonControl1.Size = new System.Drawing.Size(1109, 149);
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup2,
            this.ribbonPageGroup3,
            this.ribbonPageGroup4,
            this.ribbonPageGroup5});
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.barDataType);
            this.ribbonPageGroup1.Text = "元数据类型";
            // 
            // barDataType
            // 
            this.barDataType.Edit = this.repositoryItemComboBox1;
            this.barDataType.Id = 1;
            this.barDataType.Name = "barDataType";
            this.barDataType.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barDataType.Width = 200;
            // 
            // repositoryItemComboBox1
            // 
            this.repositoryItemComboBox1.AutoHeight = false;
            this.repositoryItemComboBox1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox1.Name = "repositoryItemComboBox1";
            // 
            // ribbonPageGroup2
            // 
            this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemDataName);
            this.ribbonPageGroup2.ItemLinks.Add(this.barButtonItemCreateField, true);
            this.ribbonPageGroup2.ItemLinks.Add(this.barButtonItemDeleteField, true);
            this.ribbonPageGroup2.ItemLinks.Add(this.barButtonItemKey, true);
            this.ribbonPageGroup2.Name = "ribbonPageGroup2";
            this.ribbonPageGroup2.Text = "元数据修改";
            // 
            // barEditItemDataName
            // 
            this.barEditItemDataName.Caption = "数据名称";
            this.barEditItemDataName.Edit = this.repositoryItemTextEdit1;
            this.barEditItemDataName.Id = 2;
            this.barEditItemDataName.Name = "barEditItemDataName";
            this.barEditItemDataName.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barEditItemDataName.Width = 120;
            // 
            // repositoryItemTextEdit1
            // 
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            // 
            // barButtonItemCreateField
            // 
            this.barButtonItemCreateField.Caption = "创建字段";
            this.barButtonItemCreateField.Id = 4;
            this.barButtonItemCreateField.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.创建字段2;
            this.barButtonItemCreateField.Name = "barButtonItemCreateField";
            this.barButtonItemCreateField.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonItemCreateField.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemCreateField_ItemClick);
            // 
            // barButtonItemDeleteField
            // 
            this.barButtonItemDeleteField.Caption = "删除字段";
            this.barButtonItemDeleteField.Id = 5;
            this.barButtonItemDeleteField.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.删除字段;
            this.barButtonItemDeleteField.Name = "barButtonItemDeleteField";
            this.barButtonItemDeleteField.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonItemDeleteField.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemDeleteField_ItemClick);
            // 
            // barButtonItemKey
            // 
            this.barButtonItemKey.Caption = "主键";
            this.barButtonItemKey.Id = 6;
            this.barButtonItemKey.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.主键1;
            this.barButtonItemKey.Name = "barButtonItemKey";
            this.barButtonItemKey.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonItemKey.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemKey_ItemClick);
            // 
            // ribbonPageGroup3
            // 
            this.ribbonPageGroup3.ItemLinks.Add(this.barButtonItemSave, true);
            this.ribbonPageGroup3.Name = "ribbonPageGroup3";
            this.ribbonPageGroup3.Text = "保存";
            // 
            // barButtonItemSave
            // 
            this.barButtonItemSave.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.barButtonItemSave.Caption = "保存";
            this.barButtonItemSave.Id = 10;
            this.barButtonItemSave.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.保存3;
            this.barButtonItemSave.Name = "barButtonItemSave";
            this.barButtonItemSave.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonItemSave.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemSave_ItemClick);
            // 
            // ribbonPageGroup4
            // 
            this.ribbonPageGroup4.ItemLinks.Add(this.barButtonItemCreateDataType);
            this.ribbonPageGroup4.ItemLinks.Add(this.barButtonItemMoveNode, true);
            this.ribbonPageGroup4.ItemLinks.Add(this.barButtonItemDeleteNode, true);
            this.ribbonPageGroup4.Name = "ribbonPageGroup4";
            this.ribbonPageGroup4.Text = "类型管理";
            // 
            // barButtonItemCreateDataType
            // 
            this.barButtonItemCreateDataType.Caption = "创建数据类型";
            this.barButtonItemCreateDataType.Id = 12;
            this.barButtonItemCreateDataType.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.添加数据类型;
            this.barButtonItemCreateDataType.Name = "barButtonItemCreateDataType";
            this.barButtonItemCreateDataType.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonItemCreateDataType.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemCreateDataType_ItemClick);
            // 
            // barButtonItemMoveNode
            // 
            this.barButtonItemMoveNode.Caption = "移动节点";
            this.barButtonItemMoveNode.Id = 14;
            this.barButtonItemMoveNode.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.移动节点1;
            this.barButtonItemMoveNode.Name = "barButtonItemMoveNode";
            this.barButtonItemMoveNode.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonItemMoveNode.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemMoveNode_ItemClick);
            // 
            // barButtonItemDeleteNode
            // 
            this.barButtonItemDeleteNode.Caption = "删除节点";
            this.barButtonItemDeleteNode.Id = 15;
            this.barButtonItemDeleteNode.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.删除节点1;
            this.barButtonItemDeleteNode.Name = "barButtonItemDeleteNode";
            this.barButtonItemDeleteNode.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonItemDeleteNode.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemDeleteNode_ItemClick);
            // 
            // ribbonPageGroup5
            // 
            this.ribbonPageGroup5.ItemLinks.Add(this.barEditItem1);
            this.ribbonPageGroup5.ItemLinks.Add(this.barButtonItemSaveType, true);
            this.ribbonPageGroup5.Name = "ribbonPageGroup5";
            this.ribbonPageGroup5.Text = "新建数据类型";
            this.ribbonPageGroup5.Visible = false;
            // 
            // barEditItem1
            // 
            this.barEditItem1.Caption = "数据类型名：";
            this.barEditItem1.Edit = this.repositoryItemTextEditType;
            this.barEditItem1.Id = 16;
            this.barEditItem1.Name = "barEditItem1";
            this.barEditItem1.Width = 100;
            // 
            // repositoryItemTextEditType
            // 
            this.repositoryItemTextEditType.AutoHeight = false;
            this.repositoryItemTextEditType.Name = "repositoryItemTextEditType";
            this.repositoryItemTextEditType.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.repositoryItemTextEditType_EditValueChanging);
            // 
            // barButtonItemSaveType
            // 
            this.barButtonItemSaveType.Caption = "保存类型";
            this.barButtonItemSaveType.Enabled = false;
            this.barButtonItemSaveType.Id = 17;
            this.barButtonItemSaveType.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.保存更新;
            this.barButtonItemSaveType.Name = "barButtonItemSaveType";
            this.barButtonItemSaveType.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemSaveType_ItemClick);
            // 
            // rucMetadataModifier
            // 
            this.Name = "rucMetadataModifier";
            this.Size = new System.Drawing.Size(1109, 150);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEditType)).EndInit();
            this.ResumeLayout(false);

        }

        /// <summary>
        /// 当muctreeview索引变化时发生
        /// 1.更改元数据类型barDataType
        /// 2.如果选中的是非表节点，则禁用‘元数据修改‘
        /// 3.如果是表节点，则加载该节点对应的表结构
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void TreeSelectedIndexChanged(object sender, EventArgs e)
        {
            metadatacatalognode_Mdl metadatacatalognode_mdl = (metadatacatalognode_Mdl)((mucMetadataModifier)ObjMainUC).treeView1.SelectedNode.Tag;

            string sbDbCode = ((mucMetadataModifier)ObjMainUC).treeView1.SelectedNode.Name.Substring(0, 4);
            subDb = TheUniversal.GetsubDbByCODE(sbDbCode);
            //更改元数据类型barDataType
            if (metadatacatalognode_mdl.IS_DATASET)
            {
                barDataType.EditValue = ((mucMetadataModifier)ObjMainUC).treeView1.SelectedNode.FullPath;
                barEditItemDataName.EditValue = "";
                barEditItemDataName.Enabled = false;
                barButtonItemCreateField.Enabled = false;
                barButtonItemDeleteField.Enabled = false;
                barButtonItemSave.Enabled = false;
            //    barButtonItemSaveAs.Enabled = false;
                barButtonItemKey.Enabled = false;
                ((mucMetadataModifier)ObjMainUC).ctrlTableManager1.ClearCtrl();
            }
            else
            {
                barDataType.EditValue = ((mucMetadataModifier)ObjMainUC).treeView1.SelectedNode.Parent.FullPath;
                barEditItemDataName.EditValue = ((mucMetadataModifier)ObjMainUC).treeView1.SelectedNode.Text;
                if (metadatacatalognode_mdl.GROUP_TYPE != EnumDataKind.System_Vector.ToString() && metadatacatalognode_mdl.GROUP_TYPE != EnumDataKind.System_Tile.ToString())
                {
                    barEditItemDataName.Enabled = true;
                    barButtonItemCreateField.Enabled = true;
                    barButtonItemDeleteField.Enabled = true;

                    barButtonItemSave.Enabled = true;
                    //       barButtonItemSaveAs.Enabled = true;
                    barButtonItemKey.Enabled = true;
                 
                }
                else
                {
                    if (metadatacatalognode_mdl.GROUP_TYPE == EnumDataKind.System_Tile.ToString())
                    {
                        ((mucMetadataModifier)ObjMainUC).ctrlTableManager1.ClearCtrl();
                        return;
                    }

                    barEditItemDataName.Enabled = false;
                    barButtonItemCreateField.Enabled = false;
                    barButtonItemDeleteField.Enabled = false;
                    barButtonItemSave.Enabled = false;
                    //    barButtonItemSaveAs.Enabled = false;
                    barButtonItemKey.Enabled = false;
                }
          
                string tablecode = metadatacatalognode_mdl.DATA_CODE;
                try
                {
                    TABLES_Mdl tablemdl = subDb.GetTableMdlByCode(tablecode);
                    ((mucMetadataModifier)ObjMainUC).ctrlTableManager1.InitializeCtrlWithTableObj(tablemdl, false);
                    ((mucMetadataModifier)ObjMainUC).ctrlTableManager1.InitializeViewTable(subDb.tableview_Dal.GetTableViewLstByViewName(tablemdl.VIEWNAME));
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show(ex.ToString());
                }

            }


        }

        /// <summary>
        /// 添加主键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemKey_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ((mucMetadataModifier)ObjMainUC).ctrlTableManager1.KeyClick();
        }

        /// <summary>
        /// 保存对表结构的修改
        /// 1. 检查数据名称的有效性
        /// 2.执行元数据修改脚本
        /// 3.将数据名称修改到metadatacatalognode中
        /// 4.更新元数据树结构
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Check())
            {
                List<string> sqlLst = new List<string>();
                try
                {
                    //表结构修改脚本
                    TABLES_Mdl newTableMdl = new TABLES_Mdl();
                    string alterScript = ((mucMetadataModifier)ObjMainUC).ctrlTableManager1.ConvertToAlterSQL(out newTableMdl);
                   string errorMsg;
                    if (!string.IsNullOrEmpty(alterScript))
                    {
                       // subDb.sqlUtilities.ex
                         sqlLst.Add(alterScript);
                        int count= subDb.sqlUtilities.ExecuteSqlTran(sqlLst, out errorMsg);
                        if (count ==-1)
                        {
                            XtraMessageBox.Show("元数据表结构修改失败，请确认表修改脚本是否合法！");
                            return;
                        }
                        else
                        {
                            sqlLst.Clear();
                        }
                    } 
                    //视图创建脚本
                    sqlLst.Add(((mucMetadataModifier)ObjMainUC).ctrlTableManager1.GetViewScript());
                    //修改table_view脚本,删除原有字段视图记录，增加新的字段视图记录
                    sqlLst.Add(subDb.tableview_Dal.GetDeleteScript(string.Format("TABLE_NAME = '{0}'", newTableMdl.TABLE_NAME)));

                    TableLocker dblock = new TableLocker(subDb.sqlUtilities);
                    dblock.LockTable("table_view"); 
                    int id = subDb.sqlUtilities.GetMaxID("ID", "table_view");
                    List<table_view_Mdl> tableviewMdl = ((mucMetadataModifier)ObjMainUC).ctrlTableManager1.GetTableViewMdl();
                    for (int i = 0 ; i < tableviewMdl.Count ; i++)
                    {
                        tableviewMdl[i].ID = id;
                        sqlLst.Add(subDb.tableview_Dal.GetAddScript(tableviewMdl[i]));
                        id++;
                    }
                    //执行事务，修改元数据表与视图
                 
                 int successCount =  subDb.sqlUtilities.ExecuteSqlTran(sqlLst,out errorMsg);
                 dblock.UnlockTable("table_view");
                 if (successCount == -1)
                 {
                     XtraMessageBox.Show("视图修改失败，请确认视图脚本是否合法！");
                     return;
                 }
                    string dataName = barEditItemDataName.EditValue.ToString();
                    if (!(dataName == ((mucMetadataModifier)ObjMainUC).treeView1.SelectedNode.Text))
                    {
                        metadatacatalognode_Mdl mdl = (metadatacatalognode_Mdl)((mucMetadataModifier)ObjMainUC).treeView1.SelectedNode.Tag;
                        mdl.NAME = dataName;
                        subDb.metadatacatalognode.Update(mdl);
                        //更新元数据树
                        ((mucMetadataModifier)ObjMainUC).treeView1.SelectedNode.Text = dataName;
                    }
                    //重新加载表结构
                    TABLES_Mdl tablemdl = subDb.GetTableMdl(newTableMdl.TABLE_NAME);
                    ((mucMetadataModifier)ObjMainUC).ctrlTableManager1.InitializeCtrlWithTableObj(tablemdl, false);
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show("元数据表更新失败," + ex.ToString());
                    return;
                }
                XtraMessageBox.Show("元数据表更新成功！");
            }
        }

        private bool Check()
        {
            if (barEditItemDataName.EditValue == null || string.IsNullOrEmpty(barEditItemDataName.EditValue.ToString()))
            {
                XtraMessageBox.Show("请输入数据名称！");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 将表结构另存为脚本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemSaveAs_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        /// <summary>
        /// 创建数据类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemCreateDataType_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ribbonPageGroup5.Visible = true;
        }

        /// <summary>
        /// 移动元数据树节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemMoveNode_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        /// <summary>
        /// 创建字段
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemCreateField_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ((mucMetadataModifier)ObjMainUC).ctrlTableManager1.AddNewRow();
        }

        /// <summary>
        /// 删除当前选中字段
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemDeleteField_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ((mucMetadataModifier)ObjMainUC).ctrlTableManager1.DeleteField();
        }

        /// <summary>
        /// 删除数据节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemDeleteNode_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            metadatacatalognode_Mdl node = (metadatacatalognode_Mdl)((mucMetadataModifier)ObjMainUC).treeView1.SelectedNode.Tag;
            
            System.Windows.Forms.DialogResult result = DevExpress.XtraEditors.XtraMessageBox.Show(this, "删除该节点，将失去该节点及其子节点所有数据，确定要删除" + node.NAME + "及其子节点？", "提示", System.Windows.Forms.MessageBoxButtons.YesNo);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                subDb.DeleteNode(node);
                ((mucMetadataModifier)ObjMainUC).RefreshTree();
            }
           
        }


        string dataName = "";
        private void repositoryItemTextEditType_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.NewValue.ToString()))
            {
                barButtonItemSaveType.Enabled = true;
            }
            else
            {
                barButtonItemSaveType.Enabled = false;
            }
            dataName = e.NewValue.ToString();
        }
        
        /// <summary>
        /// 保存添加的数据类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemSaveType_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!string.IsNullOrEmpty(dataName))
            {
                metadatacatalognode_Mdl fatherNode ;
                if (((metadatacatalognode_Mdl)((mucMetadataModifier)ObjMainUC).treeView1.SelectedNode.Tag).IS_DATASET)
                {
                    fatherNode = (metadatacatalognode_Mdl)((mucMetadataModifier)ObjMainUC).treeView1.SelectedNode.Tag;
                }
                else
                    fatherNode = (metadatacatalognode_Mdl)((mucMetadataModifier)ObjMainUC).treeView1.SelectedNode.Parent.Tag;
                metadatacatalognode_Mdl childNode = new metadatacatalognode_Mdl() {NAME = dataName, GROUP_TYPE = EnumDataKind.System_DataSet.ToString()};
                subDb.AddMetadata(childNode,fatherNode);
                ribbonPageGroup5.Visible = false;
                ((mucMetadataModifier)ObjMainUC).RefreshTree();
            }

            
        }
    }
}
