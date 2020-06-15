using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using QRST_DI_DS_Basis;
using QRST_DI_DS_Basis.DBEngine;
using MySql.Data.MySqlClient;
using System.Configuration;
using QRST_DI_MS_Basis;
using QRST_DI_MS_Basis.UserRole;
using QRST_DI_MS_Basis.VirtualDir;

namespace WindowsFormsApplication2
{
    public partial class UserLogin : DevComponents.DotNetBar.Office2007Form
    {
        public static Ribbon ribbon;
        userInfo ui;
        private static VirtualDirEngine _vde;
        public static UserLogin _userlogin
        { get; set; }
        public static userInfo _userInfo
        { get; set; }

        public UserLogin()
        {
           
            InitializeComponent();
        }
       
        public static string username = "";
        public static string pwd = "";
      //  public static QRST_DI_MS_Basis.VirtualDir _VirtualDirEngine;
        private void buttonX1_Click(object sender, EventArgs e)
        {


            username = textBoxX1.Text.Trim();
            pwd = textBoxX2.Text.Trim();
            Ribbon ribbon = (Ribbon)this.Owner;
            if (username != "" && pwd != "")
            {

                /*此处需要连接数据库判断用户*/
                _vde = new VirtualDirEngine();
                bool login = VirtualDirEngine.Login(username, pwd);
               // bool login = _vde.Login(username, pwd);
                if (login)
                {
                    ribbon.strValue = username;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("用户名或密码错误！");
                }

            }
            else
            {
                MessageBox.Show("用户名或密码错误！");
            }
           
           
           
             
        }
        private bool checkUser()
        {
            List<userInfo> userLst = userInfo.GetList(string.Format("NAME = '{0}'", textBoxX1.Text.Trim()));
            if (userLst.Count > 0)
            {
                if (userLst[0].PASSWORD == textBoxX2.Text)
                {
                    ui = userLst[0];
                    return true;
                }
                else
                {
                    MessageBox.Show("输入的用户密码不正确！");
                }
            }
            else
            {
                MessageBox.Show("输入的用户名不正确！");
            }
            return false;
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            this.Close();
        }


       
    }
}