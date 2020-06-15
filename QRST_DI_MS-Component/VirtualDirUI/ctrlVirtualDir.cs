using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using DevComponents.AdvTree;
using QRST_DI_MS_Basis.VirtualDir;
using QRST_DI_Resources;
using System.Linq;
 
namespace QRST_DI_MS_Component.VirtualDirUI
{
    public partial class ctrlVirtualDir : UserControl
    {
        public VirtualDirEngine _vde;
        
        public string targetDir { get; set; }
        public string user { get; set; }
        public string pwd { get; set; }
        public string Title { get; set; }
        public string str { get; set; }
        public event EventHandler CopyBtnVisitableChanged;
        public event EventHandler PastBtnVisitableChanged;
        public bool iscopy = false;
        /// <summary>
        /// 获取要复制的节点
        /// </summary>
        public List<Node> checkednode;
        public List<Node> nodeparents;
        public AdvTree _AdvTree { get; set; }


        public bool ShowCheckBox { get; set; }
        public bool ShowFileNode { get; set; }
        public bool ShowContextMenu { get; set; }
        public event EventHandler OnDataImportItemClick;

        public ctrlVirtualDir()
        {

            InitializeComponent();
            AdvTreeSettings.SelectedScrollIntoViewHorizontal = false;
            _AdvTree = this.advTree1;
            _vde = new VirtualDirEngine();
            ShowCheckBox = true;
            ShowFileNode = true;
            ShowContextMenu = true;
            initializeToolStripMenuItemClick();
        }

        private void FindCheckedNode(Node advtn)
        {
            // checkednode = new List<Node>();
            if (advtn == null)
            {
                return;
            }
            if (advtn.Checked == true)
            {
                //如果该节点为选中，则所有子节点都为选中
                checkednode.Add(advtn);
                return;
            }
            if (advtn.Nodes.Count > 0)
            {
                foreach (Node nd in advtn.Nodes)
                {
                    FindCheckedNode(nd);
                }
            }

        }


        /// <summary>
        /// 要下载的文件节点列表
        /// </summary>
        List<Node> checkedfilenode;
        private void FindCheckedFileNode(Node advtn)
        {
            // checkednode = new List<Node>();
            if (advtn == null)
            {
                return;
            }
            if (!VirtualDirEngine.IsDir(advtn.Tag.ToString()))
            {
                if (advtn.Checked == true)
                {
                    //如果该节点为选中，且为文件
                    checkedfilenode.Add(advtn);
                    return;
                }
            }
            else
            {
                if (advtn.Nodes.Count > 0)
                {
                    foreach (Node nd in advtn.Nodes)
                    {
                        FindCheckedFileNode(nd);
                    }
                }
            }

        }

        private void transformnode(TreeNode ptn, out Node advtn)
        {
            advtn = null;
            if (ptn == null)
            {
                return;
            }

            List<TreeNode> rootNode = new List<TreeNode>();
            if (VirtualDirEngine.IsDir(ptn.Tag.ToString()))
            {
                advtn = new Node(ptn.Text);
                advtn.Text = ptn.Text;
                advtn.Tag = ptn.Tag;
                rootNode = getAllNodeList(ptn);
                advtn.Image = global::QRST_DI_MS_Component.Properties.Resources.FolderClosed1;
                advtn.ImageExpanded = global::QRST_DI_MS_Component.Properties.Resources.FolderOpen1;
                advtn.Cells.Add(new Cell("文件夹"));
                advtn.Cells.Add(new Cell(""));
                advtn.Cells.Add(new Cell(_vde.GetCreatTimeByVirtualDirCode(ptn.Tag.ToString())));
                advtn.Cells.Add(new Cell(_vde.GetModifyTimeByVirtualDirCode(ptn.Tag.ToString())));
                advtn.Cells.Add(new Cell(ptn.Tag.ToString()));
                advtn.Cells.Add(new Cell(_vde.GetRemarkByVirtualDirCode(ptn.Tag.ToString())));
                advtn.CheckBoxVisible = ShowCheckBox;
                advtn.ExpandVisibility = eNodeExpandVisibility.Visible;
            }
            else
            {
                if (ShowFileNode)
                {
                    advtn = new Node(ptn.Text);

                    advtn.Nodes.Clear();
                    advtn.Text = ptn.Text;
                    advtn.Tag = ptn.Tag;

                    advtn.Image = global::QRST_DI_MS_Component.Properties.Resources.Document1;

                    advtn.Cells.Add(new Cell("文件"));
                    advtn.Cells.Add(new Cell(""));
                    advtn.Cells.Add(new Cell(_vde.GetCreatTimeByFileLinkCode(ptn.Tag.ToString())));
                    advtn.Cells.Add(new Cell(""));
                    advtn.Cells.Add(new Cell(ptn.Tag.ToString()));
                    advtn.Cells.Add(new Cell(""));

                    advtn.CheckBoxVisible = ShowCheckBox;
                    advtn.ExpandVisibility = eNodeExpandVisibility.Visible;
                }

            }

            foreach (TreeNode tn in rootNode)
            {
                Node advctn = null;
                transformnode(tn, out advctn);
                if (advctn != null)
                {
                    advtn.Nodes.Add(advctn);
                }
            }
        }

