using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using QRST_DI_DS_Metadata.MetaDataDefiner.Mdl;
using DevExpress.XtraNavBar;

namespace QRST_DI_MS_Console.UserInterfaces
{
	public partial class LeftNbGroupUserControl : UserControl
	{
		List<NavBarGroup> navList = new List<NavBarGroup>();
		muc3DSearcher mu;
		public LeftNbGroupUserControl()
		{
			mu = new muc3DSearcher();
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
					NavBarGroup nbgroup = new NavBarGroup();
					navBarControl1.Groups.Add(nbgroup);
					nbgroup.GroupStyle = DevExpress.XtraNavBar.NavBarGroupStyle.ControlContainer;
					nbgroup.Caption = tn.Text;
					nbgroup.ControlContainer = new DevExpress.XtraNavBar.NavBarGroupControlContainer();
					nbgroup.ControlContainer.ClientSize = new Size(200, 300);
					nbgroup.ControlContainer.Controls.Add(treeview);
					navList.Add(nbgroup);
					treeview.Dock = DockStyle.Fill;
					treeview.Visible = true;
					//treeViewEvdb.Nodes.Add(tn);
				}
				//}
			}
			foreach (NavBarGroup item in navList)
			{
				if (item.Caption == "实验验证数据库")
				{
					item.Expanded = true;

				}
				else
					item.Expanded = false;
			}
		}
		void treeview_AfterSelect(object sender, TreeViewEventArgs e)
		{
			TreeView tr = (TreeView)sender;
			muc3DSearcher._metadatacatalognode_Mdl = (metadatacatalognode_Mdl)tr.SelectedNode.Tag;
			muc3DSearcher._selectDataType = tr.SelectedNode.Text;
		}
		private void navBarControl1_Click(object sender, EventArgs e)
		{
			NavBarControl col = (NavBarControl)sender;
			NavBarGroup navg = (NavBarGroup)col.PressedGroup;
			foreach (var item in navList)
			{
				if (item.Caption == navg.Caption)
					item.Expanded = true;
				else
					item.Expanded = false;
			}
		}
	}
}
