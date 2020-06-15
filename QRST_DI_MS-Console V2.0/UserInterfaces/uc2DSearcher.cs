using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.IO;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security.Permissions;
using System.Web;

 
namespace QRST_DI_MS_Desktop.UserInterfaces
{
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public partial class uc2DSearcher : UserControl
    {
        public HtmlDocument doc;
        private double boundleft;
        private double boundright;
        private double boundbottom;
        private double boundtop;
        public Color lineColor;

        public uc2DSearcher()
        {
            InitializeComponent();
            Uri Url = new Uri(Environment.CurrentDirectory + @"/2DMapWeb/2DMap.html");
            if (System.IO.File.Exists(Url.LocalPath))
            {
                this.webBrowser1.ScriptErrorsSuppressed = true;
                this.webBrowser1.ObjectForScripting = this;
                this.webBrowser1.Navigate(Url);
            }
            doc = webBrowser1.Document;
        }
        public void enabledDrawPolygon()
        {
            doc.InvokeScript("enabledDrawPolygon");
        }
        public void zoomToDefault()
        {
            if (doc != null)
            {
                doc.InvokeScript("zoomToDefault");
            }
        }
        public void showRaster()
        {
            doc.InvokeScript("showRaster");
        }
        public void showVector()
        {
            doc.InvokeScript("showVector");
        }


        public Action drawPolygonCompletedEvent;
        public Action<double,double> onClickCompletedEvent;

        public void getPos(string lon,string lat)
        {
            double dlon = double.Parse(lon);
            double dlat = double.Parse(lat);
            onClickCompletedEvent(dlat,dlon);
        }

        public void sendBound(string left, string right, string bottom, string top)
        {
            this.boundleft = Convert.ToDouble(left);
            this.boundright = Convert.ToDouble(right);
            this.boundbottom = Convert.ToDouble(bottom);
            this.boundtop = Convert.ToDouble(top);
            drawPolygonCompletedEvent();
            //drawPolygonCompletedEvent(null,null);
        }
        public double[] GetSelectRectangle()
        {
            double[] bound = { boundleft, boundright, boundbottom, boundtop };
            return bound;
        }
        public void DrawSearchResultExtents(string extents)
        {
            doc.InvokeScript("displayTiles",new object[]{ extents });
        }
        public void DisplayTile(string para)
        {
            doc.InvokeScript("displayTile", new object[] { para });
        }

        public void DisplayRaster(string para)
        {
            doc.InvokeScript("displayRaster",new object[]{para});
        }
        public void destroyDisplay()
        {
            doc.InvokeScript("destroyDisplay");
        }
        public void zoomTo(string extent)
        {
            doc.InvokeScript("zoomTo",new object[]{extent});
        }
        public void layerClear()
        {
            doc.InvokeScript("layerClear");
        }
        public void DrawSelectedExtents(string extent)
        {
            doc.InvokeScript("drawSelectedExtent",new object[]{extent});
        }
        public void addMapClickEvent()
        {
            doc.InvokeScript("addMapClickEvent");
        }
        public void removeMapClickEvent()
        {
            doc.InvokeScript("removeMapClickEvent");
        }
        public void setSize(int[] size)
        {
            try
            {
                string mapSize = size[0].ToString() + "," + size[1].ToString();
                doc.InvokeScript("setSize", new object[] { mapSize });
            }
            catch { }
        }
        public void DrawSearchResultExtents(List<System.Drawing.RectangleF> extents)
        {
            float minLonExtent = extents[0].X;
            float minLatExtent = extents[0].Y;
            float maxLonExtent = extents[0].X + extents[0].Width;
            float maxLatExtent = extents[0].Y + extents[0].Height;
            for (int i = 0; i < extents.Count; i++)
            {
                //extents.Add(new System.Drawing.RectangleF(minLon, minLat, maxLon - minLon, maxLat - minLat));
                if (extents[i].X < minLonExtent)
                {
                    minLonExtent = extents[i].X;
                }
                if (extents[i].Y < minLatExtent)
                {
                    minLatExtent = extents[i].Y;
                }
                if ((extents[i].X + extents[i].Width) > maxLonExtent)
                {
                    maxLonExtent = extents[i].X + extents[i].Width;
                }
                if ((extents[i].Y + extents[i].Height) > maxLatExtent)
                {
                    maxLatExtent = extents[i].Y + extents[i].Height;
                }

            }
            int iminLon = Convert.ToInt32(minLonExtent) - 1;
            int iminLat = Convert.ToInt32(minLatExtent) - 1;
            int imaxLon = Convert.ToInt32(maxLonExtent) + 1;
            int imaxLat = Convert.ToInt32(maxLatExtent) + 1;
            int pngWidth = (imaxLon - iminLon) * 20;
            int pngHeight = (imaxLat - iminLat) * 20;
            Bitmap bt = new Bitmap(pngWidth, pngHeight);
            Graphics g = Graphics.FromImage(bt);
            for (int j = 0; j < extents.Count; j++)
            {
                float x = (extents[j].X - iminLon) * 20;
                float y = (imaxLat - (extents[j].Height + extents[j].Y)) * 20;
                float width = (extents[j].Width) * 20;
                float height = (extents[j].Height) * 20;
                g.DrawRectangle(new Pen(Color.FromArgb(90, 0, 255, 0)), x, y, width, height);
            }
            g.Dispose();
            string filepath = Environment.CurrentDirectory + @"\2DMapWeb\img\raster\";
            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
            }
            Random random1 = new Random();
            int num = random1.Next(0, 100001);
            string pngName = num + ".png";
            string raster = filepath + pngName;
            bt.Save(raster, ImageFormat.Png);
            bt.Dispose();
            string para = pngName + "," + pngWidth.ToString() + "," + pngHeight.ToString() + "," + iminLon.ToString() + "," + iminLat.ToString() + "," + imaxLon.ToString() + "," + imaxLat.ToString();
            this.DisplayRaster(para);
            string ext = minLonExtent.ToString() + "," + minLatExtent.ToString() + "," + maxLonExtent.ToString() + "," + maxLatExtent.ToString();
            if (this.Controls.ContainsKey("colorRange"))
            {
                this.Controls.RemoveByKey("colorRange");
            }
            this.zoomTo(ext);
        }
        public void DrawSearchResultExtents(Dictionary<System.Drawing.RectangleF, int> extents, out int maxNum)
        {
            int maxCount = 0;
            foreach (KeyValuePair<RectangleF, int> kvp in extents)
            {
                if (kvp.Value > maxCount)
                {
                    maxCount = kvp.Value;
                }
            }
            float minLonExtent = 180;
            float minLatExtent = 90;
            float maxLonExtent = -180;
            float maxLatExtent = -90;
            foreach (KeyValuePair<RectangleF, int> kvp in extents)
            {
                if (kvp.Key.X < minLonExtent)
                {
                    minLonExtent = kvp.Key.X;
                }
                if (kvp.Key.Y < minLatExtent)
                {
                    minLatExtent = kvp.Key.Y;
                }
                if ((kvp.Key.X + kvp.Key.Width) > maxLonExtent)
                {
                    maxLonExtent = kvp.Key.X + kvp.Key.Width;
                }
                if ((kvp.Key.Y - kvp.Key.Height) > maxLatExtent)
                {
                    maxLatExtent = kvp.Key.Y - kvp.Key.Height;
                }
            }
            int iminLon = Convert.ToInt32(minLonExtent) - 1;
            int iminLat = Convert.ToInt32(minLatExtent) - 1;
            int imaxLon = Convert.ToInt32(maxLonExtent) + 1;
            int imaxLat = Convert.ToInt32(maxLatExtent) + 1;
            int pngWidth = (imaxLon - iminLon) * 100;
            int pngHeight = (imaxLat - iminLat) * 100;
            Bitmap bt = new Bitmap(pngWidth, pngHeight);
            Graphics g = Graphics.FromImage(bt);
            foreach (KeyValuePair<RectangleF, int> kvp in extents)
            {
                float x = (kvp.Key.X - iminLon) * 100;
                float y = (imaxLat - (-kvp.Key.Height + kvp.Key.Y)) * 100;
                float width = (kvp.Key.Width) * 100;
                float height = (-kvp.Key.Height) * 100;
                lineColor = getColorbyCount(kvp.Value, maxCount);
                g.DrawRectangle(new Pen(lineColor), x, y, width, height);
            }
            g.Dispose();
            string filepath = Environment.CurrentDirectory + @"\2DMapWeb\img\tile\";
            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
            }
            Random random1 = new Random();
            int num = random1.Next(0, 100001);
            string pngName = num + ".png";
            string raster = filepath + pngName;
            bt.Save(raster, ImageFormat.Png);
            bt.Dispose();
            string para = pngName + "," + pngWidth.ToString() + "," + pngHeight.ToString() + "," + iminLon.ToString() + "," + iminLat.ToString() + "," + imaxLon.ToString() + "," + imaxLat.ToString();
            this.DisplayTile(para);
            string ext = minLonExtent.ToString() + "," + minLatExtent.ToString() + "," + maxLonExtent.ToString() + "," + maxLatExtent.ToString();
            this.zoomTo(ext);
            maxNum = maxCount;
        }


        private Color getColorbyCount(int p, int max)
        {
            int blue = 255;
            int green = 0;
            int red = 0;
            int all = (int)(383 * ((float)p / (float)max));
            for (int i = 0; i < all; i++)
            {
                if (blue > green)
                {
                    green = green + 1;
                    blue = blue - 1;
                }
                else if (blue > 0)
                {
                    red = red + 1;
                    blue = blue - 1;
                }
                else
                {
                    green = green - 1;
                    red = red + 1;
                }

            }
            return Color.FromArgb(255, red, green, blue);
        }

        //click event
        ///// <summary>
        ///// 绘制矩形框完毕
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //void qrstAxGlobeControl1_OnDrawRectangleCompeleted(object sender, EventArgs e);

        //public delegate void OnDrawPolygonCompletedEvent();
        //public OnDrawPolygonCompletedEvent drawPolygonCompletedEvent;
    }
}
