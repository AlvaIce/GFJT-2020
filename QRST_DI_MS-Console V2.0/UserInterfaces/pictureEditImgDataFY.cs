using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
 
namespace QRST_DI_MS_Desktop.UserInterfaces
{
    public partial class pictureEditImgDataFY : UserControl
    {
        mucDetailViewer _MainCtrl;
        public static string selectname;
        int preID = -1;
        int nextID = -1;
        bool isCreated = false;
       
        //添加的
        Bitmap m_bmp;               //画布中的图像
        Point m_ptCanvas;           //画布原点在设备上的坐标
        Point m_ptCanvasBuf;        //重置画布坐标计算时用的临时变量
        Point m_ptBmp;              //图像位于画布坐标系中的坐标
        float m_nScale = 1.0F;      //缩放比例
        Point m_ptMouseDown;        //鼠标点下是在设备坐标上的坐标
        string m_strMousePt;        //鼠标当前位置对应的坐标

        Point mouseLocation;//鼠标位置

        public pictureEditImgDataFY()
        {
            InitializeComponent();
           //this.StartPosition = FormStartPosition.CenterScreen;
           // this.pictureEditFY.BorderStyle = BorderStyle.FixedSingle;
           // this.pictureEditFY.BackColor = Color.DarkGray;
          //  this.MouseWheel += new MouseEventHandler(pictureEditImgDataFY_MouseWheel);
          
        }
       
        public pictureEditImgDataFY(mucDetailViewer mainctrl)
        {
            InitializeComponent();
           
            _MainCtrl = mainctrl;
            if (_MainCtrl != null)
            {
                if (ruc3DSearcher.clickFlag == true)
                {
                    _MainCtrl.gridView1.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(gridView1_FocusedRowChanged);
                }
                else
                {
                    _MainCtrl.gridViewMain.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(gridViewMain_FocusedRowChanged);

                }

            }
            this.MouseClick += new MouseEventHandler(pictureEditImgDataFY_MouseClick);
            //this.MouseWheel += new MouseEventHandler(pictureEditImgDataFY_MouseWheel);
         
        }

        public bool create(mucDetailViewer mainctrl)
        {
            if (isCreated)
            {
                return true;
            }
          
            _MainCtrl = mainctrl;
            if (_MainCtrl != null)
            {
                if (ruc3DSearcher.clickFlag == true)
                {
                    _MainCtrl.gridView1.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(gridView1_FocusedRowChanged);
                }
                else
                {
                    _MainCtrl.gridViewMain.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(gridViewMain_FocusedRowChanged);

                }
               
            }
            this.MouseClick += new MouseEventHandler(pictureEditImgDataFY_MouseClick);
            this.MouseWheel += new MouseEventHandler(pictureEditImgDataFY_MouseWheel);
            isCreated = true;
            return true;
        }
        void pictureEditImgDataFY_MouseClick(object sender, MouseEventArgs e)
        {
        }

