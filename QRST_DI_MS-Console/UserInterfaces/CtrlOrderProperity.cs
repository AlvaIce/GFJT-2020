using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using QRST_DI_TS_Process.Orders;

namespace QRST_DI_MS_Console.UserInterfaces
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
