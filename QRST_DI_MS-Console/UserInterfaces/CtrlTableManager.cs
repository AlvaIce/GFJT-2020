using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using QRST_DI_DS_Basis.DBEngine;
using QRST_DI_DS_Metadata.MetaDataDefiner;
using QRST_DI_DS_Metadata.MetaDataDefiner.Mdl;

namespace QRST_DI_MS_Console.UserInterfaces
{
    public partial class CtrlTableManager : DevExpress.XtraEditors.XtraUserControl
    {
        private bool isCreate = true;    //默认的操作是创建表
        public TABLES_Mdl tableObj;  //表结构对象
        private List<string> alterScript = new List<string>();  //表结构的修改脚本
        private DataTable columnDT;      //存储列结构的表
        private DataTable viewDT;           //存储视图结构的表

        public CtrlTableManager()
        {
            InitializeComponent();
          //  mySqlDb = new MySqlDB();
            InitialColumns();
            gridControl1.DataSource = columnDT;
            gridControlView.DataSource = viewDT;
        }

        private void CtrlTableManager_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        ///初始化列表结构 
        /// </summary>
        void InitialColumns()
        {
            columnDT = new DataTable();
            viewDT = new DataTable();
            DataColumn columnName = new DataColumn { ColumnName = "COLUMN_NAME", DataType = Type.GetType("System.String") };
            columnDT.Columns.Add(columnName);

            DataColumn dataTypeName = new DataColumn { ColumnName = "COLUMN_TYPE", DataType = Type.GetType("System.String") };
            columnDT.Columns.Add(dataTypeName);

            DataColumn isKeyName = new DataColumn { ColumnName = "KEY", DataType = Type.GetType("System.Boolean") };
            columnDT.Columns.Add(isKeyName);

            DataColumn NN = new DataColumn { ColumnName = "NN", DataType = Type.GetType("System.Boolean") };
            columnDT.Columns.Add(NN);

            DataColumn BIN = new DataColumn { ColumnName = "BIN", DataType = Type.GetType("System.Boolean") };
            columnDT.Columns.Add(BIN);

            DataColumn UN = new DataColumn { ColumnName = "UN", DataType = Type.GetType("System.Boolean") };
            columnDT.Columns.Add(UN);

            DataColumn ZF = new DataColumn { ColumnName = "ZF", DataType = Type.GetType("System.Boolean") };
            columnDT.Columns.Add(ZF);

            DataColumn AI = new DataColumn { ColumnName = "AI", DataType = Type.GetType("System.Boolean") };
            columnDT.Columns.Add(AI);

            //DataColumn VIEWNAME = new DataColumn { ColumnName = "VIEWNAME", DataType = Type.GetType("System.String") };
           // columnDT.Columns.Add(VIEWNAME);

            DataColumn COMMENT = new DataColumn { ColumnName = "COMMENT", DataType = Type.GetType("System.String") };
            columnDT.Columns.Add(COMMENT);

            DataColumn DEFAULTVALUE = new DataColumn { ColumnName = "DEFAULTVALUE", DataType = Type.GetType("System.String") };
            columnDT.Columns.Add(DEFAULTVALUE);

            DataColumn COLID = new DataColumn { ColumnName = "COLID", DataType = Type.GetType("System.Int32") };
            columnDT.Columns.Add(COLID);      //用于标记列
            //DataColumn columnName1 = new DataColumn { ColumnName = "COLUMN_NAME", DataType = Type.GetType("System.String") };
            //viewDT.Columns.Add(columnName1);
            //viewDT.Columns.Add(VIEWNAME);
        }

