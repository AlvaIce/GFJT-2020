using System;
using System.Data;
using System.Windows.Forms;
using QRST_DI_DS_Basis;
using QRST_DI_DS_Metadata.MetaDataDefiner.Dal;
using QRST_DI_Resources;
using QRST_DI_SS_Basis.MetaData;
using QRST_DI_SS_DBInterfaces.IDBEngine;
 
namespace QRST_DI_MS_Desktop.UserInterfaces
{
    public partial class FrmMetEdit : DevExpress.XtraEditors.XtraForm
    {
        private string _qrstCode;
        private IDbBaseUtilities mySqlUtilities = Constant.IdbServerUtilities;
        private IDbOperating mySqlOperater = Constant.IdbOperating;
        private string _metadata;
        private int _rowLocation;
        private DataTable _dt;

        /// <summary>
        /// 元数据表字符串
        /// </summary>
        public string Metadata
        {
            get { return _metadata; }
        }

        public FrmMetEdit(string DQSTCODE)
        {
            _qrstCode = DQSTCODE;
            InitializeComponent();
            string db = _qrstCode.Substring(0, 4);

            switch (db)
            {
                case "BSDB":
                    mySqlUtilities = mySqlOperater.GetSubDbUtilities(EnumDBType.BSDB);
                    break;
                case "EVDB":
                    mySqlUtilities = mySqlOperater.GetSubDbUtilities(EnumDBType.EVDB);
                    break;
                case "RCDB":
                    mySqlUtilities = mySqlOperater.GetSubDbUtilities(EnumDBType.RCDB);
                    break;
                case "ISDB":
                    mySqlUtilities = mySqlOperater.GetSubDbUtilities(EnumDBType.ISDB);
                    break;
                case "MADB":
                    mySqlUtilities = mySqlOperater.GetSubDbUtilities(EnumDBType.MADB);
                    break;
                case "IPDB":
                    mySqlUtilities = mySqlOperater.GetSubDbUtilities(EnumDBType.IPDB);
                    break;
                default:
                    break;
            }

            string sql = String.Format("select table_name from tablecode where QRST_CODE = '{0}'", _qrstCode);
            string tableName = mySqlUtilities.GetDataSet(sql).Tables[0].Rows[0][0].ToString();
            sql = "select * from " + tableName + " where ID = -999999999999999";
            gridControl1.DataSource = mySqlUtilities.GetDataSet(sql).Tables[0];
            _dt = gridControl1.DataSource as DataTable;
            for (int i = 0; i < gridView1.Columns.Count;i++ )
            {
                gridView1.Columns[i].Width = 150;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DataTable dt = gridControl1.DataSource as DataTable;
            if (dt.Rows.Count > 0)
            {
                dt.TableName = GetTableNameBydataCode(_qrstCode);
                _metadata = SerializerUtil.GetXmlFormatDs(dt);
                this.DialogResult = DialogResult.OK;
                this.Hide();
            }
        }

        private void btnAddRow_Click(object sender, EventArgs e)
        {
            DataRow dr = _dt.NewRow();
            if (gridView1.FocusedRowHandle > 0)
            {
                _rowLocation = gridView1.FocusedRowHandle;
            }
            _dt.Rows.InsertAt(dr, _rowLocation);
            gridControl1.RefreshDataSource();
        }

        private void btnDeleteRow_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0)
            {
                return;
            }
            _rowLocation = gridView1.FocusedRowHandle;
            _dt.Rows.RemoveAt(_rowLocation);
            gridControl1.RefreshDataSource();
        }

        private string GetTableNameBydataCode(string _dataCode)
        {
            string tablename = "";
            tablename = (new tablecode_Dal(mySqlUtilities)).GetTableName(_dataCode);
            return tablename;
        }


        
    }
}