        public void UpdateContent(TreeNode roottn)
        {
            advTree1.BeginUpdate();
            advTree1.Nodes.Clear();
            Node advrootNode = null;
            transformnode(roottn, out advrootNode);
            advTree1.Nodes.Add(advrootNode);
            if (advrootNode != null && advrootNode.Parent == null && advrootNode.Text.ToUpper() == "ROOT")
            {
                advrootNode.Text = "我的空间";
            } 
            
            advTree1.EndUpdate();
            if (!_vde.IsRegister) _vde.Register(this.user, this.pwd);
            //advTree1.ExpandAll();
            //roottn.Expand();
            advTree1.Nodes[0].Expand();//20161212

           

        }

        /// <summary>
        /// 控制节点的顺序，从而使得文件夹显示在前面，文件显示在后
        /// </summary>
        /// <param name="ptn"></param>
        /// <returns></returns>
        public List<TreeNode> getAllNodeList(TreeNode ptn)
        {
            int sum = ptn.Nodes.Count;
            List<TreeNode> rootNode = new List<TreeNode>();
            for (int i = 0; i < sum; i++)
            {
                if (VirtualDirEngine.IsDir(ptn.Nodes[i].Tag.ToString()))
                {
                    rootNode.Add(ptn.Nodes[i]);

                }
            }
            for (int i = 0; i < sum; i++)
            {
                if (!VirtualDirEngine.IsDir(ptn.Nodes[i].Tag.ToString()))
                {
                    rootNode.Add(ptn.Nodes[i]);

                }

            }
            return rootNode;
        }

        public void setGrouPanleName(String username)
        {
            this.groupPanel1.Text = username;
        }


        private void advTree1_AfterCheck(object sender, AdvTreeCellEventArgs e)
        {
            if (e.Action == eTreeAction.Mouse)
            {
                Node childNode = e.Cell.Parent;
                Node parentNode = childNode.Parent;
                if (e.Cell.Parent.Nodes.Count > 0)
                {
                    string state = e.Cell.Parent.CheckState.ToString();
                    if (state == "Checked")
                        checkAllChildNode(e.Cell.Parent.Nodes);
                    else
                        uncheckAllChildNode(e.Cell.Parent.Nodes);

                }
                if (parentNode != null) judgeNodeChecked(parentNode, childNode);
            }

        }

