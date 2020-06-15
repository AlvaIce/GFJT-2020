using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.DirectX.Direct3D;

namespace QRST.WorldGlobeTool.Utility
{
    /// <summary>
    /// Various image manipulation functions.
    /// 提供多个影像处理方法
    /// </summary>
    public sealed class ImageHelper
    {
        /// <summary>
        /// Static class
        /// 静态类，私有构造函数，防止被初始化
        /// </summary>
        private ImageHelper()
        {
        }

        #region 图像纹理加载

        /// <summary>
        /// Loads an image file from disk into a texture.
        /// 从磁盘中加载一个图像文件到纹理中
        /// </summary>
        /// <param name="textureFileName">Path/filename to the image file图像文件路径/文件名</param>
        public static Texture LoadTexture(string textureFileName)
        {
            Texture texture = LoadTexture(textureFileName, 0);
            return texture;
        }

        /// <summary>
        /// Loads an image file from disk into a texture.
        /// 从磁盘中加载一个图像文件到纹理中
        /// </summary>
        /// <param name="textureFileName">Path/filename to the image file图像文件路径/文件名</param>
        /// <param name="colorKey">Transparent color. Any pixels in the image with this color will be made transparent.
        /// 透明色，图像中到任意一个拥有此颜色值到像素都将被设置为透明</param>
        public static Texture LoadTexture(string textureFileName, int colorKey)
        {
            try
            {
                using (Stream imageStream = File.OpenRead(textureFileName))
                    return LoadTexture(imageStream, colorKey);
            }
            catch
            {
                throw new Microsoft.DirectX.Direct3D.InvalidDataException(string.Format("Error reading image file '{0}'.", textureFileName));
            }
        }

        /// <summary>
        /// 从磁盘中加载一个图像文件到纹理中，按照图像原有大小进行加载
        /// </summary>
        /// <param name="textureFileName">纹理文件名称</param>
        /// <param name="colorKey">透明色</param>
        /// <returns>返回图像纹理</returns>
        public static Texture LoadTextureWidthOriginalSize(string textureFileName, int colorKey)
        {
            try
            {
                if (Path.GetExtension(textureFileName).ToLower() == ".dds")
                {
                    return LoadTexture(textureFileName, colorKey);
                }
                else
                {
                    using (Image textureFileImage = Image.FromFile(textureFileName))
                    {
                        return TextureLoader.FromFile(DrawArgs.Device, textureFileName,
                        textureFileImage.Width, textureFileImage.Height,
                        1, Usage.None, World.Settings.TextureFormat, Pool.Managed, Filter.Box, Filter.Box, colorKey);
                    }
                }
                
            }
            catch (Microsoft.DirectX.Direct3D.InvalidDataException)
            {
            }

            try
            {
                // DirectX failed to load the file, try GDI+
                // Additional formats supported by GDI+: GIF, TIFF
                // TODO: Support color keying.  See: System.Drawing.Imaging.ImageAttributes
                using (Bitmap image = (Bitmap)Image.FromFile(textureFileName))
                {
                    Texture texture = new Texture(DrawArgs.Device, image, Usage.None, Pool.Managed);
                    return texture;
                }
            }
            catch
            {
                throw new Microsoft.DirectX.Direct3D.InvalidDataException("Error reading image stream.");
            }
        }

