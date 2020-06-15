using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;
// 
namespace QRST_DI_MS_Desktop.UserInterfaces
{
    public partial class LeftButtonUserControlMetadata : UserControl
	{
		Dictionary<string, TreeView> treeDir = new Dictionary<string, TreeView>();
		public List<TreeView> treeList = new List<TreeView>();
		public TreeView treeView;
		public LeftButtonUserControlMetadata()
		{
			InitializeComponent();
            if (TheUniversal.subDbLst != null)
            {
                for (int i = 0; i < TheUniversal.subDbLst.Count; i++)
                {
                    //if (TheUniversal.subDbLst[i].NAME == ("evdb").ToUpper())
                    //{
                    TreeNode tn = TheUniversal.subDbLst[i].GetDbNode();
                    if (tn != null && tn.FirstNode != null)
                    {
                        TreeView treeview = new TreeView();
                        treeview.Dock = DockStyle.Fill;
                        QRST_DI_DS_Metadata.MetaDataDefiner.SiteDb.InitImageList();
                        treeview.ImageList = QRST_DI_DS_Metadata.MetaDataDefiner.SiteDb.DBTreeImageList;
                        treeview.AfterSelect += new TreeViewEventHandler(treeview_AfterSelect);
                        //treeview.AfterSelect += new TreeViewEventHandler(treeview_AfterSelect);
                        //foreach (TreeNode ctn in tn.Nodes)
                        //{
                        //    treeview.Nodes.Add(ctn);
                        //}
                        treeview.Nodes.Add(tn);

                        treeview.ExpandAll();
                        treeDir.Add(tn.Text, treeview);
                        treeList.Add(treeview);
                    }
                }

                button_Click(button1, null);
                if (treeDir != null && treeDir.ContainsKey("实验验证数据库"))
                {
                    TreeView gftv = treeDir["实验验证数据库"];
                    treeView = gftv;
                    foreach (TreeNode node in gftv.Nodes)
                    {
                        foreach (TreeNode cnode in node.Nodes)
                        {
                            if (cnode.Text == "其他高分卫星数据")
                            {
                                gftv.SelectedNode = cnode;
                                //TreeSelete();
                                break;
                            }
                        }
                    }
                    gftv.Focus();
                }
                Refresh();
            }
		}

        public Image GetImageByDataType()
        {
            return null;
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

		public void button_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < this.groupControl1.Controls.Count; i++)
			{
				if ((this.groupControl1.Controls[i] as CheckButton) != null)
				{
					if (this.groupControl1.Controls[i] == sender)
					{
						(this.groupControl1.Controls[i] as CheckButton).Checked = true;
					}
					else
					{
						(this.groupControl1.Controls[i] as CheckButton).Checked = false;
					}
				}
			}

			this.panelControl1.Controls.Clear();
			try
			{
				treeView = treeDir[(sender as CheckButton).Text.Replace(System.Environment.NewLine, "")];
				this.panelControl1.Controls.Add(treeView);
                treeView.Dock = DockStyle.Fill;
				treeDir[(sender as CheckButton).Text.Replace(System.Environment.NewLine, "")].Visible = true;
				treeDir[(sender as CheckButton).Text.Replace(System.Environment.NewLine, "")].Dock = DockStyle.Fill;
			}
			catch
			{
				TreeView tr = new TreeView();
				this.panelControl1.Controls.Add(tr);
				tr.Visible = true;
				tr.Dock = DockStyle.Fill;
			}
			//throw new NotImplementedException();
		}

	}
}
