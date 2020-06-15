namespace QRST.WorldGlobeTool
{
    /// <summary>
    /// 鼠标样式
    /// </summary>
    public enum CursorType
    {
        /// <summary>
        /// 箭头光标
        /// </summary>
        Arrow = 0,
        /// <summary>
        /// 手型光标
        /// </summary>
        Hand,
        /// <summary>
        /// 十字线光标
        /// </summary>
        Cross,
        /// <summary>
        /// 测量光标
        /// </summary>
        Measure,
        /// <summary>
        /// 四向大小调整光标，该光标由相联接的、分别指向东南西北的四个箭头组成
        /// </summary>
        SizeAll,
        /// <summary>
        /// 双向水平（西/东）大小调整光标
        /// </summary>
        SizeWE,
        /// <summary>
        /// 双向垂直（北/南）大小调整光标
        /// </summary>
        SizeNS,
        /// <summary>
        /// 双向对角线（东北/西南）大小调整光标
        /// </summary>
        SizeNESW,
        /// <summary>
        /// 双向对角线（西北/东南）大小调整光标
        /// </summary>
        SizeNWSE
    }

    /// <summary>
    /// 图层绘制优先级
    /// </summary>
    public enum RenderPriority
    {
        /// <summary>
        /// 表面
        /// </summary>
        SurfaceImages = 0,
        /// <summary>
        /// 地形图
        /// </summary>
        TerrainMappedImages = 100,
        /// <summary>
        /// 大气图片
        /// </summary>
        AtmosphericImages = 200,
        /// <summary>
        /// 线路
        /// </summary>
        LinePaths = 300,
        /// <summary>
        /// 图标
        /// </summary>
        Icons = 400,
        /// <summary>
        /// 地名
        /// </summary>
        Placenames = 500,
        /// <summary>
        /// 自定义
        /// </summary>
        Custom = 600,
        /// <summary>
        /// 控制点
        /// </summary>
        GCPs = 700
    }

    /// <summary>
    /// 下载方式
    /// </summary>
    public enum DownloadType
    {
        Unspecified,
        Wms
    }

    /// <summary>
    /// 控件展示类型
    /// </summary>
    public enum ShowType
    {
        Image3D,
        Map3D,
        Image2D,
        Map2D
    }

    /// <summary>
    /// 矢量类型
    /// </summary>
    public enum ShapeFeatureType
    {
        /// <summary>
        /// 自定义或没有特别指定
        /// </summary>
        Unspecified = 0,

        /// <summary>
        /// 点类型矢量
        /// </summary>
        Point = 1,

        /// <summary>
        /// 线类型矢量
        /// </summary>
        Line = 2,

        /// <summary>
        /// 多边形矢量
        /// </summary>
        Polygon = 3,

        /// <summary>
        /// 多点矢量
        /// </summary>
        MultiPoint = 4
    }
}
