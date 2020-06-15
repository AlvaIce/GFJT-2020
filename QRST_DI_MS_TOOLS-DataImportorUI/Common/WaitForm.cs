using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace QRST_DI_MS_TOOLS_DataImportorUI.Common
{
    public partial class WaitForm : Form
    {

        public WaitForm()
        {
            InitializeComponent();
        }

        public delegate void doTaskDel();
        public doTaskDel datask;

        public delegate void closeWindowDel();
        public closeWindowDel closewindow;

        void closeWindow()
        {
            this.Close();
        }

        public void beginShowDialog()
        {
            closewindow = new closeWindowDel(closeWindow);
            Task.Factory.StartNew(() => {
                if (datask != null)
                    datask();
                this.Invoke(closewindow);
            });
            this.ShowDialog();
        }
    }
}
