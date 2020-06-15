using System;

namespace QRST.WorldGlobeTool.Stores
{
    public class GeotiffLayerStore : ImageStore
    {
        public string[] Layers = null;
        int m_textureSizePixels = 512;
        public string formate = "image/png";

        //public override Texture LoadFile(QuadTile qt)
        //{
        //    //获取切片的本地路径
        //    string filePath = GetLocalPath(qt);
        //    qt.ImageFilePath = filePath;

        //    Texture texture = null;
        //    string ddsPath = filePath;
        //    ////判断是否是DDS文件
        //    if (World.Settings.ConvertDownloadedImagesToDds)
        //        ddsPath = Path.GetDirectoryName(filePath) + "\\" + Path.GetFileNameWithoutExtension(filePath) + ".dds";

        //    filePath = ddsPath;
        //    //判断磁盘上，是否有当前文件，若有，则直接读取，若没有，则重新加载
        //    if (!File.Exists(filePath))
        //    {
        //        filePath = Path.GetDirectoryName(filePath) + "\\" + Path.GetFileNameWithoutExtension(filePath) + "." + ImageExtension;
        //        //动态读取Tiff影像中的值
        //        SharpMap.Map map = new SharpMap.Map();
        //        for (int i = 0; i < Layers.Length; i++)
        //        {
        //            SharpMap.Layers.GdalRasterLayer layer;
        //            layer = new SharpMap.Layers.GdalRasterLayer(Path.GetFileNameWithoutExtension(Layers[i]), Layers[i]);

        //            layer.SRID = 4326;
        //            layer.Enabled = true;
        //            map.Layers.Add(layer);
        //        }

        //        //设置Map的背景色为透明色
        //        map.BackColor = System.Drawing.Color.Transparent;
        //        //设置地图的投影
        //        map.Projection = CoordinatesDescription.GCS;
        //        //图像的formate
        //        System.Drawing.Imaging.ImageCodecInfo imageEncoder = GetEncoderInfo(formate);
        //        //图片的大小
        //        map.Size = new System.Drawing.Size(m_textureSizePixels, m_textureSizePixels);
        //        //请求图像的范围
        //        string bboxString = "";
        //        bboxString = qt.West.ToString() + "," + qt.South.ToString() + "," + qt.East.ToString() + "," + qt.North.ToString();
        //        SharpMap.Geometries.BoundingBox bbox = this.ParseBBOX(bboxString);
        //        //判断图像的拉伸范围
        //        map.PixelAspectRatio = ((double)m_textureSizePixels / (double)m_textureSizePixels) / (bbox.Width / bbox.Height);
        //        map.Center = bbox.GetCentroid();
        //        map.Zoom = bbox.Width;
        //        //获得当前请求的图片
        //        System.Drawing.Image img = map.GetMap();
        //        if (!Directory.Exists(Path.GetDirectoryName(filePath)))
        //            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
        //        //保存到本地磁盘

        //        img.Save(filePath);
        //    }
        //    //设置是否显示图片的无效值
        //    if (qt.QuadTileSet.HasTransparentRange)
        //    {
        //        texture = ImageHelper.LoadTexture(filePath, qt.QuadTileSet.ColorKey,
        //            qt.QuadTileSet.ColorKeyMax);
        //    }
        //    else
        //    {
        //        texture = ImageHelper.LoadTexture(filePath, qt.QuadTileSet.ColorKey);
        //    }

        //    ////判断，是否转化为dds文件
        //    if (World.Settings.ConvertDownloadedImagesToDds)
        //        ConvertImage(texture, filePath);
        //    //返回当前纹理
        //    return texture;

        //}

        private System.Drawing.Imaging.ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            foreach (System.Drawing.Imaging.ImageCodecInfo encoder in System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders())
                if (encoder.MimeType == mimeType)
                    return encoder;
            return null;
        }
        //private SharpMap.Geometries.BoundingBox ParseBBOX(string strBBOX)
        //{
        //    string[] strVals = strBBOX.Split(new char[] { ',' });
        //    if (strVals.Length != 4)
        //        return null;
        //    double minx = 0; double miny = 0;
        //    double maxx = 0; double maxy = 0;
        //    if (!double.TryParse(strVals[0], System.Globalization.NumberStyles.Float, SharpMap.Map.NumberFormatEnUs, out minx))
        //        return null;
        //    if (!double.TryParse(strVals[2], System.Globalization.NumberStyles.Float, SharpMap.Map.NumberFormatEnUs, out maxx))
        //        return null;
        //    if (maxx < minx)
        //        return null;

        //    if (!double.TryParse(strVals[1], System.Globalization.NumberStyles.Float, SharpMap.Map.NumberFormatEnUs, out miny))
        //        return null;
        //    if (!double.TryParse(strVals[3], System.Globalization.NumberStyles.Float, SharpMap.Map.NumberFormatEnUs, out maxy))
        //        return null;
        //    if (maxy < miny)
        //        return null;

        //    return new SharpMap.Geometries.BoundingBox(minx, miny, maxx, maxy);
        //}

    }
}
