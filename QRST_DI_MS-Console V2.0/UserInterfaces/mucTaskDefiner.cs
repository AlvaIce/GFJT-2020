using System;
using System.Data;
using QRST_DI_TS_Process.Tasks;
 
namespace QRST_DI_MS_Desktop.UserInterfaces
{
    public partial class mucTaskDefiner : DevExpress.XtraEditors.XtraUserControl
    {
        public delegate void FocusedRowChangedDel(TaskClass _focusedTase);
        public FocusedRowChangedDel rowChangedDel;

        bool isFirestLoad = true;
        public mucTaskDefiner()
        {
            InitializeComponent();
        }

        private void mucTaskDefiner_VisibleChanged(object sender, EventArgs e)
        {
            if (isFirestLoad && this.Visible)
            {
                RefreshDataLst();
                isFirestLoad = false;
            }
        }

        public taskdef GetFocusedTask()
        {
            DataTable dt = (DataTable)gridControlTaskDefList.DataSource;
            if (gridView1.FocusedRowHandle < 0 || gridView1.FocusedRowHandle > dt.Rows.Count)
            {
                return null;
            }
            else
            {
                return taskdef.DBRow2TaskDefCls(dt.Rows[gridView1.FocusedRowHandle]);
            }
        }

        public void Query(string queryCondition)
        {
            DataSet ds = taskdef.GetDataSet(queryCondition);
            if (ds != null && ds.Tables.Count > 0)
            {
                gridControlTaskDefList.DataSource = ds.Tables[0];
                if (ds.Tables[0].Rows.Count > 0)
                {
                    gridView1.FocusedRowHandle = 0;
                }
            }
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (rowChangedDel != null)
            {
                DataTable dt = (DataTable)gridControlTaskDefList.DataSource;
                if (gridView1.FocusedRowHandle >= 0 && dt.Rows.Count > gridView1.FocusedRowHandle)
                    rowChangedDel(TaskClass.DBRow2TaskCls(dt.Rows[gridView1.FocusedRowHandle]));
            }
        }

        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        public void RefreshDataLst()
        {
            DataSet ds = taskdef.GetDataSet("");
            if (ds != null && ds.Tables.Count > 0)
            {
                gridControlTaskDefList.DataSource = ds.Tables[0];
                if (ds.Tables[0].Rows.Count > 0)
                {
                    gridView1.FocusedRowHandle = 0;
                }
            }
        }
    }
}
