using System.Windows.Forms;
using System.Threading.Tasks;

namespace QRST_DI_DataImportTool
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
