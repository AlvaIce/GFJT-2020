using System;
using System.Windows.Forms;
 
namespace QRST_DI_MS_Desktop.UserInterfaces
{
    public partial class InputMessageDialog : DevExpress.XtraEditors.XtraForm
    {
        public string inputContent = null;
        public InputMessageDialog()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen; 
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            if (tbInputText.Text == "")
            {
                groupBox1.Text = "输入的内容不能为空！";
            }
            else
            {
                this.inputContent = tbInputText.Text;
                this.Close();
            }
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
