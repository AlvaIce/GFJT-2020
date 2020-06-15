using System;
using System.Windows.Forms;
using DevComponents.AdvTree;
using QRST_DI_MS_Basis.VirtualDir;

namespace QRST_DI_MS_Component.VirtualDirUI
{ 
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