using Microsoft.DirectX.Direct3D;
using System.IO;
using System.Globalization;
using System.Net;
using QRST.WorldGlobeTool.Renderable;
using QRST.WorldGlobeTool.Utility;
using QRST.WorldGlobeTool.Net;

namespace QRST.WorldGlobeTool.Stores
{
    public class TiledImageStore : ImageStore
    {
        public string m_dataSetName;
        public string m_serverUri;

        public override Texture LoadFile(QuadTile qt)
        {
            //获取切片的本地路径
            string filePath = GetLocalPath(qt);
            qt.ImageFilePath = filePath;
            Texture texture = null;
            string ddsPath = filePath;
            ////判断是否是DDS文件
            if (World.Settings.ConvertDownloadedImagesToDds)
                ddsPath = Path.GetDirectoryName(filePath) + "\\" + Path.GetFileNameWithoutExtension(filePath) + ".dds";

            filePath = ddsPath;
            //判断磁盘上，是否有当前文件，若有，则直接读取，若没有，则重新加载
            if (!File.Exists(filePath))
            {
                QueueDownload(qt, filePath);
                //return null;
            }

            //设置是否显示图片的无效值
            if (qt.QuadTileSet.HasTransparentRange)
            {
                texture = ImageHelper.LoadTexture(filePath, qt.QuadTileSet.ColorKey,
                    qt.QuadTileSet.ColorKeyMax);
            }
            else
            {
                texture = ImageHelper.LoadTexture(filePath, qt.QuadTileSet.ColorKey);
            }

            ////判断，是否转化为dds文件
            if (World.Settings.ConvertDownloadedImagesToDds)
                ConvertImage(texture, filePath);
            //返回当前纹理
            return texture;

        }

        void QueueDownload(QuadTile qt, string SavedFilePath)
        {
            string url = GetDownloadUrl(qt);
            // Download to file
            string targetDirectory = Path.GetDirectoryName(SavedFilePath);
            if (targetDirectory.Length > 0)
                Directory.CreateDirectory(targetDirectory);
            FileStream ContentStream = new FileStream(SavedFilePath, FileMode.Create);

            // Create the request object.
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.UserAgent = "";

            request.Proxy = ProxyHelper.DetermineProxyForUrl(
                url,
                true,
                true,
                "",
                "",
                "");
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string strContentLength = response.Headers["Content-Length"];
                    int ContentLength;
                    if (strContentLength != null)
                    {
                        ContentLength = int.Parse(strContentLength, CultureInfo.InvariantCulture);
                    }

                    byte[] readBuffer = new byte[1500];
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        while (true)
                        {
                            int bytesRead = responseStream.Read(readBuffer, 0, readBuffer.Length);
                            if (bytesRead <= 0)
                                break;
                            ContentStream.Write(readBuffer, 0, bytesRead);
                        }
                    }

                }
            }
            ContentStream.Close();
            ContentStream = null;
        }

        protected override string GetDownloadUrl(QuadTile qt)
        {
            return string.Format(CultureInfo.InvariantCulture,
                "{0}?T={1}&L={2}&X={3}&Y={4}", m_serverUri,
                m_dataSetName, qt.Level, qt.Col, qt.Row);
        }
    }
}
