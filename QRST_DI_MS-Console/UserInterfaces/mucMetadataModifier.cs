using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using QRST_DI_DS_Metadata.MetaDataDefiner;
using QRST_DI_DS_Metadata.MetaDataDefiner.Mdl;
using QRST_DI_DS_Basis.DBEngine;
using QRST_DI_MS_Console.UserInterfaces.ChildUI;


namespace QRST_DI_MS_Console.UserInterfaces
{
    public partial class mucMetadataModifier : DevExpress.XtraEditors.XtraUserControl
    {

        private MSUserInterface mucinterface;

        private RibbonPage rucmetadatadefinerPage;
		public TreeView treeView1;
        //自定义类型改变事件，便于和ribbon进行通信
        public delegate void DataTypeChangedEventHandler(object sender, EventArgs e);
        public event DataTypeChangedEventHandler DataTypeChanged;
        DatabaseCatalogUI left;
        public mucMetadataModifier()
        {
            InitializeComponent();
            left = new DatabaseCatalogUI();
			this.dockPanel1.Controls.Add(left);
			left.Dock = DockStyle.Fill;
			treeView1 = left.treeView;
			foreach (TreeView tv in left.treeList)
			{
				tv.AfterSelect+=new TreeViewEventHandler(treeView1_AfterSelect);
			}
        }

        private void mucMetadataModifier_Load(object sender, EventArgs e)
        {
            mucinterface = MSUserInterface.GetMSUIbyMainUC(this);
            string type = mucinterface.uiRibbonPage.GetType().ToString();
            // if (mucinterface.uiRibbonPage.get )
        }

        void InitialDBTree()
        {
            //TreeNode tn=new TreeNode(){Text = "QRST",Tag = "root",Name = "root"};
            //获取所有子库列表，初始化元数据树结构
			//for (int i = 0 ; i < TheUniversal.subDbLst.Count ; i++)
			//{
			//    TreeNode tn = TheUniversal.subDbLst[i].GetDbNode();
			//    if (tn != null)
			//    {
			//        treeView1.Nodes.Add(tn);
			//    }

			//}
			//treeView1.ExpandAll();
        }


        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
			treeView1 = left.treeView;
            if (DataTypeChanged != null)
            {
                DataTypeChanged(this, new EventArgs());
            }
        }

        //切换到该页面时加载结构树
        private void mucMetadataModifier_VisibleChanged(object sender, EventArgs e)
        {
            //初始化数据类型树
            if (this.Visible)
            {
                RefreshTree();
            }
        }

        public void RefreshTree()
        {
			//treeView1.Nodes.Clear();
			//InitialDBTree();
			//treeView1.SelectedNode = treeView1.Nodes[0];
			//ctrlTableManager1.SetSchema(treeView1.Nodes[0].Name.Substring(0, 4));
        }

    }
}
