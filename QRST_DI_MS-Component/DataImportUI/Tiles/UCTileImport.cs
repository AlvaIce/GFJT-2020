using System;
using System.Windows.Forms;
using System.Threading;
using QRST_DI_MS_Basis.UserRole;
using QRST_DI_Resources;
using QRST_DI_TS_Process;
using QRST_DI_TS_Basis.DirectlyAddress;
using System.Data;

namespace TilesImport
{ 
    public partial class UCTileImport : UserControl
    {
        #region 将后台线程信息显示到窗体的控件中
        private delegate void ShowMessageLabel2Delegate(string message);
        public void ShowmessageLabel2(string message)
        {
            if (label2.InvokeRequired)
            {
                ShowMessageLabel2Delegate d = ShowmessageLabel2;
                label2.Invoke(d, message);
            }
            else
            {
                label2.Text = message;
            }
        }
        private delegate void ShowMessageLabel3Delegate(string message);
        public void ShowmessageLabel3(string message)
        {
            if (label3.InvokeRequired)
            {
                ShowMessageLabel3Delegate d = ShowmessageLabel3;
                label3.Invoke(d, message);
            }
            else
            {
                label3.Text = message;
            }
        }
        private delegate void ShowMessageLabel4Delegate(string message);
        public void ShowmessageLabel4(string message)
        {
            if (label4.InvokeRequired)
            {
                ShowMessageLabel4Delegate d = ShowmessageLabel4;
                label4.Invoke(d, message);
            }
            else
            {
                label4.Text = message;
            }
        }
        private delegate void ShowMessageLabel5Delegate(string message);
        public void ShowmessageLabel5(string message)
        {
            if (label5.InvokeRequired)
            {
                ShowMessageLabel5Delegate d = ShowmessageLabel5;
                label5.Invoke(d, message);
            }
            else
            {
                label5.Text = message;
            }
        }
        private delegate void ShowMessageLabel6Delegate(string message);
        public void ShowmessageLabel6(string message)
        {
            if (label6.InvokeRequired)
            {
                ShowMessageLabel6Delegate d = ShowmessageLabel6;
                label6.Invoke(d, message);
            }
            else
            {
                label6.Text = message;
            }
        }
        private delegate void ShowMessageLabel7Delegate(string message);
        public void ShowmessageLabel7(string message)
        {
            if (label7.InvokeRequired)
            {
                ShowMessageLabel7Delegate d = ShowmessageLabel7;
                label7.Invoke(d, message);
            }
            else
            {
                label7.Text = message;
            }
        }
        private delegate void ShowMessageLabel8Delegate(string message);
        public void ShowmessageLabel8(string message)
        {
            if (label8.InvokeRequired)
            {
                ShowMessageLabel8Delegate d = ShowmessageLabel8;
                label8.Invoke(d, message);
            }
            else
            {
                label8.Text = message;
            }
        }
        private delegate void ShowMessageLabel9Delegate(string message);
        public void ShowmessageLabel9(string message)
        {
            if (label9.InvokeRequired)
            {
                ShowMessageLabel9Delegate d = ShowmessageLabel9;
                label9.Invoke(d, message);
            }
            else
            {
                label9.Text = message;
            }
        }
        private delegate void ShowMessageLabel10Delegate(string message);
        public void ShowmessageLabel10(string message)
        {
            if (label10.InvokeRequired)
            {
                ShowMessageLabel10Delegate d = ShowmessageLabel10;
                label10.Invoke(d, message);
            }
            else
            {
                label10.Text = message;
            }
        }
        private delegate void ButtonHandleDelegate();
        public void ButtonHandle()
        {
            if (buttonImport.InvokeRequired)
            {
                ButtonHandleDelegate d = ButtonHandle;
                buttonImport.Invoke(d);
            }
            else
            {
                buttonImport.Enabled=true;
            }
        }
        private delegate void TextBox1Delegate(string text);
        public void ShowtextBox1(string text)
        {
            if (textBox1.InvokeRequired)
            {
                TextBox1Delegate d = ShowtextBox1;
                textBox1.Invoke(d, text);
            }
            else
            {
                textBox1.Text = text;
            }
        }
        #endregion
        private Thread tilesImportThread;
        public UCTileImport()
        {
            InitializeComponent();
            this.label2.Text = "";
            this.label3.Text = "";
            this.label4.Text = "";
            this.label5.Text = "";
            this.label6.Text = "";
            this.label7.Text = "";
            this.label8.Text = "";
            this.label9.Text = "";
            this.label10.Text = "";
            log4net.Config.XmlConfigurator.Configure(); 
        }
        userInfo _currentUser;
        public void Create(userInfo currentUser)
        {
            _currentUser = currentUser;
            ctrlVirtualDirSetting1.Create(currentUser.NAME, currentUser.PASSWORD);
        }

        //从MySQL中获取IP
        public string[] GetIPFromMySql()
        {
            DataSet IPDateSet = DirectlyAddressingIPMod.GetTileDsMod();
            int tbl = IPDateSet.Tables[0].Rows.Count;
            string[] s = new string[tbl];
            for (int i = 0; i < tbl; i++)
                s[i] = IPDateSet.Tables[0].Rows[i][0].ToString();
            return s;
        }


        private void buttonImport_Click(object sender, EventArgs e)
        {
            if (!Constant.Created)
            {
                Constant.InitializeTcpConnection();
                Constant.Create();
            }

            //TSPCommonReference.Create();
            buttonImport.Enabled = false;
            this.Cursor = Cursors.WaitCursor;
            this.ShowmessageLabel2("正在检查站点状态...");
            string[] IPcol = GetIPFromMySql();
            string missingIp = "";
            //IPcol = new string[] { "172.16.0.185" };
            foreach (string ip in IPcol)
            {
                //针对站点奔溃影响检索效率问题的解决方案：          joki170520
                if (!QRST_DI_TS_Process.Site.TServerSiteManager.optimalStorageSiteIPs.Contains(ip))
                {
                    missingIp = missingIp + ip + "; ";
                    //Console.WriteLine("站点" + ip + "无法访问");
                    //continue;
                }
            }
            DialogResult dr = DialogResult.Yes;
            if (missingIp != "")
            {
                dr = MessageBox.Show("站点" + missingIp + "无法访问，是否继续入库?", "提示", MessageBoxButtons.YesNo);
            }
            if (dr == DialogResult.Yes)
            {

                TileStoreWithUI tileStore = new TileStoreWithUI(this);
                tilesImportThread = new Thread(tileStore.TilesImport);
                tilesImportThread.Start(this.textBox1.Text);
            }
            this.Cursor = Cursors.Default;
            //this.label2.Text = string.Format("入库结果：本次执行共提交 {0} 条切片，入库失败 {1} 条。 ", TileStore.TilesCount, TileStore.TilesImportFailed);
        }
        private void buttonOpenFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            //fbd.ShowDialog();

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                this.textBox1.Text = fbd.SelectedPath;
            }
        }

        public void FormTileImport_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(tilesImportThread!=null)
                tilesImportThread.Abort();
        }
    }
}
