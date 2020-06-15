using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using QRST_DI_MS_Basis;
using QRST_DI_MS_Basis.UserRole;
using QRST_DI_MS_Basis;
using MySql.Data.MySqlClient;
using QRST_DI_DS_Basis;
using QRST_DI_TS_Basis;
using QRST_DI_DS_Basis.DBEngine;
using DevComponents.AdvTree;
using QRST_DI_MS_Basis.VirtualDir;

namespace WindowsFormsApplication2
{
    //public partial class NewFile : DevComponents.DotNetBar.Office2007Form
    //{

    //    public NewFile()
    //    {
    //        InitializeComponent();
    //        targetNode = null;
    //    }

    //    public NewFile(Node selectnode)
    //    {
    //        InitializeComponent();
    //        targetNode = selectnode;
    //    }

    //    public Node targetNode;

    //    public string Name { get; private set; }
    //    public string Remark { get; private set; }
    //    public bool Ok = false;
    //    /// <summary>
    //    /// 新建文件夹
    //    /// </summary>
    //    /// <param name="sender"></param>
    //    /// <param name="e"></param>
    //    private void buttonX1_Click(object sender, EventArgs e)
    //    {
    //        //我先给注释了
    //        //this.label1.Text = "";
    //        //if (this.textBoxX1.Text != "")
    //        //{
                
    //        //    Ribbon ribbon = (Ribbon)this.Owner;
    //        //    ribbon.strNewFile = this.textBoxX1.Text;
    //        //    this.Close();
    //        //}
    //        //else
    //        //{
    //        //    this.label1.Text = "文件名不能为空！";
    //        //}

    //        //需要判断 用户在那个目录下添加的新建文件夹 需要考虑???
    //        if (targetNode==null)
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

    //        this.Name = this.textBoxX1.Text.Trim();
    //        this.Remark = this.textBoxX2.Text.Trim();
    //        this.Ok = true;

    //        _vde.NewDir(targetNode.Tag.ToString(), Name,Remark);
    //        this.Close();

    //    }

    //    private void buttonX2_Click(object sender, EventArgs e)
    //    {
    //        this.Close();
    //    }

        
    //}
    public partial class NewFile : DevComponents.DotNetBar.Office2007Form
    {
        public NewFile()
        {
            InitializeComponent();
            targetNode = null;
        }

        public NewFile(Node selectnode, string title, VirtualDirEngine _ve)
        {
            InitializeComponent();
            targetNode = selectnode;
            targetTitle = title;
            _vde = _ve;
        }

        public Node targetNode;
        public string Name { get; private set; }
        public string Remark { get; private set; }
        public bool Ok = false;
        public string targetTitle;
        private static VirtualDirEngine _vde;
        /// <summary>
        /// 新建文件夹
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonX1_Click(object sender, EventArgs e)
        {
            //需要判断 用户在那个目录下添加的新建文件夹 需要考虑???
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

            this.Name = this.textBoxX1.Text.Trim();
            this.Remark = this.textBoxX2.Text.Trim();
            this.Ok = true;
            _vde.NewDir(targetNode.Tag.ToString(), Name, Remark);
            this.Close();

        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}