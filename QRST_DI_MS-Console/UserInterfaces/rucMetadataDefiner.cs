using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraBars.Ribbon;
using QRST_DI_DS_Metadata.MetaDataDefiner;
using QRST_DI_DS_Metadata.MetaDataDefiner.Mdl;
using DevExpress.XtraEditors;
using QRST_DI_DS_Metadata;
using QRST_DI_DS_Basis.DBEngine;

namespace QRST_DI_MS_Console.UserInterfaces
{
    class rucMetadataDefiner : RibbonPageBaseUC
    {
        private DevExpress.XtraBars.BarEditItem barEditItemDataType;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
        private DevExpress.XtraBars.BarEditItem barEditItemDataName;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit2;
        private DevExpress.XtraBars.BarButtonItem btnCreateField;
        private DevExpress.XtraBars.BarButtonItem btnDeleteField;
        private DevExpress.XtraBars.BarButtonItem btnInsertField;
        private DevExpress.XtraBars.BarButtonItem btnKeyWord;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup3;
        private DevExpress.XtraBars.BarButtonItem btnSave;
        private DevExpress.XtraEditors.Repository.RepositoryItemRadioGroup repositoryItemRadioGroup1;
        private DevExpress.XtraBars.BarEditItem barEditItemCreateView;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
        private DevExpress.XtraBars.BarEditItem barEditItemDataFormat;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboDataFormat;


        private metadatacatalognode_Mdl parentCatalogMdl;

        private Dictionary<string, string> dicDataFormat;

        string mouldName = "";

             public rucMetadataDefiner()
            : base()
        {
            InitializeComponent();
        }

             public rucMetadataDefiner(object objMUC)
            : base(objMUC)
        {
            InitializeComponent();
                 ((mucMetadataDefiner) ObjMainUC).DataTypeChanged += SetTextType;
        }
      
