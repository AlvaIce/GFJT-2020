using System;
using System.Collections;
using Microsoft.DirectX.Direct3D;
using QRST.WorldGlobeTool.Net;
using QRST.WorldGlobeTool.Camera;
using QRST.WorldGlobeTool.Utility;

namespace QRST.WorldGlobeTool
{
    /// <summary>
    /// 球渲染参数对象
    /// </summary>
    public class DrawArgs : IDisposable
    {
        /// <summary>
        /// D3D设备对象
        /// </summary>
        public Device device;

        /// <summary>
        /// 父窗体对象
        /// </summary>
        public System.Windows.Forms.Control parentControl;
        /// <summary>
        /// 静态父窗体
        /// </summary>
        public static System.Windows.Forms.Control ParentControl = null;
        /// <summary>
        /// 边界点数
        /// </summary>
        public int numBoundaryPointsTotal;
        /// <summary>
        /// 要被渲染的边界点数
        /// </summary>
        public int numBoundaryPointsRendered;
        /// <summary>
        /// 要被渲染的边界数
        /// </summary>
        public int numBoundariesDrawn;
        /// <summary>
        /// 默认的3D字体
        /// </summary>
        public Font DefaultDrawingFont;
        /// <summary>
        /// Icon的名字的字体
        /// </summary>
        public Font iconNameFont;
        /// <summary>
        /// Icon的名字的字体
        /// </summary>
        public Font iconAwaysVisbleNameFont;
        /// <summary>
        /// GCP的名字的字体
        /// </summary>
        public Font GCPNameFont;
        /// <summary>
        /// 工具条字体
        /// </summary>
        public Font ToolbarFont;
        /// <summary>
        /// 默认子标题字体
        /// </summary>
        public System.Drawing.Font DefaultSubTitleFont;
        /// <summary>
        /// 默认子标题绘制字体
        /// </summary>
        public Font DefaultSubTitleDrawingFont;
        /// <summary>
        /// 屏幕宽度
        /// </summary>
        public int ScreenWidth;
        /// <summary>
        /// 屏幕高度
        /// </summary>
        public int ScreenHeight;
        /// <summary>
        /// 最后一个鼠标点击球的屏幕坐标的位置
        /// </summary>
        public static System.Drawing.Point LastMousePosition;
        /// <summary>
        /// 不在显示区域的Tile
        /// </summary>
        public int numberTilesDrawn;
        /// <summary>
        /// 当前鼠标点的屏幕坐标位置。
        /// </summary>
        public static System.Drawing.Point CurrentMousePosition;
        /// <summary>
        /// 当前鼠标点的纬度
        /// </summary>
        public static Angle CurrentMouseLatitude;
        /// <summary>
        /// 当前鼠标垫的经度
        /// </summary>
        public static Angle CurrentMouseLongtitude;
        /// <summary>
        /// 当前视窗北边纬度
        /// </summary>
        public static double CurrentViewNorthLatitude;
        /// <summary>
        /// 当前视窗南边纬度
        /// </summary>
        public static double CurrentViewSouthLatitude;
        /// <summary>
        /// 当前视窗西边经度
        /// </summary>
        public static double CurrentViewWestLongtitude;
        /// <summary>
        /// 当前视窗东边经度
        /// </summary>
        public static double CurrentViewEastLongtitude;
        /// <summary>
        /// 左上角的文本内容
        /// </summary>
        public string UpperLeftCornerText = "";
        /// <summary>
        /// 照相机对象
        /// </summary>
        CameraBase m_WorldCamera;
        /// <summary>
        /// 当前世界对象
        /// </summary>
        public World m_CurrentWorld = null;
        /// <summary>
        /// 记录了是否是按下了的鼠标左健
        /// </summary>
        public static bool IsLeftMouseButtonDown = false;
        /// <summary>
        /// 记录了是否是按下了的鼠标右健
        /// </summary>
        public static bool IsRightMouseButtonDown = false;
        /// <summary>
        /// 下载队列对象
        /// </summary>
        public static DownloadQueue DownloadQueue = new DownloadQueue();
        /// <summary>
        /// 被载入当前视域的纹理数
        /// </summary>
        public int TexturesLoadedThisFrame = 0;
        /// <summary>
        /// 位图对象
        /// </summary>
        private static System.Drawing.Bitmap bitmap;
        /// <summary>
        /// GDI+绘图图面
        /// </summary>
        public static System.Drawing.Graphics Graphics = null;
        /// <summary>
        /// 
        /// </summary>
        public bool RenderWireFrame = false;
        /// <summary>
        /// 被载入当前视域范围内的所有纹理对象
        /// </summary>
        protected static Hashtable m_textures = new Hashtable();
        /// <summary>
        /// 获取被载入当前视域范围内的所有纹理对象
        /// </summary>
        public static Hashtable Textures
        {
            get { return m_textures; }
        }
        /// <summary>
        /// 获取说设置摄像机对象
        /// </summary>
        public static CameraBase Camera = null;
        /// <summary>
        /// 获取或设置摄像机对象
        /// </summary>
        public CameraBase WorldCamera
        {
            get
            {
                return m_WorldCamera;
            }
            set
            {
                m_WorldCamera = value;
                Camera = value;
            }
        }
        /// <summary>
        /// 获取或设置当前星球对象
        /// </summary>
        public World CurrentWorld
        {
            get
            {
                return m_CurrentWorld;
            }
            set
            {
                m_CurrentWorld = value;
            }
        }
        /// <summary>
        /// 当前开始记时的绝对时间
        /// </summary>
        public static long CurrentFrameStartTicks;
        /// <summary>
        /// 总共的时间间隔的绝对时间
        /// </summary>
        public static float LastFrameSecondsElapsed;
        /// <summary>
        /// 鼠标的样式
        /// </summary>
        static CursorType mouseCursor;
        /// <summary>
        /// 上次操作鼠标的样式
        /// </summary>
        static CursorType lastCursor;
        /// <summary>
        /// 是否进行重浍
        /// </summary>
        bool isRepaint = true;
        /// <summary>
        /// 是否正在重浍
        /// </summary>
        bool isPainting;
        /// <summary>
        /// 字体哈西表，存储了所有的缓存字体
        /// </summary>
        Hashtable fontList = new Hashtable();
        /// <summary>
        /// D3D适配器对象
        /// </summary>
        public static Device Device = null;
        /// <summary>
        /// 测量时鼠标的形状
        /// </summary>
        System.Windows.Forms.Cursor measureCursor;
        /// <summary>
        /// 初始化一个渲染参数对象
        /// </summary>
        /// <param name="device"></param>
        /// <param name="parentForm"></param>
        public DrawArgs(Device device, System.Windows.Forms.Control parentForm)
        {
            //这里既保存了静态的，又保存了对象的属性
            this.parentControl = parentForm; //绑定父窗体（要进行绘制的窗体）
            DrawArgs.ParentControl = parentForm;//绑定给静态对象
            DrawArgs.Device = device;//绑定适配器对象
            this.device = device;

            //从WorldSetting中，读取默认绘制字体的样式
            DefaultDrawingFont = CreateFont(World.Settings.defaultFontName, World.Settings.defaultFontSize);
            //若不存在，则新建一个微软雅黑字体
            if (DefaultDrawingFont == null)
                DefaultDrawingFont = CreateFont("微软雅黑", 20, System.Drawing.FontStyle.Bold);
            //绘制图标名称字体
            iconNameFont = CreateFont("微软雅黑", 20, System.Drawing.FontStyle.Bold);
            //绘制图标名称字体
            iconAwaysVisbleNameFont = CreateFont("微软雅黑", 20, System.Drawing.FontStyle.Regular);
            //绘制控制点的字体
            GCPNameFont = CreateFont("微软雅黑", 20, System.Drawing.FontStyle.Bold);

            DefaultSubTitleFont = new System.Drawing.Font("Ariel", 8.0f);
            DefaultSubTitleDrawingFont = new Font(device, DefaultSubTitleFont);
            if (DefaultSubTitleDrawingFont == null)
                DefaultSubTitleDrawingFont = CreateFont("", 8);
			
            ToolbarFont = CreateFont(World.Settings.ToolbarFontName, World.Settings.ToolbarFontSize, World.Settings.ToolbarFontStyle);
		

            //创建要进行绘制的面版。
            bitmap = new System.Drawing.Bitmap(256, 256, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            DrawArgs.Graphics = System.Drawing.Graphics.FromImage(bitmap);
        }

        /// <summary>
        /// 开始准备进行绘制
        /// </summary>
        public void BeginRender()
        {
            //在渲染之前先进行初始化
            this.numberTilesDrawn = 0;
            this.TexturesLoadedThisFrame = 0;
            this.UpperLeftCornerText = "";
            this.numBoundaryPointsRendered = 0;
            this.numBoundaryPointsTotal = 0;
            this.numBoundariesDrawn = 0;
            this.isPainting = true;
        }
        /// <summary>
        /// 结束绘制
        /// </summary>
        public void EndRender()
        {
            this.isPainting = false;
        }

        /// <summary>
        /// 显示Render后的图象，在EndRender方法之后调用.
        /// </summary>
        public void Present()
        {
            //显示要之前定义好的要进行渲染的东西
            device.Present();
        }

        #region 创建字体

        /// <summary>
        /// 创建字体
        /// </summary>
        public Font CreateFont(string familyName, float emSize)
        {
            return CreateFont(familyName, emSize, System.Drawing.FontStyle.Regular);
        }

        /// <summary>
        /// 创建字体
        /// </summary>
        public Font CreateFont(string familyName, float emSize, System.Drawing.FontStyle style)
        {
            try
            {
                FontDescription description = new FontDescription();
                description.FaceName = familyName;
                //description.Height = (int)(1.9 * 10);
                description.Height = (int)(emSize);

                if (style == System.Drawing.FontStyle.Regular)
                    return CreateFont(description);
                if ((style & System.Drawing.FontStyle.Italic) != 0)
                    description.IsItalic = true;
                if ((style & System.Drawing.FontStyle.Bold) != 0)
                    description.Weight = FontWeight.Heavy;
                description.Quality = FontQuality.AntiAliased;
                return CreateFont(description);
            }
            catch
            {
                return DefaultDrawingFont;
            }
        }

        /// <summary>
        /// 创建字体
        /// </summary>
        public Font CreateFont(FontDescription description)
        {
            try
            {
                if (World.Settings.AntiAliasedText)
                    description.Quality = FontQuality.ClearTypeNatural;
                else
                    description.Quality = FontQuality.Default;

                // TODO: Improve font cache
                string hash = description.ToString();//.GetHashCode(); returned hash codes are not correct

                Font font = (Font)fontList[hash];
                if (font != null)
                    return font;

                font = new Font(this.device, description);
                //newDrawingFont.PreloadText("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXRZ");
                fontList.Add(hash, font);
                return font;
            }
            catch
            {
                return DefaultDrawingFont;
            }
        }

        #endregion

        /// <summary>
        /// 鼠标的形状。
        /// </summary>
        public static CursorType MouseCursor
        {
            get
            {
                return mouseCursor;
            }
            set
            {
                mouseCursor = value;
            }
        }

        /// <summary>
        /// 更新父窗体的鼠标形状
        /// </summary>
        /// <param name="parent"></param>
        public void UpdateMouseCursor(System.Windows.Forms.Control parent)
        {
            if (lastCursor == mouseCursor)
                return;

            switch (mouseCursor)
            {
                case CursorType.Hand:
                    parent.Cursor = System.Windows.Forms.Cursors.Hand;
                    break;
                case CursorType.Cross:
                    parent.Cursor = System.Windows.Forms.Cursors.Cross;
                    break;
                case CursorType.Measure:
                    if (measureCursor == null)
                    {
                        string cursorPath = ((QRSTWorldGlobeControl)parentControl).DataDirectory + "\\Icons\\measure.cur";
                        measureCursor = ImageHelper.LoadCursor(cursorPath);
                    }
                    parent.Cursor = measureCursor;
                    break;
                case CursorType.SizeAll:
                    parent.Cursor = System.Windows.Forms.Cursors.SizeAll;
                    break;
                case CursorType.SizeWE:
                    parent.Cursor = System.Windows.Forms.Cursors.SizeWE;
                    break;
                case CursorType.SizeNS:
                    parent.Cursor = System.Windows.Forms.Cursors.SizeNS;
                    break;
                case CursorType.SizeNESW:
                    parent.Cursor = System.Windows.Forms.Cursors.SizeNESW;
                    break;
                case CursorType.SizeNWSE:
                    parent.Cursor = System.Windows.Forms.Cursors.SizeNWSE;
                    break;
                default:
                    parent.Cursor = System.Windows.Forms.Cursors.Arrow;
                    break;
            }
            lastCursor = mouseCursor;
        }

        /// <summary>
        /// 获取是否正在进行绘制
        /// </summary>
        public bool IsPainting
        {
            get
            {
                return this.isPainting;
            }
        }

        /// <summary>
        /// 获取或设置是否进行重新绘制
        /// </summary>
        public bool Repaint
        {
            get
            {
                return this.isRepaint;
            }
            set
            {
                this.isRepaint = value;
            }
        }

        #region IDisposable Members

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            foreach (IDisposable font in fontList.Values)
            {
                if (font != null)
                {
                    font.Dispose();
                }
            }
            fontList.Clear();

            if (measureCursor != null)
            {
                measureCursor.Dispose();
                measureCursor = null;
            }

            if (DownloadQueue != null)
            {
                DownloadQueue.Dispose();
                DownloadQueue = null;
            }

            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
