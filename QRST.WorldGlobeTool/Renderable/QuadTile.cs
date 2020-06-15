using System;
using System.Collections.Generic;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
using QRST.WorldGlobeTool.Geometries;
using QRST.WorldGlobeTool.Utility;
using QRST.WorldGlobeTool.Terrain;
using System.IO;

namespace QRST.WorldGlobeTool.Renderable
{
    /// <summary>
    /// 方形瓦片类，代表三维球上每个网格里显示的图片
    /// </summary>
    public class QuadTile : IDisposable
    {
        /// <summary>
        /// 瓦片的孩子的位置枚举
        /// </summary>
        public enum ChildLocation
        {
            /// <summary>
            /// 西北子块
            /// </summary>
            NorthWest,
            /// <summary>
            /// 西南子块
            /// </summary>
            SouthWest,
            /// <summary>
            /// 东北子块
            /// </summary>
            NorthEast,
            /// <summary>
            /// 东南子块
            /// </summary>
            SouthEast
        }

        #region  字段

        /// <summary>
        /// 瓦片数据集
        /// </summary>
        public QuadTileSet QuadTileSet;
        /// <summary>
        /// 最西经度
        /// </summary>
        public double West;
        /// <summary>
        /// 最东经度
        /// </summary>
        public double East;
        /// <summary>
        /// 最北纬度
        /// </summary>
        public double North;
        /// <summary>
        /// 最南纬度
        /// </summary>
        public double South;
        /// <summary>
        /// 中心纬度
        /// </summary>
        public Angle CenterLatitude;
        /// <summary>
        /// 中心纬度
        /// </summary>
        public Angle CenterLongitude;
        /// <summary>
        /// 经度跨度
        /// </summary>
        public double LatitudeSpan;
        /// <summary>
        /// 纬度跨度
        /// </summary>
        public double LongitudeSpan;
        /// <summary>
        /// 瓦片所属的层级
        /// </summary>
        public int Level;
        /// <summary>
        /// 瓦片所在行
        /// </summary>
        public int Row;
        /// <summary>
        /// 瓦片所在列
        /// </summary>
        public int Col;
        /// <summary>
        /// 是否初始化了
        /// </summary>
        public bool isInitialized;
        /// <summary>
        /// 当前瓦片包围盒
        /// </summary>
        public BoundingBox BoundingBox;
        /// <summary>
        /// 地理空间信息下载请求
        /// </summary>
        public GeoSpatialDownloadRequest DownloadRequest;
        /// <summary>
        /// 纹理数组
        /// </summary>
        protected Texture[] textures;
        /// <summary>
        /// 子平面格网的顶点数量：40个顶点
        /// Number of points in child flat mesh grid (times 2)
        /// </summary>
        protected static int vertexCount = 40;
        /// <summary>
        /// 子地形格网的顶点数量：40个顶点
        /// Number of points in child terrain mesh grid (times 2)
        /// </summary>
        protected static int vertexCountElevated = 40;
        /// <summary>
        /// 西北孩子
        /// </summary>
        protected QuadTile northWestChild;
        /// <summary>
        /// 西南孩子
        /// </summary>
        protected QuadTile southWestChild;
        /// <summary>
        /// 东北孩子
        /// </summary>
        protected QuadTile northEastChild;
        /// <summary>
        /// 东南孩子
        /// </summary>
        protected QuadTile southEastChild;
        /// <summary>
        /// 西北孩子顶点
        /// </summary>
        protected CustomVertex.PositionNormalTextured[] northWestVertices;
        /// <summary>
        /// 西南孩子顶点
        /// </summary>
        protected CustomVertex.PositionNormalTextured[] southWestVertices;
        /// <summary>
        /// 东北孩子顶点
        /// </summary>
        protected CustomVertex.PositionNormalTextured[] northEastVertices;
        /// <summary>
        /// 东南孩子顶点
        /// </summary>
        protected CustomVertex.PositionNormalTextured[] southEastVertices;
        /// <summary>
        /// 顶点索引
        /// </summary>
        protected short[] vertexIndexes;
        /// <summary>
        /// 原始位置，增加这个偏移量来获取世界坐标
        /// Add this offset to get world coordinates
        /// </summary>
        protected Point3d localOrigin;
        /// <summary>
        /// 是否重置缓存
        /// </summary>
        protected bool m_isResetingCache;
        /// <summary>
        /// 垂直夸大因子
        /// The vertical exaggeration the tile mesh was computed for
        /// </summary>
        protected float verticalExaggeration;
        /// <summary>
        /// 是否下载地形信息
        /// </summary>
        protected bool isDownloadingTerrain;
        /// <summary>
        /// 是否等待下载
        /// </summary>
        public bool IsWaitingForDownload = false;
        /// <summary>
        /// 是否正在下载
        /// </summary>
        public bool IsDownloadingImage = false;
        /// <summary>
        /// 基础半径
        /// </summary>
        double meshBaseRadius = 0;
        /// <summary>
        /// 影像文件路径
        /// </summary>
        public string ImageFilePath = null;
        /// <summary>
        /// 
        /// </summary>
        bool renderStruts = true;
        /// <summary>
        /// 
        /// </summary>
        static Effect grayscaleEffect = null;
        /// <summary>
        /// 下载矩形框
        /// </summary>
        protected CustomVertex.PositionColored[] downloadRectangle = new CustomVertex.PositionColored[5];
        /// <summary>
        /// 当前不透明度
        /// </summary>
        protected byte m_CurrentOpacity = 255;

        #endregion

