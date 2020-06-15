using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using QRST_DI_DS_Metadata.MetaDataDefiner;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Xml;
using System.Runtime.InteropServices;
using System.Diagnostics;
using QRST_DI_DS_Basis.DBEngine;
using System.Net;
using QRST_DI_MS_Basis;
using System.Reflection;

namespace QRST_DI_MS_Console.UserInterfaces
{
    public partial class mucDataBacker : UserControl
    {
        public static string databasename = "";
        public List<SiteDb> dataBases;
        public string sql = "", sql2 = "";
        public static string imageaddress = Application.StartupPath + @"\Resources\db.Table.16x16.png";
        public Image ima2 = Image.FromFile(imageaddress);
        public static string newpath;
        public static string dbname1;
        public string autoBackupTables = "";//用于存储自动备份的表的字符串
        public string currentDatabaseName;//当前选择的数据库的名字

        //   public backupMessage msg = new backupMessage();
        public struct autoTbName   //用于将自动备份的字符串进行拆分，处理成相应内容存储
        {
            public List<string> midbtables;
            public List<string> bsdbtables;
            public List<string> evdbtables;
            public List<string> indbtables;
            public List<string> ipdbtables;
            public List<string> isdbtables;
            public List<string> madbtables;
            public List<string> rcdbtables;
        }


        public autoTbName autoTableName = new autoTbName();

        public Dictionary<string, List<string>> autoTbName1 = new Dictionary<string, List<string>>();

        NoticeMessagerForFileBackup _noticeMsger;//消息监听和发送
        public mucDataBacker()
        {
            InitializeComponent();
            _noticeMsger = new NoticeMessagerForFileBackup();//默认监听端口51111，发送端口52222            
            _noticeMsger.ReciewedMessage2 += new RecievedMessage2Handle(_noticeMsger_ReciewedMessage);
        }

        private void mucDataBacker_Load(object sender, EventArgs e)
        {
            //Control.CheckForIllegalCrossThreadCalls = false;//不检查跨线程的调用是否合法.因为本muc有跨线程界面操作。如改动，请修正相应代码
            //this.tabControl1.Region = new Region(new RectangleF(this.tabPage1.Left, this.tabPage1.Top, this.tabPage1.Width, this.tabPage1.Height));//C# TabControl 不显示选项卡标题 
            AddDBGridRow();
            adddGVdb();
            getautoBackupTables();
            backuptranslog();

            txtAutoBackupAddress.BackColor = System.Drawing.SystemColors.Window;
            txtBackupAddress.BackColor = Color.FromArgb(160, 160, 160);
            txtAutoBackupAddress.Text = GetAutoBackupAddress();

            //使Groupbox1居中
            groupBox1.Left = tabControl1.Width / 2 - groupBox1.Width / 2;


            btnChooseBackupAddress.Enabled = false;

            DirectoryInfo directory;
            string sCurPath = GetAutoBackupAddress();
            try
            {
                directory = new DirectoryInfo(sCurPath);
                if (directory.Exists == true)
                {
                    getSubDirs(treeView1.Nodes[0], sCurPath + "\\FullTableBackup");
                    getSubDirs(treeView1.Nodes[1], sCurPath + "\\TableBackup");
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            //配置和日志的功能中，自动加载相应的参数
            txtChangeAutoAddress.Text = GetAutoBackupAddress();
            txtChangeAutoSpan.Text = GetAutoBackupSpan();

            //在备份界面中，加载完全备份的按钮对其他控件的操作
            allBackupSetting();

            //在label9中，加载写在appsetting中的要自动备份的数据表名称
            if (autoBackupTables != "")
            {
                txtAutoBackupTables.Text = autoBackupTables;

            }
            else
            {
                txtAutoBackupTables.Text = "（空）";
            }
            InitControl4();
        }

        private void AddDBGridRow()
        {
            DataTable dtDB = new DataTable();
            DataTable dtTable = new DataTable();

            dtDB.Columns.Add("DBCheckColumn", typeof(bool));
            dtDB.Columns.Add("DBImageColumn", typeof(System.Drawing.Image));
            dtDB.Columns.Add("DBStringColumn", typeof(System.String));
            dtDB.Columns.Add("DBStringColumn2", typeof(System.String));

            ColumnCheckDB.DataPropertyName = "DBCheckColumn";
            ColumnDBImg.DataPropertyName = "DBImageColumn";
            ColumnDbList.DataPropertyName = "DBStringColumn";
            ColumnDbDiscription.DataPropertyName = "DBStringColumn2";
            dataGridViewDBData.DataSource = dtDB;

            string strPath = Application.StartupPath;
            strPath += @"\Resources\db.Schema.16x16.png";
            Image img = Image.FromFile(strPath);

            for (int i = 0; i < TheUniversal.subDbLst.Count; i++)
            {
                DataRow dr = dtDB.NewRow();
                dr["DBCheckColumn"] = true;
                dr["DBImageColumn"] = img;
                //dr["DBStringColumn"] = TheUniversal.subDbLst[i].DESCRIPTION + TheUniversal.subDbLst[i].NAME;
                dr["DBStringColumn"] = TheUniversal.subDbLst[i].NAME;
                dr["DBStringColumn2"] = TheUniversal.subDbLst[i].DESCRIPTION;
                dtDB.Rows.Add(dr);
            }
        }



        private void dataGridViewDBData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewDBData.CurrentCell.ColumnIndex == 0)
            {
                if (BackupType() == 3)
                {

                    for (int i = 0; i < dataGridViewDBData.Rows.Count; i++)
                    {
                        dataGridViewDBData.Rows[i].Cells[0].Value = false;
                    }
                    dataGridViewDBData.CurrentRow.Cells[0].Value = false;


                    if (Convert.ToBoolean(dataGridViewDBData.CurrentCell.Value) == true)
                    {
                        for (int i = 0; i < dataGridViewTalbe.Rows.Count; i++)
                        {
                            dataGridViewTalbe.Rows[i].Cells[0].Value = true;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < dataGridViewTalbe.Rows.Count; i++)
                        {
                            dataGridViewTalbe.Rows[i].Cells[0].Value = false;
                        };
                    }



                    dataGridViewTalbe.Rows.Clear();
                    string str1 = dataGridViewDBData.CurrentRow.Cells["ColumnDbList"].Value.ToString();
                    int dbindex = dataGridViewDBData.CurrentRow.Index;
                    dataBases = GetSelectedDataBase2(dbindex);
                    string[] str = dataBases[0].ConnectStr.Split(";".ToCharArray());
                    string ipAddress = str[0].Substring(str[0].IndexOf("=") + 1).Trim();
                    string user = str[1].Substring(str[1].IndexOf("=") + 1).Trim();
                    string password = str[2].Substring(str[2].IndexOf("=") + 1).Trim();
                    string databasename = str[3].Substring(str[3].IndexOf("=") + 1).Trim();
                    sql = string.Format(@"select table_name from information_schema.tables where table_schema='" + databasename + "' and table_type='base table'");
                    sql2 = string.Format(@"select table_name from information_schema.tables where table_schema='" + databasename + "' and table_type='view'");
                    List<string> tablename1, tablename2;
                    tablename1 = GetAllTablesName(sql);
                    tablename2 = GetAllViewName(sql2);

                    //DataTable dtTable = new DataTable();
                    //dtTable.Columns.Add("TableBCheckColumn", typeof(bool));
                    //dtTable.Columns.Add("TableImageColumn", typeof(System.Drawing.Image));
                    //dtTable.Columns.Add("TableStringColumn", typeof(System.String));

                    //ColumnCheckDB.DataPropertyName = "TableCheckColumn";
                    //ColumnDBImg.DataPropertyName = "TableImageColumn";
                    //ColumnDbList.DataPropertyName = "TableStringColumn";
                    //dataGridViewTalbe.DataSource = dtTable;

                    string strPath = Application.StartupPath;
                    strPath += @"\Resources\db.Table.16x16.PNG";
                    Image img = Image.FromFile(strPath);

                    string strPath2 = Application.StartupPath;
                    strPath2 += @"\Resources\db.View.16x16.PNG";
                    Image img2 = Image.FromFile(strPath2);
                    for (int j = 0; j < tablename1.Count; j++)
                    {
                        //dataGridViewTalbe.ColumnAdded+=
                        //DataRow dt = dtTable.NewRow();
                        //dt["TableCheck"] = true;
                        //dt["TableImage"] = img;
                        ////dr["DBStringColumn"] = TheUniversal.subDbLst[i].DESCRIPTION + TheUniversal.subDbLst[i].NAME;
                        //dt["TableAndView"] = tablename1[i];
                        //dtTable.Rows.Add(dt);
                        DataGridViewRow row = new DataGridViewRow();
                        int index = dataGridViewTalbe.Rows.Add(row);
                        //如果该项前面打了对勾，则后面的对应表全部打上对勾
                        if (Convert.ToBoolean(dataGridViewDBData.CurrentRow.Cells[0].EditedFormattedValue) == true)
                        {

                            dataGridViewTalbe.Rows[index].Cells[0].Value = true;
                        }
                        else
                        {
                            dataGridViewTalbe.Rows[index].Cells[0].Value = false;
                        }
                        dataGridViewTalbe.Rows[index].Cells[1].Value = img;
                        dataGridViewTalbe.Rows[index].Cells[2].Value = tablename1[j];

                    }

                    for (int j = 0; j < tablename2.Count; j++)
                    {
                        //dataGridViewTalbe.ColumnAdded+=
                        //DataRow dt = dtTable.NewRow();
                        //dt["TableCheck"] = true;
                        //dt["TableImage"] = img;
                        ////dr["DBStringColumn"] = TheUniversal.subDbLst[i].DESCRIPTION + TheUniversal.subDbLst[i].NAME;
                        //dt["TableAndView"] = tablename1[i];
                        //dtTable.Rows.Add(dt);
                        DataGridViewRow row = new DataGridViewRow();
                        int index = dataGridViewTalbe.Rows.Add(row);
                        if (Convert.ToBoolean(dataGridViewDBData.CurrentRow.Cells[0].EditedFormattedValue) == true)
                        {
                            dataGridViewTalbe.Rows[index].Cells[0].Value = true;
                        }
                        else
                        {
                            dataGridViewTalbe.Rows[index].Cells[0].Value = false;
                        }
                        dataGridViewTalbe.Rows[index].Cells[1].Value = img2;
                        dataGridViewTalbe.Rows[index].Cells[2].Value = tablename2[j];

                    }
                }               

            }

            if (dataGridViewDBData.CurrentCell.ColumnIndex == 1 || dataGridViewDBData.CurrentCell.ColumnIndex == 2)
            {

            }
        }

