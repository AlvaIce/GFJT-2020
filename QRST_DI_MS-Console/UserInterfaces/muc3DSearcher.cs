using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.IO;
using DevExpress.XtraNavBar;
using QRST_DI_DS_Metadata.MetaDataDefiner.Mdl;

namespace QRST_DI_MS_Console.UserInterfaces
{
    public partial class muc3DSearcher : UserControl
    {
		public static string _selectDataType;
		public static metadatacatalognode_Mdl _metadatacatalognode_Mdl;
        bool _is3DViewer;
        public  bool Is3DViewer {  get{return _is3DViewer;} }		
		public delegate void TreeSeleteDeletege();
		public event TreeSeleteDeletege TreeSelete;
		//LeftTabUserControl control;
		LeftButtonUserControl control;
        public muc3DSearcher()
        {
            InitializeComponent();
            uc2DSearcher1.Dock = DockStyle.Fill;
            qrstAxGlobeControl1.Dock = DockStyle.Fill;
            uc2DSearcher1.Visible = true;
            qrstAxGlobeControl1.Visible = true;
			control = new LeftButtonUserControl();
			//this.dockPanel1_Container.Size = control.Size;
			//this.dockPanel1_Container.Controls.Add(control);
			this.dockPanel1_Container.Controls.Add(control);
			control.Dock = DockStyle.Fill;
			foreach (var item in control.treeList)
			{
				item.NodeMouseClick += new TreeNodeMouseClickEventHandler(item_NodeMouseClick);
			}
        }

		void item_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			_metadatacatalognode_Mdl = (metadatacatalognode_Mdl)e.Node.Tag;
			_selectDataType = e.Node.Text;
			//this.Cursor = Cursors.WaitCursor;
			TreeSelete();
		}

		void item_Click(object sender, EventArgs e)
		{
			TreeView tr = (TreeView)sender;
			_metadatacatalognode_Mdl = (metadatacatalognode_Mdl)tr.SelectedNode.Tag;
			_selectDataType = tr.SelectedNode.Text;
			//this.Cursor = Cursors.WaitCursor;
			TreeSelete();
		}

		public void treeview_AfterSelect(object sender, TreeViewEventArgs e)
		{
			TreeView tr = (TreeView)sender;
			_metadatacatalognode_Mdl = (metadatacatalognode_Mdl)tr.SelectedNode.Tag;
			_selectDataType = tr.SelectedNode.Text;
			//this.Cursor = Cursors.WaitCursor;
			TreeSelete();
			//this.Controls = Cursors.Default;
		}

		//void treeview_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
		//{
		//    TreeView tr=(TreeView)sender;
		//    _metadatacatalognode_Mdl = (metadatacatalognode_Mdl)tr.SelectedNode.Tag;
		//    _selectDataType = tr.SelectedNode.Text;
		//    TreeSelete();
		//    //TreeSelete(_selectDataType)
		//}

        public void Show3DViewer()
        {
            if (!EarthLoaded)
            {
                EarthLoad();
            }
            _is3DViewer = true;
            SetQrstAxGlobeControl();
        }

        public void Show2DViewer()
        {
            _is3DViewer = false;
            SetQrstAxGlobeControl();
        }

        //this.qrstAxGlobeControl1.Click += new System.EventHandler(this.qrstAxGlobeControl1_Click);
        //_muc3DSearcher.globeClickEvent += GlobeClickEvent;
        public delegate void GlobeClickEvent(double lat,double lon);
        public GlobeClickEvent globeClickEvent;
        private void qrstAxGlobeControl1_Click(object sender, EventArgs e)
        {
            MouseEventArgs ex = (MouseEventArgs)e;
            double[] arr = qrstAxGlobeControl1.ConvertScreenPos2LatAndLon(ex.X, ex.Y);
            if (arr != null && globeClickEvent != null)
            {
                globeClickEvent(arr[0], arr[1]);
            }
        }

        private bool EarthLoaded = false;
        private void EarthLoad()
        {
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(Application.StartupPath);
            string globeDataPath = string.Format(@"{0}\{1}",Application.StartupPath, "SpatialData");
            string cachePath = string.Format(@"{0}\{1}",Application.StartupPath, "Cache");

            this.qrstAxGlobeControl1.DataDirectory = globeDataPath;
            this.qrstAxGlobeControl1.CacheDirectory = cachePath;
            this.qrstAxGlobeControl1.GlobeLoad();
            this.qrstAxGlobeControl1.QrstGlobe.AddPlacesLayer
                (
                    "中国省界图标", Path.Combine(this.qrstAxGlobeControl1.DataDirectory, @"China\ChinaSheng.qrstp"),
                    25, 25, Path.Combine(this.qrstAxGlobeControl1.DataDirectory, @"Icons\placemark_circle.png"), 5000000
                );
            this.qrstAxGlobeControl1.DrawShapeFile(Path.Combine(this.qrstAxGlobeControl1.DataDirectory, @"China\chinaXian.shp"), Color.Red, 1.0f, 1200000, 100000);
            this.qrstAxGlobeControl1.DrawShapeFile(Path.Combine(this.qrstAxGlobeControl1.DataDirectory, @"China\chinaSheng.shp"), Color.Yellow, 2.0f, 10000000, 500000);
            this.qrstAxGlobeControl1.QrstGlobe.AddURLLayer("testimagefinal", 0, 0, 55, 70, 140, true, 36, 7, Path.Combine(this.qrstAxGlobeControl1.CacheDirectory, "世界地图"), "http://192.168.0.101/Tileset.aspx", "testimageoutFinal");
            EarthLoaded = true;
        }
        public void SetQrstAxGlobeControl()
        {
			//this.Controls.Clear();
            if (_is3DViewer)
            {
				if (!this.panelControl1.Controls.Contains(this.qrstAxGlobeControl1))
				{
					this.panelControl1.Controls.Add(this.qrstAxGlobeControl1);
				}				
                this.qrstAxGlobeControl1.Visible=true;
				this.uc2DSearcher1.Visible = false;
            }
            else
			{
				if (!this.panelControl1.Controls.Contains(this.uc2DSearcher1))
				{
					this.panelControl1.Controls.Add(this.uc2DSearcher1);
				}	
				this.uc2DSearcher1.Visible = true;
				this.qrstAxGlobeControl1.Visible = false;
            }
        }

        private void muc3DSearcher_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 显示的时候重新加载切片覆盖范围
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void muc3DSearcher_VisibleChanged(object sender, EventArgs e)
        {
          
        }		
    }
}
