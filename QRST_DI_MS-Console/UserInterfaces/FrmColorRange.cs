using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace QRST_DI_MS_Console.UserInterfaces
{
    public partial class FrmColorRange : DevExpress.XtraEditors.XtraForm
    {

        int max=0;
        ColorRange colorrange;
        public FrmColorRange()
        {
            InitializeComponent();
        }

        public FrmColorRange(int _max)
        {
            InitializeComponent();
            max = _max;
        //    labelControlMax.Text = _max.ToString();
          //  colorrange = new ColorRange(_max);
            colorrange.Dock = DockStyle.Left;
            this.Controls.Add(colorrange);
        }

        public void  Draw()
        {
            float height = this.Height / ((float)max);
            float width = 35;
            for (int i = 0; i < max+1; i++)
            {
                float x = 0;
                float y = height * (max-i);
                Color color = getColorbyCount(i, max);
                DrawRectangle(color, x, y, width, height);
            }
        }

        /// <summary>
        /// 根据覆盖次数，显示颜色
        /// </summary>
        /// <param name="p"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        private Color getColorbyCount(int p, int max)
        {
            //000000255 000128128 1281280000 255000000

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

        private void DrawRectangle(Color color, float x, float y, float width, float height)
        {
           
            Graphics graphic = this.CreateGraphics();
            graphic.DrawRectangle(new Pen(color), x, y, width, height);
            graphic.FillRectangle(new SolidBrush(color), x, y, width, height);
        }

        private void FrmColorRange_Paint(object sender, PaintEventArgs e)
        {
           // Draw();
            //if(colorrange!=null)
            //{
            //    colorrange.Draw();
            //}
        }
    }
}