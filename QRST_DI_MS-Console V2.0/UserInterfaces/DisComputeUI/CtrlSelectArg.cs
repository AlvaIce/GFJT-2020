using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace QRST_DI_MS_Desktop.UserInterfaces.DisComputeUI
{
    public partial class CtrlSelectArg : DevExpress.XtraEditors.XtraUserControl
    {
        public CtrlSelectArg()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofg = new OpenFileDialog();
            ofg.Multiselect = false;
            if (ofg.ShowDialog() == DialogResult.OK)
            {
                memoEdit.Text = ofg.FileName;
            }
        }
    }
}