        private bool HasNodeChecked(Node nd)
        {
            if (nd.Checked)
            {
                return true;
            }
            else if (nd.Nodes.Count > 0)
            {
                foreach (Node cnd in nd.Nodes)
                {
                    if (HasNodeChecked(cnd))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 将所有的节点都选中
        /// </summary>
        /// <param name="node"></param>
        private void checkAllChildNode(NodeCollection node)
        {
            for (int i = 0; i < node.Count; i++)
            {
                if (node[i].Nodes.Count > 0)
                {
                    node[i].Checked = true;
                    checkAllChildNode(node[i].Nodes);
                }
                else node[i].Checked = true;
            }

        }
        /// <summary>
        /// 将所有的节点都取消选中
        /// </summary>
        /// <param name="node"></param>
        private void uncheckAllChildNode(NodeCollection node)
        {

            for (int i = 0; i < node.Count; i++)
            {
                if (node[i].Nodes.Count > 0)
                {
                    node[i].Checked = false;
                    uncheckAllChildNode(node[i].Nodes);
                }
                else node[i].Checked = false;

            }

        }

        //判断子节点的状态，然后对根节点做出相应的勾选
        private void judgeNodeChecked(Node parentNode, Node childNode)
        {
            string checkState = childNode.CheckState.ToString();
            for (int i = 0; i < parentNode.Nodes.Count; i++)
            {
                if (parentNode.Nodes[i].CheckState.ToString() != checkState) { parentNode.Checked = false; break; }
                else
                {
                    if (i == (parentNode.Nodes.Count - 1) && !parentNode.Nodes[i].Checked) parentNode.Checked = parentNode.Nodes[i].Checked;        //只取消
                }
            }
            if (parentNode.Parent != null)
            {
                judgeNodeChecked(parentNode.Parent, parentNode);
            }

        }

        private void advTree1_NodeClick(object sender, TreeNodeMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point ClickPoint = new Point(e.X, e.Y);
                Node CurrentNode = advTree1.GetNodeAt(ClickPoint);
                advTree1.SelectedNode = CurrentNode;

                if (CurrentNode != null)
                {
                    if (!VirtualDirEngine.IsDir(CurrentNode.Tag.ToString()))
                    {
                        this.新建ToolStripMenuItem.Visible = false;
                        this.toolStripMenuItem1.Visible = true;
                        this.重命名ToolStripMenuItem.Visible = false;
                    }
                    else
                    {
                        this.新建ToolStripMenuItem.Visible = true;
                        this.toolStripMenuItem1.Visible = false;
                        this.重命名ToolStripMenuItem.Visible = true;
                    }
                    if (!VirtualDirEngine.IsDir(CurrentNode.Tag.ToString()))
                    {
                        this.粘贴ToolStripMenuItem.Visible = false;
                    }

                    //if 选中对象不为空
                    //复制为true，否则为false
                    if (advTree1.SelectedNode == null && advTree1.Nodes.Count > 0 && !HasNodeChecked(advTree1.Nodes[0]))
                    {
                        this.复制ToolStripMenuItem.Visible = false;
                        if (CopyBtnVisitableChanged != null)
                        {
                            CopyBtnVisitableChanged(true, new EventArgs());
                        }
                    }
                    else
                    {
                        this.复制ToolStripMenuItem.Visible = true;
                        if (PastBtnVisitableChanged != null)
                        {
                            PastBtnVisitableChanged(true, new EventArgs());
                        }
                    }
                    //if List（粘贴板）不为空
                    //粘贴为true，否则为false
                    if (VirtualDirUC.commonCheckNode.Count > 0)
                    {
                        this.粘贴ToolStripMenuItem.Visible = true;

                    }
                    else
                    {
                        this.粘贴ToolStripMenuItem.Visible = false;

                    }
                    this.contextMenuStrip1.Show(this.PointToScreen(new Point(e.X, e.Y + 20)));
                }
            }
        }

        public void NewDir()
        {
            if (advTree1.SelectedNode == null)
            {
                MessageBox.Show("您没有选中文件夹!");
                return;
            }
            Node node = this.advTree1.SelectedNode;
            if (!VirtualDirEngine.IsDir(node.Tag.ToString()))
            {
                MessageBox.Show("您选中的不是文件夹!");
                return;
            }
            NewFile newfile = new NewFile(node, this.Title, _vde);
            newfile.ShowDialog();
            if(newfile.Ok)
            UpdateOneNode(node);//20161212
            // TreeNode roottn = getNeedUpdateTree();
            // UpdateContent(roottn);
        }
        public void ReNameDir()
        {
            if (advTree1.SelectedNode == null)
            {
                MessageBox.Show("您没有选中文件夹!");
                return;
            }
            if (advTree1.SelectedNode.Text == "我的空间")
            {
                MessageBox.Show("根目录名称不能被修改!");
                return;
            }
            Node node = this.advTree1.SelectedNode;
            if (!VirtualDirEngine.IsDir(node.Tag.ToString()))
            {
                MessageBox.Show("您选中的不是文件夹!");
                return;
            }
            FileReName filerename = new FileReName(node, this.Title, _vde);
            filerename.ShowDialog();
            //TreeNode roottn = getNeedUpdateTree();
            //UpdateContent(roottn);
            UpdateOneNode(node.Parent);//20161212

        }

        /// <summary>
        /// 获取树中选择的文件节点的qrstcode列表，用以前台的下载
        /// </summary>
        /// <returns></returns>
        public List<string> GetDownLoadFileListQRSTCODE()
        {
            List<string> downloadfilecodelist = new List<string>();
            if (checkedfilenode == null) checkedfilenode = new List<Node>();
            checkedfilenode.Clear();
            FindCheckedFileNode(_AdvTree.Nodes[0]);

            foreach (Node nd in checkedfilenode)
            {
                downloadfilecodelist.Add(nd.Tag.ToString());

            }
            return downloadfilecodelist;
        }

        public void Copy()
        {
            VirtualDirUC.commonCheckNode.Clear();
            if ((advTree1.SelectedNode == null && advTree1.Nodes.Count > 0 && !HasNodeChecked(advTree1.Nodes[0])))
            {
                MessageBox.Show("您没有选择要复制的文件夹!");
                return;
            }
            if (!(HasNodeChecked(advTree1.Nodes[0])))
            {
                MessageBox.Show("您没有选择要复制的文件夹!");
                return;

            }
            Node node = this.advTree1.Nodes[0];
            checkednode = new List<Node>();
           FindCheckedNode(node);
            iscopy = true;
            VirtualDirUC.commonCheckNode = checkednode;

        }
        public void Move()
        {
            VirtualDirUC.commonCheckNode.Clear();
            if ((advTree1.SelectedNode == null && advTree1.Nodes.Count > 0 && !HasNodeChecked(advTree1.Nodes[0])))
            {
                MessageBox.Show("您没有选择要移动的文件夹!");
                return;
            }
            if (!(HasNodeChecked(advTree1.Nodes[0])))
            {
                MessageBox.Show("您没有选择要移动的文件夹!");
                return;

            }
            Node node = this.advTree1.Nodes[0];
            checkednode = new List<Node>();
            FindCheckedNode(node);
            iscopy = true;
            VirtualDirUC.commonCheckNode = checkednode;

           

        }
        public void Paste()
        {
            if (advTree1.SelectedNode == null)
            {
                MessageBox.Show("您没有选中文件夹!");
                return;
            }
              Node node = this.advTree1.SelectedNode;
              if (!VirtualDirEngine.IsDir(node.Tag.ToString()))
              {
                  MessageBox.Show("不能粘贴到该文件下!");
                  return;
              
              }

            Node[] copycode = VirtualDirUC.commonCheckNode.ToArray();
          
            
                foreach (Node cc in copycode)
                {
                    if (VirtualDirEngine.IsDir(cc.Tag.ToString()))
                    {
                        if (str == null)
                        {
                            _vde.MultiUserCopyDir(this.user, this.pwd, node.Tag.ToString(), cc.Tag.ToString());
                        }
                        else
                            _vde.CopyDir(node.Tag.ToString(), cc.Tag.ToString());
                    }
                    else
                    {
                        if (str == null)
                        {
                            _vde.MultiUserCopyFileLink(this.user, this.pwd, node.Tag.ToString(), cc.Tag.ToString());
                        }
                        else
                            _vde.CopyFileLink(node.Tag.ToString(), cc.Tag.ToString());
                    }
                }

                //TreeNode roottn = getNeedUpdateTree();
                //UpdateContent(roottn);
                UpdateOneNode(node);//20161212
                iscopy = false;
            

        }
        public void DeleteDir()
        {
            if (advTree1.SelectedNode == null)
            {
                MessageBox.Show("您没有选中文件夹!");
                return;
            }
            if (advTree1.SelectedNode.Text == "我的空间")
            {
                MessageBox.Show("根目录不能被删除!");
                return;
            }
            Node node = this.advTree1.SelectedNode;
            if (VirtualDirEngine.IsDir(node.Tag.ToString()))
            {
                _vde.DelDir(node.Tag.ToString());
            }
            else
            {
                _vde.DelFileLink(node.Parent.Tag.ToString(), node.Tag.ToString());
            }
            //TreeNode roottn = getNeedUpdateTree();
            //UpdateContent(roottn);
            UpdateOneNode(node.Parent);//20161212

        }
        public void MoveDir()
        {
            Node node = this.advTree1.SelectedNode;
            if (advTree1.SelectedNode == null)
            {
                MessageBox.Show("您没有选中文件夹!");
                return;
            }
            if (advTree1.SelectedNode.CheckState == CheckState.Checked)
            {
                MessageBox.Show("目标文件夹是源文件夹下的子文件，不能移动!");
                return;
            }
            if (!VirtualDirEngine.IsDir(node.Tag.ToString()))
            {
                MessageBox.Show("不能粘贴到该文件下!");
                return;

            }
            Node[] copycode = VirtualDirUC.commonCheckNode.ToArray();
            nodeparents = new List<Node>();
            for (int i = copycode.Length-1; i > -1; i--)
            {
                if (VirtualDirEngine.IsDir(copycode[i].Tag.ToString()))
                {
                    if (str == null)
                    {
                        _vde.MultiUserMoveDir(this.user, this.pwd, node.Tag.ToString(), copycode[i].Tag.ToString());
                    }
                    _vde.MoveDir(copycode[i].Tag.ToString(), node.Tag.ToString());
                }
                else
                {
                    if (str == null)
                    {
                        _vde.MultiUserMoveFileLink(this.user, this.pwd, copycode[i].Parent.Tag.ToString(), node.Tag.ToString(), copycode[i].Tag.ToString());
                    }
                    _vde.MoveFileLink(copycode[i].Parent.Tag.ToString(), node.Tag.ToString(), copycode[i].Tag.ToString());                 
                }
                
                //copycode[i].Parent.Nodes.Remove(copycode[i]);
                //UpdateOneNode(copycode[i].Parent);
               
                nodeparents.Add(copycode[i].Parent);
            }
            foreach (Node cc in nodeparents)
            {
                UpdateOneNode(cc);
            }
            #region
            //foreach (Node cc in copycode)
            //{
            //    if (VirtualDirEngine.IsDir(cc.Tag.ToString()))
            //    {
            //        if (str == null)
            //        {
            //            _vde.MultiUserMoveDir(this.user, this.pwd, node.Tag.ToString(), cc.Tag.ToString());
            //        }
            //        //else updateMoveDir(copycode, node);
            //       // else updateMoveDir(cc, node);
            //        _vde.MoveDir(cc.Tag.ToString(), node.Tag.ToString());
            //    }
            //    else
            //    {
            //        if (str == null)
            //        {
            //            _vde.MultiUserMoveFileLink(this.user, this.pwd, cc.Parent.Tag.ToString(), node.Tag.ToString(), cc.Tag.ToString());
            //        }
            //        _vde.MoveFileLink(cc.Parent.Tag.ToString(), node.Tag.ToString(), cc.Tag.ToString());
            //       // else updateMoveDir(copycode, node);
            //        //else updateMoveDir(cc, node);
                    
            //    }
            // cc.Parent.Nodes.Remove(cc);
            //    UpdateOneNode(cc.Parent);
            //}            
            #endregion
            UpdateOneNode(node);//20161212
          
        }


        public void Initialize(string user, string pwd)
        {
            this.user = user;
            this.pwd = pwd;
            this.setGrouPanleName(user);
            if (_vde == null)
            {
                _vde = new VirtualDirEngine();
            }
            if (!_vde.IsRegister) _vde.Register(this.user, this.pwd);
            TreeNode roottn = _vde.List();
            this.UpdateContent(roottn);
        }

        /// <summary>
        /// 根据当前用户返回指定的TreeNode对象
        /// </summary>
        /// <returns></returns>
        public void UpdateTreeContent()
        {
            TreeNode tree = null;
            tree = _vde.List(this.user, this.pwd);
            UpdateContent(tree);
        }
        public void updateMoveDir(Node[] copycode, Node node)
        {
            foreach (Node cc in copycode)
            {
                if (VirtualDirEngine.IsDir(cc.Tag.ToString()))
                {
                    _vde.MoveDir(cc.Tag.ToString(), node.Tag.ToString());
                }
                else
                {
                    _vde.MoveFileLink(cc.Parent.Tag.ToString(), node.Tag.ToString(), cc.Tag.ToString());
                }
                UpdateOneNode(cc.Parent);
            }


        }


        public void updateMoveDir(Node copycode, Node node)
        {

            if (VirtualDirEngine.IsDir(copycode.Tag.ToString()))
                {
                    _vde.MoveDir(copycode.Tag.ToString(), node.Tag.ToString());
                }
                else
                {
                    _vde.MoveFileLink(copycode.Parent.Tag.ToString(), node.Tag.ToString(), copycode.Tag.ToString());
                }
            UpdateOneNode(copycode.Parent);
            //UpdateOneNode(node);
            
        }


        private void 复制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Copy();
            this.str = this.Title;
            //UpdateTreeContent();//20161212
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteDir();
        }

        private void 粘贴ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*12.12 删除*/
            //setAllNodeUncheck(this.advTree1.Nodes[0]);
            //如果为true 则表明它是移动
            if (VirtualDirUC._MoveFlag)
            {
                MoveDir();
                VirtualDirUC._MoveFlag = false;
            }
            else Paste();
            str = null;

        }

