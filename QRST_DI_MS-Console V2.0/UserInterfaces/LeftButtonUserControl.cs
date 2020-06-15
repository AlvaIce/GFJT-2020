using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using QRST_DI_DS_Metadata.MetaDataDefiner.Mdl;
using DevExpress.XtraEditors;
 
namespace QRST_DI_MS_Desktop.UserInterfaces
{
    public partial class LeftButtonUserControl : UserControl
	{
		Dictionary<string, TreeView> treeDir = new Dictionary<string, TreeView>();
		public List<TreeView> treeList = new List<TreeView>();

        public static string button = null;
		public LeftButtonUserControl()
		{
			InitializeComponent();
			for (int i = 0; i < TheUniversal.subDbLst.Count; i++)
			{
				//if (TheUniversal.subDbLst[i].NAME == ("evdb").ToUpper())
				//{
				TreeNode tn = TheUniversal.subDbLst[i].GetDbNode();
				if (tn != null && tn.FirstNode != null)
				{
					TreeView treeview = new TreeView();
                    QRST_DI_DS_Metadata.MetaDataDefiner.SiteDb.InitImageList();
                    treeview.ImageList = QRST_DI_DS_Metadata.MetaDataDefiner.SiteDb.DBTreeImageList;
					treeview.AfterSelect += new TreeViewEventHandler(treeview_AfterSelect);
					//treeview.AfterSelect += new TreeViewEventHandler(treeview_AfterSelect);
                    foreach (TreeNode ctn in tn.Nodes)
                    {
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
			TreeView gftv = treeDir["数据产品库"];
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
                    else if (cnode.Text == "高分3号卫星")
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
            button = ((CheckButton)sender).Tag.ToString();
            //Type t = sender.GetType();
            //IEnumerable<System.Reflection.PropertyInfo> property = from pi in t.GetProperties() where pi.Name.ToLower() == field.ToLower() select pi;
            //property.First().GetValue(sender, null);
            //button = sender.
            //foreach (System.Reflection.PropertyInfo p in sender.GetType().GetProperties())
            //{
            //    button =Convert.ToString( p.GetValue(sender, null));
            //    Console.WriteLine("Name:{0} Value:{1}", p.Name, p.GetValue(sender,null));
            //}

			this.panelControl1.Controls.Clear();
			try
			{
				this.panelControl1.Controls.Add(treeDir[(sender as CheckButton).Text.Replace(System.Environment.NewLine, "")]);
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
