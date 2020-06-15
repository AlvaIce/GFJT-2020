using System;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using QRST_DI_DS_MetadataQuery.PagingQuery;
using System.IO;
using QRST_DI_Resources;
using QRST_DI_SS_DBInterfaces.IDBEngine;
 
namespace QRST_DI_MS_Desktop.UserInterfaces
{
    public partial class mucDeviceManage : DevExpress.XtraEditors.XtraUserControl
    {
        public mucDeviceManage()
        {
            InitializeComponent();
            initialDeviceTableName();
            #region 事件绑定
            listBoxControl1.MouseClick += new MouseEventHandler(listBoxControl1_MouseClick);
            listBoxControl2.MouseClick += new MouseEventHandler(listBoxControl2_MouseClick);
            listBoxControlSystem.MouseClick += new MouseEventHandler(listBoxControlSystem_MouseClick);
            #endregion
        }
        //初始化各库表索引
        public void initialDeviceTableName()
        {
            listBoxControlSystem.Items.Add("供应商信息");
            listBoxControlSystem.Items.Add("供应设备信息");
            listBoxControlSystem.Items.Add("计划采购记录");
            listBoxControlSystem.Items.Add("购入设备记录");
            listBoxControl1.Items.Add("中心设备资产");
            listBoxControl1.Items.Add("保密调查记录");
            listBoxControl1.Items.Add("设备维修记录");
            listBoxControl1.Items.Add("设备破损记录");
            listBoxControl1.Items.Add("设备借用记录");
            listBoxControl2.Items.Add("计划报废记录");
            listBoxControl2.Items.Add("实际报废记录");
            ctrlpageTemp.devicequeryevent += QueryFinishedEvent;
            gridView2.CellValueChanging += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(gridView2_CellValueChanged);
            gridView2.RowCellClick += new DevExpress.XtraGrid.Views.Grid.RowCellClickEventHandler(gridView2_RowCellClick);
        }
        string ChooseDataColumnCaption
        {
            get
            {
                string caption = "";
                switch (Constant.SystemLanguage)
                {
                    case EnumLanguage.ch:
                        caption = "select the data";
                        break;
                    case EnumLanguage.en:
                        caption = "选择数据";
                        break;
                }
                return caption;

            }
        }
        public string FocustTableName;
        NormalPagingQuery MucPagingQuery;
        public CtrlPage ctrlpageTemp = new CtrlPage();
        public DevExpress.XtraEditors.ListBoxControl listBoxTemp;
        #region 点击条目时事件
        public void listBoxControl1_MouseClick(object sender, MouseEventArgs e)
        {
            Point pt = new Point(e.X, e.Y);
            int index = listBoxControl1.IndexFromPoint(pt);
            listBoxControl1.SelectedIndex = index;
            if (listBoxControl1.SelectedIndex != -1)
            {
                FocustTableName = listBoxControl1.SelectedItem.ToString();
            }
        }
        public void listBoxControl2_MouseClick(object sender, MouseEventArgs e)
        {
            Point pt = new Point(e.X, e.Y);
            int index = listBoxControl2.IndexFromPoint(pt);
            listBoxControl2.SelectedIndex = index;
            if (listBoxControl2.SelectedIndex != -1)
            {
                FocustTableName = listBoxControl2.SelectedItem.ToString();
            }
        }
        public void listBoxControlSystem_MouseClick(object sender, MouseEventArgs e)
        {
            Point pt = new Point(e.X, e.Y);
            int index = listBoxControlSystem.IndexFromPoint(pt);
            listBoxControlSystem.SelectedIndex = index;
            if (listBoxControlSystem.SelectedIndex != -1)
            {
                FocustTableName = listBoxControlSystem.SelectedItem.ToString();
            }
        }
        #endregion
        public void Query(NormalPagingQuery _normalPagingQueryObj)
        {
            if (_normalPagingQueryObj != null)
            {
                ctrlpageTemp.Binding(_normalPagingQueryObj);//绑定一些状态参数
                ctrlpageTemp.FirstQuery();
                ctrlpageTemp.UpdatePageUC();
            }
        }
        public DevExpress.XtraGrid.GridControl gridControlTempt;
        public DevExpress.XtraGrid.Views.Grid.GridView gridViewTemp;
        public void QueryFinishedEvent()
        {
            try
            {
                ds.Tables.Clear();
                ds = Constant.IdbServerUtilities.GetDataSet(rucDeviceManage.normalPagingQuery.desql);
                //msda.Fill(ds);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "请重新选择表名以匹配字段！");
                return;
            }
            datableTemp = ds.Tables[0];
            this.SetTableFormat(datableTemp);//在这之前可以添加改动
            return;
        }
        //设备表格式，并填充
        public void SetTableFormat(DataTable dt)
        {
            gridViewTemp.Columns.Clear();
            DataColumn checkDownColumn = new DataColumn()
            {
                ColumnName = ChooseDataColumnCaption,
                DataType = typeof(bool)
            };
            //DLF 0822因异常添加
            if (dt == null)
            {
                gridControlTempt.DataSource = null;
                return;
            }
            if (!dt.Columns.Contains(ChooseDataColumnCaption))
                dt.Columns.Add(checkDownColumn);
            gridControlTempt.DataSource = dt;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i][ChooseDataColumnCaption] = false;
            }
            for (int i = 0; i < gridViewTemp.Columns.Count; i++)
            {
                if (i == 0)
                {
                    gridViewTemp.Columns[i].Visible = false;
                }
                else if (i >= 0)
                {
                    if (gridViewTemp.Columns[i].FieldName == "照片信息" || gridViewTemp.Columns[i].FieldName == "设备ID")
                    {
                        gridViewTemp.Columns[i].OptionsColumn.AllowEdit = false;
                    }
                    gridViewTemp.Columns[i].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                }
            }
        }
        public DataTable datableTemp;
        public string sql;
        public string tableName;
        //public static MySqlDataAdapter msda;
        public DataSet ds = new DataSet();
        //表结构主函数
        public void GetTableStruct(string TableName)
        {
            if (TableName == "供应商信息")
            {
                this.TableStruct("de_supplier_view");
            }
            else if (TableName == "供应设备信息")
            {
                this.TableStruct("de_supplyinfo_view");
            }
            else if (TableName == "计划采购记录")
            {
                this.TableStruct("de_purchasesche_view");
            }
            else if (TableName == "购入设备记录")
            {
                this.TableStruct("de_purchaserec_view");
            }
            else if (TableName == "中心设备资产")
            {
                this.TableStruct("de_assetinfo_view");
            }
            else if (TableName == "保密调查记录")
            {
                this.TableStruct("de_censorshipinfo_view");
            }
            else if (TableName == "设备维修记录")
            {
                this.TableStruct("de_repairrec_view");
            }
            else if (TableName == "设备破损记录")
            {
                this.TableStruct("de_breakrec_view");
            }
            else if (TableName == "设备借用记录")
            {
                this.TableStruct("de_borrowrec_view");
            }
            else if (TableName == "计划报废记录")
            {
                this.TableStruct("de_discardedsche_view");
            }
            else
            {
                this.TableStruct("de_discardedrec_view");
            }
        }
        //得到表结构子函数
        public void TableStruct(string Name)
        {
            IDbBaseUtilities baseUtilities = Constant.IdbServerUtilities;
            sql = "select * from " + Name;
            tableName = Name;
            //msda = new MySqlDataAdapter(sql, TheUniversal.MIDB.sqlUtilities.con);
            ds.Tables.Clear();
            //msda.Fill(ds);
            ds = baseUtilities.GetDataSet(sql);
            datableTemp = ds.Tables[0];
            datableTemp.Clear();
            this.SetTableFormat(datableTemp);
        }
        //gridview2中zpinfo字段单击事件
        public void gridView2_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (e.Column.FieldName == "照片信息" && e.Button == MouseButtons.Left)
            {
                int introwhandle = e.RowHandle;
                this.openFileDialog1.Filter = "jpg文件(*.jpg)|*.jpg|gif文件(*.gif)|*.gif";
                if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    byte[] bytes = File.ReadAllBytes(this.openFileDialog1.FileName);
                    gridView2.SetRowCellValue(introwhandle, e.Column, bytes);
                }
            }
            if (e.Column.FieldName == "设备ID" && e.Button == MouseButtons.Left)
            {
                int rowNum = e.RowHandle;
                string[] cell = getCellValue(rowNum);
                string deviceID = cell[0] + cell[1];
                gridView2.SetRowCellValue(rowNum, e.Column, deviceID);//设置值
            }
        }
        public string[] getCellValue(int RN)
        {
            string[] cellvalue = new string[2];
            string place = gridView2.GetRowCellValue(RN, gridView2.Columns["使用地点"]).ToString();//获取值
            if (place.Contains("-"))
            {
                place = place.Remove(place.IndexOf("-"));
                if (place == "北京")
                {
                    place = "bj_zx_21_";
                }
                else if (place == "新疆")
                {
                    place = "xj_fzx_wlmq_21_";
                }
            }
            cellvalue[0] = place;
            string nbip = gridView2.GetRowCellValue(RN, gridView2.Columns["内部IP"]).ToString();
            string[] cip = nbip.Split('.');
            if (cip.Length == 4)
            {
                nbip = cip[2] + "_" + cip[3];
            }
            cellvalue[1] = nbip;
            return cellvalue;
        }
        //gridview2中的sbID与内部ip的关联
        public void gridView2_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            int introwhandle;
            if (e.Column.FieldName == "内部IP")
            {
                introwhandle = e.RowHandle;
                string aip = e.Value.ToString();
                string[] cip = aip.Split('.');
                if (cip.Length == 4)
                {
                    string sbID = "bj_zx_21" + "_" + cip[2] + "_" + cip[3];
                    gridView2.SetRowCellValue(introwhandle, gridView2.Columns["设备ID"], sbID);
                }
            }
        }
    }
}
