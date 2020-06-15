using System;
using System.Drawing;

namespace QRST_DI_TS_Process.Tasks.InstalledTasks
{
    public class ITCreateThumbnail : TaskClass
    {
        /// <summary>
        /// 任务名,定义唯一标识，不可动态修改
        /// </summary>
        public override string TaskName
        {
            get { return "ITCreateThumbnail"; }
            set { }
        }

        public override void Process()
        {
            //Make Thumbnail
            string srcFilePath = this.ProcessArgu[0];
            string thumbFilePath = this.ProcessArgu[1];

            try
            {
                if (System.IO.File.Exists(srcFilePath))
                {

                    if (!System.IO.File.Exists(thumbFilePath))
                    {
                        this.ParentOrder.Logs.Add("正在创建缩略图...");
                        MakeThumbnail(srcFilePath, 128, 128, "HW").Save(thumbFilePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                        this.ParentOrder.Logs.Add("完成缩略图创建。");
                    }
                }
            }
            catch (Exception ex)
            {
                this.ParentOrder.Logs.Add(string.Format("创建缩略图出现异常{0}", ex.Message));
            }
        }

        public Image MakeThumbnail(string originalImagePath, int width, int height, string mode)
        {
            Image originalImage = Image.FromFile(originalImagePath);

            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            switch (mode)
            {
                case "HW"://指定高宽缩放（可能变形）                
                    break;
                case "W"://指定宽，高按比例                    
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H"://指定高，宽按比例
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "Cut"://指定高宽裁减（不变形）                
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }

            //新建一个bmp图片
            Image bitmap = new Bitmap(towidth, toheight);

            //新建一个画板
            Graphics g = Graphics.FromImage(bitmap);

            //设置高质量插值法
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充
            g.Clear(Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分
            g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight),
                new Rectangle(x, y, ow, oh),
                GraphicsUnit.Pixel);

            try
            {
                return (Image)bitmap.Clone();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }
    }
}
