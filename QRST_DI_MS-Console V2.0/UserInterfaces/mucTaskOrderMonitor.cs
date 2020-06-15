using System.Data;
using System;

namespace QRST_DI_MS_Desktop.UserInterfaces
{
    public partial class mucTaskOrderMonitor : DevExpress.XtraEditors.XtraUserControl
    {
        
        public mucTaskOrderMonitor()
        {
            InitializeComponent();
        }

        //刷新任务查询列表
        public void GridSet(DataTable dt, int list)
        {
            gridControl1.DataSource = dt;
            if (list == 1)
            {
                taskOutput.Visible = false;
                taskSubmitTime.Visible = false;
            }
        }
    }
}
