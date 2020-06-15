using QRST_DI_TS_Process.Orders;

namespace QRST_DI_MS_Desktop.UserInterfaces
{
    public partial class CtrlOrderProperity : DevExpress.XtraEditors.XtraUserControl
    {
        public CtrlOrderProperity()
        {
            InitializeComponent();
            OrderDef orderdef = new OrderDef();
            propertyGridControl1.SelectedObject = orderdef;
        }
         
 
    }
}
