using System.Drawing;
using System.Windows.Forms;

namespace QRST_DI_MS_Desktop.UserInterfaces.ChildUI
{ 
    public partial class DragDropTreeView : TreeView
    {
        static bool _permissionEdit = true;   //是否允许编辑
        Image upicon;
        Image downicon;
        Image intoicon;
        Rectangle ivrect;
        TreeNode lastnode;
        Image dragstateImg;
        public DragDropTreeView()
            :base()
        {
            InitializeComponent();
            ivrect = new Rectangle(0, 0, 0, 0);

            upicon = QRST_DI_MS_Desktop.Properties.Resources.ModelEditor_IndexUp as Image;
            downicon = QRST_DI_MS_Desktop.Properties.Resources.ModelEditor_IndexDown as Image;
            intoicon = QRST_DI_MS_Desktop.Properties.Resources.Action_Change_State as Image;
            base.AllowDrop = true;
            base.DragDrop += new DragEventHandler(treeview_DragDrop);
            base.DragEnter += new DragEventHandler(treeview_DragEnter);
            base.DragOver += new DragEventHandler(treeview_DragOver);
            base.ItemDrag += new ItemDragEventHandler(treeview_ItemDrag);
            base.AfterSelect += new TreeViewEventHandler(treeview_AfterSelect);
        }

        void treeview_DragOver(object sender, DragEventArgs e)
        {
            //throw new NotImplementedException();
            // 拖放的目标节点
            TreeNode targetTreeNode;
            // 获取当前光标所处的坐标
            // 定义一个位置点的变量，保存当前光标所处的坐标点
            Point point = ((TreeView)sender).PointToClient(new Point(e.X, e.Y));
            // 根据坐标点取得处于坐标点位置的节点
            targetTreeNode = ((TreeView)sender).GetNodeAt(point);
            // 获取被拖动的节点
            TreeNode treeNode = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");

            if (targetTreeNode != null)
            {
                //            if (e.Y<targetTreeNode.)
                //{

                //}
                int treenodehight = 16;
                int treenoderetrack = 19;
                int tnlocX = (targetTreeNode.Level) * treenoderetrack - 9;
                int tnlocY = (point.Y / treenodehight) * treenodehight;
                Image dimg = null;
                if (point.Y > tnlocY + 3.0 * treenodehight / 4.0)
                {
                    dimg = downicon;
                }
                else if (point.Y < tnlocY + treenodehight / 4.0)
                {
                    dimg = upicon;
                }
                else
                {
                    dimg = intoicon;
                }


                if (lastnode != targetTreeNode || (dimg != null && dimg != dragstateImg))
                {
                    if (ivrect.Width > 0 && ivrect.Height > 0)
                    {
                        (sender as TreeView).Invalidate(ivrect);
                        (sender as TreeView).Update();
                    }

                    if (treeNode != targetTreeNode)
                    {
                        Graphics g = (sender as TreeView).CreateGraphics();

                        dragstateImg = dimg;
                        g.DrawImage(dragstateImg, tnlocX, tnlocY);
                        ivrect.X = tnlocX;
                        ivrect.Y = tnlocY;
                        ivrect.Width = upicon.Width;
                        ivrect.Height = upicon.Height;
                    }
                    else
                    {
                        dragstateImg = null;
                        ivrect.X = 0;
                        ivrect.Y = 0;
                        ivrect.Width = 0;
                        ivrect.Height = 0;
                    }
                }
            }
            lastnode = targetTreeNode;

        }
        void treeview_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (_permissionEdit)
            {
                // 开始进行拖放操作，并将拖放的效果设置成移动。
                this.DoDragDrop(e.Item, DragDropEffects.Move);
            }
        }

        void treeview_DragEnter(object sender, DragEventArgs e)
        {
            // 拖动效果设成移动
            e.Effect = DragDropEffects.Move;
        }

        void treeview_DragDrop(object sender, DragEventArgs e)
        {
            // 定义一个中间变量
            TreeNode treeNode;
            //判断拖动的是否为TreeNode类型，不是的话不予处理
            if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode", false))
            {
                // 拖放的目标节点
                TreeNode targetTreeNode;
                // 获取当前光标所处的坐标
                // 定义一个位置点的变量，保存当前光标所处的坐标点
                Point point = ((TreeView)sender).PointToClient(new Point(e.X, e.Y));
                // 根据坐标点取得处于坐标点位置的节点
                targetTreeNode = ((TreeView)sender).GetNodeAt(point);
                // 获取被拖动的节点
                treeNode = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");
                if (targetTreeNode!=null)
                {
                    if (dragstateImg == downicon)
                    {
                        //在一个数据集中调整顺序，插入后部
                        TreeNode tn = (TreeNode)treeNode.Clone();
                        if (targetTreeNode.Parent == null)
                        {
                            targetTreeNode.TreeView.Nodes.Insert(targetTreeNode.Index + 1, tn);
                        }
                        else
                        {
                            targetTreeNode.Parent.Nodes.Insert(targetTreeNode.Index + 1, tn);
                        }
                        treeNode.Remove();
                        tn.TreeView.SelectedNode = tn;
                    }
                    else if (dragstateImg == upicon)
                    {
                        //在一个数据集中调整顺序，插入前部
                        TreeNode tn = (TreeNode)treeNode.Clone();
                        if (targetTreeNode.Parent == null)
                        {
                            targetTreeNode.TreeView.Nodes.Insert(targetTreeNode.Index, tn);
                        }
                        else
                        {
                            targetTreeNode.Parent.Nodes.Insert(targetTreeNode.Index, tn);
                        }
                        treeNode.Remove();
                        tn.TreeView.SelectedNode = tn;
                    }
                    else if (dragstateImg == intoicon)
                    {
                        //移动到一个数据集中
                        TreeNode tn = (TreeNode)treeNode.Clone();
                        targetTreeNode.Nodes.Add(tn);
                        // 将被拖动的节点移除
                        treeNode.Remove();
                        tn.TreeView.SelectedNode = tn;
                    }
                }

                //// 判断拖动的节点与目标节点是否是同一个,同一个不予处理
                //if (targetTreeNode != null && treeNode.Name != targetTreeNode.Name)
                //{
                //    if (((metadatacatalognode_Mdl)targetTreeNode.Tag).IS_DATASET)
                //    {
                //        //移动到一个数据集中
                //        targetTreeNode.Nodes.Add((TreeNode)treeNode.Clone());
                //        // 将被拖动的节点移除
                //        treeNode.Remove();
                //    }
                //    else if (targetTreeNode.Parent != null && targetTreeNode.Parent == treeNode.Parent && !((metadatacatalognode_Mdl)targetTreeNode.Tag).IS_DATASET)
                //    {
                       
                //    }
                //}
            }
            (sender as TreeView).Refresh();
        }

        void treeview_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeView tv = sender as TreeView;
            foreach (TreeNode tn in tv.Nodes)
            {
                clearAllNodeColor(tn);
            }
            e.Node.BackColor = Color.Blue;
            e.Node.ForeColor = Color.White;
        }

        private void clearAllNodeColor(TreeNode tn)
        {
            tn.BackColor = Color.White;
            tn.ForeColor = Color.Black;

            foreach (TreeNode ctn in tn.Nodes)
            {
                clearAllNodeColor(ctn);
            }
        }

    }
}
