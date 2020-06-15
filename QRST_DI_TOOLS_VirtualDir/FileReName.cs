using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using QRST_DI_MS_Basis;
using MySql.Data.MySqlClient;
using QRST_DI_DS_Basis;
using QRST_DI_TS_Basis;
using QRST_DI_DS_Basis.DBEngine;
using DevComponents.AdvTree;
using QRST_DI_MS_Basis.VirtualDir;

namespace WindowsFormsApplication2
{
    //public partial class FileReName : DevComponents.DotNetBar.Office2007Form
    //{
    //    public Node targetNode;
    //    public string Name { get; private set; }
    //    public bool Ok = false;
    //    public FileReName()
    //    {
    //        InitializeComponent();
    //        targetNode = null;
    //    }
    //    public FileReName(Node selectnode)
    //    {
    //        InitializeComponent();
    //        targetNode = selectnode;
    //    }

    //    private void buttonX1_Click(object sender, EventArgs e)
    //    {
    //        if (targetNode == null)
    //        {
    //            MessageBox.Show("您没有选中文件夹!");
    //            return;
    //        }

    //        if (!VirtualDirEngine.IsDir(targetNode.Tag.ToString()))
    //        {
    //            MessageBox.Show("您选中的不是文件夹!");
    //            return;
    //        }

    //        if (this.textBoxX1.Text == "")
    //        {
    //            MessageBox.Show("请填写文件夹名称!");
    //            return;
    //        }

    //        this.Name = this.textBoxX1.Text;
    //        this.Ok = true;

    //        _vde.ReName(targetNode.Tag.ToString(), Name);
    //        this.Close();
    //    }

    //    private void buttonX2_Click(object sender, EventArgs e)
    //    {
    //        this.Close();
    //    }
    //}

     
    public partial class FileReName : DevComponents.DotNetBar.Office2007Form
    {
        public Node targetNode;
        public string targetTitle;
        public string Name;
        public bool Ok = false;
        public VirtualDirEngine _vde;
        public FileReName()
        {
            InitializeComponent();
            targetNode = null;
            targetTitle = null;
        }
        public FileReName(Node selectnode, string title, VirtualDirEngine _ve)
        {
            InitializeComponent();
            targetNode = selectnode;
            targetTitle = title;
            _vde = _ve;
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            if (targetNode == null)
            {
                MessageBox.Show("您没有选中文件夹!");
                return;
            }

            if (!VirtualDirEngine.IsDir(targetNode.Tag.ToString()))
            {
                MessageBox.Show("您选中的不是文件夹!");
                return;
            }

            if (this.textBoxX1.Text == "")
            {
                MessageBox.Show("请填写文件夹名称!");
                return;
            }
            this.Name = this.textBoxX1.Text;
            this.Ok = true;
            _vde.ReName(targetNode.Tag.ToString(), Name);
            this.Close();
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
