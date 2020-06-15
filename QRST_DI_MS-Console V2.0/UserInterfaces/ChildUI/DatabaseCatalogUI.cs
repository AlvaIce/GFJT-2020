using System;
using System.Collections.Generic;
using System.Windows.Forms;
using QRST_DI_DS_Metadata.MetaDataDefiner.Mdl;
using DevExpress.XtraEditors;
 
namespace QRST_DI_MS_Desktop.UserInterfaces.ChildUI
{
    public partial class DatabaseCatalogUI : UserControl
	{
        public static Dictionary<string, DragDropTreeView> DTree = new Dictionary<string, DragDropTreeView>();
        public Dictionary<string, DragDropTreeView> treeDir = new Dictionary<string, DragDropTreeView>();
        public List<DragDropTreeView> treeList = new List<DragDropTreeView>();
        public DragDropTreeView treeView;
        public Dictionary<string, DragDropTreeView> GetTreeDir()
        {
            return treeDir;
        }
        public DatabaseCatalogUI()
		{
			InitializeComponent();

			for (int i = 0; i < TheUniversal.subDbLst.Count; i++)
			{
				//if (TheUniversal.subDbLst[i].NAME == ("evdb").ToUpper())
				//{
				TreeNode tn = TheUniversal.subDbLst[i].GetDbNode();
				if (tn != null && tn.FirstNode != null)
				{
                    DragDropTreeView treeview = new DragDropTreeView();
                    treeview.ImageList = imageList1;
					foreach (TreeNode ctn in tn.Nodes)
					{
                        ctn.ImageIndex = 0;
                        //ctn.Nodes.Add("test");
                        //ctn.Nodes[0].Nodes.Add("test");
                        //ctn.Nodes[0].Nodes[0].Nodes.Add("test");
                        //ctn.Nodes[0].Nodes[0].Nodes[0].Nodes.Add("test");
                        //ctn.Nodes[0].Nodes[0].Nodes[0].Nodes[0].Nodes.Add("test");
                        //ctn.Nodes[0].Nodes[0].Nodes[0].Nodes[0].Nodes[0].Nodes.Add("test");
						treeview.Nodes.Add(ctn);
					}
                    treeview.Dock = DockStyle.Fill;
					treeview.ExpandAll();
					treeDir.Add(tn.Text, treeview);
					treeList.Add(treeview);
					//TabPage nbgroup = new TabPage();
					//nbgroup.Text = tn.Text;
					//this.tabControl1.TabPages.Add(nbgroup);
					//nbgroup.Controls.Add(treeview);
					//tabList.Add(nbgroup);
					//treeview.Dock = DockStyle.Fill;
					//treeview.Visible = true;
					//treeViewEvdb.Nodes.Add(tn);
				}
				//}
			}
			button_Click(button2, null);
            DragDropTreeView gftv = treeDir["数据产品库"];
            treeView = gftv;
            foreach (TreeNode node in gftv.Nodes)
			{
				foreach (TreeNode cnode in node.Nodes)
				{
					if (cnode.Text == "其他高分卫星数据")
					{
						gftv.SelectedNode = cnode;
						muc3DSearcher._metadatacatalognode_Mdl = (metadatacatalognode_Mdl)gftv.SelectedNode.Tag;
						//TreeSelete();
						break;
					}
				}
			}
			gftv.Focus();
			Refresh();
		}

		void button_Click(object sender, EventArgs e)
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
				this.panelControl1.Controls.Add(treeDir[(sender as CheckButton).Text.Replace(System.Environment.NewLine, "")]);
				treeDir[(sender as CheckButton).Text.Replace(System.Environment.NewLine, "")].Visible = true;
				treeDir[(sender as CheckButton).Text.Replace(System.Environment.NewLine, "")].Dock = DockStyle.Fill;
			}
			catch
			{
                DragDropTreeView tr = new DragDropTreeView();
				this.panelControl1.Controls.Add(tr);
				tr.Visible = true;
				tr.Dock = DockStyle.Fill;
			}
			//throw new NotImplementedException();
		}

	}
}
