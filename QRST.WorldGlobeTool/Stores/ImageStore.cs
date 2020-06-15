﻿using System;
using System.IO;
using Microsoft.DirectX.Direct3D;
using QRST.WorldGlobeTool.Utility;
using QRST.WorldGlobeTool.Renderable;

namespace QRST.WorldGlobeTool.Stores
{
    /// <summary>
    /// Base class for calculating local image paths and remote download urls
    /// 计算本地影像路径和远端下载RUL基类
    /// </summary>
    public class ImageStore
    {
        #region 字段

        /// <summary>
        /// 数据存放路径
        /// </summary>
        protected string m_dataDirectory;
        /// <summary>
        /// 0级切片度数
        /// </summary>
        protected double m_levelZeroTileSizeDegrees = 36;
        /// <summary>
        /// 分级数
        /// </summary>
        protected int m_levelCount = 1;
        /// <summary>
        /// 图片的后缀名
        /// </summary>
        protected string m_imageFileExtension;
        /// <summary>
        /// 缓存路径
        /// </summary>
        protected string m_cacheDirectory;
        /// <summary>
        /// 复制的纹理路径
        /// </summary>
        protected string m_duplicateTexturePath;
        /// <summary>
        /// 服务器logo的路径
        /// </summary>
        protected string m_serverlogo;

        #endregion

        #region 属性

        /// <summary>
        /// 0级切片度数
        /// Coverage of outer level 0 bitmaps (decimal degrees)
        /// Level 1 has half the coverage, level 2 half of level 1 (1/4) etc.
        /// </summary>
        public double LevelZeroTileSizeDegrees
        {
            get
            {
                return m_levelZeroTileSizeDegrees;
            }
            set
            {
                m_levelZeroTileSizeDegrees = value;
            }
        }

        /// <summary>
        /// 服务器logo的路径
        /// Server Logo path for Downloadable layers
        /// </summary>
        public string ServerLogo
        {
            get
            {
                return m_serverlogo;
            }
            set
            {
                m_serverlogo = value;
            }
        }

        /// <summary>
        /// 分级数
        /// Number of detail levels
        /// </summary>
        public int LevelCount
        {
            get
            {
                return m_levelCount;
            }
            set
            {
                m_levelCount = value;
            }
        }

        /// <summary>
        /// 图片的后缀名
        /// File extension of the source image file format
        /// </summary>
        public string ImageExtension
        {
            get
            {
                return m_imageFileExtension;
            }
            set
            {
                // Strip any leading dot
                m_imageFileExtension = value.Replace(".", "");
            }
        }

        /// <summary>
        /// 缓存路径
        /// Cache subdirectory for this layer
        /// </summary>
        public string CacheDirectory
        {
            get
            {
                return m_cacheDirectory;
            }
            set
            {
                m_cacheDirectory = value;
            }
        }

        /// <summary>
        /// 数据存放路径
        /// Data directory for this layer (permanently stored images)
        /// </summary>
        public string DataDirectory
        {
            get
            {
                return m_dataDirectory;
            }
            set
            {
                m_dataDirectory = value;
            }
        }

        /// <summary>
        /// 默认的纹理路径。
        /// Default texture to be used (always ocean?)
        /// Can be either file or url
        /// </summary>
        public string DuplicateTexturePath
        {
            get
            {
                return m_duplicateTexturePath;
            }
            set
            {
                m_duplicateTexturePath = value;
            }
        }

        /// <summary>
        /// 是否可以被下载
        /// </summary>
        public virtual bool IsDownloadableLayer
        {
            get
            {
                //return true;
                return false;  //20130922-ZYM：禁用下载操作
            }
        }

        #endregion
        