        /// <summary>
        /// 如果是修改表结构，则用一个表结构实例初始化组件，将该实例信息显示在组件上
        /// </summary>
        /// <param name="mdl"></param>
        public void InitializeCtrlWithTableObj(TABLES_Mdl mdl,bool iscreate)
        {
            contextMenuStrip1.Enabled = true;
            isCreate = iscreate;
            alterScript.Clear();
            if(!isCreate)
            {
                textTableName.Properties.ReadOnly = true;
                memoTableComment.Properties.ReadOnly = true;
                cmbEngine.Properties.ReadOnly = true;
                cmbCharSet.Properties.ReadOnly = true;
                textTableName.Properties.ReadOnly = true;
                textTableName.Text = mdl.TABLE_NAME;
            }
            else
            {
                //cmbCharSet.Properties.Items.AddRange(mySqlDb.GetCharSet());
                //cmbEngine.Properties.Items.AddRange(mySqlDb.GetEngineLst());
                 textTableName.Text = "";
                //cmbCharSet.SelectedIndex = 0;
                //cmbEngine.SelectedIndex = 0;
            }

            tableObj = mdl;
         
            memoTableComment.Text = mdl.TABLE_COMMENT;
          //  labelSchema.Text = mdl.TABLE_SCHEMA;
            //写方案名
            cmbCharSet.Text = mdl.DEFAULTCHARSET;
            cmbEngine.Text = mdl.ENGINE;

            //初始化字段列表
            columnDT.Rows.Clear();
            foreach (COLUMNS_Mdl colMdl in mdl.columns)
            {
                AddNewRow(colMdl);
            }

            //初始化主键
            for (int i = 0 ; i < columnDT.Rows.Count ; i++)
            {
                if (mdl.keyColumns.Contains(columnDT.Rows[i]["COLUMN_NAME"].ToString()))
                {
                    columnDT.Rows[i]["KEY"] = true;
                }
            }
            if (bandedGridView1.FocusedRowHandle < 0)
            {
                return;
            }

            textComment.Text = columnDT.Rows[bandedGridView1.FocusedRowHandle]["comment"].ToString();
            textDefaultValue.Text = columnDT.Rows[bandedGridView1.FocusedRowHandle]["DEFAULTVALUE"].ToString();
        }

        /// <summary>
        /// 初始化视图信息
        /// </summary>
        public void InitializeViewTable(DataTable dt)
        {
            gridControlView.DataSource = dt;
            gridControlView.RefreshDataSource();
        }

        /// <summary>
        /// 添加新列
        /// </summary>
        public void AddNewRow()
        {
            DataRow dr = columnDT.NewRow();

            dr["COLUMN_NAME"] = "new_columns";
            dr["COLUMN_TYPE"] = "varchar(45)";
            dr["KEY"] = false;
            dr["NN"] = false;
            dr["BIN"] = false;
            dr["UN"] = false;
            dr["ZF"] = false;
            dr["AI"] = false;
          //  dr["VIEWNAME"] = "";
            dr["COMMENT"] = "";
            dr["DEFAULTVALUE"] = "";
            dr["COLID"] = -1;
       
            columnDT.Rows.Add(dr);
            gridControl1.RefreshDataSource();
            bandedGridView1.FocusedRowHandle = columnDT.Rows.Count - 1;
        }

        /// <summary>
        /// 用列对象实例化一行
        /// </summary>
        /// <param name="colMdl"></param>
        private void AddNewRow(COLUMNS_Mdl colMdl)
        {

            DataRow dr = columnDT.NewRow();

            dr["COLUMN_NAME"] = colMdl.COLUMN_NAME;
            dr["COLUMN_TYPE"] = colMdl.COLUMN_TYPE.Split(" ".ToCharArray())[0];
            dr["KEY"] = colMdl.ISKEY;
            dr["NN"] = (colMdl.IS_NULLABLE == "NO");
            dr["BIN"] = false;
            dr["UN"] = colMdl.COLUMN_TYPE.Contains("unsigned");
            dr["ZF"] = colMdl.COLUMN_TYPE.Contains("zerofill");
            dr["AI"] = colMdl.EXTRA.Contains("AUTO_INCREMENT");

            // dr["VIEWNAME"] = dr[0];
            dr["COMMENT"] = colMdl.COLUMN_COMMENT;
            textComment.Text = colMdl.COLUMN_COMMENT;
            dr["DEFAULTVALUE"] = colMdl.COLUMN_DEFAULT;
            textDefaultValue.Text = colMdl.COLUMN_DEFAULT;
            dr["COLID"] = colMdl.COLID;
           // dr["VIEWNAME"] = colMdl.VIEW_NAME;
            columnDT.Rows.Add(dr);

            //添加视图行
            //DataRow drView = viewDT.NewRow();
            //drView["COLUMN_NAME"] = colMdl.COLUMN_NAME;
            //drView["VIEWNAME"] = colMdl.VIEW_NAME;
            //gridControl1.RefreshDataSource();
        }

