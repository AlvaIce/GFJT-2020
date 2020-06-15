using System.Windows.Forms;

namespace TilesImport
{
    public partial class FormTileImport : Form
    {
        public FormTileImport()
        {
            InitializeComponent();
            this.FormClosing += ucTileImport1.FormTileImport_FormClosing;
            this.ucTileImport1.textBox1.Text = "";      // @"\\127.0.0.1\QRST_DB_Tile\新建文件夹";
        }

    }
}
