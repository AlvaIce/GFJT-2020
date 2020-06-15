using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using QRST_DI_MS_Basis.UserRole;

namespace QRST_DI_MS_Console.UserInterfaces
{
    public partial class FrmLogin : DevExpress.XtraEditors.XtraForm
    {
        public FrmLogin()
        {
            InitializeComponent();
            userInfo.sqlUtilities = new QRST_DI_DS_Basis.DBEngine.MySqlBaseUtilities();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if(Check())
            { 
                List<userInfo> userLst = userInfo.GetList(string.Format("NAME = '{0}'",textEditUserName.Text.Trim()));
                if (userLst.Count > 0)
                {
                    if (userLst[0].PASSWORD == textEditPassword.Text)
                    {
                        TheUniversal.currentUser = userLst[0];
                        this.DialogResult = DialogResult.OK;
                       
                    }
                    else
                    {
                        XtraMessageBox.Show("输入的用户密码不正确！");
                    }
                }
                else
                {
                    XtraMessageBox.Show("输入的用户名不正确！");
                }
            }
           
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            textEditPassword.Text = "";
            textEditUserName.Text = "";
        }

        bool Check()
        {
            if(string.IsNullOrEmpty(textEditUserName.Text))
            {
                XtraMessageBox.Show("请输入用户名！");
                return false;
            }
            if (string.IsNullOrEmpty(textEditPassword.Text))
            {
                XtraMessageBox.Show("请输入用户密码！");
                return false;
            }
            return true;
        }
    }
}