        private void bandedGridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        /// <summary>
        /// 将创建的表生成SQL脚本
        /// </summary>
        /// <returns></returns>
        public string ConvertToScript()
        {
            if (!isCreate)
            {
                return "";
            }
                tableObj = GetTableObj();
                if (tableObj != null)
                {
                    string str = tableObj.CovertToSQLScript();
                    return str;
                }
                return "";
        }

        /// <summary>
        /// 将修改表生成SQL语句
        /// </summary>
        /// <param name="colID"></param>
        /// <returns></returns>
        public string ConvertToAlterSQL(out TABLES_Mdl newTableMdl)
        {
            //保存列对象
            List<COLUMNS_Mdl> oldcolLst = new List<COLUMNS_Mdl>();
            oldcolLst.AddRange(tableObj.columns);

            StringBuilder alterSb = new StringBuilder();
            if (isCreate)
            {
                newTableMdl = tableObj;
                return "";
            }
            //检核原表对象与修改后的表对象的不同之处
            //检核范围仅仅限于列的修改，如添加字段、删除字段、修改字段类型、列名等
            newTableMdl = GetTableObj();
            if (newTableMdl == null)
            {
                return "";
            }

            for (int i = 0; i < newTableMdl.columns.Count; i++)
            {
                //新添加的列
                if (newTableMdl.columns[i].COLID < 0)
                {
                    alterSb.AppendFormat("alter table {0} add {1} ", tableObj.TABLE_NAME, newTableMdl.columns[i].ConvertToAlterScript());
                }
                else //检核原有列是否有修改
                {
                    COLUMNS_Mdl oldCol = tableObj.GetColumnByColID(newTableMdl.columns[i].COLID);
                    if (!oldCol.Equailvant(newTableMdl.columns[i]))  //该列有修改,添加修改语句
                    {
                        alterSb.AppendFormat("ALTER TABLE {0} CHANGE {1} {2}  ", tableObj.TABLE_NAME, oldCol.COLUMN_NAME, newTableMdl.columns[i].ConvertToAlterScript());
                    }
                    tableObj.columns.Remove(oldCol);     //将比较后的列移去
                }
            }

            //原有table对象中多余的列全部是新对象中删除的列，因此应该添加删除语句
            for (int i = 0; i < tableObj.columns.Count; i++)
            {
                //添加列删除语句
                alterSb.AppendFormat("ALTER TABLE {0} DROP {1}  ;", tableObj.TABLE_NAME, tableObj.columns[i].COLUMN_NAME);
            }

            //还原列对象
            tableObj.columns.Clear();
            tableObj.columns.AddRange(oldcolLst);

           return alterSb.ToString();
        }

    

        private COLUMNS_Mdl GetColumnByID(int colID)
        {
            for (int i = 0 ; i < tableObj.columns.Count ; i++)
            {
                if (tableObj.columns[i].COLID == colID)
                {
                    return tableObj.columns[i];
                }
            }
            return null;
        }