        //获取备份时选中的数据库
        public List<SiteDb> GetSelectedDataBase()
        {
            DataTable dt = (DataTable)dataGridViewDBData.DataSource;
            List<SiteDb> siteDbLst = new List<SiteDb>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if ((bool)dataGridViewDBData.Rows[i].Cells[0].EditedFormattedValue)
                {
                    siteDbLst.Add(TheUniversal.subDbLst[i]);
                }
                //if ((bool)dt.Rows[i]["DBCheckColumn"])
                //{
                //    siteDbLst.Add(TheUniversal.subDbLst[i]);
                //}
            }
            return siteDbLst;
        }

        public List<string> GetSelectTable()
        {
            List<string> tableList = new List<string>();
            for (int i = 0; i < dataGridViewTalbe.Rows.Count; i++)
            {
                if ((bool)dataGridViewTalbe.Rows[i].Cells[0].EditedFormattedValue == true)
                {
                    tableList.Add(dataGridViewTalbe.Rows[i].Cells[2].Value.ToString());
                }
            }
            return tableList;
        }

        public List<SiteDb> GetSelectedDataBase2(int i)
        {
            DataTable dt = (DataTable)dataGridViewDBData.DataSource;
            List<SiteDb> siteDbLst = new List<SiteDb>();
            siteDbLst.Add(TheUniversal.subDbLst[i]);
            return siteDbLst;
        }