        /// <summary>
        /// 获取切片的本地存放路径或缓存存放路径
        /// </summary>
        /// <param name="qt"></param>
        /// <returns></returns>
        public virtual string GetLocalPath(QuadTile qt)
        {
            return GetLocalPath(qt.Level, qt.Row, qt.Col);
        }
        /// <summary>
        /// 获取切片的本地存放路径或缓存存放路径
        /// </summary>
        /// <param name="qt"></param>
        /// <returns></returns>
        public virtual string GetLocalPath(int Level,int Row, int Col)
        {
            //判断请求的层数，是否超出了范围
            if (Level >= m_levelCount)
                throw new ArgumentException(string.Format("层 {0} 不可用.",
                    Level));

            //获得请求的相对路径
            //string relativePath = String.Format(@"{0}\{1:D4}\{1:D4}_{2:D4}.{3}",
            //    qt.Level, qt.Row, qt.Col, m_imageFileExtension);
            string relativePath = String.Format(@"{0}\{1}\{1}_{2}",
                Level, Row, Col);

            //判断m_dataDirectory路径是否存在
            if (m_dataDirectory != null)
            {
                // Search data directory first
                string rawFullPath = Path.Combine(m_dataDirectory, relativePath);
                if (File.Exists(rawFullPath+".dds"))
                    return rawFullPath + ".dds";
                else if (File.Exists(rawFullPath + ".jpg"))
                    return rawFullPath + ".jpg";
                else if (File.Exists(rawFullPath + ".png"))
                    return rawFullPath + ".png";
                else if (File.Exists(rawFullPath + ".bil"))
                    return rawFullPath + ".bil";
                else if (File.Exists(rawFullPath + ".tif"))
                    return rawFullPath + ".tif";
                else if (File.Exists(rawFullPath + ".bmp"))
                    return rawFullPath + ".bmp";
                else if (File.Exists(rawFullPath + ".dib"))
                    return rawFullPath + ".dib";
                else if (File.Exists(rawFullPath + ".hdr"))
                    return rawFullPath + ".hdr";
                else if (File.Exists(rawFullPath + ".jpeg"))
                    return rawFullPath + ".jpeg";
                else if (File.Exists(rawFullPath + ".pfm"))
                    return rawFullPath + ".pfm";
                else if (File.Exists(rawFullPath + ".ppm"))
                    return rawFullPath + ".ppm";
                else if (File.Exists(rawFullPath + ".gif"))
                    return rawFullPath + ".gif";
                else if (File.Exists(rawFullPath + ".tga"))
                    return rawFullPath + ".tga";
            }

            //若缓存路径不存在，则返回默认纹理的路径
            // If cache doesn't exist, fall back to duplicate texture path.
            if (m_cacheDirectory == null)
                return m_duplicateTexturePath;

            //获得Cache的存放全部路径
            // Try cache with default file extension
            string cacheFullPath = Path.Combine(m_cacheDirectory, relativePath);

            if (File.Exists(cacheFullPath + ".jpg"))
                return cacheFullPath + ".jpg";
            else if (File.Exists(cacheFullPath + ".png"))
                return cacheFullPath + ".png";
            else if (File.Exists(cacheFullPath + ".dds"))
                return cacheFullPath + ".dds";
            else  if (File.Exists(cacheFullPath + ".bil"))
                return cacheFullPath + ".bil";
            else if (File.Exists(cacheFullPath + ".tif"))
                return cacheFullPath + ".tif";
            else if (File.Exists(cacheFullPath + ".bmp"))
                return cacheFullPath + ".bmp";
            else if (File.Exists(cacheFullPath + ".dib"))
                return cacheFullPath + ".dib";
            else if (File.Exists(cacheFullPath + ".hdr"))
                return cacheFullPath + ".hdr";
            else if (File.Exists(cacheFullPath + ".jpeg"))
                return cacheFullPath + ".jpeg";
            else if (File.Exists(cacheFullPath + ".pfm"))
                return cacheFullPath + ".pfm";
            else if (File.Exists(cacheFullPath + ".ppm"))
                return cacheFullPath + ".ppm";
            else if (File.Exists(cacheFullPath + ".gif"))
                return cacheFullPath + ".gif";
            else if (File.Exists(cacheFullPath + ".tga"))
                return cacheFullPath + ".tga";

            // Try cache but accept any valid image file extension
            const string ValidExtensions = ".bmp.dds.dib.hdr.jpg.jpeg.pfm.png.ppm.tga.gif.tif";

            string cacheSearchPath = Path.GetDirectoryName(cacheFullPath);
            if (Directory.Exists(cacheSearchPath))
            {
                foreach (string imageFile in Directory.GetFiles(
                    cacheSearchPath,
                    Path.GetFileNameWithoutExtension(cacheFullPath) + ".*"))
                {
                    string extension = Path.GetExtension(imageFile).ToLower();
                    if (ValidExtensions.IndexOf(extension) < 0)
                        continue;

                    return imageFile;
                }
            }

            return "";
        }

