using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using QRST_DI_DS_Metadata.MetaDataDefiner;
using QRST_DI_DS_Metadata.MetaDataDefiner.Mdl;
using QRST_DI_DS_Basis.DBEngine;
using QRST_DI_MS_Console.UserInterfaces.ChildUI;

namespace QRST_DI_MS_Console.UserInterfaces
{
    public partial class mucMetadataDefiner : DevExpress.XtraEditors.XtraUserControl
    {
        //private MSUserInterface mucinterface;

        //private RibbonPage rucmetadatadefinerPage; 

        //自定义类型改变事件，便于和ribbon进行通信
        public delegate void DataTypeChangedEventHandler(object sender, EventArgs e);
        public event DataTypeChangedEventHandler DataTypeChanged;
		public TreeView treeView1;
        DatabaseCatalogUI left;
        public mucMetadataDefiner()
        {
            InitializeComponent();
            //ctrlDisplayInfo1.Visible = false;
            left = new DatabaseCatalogUI();
			this.dockPanel1.Controls.Add(left);
			left.Dock = DockStyle.Fill;
			foreach (TreeView item in left.treeList)
			{
				item.AfterSelect+=new TreeViewEventHandler(treeView1_AfterSelect);
			}
			treeView1 = left.treeView;
        }

        private void mucMetadataDefiner_Load(object sender, EventArgs e)
        {
             //mucinterface = MSUserInterface.GetMSUIbyMainUC(this);
             // rucmetadatadefinerPage = mucinterface.uiRibbonPage;
           

         

          //  ctrlTableManager1.InitializeCtrlWithTableObj(tablemdl,true);
        }

        void InitialDBTree()
        {
            //TreeNode tn=new TreeNode(){Text = "QRST",Tag = "root",Name = "root"};
            //获取所有子库列表，初始化元数据树结构
			//for (int i = 0; i < TheUniversal.subDbLst.Count; i++)
			//{
			//    TreeNode tn = TheUniversal.subDbLst[i].GetDbNode();
			//    if (tn!=null)
			//    {
			//         treeView1.Nodes.Add(tn);
			//    }
               
			//}
			//treeView1.ExpandAll();
        }


   
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
           //获取方案名
			treeView1 = left.treeView;
            string schema = treeView1.SelectedNode.Name.Substring(0, 4);
            ctrlTableManager1.SetSchema(schema);
          

            if (DataTypeChanged != null)
            {
                DataTypeChanged(this, new EventArgs());
            }
        }

        private void mucMetadataDefiner_VisibleChanged(object sender, EventArgs e)
        {
            //初始化数据类型树
			//treeView1.Nodes.Clear();
			//InitialDBTree();
            if (treeView1.Nodes.Count != 0)
            {
                treeView1.SelectedNode = treeView1.Nodes[0];
                ctrlTableManager1.SetSchema(treeView1.Nodes[0].Name.Substring(0, 4));
            }
            ctrlDisplayInfo1.Message = "请选择创建元数据的数据格式！";
        }

        private void ctrlDisplayInfo1_SizeChanged(object sender, EventArgs e)
        {
            ctrlDisplayInfo1.AdjustLocation();
        }
    }
}