        public static DBMySqlOperating dbOperating = new DBMySqlOperating();
        public static List<string> GetAllTablesName(string sql)
        {
            //switch(dbname)
            //{ case "midb": tablename = dbOperating.MIDB.myExcuteReader(sql); break;
            //  case "bsdb": tablename = dbOperating.BSDB.myExcuteReader(sql); break;
            //  case "evdb": tablename = dbOperating.EVDB.myExcuteReader(sql); break;
            //  case "ipdb": tablename = dbOperating.IPDB.myExcuteReader(sql); break;
            //  case "isdb": tablename = dbOperating.ISDB.myExcuteReader(sql); break;
            //  case "madb": tablename = dbOperating.MADB.myExcuteReader(sql); break;
            //  case "rcdb": tablename = dbOperating.RCDB.myExcuteReader(sql); break;
            //    case "indb":tablename=dbOperating.db
            //}
            List<string> tablename;
            tablename = dbOperating.MIDB.myExcuteReader(sql);
            return tablename;
        }
        public static List<string> GetAllViewName(string sql2)
        {
            List<string> viewname;
            viewname = dbOperating.MIDB.myExcuteReader(sql2);
            return viewname;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //DataTable dt = (DataTable)dataGridViewDBData.DataSource;
            ////dt.Columns["DBCheckColumn"].ReadOnly = true;
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{

            //    dt.Rows[i]["DBCheckColumn"] = true;
            //}
            ////dt.Columns["DBCheckColumn"].ReadOnly = false;
            //dataGridViewTalbe.Rows.Clear();
            for (int i = 0; i < dataGridViewDBData.Rows.Count; i++)
            {
                dataGridViewDBData.Rows[i].Cells[0].ReadOnly = true;
                dataGridViewDBData.Rows[i].Cells[0].Value = true;
                dataGridViewDBData.Rows[i].Cells[0].ReadOnly = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //DataTable dt = (DataTable)dataGridViewDBData.DataSource;
            ////dt.Columns["DBCheckColumn"].ReadOnly = true;
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{   
            //    dt.Rows[i]["DBCheckColumn"] = false;
            //}
            ////dt.Columns["DBCheckColumn"].ReadOnly = false;
            //dataGridViewTalbe.Rows.Clear();

            for (int i = 0; i < dataGridViewDBData.Rows.Count; i++)
            {
                dataGridViewDBData.Rows[i].Cells[0].ReadOnly = true;
                dataGridViewDBData.Rows[i].Cells[0].Value = false;
                dataGridViewDBData.Rows[i].Cells[0].ReadOnly = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //DataTable dt = (DataTable)dataGridViewTalbe.DataSource;
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    dt.Rows[i]["TableCheck"] = true;
            //}

            for (int i = 0; i < dataGridViewTalbe.Rows.Count; i++)
            {
                dataGridViewTalbe.Rows[i].Cells[0].ReadOnly = true;
                dataGridViewTalbe.Rows[i].Cells[0].Value = true;
                dataGridViewTalbe.Rows[i].Cells[0].ReadOnly = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridViewTalbe.Rows.Count; i++)
            {
                dataGridViewTalbe.Rows[i].Cells[0].ReadOnly = true;
                dataGridViewTalbe.Rows[i].Cells[0].Value = false;
                dataGridViewTalbe.Rows[i].Cells[0].ReadOnly = false;
            }
        }

        private void btnChooseBackupAddress_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            fbd.RootFolder = Environment.SpecialFolder.Desktop;
            fbd.SelectedPath = "C:\\";
            fbd.ShowNewFolderButton = true;
            fbd.Description = "请选择文件备份目录：";
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                this.txtBackupAddress.Text = fbd.SelectedPath;
            }
            fbd.Dispose();

        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            txtAutoBackupAddress.BackColor = Color.FromArgb(160, 160, 160);
            txtBackupAddress.BackColor = System.Drawing.SystemColors.Window;
            btnChooseBackupAddress.Enabled = true;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            txtAutoBackupAddress.BackColor = System.Drawing.SystemColors.Window;
            txtBackupAddress.BackColor = Color.FromArgb(160, 160, 160);
            btnChooseBackupAddress.Enabled = false;
        }

        //获取自动备份设定的地址
        public static string GetAutoBackupAddress()
        {
            string AutoSql = string.Format(@"select value from appsettings where `key`='BackupAddress'");
            string AutoAddress = dbOperating.MIDB.myExcuteScalar(AutoSql);
            return AutoAddress;
        }

        //获取自动备份设定的时间间隔
        public static string GetAutoBackupSpan()
        {
            string AutoSpanSql = string.Format(@"select value from appsettings where `key`='MysqlBackupSpan'");
            string AutoSpan = dbOperating.MIDB.myExcuteScalar(AutoSpanSql);
            return AutoSpan;
        }

        //获取上次自动备份的时间
        public static string GetLastBackupTime()
        {
            string AutoLastTimeSql = string.Format(@"select value from appsettings where `key`='MysqlBackupDatetime'");
            string AutoLastBackupTime = dbOperating.MIDB.myExcuteScalar(AutoLastTimeSql);
            return AutoLastBackupTime;
        }

        

        public void CloseEditor()
        {
            //dataGridViewDBData.
        }

        public string GetBackUpPath()
        {
            if (radioButton3.Checked == true)
                return txtAutoBackupAddress.Text;
            else
                return txtBackupAddress.Text;
        }

        //传值给rucDataBacker选中的是完全备份还是备份部分库或表
        public int BackupType()
        {
            int backuptype = 1;
            if (radioButton5.Checked == true)
                backuptype = 1;
            if (radioButton6.Checked == true)
                backuptype = 2;
            if (radioButton7.Checked == true)
                backuptype = 3;
            return backuptype;
        }

        //传值给rucDataBacker选中的是每个库备份一个文件还是每张表备份一个文件
        public bool GetBackupRequireBool()
        {
            if (radioButton1.Checked == true)
                return true;
            else
                return false;
        }

        //传值给rucDataBacker选中的是备份到设定的自动备份地址还是手动选择备份地址
        public bool GetBackupAddressBool()
        {
            if (radioButton3.Checked == true)
                return true;
            else
                return false;
        }


        /// <summary>
        /// 但选择完全备份的按钮时，对其他控件进行相应的操作
        /// </summary>
        public void allBackupSetting()
        {
            button1.ForeColor = Color.FromArgb(200, 200, 200);
            button1.BackColor = Color.FromArgb(230, 230, 230);
            button1.Enabled = false;
            button2.ForeColor = Color.FromArgb(200, 200, 200);
            button2.BackColor = Color.FromArgb(230, 230, 230);
            button2.Enabled = false;
            button3.ForeColor = Color.FromArgb(200, 200, 200);
            button3.BackColor = Color.FromArgb(230, 230, 230);
            button3.Enabled = false;
            button4.ForeColor = Color.FromArgb(200, 200, 200);
            button4.BackColor = Color.FromArgb(230, 230, 230);
            button4.Enabled = false;
            //选中全部的数据库
            for (int i = 0; i < dataGridViewDBData.Rows.Count; i++)
            {
                dataGridViewDBData.Rows[i].Cells[0].ReadOnly = true;
                dataGridViewDBData.Rows[i].Cells[0].Value = true;
                dataGridViewDBData.Rows[i].Cells[0].ReadOnly = false;
            }

            radioButton3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewDBData.Enabled = false;
            dataGridViewTalbe.Rows.Clear();
            dataGridViewTalbe.Enabled = false;
        }

        /// <summary>
        /// 获取子节点
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="str1"></param>
        private void getSubDirs(TreeNode parent, string str1)
        {
            DirectoryInfo directory;
            try
            {
                // 如果还没有检查过这个文件夹，则检查之 
                if (parent.Nodes.Count == 0)
                {
                    directory = new DirectoryInfo(str1);
                    foreach (DirectoryInfo dir in directory.GetDirectories())
                    {
                        // 新建一个树节点，并添加到目录树视 
                        TreeNode newNode = new TreeNode(dir.Name);
                        parent.Nodes.Add(newNode);
                        if (newNode.Level == 1)
                        {
                            newNode.ImageIndex = 0;
                            newNode.SelectedImageIndex = 0;
                            newNode.NodeFont = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                        }
                        if (newNode.Level == 2)
                        {
                            newNode.ImageIndex = 1;
                            newNode.SelectedImageIndex = 1;
                            newNode.NodeFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                        }
                    }
                }

                foreach (TreeNode node in parent.Nodes)
                {
                    // 如果还没有检查过这个文件夹，则检查 
                    if (node.Nodes.Count == 0)
                    {
                        directory = new DirectoryInfo(node.FullPath);

                        // 检查该目录上的任何子目录 
                        foreach (DirectoryInfo dir in directory.GetDirectories())
                        {
                            // 新建一个数节点，并添加到目录树视 
                            TreeNode newNode = new TreeNode(dir.Name);
                            node.Nodes.Add(newNode);
                            if (newNode.Level == 1)
                            {
                                newNode.ImageIndex = 0;
                                newNode.SelectedImageIndex = 0;
                                newNode.NodeFont = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                            }
                            if (newNode.Level == 2)
                            {
                                newNode.ImageIndex = 1;
                                newNode.SelectedImageIndex = 1;
                                newNode.NodeFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                            }
                        }

                    }
                }
            }
            catch (Exception doh)
            {
                Console.WriteLine(doh.Message);
            }
        }

        private void treeView1_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            string path = e.Node.FullPath;
            newpath = path.Replace("完全恢复（按日期选择）", GetAutoBackupAddress() + "\\FullTableBackup").Replace("表恢复（按日期选择）", GetAutoBackupAddress() + "\\TableBackup");
            getSubDirs(e.Node, newpath);
            dbname1 = e.Node.Text;
            filDataGridView(fixPath(e.Node));


            //if (treeView1.Nodes[2].IsSelected == true)
            //{
            //    FolderBrowserDialog fbd = new FolderBrowserDialog();

            //    fbd.RootFolder = Environment.SpecialFolder.Desktop;
            //    fbd.SelectedPath = "C:\\";
            //    fbd.ShowNewFolderButton = true;
            //    fbd.Description = "请选择文件备份目录：";
            //    if (fbd.ShowDialog() == DialogResult.OK)
            //    {
            //        this.txtBackupAddress.Text = fbd.SelectedPath;
            //    }
            //}
        }

        /// <summary>
        /// 填充恢复的datagridview
        /// </summary>
        /// <param name="lv"></param>
        /// <param name="strPath"></param>
        private void filDataGridView(string strPath)
        {
            DirectoryInfo directory = new DirectoryInfo(strPath);
            dGVRecover.Rows.Clear();
            //foreach (DirectoryInfo dir in directory.GetDirectories())
            //{
            //    ListViewItem item = new ListViewItem(dir.Name);
            //    item.SubItems.Add(string.Empty);
            //    item.SubItems.Add("文件夹");
            //    item.SubItems.Add(string.Empty);
            //    lv.Items.Add(item);
            //}
            foreach (FileInfo file in directory.GetFiles())
            {
                DataGridViewRow row = new DataGridViewRow();
                int index = dGVRecover.Rows.Add(row);

                //try
                //{
                //    Column2.Image = imageaddress;
                //}
                //catch
                //{ }


                dGVRecover.Rows[index].Cells[1].Value = ima2;
                dGVRecover.Rows[index].Cells[2].Value = file.Name;
                dGVRecover.Rows[index].Cells[3].Value = (file.Length / 1024).ToString() + " KB";
                dGVRecover.Rows[index].Cells[4].Value = file.FullName;
                dGVRecover.Rows[index].Cells[5].Value = file.LastWriteTime.ToString();

                //ListViewItem item = new ListViewItem(file.Name);
                //item.SubItems.Add((file.Length / 1024).ToString() + " KB");
                //item.SubItems.Add(file.Extension + "文件");
                //item.SubItems.Add(file.LastWriteTime.ToString());
                //lv.Items.Add(item);
            }
        }

        private string fixPath(TreeNode node)
        {
            string sRet = "";
            try
            {
                sRet = node.FullPath;
                //if (sRet.IndexOf("库恢复（按日期选择）") >= 1)
                //{
                //    imageaddress =Application.StartupPath+ @"\Resources\db.Schema.16x16.png";

                //}
                //if (sRet.IndexOf("表恢复（按日期选择）") >= 1)
                //{
                //    imageaddress = Application.StartupPath + @"\Resources\db.Table.16x16.PNG"; 
                //}
                //ima2 = Image.FromFile(imageaddress);

                sRet = sRet.Replace("完全恢复（按日期选择）", GetAutoBackupAddress() + "\\FullTableBackup").Replace("表恢复（按日期选择）", GetAutoBackupAddress() + "\\TableBackup");
                int index = sRet.IndexOf("\\\\");
                if (index > 1)
                {
                    sRet = node.FullPath.Remove(index, 1);

                }
            }
            catch (Exception doh)
            {
                Console.WriteLine(doh.Message);
            }
            return sRet;
        }





        //返回数据库恢复时的sql文件地址
        public List<string> getRecoverAddress()
        {
            List<string> Recoveraddress = new List<string>();
            for (int i = 0; i < dGVRecover.Rows.Count; i++)
            {
                if ((bool)dGVRecover.Rows[i].Cells[0].EditedFormattedValue)
                {
                    Recoveraddress.Add(dGVRecover.Rows[i].Cells[4].Value.ToString());
                    //Recoveraddress.Add(newpath+"\\" +dGVRecover.Rows[i].Cells[2].Value.ToString());
                }
            }
            return Recoveraddress;
        }

        //返回要恢复的数据库
        public string getRecoverDb()
        {
            string Recoverdb = dbname1;
            return Recoverdb;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            CloseEditor();
            if (Check())
            {
                FrmDisplayInfo frmDisplayInfo = new FrmDisplayInfo(GetBackUpPath(), GetSelectedDataBase(), GetBackupRequireBool(), GetBackupAddressBool(), BackupType(), GetSelectTable());
                frmDisplayInfo.ShowDialog();
                //  frmDisplayInfo.StartBackUp();
            }
        }

        /// <summary>
        /// 检核要备份的内容
        /// </summary>
        /// <returns></returns>
        bool Check()
        {
            if (string.IsNullOrEmpty(GetBackUpPath()))
            {
                XtraMessageBox.Show("请选择数据备份路径！");
                return false;
            }
            if (GetSelectedDataBase().Count == 0)
            {
                XtraMessageBox.Show("您还没有选择需要备份的数据库！");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 切换Tabcontrol的page
        /// </summary>
        /// <param name="i"></param>
        public void ChangeTabpage(int i)
        {
            tabControl1.SelectedIndex = i;
        }



        private void button6_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "sql文件 (*.sql)|*.sql"; //过滤文件类型
            fd.InitialDirectory = Application.StartupPath + "\\Temp\\";//设定初始目录
            fd.ShowReadOnly = true; //设定文件是否只读
            DialogResult r = fd.ShowDialog();
            if (r == DialogResult.OK)
            {
                string filename = fd.FileName;
                FileInfo info = new FileInfo(filename);
                this.textBox1.Text = fd.FileName;

                dGVRecover.Rows.Clear();

                DataGridViewRow row = new DataGridViewRow();
                int index = dGVRecover.Rows.Add(row);
                dGVRecover.Rows[index].Cells[0].Value = true;
                dGVRecover.Rows[index].Cells[1].Value = ima2;
                dGVRecover.Rows[index].Cells[2].Value = info.Name;
                dGVRecover.Rows[index].Cells[3].Value = (info.Length / 1024).ToString() + " KB";
                dGVRecover.Rows[index].Cells[4].Value = info.FullName;
                dGVRecover.Rows[index].Cells[5].Value = info.LastWriteTime.ToString();

            }
            fd.Dispose();
        }


        /// <summary>
        /// 开始恢复数据表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button7_Click(object sender, EventArgs e)
        {
            if (getRecoverAddress() == null)
            {
                MessageBox.Show("还未选择要恢复的文件");
                return;
            }
            if (getRecoverDb() == null)
            {
                MessageBox.Show("从外部恢复需要选择您要恢复到哪个数据库");
                return;
            }
            FrmRecover frmrecover = new FrmRecover(getRecoverAddress(), getRecoverDb());
            frmrecover.ShowDialog();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            dbname1 = comboBox1.SelectedItem.ToString();
        }

        //浏览文件夹，重新选择自动备份的地址
        private void button8_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            fbd.RootFolder = Environment.SpecialFolder.Desktop;
            fbd.SelectedPath = "C:\\";
            fbd.ShowNewFolderButton = true;
            fbd.Description = "请选择文件备份目录：";
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                txtChangeAutoAddress.Text = fbd.SelectedPath;
            }
            fbd.Dispose();
        }

