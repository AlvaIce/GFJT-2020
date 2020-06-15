using System.Windows.Forms;
using System.Threading.Tasks;

namespace QRST_DI_MS_Component.Common
{
    public partial class WaitForm : Form
    {
         
        public WaitForm()
        {
            InitializeComponent();
            this.ShowInTaskbar = false;
        }
        public WaitForm(string info)
        {
            InitializeComponent();
            this.label1.Text = "";
            this.label1.Text = info;
            this.ShowInTaskbar = false;
        }
        public delegate void doTaskDel(object[] objs);
        public doTaskDel datask;

        public delegate void closeWindowDel();
        public closeWindowDel closewindow;

        void closeWindow()
        {
            this.Close();
        }

        public void beginShowDialog(object[] objs)
        {
            closewindow = new closeWindowDel(closeWindow);
            Task.Factory.StartNew(() => {
                if (datask != null)
                    datask(objs);
                this.Invoke(closewindow);
            });
            this.ShowDialog();
        }
        
    }
}
