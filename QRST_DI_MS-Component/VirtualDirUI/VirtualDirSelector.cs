using System;
using System.Windows.Forms;
 
namespace QRST_DI_MS_Component.VirtualDirUI
{
    public partial class VirtualDirSelector : Form
    {
        public DevComponents.AdvTree.Node SelectNode { get; private set; }
        public string target;
        public string VDCode
        {
            get
            {
                if (this.SelectNode != null)
                {
                    return this.SelectNode.Tag.ToString();
                }
                else if (this.target != null)
                {
                    return target;
                   
                }
                else
                {
                    return "";
                }
            }
            set
            {
                target = value;
                
            }


        }
        public string VDName
        {
            get
            {
                if (this.SelectNode != null)
                {
                    return this.SelectNode.Text.ToString();
                }
                else
                {
                    return "";
                }
            }
        }
        public string VDPath
        {
            get
            {
                string vdpath = "";
                if (this.SelectNode != null)
                {
                    DevComponents.AdvTree.Node nd = this.SelectNode;
                    while (nd != null)
                    {
                        vdpath = nd.Text + @"\" + vdpath;
                        nd = nd.Parent;
                    }
                    return vdpath;
                }
                else
                {
                    return "";
                }
            }
        }
        public bool IsCreated { get; private set; }
        public string _user { get; private set; }
        private string _password;

        public VirtualDirSelector()
        {
            InitializeComponent();
            virtualDirUC1.MainVirtualDirCtrl.ShowCheckBox = false;
            virtualDirUC1.MainVirtualDirCtrl.ShowFileNode = false;
            virtualDirUC1.MainVirtualDirCtrl.ShowContextMenu = false;
            virtualDirUC1.MainVirtualDirCtrl._AdvTree.MultiSelect = false;
            virtualDirUC1.MainVirtualDirCtrl._AdvTree.NodeDoubleClick += new DevComponents.AdvTree.TreeNodeMouseEventHandler(_AdvTree_NodeDoubleClick);
            virtualDirUC1.MainVirtualDirCtrl.MouseClick += new MouseEventHandler(MainVirtualDirCtrl_MouseClick);
            IsCreated = false;
        }

        public void Create(string user,string pwd)
        {
            virtualDirUC1.InitializeVirtualDir(user, pwd);
            _user = user;
            _password = pwd;
            IsCreated = true;
        }

        void MainVirtualDirCtrl_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button== System.Windows.Forms.MouseButtons.Right)
            {
                Cancel();
            }
        }

        void _AdvTree_NodeDoubleClick(object sender, DevComponents.AdvTree.TreeNodeMouseEventArgs e)
        {
            //virtualDirUC1.MainVirtualDirCtrl._AdvTree.SelectedNode = e.Node;

            //OK();
        }


        private void btn_ok_Click(object sender, EventArgs e)
        {
            OK();
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            Cancel();
        }


        public void OK()
        {
            SelectNode = virtualDirUC1.MainVirtualDirCtrl._AdvTree.SelectedNode;
            if (SelectNode!=null)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Hide();
            }
            else
            {
                MessageBox.Show("请选择目标目录");
            }
        }

        private void Cancel()
        {
            SelectNode = null;
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Hide();
        }

        public void UpdateVirtualDir()
        {
            virtualDirUC1.InitializeVirtualDir(_user, _password);
        }

        public DialogResult ShowSelectorForm()
        {
            UpdateVirtualDir();
            return this.ShowDialog();
        }

    }
}