        /// <summary>
        /// Figure out how to download the image.
        /// TODO: Allow subclasses to have control over how images are downloaded, 
        /// not just the download url.
        /// 允许子类可以控制如何下载影像，不仅仅靠下载的URL
        /// </summary>
        protected virtual string GetDownloadUrl(QuadTile qt)
        {
            // No local image, return our "duplicate" tile if any
            if (m_duplicateTexturePath != null && File.Exists(m_duplicateTexturePath))
                return m_duplicateTexturePath;

            // No image available anywhere, give up
            return "";
        }

        /// <summary>
        /// 删除本地的切片
        /// Deletes the cached copy of the tile.
        /// </summary>
        /// <param name="qt"></param>
        public virtual void DeleteLocalCopy(QuadTile qt)
        {
            string filename = GetLocalPath(qt);
            if (File.Exists(filename))
                File.Delete(filename);
        }


        /// <summary>
        /// 把Image转换为DDS。
        /// Converts image file to DDS
        /// </summary>
        protected virtual void ConvertImage(Texture texture, string filePath)
        {
            if (filePath.ToLower().EndsWith(".dds"))
                // Image is already DDS
                return;

            // User has selected to convert downloaded images to DDS
            string convertedPath = Path.Combine(
                Path.GetDirectoryName(filePath),
                Path.GetFileNameWithoutExtension(filePath) + ".dds");

            TextureLoader.Save(convertedPath, ImageFileFormat.Dds, texture);

            // Delete the old file
            try
            {
                File.Delete(filePath);
            }
            catch
            {
            }
        }

        /// <summary>
        /// 获得纹理图片。
        /// </summary>
        /// <param name="qt"></param>
        /// <returns></returns>
        public virtual Texture LoadFile(QuadTile qt)
        {
            //获取切片的本地路径
            string filePath = GetLocalPath(qt);
            if (filePath=="")
            {
                return null;
            }
            qt.ImageFilePath = filePath;
            if (!File.Exists(filePath))
            {
                return null;
            }

            //if (!File.Exists(filePath))
            //{
            //    string badFlag = filePath + ".txt";
            //    if (File.Exists(badFlag))
            //    {
            //        FileInfo fi = new FileInfo(badFlag);
            //        if (DateTime.Now - fi.LastWriteTime < TimeSpan.FromDays(1))
            //        {
            //            return null;
            //        }
            //        // Timeout period elapsed, retry
            //        File.Delete(badFlag);
            //    }

            //    //是否可以下载，若是可以下载的图层，则进行下载
            //    if (IsDownloadableLayer)
            //    {
            //        QueueDownload(qt, filePath);
            //        return null;
            //    }

            //    if (DuplicateTexturePath == null)
            //        // No image available, neither local nor online.
            //        return null;

            //    filePath = DuplicateTexturePath;
            //}

            //是否使用颜色表
            // Use color key
            Texture texture = null;
            if (qt.QuadTileSet.HasTransparentRange)
            {
                texture = ImageHelper.LoadTexture(filePath, qt.QuadTileSet.ColorKey,
                    qt.QuadTileSet.ColorKeyMax);
            }
            else
            {
                texture = ImageHelper.LoadTexture(filePath, qt.QuadTileSet.ColorKey);
            }


            if (qt.QuadTileSet.CacheExpirationTime != TimeSpan.MaxValue)
            {
                FileInfo fi = new FileInfo(filePath);
                DateTime expiry = fi.LastWriteTimeUtc.Add(qt.QuadTileSet.CacheExpirationTime);
                if (DateTime.UtcNow > expiry)
                    QueueDownload(qt, filePath);
            }

            // Only convert images that are downloadable (don't mess with things the user put here!)
            if (World.Settings.ConvertDownloadedImagesToDds && IsDownloadableLayer)
                ConvertImage(texture, filePath);

            return texture;
        }

        /// <summary>
        /// 下载队列
        /// </summary>
        /// <param name="qt"></param>
        /// <param name="filePath"></param>
        private void QueueDownload(QuadTile qt, string filePath)
        {//TODO:20130917-禁用数据下载
            //string url = GetDownloadUrl(qt);
            //qt.QuadTileSet.AddToDownloadQueue(qt.QuadTileSet.Camera,
            //    new GeoSpatialDownloadRequest(qt, this, filePath, url));
        }
    }
}
