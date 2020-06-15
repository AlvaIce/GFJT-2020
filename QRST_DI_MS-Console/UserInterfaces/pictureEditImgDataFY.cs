using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QRST_DI_MS_Console.UserInterfaces
{
    public partial class pictureEditImgDataFY : UserControl
    {
        mucDetailViewer _MainCtrl;
        int preID = -1;
        int nextID = -1;
        public pictureEditImgDataFY(mucDetailViewer mainctrl)
        {
            InitializeComponent();

            _MainCtrl = mainctrl;
            if (_MainCtrl != null)
            {
                _MainCtrl.gridViewMain.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(gridViewMain_FocusedRowChanged);
            }
            this.MouseClick += new MouseEventHandler(pictureEditImgDataFY_MouseClick);

        }

        void pictureEditImgDataFY_MouseClick(object sender, MouseEventArgs e)
        {
        }

        bool mousemoveIn()
        {
            if (Control.MousePosition.X > this.Location.X && Control.MousePosition.X < (this.Location.X + this.Width)
                && Control.MousePosition.Y > this.Location.Y && Control.MousePosition.Y < (this.Location.Y + this.Height))
            {
                return true;
            }
            return false;
        }

        void gridViewMain_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (_MainCtrl.selectedQueryObj != null && _MainCtrl.selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_tile"))
            {
                DataTable dt = (_MainCtrl.gridViewMain.DataSource as DataView).Table;
                if (e.FocusedRowHandle > 0 && e.FocusedRowHandle < dt.Rows.Count)
                {

                    string lv = dt.Rows[e.FocusedRowHandle]["Level"].ToString();
                    string col = dt.Rows[e.FocusedRowHandle]["Col"].ToString();
                    string row = dt.Rows[e.FocusedRowHandle]["Row"].ToString();

                    DataRow[] tilerows = dt.Select(string.Format("Level={0} and Col={1} and Row={2}", lv, col, row));
                    int curIdx = tilerows.ToList().IndexOf(dt.Rows[e.FocusedRowHandle]);
                    if (tilerows.Length < 2)
                    {
                        preID = -1;
                        nextID = -1;
                        PageCounter.Text = "1/1";
                    }
                    else
                    {
                        int preIdx = ((curIdx - 1) < 0) ? (tilerows.Length - 1) : (curIdx - 1);
                        int nextIdx = ((curIdx + 1) < tilerows.Length) ? (curIdx + 1) : 0;

                        preID = dt.Rows.IndexOf(tilerows[preIdx]);
                        nextID = dt.Rows.IndexOf(tilerows[nextIdx]);
                        PageCounter.Text = string.Format("{0}/{1}", curIdx + 1, tilerows.Length);
                    }
                }
            }
        }

        private void goPrePage()
        {
            if (preID != -1)
            {
                _MainCtrl.gridViewMain.FocusedRowHandle = preID;
            }

        }

        private void goNextPage()
        {
            if (nextID != -1)
            {
                _MainCtrl.gridViewMain.FocusedRowHandle = nextID;
            }

        }

        private void pictureEditFY_MouseEnter(object sender, EventArgs e)
        {
            if (_MainCtrl.selectedQueryObj != null && _MainCtrl.selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_tile"))
            {
                PrePage.Visible = true;
                NextPage.Visible = true;
                PageCounter.Visible = true;
                midline.Visible = true;
            }
        }

        private void pictureEditFY_MouseLeave(object sender, EventArgs e)
        {
            if (_MainCtrl.selectedQueryObj != null && _MainCtrl.selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_tile"))
            {
                PrePage.Visible = false;
                NextPage.Visible = false;
                PageCounter.Visible = false;
                midline.Visible = false;
            }
        }

        private void pictureEditFY_Click(object sender, EventArgs e)
        {
            if (clickLeft())
            {
                goPrePage();
            }
            else
            {
                goNextPage();
            }
        }

        private bool clickLeft()
        {
            Point mloc = this.PointToClient(Control.MousePosition);
            if (mloc.X > this.Size.Width / 2)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void NextPage_Click(object sender, EventArgs e)
        {

        }

    
        private void pictureEditImgDataFY_SizeChanged(object sender, EventArgs e)
        {
            midline.Location = new Point(this.Width / 2, 0);
            PageCounter.Location = new Point((this.Width - PageCounter.Size.Width) / 2, this.Height / 2);
        }


    }
}
