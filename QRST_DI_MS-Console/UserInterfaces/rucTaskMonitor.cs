using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace QRST_DI_MS_Console.UserInterfaces
{
    public partial class rucTaskMonitor : RibbonPageBaseUC
    {
        public rucTaskMonitor():base()
        {
            InitializeComponent();
        }
        public rucTaskMonitor(object objMUC):base(objMUC)
    {
            InitializeComponent();
    }

        private void barListItem1_ListItemClick(object sender, DevExpress.XtraBars.ListItemClickEventArgs e)
        {

        }
    }
}
