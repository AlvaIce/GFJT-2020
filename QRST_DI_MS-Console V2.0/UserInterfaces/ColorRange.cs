using System;
using System.Drawing;
using System.Windows.Forms;
 
namespace QRST_DI_MS_Desktop.UserInterfaces
{
    public partial class ColorRange : UserControl
    {
        public Control parentControl;

        int max = 0;

        public ColorRange()
        {
            InitializeComponent();
        }

        public ColorRange(int _max,Control _parentControl)
        {
            InitializeComponent();
            labelControlMax.Text = _max.ToString();
            labelMin.Text = "1";
           
            max = _max;
            parentControl = _parentControl;
            this.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
          //  this.Height = parentControl.Height / 3;
           // this.Location = new Point(parentControl.Location.X + parentControl.Width - this.Width - 5, parentControl.Location.Y + parentControl.Height - this.Height - 5);

            parentControl.LocationChanged += new EventHandler(parentControl_LocationChanged);
            parentControl.SizeChanged += new EventHandler(parentControl_LocationChanged);
       
         
        }

        void parentControl_LocationChanged(object sender, EventArgs e)
        {
            this.Height = parentControl.Height / 3;
            this.Location = new Point(parentControl.Location.X + parentControl.Width - this.Width - 5, parentControl.Location.Y + parentControl.Height - this.Height - 5);
        }

        /// <summary>
        /// 根据覆盖次数，显示颜色
        /// </summary>
        /// <param name="p">当前数值，当前瓦片的个数</param>
        /// <param name="max">最大数值，总数</param>
        /// <returns></returns>
        private Color getColorbyCount(int p, int max)
        {
            //000000128 000128128 1281280000 255000000

            int blue = 128; 
            int green = 0;
            int red = 0;
            int all = (int)(383 * ((float)p / (float)max));     //383=255+128
            for (int i = 0; i < all; i++)
            {
                if (blue > green)
                {
                    green = green + 1;
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

        /// <summary>
        /// 绘制色带，this.Height是色带的高度
        /// </summary>
        public void Draw()
        {
            float height = this.Height / ((float)max);
            float width = 33;
            for (int i = 0; i < max + 1; i++)
            {
                float x = 0;
                float y = height * (max - i);
                Color color = getColorbyCount(i, max);
                DrawRectangle(color, x, y, width, height);
            }
        }


        private void DrawRectangle(Color color, float x, float y, float width, float height)
        {
            Graphics graphic = this.CreateGraphics();
            graphic.DrawRectangle(new Pen(color), x, y, width, height);
            graphic.FillRectangle(new SolidBrush(color), x, y, width, height);
        }

        private void ColorRange_Paint(object sender, PaintEventArgs e)
        {
            float height = this.Height / ((float)max);
            float width = 33;
            for (int i = 0; i < max + 1; i++)
            {
                float x = 0;
                float y = height * (max - i);
                Color color = getColorbyCount(i, max);
                DrawRectangle(color, x, y, width, height);
            }
        }

  
    }
}
