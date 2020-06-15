using System;
using System.Drawing;
using QRST.WorldGlobeTool;
 
namespace QRST_DI_MS_Desktop.UserInterfaces
{
    public partial class CtrlImageDisplay : DevExpress.XtraEditors.XtraUserControl
    {
        mucDetailViewer _MainCtrl;
        private uc2DSearcher uc2DSearcher1;
        private QRST.WorldGlobeTool.QRSTWorldGlobeControl qrstAxGlobeControl;
      
        public Image Image
        {
            set
            {
                pictureEdit.pictureEditFY.Image = value;
             
            }
            get
            {
                return pictureEdit.pictureEditFY.Image;
              
            }
        }//用于展示显示的图像


        public pictureEditImgDataFY pfy
        {
            get
            {
                return pictureEdit;
            }
        }
        public CtrlImageDisplay()
        {
            _MainCtrl = null;
            InitializeComponent();
            this.splitContainerControl1.SplitterPosition = this.splitContainerControl1.Width / 2;
           
        }
        public CtrlImageDisplay(mucDetailViewer mainctrl)
        {
            _MainCtrl = mainctrl;
            InitializeComponent();
            this.splitContainerControl1.SplitterPosition = this.splitContainerControl1.Width / 2;
            this.pictureEdit.create(mainctrl);
        }

        public CtrlImageDisplay(mucDetailViewer mainctrl, QRSTWorldGlobeControl _qrstAxGlobeControl)
        {
            _MainCtrl = mainctrl;
            InitializeComponent();
            this.splitContainerControl1.SplitterPosition = this.splitContainerControl1.Width / 2;
            qrstAxGlobeControl = _qrstAxGlobeControl;
            if(qrstAxGlobeControl!= null)
            {
                this.splitContainerControl1.Panel1.Controls.Add(this.qrstAxGlobeControl);
                this.qrstAxGlobeControl.Dock = System.Windows.Forms.DockStyle.Fill;
            }
            this.pictureEdit.create(mainctrl);
        }
        public CtrlImageDisplay(mucDetailViewer mainctrl,uc2DSearcher _uc2DSearcher)
        {
            _MainCtrl = mainctrl;
            InitializeComponent();
            this.splitContainerControl1.SplitterPosition = this.splitContainerControl1.Width / 2;
            uc2DSearcher1 = _uc2DSearcher;
            if (uc2DSearcher1 != null)
            {
                this.splitContainerControl1.Panel1.Controls.Add(this.uc2DSearcher1);
                this.uc2DSearcher1.Dock = System.Windows.Forms.DockStyle.Fill;
            }
            this.pictureEdit.create(mainctrl);
        }

        public QRSTWorldGlobeControl GetQrstAxGlobeControl()
        {
            return qrstAxGlobeControl;
        }
        public uc2DSearcher GetUc2DSearcher()
        {
            return uc2DSearcher1;
        }

        public void SetQrstAxGlobeControl(QRSTWorldGlobeControl _qrstAxGlobeControl)
        {
            qrstAxGlobeControl = _qrstAxGlobeControl;
            if (qrstAxGlobeControl != null)
            {
                this.splitContainerControl1.Panel1.Controls.Add(this.qrstAxGlobeControl);
                this.qrstAxGlobeControl.Dock = System.Windows.Forms.DockStyle.Fill;
            }
            else
            {
                this.splitContainerControl1.Panel1.Controls.Clear();
            }
        }
        public int[] getSize()
        {
            int[] size=new int[2];
            size[0]=this.splitContainerControl1.Panel1.Width;
            size[1]=this.splitContainerControl1.Panel1.Height;
            return size;
        }
        public void SetUc2DSeacher(uc2DSearcher _uc2DSearcher)
        {
            uc2DSearcher1 = _uc2DSearcher;
            if (uc2DSearcher1 != null)
            {
                this.splitContainerControl1.Panel1.Controls.Add(this.uc2DSearcher1);
                this.uc2DSearcher1.Dock = System.Windows.Forms.DockStyle.Fill;
            }
            else
            {
                this.splitContainerControl1.Panel1.Controls.Clear();
            }
        }

        private void splitContainerControl1_SizeChanged(object sender, EventArgs e)
        {
           // this.splitContainerControlImgAndDetail.SplitterPosition = this.splitContainerControlImgAndDetail.Width - 400;
            splitContainerControl1.SplitterPosition = this.splitContainerControl1.Width / 2;
        }
    }
}
