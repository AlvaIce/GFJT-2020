namespace QRST.WorldGlobeTool.Net.WMS
{
    /// <summary>
    /// 密封类，WMS描述
    /// </summary>
    public sealed class WmsDescriptor
    {
        /// <summary>
        /// WMS服务的URI地址
        /// </summary>
        private System.Uri uri;
        /// <summary>
        /// 不透明度
        /// </summary>
        private double opacity;

        public WmsDescriptor() { }

        public WmsDescriptor(System.Uri url, double opacity)
        {
            this.uri = url;
            this.opacity = opacity;
        }

        /// <summary>
        /// 获取或设置WMS服务的URI地址
        /// </summary>
        public System.Uri Url
        {
            get { return this.uri; }
            set { this.uri = value; }
        }

        /// <summary>
        /// 获取或设置不透明度
        /// </summary>
        public double Opacity
        {
            get { return this.opacity; }
            set { this.opacity = value; }
        }
    }
}
