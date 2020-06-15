using System;
using System.Windows.Forms;
using QRST_DI_MS_Basis.VirtualDir;
 
namespace QRST_DI_MS_Component.VirtualDirUI
{
    public partial class UserLogin : DevComponents.DotNetBar.Office2007Form
    {
        public string Username = "";
        public string Password = "";
        public UserLogin()
        {
            InitializeComponent();
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }
        
        private void buttonX1_Click(object sender, EventArgs e)
        {
            Username = textBoxX1.Text.Trim();
            Password = textBoxX2.Text.Trim();
            //RibbonDemo ribbon = (RibbonDemo)this.Owner;
            if (Username != "" && Password != "")
            {

                /*此处需要连接数据库判断用户*/
                //ribbon.strValue = this.textBoxX1.Text;
                bool loginsuc = VirtualDirEngine.Login(Username, Password);
                if (loginsuc)
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("输入的用户密码不正确！");
                }

            }
            else
            {
                MessageBox.Show("用户名或密码不能为空！");
            }
        }
       
    }
}