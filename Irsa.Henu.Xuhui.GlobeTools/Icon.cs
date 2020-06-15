using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Net;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Qrst.Renderable
{
    /// <summary>
    /// Icon纹理类
    /// </summary>
    public class IconTexture : IDisposable
    {
        /// <summary>
        /// Icon所对应的纹理
        /// </summary>
        public Texture Texture;
        /// <summary>
        /// Icon的宽度
        /// </summary>
        public int Width;
        /// <summary>
        /// Icon的高度
        /// </summary>
        public int Height;
        public int ReferenceCount;

        /// <summary>
        /// 获得Icon纹理对象
        /// </summary>
        /// <param name="device">d3d对象</param>
        /// <param name="textureFileName">图片的路径</param>
        public IconTexture(Device device, string textureFileName)
        {
            //判断图片路径是否为空
            if ((textureFileName != null) && textureFileName.Length > 0)
            {
                //判断是否是GDI支持的图片，若是，则用GDI读成Image或Bitmap对象，再转换为纹理，速度会快
                if (ImageHelper.IsGdiSupportedImageFormat(textureFileName))
                {
                    //转换为Texture对象
                    using (Image image = ImageHelper.LoadImage(textureFileName))
                        LoadImage(device, image);
                }
                else
                {
                    //只能用DirectX读取的对象
                    Texture = ImageHelper.LoadIconTexture(textureFileName);
                    using (Surface s = Texture.GetSurfaceLevel(0))
                    {
                        SurfaceDescription desc = s.Description;
                        Width = desc.Width;
                        Height = desc.Height;
                    }
                }
            }
        }

        /// <summary>
        /// 获得Icon纹理对象 <see cref= "T:Qrst.Renderable.IconTexture"/> class 
        /// 从一个Bitmap对象.
        /// </summary>
        public IconTexture(Device device, Bitmap image)
        {
            LoadImage(device, image);
        }

        /// <summary>
        /// 直接得到纹理对象
        /// </summary>
        /// <param name="device"></param>
        /// <param name="image"></param>
        protected void LoadImage(Device device, Image image)
        {
            Width = (int)Math.Round(Math.Pow(2, (int)(Math.Ceiling(Math.Log(image.Width) / Math.Log(2)))));
            if (Width > device.DeviceCaps.MaxTextureWidth)
                Width = device.DeviceCaps.MaxTextureWidth;

            Height = (int)Math.Round(Math.Pow(2, (int)(Math.Ceiling(Math.Log(image.Height) / Math.Log(2)))));
            if (Height > device.DeviceCaps.MaxTextureHeight)
                Height = device.DeviceCaps.MaxTextureHeight;

            using (Bitmap textureSource = new Bitmap(Width, Height))
            using (Graphics g = Graphics.FromImage(textureSource))
            {
                g.DrawImage(image, 0, 0, Width, Height);
                if (Texture != null)
                    Texture.Dispose();
                Texture = new Texture(device, textureSource, Usage.None, Pool.Managed);
            }
        }

        #region 释放纹理对象

        public void Dispose()
        {
            if (Texture != null)
            {
                Texture.Dispose();
                Texture = null;
            }

            GC.SuppressFinalize(this);
        }

        #endregion
    }

    /// <summary>
    /// 图标集合对象
    /// </summary>
    public class Icons : RenderableObjectList
    {
        /// <summary>
        /// 纹理缓存对象
        /// </summary>
        protected Hashtable m_textures = new Hashtable();
        /// <summary>
        /// DirectX绘制图标Icon对象
        /// </summary>
        protected Sprite m_sprite;

        /// <summary>
        /// 鼠标Hover上去的颜色
        /// </summary>
        static int hotColor = Color.White.ToArgb();
        /// <summary>
        /// 平常时的颜色
        /// </summary>
        static int normalColor = Color.FromArgb(200, 255, 255, 255).ToArgb();
        /// <summary>
        /// 名字的颜色
        /// </summary>
        static int nameColor = Color.White.ToArgb();
        /// <summary>
        /// 描述信息的文字颜色
        /// </summary>
        static int descriptionColor = Color.White.ToArgb();

        /// <summary>
        /// 刷新时间
        /// </summary>
        System.Timers.Timer refreshTimer;

        /// <summary>
        /// 当前鼠标Hover的对象
        /// </summary>
        protected Icon mouseOverIcon;

        /// <summary>
        /// 初始化一个Icons对象 <see cref= "T:Qrst.Renderable.Icons"/> class 
        /// </summary>
        /// <param name="name"></param>
        public Icons(string name)
            : base(name)
        {

        }

        public Icons(string name,
            string dataSource,
            TimeSpan refreshInterval,
            World parentWorld,
            Cache cache)
            : base(name, dataSource, refreshInterval, parentWorld, cache)
        {
        }

        /// <summary>
        /// 添加一个Icon对象.
        /// </summary>
        public void AddIcon(Icon icon)
        {
            Add(icon);
        }

        #region RenderableObject methods

        /// <summary>
        /// 添加一个Icon对象.
        /// </summary>
        public override void Add(RenderableObject ro)
        {
            m_children.Add(ro);
            isInitialized = false;
        }

        /// <summary>
        /// 初始化图标集合：1.把所有的图标读到缓存Hashtable中去,并设置Icon的高宽.2.计算每个Icon的包围范围
        /// </summary>
        /// <param name="drawArgs"></param>
        public override void Initialize(DrawArgs drawArgs)
        {
            //判断是否显示此图层，若不显示，则返回
            if (!isOn)
                return;
            //判断绘制Icon的对象，是否存在，若不存在，则创建，若存在，则先释放
            if (m_sprite != null)
            {
                m_sprite.Dispose();
                m_sprite = null;
            }
            m_sprite = new Sprite(drawArgs.device);


            System.TimeSpan smallestRefreshInterval = System.TimeSpan.MaxValue;

            // 添加所有的Icon对象的纹理到缓存中去，并设置Icon的宽与高
            foreach (RenderableObject ro in m_children)
            {
                Icon icon = ro as Icon;
                if (icon == null)
                {
                    // 判断当前Icon是否显示，若不显示，则Continue下一个
                    if (ro.IsOn)
                        ro.Initialize(drawArgs);
                    continue;
                }


                if (icon.RefreshInterval.TotalMilliseconds != 0 && icon.RefreshInterval != TimeSpan.MaxValue && icon.RefreshInterval < smallestRefreshInterval)
                    smallestRefreshInterval = icon.RefreshInterval;

                // 子Icon初始化
                icon.Initialize(drawArgs);
                object key = null;
                //创建一个IconeTexture对象
                IconTexture iconTexture = null;

                //从文件中读取当前Icon
                if (icon.TextureFileName != null && icon.TextureFileName.Length > 0)
                {

                    // 从缓存中读取Icon纹理对象
                    iconTexture = (IconTexture)m_textures[icon.TextureFileName];
                    //若缓存中不存在，则从文件中读取Icon对象
                    if (iconTexture == null)
                    {
                        key = icon.TextureFileName;
                        iconTexture = new IconTexture(drawArgs.device, icon.TextureFileName);
                    }
                }
                //从Bitmap对象中读取Icon对象
                else
                {
                    if (icon.Image != null)
                    {
                        iconTexture = (IconTexture)m_textures[icon.Image];
                        if (iconTexture == null)
                        {
                            key = icon.Image;
                            iconTexture = new IconTexture(drawArgs.device, icon.Image);
                        }
                    }
                }
                //若仍然没有纹理，则循环下一个
                if (iconTexture == null)
                    continue;
                //若有纹理的话
                if (key != null)
                {
                    //把纹理放到缓存中去
                    m_textures.Add(key, iconTexture);

                    // 设置Icon的宽度
                    if (icon.Width == 0)
                        icon.Width = iconTexture.Width;
                    // 设置Icon的高度
                    if (icon.Height == 0)
                        icon.Height = iconTexture.Height;
                }
            }

            // 计算Icon的包围盒
            foreach (RenderableObject ro in m_children)
            {
                Icon icon = ro as Icon;
                if (icon == null)
                    continue;

                if (GetTexture(icon) == null)
                {
                    //计算文字的范围
                    icon.SelectionRectangle = drawArgs.defaultDrawingFont.MeasureString(null, icon.Name, DrawTextFormat.None, 0);
                }
                else
                {
                    //计算图标的范围
                    icon.SelectionRectangle = new Rectangle(0, 0, icon.Width, icon.Height);
                }

                // Center the box at (0,0)
                icon.SelectionRectangle.Offset(-icon.SelectionRectangle.Width / 2, -icon.SelectionRectangle.Height / 2);
            }

            if (refreshTimer == null && smallestRefreshInterval != TimeSpan.MaxValue)
            {
                refreshTimer = new System.Timers.Timer(smallestRefreshInterval.TotalMilliseconds);
                refreshTimer.Elapsed += new System.Timers.ElapsedEventHandler(refreshTimer_Elapsed);
                refreshTimer.Start();
            }

            isInitialized = true;
        }

        /// <summary>
        /// 释放所有的对象
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();

            if (m_textures != null)
            {
                foreach (IconTexture iconTexture in m_textures.Values)
                    iconTexture.Texture.Dispose();
                m_textures.Clear();
            }

            if (m_sprite != null)
            {
                m_sprite.Dispose();
                m_sprite = null;
            }

            if (refreshTimer != null)
            {
                refreshTimer.Stop();
                refreshTimer.Dispose();
                refreshTimer = null;
            }
        }

        /// <summary>
        /// 鼠标点击事件
        /// </summary>
        /// <param name="drawArgs"></param>
        /// <returns></returns>
        public override bool PerformSelectionAction(DrawArgs drawArgs)
        {
            //执行所有的子Icon的PerformSelectionAction方法
            foreach (RenderableObject ro in m_children)
            {
                if (!ro.IsOn)
                    continue;
                if (!ro.isSelectable)
                    continue;

                Icon icon = ro as Icon;
                if (icon == null)
                {
                    if (ro.PerformSelectionAction(drawArgs))
                        return true;
                    continue;
                }
                //判断当前Icon的位置，是否在视域范围内,若不在则返回
                if (!drawArgs.WorldCamera.ViewFrustum.ContainsPoint(icon.Position))
                    continue;

                //判断鼠标的位置是否在图标的包围盒内，若不在，则返回
                Vector3 referenceCenter = new Vector3(
                    (float)drawArgs.WorldCamera.ReferenceCenter.X,
                    (float)drawArgs.WorldCamera.ReferenceCenter.Y,
                    (float)drawArgs.WorldCamera.ReferenceCenter.Z);

                Vector3 projectedPoint = drawArgs.WorldCamera.Project(icon.Position - referenceCenter);
                if (!icon.SelectionRectangle.Contains(
                    DrawArgs.LastMousePosition.X - (int)projectedPoint.X,
                    DrawArgs.LastMousePosition.Y - (int)projectedPoint.Y))
                    continue;

                try
                {
                    //若当前鼠标是左键事件
                    if (DrawArgs.IsLeftMouseButtonDown && !DrawArgs.IsRightMouseButtonDown)
                    {
                        //使摄像机设置到图标设置的范围
                        if (icon.OnClickZoomAltitude != double.NaN || icon.OnClickZoomHeading != double.NaN || icon.OnClickZoomTilt != double.NaN)
                        {
                            drawArgs.WorldCamera.SetPosition(
                                icon.Latitude,
                                icon.Longitude,
                                icon.OnClickZoomHeading,
                                icon.OnClickZoomAltitude,
                                icon.OnClickZoomTilt);
                        }
                        //执行当前Icon的PerformSelection方法
                        if (icon.isSelectable)
                            icon.PerformSelectionAction(drawArgs);

                    }
                    //若当前鼠标是右键事件
                    else if (!DrawArgs.IsLeftMouseButtonDown && DrawArgs.IsRightMouseButtonDown)
                    {
                        //不做任何处理
                    }
                    return true;
                }
                catch
                {
                }
            }
            return false;
        }

        /// <summary>
        /// 渲染对象：1.渲染所有的非Icon对象；2.渲染所有的非选中状态的Icon对象；3.渲染选中的Icon对象
        /// </summary>
        /// <param name="drawArgs"></param>
        public override void Render(DrawArgs drawArgs)
        {
            if (!isOn)
                return;

            if (!isInitialized)
                return;

            // 渲染所有的非Icon对象
            foreach (RenderableObject ro in m_children)
            {
                if (!ro.IsOn)
                    continue;
                ro.Render(drawArgs);
            }

            int closestIconDistanceSquared = int.MaxValue;
            Icon closestIcon = null;

            // 开始渲染所有的的非Icon图层
            m_sprite.Begin(SpriteFlags.AlphaBlend);
            foreach (RenderableObject ro in m_children)
            {
                if (!ro.IsOn)
                    continue;
                Icon icon = ro as Icon;
                if (icon == null)
                    continue;

                //计算图标的位置
                Vector3 translationVector = new Vector3(
                (float)(icon.PositionD.X - drawArgs.WorldCamera.ReferenceCenter.X),
                (float)(icon.PositionD.Y - drawArgs.WorldCamera.ReferenceCenter.Y),
                (float)(icon.PositionD.Z - drawArgs.WorldCamera.ReferenceCenter.Z));

                // 计算最近的一个Icon对象
                Vector3 projectedPoint = drawArgs.WorldCamera.Project(translationVector);

                int dx = DrawArgs.LastMousePosition.X - (int)projectedPoint.X;
                int dy = DrawArgs.LastMousePosition.Y - (int)projectedPoint.Y;
                if (icon.SelectionRectangle.Contains(dx, dy))
                {
                    // Mouse is over, check whether this icon is closest
                    int distanceSquared = dx * dx + dy * dy;
                    if (distanceSquared < closestIconDistanceSquared)
                    {
                        closestIconDistanceSquared = distanceSquared;
                        closestIcon = icon;
                    }
                }
                //渲染不是被Hover的图层
                if (icon != mouseOverIcon)
                    Render(drawArgs, icon, projectedPoint);
            }

            // 渲染被Hover的图层
            if (mouseOverIcon != null)
            {
                Vector3 translationVector = new Vector3(
                    (float)(mouseOverIcon.PositionD.X - drawArgs.WorldCamera.ReferenceCenter.X),
                    (float)(mouseOverIcon.PositionD.Y - drawArgs.WorldCamera.ReferenceCenter.Y),
                    (float)(mouseOverIcon.PositionD.Z - drawArgs.WorldCamera.ReferenceCenter.Z));

                Render(drawArgs, mouseOverIcon, drawArgs.WorldCamera.Project(translationVector));
            }

            mouseOverIcon = closestIcon;

            m_sprite.End();
        }

        #endregion

        /// <summary>
        /// 渲染图标
        /// </summary>
        protected virtual void Render(DrawArgs drawArgs, Icon icon, Vector3 projectedPoint)
        {
            //判断当前Icon是否呗初始化了，比如，若重新定义了图标的位置，则它的isInitialized就是false
            if (!icon.isInitialized)
                icon.Initialize(drawArgs);
            //判断当前图标是否在视地范围内，若不在，则不进行绘制
            if (!drawArgs.WorldCamera.ViewFrustum.ContainsPoint(icon.Position))
                return;

            // 判断图标是否在最大，最小可见的范围内，若不在，则不进行绘制
            double distanceToIcon = Vector3.Length(icon.Position - drawArgs.WorldCamera.Position);
            if (distanceToIcon > icon.MaximumDisplayDistance)
                return;
            if (distanceToIcon < icon.MinimumDisplayDistance)
                return;
            //获得当前图层的纹理对象
            IconTexture iconTexture = GetTexture(icon);
            //判断是否是MouseOver对象
            bool isMouseOver = icon == mouseOverIcon;
            //若是MouseOver对象，则绘制Description里的内容
            if (isMouseOver)
            {
                //若是MouseOver对象
                isMouseOver = true;
                //若当前图层可以操作，则设置当前的鼠标是Hand
                if (icon.isSelectable)
                    DrawArgs.MouseCursor = CursorType.Hand;
                ////显示文字描述信息,暂时不需要
                //string description = icon.Description;
                //if(description==null)
                //    description = icon.ClickableActionURL;
                ////绘制文字信息
                //if(description!=null)
                //{
                //    //设置文字的绘制区域
                //    DrawTextFormat format = DrawTextFormat.NoClip | DrawTextFormat.WordBreak | DrawTextFormat.Bottom;
                //    int left = 10;
                //    if(World.Settings.showLayerManager)
                //        left += World.Settings.layerManagerWidth;
                //    Rectangle rect = Rectangle.FromLTRB(left, 10, drawArgs.screenWidth - 10, drawArgs.screenHeight - 10 );

                //    //绘制边框
                //    drawArgs.defaultDrawingFont.DrawText(
                //        m_sprite, description,
                //        rect,
                //        format, 0xb0 << 24 );

                //    rect.Offset(2,0);
                //    drawArgs.defaultDrawingFont.DrawText(
                //        m_sprite, description,
                //        rect,
                //        format, 0xb0 << 24 );

                //    rect.Offset(0,2);
                //    drawArgs.defaultDrawingFont.DrawText(
                //        m_sprite, description,
                //        rect,
                //        format, 0xb0 << 24 );

                //    rect.Offset(-2,0);
                //    drawArgs.defaultDrawingFont.DrawText(
                //        m_sprite, description,
                //        rect,
                //        format, 0xb0 << 24 );

                //    // 绘制文字信息
                //    rect.Offset(1,-1);
                //    drawArgs.defaultDrawingFont.DrawText(
                //        m_sprite, description,
                //        rect, 
                //        format, descriptionColor );
                //}
            }

            //获取颜色
            int color = isMouseOver ? hotColor : normalColor;
            if (iconTexture == null || isMouseOver || icon.NameAlwaysVisible)
            {
                // 绘制图标的名称
                if (icon.Name != null)
                {
                    // Render name field
                    const int labelWidth = 1000; // Dummy value needed for centering the text
                    if (iconTexture == null)
                    {
                        // Center over target as we have no bitmap
                        Rectangle rect = new Rectangle(
                            (int)projectedPoint.X - (labelWidth >> 1),
                            (int)(projectedPoint.Y - (drawArgs.iconNameFont.Description.Height >> 1)),
                            labelWidth,
                            drawArgs.screenHeight);

                        drawArgs.iconNameFont.DrawText(m_sprite, icon.Name, rect, DrawTextFormat.Center, color);
                    }
                    else
                    {
                        // Adjust text to make room for icon
                        int spacing = (int)(icon.Width * 0.3f);
                        if (spacing > 10)
                            spacing = 10;
                        int offsetForIcon = (icon.Width >> 1) + spacing;

                        Rectangle rect = new Rectangle(
                            (int)projectedPoint.X + offsetForIcon,
                            (int)(projectedPoint.Y - (drawArgs.iconNameFont.Description.Height >> 1)),
                            labelWidth,
                            drawArgs.screenHeight);

                        drawArgs.iconNameFont.DrawText(m_sprite, icon.Name, rect, DrawTextFormat.WordBreak, color);
                    }
                }
            }

            //绘制图标
            if (iconTexture != null)
            {
                // Render icon
                float xscale = (float)icon.Width / iconTexture.Width;
                float yscale = (float)icon.Height / iconTexture.Height;
                m_sprite.Transform = Matrix.Scaling(xscale, yscale, 0);

                if (icon.IsRotated)
                    m_sprite.Transform *= Matrix.RotationZ((float)icon.Rotation.Radians - (float)drawArgs.WorldCamera.Heading.Radians);

                m_sprite.Transform *= Matrix.Translation(projectedPoint.X, projectedPoint.Y, 0);
                m_sprite.Draw(iconTexture.Texture,
                    new Vector3(iconTexture.Width >> 1, iconTexture.Height >> 1, 0),
                    Vector3.Empty,
                    color);

                // Reset transform to prepare for text rendering later
                m_sprite.Transform = Matrix.Identity;
            }
        }

        /// <summary>
        /// 从缓存中获得当前图层的纹理对象，若不存在，则返回null
        /// </summary>
        protected IconTexture GetTexture(Icon icon)
        {
            object key = null;

            if (icon.Image == null)
            {
                key = icon.TextureFileName;
            }
            else
            {
                key = icon.Image;
            }
            if (key == null)
                return null;

            IconTexture res = (IconTexture)m_textures[key];
            return res;
        }

        bool isUpdating = false;
        /// <summary>
        /// 刷新图标集合图层
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void refreshTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (isUpdating)
                return;
            isUpdating = true;
            try
            {
                for (int i = 0; i < this.ChildObjects.Count; i++)
                {
                    RenderableObject ro = (RenderableObject)this.ChildObjects[i];
                    if (ro != null && ro.IsOn && ro is Icon)
                    {
                        Icon icon = (Icon)ro;

                        if (icon.RefreshInterval == TimeSpan.MaxValue || icon.LastRefresh > System.DateTime.Now - icon.RefreshInterval)
                            continue;

                        object key = null;
                        IconTexture iconTexture = null;

                        if (icon.TextureFileName != null && icon.TextureFileName.Length > 0)
                        {
                            if (icon.TextureFileName.ToLower().StartsWith("http://") && icon.SaveFilePath != null)
                            {
                                //download it
                                try
                                {
                                    Qrst.Net.WebDownload webDownload = new Qrst.Net.WebDownload(icon.TextureFileName);
                                    webDownload.DownloadType = Qrst.Net.DownloadType.Unspecified;

                                    System.IO.FileInfo saveFile = new System.IO.FileInfo(icon.SaveFilePath);
                                    if (!saveFile.Directory.Exists)
                                        saveFile.Directory.Create();

                                    webDownload.DownloadFile(saveFile.FullName);
                                }
                                catch { }

                                iconTexture = (IconTexture)m_textures[icon.SaveFilePath];
                                if (iconTexture != null)
                                {
                                    IconTexture tempTexture = iconTexture;
                                    m_textures[icon.SaveFilePath] = new IconTexture(DrawArgs.Device, icon.SaveFilePath);
                                    tempTexture.Dispose();
                                }
                                else
                                {
                                    key = icon.SaveFilePath;
                                    iconTexture = new IconTexture(DrawArgs.Device, icon.SaveFilePath);

                                    // New texture, cache it
                                    m_textures.Add(key, iconTexture);

                                    // Use default dimensions if not set
                                    if (icon.Width == 0)
                                        icon.Width = iconTexture.Width;
                                    if (icon.Height == 0)
                                        icon.Height = iconTexture.Height;
                                }

                            }
                            else
                            {
                                // Icon image from file
                                iconTexture = (IconTexture)m_textures[icon.TextureFileName];
                                if (iconTexture != null)
                                {
                                    IconTexture tempTexture = iconTexture;
                                    m_textures[icon.SaveFilePath] = new IconTexture(DrawArgs.Device, icon.TextureFileName);
                                    tempTexture.Dispose();
                                }
                                else
                                {
                                    key = icon.SaveFilePath;
                                    iconTexture = new IconTexture(DrawArgs.Device, icon.TextureFileName);

                                    // New texture, cache it
                                    m_textures.Add(key, iconTexture);

                                    // Use default dimensions if not set
                                    if (icon.Width == 0)
                                        icon.Width = iconTexture.Width;
                                    if (icon.Height == 0)
                                        icon.Height = iconTexture.Height;
                                }
                            }
                        }
                        else
                        {
                            // Icon image from bitmap
                            if (icon.Image != null)
                            {
                                iconTexture = (IconTexture)m_textures[icon.Image];
                                if (iconTexture != null)
                                {
                                    IconTexture tempTexture = iconTexture;
                                    m_textures[icon.SaveFilePath] = new IconTexture(DrawArgs.Device, icon.Image);
                                    tempTexture.Dispose();
                                }
                                else
                                {
                                    key = icon.SaveFilePath;
                                    iconTexture = new IconTexture(DrawArgs.Device, icon.Image);

                                    // New texture, cache it
                                    m_textures.Add(key, iconTexture);

                                    // Use default dimensions if not set
                                    if (icon.Width == 0)
                                        icon.Width = iconTexture.Width;
                                    if (icon.Height == 0)
                                        icon.Height = iconTexture.Height;
                                }
                            }
                        }

                        icon.LastRefresh = System.DateTime.Now;
                    }
                }
            }
            catch { }
            finally
            {
                isUpdating = false;
            }
        }
    }

    /// <summary>
    /// 一个图标图层
    /// </summary>
    public class Icon : RenderableObject
    {
        public double OnClickZoomAltitude = double.NaN;
        public double OnClickZoomHeading = double.NaN;
        public double OnClickZoomTilt = double.NaN;
        public string SaveFilePath = null;
        public System.DateTime LastRefresh = System.DateTime.MinValue;
        public System.TimeSpan RefreshInterval = System.TimeSpan.MaxValue;
        System.Collections.ArrayList overlays = new ArrayList();

        private Angle m_rotation = Angle.Zero;//旋转的角度
        private bool m_isRotated = false;//Icon是否旋转
        private Point3d m_positionD = new Point3d();//Icon的位置
        private bool m_nameAlwaysVisible = false;



        /// <summary>
        /// 点击此Icon的URL
        /// </summary>
        protected string m_clickableActionURL;

        /// <summary>
        /// 此Icon的纬度
        /// </summary>
        protected double m_latitude;

        /// <summary>
        /// 此Icon的经度
        /// </summary>
        protected double m_longitude;


        /// <summary>
        /// Icon的高度
        /// </summary>
        public double Altitude;
        /// <summary>
        /// Icon的文件的路径
        /// </summary>
        public string TextureFileName;

        /// <summary>
        /// Icon所对应的图片对象.
        /// </summary>
        public Bitmap Image;

        /// <summary>
        /// Icon的宽度.
        /// </summary>
        public int Width;

        /// <summary>
        /// Icon的高度
        /// </summary>
        public int Height;

        /// <summary>
        /// 点击Icon后，链接到的网址地址
        /// </summary>
        public string ClickableActionURL
        {
            get
            {
                return m_clickableActionURL;
            }
            set
            {
                isSelectable = value != null;
                m_clickableActionURL = value;
            }
        }

        public Point3d PositionD
        {
            get { return m_positionD; }
            set { m_positionD = value; }
        }

        /// <summary>
        /// 最高能见度
        /// </summary>
        public double MaximumDisplayDistance = double.MaxValue;

        /// <summary>
        /// 最低能见度
        /// </summary>
        public double MinimumDisplayDistance;

        /// <summary>
        /// 判断鼠标是否在此巨型范围内
        /// </summary>
        public Rectangle SelectionRectangle;

        /// <summary>
        /// 经度
        /// </summary>
        public double Latitude
        {
            get { return m_latitude; }
        }

        /// <summary>
        /// 纬度
        /// </summary>
        public double Longitude
        {
            get { return m_longitude; }
        }

        /// <summary>
        /// 旋转图标的角度
        /// </summary>
        public Angle Rotation
        {
            get
            {
                return m_rotation;
            }
            set
            {
                m_rotation = value;
            }
        }

        /// <summary>
        /// 判断图标是否旋转
        /// </summary>
        public bool IsRotated
        {
            get
            {
                return m_isRotated;
            }
            set
            {
                m_isRotated = value;
            }
        }

        /// <summary>
        /// 是否一直显示Icon的名称
        /// </summary>
        public bool NameAlwaysVisible
        {
            get { return m_nameAlwaysVisible; }
            set { m_nameAlwaysVisible = value; }
        }

        /// <summary>
        /// 初始化一个Icon对象 <see cref= "T:Qrst.Renderable.Icon"/> class 
        /// </summary>
        /// <param name="name">Icon对象名称</param>
        /// <param name="latitude"> 纬度.</param>
        /// <param name="longitude">经度.</param>
        public Icon(string name,
            double latitude,
            double longitude)
            : base(name)
        {
            //设置经纬度
            m_latitude = latitude;
            m_longitude = longitude;
            this.RenderPriority = RenderPriority.Icons;
        }

        /// <summary>
        /// 初始化一个Icon对象 <see cref= "T:Qrst.Renderable.Icon"/> class 
        /// </summary>
        /// <param name="name">Icon对象名称</param>
        /// <param name="latitude"> 纬度.</param>
        /// <param name="longitude">经度.</param>
        /// <param name="heightAboveSurface">Icon的高度.</param>
        public Icon(string name,
            double latitude,
            double longitude,
            double heightAboveSurface)
            : base(name)
        {
            //设置经纬度
            m_latitude = latitude;
            m_longitude = longitude;
            //设置高度
            Altitude = heightAboveSurface;
            this.RenderPriority = RenderPriority.Icons;
        }

        #region Obsolete

        /// <summary>
        /// Initializes a new instance of the <see cref= "T:Qrst.Renderable.Icon"/> class 
        /// </summary>
        /// <param name="name">Name of the icon</param>
        /// <param name="latitude">Latitude in decimal degrees.</param>
        /// <param name="longitude">Longitude in decimal degrees.</param>
        /// <param name="heightAboveSurface">Icon height (meters) above sea level.</param>
        [Obsolete]
        public Icon(string name,
            double latitude,
            double longitude,
            double heightAboveSurface,
            World parentWorld)
            : base(name)
        {
            m_latitude = latitude;
            m_longitude = longitude;
            this.Altitude = heightAboveSurface;
            this.RenderPriority = RenderPriority.Icons;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref= "T:Qrst.Renderable.Icon"/> class 
        /// </summary>
        /// <param name="name">Name of the icon</param>
        /// <param name="latitude">Latitude in decimal degrees.</param>
        /// <param name="longitude">Longitude in decimal degrees.</param>
        /// <param name="heightAboveSurface">Icon height (meters) above sea level.</param>
        [Obsolete]
        public Icon(string name,
            string description,
            double latitude,
            double longitude,
            double heightAboveSurface,
            World parentWorld,
            Bitmap image,
            int width,
            int height,
            string actionURL)
            : base(name)
        {
            this.Description = description;
            m_latitude = latitude;
            m_longitude = longitude;
            this.Altitude = heightAboveSurface;
            this.Image = image;
            this.Width = width;
            this.Height = height;
            ClickableActionURL = actionURL;
            this.RenderPriority = RenderPriority.Icons;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref= "T:Qrst.Renderable.Icon"/> class 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="heightAboveSurface"></param>
        /// <param name="parentWorld"></param>
        /// <param name="TextureFileName"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="actionURL"></param>
        [Obsolete]
        public Icon(string name,
            string description,
            double latitude,
            double longitude,
            double heightAboveSurface,
            World parentWorld,
            string TextureFileName,
            int width,
            int height,
            string actionURL)
            : base(name)
        {
            this.Description = description;
            m_latitude = latitude;
            m_longitude = longitude;
            this.Altitude = heightAboveSurface;
            this.TextureFileName = TextureFileName;
            this.Width = width;
            this.Height = height;
            ClickableActionURL = actionURL;
            this.RenderPriority = RenderPriority.Icons;
        }

        #endregion

        /// <summary>
        /// 设置Icon的位置
        /// </summary>
        /// <param name="latitude"> 纬度.</param>
        /// <param name="longitude">经度.</param>
        public void SetPosition(double latitude, double longitude)
        {
            m_latitude = latitude;
            m_longitude = longitude;

            // 重新计算Icon的坐标，在初始化方法中
            isInitialized = false;
        }

        /// <summary>
        /// 设置Icon的位置
        /// </summary>
        /// <param name="latitude"> 纬度.</param>
        /// <param name="longitude">经度.</param>
        /// <param name="altitude"> 高度.</param>
        public void SetPosition(double latitude, double longitude, double altitude)
        {
            m_latitude = latitude;
            m_longitude = longitude;
            Altitude = altitude;

            // 重新计算Icon的坐标，在初始化方法中
            isInitialized = false;
        }

        #region RenderableObject methods

        /// <summary>
        /// 初始化Icon对象
        /// </summary>
        /// <param name="drawArgs"></param>
        public override void Initialize(DrawArgs drawArgs)
        {
            double samplesPerDegree = 50.0 / (drawArgs.WorldCamera.ViewRange.Degrees);//计算当前每一度有多少个格网
            double elevation = drawArgs.CurrentWorld.TerrainAccessor.GetElevationAt(m_latitude, m_longitude, samplesPerDegree);//计算当前经纬度的海拔系信息
            double altitude = (World.Settings.VerticalExaggeration * Altitude + World.Settings.VerticalExaggeration * elevation);//计算Icon的实际显示高度=夸大因子*Altitude+海拔高度

            //转换为屏幕向量信息
            Position = MathEngine.SphericalToCartesian(m_latitude, m_longitude,
                altitude + drawArgs.WorldCamera.WorldRadius);

            //转换到屏幕坐标
            m_positionD = MathEngine.SphericalToCartesianD(
                Angle.FromDegrees(m_latitude),
                Angle.FromDegrees(m_longitude),
                altitude + drawArgs.WorldCamera.WorldRadius);

            isInitialized = true;
        }

        /// <summary>
        /// 释放Icon对象
        /// </summary>
        public override void Dispose()
        {

        }

        ///// <summary>
        ///// 点击事件
        ///// </summary>
        ///// <param name="drawArgs"></param>
        ///// <returns></returns>
        //public override bool PerformSelectionAction(DrawArgs drawArgs)
        //{
        //    return false;
        //}

        /// <summary>
        /// 记录了上次的视图矩阵
        /// </summary>
        Matrix lastView = Matrix.Identity;

        public override void Update(DrawArgs drawArgs)
        {
            if (drawArgs.WorldCamera.ViewMatrix != lastView && drawArgs.CurrentWorld.TerrainAccessor != null && drawArgs.WorldCamera.Altitude < 300000)
            {
                double samplesPerDegree = 50.0 / drawArgs.WorldCamera.ViewRange.Degrees;
                double elevation = drawArgs.CurrentWorld.TerrainAccessor.GetElevationAt(m_latitude, m_longitude, samplesPerDegree);
                double altitude = World.Settings.VerticalExaggeration * Altitude + World.Settings.VerticalExaggeration * elevation;
                Position = MathEngine.SphericalToCartesian(m_latitude, m_longitude,
                    altitude + drawArgs.WorldCamera.WorldRadius);

                lastView = drawArgs.WorldCamera.ViewMatrix;
            }
        }

        public override void Render(DrawArgs drawArgs)
        {

        }

        #endregion

        private void RefreshTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {

        }
    }


    /// <summary>
    /// 地标图层
    /// </summary>
    public class PlaceIcon : Icon
    {
        /// <summary>
        /// 初始化一个Icon对象 <see cref= "T:Qrst.Renderable.Icon"/> class 
        /// </summary>
        /// <param name="name">Icon对象名称</param>
        /// <param name="latitude"> 纬度.</param>
        /// <param name="longitude">经度.</param>
        public PlaceIcon(string name,
            double latitude,
            double longitude)
            : base(name, latitude, longitude)
        {
        }

        /// <summary>
        /// 初始化一个Icon对象 <see cref= "T:Qrst.Renderable.Icon"/> class 
        /// </summary>
        /// <param name="name">Icon对象名称</param>
        /// <param name="latitude"> 纬度.</param>
        /// <param name="longitude">经度.</param>
        /// <param name="heightAboveSurface">Icon的高度.</param>
        public PlaceIcon(string name,
            double latitude,
            double longitude,
            double heightAboveSurface)
            : base(name, latitude, longitude, heightAboveSurface)
        {

        }

        ///// <summary>
        ///// 点击事件
        ///// </summary>
        ///// <param name="drawArgs"></param>
        ///// <returns></returns>
        //public override bool PerformSelectionAction(DrawArgs drawArgs)
        //{

        //    return false;
        //}

    }

    public enum IconType
    {
        None,
        Target
    }


}