        void pictureEditFY_MouseWheel(object sender, MouseEventArgs e)
        {
            //this.pictureEditFY.Width += e.Delta;
            //this.pictureEditFY.Height += e.Delta;
            zoom(e.Delta);//>1或<-1
        }
        void pictureEditImgDataFY_MouseWheel(object sender, MouseEventArgs e)
        {
            //this.pictureEditFY.Width += e.Delta;
            //this.pictureEditFY.Height += e.Delta;
           // zoom(e.Delta);
            if (m_nScale < 0.4 && e.Delta <= 0)
            {
               
                MessageBox.Show(this, "图像已是最小，不能再缩小了！", "提示对话框", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;       //缩小下线
            }
            if (m_nScale > 6.0 && e.Delta >= 0)
            {
                MessageBox.Show(this, "图像已是最大，不能再放大了！", "提示对话框", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;        //放大上线
            }
            //获取 当前点到画布坐标原点的距离
            SizeF szSub = (Size)m_ptCanvas - (Size)e.Location;
            //当前的距离差除以缩放比还原到未缩放长度
            float tempX = szSub.Width / m_nScale;           //这里
            float tempY = szSub.Height / m_nScale;          //将画布比例
            //还原上一次的偏移                               //按照当前缩放比还原到
            m_ptCanvas.X -= (int)(szSub.Width - tempX);     //没有缩放
            m_ptCanvas.Y -= (int)(szSub.Height - tempY);    //的状态
            //重置距离差为  未缩放状态                       
            szSub.Width = tempX;
            szSub.Height = tempY;
            m_nScale += e.Delta > 0 ? 0.1F : -0.1F;
            //重新计算 缩放并 重置画布原点坐标
            m_ptCanvas.X += (int)(szSub.Width * m_nScale - szSub.Width);
            m_ptCanvas.Y += (int)(szSub.Height * m_nScale - szSub.Height);
          
            pictureEditFY.Invalidate();
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
      
        /// <summary>
        /// 20170330  xmh 添加瓦片的全覆盖时，和对应的表格关联
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void gridViewMain_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (_MainCtrl.selectedQueryObj != null && _MainCtrl.selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_tile"))
            {
                if (ruc3DSearcher.clickFlag == true)
                {
                    _MainCtrl.gridView1.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(gridView1_FocusedRowChanged);
                }
                else 
                {
                    DataTable dt = (_MainCtrl.gridViewMain.DataSource as DataView).Table;
                    if (e.FocusedRowHandle > -1 && e.FocusedRowHandle < dt.Rows.Count)
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
                    ismove = false;
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
        /// <summary>
        /// 20170330   xmh 
        /// 用于当点击上一张、下一张时与GridView1表关联
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (_MainCtrl.selectedQueryObj != null && _MainCtrl.selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_tile") && ruc3DSearcher.clickFlag == true)
            {
                DataTable dt = (_MainCtrl.gridView1.DataSource as DataView).Table;
                if (e.FocusedRowHandle > -1 && e.FocusedRowHandle < dt.Rows.Count)
                {
                    //DataRow[] tilerows = dt.Rows;
                   int curIdx = e.FocusedRowHandle ;
                   if (dt.Rows.Count < 2)
                   {
                       preID = -1;
                       nextID = -1;
                       PageCounter.Text = "1/1";
                       
                   }
                   else
                   {
                       int preIdx = ((curIdx - 1) < 0) ? (dt.Rows.Count - 1) : (curIdx - 1);
                       int nextIdx = ((curIdx + 1) < dt.Rows.Count) ? (curIdx + 1) : 0;
                       preID = preIdx ;
                       nextID = nextIdx ;
                       PageCounter.Text = string.Format("{0}/{1}", curIdx + 1, dt.Rows.Count);
                   }
                }
                ismove = false;
            }
        }
        private void goTilePrePage() 
        {
            if (preID != -1)
            {
                _MainCtrl.gridView1.FocusedRowHandle = preID;
            }
        }
        private void goTileNextPage()
        {
            if (nextID != -1)
            {
                _MainCtrl.gridView1.FocusedRowHandle = nextID;
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
                buttonX1.Visible = false;
                simpleButton1.Visible = false;
                simpleButton2.Visible = true;
            }
            
            else
            {
                buttonX1.Visible = false;
                simpleButton1.Visible = true;
                simpleButton2.Visible = true;
            
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
                buttonX1.Visible = false;
                simpleButton1.Visible = false;
                simpleButton2.Visible = true;
            }
            //if (_MainCtrl.selectedQueryObj != null && _MainCtrl.selectedQueryObj.GROUP_TYPE.ToLower().Equals("System_Raster"))//System_Raster
            //{

            //    buttonX1.Visible = true;
            //}
            else
            {
                buttonX1.Visible = false;
                simpleButton1.Visible = true;
                simpleButton2.Visible = true;

            }
        }

        private void pictureEditFY_Click(object sender, EventArgs e)
        {
            //if (clickLeft())
            //{
            //    goPrePage();
            //}
            //else
            //{
            //    goNextPage();
            //}
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
        /// <summary>
        /// 放大
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonZoomIn_Click(object sender, EventArgs e)
        {
            if (pictureEditFY.Image == null)
            {
                return;
            }
            else
            {
                if (m_nScale > 5.0)
                {
                    MessageBox.Show(this, "图像已是最大，不能再放大了！", "提示对话框", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;        //放大上线
                }
                //获取 当前点到画布坐标原点的距离
                SizeF szSub = (Size)m_ptCanvas - (Size)mouseLocation;
                //当前的距离差除以缩放比还原到未缩放长度
                float tempX = szSub.Width / m_nScale;           //这里
                float tempY = szSub.Height / m_nScale;          //将画布比例
                //还原上一次的偏移                               //按照当前缩放比还原到
                m_ptCanvas.X -= (int)(szSub.Width - tempX);     //没有缩放
                m_ptCanvas.Y -= (int)(szSub.Height - tempY);    //的状态
                //重置距离差为  未缩放状态                       
                szSub.Width = tempX;
                szSub.Height = tempY;
                m_nScale += 0.1F;
                //重新计算 缩放并 重置画布原点坐标
                m_ptCanvas.X += (int)(szSub.Width * m_nScale - szSub.Width);
                m_ptCanvas.Y += (int)(szSub.Height * m_nScale - szSub.Height);
                pictureEditFY.Invalidate();

            }

        }
        /// <summary>
        /// 缩小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonZoomOut_Click(object sender, EventArgs e)
        {

          
            if (pictureEditFY.Image == null)
            {
                return;
            }
            else
            {
                if (m_nScale < 0.2)
                {
                    MessageBox.Show(this, "图像已是最小，不能再缩小了！", "提示对话框", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;        //放大上线
                }
                //获取 当前点到画布坐标原点的距离
                SizeF szSub = (Size)m_ptCanvas - (Size)mouseLocation;
                //当前的距离差除以缩放比还原到未缩放长度
                float tempX = szSub.Width / m_nScale;           //这里
                float tempY = szSub.Height / m_nScale;          //将画布比例
                //还原上一次的偏移                               //按照当前缩放比还原到
                m_ptCanvas.X -= (int)(szSub.Width - tempX);     //没有缩放
                m_ptCanvas.Y -= (int)(szSub.Height - tempY);    //的状态
                //重置距离差为  未缩放状态                       
                szSub.Width = tempX;
                szSub.Height = tempY;
                m_nScale += -0.1F;
                //重新计算 缩放并 重置画布原点坐标
                m_ptCanvas.X += (int)(szSub.Width * m_nScale - szSub.Width);
                m_ptCanvas.Y += (int)(szSub.Height * m_nScale - szSub.Height);
                pictureEditFY.Invalidate();
            }
        }

        private void pictureEditFY_MouseClick(object sender, MouseEventArgs e)
        {
            //Point p = e.Location;

            //Point center = new Point();
            //center.X = pictureEditFY.ClientSize.Width / 2;
            //center.Y = pictureEditFY.ClientSize.Height / 2;


            //DevExpress.XtraEditors.VScrollBar vScrl = null;
            //DevExpress.XtraEditors.HScrollBar hScrl = null;

            //foreach (Control ctrl in pictureEditFY.Controls)
            //{
            //    if (ctrl is DevExpress.XtraEditors.VScrollBar)
            //        vScrl = ctrl as DevExpress.XtraEditors.VScrollBar;
            //    if (ctrl is DevExpress.XtraEditors.HScrollBar)
            //        hScrl = ctrl as DevExpress.XtraEditors.HScrollBar;
            //}

            //p.X += hScrl.Value;
            //p.Y += vScrl.Value;

            //int deltaX = p.X - center.X;
            //int deltaY = p.Y - center.Y;

            //hScrl.Value = deltaX;
            //vScrl.Value = deltaY;
        }

        #region  没有用
        //public Image GetReducedImage(Image img)
        //{
        //    System.Drawing.Imaging.Encoder encoder = System.Drawing.Imaging.Encoder.Quality;//获取品质(压缩率)编码  
        //    EncoderParameter mycoder = new EncoderParameter(encoder, 30L);//0压缩率最大,100品质最高,此处输入30L最适合  
        //    EncoderParameters myCoders = new EncoderParameters(1);//参数数组,大小为1  
        //    myCoders.Param[0] = mycoder;//添加一个参数  
        //    ImageCodecInfo jpgInfo = GetEncoder(ImageFormat.Jpeg);//获取JPG格式编解码信息  

        //    Bitmap bmp = KiResizeImage((Bitmap)img, img.Width / 5, img.Height / 5); //设置为了原图片的五分之一进行缩放,测试原图片 50KB,执行后2KB  
        //    return bmp;
        //}
        //private ImageCodecInfo GetEncoder(ImageFormat format)//获取特定的图像编解码信息  
        //{
        //    ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

        //    foreach (ImageCodecInfo codec in codecs)
        //    {
        //        if (codec.FormatID == format.Guid)
        //        {
        //            return codec;
        //        }
        //    }
        //    return null;
        //}
        //public static Bitmap KiResizeImage(Bitmap bmp, int newW, int newH)
        //{
        //    try
        //    {
        //        Bitmap b = new Bitmap(newW, newH);
        //        Graphics g = Graphics.FromImage(b);

        //        // 插值算法的质量  
        //        g.InterpolationMode = InterpolationMode.HighQualityBicubic;

        //        g.DrawImage(bmp, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
        //        g.Dispose();

        //        return b;
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

        //public static byte[] GetByteImage(Image img)
        //{

        //    byte[] bt = null;

        //    if (img != null)
        //    {
        //        using (MemoryStream mostream = new MemoryStream())
        //        {
        //            Bitmap bmp = new Bitmap(img);

        //            bmp.Save(mostream, System.Drawing.Imaging.ImageFormat.Jpeg);//将图像以指定的格式存入缓存内存流  

        //            bt = new byte[mostream.Length];

        //            mostream.Position = 0;//设置留的初始位置  

        //            mostream.Read(bt, 0, Convert.ToInt32(bt.Length));

        //        }

        //    }

        //    return bt;

        //}



        //void SetPictrueEdit()
        //{
        //    txt图片.Properties.AllowScrollViaMouseDrag = true;
        //    txt图片.Properties.AllowZoomOnMouseWheel = DevExpress.Utils.DefaultBoolean.True;
        //    txt图片.Properties.Appearance.Options.UseTextOptions = true;
        //    txt图片.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
        //    txt图片.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
        //    txt图片.Properties.ContextButtonOptions.BottomPanelColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
        //    txt图片.Properties.ContextButtonOptions.Indent = 3;
        //    txt图片.Properties.ContextButtonOptions.TopPanelColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));

        //    contextButton6.Alignment = DevExpress.Utils.ContextItemAlignment.BottomFar;
        //    contextButton6.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.True;
        //    contextButton6.AppearanceHover.ForeColor = System.Drawing.Color.White;
        //    contextButton6.AppearanceHover.Options.UseForeColor = true;
        //    contextButton6.AppearanceNormal.ForeColor = System.Drawing.Color.White;
        //    contextButton6.AppearanceNormal.Options.UseForeColor = true;
        //    contextButton6.Glyph = m_imgContextButtonClose;
        //    contextButton6.Id = new System.Guid("14a76b7c-b704-4644-b1b7-fe833ae4acc5");
        //    contextButton6.Name = "itemRemove";

        //    contextButton7.Alignment = DevExpress.Utils.ContextItemAlignment.BottomNear;
        //    contextButton7.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.True;
        //    contextButton7.AppearanceHover.ForeColor = System.Drawing.Color.White;
        //    contextButton7.AppearanceHover.Options.UseForeColor = true;
        //    contextButton7.AppearanceNormal.ForeColor = System.Drawing.Color.White;
        //    contextButton7.AppearanceNormal.Options.UseForeColor = true;
        //    contextButton7.Glyph = m_imgcontextButtonDownLoad;
        //    contextButton7.Id = new System.Guid("7346c2ed-8a36-4c16-bfc0-c8599f20a63d");
        //    contextButton7.Name = "itemDownload";

        //    trackBarContextButton1.Alignment = DevExpress.Utils.ContextItemAlignment.MiddleBottom;
        //    trackBarContextButton1.AllowUseMiddleValue = true;
        //    trackBarContextButton1.Id = new System.Guid("04d18c6d-6c39-4bed-81a5-fee056e3aad9");
        //    trackBarContextButton1.Maximum = 500;
        //    trackBarContextButton1.Middle = 100;
        //    trackBarContextButton1.Name = "TrackBarContextButton";
        //    trackBarContextButton1.ShowZoomButtons = false;
        //    txt图片.Properties.ContextButtons.Add(contextButton6);
        //    txt图片.Properties.ContextButtons.Add(contextButton7);
        //    txt图片.Properties.ContextButtons.Add(trackBarContextButton1);
        //    txt图片.Properties.NullText = "没有图像";
        //    txt图片.Properties.ZoomingOperationMode = DevExpress.XtraEditors.Repository.ZoomingOperationMode.MouseWheel;
        //    txt图片.Properties.ContextButtonClick += new DevExpress.Utils.ContextItemClickEventHandler(txt图片_Properties_ContextButtonClick);
        //    txt图片.Properties.ContextButtonValueChanged += new DevExpress.Utils.ContextButtonValueChangedEventHandler(txt图片_Properties_ContextButtonValueChanged);
        //    txt图片.Size = new System.Drawing.Size(366, 244);
        //    txt图片.TabIndex = 0;
        //    txt图片.ZoomPercentChanged += new System.EventHandler(txt图片_ZoomPercentChanged);
        //}

        //private void txt图片_Properties_ContextButtonValueChanged(object sender, ContextButtonValueEventArgs e)
        //{
        //    if (e.Item.Name == "TrackBarContextButton")
        //    {
        //        txt图片.Properties.ZoomPercent = Convert.ToDouble(e.Value);
        //    }
        //}

        //private void txt图片_Properties_ContextButtonClick(object sender, ContextItemClickEventArgs e)
        //{
        //    ContextItemClick(e);
        //}
        //void ContextItemClick(ContextItemClickEventArgs e)
        //{
        //    if (e.Item.Name == "itemDownload")
        //    {
        //        Image img = ImportImage();
        //        if (img != null)
        //        {
        //            txt图片.Image = img;
        //        }
        //    }
        //    else if (e.Item.Name == "itemRemove")
        //    {
        //        txt图片.Image = null;
        //    }
        //}


        //private void txt图片_ZoomPercentChanged(object sender, EventArgs e)
        //{
        //    (txt图片.Properties.ContextButtons["TrackBarContextButton"] as TrackBarContextButton).Value = Convert.ToInt32(txt图片.Properties.ZoomPercent);
        //}

        #endregion
        #region  没有用
        /// <summary>  
        ///  Resize图片   
        /// </summary>  
        /// <param name="bmp">原始Bitmap </param>  
        /// <param name="newW">新的宽度</param>  
        /// <param name="newH">新的高度</param>  
        /// <returns>处理以后的图片</returns>  
        public static Bitmap ResizeImage(Bitmap bmp, int newW, int newH)
        {
            try
            {
                Bitmap b = new Bitmap(newW, newH);
                Graphics g = Graphics.FromImage(b);
                // 插值算法的质量   
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(bmp, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
                g.Dispose();
                return b;
            }
            catch
            {
                return null;
            }
        }  
        public static Bitmap CreateThumbnail(Bitmap source, int thumbWi, int thumbHi, bool maintainAspect)
        {
            // return the source image if it’s smaller than the designated thumbnail
            if (source.Width < thumbWi && source.Height < thumbHi) return source;

            Bitmap ret;
            try
            {
                int wi = thumbWi;
                int hi = thumbHi;

                if (maintainAspect)
                {
                    // maintain the aspect ratio despite the thumbnail size parameters
                    if (source.Width > source.Height)
                    {
                        wi = thumbWi;
                        hi = (int)(source.Height * ((decimal)thumbWi / source.Width));
                    }
                    else
                    {
                        hi = thumbHi;
                        wi = (int)(source.Width * ((decimal)thumbHi / source.Height));
                    }
                }

                // original code that creates lousy thumbnails
                // System.Drawing.Image ret = source.GetThumbnailImage(wi,hi,null,IntPtr.Zero);
                ret = new Bitmap(wi, hi);
                using (Graphics g = Graphics.FromImage(ret))
                {
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.FillRectangle(Brushes.White, 0, 0, wi, hi);
                    g.DrawImage(source, 0, 0, wi, hi);
                }
            }
            catch
            {
                ret = null;
            }

            return ret;
        }
        /// <summary>
        /// 图片等比例缩放
        /// </summary>
        /// <param name="b"></param>
        /// <param name="destHeight"></param>
        /// <param name="destWidth"></param>
        /// <returns></returns>
        public static Bitmap GetThumbnail(Bitmap b, int destHeight, int destWidth)
        {
            System.Drawing.Image imgSource = b;
            System.Drawing.Imaging.ImageFormat thisFormat = imgSource.RawFormat;
            int sW = 0, sH = 0;
            // 按比例缩放           
            int sWidth = imgSource.Width;
            int sHeight = imgSource.Height;
            if (sHeight > destHeight || sWidth > destWidth)
            {
                if ((sWidth * destHeight) > (sHeight * destWidth))
                {
                    sW = destWidth;
                    sH = (destWidth * sHeight) / sWidth;
                }
                else
                {
                    sH = destHeight;
                    sW = (sWidth * destHeight) / sHeight;
                }
            }
            else
            {
                sW = sWidth;
                sH = sHeight;
            }
            Bitmap outBmp = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage(outBmp);
            g.Clear(Color.Transparent);
            // 设置画布的描绘质量         
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(imgSource, new Rectangle((destWidth - sW) / 2, (destHeight - sH) / 2, sW, sH), 0, 0, imgSource.Width, imgSource.Height, GraphicsUnit.Pixel);
            g.Dispose();
            // 以下代码为保存图片时，设置压缩质量     
            EncoderParameters encoderParams = new EncoderParameters();
            long[] quality = new long[1];
            quality[0] = 100;
            EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            encoderParams.Param[0] = encoderParam;
            imgSource.Dispose();
            return outBmp;
        }


        private int count = 10;
        private void zoom(int delta)//delta鼠标转动的圈数吧 放大缩小           福
        {
            if (delta >= 1)
            {
                resize(--count);//++count 之前是这个
            }
            else if (delta <= -1)
            {
                resize(++count);
            }
        }

        private void resize(int c)
        {
            int w = this.Width;
            int h = this.Height;
            decimal percent = (decimal)(c + 100) / (decimal)100;
            decimal width = percent * w;
            decimal height = percent * h;
           // pictureEditFY.Width = Convert.ToInt32(width);
          //  pictureEditFY.Height = Convert.ToInt32(height);

            Bitmap bmp;
            bmp = (Bitmap)this.pictureEditFY.Image;
            pictureEditFY.Image = GetThumbnail(bmp, Convert.ToInt32(width), Convert.ToInt32(height));
            //panel2.Width = this.Width - 10;
            //panel1.Height = this.Height - 60;
            //if (panel1.Height < pictureEditFY.Height || panel1.Width < pictureEditFY.Width)
            //{
            //    panel1.AutoScroll = true;
            //}

        }
        public static string userselectname = null;
        public string[] name = null;

        #endregion

        private void comboBoxEdit1_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_nScale = 1.0F;
            ThumbComboItem thumcmbit = comboBoxEdit1.SelectedItem as ThumbComboItem;
            if (thumcmbit != null)
            {
                string imgpath = thumcmbit.ImagePath;
                if (File.Exists(imgpath))
                {
                    Image img = Image.FromFile(imgpath);
                    if (img != null)
                    {
                        this.pictureEditFY.Image = img;
                        ismove = false;
                    }
                }
            }
        }
        //重绘
        public void pictureEditFY_Paint(object sender, PaintEventArgs e)
        {

            m_bmp = (Bitmap)this.pictureEditFY.Image;
            if (m_bmp == null)
            {
                return;
            }
            else
            {
                if (!ismove)//ture代表先是平移然后调用重绘方法 图片不需要再中间显示
                {
                    m_ptCanvas = new Point(this.pictureEditFY.Width / 2, this.pictureEditFY.Height / 2);//原点坐标
                }

                int sW = 0, sH = 0;
                int imgwidth = m_bmp.Width; 
                int imgheight = m_bmp.Height;
                int picturewidth = this.pictureEditFY.Width;
                int pictureheight = this.pictureEditFY.Height;
                if ((imgwidth * ((decimal)pictureheight / imgheight)) > picturewidth)
                {
                    sW = picturewidth;
                    sH = (int)(imgheight * ((decimal)picturewidth / imgwidth));
                }
                else
                {
                    sW = (int)(imgwidth * ((decimal)pictureheight / imgheight));
                    sH = pictureheight;
                }
                m_ptBmp = new Point(-(m_bmp.Width / 2), -(m_bmp.Height / 2));//图像坐标
                Graphics g = e.Graphics;
                g.TranslateTransform(m_ptCanvas.X, m_ptCanvas.Y);       //设置坐标偏移
                g.ScaleTransform(m_nScale, m_nScale);                   //设置缩放比
                g.Clear(Color.White);
                //g.DrawImage(m_bmp, m_ptBmp);                            //绘制图像
                // g.ResetTransform();                                     //重置坐标系         
                g.DrawImage(m_bmp, -(sW / 2), -(sH / 2), sW, sH);
                // g.DrawImage(m_bmp, -(m_bmp.Width / 2), -(m_bmp.Height / 2), sW, sH);
                //g.DrawImage(m_bmp, new Rectangle((picturewidth - sW) / 2, (pictureheight - sH) / 2, sW, sH), 0, 0, imgwidth, imgheight, GraphicsUnit.Pixel);
                // g.DrawImage(m_bmp, new Rectangle(0, 0, sW, sH), new Rectangle(0, 0, m_bmp.Width, m_bmp.Height), GraphicsUnit.Pixel);
            }
        }
        //添加的
        private void pictureEditFY_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {      //如果左键点下    初始化计算要用的临时数据
                m_ptMouseDown = e.Location;
                m_ptCanvasBuf = m_ptCanvas;
            }
           // m_ptMouseDown = e.Location;
            pictureEditFY.Focus();
        }
        //平移
        public static bool ismove = false;
        private void pictureEditFY_MouseMove(object sender, MouseEventArgs e)
        {
           // this.mouseLocation = e.Location;

            if (e.Button == MouseButtons.Left)
            {     
                m_ptCanvas = (Point)((Size)m_ptCanvasBuf + ((Size)e.Location - (Size)m_ptMouseDown));
                ismove = true;
                pictureEditFY.Invalidate();
            }
            //计算 右上角显示的坐标信息
            SizeF szSub = (Size)e.Location - (Size)m_ptCanvas;  //计算鼠标当前点对应画布中的坐标
            szSub.Width /= m_nScale;
            szSub.Height /= m_nScale;
            Size sz = TextRenderer.MeasureText(m_strMousePt, this.Font);    //获取上一次的区域并重绘
            pictureEditFY.Invalidate(new Rectangle(pictureEditFY.Width - sz.Width, 0, sz.Width, sz.Height));
            m_strMousePt = e.Location.ToString() + "\n" + ((Point)(szSub.ToSize())).ToString();
            sz = TextRenderer.MeasureText(m_strMousePt, this.Font);         //绘制新的区域
            pictureEditFY.Invalidate(new Rectangle(pictureEditFY.Width - sz.Width, 0, sz.Width, sz.Height));
           
           
        }
        //换一张
        public static int comNum;
        public List<ThumbComboItem> list=new List<ThumbComboItem>();
        private void buttonX1_Click(object sender, EventArgs e)
        {
            //int index = comboBoxEdit1.SelectedIndex;
            //comboBoxEdit1_SelectedIndexChanged(null,e);
            if (comboBoxEdit1.Properties.Items.Count == 0)
                return;
            m_nScale = 1.0F;          
            comNum++;
            if (comNum == comboBoxEdit1.Properties.Items.Count) comNum = 0;
            ThumbComboItem thumcmbit = comboBoxEdit1.Properties.Items[comNum] as ThumbComboItem;
            if (thumcmbit != null)
            {
                string imgpath = thumcmbit.ImagePath;
                if (File.Exists(imgpath))
                {
                    Image img = Image.FromFile(imgpath);
                    if (img != null)
                    {
                        this.pictureEditFY.Image = img;
                        ismove = false;
                    }
                }
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (comboBoxEdit1.Properties.Items.Count == 0)
                return;
            m_nScale = 1.0F;
            comNum++;
            if (comNum == comboBoxEdit1.Properties.Items.Count) comNum = 0;
            ThumbComboItem thumcmbit = comboBoxEdit1.Properties.Items[comNum] as ThumbComboItem;
            if (thumcmbit != null)
            {
                string imgpath = thumcmbit.ImagePath;
                if (File.Exists(imgpath))
                {
                    Image img = Image.FromFile(imgpath);
                    if (img != null)
                    {
                        this.pictureEditFY.Image = img;
                        ismove = false;
                    }
                }
            }

        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            m_nScale = 1.0F;
            ismove = false;
            pictureEditFY.Invalidate();
        }
        /// <summary>
        /// xmh 新加一种瓦片全覆盖的关联
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureEditFY_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (_MainCtrl.selectedQueryObj != null && _MainCtrl.selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_tile") && ruc3DSearcher.clickFlag == true)
            {
                if (clickLeft())
                {
                    goTilePrePage();
                }
                else
                {
                    goTileNextPage();
                }
            }
            else
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
        }

    }

    public class ThumbComboItem
    {
        public string ImageName { get; set; }
        public string ImagePath { get; set; }
        public ThumbComboItem(string path)
        {
            ImagePath = path;
            ImageName = Path.GetFileNameWithoutExtension(path);
            pictureEditImgDataFY.comNum = 0;
        }
        public override string ToString()
        {
            return ImageName;
        }
    }
}


