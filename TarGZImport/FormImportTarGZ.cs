using System.Windows.Forms;
using QRST_DI_Resources;

namespace TarGZImport
{
    public partial class FormImportTarGZ : Form
    {
        public FormImportTarGZ()
        {
            if (!Constant.Created)
            {
                Constant.InitializeTcpConnection();
                Constant.Create();

            }

            InitializeComponent();
        }

    }
}
