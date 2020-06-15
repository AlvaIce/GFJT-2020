using System;
 
namespace QRST_DI_MS_Desktop.UserInterfaces
{
    public partial class CtrlDisplayInfo : DevExpress.XtraEditors.XtraUserControl
    {
        public CtrlDisplayInfo()
        {
            InitializeComponent();
        }

        public string Message
        {
            get
            {
                return labelMsg.Text;
            }
            set
            {
                labelMsg.Text = value;
                AdjustLocation();
            }
        }

        public void AdjustLocation()
        {
            this.labelMsg.Location = new System.Drawing.Point((this.Width - labelMsg.Width) / 2, (this.Height - labelMsg.Height) / 3);
        }

        private void CtrlDisplayInfo_Load(object sender, EventArgs e)
        {

        }
    }
}
