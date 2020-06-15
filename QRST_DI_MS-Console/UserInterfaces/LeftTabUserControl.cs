using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using QRST_DI_DS_Metadata.MetaDataDefiner.Mdl;

namespace QRST_DI_MS_Console.UserInterfaces
{
	public partial class LeftTabUserControl : UserControl
	{
		List<TabPage> tabList = new List<TabPage>();
		public LeftTabUserControl()
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
					treeview.AfterSelect += new TreeViewEventHandler(treeview_AfterSelect);
					foreach (TreeNode ctn in tn.Nodes)
					{
						treeview.Nodes.Add(ctn);
						foreach (TreeNode node in ctn.Nodes)
						{
							if (node.Text == "高分系列卫星数据")
							{
								treeview.SelectedNode = node;
								muc3DSearcher._metadatacatalognode_Mdl = (metadatacatalognode_Mdl)treeview.SelectedNode.Tag;
								//TreeSelete();
							}
						}

					}
					treeview.ExpandAll();
					TabPage nbgroup = new TabPage();
					nbgroup.Text = tn.Text;
					this.tabControl1.TabPages.Add(nbgroup);
					nbgroup.Controls.Add(treeview);
					tabList.Add(nbgroup);
					treeview.Dock = DockStyle.Fill;
					treeview.Visible = true;
					//treeViewEvdb.Nodes.Add(tn);
				}
				//}
			}
			
			foreach (TabPage item in tabList)
			{
				if (item.Text == "实验验证数据库")
				{
					tabControl1.SelectedTab = item;
				}				
			}
		}
		void treeview_AfterSelect(object sender, TreeViewEventArgs e)
		{
			TreeView tr = (TreeView)sender;
			muc3DSearcher._metadatacatalognode_Mdl = (metadatacatalognode_Mdl)tr.SelectedNode.Tag;
			muc3DSearcher._selectDataType = tr.SelectedNode.Text;
		}
	}
}