        /// <summary>
        /// Loads an image file from disk into a texture and makes a color range transparent.
        /// 从磁盘中加载一个图像文件到纹理中，并设置一个透明颜色范围
        /// </summary>
        /// <param name="textureFileName">Path/filename to the image file图像文件路径/文件名</param>
        /// <param name="transparentRangeDarkColor">Color for start of transparent range.透明范围的起始颜色</param>
        /// <param name="transparentRangeBrightColor">Color for end of transparent range.透明范围的结束颜色</param>
        /// <returns>返回图像纹理</returns>
        public static Texture LoadTexture(string textureFileName, int transparentRangeDarkColor, int transparentRangeBrightColor)
        {
            Bitmap image = (Bitmap)Image.FromFile(textureFileName);

            BitmapData srcInfo = image.LockBits(new Rectangle(0, 0, image.Width, image.Height),
                ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            // We must always copy it because the source might not be 32bpp ARGB
            Bitmap transparentImage = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppArgb);

            BitmapData dstInfo = transparentImage.LockBits(new Rectangle(0, 0, transparentImage.Width, transparentImage.Height),
                ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            // TODO: Optimize this code
            int max = 3 * (transparentRangeBrightColor & 0xff);
            int min = 3 * (transparentRangeDarkColor & 0xff);
            unsafe
            {
                int* srcPointer = (int*)srcInfo.Scan0;
                int* dstPointer = (int*)dstInfo.Scan0;
                for (int i = 0; i < dstInfo.Height; i++)
                {
                    for (int j = 0; j < dstInfo.Width; j++)
                    {
                        int color = *srcPointer++;
                        int sum = (color & 0xff) +
                            ((color >> 8) & 0xff) +
                            ((color >> 16) & 0xff);

                        if (sum <= max && sum >= min)
                        {
                            color &= 0xffffff; // strip alpha
                            // Add linear alpha: min = transparent, max = opaque
                            color |= (255 * (sum - min) / (max - min)) << 24;
                        }

                        *dstPointer++ = color;
                    }

                    srcPointer += (srcInfo.Stride >> 2) - srcInfo.Width;
                    dstPointer += (srcInfo.Stride >> 2) - srcInfo.Width;
                }
            }
            transparentImage.UnlockBits(dstInfo);
            image.UnlockBits(srcInfo);

            return new Texture(DrawArgs.Device, transparentImage, Usage.None, Pool.Managed);
        }

        /// <summary>
        /// Creates a texture from a data stream.
        /// 从数据流中创建一个纹理
        /// </summary>
        /// <param name="textureStream">Stream containing the image file包含图像文件的数据流</param>
        public static Texture LoadTexture(Stream textureStream)
        {
            Texture texture = LoadTexture(textureStream, 0);
            return texture;
        }

        /// <summary>
        /// Creates a texture from a data stream.
        /// 从数据流中创建一个纹理
        /// </summary>
        /// <param name="textureStream">Stream containing the image file包含图像文件的数据流</param>
        /// <param name="colorKey">Transparent color. Any pixels in the image with this color will be made transparent.
        /// 透明色，图像中到任意一个拥有此颜色值到像素都将被设置为透明</param>
        public static Texture LoadTexture(Stream textureStream, int colorKey)
        {
            try
            {
                Texture texture = TextureLoader.FromStream(DrawArgs.Device, textureStream, 0, 0,
                    1, Usage.None, World.Settings.TextureFormat, Pool.Managed, Filter.Box, Filter.Box, colorKey);

                return texture;
            }
            catch (Microsoft.DirectX.Direct3D.InvalidDataException)
            {
            }

            try
            {
                // DirectX failed to load the file, try GDI+
                // Additional formats supported by GDI+: GIF, TIFF
                // TODO: Support color keying.  See: System.Drawing.Imaging.ImageAttributes
                using (Bitmap image = (Bitmap)Image.FromStream(textureStream))
                {
                    Texture texture = new Texture(DrawArgs.Device, image, Usage.None, Pool.Managed);
                    return texture;
                }
            }
            catch
            {
                throw new Microsoft.DirectX.Direct3D.InvalidDataException("Error reading image stream.");
            }
        }

        /// <summary>
        /// Loads image from file. Returns dummy image on load fail.
        /// 从文件加载影像，加载失败时返回默认的空影像
        /// </summary>
        public static Image LoadImage(string bitmapFileName)
        {
            try
            {
                return Image.FromFile(bitmapFileName);
            }
            catch
            {
                Log.Write(Log.Levels.Error, "IMAG", "Error loading image '" + bitmapFileName + "'.");
                return CreateDefaultImage();
            }
        }

        /// <summary>
        /// Loads a custom mouse cursor from file
        /// 从文件中加载一个自定义的鼠标光标
        /// </summary>
        /// <param name="relativePath">Path and filename of the .cur file relative to Data\Icons\Interface</param>
        public static Cursor LoadCursor(string relativePath)
        {
            //string fullPath = Path.Combine("Data\\Icons\\Interface", relativePath);
            try
            {
                Cursor res = new Cursor(relativePath);
                return res;
            }
            catch (Exception caught)
            {
                Log.Write(Log.Levels.Error, "IMAG", "Unable to load cursor '" + relativePath + "': " + caught.Message);
                return Cursors.Default;
            }
        }

        /// <summary>
        /// Loads an icon texture from a file
        /// 从文件中加载一个ICON文理
        /// </summary>
        /// <param name="relativePath">Path and filename relative to Data\Icons</param>
        public static Texture LoadIconTexture(string relativePath)
        {
            try
            {
                string fullPath = FindResource(relativePath);
                if (File.Exists(fullPath))
                    return TextureLoader.FromFile(DrawArgs.Device, fullPath, 0, 0, 1, Usage.None,
                        Format.Dxt5, Pool.Managed, Filter.Box, Filter.Box, 0);
            }
            catch
            {
                Log.Write(Log.Levels.Error, "IMAG-ICON", "Error loading texture '" + relativePath + "'.");
            }

            // Make a replacement warning texture with a red cross over.
            using (Bitmap bitmap = CreateDefaultImage())
                return new Texture(DrawArgs.Device, bitmap, 0, Pool.Managed);
        }

        /// <summary>
        /// Loads an gcp texture from a file
        /// 从文件中加载一个GCP文理
        /// </summary>
        /// <param name="relativePath">Path and filename relative to Data\Icons</param>
        public static Texture LoadGCPTexture(string relativePath)
        {
            try
            {
                string fullPath = FindResource(relativePath);
                if (File.Exists(fullPath))
                    return TextureLoader.FromFile(DrawArgs.Device, fullPath, 0, 0, 1, Usage.None,
                        Format.Dxt5, Pool.Managed, Filter.Box, Filter.Box, 0);
            }
            catch
            {
                Log.Write(Log.Levels.Error, "IMAG-GCP", "Error loading texture '" + relativePath + "'.");
            }

            // Make a replacement warning texture with a red cross over.
            using (Bitmap bitmap = CreateDefaultImage())
                return new Texture(DrawArgs.Device, bitmap, 0, Pool.Managed);
        }

        #endregion

        #region 图像文件格式转换

        /// <summary>
        /// Converts an image in any format readable by GDI+ to a DXT1 DDS file.
        /// 将任意由GDI+可读的影像转换为一个DXT1格式的DDS文件
        /// </summary>
        /// <param name="originalImagePath">Input file (any supported format).原始影像路径</param>
        /// <param name="outputDdsPath">Output file to be created.输出DDS文件路径</param>
        /// <param name="eraseOriginal">Is delete original file or not.是否删除原始文件</param>
        public static void ConvertToDxt1(string originalImagePath, string outputDdsPath, bool eraseOriginal)
        {
            ConvertToDds(originalImagePath, outputDdsPath, Format.Dxt1, eraseOriginal);
        }

        /// <summary>
        /// Converts an image in any format readable by GDI+ to a DXT1 DDS file.
        /// 将任意由GDI+可读的影像转换为一个DXT1格式的DDS文件
        /// </summary>
        /// <param name="originalImageStream">Stream containing a bitmap.包含一个位图的流</param>
        /// <param name="outputDdsPath">Output file to be created.输出DDS文件路径</param>
        public static void ConvertToDxt1(Stream originalImageStream, string outputDdsPath)
        {
            ConvertToDds(originalImageStream, outputDdsPath, Format.Dxt1);
        }

        /// <summary>
        /// Converts an image in any format readable by GDI+ to a DXT3 DDS file.
        /// 将任意由GDI+可读的影像转换为一个DXT3格式的DDS文件
        /// </summary>
        /// <param name="originalImagePath">Input file (any supported format).原始影像路径</param>
        /// <param name="outputDdsPath">Output file to be created.输出DDS文件路径</param>
        /// <param name="eraseOriginal">Is delete original file or not.是否删除原始文件</param>
        public static void ConvertToDxt3(string originalImagePath, string outputDdsPath, bool eraseOriginal)
        {
            ConvertToDds(originalImagePath, outputDdsPath, Format.Dxt3, eraseOriginal);
        }

        /// <summary>
        /// Converts an image in any format readable by GDI+ to a DXT3 DDS file.
        /// 将任意由GDI+可读的影像转换为一个DXT3格式的DDS文件
        /// </summary>
        /// <param name="originalImageStream">Stream containing a bitmap.包含一个位图的流</param>
        /// <param name="outputDdsPath">Output file to be created.输出DDS文件路径</param>
        public static void ConvertToDxt3(Stream originalImageStream, string outputDdsPath)
        {
            ConvertToDds(originalImageStream, outputDdsPath, Format.Dxt3);
        }

        /// <summary>
        /// Converts an image in any format readable by GDI+ to a DDS file.
        /// 将任意由GDI+可读的影像转换为一个DDS文件
        /// </summary>
        /// <param name="originalImagePath">Input file (any supported format).原始影像路径</param>
        /// <param name="outputDdsPath">Output file to be created.输出DDS文件路径</param>
        /// <param name="format">DirectX format of file.DirectX文件格式</param>
        /// <param name="eraseOriginal">Is delete original file or not.是否删除原始文件</param>
        public static void ConvertToDds(string originalImagePath, string outputDdsPath, Format format, bool eraseOriginal)
        {
            try
            {
                Image originalImage = Image.FromFile(originalImagePath);
                using (Texture t = TextureLoader.FromFile(
                                 DrawArgs.Device,
                                 originalImagePath,
                                 originalImage.Width, originalImage.Height,
                                 1, 0, format, Pool.Scratch,
                                 Filter.Box | Filter.DitherDiffusion, Filter.None, 0))
                    TextureLoader.Save(outputDdsPath, ImageFileFormat.Dds, t);

                if (eraseOriginal)
                    File.Delete(originalImagePath);
            }
            catch (Microsoft.DirectX.Direct3D.InvalidDataException)
            {
                throw new ApplicationException(string.Format("Failed to load image data from {0}.", originalImagePath));
            }
        }

        /// <summary>
        /// Converts an image in any format readable by GDI+ to a DDS file.
        /// 将任意由GDI+可读的影像转换为一个DDS文件
        /// </summary>
        /// <param name="originalImageStream">Input file (any supported format).原始影像流</param>
        /// <param name="outputDdsPath">Output file to be created.输出DDS文件路径</param>
        /// <param name="format">DirectX format of file.DirectX文件格式</param>
        public static void ConvertToDds(Stream originalImageStream, string outputDdsPath, Format format)
        {
            try
            {
                originalImageStream.Seek(0, SeekOrigin.Begin);
                using (Texture t = TextureLoader.FromStream(
                                 DrawArgs.Device,
                                 originalImageStream,
                                 0, 0,
                                 1, 0, format, Pool.Scratch,
                                 Filter.Box | Filter.DitherDiffusion, Filter.None, 0))
                    TextureLoader.Save(outputDdsPath, ImageFileFormat.Dds, t);
            }
            catch (Microsoft.DirectX.Direct3D.InvalidDataException)
            {
                throw new ApplicationException("Failed to load image data from stream.");
            }
        }


        /// <summary>
        /// 把输入的图像纹理转换为DDS格式文件
        /// </summary>
        /// <param name="texture">图像纹理</param>
        /// <param name="filePath">图像文件路径</param>
        /// <param name="eraseOriginal">是否删除原有文件，默认不删除</param>
        public static void ConvertTextureToDDS(Texture texture, string filePath, bool eraseOriginal = false)
        {
            if (filePath.ToLower().EndsWith(".dds"))  // Image is already DDS                
                return;

            string convertedPath = Path.Combine(
                Path.GetDirectoryName(filePath),
                Path.GetFileNameWithoutExtension(filePath) + ".dds");
            TextureLoader.Save(convertedPath, ImageFileFormat.Dds, texture);
            
            if (eraseOriginal)
            {
                // Delete the old file
                try
                {
                    File.Delete(filePath);
                }
                catch
                {  }
            }            
        }


        #endregion

        #region 公共方法

        /// <summary>
        /// Tests based on file extension whether the image format is supported by GDI+ image loader.
        /// 基于文件扩展名测试影像格式是否被GDI+支持加载
        /// </summary>
        /// <param name="imageFileName">Full path or just filename incl. extension.</param>
        public static bool IsGdiSupportedImageFormat(string imageFileName)
        {
            string extension = Path.GetExtension(imageFileName).ToLower();
            const string GdiSupportedExtensions = ".bmp.gif.jpg.jpeg.png.gif.tif";
            return GdiSupportedExtensions.IndexOf(extension) >= 0;
        }

        /// <summary>
        /// Tries it's best to locate an image file specified using relative path.
        /// 使用相对路径尽最大可能定位一个图像文件
        /// </summary>
        /// <param name="relativePath">相对路径</param>
        /// <returns>返回图像文件到绝对路径</returns>
        public static string FindResource(string relativePath)
        {
            if (File.Exists(relativePath))
                return relativePath;

            FileInfo executableFile = new FileInfo(System.Windows.Forms.Application.ExecutablePath);

            string fullPath = Path.Combine(Path.Combine(executableFile.Directory.FullName, "Data"), relativePath);
            if (File.Exists(fullPath))
                return fullPath;

            fullPath = Path.Combine(executableFile.Directory.FullName, relativePath);
            if (File.Exists(fullPath))
                return fullPath;

            fullPath = Path.Combine(Path.Combine(executableFile.Directory.FullName, "Data\\Icons"), relativePath);
            return fullPath;
        }

        /// <summary>
        /// Makes a default image to use when the requested bitmap wasn't available.
        /// 在需要到位图不可用时，创建一个默认到32×32大小到位图供使用
        /// </summary>
        public static Bitmap CreateDefaultImage()
        {
            Bitmap b = new Bitmap(32, 32);
            using (Graphics g = Graphics.FromImage(b))
            {
                g.Clear(Color.FromArgb(88, 255, 255, 255));
                g.DrawLine(Pens.Red, 0, 0, b.Width, b.Height);
                g.DrawLine(Pens.Red, 0, b.Height, b.Width, 0);
            }
            return b;
        }

        /// <summary>
        /// 从亮度文件中创建带透明度到PNG图像
        /// </summary>
        /// <param name="srcFilePath">原始文件路径</param>
        /// <param name="destinationPngFilePath">目标PNG文件路径</param>
        public static void CreateAlphaPngFromBrightness(string srcFilePath, string destinationPngFilePath)
        {
            Bitmap image = (Bitmap)Image.FromFile(srcFilePath);

            BitmapData srcInfo = image.LockBits(new Rectangle(0, 0, image.Width, image.Height),
                ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            // We must always copy it because the source might not be 32bpp ARGB
            Bitmap transparentImage = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppArgb);

            BitmapData dstInfo = transparentImage.LockBits(new Rectangle(0, 0, transparentImage.Width, transparentImage.Height),
                ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            unsafe
            {
                int* srcPointer = (int*)srcInfo.Scan0;
                int* dstPointer = (int*)dstInfo.Scan0;
                for (int i = 0; i < dstInfo.Height; i++)
                {
                    for (int j = 0; j < dstInfo.Width; j++)
                    {
                        int color = *srcPointer++;
                        int sum = (color & 0xff) +
                            ((color >> 8) & 0xff) +
                            ((color >> 16) & 0xff);

                        color &= 0xffffff; // strip alpha
                        color |= (sum / 3) << 24;

                        *dstPointer++ = color;
                    }

                    srcPointer += (srcInfo.Stride >> 2) - srcInfo.Width;
                    dstPointer += (srcInfo.Stride >> 2) - srcInfo.Width;
                }
            }
            transparentImage.UnlockBits(dstInfo);
            image.UnlockBits(srcInfo);

            transparentImage.Save(destinationPngFilePath, System.Drawing.Imaging.ImageFormat.Png);
            image.Dispose();
            transparentImage.Dispose();
        }

        #endregion

    }
}