        /// <summary>
        /// 获取列对象
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private COLUMNS_Mdl GetColumnObj(DataRow dr)
        {
            //string numType = "INTDOUBLEFLOAT";
            //string charType = "TEXTCHAR";

            COLUMNS_Mdl mdl = new COLUMNS_Mdl();
            if (dr != null)
            {
                mdl.COLUMN_NAME = dr["COLUMN_NAME"].ToString();
                mdl.COLUMN_TYPE = dr["COLUMN_TYPE"].ToString();
                mdl.COLUMN_COMMENT = dr["COMMENT"].ToString();
                mdl.COLUMN_DEFAULT = dr["DEFAULTVALUE"].ToString();
           //     mdl.VIEW_NAME = dr["VIEWNAME"].ToString();
                if ((bool)dr["KEY"])
                {
                    mdl.ISKEY = true;
                }
                else
                {
                    mdl.ISKEY = false;
                }

                if ((bool)dr["NN"])
                {
                    mdl.IS_NULLABLE = "NO";
                }
                else
                {
                    mdl.IS_NULLABLE = "YES";
                }
                mdl.EXTRA = "";
                mdl.COLID = int.Parse(dr["COLID"].ToString());
                if (mdl.COLUMN_TYPE.Contains("int") || mdl.COLUMN_TYPE.Contains("double") || mdl.COLUMN_TYPE.Contains("float"))
                {
                    StringBuilder sb = new StringBuilder("");
                    if ((bool)dr["UN"])
                    {
                        sb.Append(" unsigned");
                    }
                    if ((bool)dr["ZF"])
                    {
                        sb.Append(" zerofill");
                    }
                    mdl.COLUMN_TYPE = mdl.COLUMN_TYPE + sb.ToString();
                    
                    if ((bool)dr["AI"])
                    {
                        mdl.EXTRA = "AUTO_INCREMENT";
                    }
                }
                //else if (mdl.COLUMN_TYPE.Contains("TEXT") || mdl.COLUMN_TYPE.Contains("CHAR") )
                //{

                //}
                return mdl;
            }
            return null;
        }
        /// <summary>
        /// 获取创建视图的脚本
        /// </summary>
        public string GetViewScript()
        {
            if (!Check())
            {
                return "";
            }
            viewDT = (DataTable)gridControlView.DataSource; 
            StringBuilder sbColumnLst = new StringBuilder("");
            StringBuilder sbFieldLst = new StringBuilder("");

            List<string> fieldNames = new List<string>();
            List<string> alias = new List<string>();


            for (int i = 0 ; i < viewDT.Rows.Count ; i++)
            {
                if (viewDT.Rows[i].RowState == DataRowState.Modified)
                {
                    continue;
                }

                if ((!(viewDT.Rows[i]["VIEWNAME"].ToString().Trim() == "")) && (!(viewDT.Rows[i]["COLUMN_NAME"].ToString().Trim() == "")))
                {
                    fieldNames.Add(viewDT.Rows[i]["COLUMN_NAME"].ToString());
                    alias.Add(viewDT.Rows[i]["VIEWNAME"].ToString());

                }
            }
            for (int i = 0 ; i < fieldNames.Count ; i++)
            {
                if (i != fieldNames.Count - 1)
                {
                 //   sbColumnLst.Append(alias[i] + ",");
                    string str = string.Format(" {0} as {1},", fieldNames[i], alias[i]);
                    sbFieldLst.Append(str);
                }
                else
                {
                    //sbColumnLst.Append(alias[i]);
                    //sbFieldLst.Append(fieldNames[i]);
                    string str = string.Format(" {0} as {1} ", fieldNames[i], alias[i]);
                    sbFieldLst.Append(str);
                }
            }

            string viewName = textTableName.Text + "_" + "VIEW";
            if (fieldNames.Count == 0)
            {
                return "";
            }
            return string.Format("create or replace view {0} as select {1} from {2}", viewName.Trim(), sbFieldLst.ToString(), textTableName.Text);
        }

        //从表格中的设置构造表结构对象
        public TABLES_Mdl GetTableObj()
        {
            TABLES_Mdl tableMdl = new TABLES_Mdl();

            if (Check())
            {
                tableMdl.TABLE_NAME = textTableName.Text;
                tableMdl.TABLE_SCHEMA = labelSchema.Text;
                tableMdl.ENGINE = cmbEngine.Text;
                tableMdl.DEFAULTCHARSET = cmbCharSet.Text;
                tableMdl.TABLE_COMMENT = memoTableComment.Text;

                //获取列值
                for (int i = 0 ; i < columnDT.Rows.Count ; i++)
                {
                    tableMdl.columns.Add(GetColumnObj(columnDT.Rows[i]));
                }
                //获取主键
                tableMdl.keyColumns = GetKeyLst();

                return tableMdl;
            }

            return null;
        }