        private void InitializeComponent()
        {
            this.barEditItemDataType = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.barEditItemDataName = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemTextEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.repositoryItemRadioGroup1 = new DevExpress.XtraEditors.Repository.RepositoryItemRadioGroup();
            this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.barEditItemDataFormat = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemComboDataFormat = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.btnCreateField = new DevExpress.XtraBars.BarButtonItem();
            this.btnDeleteField = new DevExpress.XtraBars.BarButtonItem();
            this.btnInsertField = new DevExpress.XtraBars.BarButtonItem();
            this.btnKeyWord = new DevExpress.XtraBars.BarButtonItem();
            this.barEditItemCreateView = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.ribbonPageGroup3 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.btnSave = new DevExpress.XtraBars.BarButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemRadioGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboDataFormat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
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
            this.barEditItemDataType,
            this.barEditItemDataName,
            this.btnCreateField,
            this.btnDeleteField,
            this.btnInsertField,
            this.btnKeyWord,
            this.btnSave,
            this.barEditItemCreateView,
            this.barEditItemDataFormat});
            this.ribbonControl1.MaxItemId = 23;
            this.ribbonControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemTextEdit1,
            this.repositoryItemTextEdit2,
            this.repositoryItemRadioGroup1,
            this.repositoryItemCheckEdit1,
            this.repositoryItemComboDataFormat});
            this.ribbonControl1.Size = new System.Drawing.Size(1050, 149);
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup2,
            this.ribbonPageGroup3});
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.barEditItemDataType);
            this.ribbonPageGroup1.ItemLinks.Add(this.barEditItemDataName);
            this.ribbonPageGroup1.Text = "定制条件";
            // 
            // barEditItemDataType
            // 
            this.barEditItemDataType.Caption = "数据类型";
            this.barEditItemDataType.Edit = this.repositoryItemTextEdit1;
            this.barEditItemDataType.EditValue = "QRST";
            this.barEditItemDataType.Id = 1;
            this.barEditItemDataType.Name = "barEditItemDataType";
            this.barEditItemDataType.Width = 200;
            // 
            // repositoryItemTextEdit1
            // 
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            this.repositoryItemTextEdit1.ReadOnly = true;
            // 
            // barEditItemDataName
            // 
            this.barEditItemDataName.Caption = "数据名称";
            this.barEditItemDataName.Edit = this.repositoryItemTextEdit2;
            this.barEditItemDataName.Id = 3;
            this.barEditItemDataName.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.保存1;
            this.barEditItemDataName.Name = "barEditItemDataName";
            this.barEditItemDataName.Width = 200;
            // 
            // repositoryItemTextEdit2
            // 
            this.repositoryItemTextEdit2.AutoHeight = false;
            this.repositoryItemTextEdit2.Name = "repositoryItemTextEdit2";
            // 
            // repositoryItemRadioGroup1
            // 
            this.repositoryItemRadioGroup1.Columns = 2;
            this.repositoryItemRadioGroup1.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "栅格产品"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "矢量产品"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "文档产品"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "表格产品 ")});
            this.repositoryItemRadioGroup1.Name = "repositoryItemRadioGroup1";
            // 
            // ribbonPageGroup2
            // 
            this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemDataFormat, true);
            this.ribbonPageGroup2.ItemLinks.Add(this.btnCreateField, true);
            this.ribbonPageGroup2.ItemLinks.Add(this.btnDeleteField, true);
            this.ribbonPageGroup2.ItemLinks.Add(this.btnInsertField, true);
            this.ribbonPageGroup2.ItemLinks.Add(this.btnKeyWord, true);
            this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemCreateView, true);
            this.ribbonPageGroup2.Name = "ribbonPageGroup2";
            this.ribbonPageGroup2.Text = "方案";
            // 
            // barEditItemDataFormat
            // 
            this.barEditItemDataFormat.Caption = "数据格式";
            this.barEditItemDataFormat.Edit = this.repositoryItemComboDataFormat;
            this.barEditItemDataFormat.EditValue = "";
            this.barEditItemDataFormat.Id = 22;
            this.barEditItemDataFormat.Name = "barEditItemDataFormat";
            this.barEditItemDataFormat.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barEditItemDataFormat.Width = 150;
            // 
            // repositoryItemComboDataFormat
            // 
            this.repositoryItemComboDataFormat.AutoHeight = false;
            this.repositoryItemComboDataFormat.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboDataFormat.Name = "repositoryItemComboDataFormat";
            this.repositoryItemComboDataFormat.EditValueChanged += new System.EventHandler(this.repositoryItemComboDataFormat_EditValueChanged);
            // 
            // btnCreateField
            // 
            this.btnCreateField.Caption = "创建字段";
            this.btnCreateField.Id = 14;
            this.btnCreateField.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.创建字段2;
            this.btnCreateField.Name = "btnCreateField";
            this.btnCreateField.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.btnCreateField.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnCreateField_ItemClick);
            // 
            // btnDeleteField
            // 
            this.btnDeleteField.Caption = "删除字段";
            this.btnDeleteField.Id = 15;
            this.btnDeleteField.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.删除字段;
            this.btnDeleteField.Name = "btnDeleteField";
            this.btnDeleteField.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.btnDeleteField.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnDeleteField_ItemClick);
            // 
            // btnInsertField
            // 
            this.btnInsertField.Caption = "插入字段";
            this.btnInsertField.Id = 16;
            this.btnInsertField.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.插入字段1;
            this.btnInsertField.Name = "btnInsertField";
            this.btnInsertField.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.btnInsertField.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnInsertField_ItemClick);
            // 
            // btnKeyWord
            // 
            this.btnKeyWord.Caption = "主键";
            this.btnKeyWord.Id = 17;
            this.btnKeyWord.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.主键1;
            this.btnKeyWord.Name = "btnKeyWord";
            this.btnKeyWord.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.btnKeyWord.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnKeyWord_ItemClick);
            // 
            // barEditItemCreateView
            // 
            this.barEditItemCreateView.Caption = "创建视图：";
            this.barEditItemCreateView.Edit = this.repositoryItemCheckEdit1;
            this.barEditItemCreateView.EditValue = true;
            this.barEditItemCreateView.Id = 21;
            this.barEditItemCreateView.Name = "barEditItemCreateView";
            this.barEditItemCreateView.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            // 
            // repositoryItemCheckEdit1
            // 
            this.repositoryItemCheckEdit1.AutoHeight = false;
            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            // 
            // ribbonPageGroup3
            // 
            this.ribbonPageGroup3.ItemLinks.Add(this.btnSave);
            this.ribbonPageGroup3.Name = "ribbonPageGroup3";
            this.ribbonPageGroup3.Text = "应用";
            // 
            // btnSave
            // 
            this.btnSave.Caption = "保存";
            this.btnSave.Id = 18;
            this.btnSave.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.保存3;
            this.btnSave.Name = "btnSave";
            this.btnSave.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.btnSave.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnSave_ItemClick);
            // 
            // rucMetadataDefiner
            // 
            this.Name = "rucMetadataDefiner";
            this.Size = new System.Drawing.Size(1050, 150);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemRadioGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboDataFormat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
            this.ResumeLayout(false);

        }

        private void barEditItemDataModual_EditValueChanged(object sender, EventArgs e)
        {
          //  MySqlDB sqlDB = new MySqlDB();
          // ((mucMetadataDefiner)ObjMainUC).ctrlTableManager1.InitializeCtrlWithTableObj();
          
        }

        private void btnCreateField_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ((mucMetadataDefiner)ObjMainUC).ctrlTableManager1.AddNewRow();
        }

        private void btnDeleteField_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ((mucMetadataDefiner)ObjMainUC).ctrlTableManager1.DeleteField();
        }

        private void btnInsertField_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ((mucMetadataDefiner)ObjMainUC).ctrlTableManager1.InsertField();
        }

        private void btnKeyWord_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ((mucMetadataDefiner)ObjMainUC).ctrlTableManager1.KeyClick();
        }

        /// <summary>
        /// 保存定制的元数据类型
        /// 0.检查各项输入的有效性
        /// 1.执行元数据表创建脚本
        /// 2.将表注册到tablecode
        /// 3.维护数据类型树
        /// 4.维护视图字段表
        /// 5.将数据类型写到prods_type中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Check())
            {
                //找到子库名称
                SiteDb sitedb = TheUniversal.GetsubDbByCODE(((mucMetadataDefiner)ObjMainUC).treeView1.SelectedNode.Name.Substring(0, 4));

                metadatacatalognode_Mdl child = new metadatacatalognode_Mdl() { NAME = barEditItemDataName.EditValue.ToString(), GROUP_TYPE = mouldName};
                //执行元数据表创建脚本
                try
                {
                    if (mouldName != EnumDataKind.System_Vector.ToString()&& mouldName != EnumDataKind.System_Tile.ToString())
                    {
                        List<string> sqlLst = new List<string>();
                        string sqlScript = ((mucMetadataDefiner)ObjMainUC).ctrlTableManager1.ConvertToScript();

                        TABLES_Mdl tablemdl = ((mucMetadataDefiner)ObjMainUC).ctrlTableManager1.tableObj;
                        if (string.IsNullOrEmpty(sqlScript))
                        {
                            return;
                        }
                        //检查表名是否有重复
                        if (sitedb.tablecode.ExistTable(tablemdl.TABLE_NAME))
                        {
                            XtraMessageBox.Show("该表已经存在！");
                            return;
                        }
                        sqlLst.Add(sqlScript);

                        //创建视图
                        if ((bool)barEditItemCreateView.EditValue)
                        {
                            string viewScript = ((mucMetadataDefiner)ObjMainUC).ctrlTableManager1.GetViewScript();
                            sqlLst.Add(viewScript);
                        }
                        //维护table_view表
                        sqlLst.Add(sitedb.tableview_Dal.GetDeleteScript(string.Format("TABLE_NAME = '{0}'", tablemdl.TABLE_NAME)));

                        TableLocker dblock = new TableLocker(sitedb.sqlUtilities);
                        dblock.LockTable("table_view");
                        int id = sitedb.sqlUtilities.GetMaxID("ID", "table_view");
                        List<table_view_Mdl> tableviewMdl = ((mucMetadataDefiner)ObjMainUC).ctrlTableManager1.GetTableViewMdl();
                        for (int i = 0 ; i < tableviewMdl.Count ; i++)
                        {
                            tableviewMdl[i].ID = id;
                            sqlLst.Add(sitedb.tableview_Dal.GetAddScript(tableviewMdl[i]));
                            id++;
                        }
                        //执行元数据表创建脚本
                        string errorMsg;
                        int successSql = sitedb.sqlUtilities.ExecuteSqlTran(sqlLst,out errorMsg);
                        dblock.UnlockTable("table_view");
                        //脚本执行错误
                        if (successSql == -1)
                        {
                            XtraMessageBox.Show("元数据表创建脚本执行错误，请确认表字段与视图创建定义合法！" );
                            //撤销之前完成的工作，mysql事务貌似无法撤销创建表操作，只能手动撤销了，悲催...
                            sitedb.sqlUtilities.ExecuteSql(tablemdl.DeleteScript());
                            sitedb.sqlUtilities.ExecuteSql(tablemdl.DeleteViewScript());
                            return;
                        }

                        //将表注册到tablecode 
                        tablecode_Mdl tablecodeMdl = new tablecode_Mdl() { TABLE_NAME = ((mucMetadataDefiner)ObjMainUC).ctrlTableManager1.GetTableName(), DESCRIPTION = ((mucMetadataDefiner)ObjMainUC).ctrlTableManager1.GetDescription() };
                        tablecodeMdl = sitedb.RegistTable(tablecodeMdl);
                        //将表注册到类型表Prod_中
                        sitedb.RegistProds(tablecodeMdl, mouldName);

                        child.DATA_CODE = tablecodeMdl.QRST_CODE;
                    }
                    else
                    {
                        if (mouldName == EnumDataKind.System_Vector.ToString())
                        {
                              child.DATA_CODE = sitedb.tablecode.GetTableCode("prods_vector");
                        }
                    }
                }
                catch (Exception ex)
                {

                    XtraMessageBox.Show("元数据表创建失败"+ex.ToString());
                    return; 
                }

                // 维护数据类型树
       
                sitedb.AddMetadata(child,parentCatalogMdl);
       
                //刷新treeview
                TreeNode root = ((mucMetadataDefiner)ObjMainUC).treeView1.SelectedNode;
                while (root.Parent != null)
                {
                    root = root.Parent;
                }

                TreeNode tn1 = sitedb.GetDbNode();
                  int nodeindex = ((mucMetadataDefiner) ObjMainUC).treeView1.Nodes.IndexOf(root);
                ((mucMetadataDefiner) ObjMainUC).treeView1.Nodes.RemoveAt(nodeindex);
                ((mucMetadataDefiner)ObjMainUC).treeView1.Nodes.Insert(nodeindex, tn1);
                 ((mucMetadataDefiner) ObjMainUC).treeView1.ExpandAll();
                ((mucMetadataDefiner) ObjMainUC).treeView1.SelectedNode = tn1;
                 
                XtraMessageBox.Show("元数据表创建成功！" );
                barEditItemDataName.EditValue = "";
              
            }
        }

        /// <summary>
        /// 检查元数据定制的各项输入的合法性
        /// </summary>
        /// <returns></returns>
        bool Check()
        {
            if (barEditItemDataName.EditValue == null ||string.IsNullOrEmpty(barEditItemDataName.EditValue.ToString()))
            {
                XtraMessageBox.Show("请输入数据名称！");
                return false;
            }
            if (string.IsNullOrEmpty(((mucMetadataDefiner)ObjMainUC).ctrlTableManager1.GetTableName()) && mouldName != EnumDataKind.System_Vector.ToString() && mouldName != EnumDataKind.System_Tile.ToString())
            {
                XtraMessageBox.Show("请输入数据表名称！");
                return false;
            }
      
            return true;
        }

        //另存为脚本
        private void btnSaveScript_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
          
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|SQL files (*.sql)|*.sql";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    // Code to write the stream goes here.
                    StreamWriter sw = new StreamWriter(myStream);
                    sw.Write("hhh");
                    myStream.Close();
                }
            }

        }

        /// <summary>
        /// 设置数据类型值,当主界面的元数据类型树selectedindex发生变化时促发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SetTextType(object sender, EventArgs e)
        {  
            TreeNode tn = ((mucMetadataDefiner)sender).treeView1.SelectedNode;
            parentCatalogMdl = (metadatacatalognode_Mdl) tn.Tag;
			if (parentCatalogMdl.IS_DATASET)
			{
				barEditItemDataType.EditValue = tn.FullPath;
			}
            else if (tn.Parent!=null)
			{
				parentCatalogMdl = (metadatacatalognode_Mdl)tn.Parent.Tag;
				barEditItemDataType.EditValue = tn.Parent.FullPath;
			}

            //更新数据格式模板
            SiteDb sitedb = TheUniversal.GetsubDbByCODE(((mucMetadataDefiner)sender).treeView1.SelectedNode.Name.Substring(0, 4));
            dicDataFormat = sitedb.tablecode.GetTableMoulds();
            repositoryItemComboDataFormat.Items.Clear();
            repositoryItemComboDataFormat.Items.AddRange(dicDataFormat.Keys);
           
        }

        /// <summary>
        /// 选择数据类型模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void repositoryItemRadioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    MySqlDB mySqlDb = new MySqlDB();
        //    TABLES_Mdl tablemdl = mySqlDb.GetTableModel("midb", "mould_raster");

        //    ((mucMetadataDefiner)ObjMainUC).ctrlTableManager1.InitializeCtrlWithTableObj(tablemdl,true);
            
        //}

        /// <summary>
        /// 选择的数据格式模板发生变化，重新加载相关数据模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void repositoryItemComboDataFormat_EditValueChanged(object sender, EventArgs e)
        {
            //
            string dataStr = ((ComboBoxEdit) sender).SelectedItem.ToString();
            string dataFormat="";
            switch(dataStr)
            {
                case "矢量数据": dataFormat = EnumDataKind.System_Vector.ToString();break;
                case "栅格数据": dataFormat = EnumDataKind.System_Raster.ToString(); break;
                case "文档数据": dataFormat = EnumDataKind.System_Document.ToString(); break;
                case "表格数据": dataFormat = EnumDataKind.System_Table.ToString(); break;
                case "切片数据": dataFormat = EnumDataKind.System_Tile.ToString(); break;
            }
            mouldName = dataFormat;
            string tableName;

            //选择矢量数据，不需要创建元数据表
            if (dataFormat == EnumDataKind.System_Vector.ToString()|| dataFormat == EnumDataKind.System_Tile.ToString())
            {
                ((mucMetadataDefiner)ObjMainUC).ctrlTableManager1.Enabled = false;
                ((mucMetadataDefiner)ObjMainUC).ctrlDisplayInfo1.Visible = true;
                ((mucMetadataDefiner)ObjMainUC).ctrlDisplayInfo1.Message = "不存在对应的表创建模板！";
                SetEditTable(false);
            }
            else
            {
                ((mucMetadataDefiner)ObjMainUC).ctrlDisplayInfo1.Visible = false;
                ((mucMetadataDefiner)ObjMainUC).ctrlTableManager1.Enabled = true;
                if (dicDataFormat.TryGetValue(dataStr, out tableName))
                {
                    SetEditTable(true);

                    SiteDb sitedb = TheUniversal.GetsubDbByCODE(((mucMetadataDefiner)ObjMainUC).treeView1.SelectedNode.Name.Substring(0, 4));
                    TABLES_Mdl tablemdl = sitedb.GetTableMdl(tableName);

                    ((mucMetadataDefiner)ObjMainUC).ctrlTableManager1.InitializeViewTable(sitedb.tableview_Dal.GetTableViewLstByTableName(tableName));
                    ((mucMetadataDefiner)ObjMainUC).ctrlTableManager1.InitializeCtrlWithTableObj(tablemdl, true);
                }
                else
                {
                    SetEditTable(false);
                    XtraMessageBox.Show("没有找到对应的模板！");
                }
            }
        }

        void SetEditTable(bool enable)
        {
            btnCreateField.Enabled = enable;
            btnInsertField.Enabled = enable;
            btnKeyWord.Enabled = enable;
            btnDeleteField.Enabled = enable;
            barEditItemCreateView.Enabled = enable;
        }
    }
}