        private void 移动ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Move();
            this.str = this.Title;
            VirtualDirUC._MoveFlag = true;
            //UpdateContent(VirtualDirUC.getVirtualDirEngine(this.Title).List());//20161212
        }

        private void 重命名ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReNameDir();
        }
        private void 新建ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewDir();
        }



        //20161212
        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Refresh();
        }
        //20161212
        public void Refresh()
        {
            if (advTree1.SelectedNode == null)
            {
                MessageBox.Show("您没有选中文件夹!");
                return;
            }

            Node node = this.advTree1.SelectedNode;
            if (!VirtualDirEngine.IsDir(node.Tag.ToString()))
            {
                MessageBox.Show("您选中的不是文件夹!");
                return;
            }
            UpdateNode(node);
            //if (VirtualDirEngine.IsDir(node.Tag.ToString()))
            //{

            //    UpdateOneNode(node);
            //    if (node.Nodes.Count > 0)
            //    {
            //        node.Expand();
            //    }
            //    else
            //    {
            //       // MessageBox.Show("已经展开到最后一级目录!");
            //        return;
            //    }
            //}
        }
        /// <summary>
        /// 展开节点后
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void advTree1_AfterExpand(object sender, AdvTreeNodeEventArgs e) //20161212
        {
           // advTree1.SelectedNode = e.Node;
            UpdateNode(e.Node);          
        }
        /// <summary>
        /// 折叠节点后
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void advTree1_AfterCollapse(object sender, AdvTreeNodeEventArgs e) //20161212
        {
            advTree1.SelectedNode = e.Node;
        }
        /// <summary>
        /// 双击节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void advTree1_NodeDoubleClick(object sender, TreeNodeMouseEventArgs e)//20161212
        {
            UpdateNode(e.Node);
        }
        private void UpdateNode(Node node)//20161220
        {
            //Node node = e.Node;
            if (VirtualDirEngine.IsDir(node.Tag.ToString()))
            {
                UpdateOneNode(node);
                //if (node.Nodes.Count > 0)
                //{
                //    node.Expand();
                //}
                //else
                //{
                //   // MessageBox.Show("已经展开到最后一级目录!");
                //    return;
                //}
            }
            else
            {
               // MessageBox.Show("该目录不存在子目录!");
                node.Expand();
                return;
            }
        
        }
        /// <summary>
        /// 更新该目录的子孙目录
        /// </summary>
        /// <param name="node"></param>
        public void UpdateOneNode(Node node)//20161212
        {
            TreeNode childtn = new TreeNode(node.Text.ToString());
            childtn.Tag = node.Tag.ToString();
            TreeNode treenode = _vde.FirstList(childtn, 0);
            if (!_vde.IsRegister) _vde.Register(this.user, this.pwd);
            Node advrootNode = null;
            Getnodebytreenode(treenode, out advrootNode);
            node.Nodes.Clear();
            for (int i = advrootNode.Nodes.Count - 1; i > -1; i--)
            {
                node.Nodes.Insert(0, advrootNode.Nodes[i]);               
            }

            if (node.Nodes.Count > 0)
            {
                int sum = node.Nodes.Count;
                for (int i = 0; i < sum; i++)
                {
                    if (!VirtualDirEngine.IsDir(node.Nodes[i].Tag.ToString()) || node.Nodes[i].Nodes.Count == 0)//该目录如果是文件或者叶子节点 让它处于展开状态
                    {
                        node.Nodes[i].Expand();

                    }                   
                }
            }
            else
            {
                node.Expand();
            }
            System.Threading.Thread.Sleep(100);
            advTree1.SelectedNode = node;
           
        }
        /// <summary>
        /// 获取某个目录下的节点
        /// </summary>
        /// <param name="ptn"></param>
        /// <param name="advtn"></param>
        private void Getnodebytreenode(TreeNode ptn, out Node advtn)//20161212
        {
            if (ptn == null)
            {
                advtn = null;
                return;
            }
            advtn = new Node(ptn.Text);
            advtn.Nodes.Clear();
            advtn.Text = ptn.Text;
            advtn.Tag = ptn.Tag;
            List<TreeNode> childnode = new List<TreeNode>();
            childnode = getAllNodeList(ptn);
            foreach (TreeNode tn in childnode)
            {
                Node advctn = null;
                transformnode(tn, out advctn);
                if (advctn != null)
                {
                    advtn.Nodes.Add(advctn);
                }
            }

        }

        private void 清空选择ToolStripMenuItem_Click(object sender, EventArgs e)//20161220
        {
            Empty();
          
        }
        public void Empty()
        {
            Node node = this.advTree1.Nodes[0];
            checkednode = new List<Node>();
            FindCheckedNode(node);
            UnCheckAllChecked(checkednode);
            
        }
        /// <summary>
        /// 取消所有选择的节点
        /// </summary>
        /// <param name="node"></param>
        private void UnCheckAllChecked(List<Node> node)
        {

            for (int i = 0; i < node.Count; i++)
            {
                if (node[i].Nodes.Count > 0)
                {
                    node[i].Checked = false;
                    for (int j = 0; j < node[i].Nodes.Count; j++)
                    {
                        uncheckAllChildNode(node[i].Nodes);
                    }                  
                }
                else node[i].Checked = false;
            }
        }

        private void 下载ToolStripMenuItem_Click(object sender, EventArgs e)
        {
         
            DownloadSelectFile();
        }
        //ctrlVirtualDir页面专用下载
        public void DownloadSelectFile()
        {
            //瓦片数据的filecode是文件名
            List<string> filecodes = GetDownLoadFileListQRSTCODE();
            if (filecodes != null && filecodes.Count > 0)
            {
                Common.FrmDownLoad frmDownload = new Common.FrmDownLoad(filecodes);
               
                    frmDownload.Show();
            }

        }

        private void 导入ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void 导入ToolStripMenuItem_MouseHover(object sender, EventArgs e)
        {
           
        }
        private void initializeToolStripMenuItemClick()
        {
        
             ToolStripItemCollection itemList=this.导入ToolStripMenuItem.DropDownItems;
            foreach(ToolStripItem str in itemList)
            {
              (str as ToolStripMenuItem).Click += new EventHandler(toolstrip_click); 
             }
        }
        private void toolstrip_click(object sender, EventArgs e)
        {

            int index = Convert.ToInt32((sender as ToolStripMenuItem).Tag);
            string productName = (sender as ToolStripMenuItem).Text.ToString();
            string nodePath = selectedNodPath;
            string nodeTarget;
            if (VirtualDirEngine.IsDir(advTree1.SelectedNode.Tag.ToString()))
            {
                nodeTarget = advTree1.SelectedNode.Tag.ToString();

            }
            else 
            {
                nodeTarget = advTree1.SelectedNode.Parent.Tag.ToString();
            }

            if (OnDataImportItemClick!=null)//Application.CompanyName.Contains("QRST_DI_MS"))
            {
                //在数据库运管系统下，切换功能区，而不是弹出数据导入对话框

                OnDataImportItemClick(new object[] { (sender as ToolStripMenuItem).Text, nodePath, nodeTarget }, null);
            }
            else
            {
                InputDataForm input = new InputDataForm(index, productName, nodePath, nodeTarget);
                input.ShowDialog();
            }
        }
        public string selectedNodPath
        {
            get
            {
                string vdpath = "";
                if (advTree1.SelectedNode != null)
                {
                    Node nd = advTree1.SelectedNode;
                    while (nd != null)
                    {
                        if (VirtualDirEngine.IsDir(nd.Tag.ToString()))
                        {
                            vdpath = nd.Text + @"\" + vdpath;
                            nd = nd.Parent;
                        }
                        else
                        {
                            vdpath = nd.Parent.Text + @"\" + vdpath;
                            nd = nd.Parent.Parent;
                        }
                        
                    }
                    return vdpath;
                }
                else
                {
                    return "";
                }
            }
        }

        private void 检索ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(this.检索ToolStripMenuItem.Text=="清空检索结果")
            {
                UpdateTreeContent();
                this.检索ToolStripMenuItem.Text = "检索";
                return;
            }
            SearchFile search = new SearchFile();
            search.ShowDialog();
            if (search.searchInfo != null)
            {

                Common.WaitForm wf = new Common.WaitForm("正在检索，请稍候...");
                 wf.Show();
                 this.searchNode.Clear();
                 Node node = this.advTree1.Nodes[0];
                 //this.searchNode = node.Copy();
                 // this.searchNode.Nodes.Clear();
                // this.cc = node.Copy();
                //this.cc.Nodes.Clear();
                //Console.WriteLine(this.searchNode.Count);
                 selectNode(node,search.searchInfo);
                 //foreach (Node n in searchNode)
                 //{ 
                 //  this.cc.Nodes.Add(n);
                 //}
                 if (searchNode.Count > 0)
                 {
                     this.检索ToolStripMenuItem.Text = "清空检索结果";
                     this.advTree1.Nodes[0].Nodes.Clear();

                     for (int i = 0; i < searchNode.Count; i++)
                     {
                         Node nde = searchNode[i].Copy();
                         this.advTree1.Nodes[0].Nodes.Add(nde);
                        
                     }
                     wf.Close();
                 }
                 else
                 {
                     wf.Close();
                     MessageBox.Show("未检索到相关信息！");
                 }             
                 
            }
        }
        /// <summary>
        /// 查找包含关键字的节点
        /// </summary>
        /// <param name="advtn"></param>
        /// 
        public List<Node> searchNode = new List<Node>();
        private void selectNode(Node advtn,string info)
        {
            // checkednode = new List<Node>();
            if (advtn == null)
            {
                return;
            }
            if (advtn.Text.IndexOf(info)>-1)
            {
                searchNode.Add(advtn);
               
                return;
            }
            if (advtn.Nodes.Count > 0)
            {
                foreach (Node nd in advtn.Nodes)
                {
                    selectNode(nd, info);
                }
            }

        }
        public void UpdateSearchContent(Node roottn)
        {
            advTree1.BeginUpdate();
          
            transformnode(roottn);
            advTree1.Nodes.Add(roottn);

            advTree1.EndUpdate();
            if (!_vde.IsRegister) _vde.Register(this.user, this.pwd);
           
            advTree1.Nodes[0].Expand();//20161212



        }
        private void transformnode(Node ptn)
        {
            Node advtn = null;
            if (ptn == null)
            {
                return;
            }
            List<Node> rootNode = new List<Node>();
            if (VirtualDirEngine.IsDir(ptn.Tag.ToString()))
            {
                advtn = new Node(ptn.Text);
                advtn.Text = ptn.Text;
                advtn.Tag = ptn.Tag;
                //rootNode = getAllNodeList(ptn);
                advtn.Image = global::QRST_DI_MS_Component.Properties.Resources.FolderClosed1;
                advtn.ImageExpanded = global::QRST_DI_MS_Component.Properties.Resources.FolderOpen1;
                advtn.Cells.Add(new Cell("文件夹"));
                advtn.Cells.Add(new Cell(""));
                advtn.Cells.Add(new Cell(_vde.GetCreatTimeByVirtualDirCode(ptn.Tag.ToString())));
                advtn.Cells.Add(new Cell(_vde.GetModifyTimeByVirtualDirCode(ptn.Tag.ToString())));
                advtn.Cells.Add(new Cell(ptn.Tag.ToString()));
                advtn.Cells.Add(new Cell(_vde.GetRemarkByVirtualDirCode(ptn.Tag.ToString())));
                advtn.CheckBoxVisible = ShowCheckBox;
                advtn.ExpandVisibility = eNodeExpandVisibility.Visible;
            }
            else
            {
                if (ShowFileNode)
                {
                    advtn = new Node(ptn.Text);

                    advtn.Nodes.Clear();
                    advtn.Text = ptn.Text;
                    advtn.Tag = ptn.Tag;

                    advtn.Image = global::QRST_DI_MS_Component.Properties.Resources.Document1;

                    advtn.Cells.Add(new Cell("文件"));
                    advtn.Cells.Add(new Cell(""));
                    advtn.Cells.Add(new Cell(_vde.GetCreatTimeByFileLinkCode(ptn.Tag.ToString())));
                    advtn.Cells.Add(new Cell(""));
                    advtn.Cells.Add(new Cell(ptn.Tag.ToString()));
                    advtn.Cells.Add(new Cell(""));

                    advtn.CheckBoxVisible = ShowCheckBox;
                    advtn.ExpandVisibility = eNodeExpandVisibility.Visible;
                }

            }

            foreach (Node tn in rootNode)
            {
                
                transformnode(tn);
                if (tn != null)
                {
                    advtn.Nodes.Add(tn);
                }
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string nodeid = advTree1.SelectedNode.Tag.ToString();
            string[] arr = nodeid.Split('-').ToArray();
            //arr[arr.Length - 1]
            int i = Convert.ToInt32(arr.Last()) / 1000;
            string previewPath = Constant.PreviewPath;
            string jpgPath = string.Format("{0}\\{1}\\{2}", previewPath, i, nodeid + ".jpg");

            //string jpgPath = string.Format("{0}{1}{2}\\{3}", Application.StartupPath, "\\Cache\\Thumbnail\\", nodeid, nodeid+".jpg");
            if (File.Exists(jpgPath))
            {
                Preview preview = new Preview(jpgPath);
                preview.ShowDialog();
            }
            else
            {
                MessageBox.Show("此产品暂无缩略图！");
            }
        }
    }
}
