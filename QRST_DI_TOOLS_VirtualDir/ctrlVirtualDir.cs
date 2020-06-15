using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.AdvTree;
using System.IO;
using System.Configuration;
using QRST_DI_DS_Basis;
using QRST_DI_DS_Basis.DBEngine;
using MySql.Data.MySqlClient;
using QRST_DI_MS_Basis;
using QRST_DI_MS_Basis.UserRole;
using QRST_DI_MS_Basis.VirtualDir;
using WindowsFormsApplication2.Properties;

namespace WindowsFormsApplication2.UI.Report
{
    public partial class ReportBodyUCReportDisplay : UserControl
    {
        private static VirtualDirEngine _vde;
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
        public AdvTree _AdvTree { get; set; }
        public ReportBodyUCReportDisplay()
        {

            InitializeComponent();
            AdvTreeSettings.SelectedScrollIntoViewHorizontal = false;
            _AdvTree = this.advTree1;
            _vde = new VirtualDirEngine();

        }

        private void allcheckednode(Node advtn)
        {
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
                    allcheckednode(nd);
                }
            }

        }

        private void transformnode(TreeNode ptn, out Node advtn)
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
            List<TreeNode> rootNode = new List<TreeNode>();
            if (VirtualDirEngine.IsDir(ptn.Tag.ToString()))
            {
                rootNode = getAllNodeList(ptn);
                advtn.Image = global::WindowsFormsApplication2.Properties.Resources.FolderClosed1;
                advtn.ImageExpanded = global::WindowsFormsApplication2.Properties.Resources.FolderOpen1;
                advtn.Cells.Add(new Cell("文件夹"));
                advtn.Cells.Add(new Cell(""));
                advtn.Cells.Add(new Cell(_vde.GetCreatTimeByVirtualDirCode(ptn.Tag.ToString())));
                advtn.Cells.Add(new Cell(_vde.GetModifyTimeByVirtualDirCode(ptn.Tag.ToString())));
                advtn.Cells.Add(new Cell(ptn.Tag.ToString()));
            }
            else
            {
                advtn.Image = global::WindowsFormsApplication2.Properties.Resources.Document1;
                advtn.Cells.Add(new Cell("文件"));
                advtn.Cells.Add(new Cell(""));
                advtn.Cells.Add(new Cell(_vde.GetCreatTimeByFileLinkCode(ptn.Tag.ToString())));
                advtn.Cells.Add(new Cell(""));

                advtn.Cells.Add(new Cell(ptn.Tag.ToString()));

            }
            advtn.CheckBoxVisible = true;
            advtn.ExpandVisibility = eNodeExpandVisibility.Visible;

            foreach (TreeNode tn in rootNode)
            {
                Node advctn = null;
                transformnode(tn, out advctn);
                advtn.Nodes.Add(advctn);
            }
        }

        public void UpdateContent(TreeNode roottn)
        {
            advTree1.BeginUpdate();
            advTree1.Nodes.Clear();
            Node advrootNode = null;
            transformnode(roottn, out advrootNode);
            advTree1.Nodes.Add(advrootNode);
            advTree1.EndUpdate();
            _vde.Register(this.user, this.pwd);
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
                        checkAllChirldNode(e.Cell.Parent.Nodes);
                    else
                        uncheckAllChirldNode(e.Cell.Parent.Nodes);

                }
                
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
        private void checkAllChirldNode(NodeCollection node)
        {
            for (int i = 0; i < node.Count; i++)
            {
                if (node[i].Nodes.Count > 0)
                {
                    node[i].Checked = true;
                    checkAllChirldNode(node[i].Nodes);
                }
                else node[i].Checked = true;
            }

        }
        /// <summary>
        /// 将所有的节点都取消选中
        /// </summary>
        /// <param name="node"></param>
        private void uncheckAllChirldNode(NodeCollection node)
        {

            for (int i = 0; i < node.Count; i++)
            {
                if (node[i].Nodes.Count > 0)
                {
                    node[i].Checked = false;
                    uncheckAllChirldNode(node[i].Nodes);
                }
                else node[i].Checked = false;

            }

        }
       
        private void advTree1_NodeClick(object sender, TreeNodeMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point ClickPoint = new Point(e.X, e.Y);
                Node CurrentNode = advTree1.GetNodeAt(ClickPoint);
                advTree1.SelectedNode = CurrentNode;

                if (!VirtualDirEngine.IsDir(CurrentNode.Tag.ToString()))
                {
                    this.新建ToolStripMenuItem.Visible = false;
                    this.重命名ToolStripMenuItem.Visible = false;
                }
                else
                {
                    this.新建ToolStripMenuItem.Visible = true;
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
                if (ReportManage.commonCheckNode.Count > 0)
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
            if (advTree1.SelectedNode.Text == "root")
            {
                MessageBox.Show("不能修改根目录!");
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
        public void Copy()
        {
            ReportManage.commonCheckNode.Clear();
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
            allcheckednode(node);
            iscopy = true;
            ReportManage.commonCheckNode = checkednode;

        }
        public void Move()
        {
            ReportManage.commonCheckNode.Clear();
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
            allcheckednode(node);
            iscopy = true;
            ReportManage.commonCheckNode = checkednode;

        }
        public void Paste()
        {
            if (advTree1.SelectedNode == null)
            {
                MessageBox.Show("您没有选中文件夹!");
                return;
            }
            Node[] copycode = ReportManage.commonCheckNode.ToArray();
            Node node = this.advTree1.SelectedNode;
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
            if (advTree1.SelectedNode.Text == "root")
            {
                MessageBox.Show("不能修改根目录!");
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
            Node[] copycode = ReportManage.commonCheckNode.ToArray();
            Node node = this.advTree1.SelectedNode;
            foreach (Node cc in copycode)
            {
                if (VirtualDirEngine.IsDir(cc.Tag.ToString()))
                {
                    if (str == null)
                    {
                       _vde.MultiUserMoveDir(this.user, this.pwd, node.Tag.ToString(), cc.Tag.ToString());
                    }
                    else updateMoveDir(copycode, node);
                }
                else
                {
                    if (str == null)
                    {
                       _vde.MultiUserMoveFileLink(this.user, this.pwd, cc.Parent.Tag.ToString(), node.Tag.ToString(), cc.Tag.ToString());
                    }
                    else updateMoveDir(copycode, node);
                }
            }
            //TreeNode roottn = getNeedUpdateTree();
            //UpdateContent(roottn);
            UpdateOneNode(node);//20161212

        }
       


        //}
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
            }


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
            if (ReportManage._MoveFlag)
            {
                MoveDir();
                ReportManage._MoveFlag = false;
            }
            else Paste();
            str = null;

        }

        private void 移动ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Move();
            this.str = this.Title;
            ReportManage._MoveFlag = true;
            //UpdateContent(ReportManage.getVirtualDirEngine(this.Title).List());//20161212
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
            if (VirtualDirEngine.IsDir(node.Tag.ToString()))
            {

                UpdateOneNode(node);
                if (node.Nodes.Count > 0)
                {
                    node.Expand();
                }
                else
                {
                    MessageBox.Show("已经展开到最后一级目录!");
                    return;
                }
            }
        }
        /// <summary>
        /// 展开节点后
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void advTree1_AfterExpand(object sender, AdvTreeNodeEventArgs e) //20161212
        {
            advTree1.SelectedNode = e.Node;
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
         
            Node node = e.Node;
            if (VirtualDirEngine.IsDir(node.Tag.ToString()))
            {
                UpdateOneNode(node);
                if (node.Nodes.Count > 0)
                {
                    //node.Nodes[0].Expand();
                    node.Expand();
                }
                else
                {
                    MessageBox.Show("已经展开到最后一级目录!");
                    return;
                }
            }
            else
            {
                MessageBox.Show("该目录不存在子目录!");
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
            TreeNode treenode =_vde.FirstList(childtn, 0);
            _vde.Register(this.user, this.pwd);
            Node advrootNode = null;
            Getnodebytreenode(treenode, out advrootNode);
            node.Nodes.Clear();
            for (int i = advrootNode.Nodes.Count-1; i >-1; i--)
            {
                node.Nodes.Insert(0, advrootNode.Nodes[i]);
            }
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
                advtn.Nodes.Add(advctn);
            }

        }


        /************************************************************************************/
        /*12.12 删除*/
        //public List<Node> check;
        /*12.12 删除*/
        //public void MoveDir(List<Node> checkednode)
        //{
        //    if (advTree1.SelectedNode == null)
        //    {
        //        MessageBox.Show("您没有选中文件夹!");
        //        return;
        //    }
        //    if (advTree1.SelectedNode.CheckState == CheckState.Checked)
        //    {
        //        MessageBox.Show("目标文件夹是源文件夹下的子文件，不能移动!");
        //        return;
        //    }
        //    Node node1 = this.advTree1.Nodes[0];
        //    Node[] copycode = checkednode.ToArray();
        //    Node node = this.advTree1.SelectedNode;
        //    string otheruser = ReportManage.getOtherUserName(this.Title);
        //    foreach (Node cc in checkednode.ToArray())
        //    {
        //        if (VirtualDirEngine.IsDir(cc.Tag.ToString()))
        //        {
        //           _vde.MultiUserMoveDir(this.user, this.pwd, cc.Tag.ToString(), node.Tag.ToString());
        //        }
        //        else
        //        {
        //           _vde.MultiUserMoveFileLink(this.user, this.pwd, cc.Parent.Tag.ToString(), node.Tag.ToString(), cc.Tag.ToString());
        //        }
        //        ReportManage.updateOtherPanle(this.Title);
        //    }

        //TreeNode roottn = getNeedUpdateTree();
        //UpdateContent(roottn);
        // UpdateOneNode(node);//20161212

        /*12.12 删除*/
        //public List<Node> getAllcheckNode(Node advtn)
        //{
        //    if (advtn.Checked == true)
        //    {
        //        check.Add(advtn);
        //    }
        //    if (advtn.Nodes.Count > 0)
        //    {
        //        foreach (Node nd in advtn.Nodes)
        //        {
        //            if (nd.Checked == true && nd.Nodes.Count > 0)
        //            {
        //                check.Add(nd);
        //            }
        //            else
        //            {
        //                getAllcheckNode(nd);
        //            }
        //        }
        //    }
        //    return check;
        //}

        /* 12.12 删除*/
        //public void setAllNodeUncheck(Node advtn)
        //{
        //    if (advtn.Checked == true)
        //    {
        //        advtn.Checked = false; ;
        //    }
        //    if (advtn.Nodes.Count > 0)
        //    {
        //        foreach (Node nd in advtn.Nodes)
        //        {
        //            setAllNodeUncheck(nd);
        //        }
        //    }

        //}
        /*12.12删除*/
        //public void itDeleteFile(Node node, String currentpath)
        //{
        //    if (node.Cells[1].Text == "文件夹") Directory.Delete(currentpath, true);
        //    else
        //    {
        //        File.Delete(currentpath);
        //    }

        //}
        /*12.12删除*/
        //public string strConvert(String arr, String rootPath, String filename, string var)
        //{
        //    string[] strarr = arr.Split(new char[] { ';' });
        //    if (var == "文件夹")
        //    {
        //        for (int i = 1; i < strarr.Length; i++)
        //        {
        //            rootPath = rootPath + "\\" + strarr[i];
        //        }
        //        rootPath = rootPath + "\\" + filename;

        //    }
        //    else
        //    {
        //        for (int i = 1; i < strarr.Length - 1; i++)
        //        {
        //            rootPath = rootPath + "\\" + strarr[i];
        //        }
        //        rootPath = rootPath + "\\" + filename;

        //    }
        //    return rootPath;
        //}
        /*12.12 删除*/
        //public string strConvert(String arr, String rootPath)
        //{
        //    string[] strarr = arr.Split(new char[] { ';' });

        //    for (int i = 1; i < strarr.Length; i++)
        //    {
        //        rootPath = rootPath + "\\" + strarr[i];
        //    }

        //    return rootPath;
        //}
        /*12.12 删除*/
        //public List<Node> getCurrentCheckNode()
        //{
        //    //每次使用前先声明
        //    check = new List<Node>();
        //    if (this.Title == "Child")
        //    {
        //        check = getAllcheckNode(ReportManage.getRootNode("Root").Nodes[0]);
        //    }
        //    if (this.Title == "Root")
        //    {
        //        check = getAllcheckNode(ReportManage.getRootNode("Child").Nodes[0]);

        //    }

        //    return check;

        //}
        /*12.12 删除*/
        //public List<Node> getCurrentNeedNode()
        //{
        //    List<Node> checkednode = null;
        //    if (this.Title == "Child" && ReportManage.getClass("Root") != null)
        //    {
        //        checkednode = ReportManage.getClass("Root");
        //    }
        //    if (this.Title == "Root" && ReportManage.getClass("Child") != null)
        //    {
        //        checkednode = ReportManage.getClass("Child");

        //    }

        //    return checkednode;

        //}

        /*12.12 删除*/
        //protected internal List<Node> getRootCheckNood(string title)
        //{

        //    check = new List<Node>();
        //    check = getAllcheckNode(ReportManage.getRootNode(title).Nodes[0]);
        //    return check;

        //}
    }
}
