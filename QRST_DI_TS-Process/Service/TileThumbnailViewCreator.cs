using System;
using QRST_DI_TS_Basis.DirectlyAddress;
using System.IO;
using System.Drawing;
 
namespace QRST_DI_TS_Process.Service
{
    public class TileThumbnailViewCreator
    {
        public static byte[] GetTileMiniView(DirectlyAddressing _tileDA, string tilename)
        {
            string mvfn = DirectlyAddressing.getMiniViewFilename(tilename);

            byte[] imgdata = null;
            string mvfpath = _tileDA.GetPathByFileName(mvfn);
            if (System.IO.File.Exists(mvfpath))
            {
                imgdata = GetFileData(mvfpath);
            }
            else
            {
                imgdata = CreateMiniTinyViewFile(_tileDA, tilename, 0.1, mvfpath);
            }

            return imgdata;
        }

        public static byte[] GetTileTinyView(DirectlyAddressing _tileDA,string tilename)
        {
            string tvfn = DirectlyAddressing.getTinyViewFilename(tilename);

            byte[] imgdata = null;
            string tvfpath = _tileDA.GetPathByFileName(tvfn);
            if (System.IO.File.Exists(tvfpath))
            {
                imgdata = GetFileData(tvfpath);
            }
            else
            {
                imgdata = CreateMiniTinyViewFile(_tileDA, tilename, 0.02, tvfpath);
            }
            
            return imgdata;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tilename"></param>
        /// <param name="p">缩放比值。0.1:生成100*100图像，=100/1000；0.02：生成20*20图像，=20/1000</param>
        /// <returns></returns>
        private static byte[] CreateMiniTinyViewFile(DirectlyAddressing _tileDA, string tilename, double p, string savepath = "")
        {
            byte[] outdata = null;
            string pvfn = DirectlyAddressing.getPreViewFilename(tilename);
            string previewpath = _tileDA.GetPathByFileName(pvfn);
            if (File.Exists(previewpath))
            {
                MemoryStream ms = new MemoryStream();
                //存在1000*1000可视化影像
                Image originalImage = Image.FromFile(previewpath);


                int x = 0;
                int y = 0;
                int ow = originalImage.Width;
                int oh = originalImage.Height;

                int towidth = (int)(originalImage.Width * p);
                int toheight = (int)(originalImage.Height * p);

                //新建一个bmp图片 
                Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

                //新建一个画板 
                Graphics g = System.Drawing.Graphics.FromImage(bitmap);

                //设置高质量插值法 
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;

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
                    if (savepath != "")
                    {
                        //以jpg格式保存缩略图 
                        bitmap.Save(savepath, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }

                    bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    outdata = ms.GetBuffer();
                }
                catch (System.Exception e)
                {
                    throw e;
                }
                finally
                {
                    originalImage.Dispose();
                    bitmap.Dispose();
                    g.Dispose();
                    ms.Close();
                }
            }

            return outdata;
        }

        private static System.Drawing.Imaging.ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            System.Drawing.Imaging.ImageCodecInfo[] encoders;
            encoders = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }
        /// <summary>
        /// 将文件转换成byte[] 数组
        /// </summary>
        /// <param name="fileUrl">文件路径文件名称</param>
        /// <returns>byte[]</returns>
        protected static byte[] GetFileData(string fileUrl)
        {
            FileStream fs = new FileStream(fileUrl, FileMode.Open, FileAccess.Read);
            try
            {
                byte[] buffur = new byte[fs.Length];
                fs.Read(buffur, 0, (int)fs.Length);

                return buffur;
            }
            catch (Exception ex)
            {
                //MessageBoxHelper.ShowPrompt(ex.Message);
                return null;
            }
            finally
            {
                if (fs != null)
                {

                    //关闭资源
                    fs.Close();
                }
            }
        }
    }
}
