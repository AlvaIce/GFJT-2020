using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Qrst;
using QRST_DI_MS_Console.UserInterfaces;

namespace QRST_DI_MS_Console.UserInterfaces
{
    public partial class CtrlImageDisplay : DevExpress.XtraEditors.XtraUserControl
    {
        mucDetailViewer _MainCtrl;
        private uc2DSearcher uc2DSearcher1;
        private Qrst.QrstAxGlobeControl qrstAxGlobeControl;
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
        }

        public CtrlImageDisplay(mucDetailViewer mainctrl,QrstAxGlobeControl _qrstAxGlobeControl)
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
        }

        public Qrst.QrstAxGlobeControl GetQrstAxGlobeControl()
        {
            return qrstAxGlobeControl;
        }
        public uc2DSearcher GetUc2DSearcher()
        {
            return uc2DSearcher1;
        }

        public void SetQrstAxGlobeControl(QrstAxGlobeControl _qrstAxGlobeControl)
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
