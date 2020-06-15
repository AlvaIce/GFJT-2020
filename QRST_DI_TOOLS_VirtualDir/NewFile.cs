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
    //    /// �½��ļ���
    //    /// </summary>
    //    /// <param name="sender"></param>
    //    /// <param name="e"></param>
    //    private void buttonX1_Click(object sender, EventArgs e)
    //    {
    //        //���ȸ�ע����
    //        //this.label1.Text = "";
    //        //if (this.textBoxX1.Text != "")
    //        //{
                
    //        //    Ribbon ribbon = (Ribbon)this.Owner;
    //        //    ribbon.strNewFile = this.textBoxX1.Text;
    //        //    this.Close();
    //        //}
    //        //else
    //        //{
    //        //    this.label1.Text = "�ļ�������Ϊ�գ�";
    //        //}

    //        //��Ҫ�ж� �û����Ǹ�Ŀ¼����ӵ��½��ļ��� ��Ҫ����???
    //        if (targetNode==null)
    //        {
    //            MessageBox.Show("��û��ѡ���ļ���!");
    //            return;           
    //        }

    //        if (!VirtualDirEngine.IsDir(targetNode.Tag.ToString()))
    //        {
    //            MessageBox.Show("��ѡ�еĲ����ļ���!");
    //            return;           
    //        }

    //        if (this.textBoxX1.Text == "")
    //        {
    //            MessageBox.Show("����д�ļ�������!");
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
        /// �½��ļ���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonX1_Click(object sender, EventArgs e)
        {
            //��Ҫ�ж� �û����Ǹ�Ŀ¼����ӵ��½��ļ��� ��Ҫ����???
            if (targetNode == null)
            {
                MessageBox.Show("��û��ѡ���ļ���!");
                return;
            }

            if (!VirtualDirEngine.IsDir(targetNode.Tag.ToString()))
            {
                MessageBox.Show("��ѡ�еĲ����ļ���!");
                return;
            }

            if (this.textBoxX1.Text == "")
            {
                MessageBox.Show("����д�ļ�������!");
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