        bool Check()
        {
            bandedGridView1.CloseEditor();
            bandedGridView1.UpdateCurrentRow();

            gridView1.CloseEditor();
            gridView1.UpdateCurrentRow();

            if (textTableName.Text.Trim() == "")
            {
                XtraMessageBox.Show("请输入数据库表名！");
                textTableName.Focus();
                return false;
            }
            string dupCol;
            if (IsColDuplicated("COLUMN_NAME", out dupCol))
            {
                XtraMessageBox.Show(string.Format("字段名'{0}'重复出现！", dupCol));
                return false;
            }
            if (IsColDuplicated("VIEWNAME", out dupCol))
            {
                XtraMessageBox.Show(string.Format("视图名'{0}'重复出现！", dupCol));
                return false;
            }
            
            return true;
        }

        bool IsColDuplicated(string colName,out string col)
        {
            viewDT = (DataTable)gridControlView.DataSource;
            for (int i = 0 ; i < viewDT.Rows.Count - 1 ; i++)
            {
                //if (viewDT.Rows[i].RowState == DataRowState.Deleted)
                //{
                //    continue;
                //}
                for (int j = i + 1 ; j < viewDT.Rows.Count ; j++)
                {
                    //if (viewDT.Rows[j].RowState == DataRowState.Deleted)
                    //{
                    //    continue;
                    //}
                        string str1 = viewDT.Rows[i][colName].ToString();
                        string str2 = viewDT.Rows[j][colName].ToString();
                        if (str1 == str2 && !string.IsNullOrEmpty(str1))
                        {
                            try
                            {
                                col = columnDT.Rows[i][colName].ToString();
                                return true;
                            }
                            catch (ArgumentException)
                            {
                                col = "";
                                return true; 
                            }
                        }
                }
            }
            col = "";
            return false;
        }

        /// <summary>
        /// 获取主键列表
        /// </summary>
        /// <returns></returns>
        List<string> GetKeyLst()
        {
            List<string> keyLst = new List<string>();
            for (int i = 0 ; i < columnDT.Rows.Count ; i++)
            {
                if ((bool)columnDT.Rows[i]["KEY"])
                {
                    keyLst.Add(columnDT.Rows[i]["COLUMN_NAME"].ToString());
                }
            }

            return keyLst;
        }
        
