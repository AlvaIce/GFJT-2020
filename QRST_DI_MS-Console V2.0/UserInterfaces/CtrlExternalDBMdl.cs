using System;
using System.Drawing;
using QRST_DI_DS_Metadata.MetaDataDefiner;
using System.IO;
 
namespace QRST_DI_MS_Desktop.UserInterfaces
{
    public partial class CtrlExternalDBMdl : DevExpress.XtraEditors.XtraUserControl
    {
        //任务单击事件
        public delegate void ClickEventHandler(object sender, EventArgs e);
        public event ClickEventHandler ClickEvent;

        public CtrlExternalDBMdl()
        {
            InitializeComponent();
        }

        private Externaldb _externalDb;

        public Externaldb externalDb
        {
            set {
                _externalDb = value;
                if (_externalDb.IMAGE == null) //采用默认图片
                {
                }
                else
                {      
                    MemoryStream ms = new MemoryStream(_externalDb.IMAGE);
                   pictureBox1.Image = Image.FromStream(ms);
                   ms.Close();
                }
       
                labelDbName.Text = _externalDb.NAME;
                this.labelDbName.Location = new System.Drawing.Point((this.Width - labelDbName.Width) / 2, 141);
                
             }
            get { return _externalDb; }
        }

        public int width
        {
            set
            {
                this.Width = value;
                this.labelDbName.Location = new System.Drawing.Point((this.Width - labelDbName.Width) / 2, 141);
            }
        }

        //public Image PictureImage
        //{
        //    set { pictureBox1.Image = value; }
        //    get { return pictureBox1.Image; }
        //}

        //public string Name
        //{
        //    set { labelDbName.Text = value; }
        //    get { return labelDbName.Text; }
        //}

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            ItemClick();
        }

        public void ItemClick()
        {
            if (ClickEvent != null)
            {
                ClickEvent(this, null);
            }
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox1.Size = new System.Drawing.Size(205, 140);
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.pictureBox1.Size = new System.Drawing.Size(201, 138);
        }
    }
}

