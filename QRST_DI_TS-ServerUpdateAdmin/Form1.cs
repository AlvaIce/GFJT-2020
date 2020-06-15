using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.IO;
using QRST_DI_Resources;
using QRST_DI_SS_DBClient.TCP;
using QRST_DI_SS_DBInterfaces.IDBEngine;

namespace QRST_DI_TS_ServerUpdateAdmin
{
    public partial class Form1 : Form
    {
        //\\{IP}\QRST_DB_Prototype\QRST_DB_Program\QRST_DI_TS_SiteServer_Console\QDB_TS_Server\
        //\\{IP}\QRST_DB_Prototype\QRST_DB_Program\QRST_DI_TS_SiteServer_Console\QDB_TS_Updater\

        private IDbBaseUtilities mysql = null;
        public Form1()
        {
            InitializeComponent();
            if(!Constant.Created)
            {
                Constant.InitializeTcpConnection();
                Constant.Create();

            }
            mysql = Constant.IdbServerUtilities;
            getShareUrl();
            textBox2.Text = shareUrl;
           
            comboBox1.Items.Add("ALL");
            comboBox2.Items.Add("ALL");
            foreach (string ip in SitesIP)
            {
                comboBox1.Items.Add(ip);
                comboBox2.Items.Add(ip);
            }
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Multiselect = false;
            openFileDialog1.Filter = "Zip files (*.zip)|*.zip";
            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string updatePackage = textBox1.Text;
            if (!File.Exists(updatePackage))
            {
                richTextBox1.AppendText(string.Format("更新包：{0}不存在\r\t", updatePackage));
                richTextBox1.Refresh();
                return;
            }
            QRST_DI_TS_Process.Site.TServerSiteManager.UpdateStorageSiteList();
            List<string> SitesIP = QRST_DI_TS_Process.Site.TServerSiteManager.GetAllSiteIP();
            richTextBox1.AppendText(string.Format("共发现{0}个站点，开始执行更新...\r\t", SitesIP.Count));
            richTextBox1.Refresh();
            int failednumb = 0;
            foreach (string ip in SitesIP)
            {
                try
                {
                    getShareUrl();
                    string destdir = string.Format(@"\\{0}\{1}\Updates\", ip, shareUrl);
                    if (!Directory.Exists(destdir))
                    {
                        Directory.CreateDirectory(destdir);
                    }
                    File.Copy(updatePackage, Path.Combine(destdir, Path.GetFileName(updatePackage)), true);
                    richTextBox1.AppendText(string.Format("站点{0}当前版本{1},更新包发送成功\r\t", ip, QRST_DI_TS_Process.Site.TServerSiteManager.getSiteFromSiteIP(ip).Version));
                    //richTextBox1.AppendText(string.Format("站点{0}更新包发送成功\r\t", ip));
                    richTextBox1.Refresh();
                }
                catch (Exception ex)
                {
                    richTextBox1.AppendText(string.Format("站点{0}更新失败，{1}\r\t", ip, ex.Message));
                    richTextBox1.Refresh();
                    failednumb++;
                }

            }

            richTextBox1.AppendText(string.Format("更新过程执行完毕,成功{0}个，失败{1}个\r\t", SitesIP.Count - failednumb, failednumb));
            richTextBox1.Refresh();

        }
        List<string> SitesIP = QRST_DI_TS_Process.Site.TServerSiteManager.GetAllSiteIP();
        public void sendInfo(string str)
        {
            for (int i = 0; i < SitesIP.Count; i++)
            {
                getType(SitesIP[i], str);
            }
        }
        string shareUrl = null;
        public void getShareUrl()
        {
            if (shareUrl==null)
            {
                string sql = string.Format("select value from appsettings where key='{0}'", "QRST_Updater_Path");
                DataSet ds = mysql.GetDataSet(sql);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    shareUrl = dr[0].ToString();
                }
            }
        }
        public void getType(string ip, string type)
        {
            richTextBox1.AppendText(string.Format("正在执行的站点是{0}", ip)+Environment.NewLine);
            getShareUrl();
            //string shareUrl = @"\QRST_DB_Prototype\QDB_TS_Server\";
            string msgshareUrl = @"\"+shareUrl + @"\msg\";
            string path = string.Format(@"\\{0}{1}", ip, msgshareUrl);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string url = string.Format("{0}{1}", path, type);
            try
            {
                FileStream fs = new FileStream(url, FileMode.OpenOrCreate);
                fs.Close();
            }
            catch
            {
            }
        }
        public void MsgForUpdater(string str)
        {
            if (comboBox1.Text == "ALL")
            {
                sendInfo(str);
            }
            else
            {
                getType(comboBox1.Text, str);
            }
        }
        //关闭所有服务
        private void button3_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            MsgForUpdater("end.txt");
            updateSQl("Stopped");
        }
        //打开所有服务
        private void button4_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            MsgForUpdater("start.txt");
            updateSQl("Running");
        }
        //重启所有服务
        private void button5_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            MsgForUpdater("restart.txt");
            updateSQl("Running");
        }
        //更新数据库站点表中的状态
        //MySqlBaseUtilities mysql = new MySqlBaseUtilities();

        public void updateSQl(string state)
        {
            if (comboBox1.Text == "ALL")
            {
                foreach (string ip in SitesIP)
                {
                    string sql = string.Format("Update  tileserversitesinfo set RunningState ='{0}' where ADDRESSIP='{1}'", state, ip);
                    mysql.ExecuteSql(sql);
                }
            }
            else
            {
                string sql = string.Format("Update  tileserversitesinfo set RunningState ='{0}' where ADDRESSIP='{1}'", state, comboBox1.Text);
                mysql.ExecuteSql(sql);
            }
        }
        //暂停所有服务
        private void button7_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            updateSQl("Suspended");
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }
        public void selectSql(string str)
        {
            string sql = string.Format("SELECT ADDRESSIP,VERSION,RunningState,CurOrderCode,OrderSubTime,UpdateIsAlive FROM tileserversitesinfo WHERE ADDRESSIP = '{0}'", str);
            DataSet ds = mysql.GetDataSet(sql);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                richTextBox1.AppendText(dr[0].ToString() + "  " + dr[1].ToString() + "  " +
                    dr[2].ToString() + "  " + dr[3].ToString() + "  " + dr[4].ToString() + "    UpdaterIsAlive:" + dr[5].ToString() + Environment.NewLine);
            }
        }
        public void getResult()
        {
            if (comboBox2.Text == "ALL")
            {
                foreach (string ip in SitesIP)
                {
                    selectSql(ip);
                }
            }
            else
            {
                selectSql(comboBox2.Text);
            }
        }
        //站点信息查询
        private void button6_Click(object sender, EventArgs e)
        {
            getResult();
        }
        //更新数据库表中字段状态
        public void SetUpdateIsAliveFalse(string ip)
        {

            string updateSql = string.Format("UPDATE tileserversitesinfo set UpdateIsAlive='false' WHERE ADDRESSIP = '{0}'", ip);
            mysql.ExecuteSql(updateSql);

            //原代码
            //DataSet ds = mysql.GetDataSet(sql);
            //foreach (DataRow dr in ds.Tables[0].Rows)
            //{
            //    string path = string.Format(@"\\{0}{1}", ip, isAliveurl);
            //    if (dr[0].ToString() == "true")
            //    {
            //        if (File.Exists(path))
            //        {
            //            string updateSql = string.Format("UPDATE tileserversitesinfo set UpdateIsAlive='false' WHERE ADDRESSIP = '{0}'", ip);
            //            mysql.ExecuteSql(updateSql);
            //        }
            //    }
            //}
        }

        //查询站点updater状态
        string isAliveurl;
        private void button8_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            if (comboBox1.Text == "ALL")
            {
                foreach (string ip in SitesIP)
                {
                    SetUpdateIsAliveFalse(ip);
                }
            }
            else
            {
                SetUpdateIsAliveFalse(comboBox1.Text);
            }
            MsgForUpdater("isAlive.txt");
            //GetUpdateStateResponse("isAlive.txt");
            System.Threading.Thread.Sleep(5000);

            getResult();
        }

        private void GetUpdateStateResponse(string str)
        {
            List<string> ips = new List<string>();
            if (comboBox1.Text == "ALL")
            {
                foreach (string ip in SitesIP)
                {
                    ips.Add(ip);
                }
            }
            else
            {
                ips.Add(comboBox1.Text);
            }
            GetUpdateStateResponse(ips, str);
        }

        private void GetUpdateStateResponse(List<string> ips,string str)
        {
            System.Threading.Thread.Sleep(2000);
            bool tobecontinue = true;
            DateTime startdt = DateTime.Now;
            getShareUrl();
            string sharemsgUrl = @"\" + shareUrl + @"\msg\";
            while (tobecontinue)
            {
                for (int i = ips.Count - 1; i > -1; i--)
                {
                    string ip = ips[i];
                    string msgpath = string.Format(@"\\{0}{1}\{2}", ip, sharemsgUrl, str);
                    if (!File.Exists(msgpath))
                    {
                        string sql = string.Format("SELECT UpdateIsAlive FROM tileserversitesinfo WHERE ADDRESSIP = '{0}'", ip);
                        DataSet ds = mysql.GetDataSet(sql);
                        if (ds.Tables[0].Rows[0][0].ToString() == "true")
                        {
                            richTextBox1.AppendText(ip + " Updater is alive. " + Environment.NewLine);
                        }
                        ips.RemoveAt(i);
                    }
                }
                if (ips.Count==0)
                {
                    tobecontinue = false;
                    break;
                }
                else if ((DateTime.Now-startdt).TotalSeconds>3)
                {
                    tobecontinue = false;
                    break;
                }
                else
                {
                    System.Threading.Thread.Sleep(500);
                }

            }
            if (ips.Count != 0)
            {
                for (int i = ips.Count - 1; i > -1; i--)
                {
                    string ip = ips[i];

                    richTextBox1.AppendText(ip + " Updater is dead. " + Environment.NewLine);
                }
            }
        }

    }
}