        /// <summary>
        /// 添加单元格变化的验证
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bandedGridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.Name == "gridColDataType")
            {
                //if (!repositoryItemComboBoxDataType.Items.Contains(e.Value.ToString()))
                //{
                //    XtraMessageBox.Show(string.Format("没有给定的数据类型'{0}'", e.Value.ToString()));
                //    columnDT.Rows[e.RowHandle]["COLUMN_TYPE"] = "varchar(45)";
                //}
            }
   
        }

        private void bandedGridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                if (e.FocusedRowHandle < 0 || e.FocusedRowHandle >= columnDT.Rows.Count || e.PrevFocusedRowHandle < 0 || columnDT.Rows[e.PrevFocusedRowHandle]["COLID"] == null)
                {
                    return;
                }
            }
            catch (RowNotInTableException)
            {

                return;
            }

            textComment.Text=columnDT.Rows[e.FocusedRowHandle]["COMMENT"].ToString()  ;
            textDefaultValue.Text = columnDT.Rows[e.FocusedRowHandle]["DEFAULTVALUE"].ToString();
            ////检核修改
            //DataRow dr = columnDT.Rows[e.PrevFocusedRowHandle];
            //int colID = (int)columnDT.Rows[e.PrevFocusedRowHandle]["COLID"];
            //if (!isCreate && colID >= 0)
            //{
            //    COLUMNS_Mdl oldCol = GetColumnByID(colID);
            //    COLUMNS_Mdl newCol = GetColumnObj(columnDT.Rows[e.PrevFocusedRowHandle]);
            //    if (!newCol.Equailvant(oldCol))
            //    {
            //        string alterStr = string.Format("ALTER TABLE {0} CHANGE {1} {2}  ", tableObj.TABLE_NAME, oldCol.COLUMN_NAME, newCol.ConvertToAlterScript());
            //        alterScript.Add(alterStr);
            //    }
            //}

        }

        private void repositoryItemCheckEditAI_CheckedChanged(object sender, EventArgs e)
        {
            string dataType = columnDT.Rows[bandedGridView1.FocusedRowHandle]["COLUMN_TYPE"].ToString();
            if ((dataType.Contains("int") || dataType.Contains("float") || dataType.Contains("double")) && ((CheckEdit)sender).CheckState == CheckState.Checked)
            {
                columnDT.Rows[bandedGridView1.FocusedRowHandle]["DEFAULTVALUE"] = "";
                textDefaultValue.Text = "";
            }
        }

        private void textDefaultValue_EditValueChanged(object sender, EventArgs e)
        {
            if (bandedGridView1.FocusedRowHandle < 0)
            {
                return;
            }
            columnDT.Rows[bandedGridView1.FocusedRowHandle]["DEFAULTVALUE"] = textDefaultValue.Text;
            columnDT.Rows[bandedGridView1.FocusedRowHandle]["AI"] = false;
        }

        private void repositoryItemCheckEditPK_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckEdit)sender).CheckState == CheckState.Checked)
            {
                columnDT.Rows[bandedGridView1.FocusedRowHandle]["NN"] = true;
            }
        }

        private void ToolStripMenuItemAddField_Click(object sender, EventArgs e)
        {
            AddNewRow();
        }

        private void ToolStripMenuItemDeleteField_Click(object sender, EventArgs e)
        {
            DeleteField();
            if (columnDT.Rows.Count == 0)
            {
                ToolStripMenuItemDeleteField.Enabled = false;
            }
        }

        public void DeleteField()
        {
            if (columnDT.Rows.Count == 0)
            {
                return;
            }
            string columnName = columnDT.Rows[bandedGridView1.FocusedRowHandle]["COLUMN_NAME"].ToString();
            int colID = (int)columnDT.Rows[bandedGridView1.FocusedRowHandle]["COLID"];

            //同时删除视图中与该字段关联的表达式 zxw 2013/2/22
            DataTable dt = (DataTable)gridControlView.DataSource;
            for (int i = 0; i < dt.Rows.Count;i++ )
            {
                if ((dt.Rows[i]["COLUMN_NAME"].ToString().Contains(columnName) && dt.Rows[i]["COLUMN_NAME"].ToString().Contains(" ")) || dt.Rows[i]["COLUMN_NAME"].ToString() == columnName)
                {
                    dt.Rows.RemoveAt(i);
                }
            }

            columnDT.Rows.RemoveAt(bandedGridView1.FocusedRowHandle);
            gridControl1.RefreshDataSource();
            if (bandedGridView1.FocusedRowHandle < 0)
            {
                textComment.Text = "";
                textDefaultValue.Text = "";
            }
            else
            {
                textComment.Text = columnDT.Rows[bandedGridView1.FocusedRowHandle]["COMMENT"].ToString();
                textDefaultValue.Text = columnDT.Rows[bandedGridView1.FocusedRowHandle]["DEFAULTVALUE"].ToString();
            }
            //删除原有列

            //if (!isCreate && colID >= 0)
            //{
            //    string alterStr = string.Format("ALTER TABLE {0} DROP {1}  ", tableObj.TABLE_NAME, columnName);
            //    alterScript.Add(alterStr);

            //}
        }

        private void ToolStripMenuItemInsertField_Click(object sender, EventArgs e)
        {
            InsertField();
        }

        public void InsertField()
        {
            DataRow dr = columnDT.NewRow();

            dr["COLUMN_NAME"] = "new_columns";
            dr["COLUMN_TYPE"] = "varchar(45)";
            dr["KEY"] = false;
            dr["NN"] = false;
            dr["BIN"] = false;
            dr["UN"] = false;
            dr["ZF"] = false;
            dr["AI"] = false;
          //  dr["VIEWNAME"] = "";
            dr["COMMENT"] = "";
            dr["DEFAULTVALUE"] = "";
            dr["COLID"] = -1;
            int location = 0;
            if (bandedGridView1.FocusedRowHandle > 0)
            {
                location = bandedGridView1.FocusedRowHandle;
            }

            columnDT.Rows.InsertAt(dr, location);
            gridControl1.RefreshDataSource();
            gridControlView.RefreshDataSource();
        }

        private void CtrlTableManager_Click(object sender, EventArgs e)
        {
              
        }

        private void CtrlTableManager_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                gridControl1.ContextMenuStrip = contextMenuStrip1;

                if (!isCreate)
                {
                    ToolStripMenuItemInsertField.Enabled = false;
                }

            }
        }

        public void KeyClick()
        {
            if ((bool)columnDT.Rows[bandedGridView1.FocusedRowHandle]["KEY"])
            {
                columnDT.Rows[bandedGridView1.FocusedRowHandle]["KEY"] = false;
            }
            else
            {
                columnDT.Rows[bandedGridView1.FocusedRowHandle]["KEY"] = true;
                columnDT.Rows[bandedGridView1.FocusedRowHandle]["NN"] = true;
            }
        }

        /// <summary>
        /// 设置数据表名
        /// </summary>
        /// <param name="tablename"></param>
        public  void SetTableName(string tablename)
        {
            textTableName.Text = tablename;
        }

        /// <summary>
        /// 获取数据表名
        /// </summary>
        /// <returns></returns>
        public string GetTableName()
        {
            return textTableName.Text;
        }

        public  string GetDescription()
        {
            return memoTableComment.Text;
        }

        public  void SetSchema(string schema)
        {
            labelSchema.Text = schema;
        }

        private void textComment_EditValueChanged(object sender, EventArgs e)
        {
            if (bandedGridView1.FocusedRowHandle >=0)
            {
                columnDT.Rows[bandedGridView1.FocusedRowHandle]["COMMENT"] = textComment.Text;
            }
            
        }
       
        /// <summary>
        ///清空组件 
        /// </summary>
        public void ClearCtrl()
        {
            ((DataTable)gridControl1.DataSource).Clear();
            ((DataTable)gridControlView.DataSource).Clear();
            memoViewScript.Text = "";
          //  ((DataTable)gridControl2.DataSource).Clear();
            memoTableComment.Text = "";
            textTableName.Text = "";
            textComment.Text = "";
            textDefaultValue.Text = "";
            cmbCharSet.Text = "";
            cmbEngine.Text = "";
            contextMenuStrip1.Enabled = false;

        }

        private void bandedGridView1_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            //string str =  e.Value.ToString();
            //gridControl1.RefreshDataSource();
        
        }


        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            gridView1.CloseEditor();
            gridView1.UpdateCurrentRow();
            ((DataTable)gridControlView.DataSource).AcceptChanges();
            if (textTableName.Text == "")
            {
                return;
            }
             memoViewScript.Text = GetViewScript();
        }

        /// <summary>
        /// 获取视图表对象
        /// </summary>
        public List<table_view_Mdl> GetTableViewMdl()
        {
            List<table_view_Mdl> viewLst = new List<table_view_Mdl>();

            DataTable dt = (DataTable)gridControlView.DataSource;

            for (int i = 0 ; i < dt.Rows.Count ;i++ )
            {
                if (!string.IsNullOrEmpty(dt.Rows[i]["COLUMN_NAME"].ToString()) && !string.IsNullOrEmpty(dt.Rows[i]["VIEWNAME"].ToString()))
                {
                    table_view_Mdl mdl = new table_view_Mdl();
                    mdl.FIELD_NAME = dt.Rows[i]["COLUMN_NAME"].ToString();
                    mdl.TABLE_NAME = textTableName.Text;
                    mdl.VIEW_FIELD_NAME = dt.Rows[i]["VIEWNAME"].ToString();

                    if ((bool)dt.Rows[i]["keyWord"])
                    {
                        mdl.MEMO = "keyWord";
                    }

                    viewLst.Add(mdl);
                }
            }
            return viewLst;
        }

        /// <summary>
        /// tab页切换到视图定义页面时，加载表字段
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (e.PrevPage == xtraTabPage1 && e.Page == xtraTabPage3) 
            {
                repositoryItemComboBoxField.Items.Clear();
            //    repositoryItemComboBoxField.Items.Add()
                for (int i = 0 ; i < columnDT.Rows.Count ;i++ )
                {
                    repositoryItemComboBoxField.Items.Add(columnDT.Rows[i]["COLUMN_NAME"].ToString());
                }
            }
        }
    }
}