        /// <summary>
        /// 初始化一个新的瓦片实例
        /// </summary>
        /// <param name="south">南边纬度</param>
        /// <param name="north">北边纬度</param>
        /// <param name="west">西边经度</param>
        /// <param name="east">东边经度</param>
        /// <param name="level">瓦片所属的层级</param>
        /// <param name="quadTileSet">瓦片所在的数据集</param>
        public QuadTile(double south, double north, double west, double east, int level, QuadTileSet quadTileSet)
        {
            this.South = south;
            this.North = north;
            this.West = west;
            this.East = east;
            CenterLatitude = Angle.FromDegrees(0.5f * (North + South));
            CenterLongitude = Angle.FromDegrees(0.5f * (West + East));
            LatitudeSpan = Math.Abs(North - South);
            LongitudeSpan = Math.Abs(East - West);

            this.Level = level;
            this.QuadTileSet = quadTileSet;

            BoundingBox = new BoundingBox((float)south, (float)north, (float)west, (float)east,
                                (float)quadTileSet.LayerRadius, (float)quadTileSet.LayerRadius  + 300000f);
            //localOrigin = BoundingBox.CalculateCenter();
            localOrigin = MathEngine.SphericalToCartesianD(CenterLatitude, CenterLongitude, quadTileSet.LayerRadius);

            // To avoid gaps between neighbouring tiles truncate the origin to 
            // a number that doesn't get rounded. (nearest 10km)
            localOrigin.X = (float)(Math.Round(localOrigin.X / 10000) * 10000);
            localOrigin.Y = (float)(Math.Round(localOrigin.Y / 10000) * 10000);
            localOrigin.Z = (float)(Math.Round(localOrigin.Z / 10000) * 10000);

            Row = MathEngine.GetRowFromLatitude(South, North - South);
            Col = MathEngine.GetColFromLongitude(West, North - South);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public virtual void Initialize()
        {
            if (m_isResetingCache)
                return;

            try
            {
                if (DownloadRequest != null)
                {
                    // Waiting for download
                    return;
                }
                if (textures == null)
                {
                    textures = new Texture[QuadTileSet.ImageStores.Length];

                    // not strictly necessary
                    for (int i = 0; i < textures.Length; i++)
                        textures[i] = null;
                }

                // 假定已经完成下载
                IsWaitingForDownload = false;

                // 检查丢失的纹理
                for (int i = 0; i < textures.Length; i++)
                {
                    Texture newTexture = QuadTileSet.ImageStores[i].LoadFile(this);
                    if (newTexture == null)
                    {
                        // At least one texture missing, wait for download
                        IsWaitingForDownload = true;
                    }

                    // not entirely sure if this is a good idea...
                    if (textures[i] != null)
                        textures[i].Dispose();

                    textures[i] = newTexture;
                }
                if (IsWaitingForDownload)
                    return;

                IsDownloadingImage = false;
                CreateTileMesh();
            }
            //catch (Microsoft.DirectX.Direct3D.Direct3DXException)
            catch (Exception ex)
            {
                Log.Write(ex);
                // Texture load failed.
            }
            finally
            {
                isInitialized = true;
            }
        }

        /// <summary>
        /// 重置缓存
        /// </summary>
        public virtual void ResetCache()
        {
            try
            {
                m_isResetingCache = true;
                this.isInitialized = false;

                if (northEastChild != null)
                    northEastChild.ResetCache();

                if (northWestChild != null)
                    northWestChild.ResetCache();

                if (southEastChild != null)
                    southEastChild.ResetCache();

                if (southWestChild != null)
                    southWestChild.ResetCache();

                this.Dispose();

                for (int i = 0; i < QuadTileSet.ImageStores.Length; i++)
                {
                    if ((QuadTileSet.ImageStores[i] != null) && QuadTileSet.ImageStores[i].IsDownloadableLayer)
                        QuadTileSet.ImageStores[i].DeleteLocalCopy(this);
                }

                m_isResetingCache = false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        /// <summary>
        /// Returns the QuadTile for specified location if available.
        /// Tries to queue a download if not available.
        /// 返回指定位置的子瓦片，如果不可用则请求下载
        /// </summary>
        /// <returns>Initialized QuadTile if available locally, else null.</returns>
        private QuadTile ComputeChild(double childSouth, double childNorth, double childWest, double childEast)
        {
            return new QuadTile(
                childSouth,
                childNorth,
                childWest,
                childEast,
                this.Level + 1,
                QuadTileSet);
        }

        /// <summary>
        /// 计算当前瓦片的孩子
        /// </summary>
        public virtual void ComputeChildren()
        {
            int nextLevel = Level + 1;
            if (nextLevel >= QuadTileSet.ImageStores[0].LevelCount)
                return;
            double CenterLat = 0.5f * (South + North);
            double CenterLon = 0.5f * (East + West);
            if (northWestChild == null)
                northWestChild = ComputeChild(CenterLat, North, West, CenterLon);

            if (northEastChild == null)
                northEastChild = ComputeChild(CenterLat, North, CenterLon, East);

            if (southWestChild == null)
                southWestChild = ComputeChild(South, CenterLat, West, CenterLon);

            if (southEastChild == null)
                southEastChild = ComputeChild(South, CenterLat, CenterLon, East);

        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public virtual void Dispose()
        {
            isInitialized = false;
            if (textures != null)
            {

                for (int i = 0; i < textures.Length; i++)
                {
                    if (textures[i] != null && !textures[i].Disposed)
                    {
                        textures[i].Dispose();
                        textures[i] = null;
                    }
                }
                textures = null;
            }
            if (northWestChild != null)
            {
                northWestChild.Dispose();
                northWestChild = null;
            }
            if (southWestChild != null)
            {
                southWestChild.Dispose();
                southWestChild = null;
            }
            if (northEastChild != null)
            {
                northEastChild.Dispose();
                northEastChild = null;
            }
            if (southEastChild != null)
            {
                southEastChild.Dispose();
                southEastChild = null;
            }

            if (DownloadRequest != null)
            {
                //TODO:20130909-禁用下载操作
                //QuadTileSet.RemoveFromDownloadQueue(DownloadRequest);
                //DownloadRequest.Dispose();
                DownloadRequest = null;
            }
        }

        /// <summary>
        /// Updates this layer (background)
        /// 后台更新当前层
        /// </summary>
        public virtual void Update(DrawArgs drawArgs)
        {

            if (m_isResetingCache)
                return;

            try
            {
                double tileSize = North - South;

                if (!isInitialized)
                {
                    if (DrawArgs.Camera.ViewRange * 0.5f < Angle.FromDegrees(QuadTileSet.TileDrawDistance * tileSize)
                        && MathEngine.SphericalDistance(CenterLatitude, CenterLongitude, DrawArgs.Camera.Latitude, DrawArgs.Camera.Longitude) < Angle.FromDegrees(QuadTileSet.TileDrawSpread * tileSize * 1.25f)
                        && DrawArgs.Camera.ViewFrustum.Intersects(BoundingBox))
                        Initialize();
                }

                if (isInitialized && World.Settings.VerticalExaggeration != verticalExaggeration || m_CurrentOpacity != QuadTileSet.Opacity ||
                    QuadTileSet.RenderStruts != renderStruts)
                {
                    CreateTileMesh();
                }

                if (isInitialized)
                {
                    if (DrawArgs.Camera.ViewRange < Angle.FromDegrees(QuadTileSet.TileDrawDistance * tileSize)
                        && MathEngine.SphericalDistance(CenterLatitude, CenterLongitude, DrawArgs.Camera.Latitude, DrawArgs.Camera.Longitude) < Angle.FromDegrees(QuadTileSet.TileDrawSpread * tileSize)
                        && DrawArgs.Camera.ViewFrustum.Intersects(BoundingBox))
                    {
                        if (northEastChild == null || northWestChild == null || southEastChild == null || southWestChild == null)
                        {
                            ComputeChildren();
                        }

                        if (northEastChild != null)
                        {
                            northEastChild.Update(drawArgs);
                        }

                        if (northWestChild != null)
                        {
                            northWestChild.Update(drawArgs);
                        }

                        if (southEastChild != null)
                        {
                            southEastChild.Update(drawArgs);
                        }

                        if (southWestChild != null)
                        {
                            southWestChild.Update(drawArgs);
                        }

                    }
                    else
                    {
                        if (northWestChild != null)
                        {
                            northWestChild.Dispose();
                            northWestChild = null;
                        }

                        if (northEastChild != null)
                        {
                            northEastChild.Dispose();
                            northEastChild = null;
                        }

                        if (southEastChild != null)
                        {
                            southEastChild.Dispose();
                            southEastChild = null;
                        }

                        if (southWestChild != null)
                        {
                            southWestChild.Dispose();
                            southWestChild = null;
                        }
                    }
                }

                if (isInitialized)
                {
                    if (DrawArgs.Camera.ViewRange / 2 > Angle.FromDegrees(QuadTileSet.TileDrawDistance * tileSize * 1.5f)
                            || MathEngine.SphericalDistance(CenterLatitude, CenterLongitude, DrawArgs.Camera.Latitude, DrawArgs.Camera.Longitude) > Angle.FromDegrees(QuadTileSet.TileDrawSpread * tileSize * 1.5f))
                    {
                        if (Level != 0 || (Level == 0 && !QuadTileSet.AlwaysRenderBaseTiles))
                            this.Dispose();
                    }
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Builds flat or terrain mesh for current tile
        /// 为当前瓦片构建平坦或地形纹理
        /// </summary>
        public virtual void CreateTileMesh()
        {
            verticalExaggeration = World.Settings.VerticalExaggeration;
            m_CurrentOpacity = QuadTileSet.Opacity;
            renderStruts = QuadTileSet.RenderStruts;

            if (QuadTileSet.TerrainMapped && Math.Abs(verticalExaggeration) > 1e-3)
                CreateElevatedMesh();
            else
                CreateFlatMesh();
        }

        // Edits by Patrick Murris : fixing mesh sides normals (2006-11-18)

        /// <summary>
        /// Builds a flat mesh (no terrain)
        /// 构建一个平坦的纹理(没有地形信息)
        /// </summary>
        protected virtual void CreateFlatMesh()
        {
            double layerRadius = (double)QuadTileSet.LayerRadius;
            double scaleFactor = 1.0 / (double)vertexCount;
            int thisVertexCount = vertexCount / 2 + (vertexCount % 2);
            int thisVertexCountPlus1 = thisVertexCount + 1;

            int totalVertexCount = thisVertexCountPlus1 * thisVertexCountPlus1;
            northWestVertices = new CustomVertex.PositionNormalTextured[totalVertexCount];
            southWestVertices = new CustomVertex.PositionNormalTextured[totalVertexCount];
            northEastVertices = new CustomVertex.PositionNormalTextured[totalVertexCount];
            southEastVertices = new CustomVertex.PositionNormalTextured[totalVertexCount];

            const double Degrees2Radians = System.Math.PI / 180.0;

            // Cache western sin/cos of longitude values
            double[] sinLon = new double[thisVertexCountPlus1];
            double[] cosLon = new double[thisVertexCountPlus1];

            int baseIndex;
            double angle = West * Degrees2Radians;
            double deltaAngle = scaleFactor * LongitudeSpan * Degrees2Radians;

            for (int i = 0; i < thisVertexCountPlus1; i++)
            {
                angle = West * Degrees2Radians + i * deltaAngle;
                sinLon[i] = Math.Sin(angle);
                cosLon[i] = Math.Cos(angle);
                //angle += deltaAngle;

            }

            baseIndex = 0;
            angle = North * Degrees2Radians;
            deltaAngle = -scaleFactor * LatitudeSpan * Degrees2Radians;
            for (int i = 0; i < thisVertexCountPlus1; i++)
            {
                angle = North * Degrees2Radians + i * deltaAngle;
                double sinLat = Math.Sin(angle);
                double radCosLat = Math.Cos(angle) * layerRadius;

                for (int j = 0; j < thisVertexCountPlus1; j++)
                {
                    northWestVertices[baseIndex].X = (float)(radCosLat * cosLon[j] - localOrigin.X);
                    northWestVertices[baseIndex].Y = (float)(radCosLat * sinLon[j] - localOrigin.Y);
                    northWestVertices[baseIndex].Z = (float)(layerRadius * sinLat - localOrigin.Z);
                    northWestVertices[baseIndex].Tu = (float)(j * scaleFactor);
                    northWestVertices[baseIndex].Tv = (float)(i * scaleFactor);
                    northWestVertices[baseIndex].Normal = new Vector3(northWestVertices[baseIndex].X + (float)localOrigin.X, northWestVertices[baseIndex].Y + (float)localOrigin.Y, northWestVertices[baseIndex].Z + (float)localOrigin.Z);
                    northWestVertices[baseIndex].Normal.Normalize();

                    baseIndex += 1;
                }
                //angle += deltaAngle;
            }

            baseIndex = 0;
            angle = 0.5 * (North + South) * Degrees2Radians;
            for (int i = 0; i < thisVertexCountPlus1; i++)
            {
                angle = 0.5 * (North + South) * Degrees2Radians + i * deltaAngle;
                double sinLat = Math.Sin(angle);
                double radCosLat = Math.Cos(angle) * layerRadius;

                for (int j = 0; j < thisVertexCountPlus1; j++)
                {
                    southWestVertices[baseIndex].X = (float)(radCosLat * cosLon[j] - localOrigin.X);
                    southWestVertices[baseIndex].Y = (float)(radCosLat * sinLon[j] - localOrigin.Y);
                    southWestVertices[baseIndex].Z = (float)(layerRadius * sinLat - localOrigin.Z);
                    southWestVertices[baseIndex].Tu = (float)(j * scaleFactor);
                    southWestVertices[baseIndex].Tv = (float)((i + thisVertexCount) * scaleFactor);
                    southWestVertices[baseIndex].Normal = new Vector3(southWestVertices[baseIndex].X + (float)localOrigin.X, southWestVertices[baseIndex].Y + (float)localOrigin.Y, southWestVertices[baseIndex].Z + (float)localOrigin.Z);
                    southWestVertices[baseIndex].Normal.Normalize();

                    baseIndex += 1;
                }
                //angle += deltaAngle;
            }

            // Cache eastern sin/cos of longitude values
            angle = 0.5 * (West + East) * Degrees2Radians;
            deltaAngle = scaleFactor * LongitudeSpan * Degrees2Radians;
            for (int i = 0; i < thisVertexCountPlus1; i++)
            {
                angle = 0.5 * (West + East) * Degrees2Radians + i * deltaAngle;
                sinLon[i] = Math.Sin(angle);
                cosLon[i] = Math.Cos(angle);
                //angle += deltaAngle;
            }

            baseIndex = 0;
            angle = North * Degrees2Radians;
            deltaAngle = -scaleFactor * LatitudeSpan * Degrees2Radians;
            for (int i = 0; i < thisVertexCountPlus1; i++)
            {
                angle = North * Degrees2Radians + i * deltaAngle;
                double sinLat = Math.Sin(angle);
                double radCosLat = Math.Cos(angle) * layerRadius;

                for (int j = 0; j < thisVertexCountPlus1; j++)
                {
                    northEastVertices[baseIndex].X = (float)(radCosLat * cosLon[j] - localOrigin.X);
                    northEastVertices[baseIndex].Y = (float)(radCosLat * sinLon[j] - localOrigin.Y);
                    northEastVertices[baseIndex].Z = (float)(layerRadius * sinLat - localOrigin.Z);
                    northEastVertices[baseIndex].Tu = (float)((j + thisVertexCount) * scaleFactor);
                    northEastVertices[baseIndex].Tv = (float)(i * scaleFactor);
                    northEastVertices[baseIndex].Normal = new Vector3(northEastVertices[baseIndex].X + (float)localOrigin.X, northEastVertices[baseIndex].Y + (float)localOrigin.Y, northEastVertices[baseIndex].Z + (float)localOrigin.Z);
                    northEastVertices[baseIndex].Normal.Normalize();

                    baseIndex += 1;
                }
                //angle += deltaAngle;
            }

            baseIndex = 0;
            angle = 0.5f * (North + South) * Degrees2Radians;
            for (int i = 0; i < thisVertexCountPlus1; i++)
            {

                angle = 0.5 * (North + South) * Degrees2Radians + i * deltaAngle;
                double sinLat = Math.Sin(angle);
                double radCosLat = Math.Cos(angle) * layerRadius;

                for (int j = 0; j < thisVertexCountPlus1; j++)
                {
                    southEastVertices[baseIndex].X = (float)(radCosLat * cosLon[j] - localOrigin.X);
                    southEastVertices[baseIndex].Y = (float)(radCosLat * sinLon[j] - localOrigin.Y);
                    southEastVertices[baseIndex].Z = (float)(layerRadius * sinLat - localOrigin.Z);
                    southEastVertices[baseIndex].Tu = (float)((j + thisVertexCount) * scaleFactor);
                    southEastVertices[baseIndex].Tv = (float)((i + thisVertexCount) * scaleFactor);
                    southEastVertices[baseIndex].Normal = new Vector3(southEastVertices[baseIndex].X + (float)localOrigin.X, southEastVertices[baseIndex].Y + (float)localOrigin.Y, southEastVertices[baseIndex].Z + (float)localOrigin.Z);
                    southEastVertices[baseIndex].Normal.Normalize();

                    baseIndex += 1;
                }
                //angle += deltaAngle;
            }

            vertexIndexes = new short[2 * thisVertexCount * thisVertexCount * 3];

            for (int i = 0; i < thisVertexCount; i++)
            {
                baseIndex = (2 * 3 * i * thisVertexCount);

                for (int j = 0; j < thisVertexCount; j++)
                {
                    vertexIndexes[baseIndex] = (short)(i * thisVertexCountPlus1 + j);
                    vertexIndexes[baseIndex + 1] = (short)((i + 1) * thisVertexCountPlus1 + j);
                    vertexIndexes[baseIndex + 2] = (short)(i * thisVertexCountPlus1 + j + 1);

                    vertexIndexes[baseIndex + 3] = (short)(i * thisVertexCountPlus1 + j + 1);
                    vertexIndexes[baseIndex + 4] = (short)((i + 1) * thisVertexCountPlus1 + j);
                    vertexIndexes[baseIndex + 5] = (short)((i + 1) * thisVertexCountPlus1 + j + 1);

                    baseIndex += 6;
                }
            }
        }


        /// <summary>
        /// Build the elevated terrain mesh
        /// 构建抬起的地形纹理
        /// </summary>
        protected virtual void CreateElevatedMesh()
        {
            isDownloadingTerrain = true;
            // Get height data with one extra sample around the tile
            double degreePerSample = LatitudeSpan / vertexCountElevated;
            TerrainTile tile = QuadTileSet.World.TerrainAccessor.GetElevationArray(North + degreePerSample, South - degreePerSample, West - degreePerSample, East + degreePerSample, vertexCountElevated + 3);
            float[,] heightData = tile.ElevationData;

            int vertexCountElevatedPlus3 = vertexCountElevated / 2 + 3;
            int totalVertexCount = vertexCountElevatedPlus3 * vertexCountElevatedPlus3;
            northWestVertices = new CustomVertex.PositionNormalTextured[totalVertexCount];
            southWestVertices = new CustomVertex.PositionNormalTextured[totalVertexCount];
            northEastVertices = new CustomVertex.PositionNormalTextured[totalVertexCount];
            southEastVertices = new CustomVertex.PositionNormalTextured[totalVertexCount];
            double layerRadius = (double)QuadTileSet.LayerRadius;

            // Calculate mesh base radius (bottom vertices)
            // Find minimum elevation to account for possible bathymetry
            float minimumElevation = float.MaxValue;
            float maximumElevation = float.MinValue;
            foreach (float height in heightData)
            {
                if (height < minimumElevation)
                    minimumElevation = height;
                if (height > maximumElevation)
                    maximumElevation = height;
            }
            minimumElevation *= verticalExaggeration;
            maximumElevation *= verticalExaggeration;

            if (minimumElevation > maximumElevation)
            {
                // Compensate for negative vertical exaggeration
                minimumElevation = maximumElevation;
                maximumElevation = minimumElevation;
            }

            double overlap = 500 * verticalExaggeration; // 500m high tiles

            // Radius of mesh bottom grid
            meshBaseRadius = layerRadius + minimumElevation - overlap;

            CreateElevatedMesh(ChildLocation.NorthWest, northWestVertices, meshBaseRadius, heightData);
            CreateElevatedMesh(ChildLocation.SouthWest, southWestVertices, meshBaseRadius, heightData);
            CreateElevatedMesh(ChildLocation.NorthEast, northEastVertices, meshBaseRadius, heightData);
            CreateElevatedMesh(ChildLocation.SouthEast, southEastVertices, meshBaseRadius, heightData);

            //JOKI 140912 3km下不显示bug
            //BoundingBox = new BoundingBox((float)South, (float)North, (float)West, (float)East,
            //    (float)layerRadius, (float)layerRadius + 10000 * this.verticalExaggeration);

            QuadTileSet.IsDownloadingElevation = false;

            // Build common set of indexes for the 4 child meshes	
            int vertexCountElevatedPlus2 = vertexCountElevated / 2 + 2;
            vertexIndexes = new short[2 * vertexCountElevatedPlus2 * vertexCountElevatedPlus2 * 3];

            int elevated_idx = 0;
            for (int i = 0; i < vertexCountElevatedPlus2; i++)
            {
                for (int j = 0; j < vertexCountElevatedPlus2; j++)
                {
                    vertexIndexes[elevated_idx++] = (short)(i * vertexCountElevatedPlus3 + j);
                    vertexIndexes[elevated_idx++] = (short)((i + 1) * vertexCountElevatedPlus3 + j);
                    vertexIndexes[elevated_idx++] = (short)(i * vertexCountElevatedPlus3 + j + 1);

                    vertexIndexes[elevated_idx++] = (short)(i * vertexCountElevatedPlus3 + j + 1);
                    vertexIndexes[elevated_idx++] = (short)((i + 1) * vertexCountElevatedPlus3 + j);
                    vertexIndexes[elevated_idx++] = (short)((i + 1) * vertexCountElevatedPlus3 + j + 1);
                }
            }

            calculate_normals(ref northWestVertices, vertexIndexes);
            calculate_normals(ref southWestVertices, vertexIndexes);
            calculate_normals(ref northEastVertices, vertexIndexes);
            calculate_normals(ref southEastVertices, vertexIndexes);

            isDownloadingTerrain = false;
        }


        /// <summary>
        /// Create child tile terrain mesh
        /// Build the mesh with one extra vertice all around for proper normals calculations later on.
        /// Use the struts vertices to that effect. Struts are properly folded after normals calculations.
        /// 创建子瓦片地形纹理
        /// </summary>
        protected void CreateElevatedMesh(ChildLocation corner, CustomVertex.PositionNormalTextured[] vertices,
            double meshBaseRadius, float[,] heightData)
        {
            // Figure out child lat/lon boundaries (radians)
            double north = MathEngine.DegreesToRadians(North);
            double west = MathEngine.DegreesToRadians(West);

            // Texture coordinate offsets
            float TuOffset = 0;
            float TvOffset = 0;

            switch (corner)
            {
                case ChildLocation.NorthWest:
                    // defaults are all good
                    break;
                case ChildLocation.NorthEast:
                    west = MathEngine.DegreesToRadians(0.5 * (West + East));
                    TuOffset = 0.5f;
                    break;
                case ChildLocation.SouthWest:
                    north = MathEngine.DegreesToRadians(0.5 * (North + South));
                    TvOffset = 0.5f;
                    break;
                case ChildLocation.SouthEast:
                    north = MathEngine.DegreesToRadians(0.5 * (North + South));
                    west = MathEngine.DegreesToRadians(0.5 * (West + East));
                    TuOffset = 0.5f;
                    TvOffset = 0.5f;
                    break;
            }

            double latitudeRadianSpan = MathEngine.DegreesToRadians(LatitudeSpan);
            double longitudeRadianSpan = MathEngine.DegreesToRadians(LongitudeSpan);

            double layerRadius = (double)QuadTileSet.LayerRadius;
            double scaleFactor = 1.0 / vertexCountElevated;
            int terrainLongitudeIndex = (int)(TuOffset * vertexCountElevated) + 1;
            int terrainLatitudeIndex = (int)(TvOffset * vertexCountElevated) + 1;

            int vertexCountElevatedPlus1 = vertexCountElevated / 2 + 1;

            double radius = 0;
            int vertexIndex = 0;
            for (int latitudeIndex = -1; latitudeIndex <= vertexCountElevatedPlus1; latitudeIndex++)
            {
                double latitudeFactor = latitudeIndex * scaleFactor;
                double latitude = north - latitudeFactor * latitudeRadianSpan;

                // Cache trigonometric values
                double cosLat = Math.Cos(latitude);
                double sinLat = Math.Sin(latitude);

                for (int longitudeIndex = -1; longitudeIndex <= vertexCountElevatedPlus1; longitudeIndex++)
                {

                    // Top of mesh for all (real terrain + struts)
                    radius = layerRadius +
                         heightData[terrainLatitudeIndex + latitudeIndex, terrainLongitudeIndex + longitudeIndex]
                         * verticalExaggeration;

                    double longitudeFactor = longitudeIndex * scaleFactor;

                    // Texture coordinates
                    vertices[vertexIndex].Tu = TuOffset + (float)longitudeFactor;
                    vertices[vertexIndex].Tv = TvOffset + (float)latitudeFactor;

                    // Convert from spherical (radians) to cartesian
                    double longitude = west + longitudeFactor * longitudeRadianSpan;
                    double radCosLat = radius * cosLat;
                    vertices[vertexIndex].X = (float)(radCosLat * Math.Cos(longitude) - localOrigin.X);
                    vertices[vertexIndex].Y = (float)(radCosLat * Math.Sin(longitude) - localOrigin.Y);
                    vertices[vertexIndex].Z = (float)(radius * sinLat - localOrigin.Z);

                    vertexIndex++;
                }
            }
        }

        /// <summary>
        /// 计算归一化
        /// </summary>
        /// <param name="vertices"></param>
        /// <param name="indices"></param>
        private void calculate_normals(ref CustomVertex.PositionNormalTextured[] vertices, short[] indices)
        {
            System.Collections.ArrayList[] normal_buffer = new System.Collections.ArrayList[vertices.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                normal_buffer[i] = new System.Collections.ArrayList();
            }
            for (int i = 0; i < indices.Length; i += 3)
            {
                Vector3 p1 = vertices[indices[i + 0]].Position;
                Vector3 p2 = vertices[indices[i + 1]].Position;
                Vector3 p3 = vertices[indices[i + 2]].Position;

                Vector3 v1 = p2 - p1;
                Vector3 v2 = p3 - p1;
                Vector3 normal = Vector3.Cross(v1, v2);

                normal.Normalize();

                // Store the face's normal for each of the vertices that make up the face.
                normal_buffer[indices[i + 0]].Add(normal);
                normal_buffer[indices[i + 1]].Add(normal);
                normal_buffer[indices[i + 2]].Add(normal);
            }

            // Now loop through each vertex vector, and avarage out all the normals stored.
            for (int i = 0; i < vertices.Length; ++i)
            {
                for (int j = 0; j < normal_buffer[i].Count; ++j)
                {
                    Vector3 curNormal = (Vector3)normal_buffer[i][j];

                    if (vertices[i].Normal == Vector3.Empty)
                        vertices[i].Normal = curNormal;
                    else
                        vertices[i].Normal += curNormal;
                }

                vertices[i].Normal.Multiply(1.0f / normal_buffer[i].Count);
            }

            // Adjust/Fold struts vertices using terrain border vertices positions
            short vertexDensity = (short)Math.Sqrt(vertices.Length);
            for (int i = 0; i < vertexDensity; i++)
            {
                if (i == 0 || i == vertexDensity - 1)
                {
                    for (int j = 0; j < vertexDensity; j++)
                    {
                        int offset = (i == 0) ? vertexDensity : -vertexDensity;
                        if (j == 0) offset++;
                        if (j == vertexDensity - 1) offset--;
                        Point3d p = new Point3d(vertices[i * vertexDensity + j + offset].Position.X, vertices[i * vertexDensity + j + offset].Position.Y, vertices[i * vertexDensity + j + offset].Position.Z);
                        if (renderStruts && m_CurrentOpacity == 255) p = ProjectOnMeshBase(p);
                        vertices[i * vertexDensity + j].Position = new Vector3((float)p.X, (float)p.Y, (float)p.Z);
                    }
                }
                else
                {
                    Point3d p = new Point3d(vertices[i * vertexDensity + 1].Position.X, vertices[i * vertexDensity + 1].Position.Y, vertices[i * vertexDensity + 1].Position.Z);
                    if (renderStruts && m_CurrentOpacity == 255) p = ProjectOnMeshBase(p);
                    vertices[i * vertexDensity].Position = new Vector3((float)p.X, (float)p.Y, (float)p.Z);

                    p = new Point3d(vertices[i * vertexDensity + vertexDensity - 2].Position.X, vertices[i * vertexDensity + vertexDensity - 2].Position.Y, vertices[i * vertexDensity + vertexDensity - 2].Position.Z);
                    if (renderStruts && m_CurrentOpacity == 255) p = ProjectOnMeshBase(p);
                    vertices[i * vertexDensity + vertexDensity - 1].Position = new Vector3((float)p.X, (float)p.Y, (float)p.Z);
                }
            }
        }

        /// <summary>
        /// Project an elevated mesh point to the mesh base
        /// 将一个抬起的纹理点投影到基础纹理上
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private Point3d ProjectOnMeshBase(Point3d p)
        {
            p = p + this.localOrigin;
            p = p.normalize();
            p = p * meshBaseRadius - this.localOrigin;
            return p;
        }

        /// <summary>
        /// 迭代遍历各层级瓦片，建立需要Rander的瓦片，过滤掉四叉子瓦片已经完全覆盖区域的瓦片
        /// </summary>
        /// <param name="iscomplete"></param>
        /// <returns></returns>
        public List<QuadTile> GetRanderableChild(out bool iscomplete)
        {
            List<QuadTile> rc = new List<QuadTile>();
            iscomplete = false;

            //当前瓦片是否初始化，是否具有四角坐标
            if (!isInitialized ||
                this.northWestVertices == null ||
                this.northEastVertices == null ||
                this.southEastVertices == null ||
                this.southWestVertices == null)
                return rc;
            //先渲染子瓦片,标记四叉子瓦片是否都能渲染成功
            bool northWestChildRendered = false;
            bool southWestChildRendered = false;
            bool northEastChildRendered = false;
            bool southEastChildRendered = false;

            //过滤不在区域内的瓦片，通过已经渲染成功的标识过滤
            if (northWestChild != null && !DrawArgs.Camera.ViewFrustum.Intersects(northWestChild.BoundingBox))
                northWestChildRendered = true;
            if (southWestChild != null && !DrawArgs.Camera.ViewFrustum.Intersects(southWestChild.BoundingBox))
                southWestChildRendered = true;
            if (northEastChild != null && !DrawArgs.Camera.ViewFrustum.Intersects(northEastChild.BoundingBox))
                northEastChildRendered = true;
            if (southEastChild != null && !DrawArgs.Camera.ViewFrustum.Intersects(southEastChild.BoundingBox))
                southEastChildRendered = true;

            if (!northWestChildRendered && northWestChild != null)
                rc.AddRange(northWestChild.GetRanderableChild(out northWestChildRendered).ToArray());

            if (!southWestChildRendered && southWestChild != null)
                rc.AddRange(southWestChild.GetRanderableChild(out southWestChildRendered).ToArray());

            if (!northEastChildRendered && northEastChild != null)
                rc.AddRange(northEastChild.GetRanderableChild(out northEastChildRendered).ToArray());

            if (!southEastChildRendered && southEastChild != null)
                rc.AddRange(southEastChild.GetRanderableChild(out southEastChildRendered).ToArray());

            //如果四叉子瓦片都能渲染成功，说明当前瓦片不需要渲染显示，有下一级更高精度的瓦片显示
            if (!DrawArgs.Camera.ViewFrustum.Intersects(BoundingBox) || (northWestChildRendered && northEastChildRendered && southWestChildRendered && southEastChildRendered))
            {
                iscomplete = true;
            }
            else if (File.Exists(ImageFilePath) && DrawArgs.Camera.ViewFrustum.Intersects(BoundingBox))
            {
                //如果四叉子瓦片不能全部渲染成功，且当前瓦片有数据可渲染，则需要渲染当前瓦片
                //当前瓦片是否和视图相交，不相交不显示
                rc.Add(this);
                iscomplete = true;
            }

            return rc;
        }

        /// <summary>
        /// 渲染当前瓦片
        /// </summary>
        /// <param name="drawArgs"></param>
        /// <returns></returns>
        public virtual bool Render(DrawArgs drawArgs)
        {
            try
            {
                //否则开始渲染当前瓦片
                //首先判断是否存在瓦片文件
                if (!File.Exists(ImageFilePath))
                    return false;       //当子瓦片不存在时，如果为true就是不渲染上一层瓦片，效果为黑洞，如果为false就渲染上一层级瓦片，用低分辨率补窟窿
                
                //是否渲染显示瓦片的边框和瓦片数据文件名（用四叉子瓦片都渲染失败来表示当前瓦片是显示的瓦片）
                if (QuadTileSet.RenderFileNames)
                {
                    Vector3 referenceCenter = new Vector3(
                        (float)drawArgs.WorldCamera.ReferenceCenter.X,
                        (float)drawArgs.WorldCamera.ReferenceCenter.Y,
                        (float)drawArgs.WorldCamera.ReferenceCenter.Z);

                    RenderDownloadRectangle(drawArgs, System.Drawing.Color.FromArgb(255, 0, 0).ToArgb(), referenceCenter);

                    Vector3 cartesianPoint = MathEngine.SphericalToCartesian(
                        CenterLatitude.Degrees,
                        CenterLongitude.Degrees,
                        drawArgs.WorldCamera.WorldRadius + drawArgs.WorldCamera.TerrainElevation);

                    if (ImageFilePath != null && drawArgs.WorldCamera.ViewFrustum.ContainsPoint(cartesianPoint))
                    {
                        Vector3 projectedPoint = drawArgs.WorldCamera.Project(cartesianPoint - referenceCenter);

                        System.Drawing.Rectangle rect = new System.Drawing.Rectangle(
                            (int)projectedPoint.X - 100,
                            (int)projectedPoint.Y,
                            200,
                            200);

                        drawArgs.DefaultDrawingFont.DrawText(
                            null,
                            ImageFilePath,
                            rect,
                            DrawTextFormat.WordBreak,
                            System.Drawing.Color.Red);
                    }
                }

                Device device = DrawArgs.Device;

                for (int i = 0; i < textures.Length; i++)
                {
                    if (textures[i] == null || textures[i].Disposed)
                        return false;

                    device.SetTexture(i, textures[i]);
                }

                drawArgs.numberTilesDrawn++;

                int numpasses = 1;
                int pass;

                DrawArgs.Device.Transform.World = Matrix.Translation(
                    (float)(localOrigin.X - drawArgs.WorldCamera.ReferenceCenter.X),
                    (float)(localOrigin.Y - drawArgs.WorldCamera.ReferenceCenter.Y),
                    (float)(localOrigin.Z - drawArgs.WorldCamera.ReferenceCenter.Z)
                    );

                for (pass = 0; pass < numpasses; pass++)
                {
                    Render(device, northWestVertices, northWestChild);
                    Render(device, southWestVertices, southWestChild);
                    Render(device, northEastVertices, northEastChild);
                    Render(device, southEastVertices, southEastChild);
                }

                DrawArgs.Device.Transform.World = DrawArgs.Camera.WorldMatrix;

                return true;
            }
            catch (DirectXException)
            {

            }
            return false;
        }

        /// <summary>
        /// Render one of the 4 quadrants with optional download indicator
        /// 渲染四个中的一个，有选择的下载
        /// </summary>
        private void Render(Device device, CustomVertex.PositionNormalTextured[] verts, QuadTile child)
        {
            bool isMultitexturing = false;

            if (!World.Settings.EnableSunShading)
            {
                if (World.Settings.ShowDownloadIndicator && child != null)
                {
                    // Check/display download activity
                    GeoSpatialDownloadRequest request = child.DownloadRequest;

                    if (child.isDownloadingTerrain)
                    {
                        device.SetTexture(1, QuadTileSet.DownloadTerrainTexture);
                        isMultitexturing = true;
                    }
                    //else if (request != null)
                    else if (child.IsWaitingForDownload)
                    {
                        if (child.IsDownloadingImage)
                            device.SetTexture(1, QuadTileSet.DownloadInProgressTexture);
                        else
                            device.SetTexture(1, QuadTileSet.DownloadQueuedTexture);
                        isMultitexturing = true;
                    }
                }
            }

            if (isMultitexturing)
                device.SetTextureStageState(1, TextureStageStates.ColorOperation, (int)TextureOperation.BlendTextureAlpha);

            if (verts != null && vertexIndexes != null)
            {
                if (QuadTileSet.Effect != null)
                {
                    Effect effect = QuadTileSet.Effect;

                    // FIXME: just use the first technique for now
                    effect.Technique = effect.GetTechnique(0);
                    effect.SetValue("WorldViewProj", Matrix.Multiply(device.Transform.World, Matrix.Multiply(device.Transform.View, device.Transform.Projection)));
                    try
                    {
                        effect.SetValue("Tex0", textures[0]);
                        effect.SetValue("Tex1", textures[1]);
                        effect.SetValue("Brightness", QuadTileSet.GrayscaleBrightness);
                        float opacity = (float)QuadTileSet.Opacity / 255.0f;
                        effect.SetValue("Opacity", opacity);
                    }
                    catch
                    {
                    }

                    int numPasses = effect.Begin(0);
                    for (int i = 0; i < numPasses; i++)
                    {
                        effect.BeginPass(i);
                        device.DrawIndexedUserPrimitives(PrimitiveType.TriangleList, 0,
                            verts.Length, vertexIndexes.Length / 3, vertexIndexes, true, verts);

                        effect.EndPass();
                    }

                    effect.End();
                }
                else if (!QuadTileSet.RenderGrayscale || (device.DeviceCaps.PixelShaderVersion.Major < 1))
                {
                    if (World.Settings.EnableSunShading)
                    {
                        Point3d sunPosition = SunCalculator.GetGeocentricPosition(TimeKeeper.CurrentTimeUtc);
                        Vector3 sunVector = new Vector3(
                            (float)sunPosition.X,
                            (float)sunPosition.Y,
                            (float)sunPosition.Z);

                        device.RenderState.Lighting = true;
                        Material material = new Material();
                        material.Diffuse = System.Drawing.Color.White;
                        material.Ambient = System.Drawing.Color.White;

                        device.Material = material;
                        device.RenderState.AmbientColor = World.Settings.ShadingAmbientColor.ToArgb();
                        device.RenderState.NormalizeNormals = true;
                        device.RenderState.AlphaBlendEnable = true;

                        device.Lights[0].Enabled = true;
                        device.Lights[0].Type = LightType.Directional;
                        device.Lights[0].Diffuse = System.Drawing.Color.White;
                        device.Lights[0].Direction = sunVector;

                        device.TextureState[0].ColorOperation = TextureOperation.Modulate;
                        device.TextureState[0].ColorArgument1 = TextureArgument.Diffuse;
                        device.TextureState[0].ColorArgument2 = TextureArgument.TextureColor;
                    }
                    else
                    {
                        device.RenderState.Lighting = false;
                        device.RenderState.Ambient = World.Settings.StandardAmbientColor;
                    }

                    device.RenderState.TextureFactor = System.Drawing.Color.FromArgb(m_CurrentOpacity, 0, 0, 0).ToArgb();
                    device.TextureState[0].AlphaOperation = TextureOperation.BlendFactorAlpha;
                    device.TextureState[0].AlphaArgument1 = TextureArgument.TextureColor;
                    device.TextureState[0].AlphaArgument2 = TextureArgument.TFactor;

                    device.DrawIndexedUserPrimitives(PrimitiveType.TriangleList, 0,
                        verts.Length, vertexIndexes.Length / 3, vertexIndexes, true, verts);
                }
                else
                {
                    if (grayscaleEffect == null)
                    {
                        device.DeviceReset += new EventHandler(device_DeviceReset);
                        device_DeviceReset(device, null);
                    }

                    grayscaleEffect.Technique = "RenderGrayscaleBrightness";
                    grayscaleEffect.SetValue("WorldViewProj", Matrix.Multiply(device.Transform.World, Matrix.Multiply(device.Transform.View, device.Transform.Projection)));
                    grayscaleEffect.SetValue("Tex0", textures[0]);
                    grayscaleEffect.SetValue("Brightness", QuadTileSet.GrayscaleBrightness);
                    float opacity = (float)QuadTileSet.Opacity / 255.0f;
                    grayscaleEffect.SetValue("Opacity", opacity);

                    int numPasses = grayscaleEffect.Begin(0);
                    for (int i = 0; i < numPasses; i++)
                    {
                        grayscaleEffect.BeginPass(i);
                        device.DrawIndexedUserPrimitives(PrimitiveType.TriangleList, 0,
                            verts.Length, vertexIndexes.Length / 3, vertexIndexes, true, verts);

                        grayscaleEffect.EndPass();
                    }

                    grayscaleEffect.End();
                }
            }
            if (isMultitexturing)
                device.SetTextureStageState(1, TextureStageStates.ColorOperation, (int)TextureOperation.Disable);
        }

        /// <summary>
        /// Render a rectangle around an image tile in the specified color
        /// 采用指定的颜色在影像瓦片外渲染一个矩形框
        /// </summary>
        public void RenderDownloadRectangle(DrawArgs drawArgs, int color, Vector3 referenceCenter)
        {
            // Render terrain download rectangle
            Vector3 northWestV = MathEngine.SphericalToCartesian((float)North, (float)West, QuadTileSet.LayerRadius) - referenceCenter;
            Vector3 southWestV = MathEngine.SphericalToCartesian((float)South, (float)West, QuadTileSet.LayerRadius) - referenceCenter;
            Vector3 northEastV = MathEngine.SphericalToCartesian((float)North, (float)East, QuadTileSet.LayerRadius) - referenceCenter;
            Vector3 southEastV = MathEngine.SphericalToCartesian((float)South, (float)East, QuadTileSet.LayerRadius) - referenceCenter;

            downloadRectangle[0].X = northWestV.X;
            downloadRectangle[0].Y = northWestV.Y;
            downloadRectangle[0].Z = northWestV.Z;
            downloadRectangle[0].Color = color;

            downloadRectangle[1].X = southWestV.X;
            downloadRectangle[1].Y = southWestV.Y;
            downloadRectangle[1].Z = southWestV.Z;
            downloadRectangle[1].Color = color;

            downloadRectangle[2].X = southEastV.X;
            downloadRectangle[2].Y = southEastV.Y;
            downloadRectangle[2].Z = southEastV.Z;
            downloadRectangle[2].Color = color;

            downloadRectangle[3].X = northEastV.X;
            downloadRectangle[3].Y = northEastV.Y;
            downloadRectangle[3].Z = northEastV.Z;
            downloadRectangle[3].Color = color;

            downloadRectangle[4].X = downloadRectangle[0].X;
            downloadRectangle[4].Y = downloadRectangle[0].Y;
            downloadRectangle[4].Z = downloadRectangle[0].Z;
            downloadRectangle[4].Color = color;

            drawArgs.device.RenderState.ZBufferEnable = false;
            drawArgs.device.VertexFormat = CustomVertex.PositionColored.Format;
            drawArgs.device.TextureState[0].ColorOperation = TextureOperation.Disable;
            drawArgs.device.DrawUserPrimitives(PrimitiveType.LineStrip, 4, downloadRectangle);
            drawArgs.device.TextureState[0].ColorOperation = TextureOperation.SelectArg1;
            drawArgs.device.VertexFormat = CustomVertex.PositionNormalTextured.Format;
            drawArgs.device.RenderState.ZBufferEnable = true;
        }

        /// <summary>
        /// 重置设备
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void device_DeviceReset(object sender, EventArgs e)
        {
            Device device = (Device)sender;
            string outerrors = "";

            try
            {
                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                Stream stream = assembly.GetManifestResourceStream("QRST.WorldGlobeTool.Shaders.grayscale.fx");

                grayscaleEffect =
                    Effect.FromStream(
                    device,
                    stream,
                    null,
                    null,
                    ShaderFlags.None,
                    null,
                    out outerrors);

                if (outerrors != null && outerrors.Length > 0)
                    Log.Write(Log.Levels.Error, outerrors);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
        }
    }
}
