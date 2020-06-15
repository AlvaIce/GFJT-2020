using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using QRST_DI_DS_Metadata.MetaDataDefiner;
using System.IO;
using DevExpress.XtraEditors;

namespace QRST_DI_MS_Console.UserInterfaces
{
    public partial class mucExternalDbIntergration : UserControl
    {
        private bool isFirstLoad = true;
        public int selectedIndex = -1;
        //任务单击事件
        public delegate void ClickEventHandler(object sender, EventArgs e);
        public event ClickEventHandler ClickEvent;

        public mucExternalDbIntergration()
        {
            InitializeComponent();
        }

        private void linkurl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!string.IsNullOrEmpty(linkurl.Text) )
            {
                try
                {
                    System.Diagnostics.Process.Start(linkurl.Text);   
                }
                catch(Exception ex)
                {
                    XtraMessageBox.Show("无法连接所指定的地址，系统找不到指定的文件！");
                }
            }
              
        }

        private void mucExternalDbIntergration_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible&&isFirstLoad)
            {
                InitializeLst();

                isFirstLoad = false;
            }
        }

        public void InitializeLst()
        {
            flowLayoutPanel1.Controls.Clear();
            List<Externaldb> externalLst = Externaldb.GetList("",TheUniversal.MIDB.sqlUtilities);
            for (int i = 0 ; i < externalLst.Count ;i++ )
            {
                CtrlExternalDBMdl ctrlexternalMdl = new CtrlExternalDBMdl();
                ctrlexternalMdl.externalDb = externalLst[i];
                ctrlexternalMdl.width = flowLayoutPanel1.Width;
           //     ctrlexternalMdl.Anchor = AnchorStyles.Left | AnchorStyles.Right;
              //  ctrlexternalMdl.Dock = DockStyle.Fill;
                ctrlexternalMdl.ClickEvent += DbClick;
               flowLayoutPanel1.Controls.Add(ctrlexternalMdl);
            }
            if (externalLst.Count > 0)
            {
                externalDb = ((CtrlExternalDBMdl)flowLayoutPanel1.Controls[0]).externalDb; 
            }
        }

        public void DbClick(object sender, EventArgs e)
        {
            selectedIndex = flowLayoutPanel1.Controls.IndexOf(((CtrlExternalDBMdl)sender));
            refreshInfo(((CtrlExternalDBMdl)sender).externalDb);

            if (ClickEvent!= null)
            {
                ClickEvent(sender,e);
            }
        }

        public void refreshInfo(Externaldb externaldb)
        {
            externalDb = externaldb;
        }

        private Externaldb _externalDb;

        public Externaldb externalDb
        {
            set
            {
                _externalDb = value;
                if (_externalDb.IMAGE != null)
                { 
                    MemoryStream ms = new MemoryStream(_externalDb.IMAGE);
                   pictureBox1.Image = Image.FromStream(ms);
                   ms.Close();
                }
                else
                {
                    pictureBox1.Image = global::QRST_DI_MS_Console.Properties.Resources.卫星;
                }
               
                labelControldbName.Text = _externalDb.NAME;
                labelDescription.Text = _externalDb.DESCRIPTION;
                linkurl.Text = _externalDb.URL;
            }
            get { return _externalDb; }
        }

        public void ClearInterface()
        {
            labelDescription.Text = "";
            labelControldbName.Text = "";
            linkurl.Text = "";
            pictureBox1.Image = global::QRST_DI_MS_Console.Properties.Resources.卫星;
        }
    }
}