        //在配置和日志中的dGVdb添加相应内容
        public void adddGVdb()
        {
            string strPath = Application.StartupPath;
            strPath += @"\Resources\db.Schema.16x16.png";
            Image img = Image.FromFile(strPath);
            dGVdb.Rows.Clear();
            for (int i = 0; i < TheUniversal.subDbLst.Count; i++)
            {
                DataGridViewRow dt = new DataGridViewRow();
                int index = dGVdb.Rows.Add(dt);
                dGVdb.Rows[index].Cells[0].Value = img;
                dGVdb.Rows[index].Cells[1].Value = TheUniversal.subDbLst[i].NAME;
            }
        }

        private void dGVdb_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            adddGVtable(dGVtable, dGVdb);
        }

        /// <summary>
        /// 当在选择相应的数据库时，添加相应的表或者视图
        /// </summary>
        /// <param name="dbname"></param>
        public void adddGVtable(DataGridView dgvTable, DataGridView dgvDB)
        {
            dgvTable.Rows.Clear();
            //string str1 = dgv.CurrentRow.Cells["ColumnDbList"].Value.ToString();
            int dbindex = dgvDB.CurrentRow.Index;
            dataBases = GetSelectedDataBase2(dbindex);          //正序和倒序的时候获得的数据库是一样的，所以暂时先把该datagridview的排序功能去掉
            string[] str = dataBases[0].ConnectStr.Split(";".ToCharArray());
            string ipAddress = str[0].Substring(str[0].IndexOf("=") + 1).Trim();
            string user = str[1].Substring(str[1].IndexOf("=") + 1).Trim();
            string password = str[2].Substring(str[2].IndexOf("=") + 1).Trim();
            string databasename = str[3].Substring(str[3].IndexOf("=") + 1).Trim();
            currentDatabaseName = databasename;
            sql = string.Format(@"select table_name from information_schema.tables where table_schema='" + databasename + "' and table_type='base table'");
            sql2 = string.Format(@"select table_name from information_schema.tables where table_schema='" + databasename + "' and table_type='view'");
            List<string> tablename1, tablename2;
            tablename1 = GetAllTablesName(sql);
            tablename2 = GetAllViewName(sql2);

            string strPath = Application.StartupPath;
            strPath += @"\Resources\db.Table.16x16.PNG";
            Image img = Image.FromFile(strPath);

            string strPath2 = Application.StartupPath;
            strPath2 += @"\Resources\db.View.16x16.PNG";
            Image img2 = Image.FromFile(strPath2);

            //getautoBackupTables();  //将此语句移动到form_load里去
            List<string> result = new List<string>();
            result = autoTbName1[databasename];
            //autoTbName1.TryGetValue(databasename, out result);

            for (int j = 0; j < tablename1.Count; j++)
            {
                DataGridViewRow row = new DataGridViewRow();
                int index = dgvTable.Rows.Add(row);
                ////如果该项前面打了对勾，则后面的对应表全部打上对勾
                //if (Convert.ToBoolean(dataGridViewDBData.CurrentRow.Cells[0].EditedFormattedValue) == true)
                //{

                //    dataGridViewTalbe.Rows[index].Cells[0].Value = true;
                //}
                //else
                //{
                //    dataGridViewTalbe.Rows[index].Cells[0].Value = false;
                //}
                if (result.Contains(tablename1[j]))
                {
                    dgvTable.Rows[index].Cells[0].Value = true;
                }

                dgvTable.Rows[index].Cells[1].Value = img;
                dgvTable.Rows[index].Cells[2].Value = tablename1[j];

            }

            for (int j = 0; j < tablename2.Count; j++)
            {
                DataGridViewRow row = new DataGridViewRow();
                int index = dgvTable.Rows.Add(row);
                //if (Convert.ToBoolean(dataGridViewDBData.CurrentRow.Cells[0].EditedFormattedValue) == true)
                //{
                //    dataGridViewTalbe.Rows[index].Cells[0].Value = true;
                //}
                //else
                //{
                //    dataGridViewTalbe.Rows[index].Cells[0].Value = false;
                //}
                if (result.Contains(tablename2[j]))
                {
                    dgvTable.Rows[index].Cells[0].Value = true;
                }
                dgvTable.Rows[index].Cells[1].Value = img2;
                dgvTable.Rows[index].Cells[2].Value = tablename2[j];

            }
        }

        private void dGVdb_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            adddGVtable(dGVtable, dGVdb);

        }

        /// <summary>
        /// 从appsettings中获取autoBackupTables并将其拆分处理
        /// </summary>
        public void getautoBackupTables()
        {

            string sql = string.Format(@"select value from appsettings where `key`='MysqlBackupTables'");
            autoBackupTables = dbOperating.MIDB.myExcuteScalar(sql);

            string[] str = autoBackupTables.TrimEnd(';').Split(";".ToCharArray());

            //string ipAddress = str[0].Substring(str[0].IndexOf("=") + 1).Trim();
            //string user = str[1].Substring(str[1].IndexOf("=") + 1).Trim();
            //string password = str[2].Substring(str[2].IndexOf("=") + 1).Trim();
            //string databasename = str[3].Substring(str[3].IndexOf("=") + 1).Trim();

            {
                autoTableName.midbtables = new List<string>();
                autoTableName.bsdbtables = new List<string>();
                autoTableName.evdbtables = new List<string>();
                autoTableName.rcdbtables = new List<string>();
                autoTableName.madbtables = new List<string>();
                autoTableName.isdbtables = new List<string>();
                autoTableName.ipdbtables = new List<string>();
                autoTableName.indbtables = new List<string>();
            }

            label8.Text = "已选择要备份的数据表和视图总共有：";
            if (str[0] != "")
            {
                label8.Text += str.Length + "张";
                for (int i = 0; i < str.Length; i++)
                {
                    string[] DbAndTable = str[i].Split(".".ToCharArray());
                    string dbname = DbAndTable[0].Trim();
                    string tablename = DbAndTable[1].Trim();
                    switch (dbname)
                    {
                        case "midb": { autoTableName.midbtables.Add(tablename); } break;
                        case "evdb": { autoTableName.evdbtables.Add(tablename); } break;
                        case "bsdb": { autoTableName.bsdbtables.Add(tablename); } break;
                        case "indb": { autoTableName.indbtables.Add(tablename); } break;
                        case "ipdb": { autoTableName.ipdbtables.Add(tablename); } break;
                        case "isdb": { autoTableName.isdbtables.Add(tablename); } break;
                        case "madb": { autoTableName.madbtables.Add(tablename); } break;
                        case "rcdb": { autoTableName.rcdbtables.Add(tablename); } break;
                        default: break;
                    }
                }
            }
            else
            {
                label8.Text += "0张";
            }
            autoTbName1.Clear();
            autoTbName1.Add("midb", autoTableName.midbtables);
            autoTbName1.Add("bsdb", autoTableName.bsdbtables);
            autoTbName1.Add("evdb", autoTableName.evdbtables);
            autoTbName1.Add("rcdb", autoTableName.rcdbtables);
            autoTbName1.Add("madb", autoTableName.madbtables);
            autoTbName1.Add("isdb", autoTableName.isdbtables);
            autoTbName1.Add("ipdb", autoTableName.ipdbtables);
            autoTbName1.Add("indb", autoTableName.indbtables);
            // return autoTableName;
        }


        private void dGVtable_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            List<string> result = new List<string>();
            autoTbName1.TryGetValue(currentDatabaseName, out result);
            if (e.ColumnIndex == 0 && e.RowIndex != -1)
            {
                if ((bool)this.dGVtable.Rows[e.RowIndex].Cells[0].EditedFormattedValue)
                {

                    result.Add(dGVtable.Rows[e.RowIndex].Cells[2].EditedFormattedValue.ToString());
                }
                else
                {
                    result.Remove(dGVtable.Rows[e.RowIndex].Cells[2].EditedFormattedValue.ToString());
                }
                autoTbName1[currentDatabaseName] = result;

                autoTableName.midbtables = autoTbName1["midb"];
                autoTableName.bsdbtables = autoTbName1["bsdb"];
                autoTableName.evdbtables = autoTbName1["evdb"];
                autoTableName.rcdbtables = autoTbName1["rcdb"];
                autoTableName.madbtables = autoTbName1["madb"];
                autoTableName.isdbtables = autoTbName1["isdb"];
                autoTableName.ipdbtables = autoTbName1["ipdb"];
                autoTableName.indbtables = autoTbName1["indb"];

                //遍历autoTbName1字典，将其处理成可以写入appsettings中的字符串
                autoBackupTables = "";
                int tablecount = 0;//统计选择的表和视图的个数
                foreach (KeyValuePair<string, List<string>> kvp in autoTbName1)
                {
                    List<string> value = kvp.Value;
                    foreach (string table in kvp.Value)
                    {
                        autoBackupTables += kvp.Key + "." + table + ";";
                        tablecount++;
                    }
                }

                label8.Text = "已选择要备份的数据表和视图总共有：";
                label8.Text += tablecount + "张";
                if (autoBackupTables != "")
                {
                    txtAutoBackupTables.Text = autoBackupTables;
                }
                else
                {
                    txtAutoBackupTables.Text = "(空)";
                }
            }
        }

        private void radioButton5_MouseClick(object sender, MouseEventArgs e)
        {
            allBackupSetting();
        }

        private void radioButton6_MouseClick(object sender, MouseEventArgs e)
        {
            button1.ForeColor = System.Drawing.SystemColors.ControlText;
            button1.BackColor = System.Drawing.SystemColors.ControlLight;
            button1.Enabled = true;
            button2.ForeColor = System.Drawing.SystemColors.ControlText;
            button2.BackColor = System.Drawing.SystemColors.ControlLight;
            button2.Enabled = true;
            button3.ForeColor = System.Drawing.SystemColors.ControlText;
            button3.BackColor = System.Drawing.SystemColors.ControlLight;
            button3.Enabled = true;
            button4.ForeColor = System.Drawing.SystemColors.ControlText;
            button4.BackColor = System.Drawing.SystemColors.ControlLight;
            button4.Enabled = true;
            radioButton1.Enabled = true;
            dataGridViewDBData.Enabled = true;
            dataGridViewTalbe.Enabled = false;
            radioButton3.Enabled = true;
            radioButton3.ForeColor = System.Drawing.SystemColors.ControlText;

            //选中全部的数据库
            for (int i = 0; i < dataGridViewDBData.Rows.Count; i++)
            {
                dataGridViewDBData.Rows[i].Cells[0].ReadOnly = true;
                dataGridViewDBData.Rows[i].Cells[0].Value = true;
                dataGridViewDBData.Rows[i].Cells[0].ReadOnly = false;
            }

            //清空表和视图
            dataGridViewTalbe.Rows.Clear();
        }

        private void radioButton7_MouseClick(object sender, MouseEventArgs e)
        {
            button1.ForeColor = System.Drawing.SystemColors.ControlText;
            button1.BackColor = System.Drawing.SystemColors.ControlLight;
            button1.Enabled = true;
            button2.ForeColor = System.Drawing.SystemColors.ControlText;
            button2.BackColor = System.Drawing.SystemColors.ControlLight;
            button2.Enabled = true;
            button3.ForeColor = System.Drawing.SystemColors.ControlText;
            button3.BackColor = System.Drawing.SystemColors.ControlLight;
            button3.Enabled = true;
            button4.ForeColor = System.Drawing.SystemColors.ControlText;
            button4.BackColor = System.Drawing.SystemColors.ControlLight;
            button4.Enabled = true;
            dataGridViewDBData.Enabled = true;
            dataGridViewTalbe.Enabled = true;

            //选中全部的数据库
            for (int i = 0; i < dataGridViewDBData.Rows.Count; i++)
            {
                dataGridViewDBData.Rows[i].Cells[0].Value = false;
            }
            radioButton1.Enabled = false;
            radioButton2.Checked = true;
            radioButton3.Enabled = true;

            //radioButton3.ForeColor = Color.FromArgb(200, 200, 200);            
            //radioButton4.Checked = true;
        }

        private void radioButton3_MouseClick(object sender, MouseEventArgs e)
        {
            txtAutoBackupAddress.BackColor = System.Drawing.SystemColors.Window;
            txtBackupAddress.BackColor = Color.FromArgb(160, 160, 160);
            btnChooseBackupAddress.Enabled = false;
        }

        private void radioButton4_MouseClick(object sender, MouseEventArgs e)
        {
            txtAutoBackupAddress.BackColor = Color.FromArgb(160, 160, 160);
            txtBackupAddress.BackColor = System.Drawing.SystemColors.Window;
            btnChooseBackupAddress.Enabled = true;
        }

        private void radioButton1_MouseClick(object sender, MouseEventArgs e)
        {
            radioButton4.Checked = true;
            radioButton3.Enabled = false;
        }

        private void radioButton2_MouseClick(object sender, MouseEventArgs e)
        {
            radioButton3.Enabled = true;
            radioButton3.Checked = true;
        }

        //初始化配置参数及日志界面的控件状态
        public void InitControl4()
        {
            button8.Enabled = false;
            txtChangeAutoSpan.ReadOnly = true;
            dGVdb.Enabled = false;
            dGVtable.Enabled = false;
            button11.Enabled = false;
            button9.Enabled = true;
        }

        //当点击更改按钮时对配置参数及日志界面的空间状态进行更改
        private void button9_Click(object sender, EventArgs e)
        {
            if (button9.Text == "更改")
            {
                button8.Enabled = true;
                txtChangeAutoAddress.ReadOnly = false;
                txtChangeAutoSpan.ReadOnly = false;
                dGVdb.Enabled = true;
                dGVtable.Enabled = true;
                button11.Enabled = true;
                button9.Text = "取消";
            }
            else
            {
                button9.Text = "更改";
                InitControl4();
                //配置和日志的功能中，自动加载相应的参数
                txtChangeAutoAddress.Text = GetAutoBackupAddress();
                txtChangeAutoSpan.Text = GetAutoBackupSpan();
            }
        }
        //点击“完成”时提交对appsettings自动备份配置参数的更改
        private void button11_Click(object sender, EventArgs e)
        {

            string autoAddress = "";
            int autoSpan;
            try
            {
                autoSpan = Convert.ToInt32(txtChangeAutoSpan.Text);
                autoAddress = txtChangeAutoAddress.Text.Trim();
                autoAddress = autoAddress.Replace(@"\", @"\\");
            }
            catch
            {
                MessageBox.Show("您输入的的时间间隔不正确,请输入整数天数");
                return;
            }
            ChangeAutoBackupAddress(autoAddress);
            ChangeAutoBackupSpan(autoSpan);
            ChangeAutoBackupTables(autoBackupTables);


            InitControl4();
        }

        //更改自动备份时间
        public static void ChangeAutoBackupSpan(int span)
        {
            //BackupSpan = System.Text.Encoding.UTF8.GetString(txtAutoBackupSpan.Text);        
            string ChangeBackupSpanSql = string.Format(@"update appsettings set `value`='{0}' where `key`='MysqlBackupSpan'", span);
            dbOperating.MIDB.ExecuteScript(ChangeBackupSpanSql);
        }

        //修改自动备份的地址
        public static void ChangeAutoBackupAddress(string address)
        {
            string ChangeBacukpAddressSql = string.Format(@"update appsettings set `value`='{0}' where `key`='BackupAddress'", address);
            dbOperating.MIDB.ExecuteScript(ChangeBacukpAddressSql);
        }

        //更改自动备份的数据表
        public static void ChangeAutoBackupTables(string tables)
        {
            string ChangeBackupTableSql = string.Format(@"update appsettings set `value`='{0}' where `key`='MysqlBackupTables'", tables);
            dbOperating.MIDB.ExecuteScript(ChangeBackupTableSql);
        }


        //查找备份日志
        public void backuptranslog()
        {
            try
            {
                DataSet dst = new DataSet();
                DataTable dt = new DataTable();
                //List<string> timetranslog = new List<string>();
                string selectbackuptranslog = string.Format("select orders.SubmitTime,orders.OrderCode from orders where orders.TASKS='ITFullBackup'");
                //timetranslog = dbOperating.MIDB.myExcuteReader(selectbackuptranslog);
                dst = dbOperating.MIDB.GetDataSet(selectbackuptranslog);
                dst.Tables.Add(dt);

                for (int i = 0; i < dst.Tables[0].Rows.Count; i++)
                {
                    DataGridViewRow row = new DataGridViewRow();
                    int index = dataGridView1.Rows.Add(row);
                    dataGridView1.Rows[index].Cells[0].Value = dst.Tables[0].Rows[i][0].ToString();
                    dataGridView1.Rows[index].Cells[2].Value = "自动备份";
                    dataGridView1.Rows[index].Cells[3].Value = "详情请查看日志";
                    dataGridView1.Rows[index].Cells[4].Value = "打开";
                    dataGridView1.Rows[index].Cells[1].Value = dst.Tables[0].Rows[i][1].ToString();
                }
            }
            catch (Exception e) 
            { 
                throw (e); 
            }

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentCell.ColumnIndex == 4)
            {
                FrmOrderLogInfo frm = new FrmOrderLogInfo(dataGridView1.CurrentRow.Cells[1].Value.ToString());
                frm.Show();
            }
        }

        void _noticeMsger_ReciewedMessage(QRST_DI_MS_Basis.backupMessage msg)
        {
            int tag = 0;//做标记            
            try
            {
                for (int i = 0; i < dgvFileBackupMonitor.Rows.Count; i++)
                {
                    if (dgvFileBackupMonitor.Rows[i].Cells[1].Value.ToString().Trim() == msg.orderName.ToString().Trim())
                    {
                        tag = 1;
                        addDgvFileBackupMonitor(msg, i);
                        return;
                    }
                }
            }
            catch (Exception e)
            { throw e; }
            DataGridViewRow row = new DataGridViewRow();
            int index = dgvFileBackupMonitor.Rows.Add(row);
            addDgvFileBackupMonitor(msg, index);
        }


        public delegate void controlUI(backupMessage msg, int i);

        private void addmsg(backupMessage msg1, int i)
        {
            dgvFileBackupMonitor.Rows[i].Cells[0].Value = msg1.ipAddress;
            dgvFileBackupMonitor.Rows[i].Cells[1].Value = msg1.orderName;
            dgvFileBackupMonitor.Rows[i].Cells[2].Value = msg1.originalFile;
            dgvFileBackupMonitor.Rows[i].Cells[3].Value = msg1.targetFile;
            dgvFileBackupMonitor.Rows[i].Cells[5].Value = msg1.backupSize + "/" + msg1.allSize;
            dgvFileBackupMonitor.Rows[i].Cells[6].Value = msg1.usedTime;
            dgvFileBackupMonitor.Rows[i].Cells[7].Value = msg1.backupType;
            dgvFileBackupMonitor.Rows[i].Cells[4].Value = msg1.backupStatus;
        }

        private void addDgvFileBackupMonitor(backupMessage msg1, int i)
        {
            controlUI controlui = new controlUI(addmsg);
            dgvFileBackupMonitor.BeginInvoke(controlui, new object[] { msg1, i });
            //this.BeginInvoke(controlui, new object[] { msg1, i }); 
            //dgvFileBackupMonitor.Rows[i].Cells[0].Value = msg1.ipAddress;
            //dgvFileBackupMonitor.Rows[i].Cells[1].Value = msg1.orderName;
            //dgvFileBackupMonitor.Rows[i].Cells[2].Value = msg1.originalFile;
            //dgvFileBackupMonitor.Rows[i].Cells[3].Value = msg1.targetFile;
            //dgvFileBackupMonitor.Rows[i].Cells[5].Value = msg1.backupSize + "/" + msg1.allSize;
            //dgvFileBackupMonitor.Rows[i].Cells[6].Value = msg1.usedTime;
            //dgvFileBackupMonitor.Rows[i].Cells[7].Value = msg1.backupType;
            //dgvFileBackupMonitor.Rows[i].Cells[4].Value = msg1.backupStatus;
        } 


        #region//由于跨线程无法操作控件，通过以下部分可以实现控件操作
        //delegate void SetControlValueCallback(Control oControl, string propName, object propValue);
        //private void SetControlPropertyValue(Control oControl, string propName, object propValue)
        //{
        //    if (oControl.InvokeRequired)
        //    {
        //        SetControlValueCallback d = new SetControlValueCallback(SetControlPropertyValue);
        //        oControl.Invoke(d, new object[] { oControl, propName, propValue });
        //    }
        //    else
        //    {
        //        Type t = oControl.GetType();
        //        PropertyInfo[] props = t.GetProperties();
        //        foreach (PropertyInfo p in props)
        //        {
        //            if (p.Name.ToUpper() == propName.ToUpper())
        //            {
        //                p.SetValue(oControl, propValue, null);
        //            }
        //        }
        //    }
        //}
        #endregion

    }

}
