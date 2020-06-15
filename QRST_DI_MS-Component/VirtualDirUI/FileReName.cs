using System;
using System.Windows.Forms;
using DevComponents.AdvTree;
using QRST_DI_MS_Basis.VirtualDir;

namespace QRST_DI_MS_Component.VirtualDirUI
{
     
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
