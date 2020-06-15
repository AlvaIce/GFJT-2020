using System;
using System.Collections.Generic;
using System.Windows.Forms;
using QRST_DI_MS_Basis.UserRole;
using DevExpress.XtraEditors;
using System.IO;
using QRST_DI_Resources;
 
namespace QRST_DI_MS_Desktop.UserInterfaces
{
    public partial class FrmNewLogin : Form
    {
        public static string version = "1.7.0616.2000";
        public FrmNewLogin()
        {
            InitializeComponent();
            label1.Parent = pictureBox1;
            label2.Parent = pictureBox1;
            this.textBoxDbIp.Visible = false;
            this.buttonSave.Visible = false;
            this.label3.Visible = false;
            this.buttonSave.Enabled = false;

            label4.Parent = pictureBox1;
            label5.Parent = pictureBox1;
            this.label4.Visible = true;
            this.label5.Visible = true;
            this.label4.Text = version;
            //this.label4.BackColor = Color.Transparent;

        }


        private void btnLogin_Click(object sender, EventArgs e)
        {
            userInfo.sqlUtilities = Constant.IdbServerUtilities;
            if (Check())
            {
                List<userInfo> userLst = userInfo.GetList(string.Format("NAME = '{0}'", textEditUserName.Text.Trim()));
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

        bool Check()
        {
            if (string.IsNullOrEmpty(textEditUserName.Text))
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

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonNotify_Click(object sender, EventArgs e)
        {
            this.textBoxDbIp.Visible = true;
            this.buttonSave.Visible = true;
            this.label3.Visible = true;
            this.buttonSave.Enabled = true;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            string DBip = this.textBoxDbIp.Text;
            if (DBip == "")
            {
                MessageBox.Show("请保证数据库地址不为空");
                return;
            }
            string ConnectionStringMySql = "server =" + DBip + "; user id =HJDATABASE_ADMIN; password = dbadmin_2011; database = midb";
            string runpath = Application.StartupPath;
            string appxmlpath;
            //int index=runpath.IndexOf("bin");
            //appxmlpath = runpath.Substring(0, index) + "app.config";
            appxmlpath = Path.Combine(runpath, "QRST_DI_MS-Console.exe.config");
            //MySqlConnection con = new MySqlConnection(ConnectionStringMySql);
            string WS_QDB_GetData = "", WS_QDB_Searcher_APP = "", WS_QDB_Searcher_MySQL = "", WS_QDB_Searcher_Sqlite = "", WS_QDB_SubmitOrder = "";
            try
            {
                //con.Open();
                string sql = "select value from appSettings where appSettings.key='WS_QDB_GetData'";
                //MySqlCommand cmd = new MySqlCommand(sql, con);
                //WS_QDB_GetData = Convert.ToString(cmd.ExecuteScalar());
                WS_QDB_GetData = Constant.IdbServerUtilities.myExcuteScalar(sql);
                sql = "select value from appSettings where appSettings.key='WS_QDB_Searcher_APP'";
                //cmd = new MySqlCommand(sql, con);
                //WS_QDB_Searcher_APP = Convert.ToString(cmd.ExecuteScalar());
                WS_QDB_Searcher_APP = Constant.IdbServerUtilities.myExcuteScalar(sql);
                sql = "select value from appSettings where appSettings.key='WS_QDB_Searcher_MySQL'";
                //cmd = new MySqlCommand(sql, con);
                //WS_QDB_Searcher_MySQL = Convert.ToString(cmd.ExecuteScalar());
                WS_QDB_Searcher_MySQL= Constant.IdbServerUtilities.myExcuteScalar(sql);
                sql = "select value from appSettings where appSettings.key='WS_QDB_Searcher_Sqlite'";
                //cmd = new MySqlCommand(sql, con);
                //WS_QDB_Searcher_Sqlite = Convert.ToString(cmd.ExecuteScalar());
                WS_QDB_Searcher_Sqlite= Constant.IdbServerUtilities.myExcuteScalar(sql);
                sql = "select value from appSettings where appSettings.key='WS_QDB_SubmitOrder'";
                //cmd = new MySqlCommand(sql, con);
                //WS_QDB_SubmitOrder = Convert.ToString(cmd.ExecuteScalar());
                WS_QDB_SubmitOrder = Constant.IdbServerUtilities.myExcuteScalar(sql);
            }
            catch
            {

            }
            finally
            {
                //con.Close();
            }
            List<string> strline = new List<string>();
            using (StreamReader sr = new StreamReader(appxmlpath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Contains("ConnectionStringMySql"))
                    {
                        int valueindex = line.IndexOf("value=");
                        line = line.Substring(0, valueindex + 7);
                        line += ConnectionStringMySql + "\"/>";
                    }
                    if (line.Contains("WS_QDB_Searcher_MySQL/Service.asmx"))
                    {
                        int startindex = line.IndexOf("//");
                        int endindex = line.IndexOf(@"/WS");
                        int count = endindex - startindex - 2;
                        line = line.Remove(startindex + 2, count);
                        line = line.Insert(startindex + 2, WS_QDB_Searcher_MySQL);
                    }
                    if (line.Contains("WS_QDB_GetData/Service.asmx"))
                    {
                        int startindex = line.IndexOf("//");
                        int endindex = line.IndexOf(@"/WS");
                        int count = endindex - startindex - 2;
                        line = line.Remove(startindex + 2, count);
                        line = line.Insert(startindex + 2, WS_QDB_GetData);
                    }
                    if (line.Contains("WS_QDB_Searcher_Sqlite/Service.asmx"))
                    {
                        int startindex = line.IndexOf("//");
                        int endindex = line.IndexOf(@"/WS");
                        int count = endindex - startindex - 2;
                        line = line.Remove(startindex + 2, count);
                        line = line.Insert(startindex + 2, WS_QDB_Searcher_Sqlite);
                    }
                    if (line.Contains("WS_QDB_Searcher_APP/Service.asmx"))
                    {
                        int startindex = line.IndexOf("//");
                        int endindex = line.IndexOf(@"/WS");
                        int count = endindex - startindex - 2;
                        line = line.Remove(startindex + 2, count);
                        line = line.Insert(startindex + 2, WS_QDB_Searcher_APP);
                    }
                    if (line.Contains("WS_QDB_SubmitOrder/Service.asmx"))
                    {
                        int startindex = line.IndexOf("//");
                        int endindex = line.IndexOf(@"/WS");
                        int count = endindex - startindex - 2;
                        line = line.Remove(startindex + 2, count);
                        line = line.Insert(startindex + 2, WS_QDB_SubmitOrder);
                    }
                    strline.Add(line);
                }
            }
            using (StreamWriter sw = new StreamWriter(appxmlpath))
            {
                //if (File.Exists(appxmlpath))
                //    File.Delete(appxmlpath);
                foreach (string item in strline)
                {
                    sw.WriteLine(item);
                }
            }

            MessageBox.Show("配置成功，请重启程序！");
            Application.Exit();

        }

        private void textEditPassword_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin_Click(sender, null);
            }
        }
        private void FrmNewLogin_Shown(object sender, EventArgs e)
        {
            textEditUserName.Focus();
        }

        
    }
}
