using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using QRST.WorldGlobeTool.Net;

namespace QRST.WorldGlobeTool.VisualForms
{
    public partial class ColorbarLegend : Form
    {
        Image image;
        string oldText; // Original legend text

        /// <summary>
        /// Initializes a new instance of the <see cref= "T:Qrst.VisualControl.Colorbar"/> class.
        /// </summary>
        /// <param name="parent"></param>
        public ColorbarLegend(Form parent)
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                ControlStyles.Opaque |
                ControlStyles.ResizeRedraw |
                ControlStyles.DoubleBuffer |
                ControlStyles.UserPaint, true);

            // Required for Windows Form Designer support
            //
            InitializeComponent();

            if (parent != null)
            {
                this.Owner = parent;
                this.Icon = parent.Icon;
            }
        }

        /// <summary>
        /// Loads a bitmap from the web or a file and displays.
        /// </summary>
        public void LoadImage(string url)
        {
            if (url != null && !url.ToLower().StartsWith("http://"))
            {
                // Local file
                Image = Image.FromFile(url);
                return;
            }

            oldText = Text;
            Text = Text + ": Loading...";
            using (WebDownload client = new WebDownload(url))
            {
                client.DownloadMemory();
                DownloadComplete(client);
            }
        }

        /// <summary>
        /// Loads a bitmap from the web in background and displays.
        /// </summary>
        public void LoadImageInBackground(string url)
        {
            if (url != null && !url.ToLower().StartsWith("http://"))
            {
                // Local file
                Image = Image.FromFile(url);
                return;
            }

            oldText = Text;
            Text = Text + ": Loading...";
            WebDownload client = new WebDownload(url);
            ////	client.CompleteCallback += new DownloadCompleteHandler(DownloadComplete);
            client.BackgroundDownloadMemory();
        }

        void DownloadComplete(WebDownload downloadInfo)
        {
            if (this.InvokeRequired)
            {
                Invoke(new DownloadCompleteHandler(DownloadComplete), new object[] { downloadInfo });
                return;
            }

            try
            {
                downloadInfo.Verify();
                Image = System.Drawing.Image.FromStream(downloadInfo.ContentStream);
            }
            catch (Exception caught)
            {
                this.Visible = false;
                MessageBox.Show(caught.Message, "Legend image download failed.",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                if (downloadInfo != null)
                    downloadInfo.Dispose();
                Text = oldText;
            }
        }

        /// <summary>
        /// The image displayed in the form.
        /// </summary>
        public Image Image
        {
            get
            {
                return image;
            }
            set
            {
                if (this.image != null)
                {
                    this.image.Dispose();
                    this.image = null;
                }

                if (value == null)
                {
                    this.Visible = false;
                    return;
                }

                this.image = value;
                this.ClientSize = value.Size;
                this.MinimumSize = Size;
                this.Visible = true;
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            e.Cancel = true;
            Visible = false;

            if (Owner != null)
                Owner.Focus();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (image == null)
            {
                base.OnPaint(e);
                return;
            }

            e.Graphics.FillRectangle(SystemBrushes.Window, e.ClipRectangle);
            e.Graphics.DrawImage(image, Rectangle.FromLTRB(0, 0, ClientSize.Width - 1, ClientSize.Height - 1));
        }

        protected override void OnKeyUp(System.Windows.Forms.KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.L:
                    if (e.Modifiers == Keys.Alt)
                    {
                        Close();
                        e.Handled = true;
                    }
                    break;
                case Keys.Escape:
                    Close();
                    e.Handled = true;
                    break;
                case Keys.F4:
                    if (e.Modifiers == Keys.Control)
                    {
                        Close();
                        e.Handled = true;
                    }
                    break;
            }

            base.OnKeyUp(e);
        }
        
    }
}
