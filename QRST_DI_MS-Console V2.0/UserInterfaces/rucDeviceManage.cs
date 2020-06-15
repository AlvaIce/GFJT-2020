using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using QRST_DI_DS_MetadataQuery.PagingQuery;
using QRST_DI_Resources;
 
namespace QRST_DI_MS_Desktop.UserInterfaces
{
    public partial class rucDeviceManage : RibbonPageBaseUC
    {
        private mucDeviceManage mucdevicemaint;
        public rucDeviceManage(object objmuc)
            : base(objmuc)
        {
            InitializeComponent();
            mucdevicemaint = (mucDeviceManage)base.ObjMainUC;
            mucdevicemaint.xtraTabControl1.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(xtraTabControl1_SelectedPageChanged);
        }
        string ftTempname;
        public static NormalPagingQuery normalPagingQuery = new NormalPagingQuery();
        string sql;
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
        private void barButtonItemQuery_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FistRunCheck();
            StringBuilder sb = new StringBuilder();
            ftTempname = mucdevicemaint.FocustTableName;
            //this.CheckFocus();//判断是不是没有触发selectindexchange
            #region 判断附加条件
            if (barEditItemKeyWord3.EditValue != null && barEditItemKeyWord3.EditValue.ToString() != "")
            {
                string sbkeyWord = barEditItemKeyWord3.EditValue.ToString();
                sb.AppendFormat("设备描述 = '{0}'", sbkeyWord);
            }
            if (barEditItemKeyWord.EditValue != null && barEditItemKeyWord.EditValue.ToString() != "")
            {
                string zckeyWord = barEditItemKeyWord.EditValue.ToString();
                if (!string.IsNullOrEmpty(sb.ToString()))
                {
                    sb.Append(" and ");
                }
                sb.AppendFormat(" 设备ID = '{0}'", zckeyWord);
            }
            if (barEditItemKeyWord1.EditValue != null && barEditItemKeyWord1.EditValue.ToString() != "")
            {
                string gyskeyWord = barEditItemKeyWord1.EditValue.ToString();
                if (!string.IsNullOrEmpty(sb.ToString()))
                {
                    sb.Append(" and ");
                }
                sb.AppendFormat("供应商ID = '{0}'", gyskeyWord);
            }
            if (barEditItemKeyWord2.EditValue != null && barEditItemKeyWord2.EditValue.ToString() != "")
            {
                string cgkeyWord = barEditItemKeyWord2.EditValue.ToString();
                if (!string.IsNullOrEmpty(sb.ToString()))
                {
                    sb.Append(" and ");
                }
                sb.AppendFormat("采购ID = '{0}'", cgkeyWord);
            }
            #endregion
            #region sql查询语句组织
            if (ftTempname == "供应商信息")
            {
                normalPagingQuery = new NormalPagingQuery(TheUniversal.MIDB.sqlUtilities, "*", "de_supplier_view", sb.ToString());
            }
            else if (ftTempname == "供应设备信息")
            {
                normalPagingQuery = new NormalPagingQuery(TheUniversal.MIDB.sqlUtilities, "*", "de_supplyinfo_view", sb.ToString());
            }
            else if (ftTempname == "计划采购记录")
            {
                normalPagingQuery = new NormalPagingQuery(TheUniversal.MIDB.sqlUtilities, "*", "de_purchasesche_view", sb.ToString()); 
            }
            else if (ftTempname == "购入设备记录")
            {
                normalPagingQuery = new NormalPagingQuery(TheUniversal.MIDB.sqlUtilities, "*", "de_purchaserec_view", sb.ToString());
            }
            else if (ftTempname == "中心设备资产")
            {
                normalPagingQuery = new NormalPagingQuery(TheUniversal.MIDB.sqlUtilities, "*", "de_assetinfo_view", sb.ToString());
               
            }
            else if (ftTempname == "保密调查记录")
            {
                normalPagingQuery = new NormalPagingQuery(TheUniversal.MIDB.sqlUtilities, "*", "de_censorshipinfo_view", sb.ToString());
                
            }
            else if (ftTempname == "设备维修记录")
            {
                normalPagingQuery = new NormalPagingQuery(TheUniversal.MIDB.sqlUtilities, "*", "de_repairrec_view", sb.ToString());
            }
            else if (ftTempname == "设备破损记录")
            {
                normalPagingQuery = new NormalPagingQuery(TheUniversal.MIDB.sqlUtilities, "*", "de_breakrec_view", sb.ToString());
            }
            else if (ftTempname == "设备借用记录")
            {
                normalPagingQuery = new NormalPagingQuery(TheUniversal.MIDB.sqlUtilities, "*", "de_borrowrec_view", sb.ToString());
            }
            else if (ftTempname == "计划报废记录")
            {
                normalPagingQuery = new NormalPagingQuery(TheUniversal.MIDB.sqlUtilities, "*", "de_discardedsche_view", sb.ToString());
            }
            else
            {
                normalPagingQuery = new NormalPagingQuery(TheUniversal.MIDB.sqlUtilities, "*", "de_discardedrec_view", sb.ToString());
            }
            #endregion
            //mucDeviceManage.msda = new MySqlDataAdapter(normalPagingQuery.desql, TheUniversal.MIDB.sqlUtilities.con);
            mucdevicemaint.Query(normalPagingQuery);
            stemp = ftTempname;//查询后添加不重置表结构
        }
        public void FistRunCheck()
        {
            if (mucdevicemaint.FocustTableName == null && mucdevicemaint.gridViewTemp == null)
            {
                mucdevicemaint.listBoxControlSystem.SelectedIndex = 0;
                mucdevicemaint.FocustTableName = "供应商信息";
                mucdevicemaint.gridControlNodeLst1.DataSource = null;
                mucdevicemaint.gridControlTempt = mucdevicemaint.gridControlNodeLst1;
                mucdevicemaint.gridViewTemp = mucdevicemaint.gridView1;
                mucdevicemaint.ctrlpageTemp = mucdevicemaint.ctrlPage;
                mucdevicemaint.listBoxTemp = mucdevicemaint.listBoxControlSystem;
                mucdevicemaint.ctrlpageTemp.devicequeryevent -= mucdevicemaint.QueryFinishedEvent;//在这绑定时会出现ctrl对象为空
                mucdevicemaint.ctrlpageTemp.devicequeryevent += mucdevicemaint.QueryFinishedEvent;
            }
        }
        #region 页面切换事件
        public void xtraTabControl1_SelectedPageChanged(object sender, EventArgs e)
        {
            if (mucdevicemaint.xtraTabControl1.SelectedTabPage == mucdevicemaint.xtraTabPage1)
            {
                mucdevicemaint.listBoxControlSystem.SelectedIndex = 0;
                mucdevicemaint.gridControlNodeLst1.DataSource = null;
                mucdevicemaint.FocustTableName = "供应商信息";
                mucdevicemaint.gridControlTempt = mucdevicemaint.gridControlNodeLst1;
                mucdevicemaint.gridViewTemp = mucdevicemaint.gridView1;
                mucdevicemaint.ctrlpageTemp = mucdevicemaint.ctrlPage;
                mucdevicemaint.listBoxTemp = mucdevicemaint.listBoxControlSystem;
                mucdevicemaint.ctrlpageTemp.devicequeryevent -= mucdevicemaint.QueryFinishedEvent;//在这绑定时会出现ctrl对象为空
                mucdevicemaint.ctrlpageTemp.devicequeryevent += mucdevicemaint.QueryFinishedEvent;
            }
            else if (mucdevicemaint.xtraTabControl1.SelectedTabPage == mucdevicemaint.xtraTabPage2)
            {
                mucdevicemaint.listBoxControl1.SelectedIndex = 0;
                mucdevicemaint.gridControlNodeLst2.DataSource = null;
                mucdevicemaint.FocustTableName = "中心设备资产";
                mucdevicemaint.gridControlTempt = mucdevicemaint.gridControlNodeLst2;
                mucdevicemaint.gridViewTemp = mucdevicemaint.gridView2;
                mucdevicemaint.ctrlpageTemp = mucdevicemaint.ctrlPage1;
                mucdevicemaint.listBoxTemp = mucdevicemaint.listBoxControl1;
                mucdevicemaint.ctrlpageTemp.devicequeryevent -= mucdevicemaint.QueryFinishedEvent;//在这绑定时会出现ctrl对象为空
                mucdevicemaint.ctrlpageTemp.devicequeryevent += mucdevicemaint.QueryFinishedEvent;
            }
            else
            {
                mucdevicemaint.listBoxControl2.SelectedIndex = 0;
                mucdevicemaint.gridControlNodeLst3.DataSource = null;
                mucdevicemaint.FocustTableName = "计划报废记录";
                mucdevicemaint.gridControlTempt = mucdevicemaint.gridControlNodeLst3;
                mucdevicemaint.gridViewTemp = mucdevicemaint.gridView3;
                mucdevicemaint.ctrlpageTemp = mucdevicemaint.ctrlPage2;
                mucdevicemaint.listBoxTemp = mucdevicemaint.listBoxControl2;
                mucdevicemaint.ctrlpageTemp.devicequeryevent -= mucdevicemaint.QueryFinishedEvent;//在这绑定时会出现ctrl对象为空
                mucdevicemaint.ctrlpageTemp.devicequeryevent += mucdevicemaint.QueryFinishedEvent;
            }
        }
        #endregion
        //添加
        string stemp = "";
        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ftTempname = mucdevicemaint.FocustTableName;
            if (stemp != ftTempname)
            {
                //CheckFocus();
                this.FistRunCheck();
                mucdevicemaint.GetTableStruct(ftTempname);
            }
            mucdevicemaint.gridViewTemp.AddNewRow();
            DataTable dt = (DataTable)mucdevicemaint.gridControlTempt.DataSource;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["选择数据"] = false;
            }
            stemp = ftTempname;//非查询添加，需重置表结构
        }
        //更新保存
        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            mucdevicemaint.gridControlTempt.FocusedView.CloseEditor(); // 对于Dexpress而言非常重要
            mucdevicemaint.gridControlTempt.FocusedView.UpdateCurrentRow();
            mucdevicemaint.gridViewTemp.FocusedColumn = mucdevicemaint.gridViewTemp.Columns[0];
            //MySqlCommandBuilder cmdbuild = new MySqlCommandBuilder(mucDeviceManage.msda);
            //mucDeviceManage.msda.Update(mucdevicemaint.datableTemp);
            Constant.IdbServerUtilities.UpdateTable(normalPagingQuery.desql, mucdevicemaint.datableTemp);
            MessageBox.Show("保存成功！");
        }
        //删除保存
        List<string> zdstring = new List<string>();
        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            mucdevicemaint.gridViewTemp.FocusedColumn = mucdevicemaint.gridViewTemp.Columns[0];
            zdstring.Clear();
            int count = 0;
            mucdevicemaint.gridViewTemp.RefreshData();
            DataView dv = (DataView)mucdevicemaint.gridViewTemp.DataSource;
            DataTable dt = dv.Table;
            for (int i = 0; i < dt.Rows.Count + count; i++)
            {
                if ((bool)dt.Rows[i - count]["选择数据"])
                {
                    zdstring.Add(dt.Rows[i - count][0].ToString());
                    dt.Rows.RemoveAt(i - count);
                    count++;
                }
            }
            //MySqlCommand cmd = new MySqlCommand();
            //cmd.Connection = TheUniversal.MIDB.sqlUtilities.con;
            //cmd.Connection.Open();
            //cmd.CommandType = CommandType.Text;
            string sql=null;
            for (int i = 0; i < zdstring.Count; i++)
            {
                if (normalPagingQuery.tableName != null)
                {
                     sql = string.Format("delete from {0} where id='{1}'", normalPagingQuery.tableName, zdstring[i]);
                    //cmd.CommandText = str;
                }
                else
                {
                     sql = string.Format("delete from {0} where id='{1}'", mucdevicemaint.tableName, zdstring[i]);
                    //cmd.CommandText = str;
                }
                try
                {
                    //cmd.ExecuteNonQuery();
                    Constant.IdbServerUtilities.ExecuteSql(sql);
                }
                catch
                {
                    MessageBox.Show("数据库中没有该记录");
                    //cmd.Connection.Close();
                    return;
                }
            }
            //cmd.Connection.Close();
            MessageBox.Show("删除成功！");
        }
        #region 按钮切换
        //购入切换
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            mucdevicemaint.xtraTabPage2.PageVisible = false;
            mucdevicemaint.xtraTabPage3.PageVisible = false;
            mucdevicemaint.xtraTabControl1.SelectedTabPage = mucdevicemaint.xtraTabPage1;
            mucdevicemaint.xtraTabPage1.PageVisible = true;
        }
        //维护切换
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            mucdevicemaint.xtraTabPage1.PageVisible = false;
            mucdevicemaint.xtraTabPage3.PageVisible = false;
            mucdevicemaint.xtraTabControl1.SelectedTabPage = mucdevicemaint.xtraTabPage2;
            mucdevicemaint.xtraTabPage2.PageVisible = true;
        }
        //报废切换
        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            mucdevicemaint.xtraTabPage1.PageVisible = false;
            mucdevicemaint.xtraTabPage2.PageVisible = false;
            mucdevicemaint.xtraTabControl1.SelectedTabPage = mucdevicemaint.xtraTabPage3;
            mucdevicemaint.xtraTabPage3.PageVisible = true;
        }
        #endregion
    }
}
