using System;
using System.IO;

namespace QRST.WorldGlobeTool.Terrain
{
    public class TerrainTile : IDisposable
    {
        public string TerrainTileFilePath;
        public double TileSizeDegrees;
        public int SamplesPerTile;
        public double South;
        public double North;
        public double West;
        public double East;
        public int Row;
        public int Col;
        public int TargetLevel;
        public TerrainTileService m_owner;
        public bool IsInitialized;
        public bool IsValid;

        public float[,] ElevationData;
        protected TerrainDownloadRequest request;

        public TerrainTile(TerrainTileService owner)
        {
            m_owner = owner;
        }
        /// <summary>
        /// This method initializes the terrain tile add switches to
        /// Initialize floating point/int 16 tiles
        /// </summary>
        public void Initialize()
        {
            if (IsInitialized)
                return;

            if (!File.Exists(TerrainTileFilePath))
            {
                //TODO:20130922-ZYM:禁用下载高程图片信息
                return;

                // Download elevation
                //if (request == null)
                //{
                //    using (request = new TerrainDownloadRequest(this, m_owner, Row, Col, TargetLevel))
                //    {
                //        request.SaveFilePath = TerrainTileFilePath;
                //        request.DownloadInForeground();
                //    }
                //}
            }

            if (ElevationData == null)
                ElevationData = new float[SamplesPerTile, SamplesPerTile];

            if (File.Exists(TerrainTileFilePath))
            {
                // Load elevation file
                try
                {
                    // TerrainDownloadRequest's FlagBadTile() creates empty files
                    // as a way to flag "bad" terrain tiles.
                    // Remove the empty 'flag' files after preset time.
                    try
                    {
                        FileInfo tileInfo = new FileInfo(TerrainTileFilePath);
                        if (tileInfo.Length == 0)
                        {
                            TimeSpan age = DateTime.Now.Subtract(tileInfo.LastWriteTime);
                            if (age < m_owner.TerrainTileRetryInterval)
                            {
                                // This tile is still flagged bad
                                IsInitialized = true;
                            }
                            else
                            {
                                // remove the empty 'flag' file
                                File.Delete(TerrainTileFilePath);
                            }
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.ToString());
                        // Ignore any errors in the above block, and continue.
                        // For example, if someone had the empty 'flag' file
                        // open, the delete would fail.
                    }

                    using (Stream s = File.OpenRead(TerrainTileFilePath))
                    {
                        BinaryReader reader = new BinaryReader(s);
                        if (m_owner.DataType == "Int16")
                        {
                            /*
                            byte[] tfBuffer = new byte[SamplesPerTile*SamplesPerTile*2];
                            if (s.Read(tfBuffer,0,tfBuffer.Length) < tfBuffer.Length)
                                throw new IOException(string.Format("End of file error while reading terrain file '{0}'.", TerrainTileFilePath) );

                            int offset = 0;
                            for(int y = 0; y < SamplesPerTile; y++)
                                for(int x = 0; x < SamplesPerTile; x++)
                                    ElevationData[x,y] = tfBuffer[offset++] + (short)(tfBuffer[offset++]<<8);
                            */
                            for (int y = 0; y < SamplesPerTile; y++)
                                for (int x = 0; x < SamplesPerTile; x++)
                                    ElevationData[x, y] = reader.ReadInt16();
                        }
                        if (m_owner.DataType == "Float32")
                        {
                            /*
                            byte[] tfBuffer = new byte[SamplesPerTile*SamplesPerTile*4];
                            if (s.Read(tfBuffer,0,tfBuffer.Length) < tfBuffer.Length)
                                    throw new IOException(string.Format("End of file error while reading terrain file '{0}'.", TerrainTileFilePath) );
                            */
                            for (int y = 0; y < SamplesPerTile; y++)
                                for (int x = 0; x < SamplesPerTile; x++)
                                {
                                    ElevationData[x, y] = reader.ReadSingle();
                                }
                        }
                        IsInitialized = true;
                        IsValid = true;
                    }
                    return;
                }
                catch (IOException)
                {
                    // If there is an IO exception when reading the terrain tile,
                    // then either something is wrong with the file, or with
                    // access to the file, so try and remove it.
                    try
                    {
                        File.Delete(TerrainTileFilePath);
                    }
                    catch (Exception ex)
                    {
                        throw new ApplicationException(String.Format("Error while trying to delete corrupt terrain tile {0}", TerrainTileFilePath), ex);
                    }
                }
                catch (Exception ex)
                {
                    // Some other type of error when reading the terrain tile.
                    throw new ApplicationException(String.Format("Error while trying to read terrain tile {0}", TerrainTileFilePath), ex);
                }
            }
        }

        public float GetElevationAt(double latitude, double longitude)
        {
            //TODO:20130922-ZYM:禁用下载高程图片信息
            return 0;


            try
            {
                double deltaLat = North - latitude;
                double deltaLon = longitude - West;

                double df2 = (SamplesPerTile - 1) / TileSizeDegrees;
                float lat_pixel = (float)(deltaLat * df2);
                float lon_pixel = (float)(deltaLon * df2);

                int lat_min = (int)lat_pixel;
                int lat_max = (int)Math.Ceiling(lat_pixel);
                int lon_min = (int)lon_pixel;
                int lon_max = (int)Math.Ceiling(lon_pixel);

                if (lat_min >= SamplesPerTile)
                    lat_min = SamplesPerTile - 1;
                if (lat_max >= SamplesPerTile)
                    lat_max = SamplesPerTile - 1;
                if (lon_min >= SamplesPerTile)
                    lon_min = SamplesPerTile - 1;
                if (lon_max >= SamplesPerTile)
                    lon_max = SamplesPerTile - 1;

                if (lat_min < 0)
                    lat_min = 0;
                if (lat_max < 0)
                    lat_max = 0;
                if (lon_min < 0)
                    lon_min = 0;
                if (lon_max < 0)
                    lon_max = 0;

                float delta = lat_pixel - lat_min;
                float westElevation =
                    ElevationData[lon_min, lat_min] * (1 - delta) +
                    ElevationData[lon_min, lat_max] * delta;

                float eastElevation =
                    ElevationData[lon_max, lat_min] * (1 - delta) +
                    ElevationData[lon_max, lat_max] * delta;

                delta = lon_pixel - lon_min;
                float interpolatedElevation =
                    westElevation * (1 - delta) +
                    eastElevation * delta;

                return interpolatedElevation;
            }
            catch
            {
            }
            return 0;
        }
        #region IDisposable Members

        public void Dispose()
        {
            if (request != null)
            {
                request.Dispose();
                request = null;
            }

            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
