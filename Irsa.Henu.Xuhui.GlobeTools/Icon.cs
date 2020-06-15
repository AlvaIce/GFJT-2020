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
    /// Icon������
    /// </summary>
    public class IconTexture : IDisposable
    {
        /// <summary>
        /// Icon����Ӧ������
        /// </summary>
        public Texture Texture;
        /// <summary>
        /// Icon�Ŀ��
        /// </summary>
        public int Width;
        /// <summary>
        /// Icon�ĸ߶�
        /// </summary>
        public int Height;
        public int ReferenceCount;

        /// <summary>
        /// ���Icon�������
        /// </summary>
        /// <param name="device">d3d����</param>
        /// <param name="textureFileName">ͼƬ��·��</param>
        public IconTexture(Device device, string textureFileName)
        {
            //�ж�ͼƬ·���Ƿ�Ϊ��
            if ((textureFileName != null) && textureFileName.Length > 0)
            {
                //�ж��Ƿ���GDI֧�ֵ�ͼƬ�����ǣ�����GDI����Image��Bitmap������ת��Ϊ�����ٶȻ��
                if (ImageHelper.IsGdiSupportedImageFormat(textureFileName))
                {
                    //ת��ΪTexture����
                    using (Image image = ImageHelper.LoadImage(textureFileName))
                        LoadImage(device, image);
                }
                else
                {
                    //ֻ����DirectX��ȡ�Ķ���
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
        /// ���Icon������� <see cref= "T:Qrst.Renderable.IconTexture"/> class 
        /// ��һ��Bitmap����.
        /// </summary>
        public IconTexture(Device device, Bitmap image)
        {
            LoadImage(device, image);
        }

        /// <summary>
        /// ֱ�ӵõ��������
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

        #region �ͷ��������

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
    /// ͼ�꼯�϶���
    /// </summary>
    public class Icons : RenderableObjectList
    {
        /// <summary>
        /// ���������
        /// </summary>
        protected Hashtable m_textures = new Hashtable();
        /// <summary>
        /// DirectX����ͼ��Icon����
        /// </summary>
        protected Sprite m_sprite;

        /// <summary>
        /// ���Hover��ȥ����ɫ
        /// </summary>
        static int hotColor = Color.White.ToArgb();
        /// <summary>
        /// ƽ��ʱ����ɫ
        /// </summary>
        static int normalColor = Color.FromArgb(200, 255, 255, 255).ToArgb();
        /// <summary>
        /// ���ֵ���ɫ
        /// </summary>
        static int nameColor = Color.White.ToArgb();
        /// <summary>
        /// ������Ϣ��������ɫ
        /// </summary>
        static int descriptionColor = Color.White.ToArgb();

        /// <summary>
        /// ˢ��ʱ��
        /// </summary>
        System.Timers.Timer refreshTimer;

        /// <summary>
        /// ��ǰ���Hover�Ķ���
        /// </summary>
        protected Icon mouseOverIcon;

        /// <summary>
        /// ��ʼ��һ��Icons���� <see cref= "T:Qrst.Renderable.Icons"/> class 
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
        /// ���һ��Icon����.
        /// </summary>
        public void AddIcon(Icon icon)
        {
            Add(icon);
        }

        #region RenderableObject methods

        /// <summary>
        /// ���һ��Icon����.
        /// </summary>
        public override void Add(RenderableObject ro)
        {
            m_children.Add(ro);
            isInitialized = false;
        }

        /// <summary>
        /// ��ʼ��ͼ�꼯�ϣ�1.�����е�ͼ���������Hashtable��ȥ,������Icon�ĸ߿�.2.����ÿ��Icon�İ�Χ��Χ
        /// </summary>
        /// <param name="drawArgs"></param>
        public override void Initialize(DrawArgs drawArgs)
        {
            //�ж��Ƿ���ʾ��ͼ�㣬������ʾ���򷵻�
            if (!isOn)
                return;
            //�жϻ���Icon�Ķ����Ƿ���ڣ��������ڣ��򴴽��������ڣ������ͷ�
            if (m_sprite != null)
            {
                m_sprite.Dispose();
                m_sprite = null;
            }
            m_sprite = new Sprite(drawArgs.device);


            System.TimeSpan smallestRefreshInterval = System.TimeSpan.MaxValue;

            // ������е�Icon���������������ȥ��������Icon�Ŀ����
            foreach (RenderableObject ro in m_children)
            {
                Icon icon = ro as Icon;
                if (icon == null)
                {
                    // �жϵ�ǰIcon�Ƿ���ʾ��������ʾ����Continue��һ��
                    if (ro.IsOn)
                        ro.Initialize(drawArgs);
                    continue;
                }


                if (icon.RefreshInterval.TotalMilliseconds != 0 && icon.RefreshInterval != TimeSpan.MaxValue && icon.RefreshInterval < smallestRefreshInterval)
                    smallestRefreshInterval = icon.RefreshInterval;

                // ��Icon��ʼ��
                icon.Initialize(drawArgs);
                object key = null;
                //����һ��IconeTexture����
                IconTexture iconTexture = null;

                //���ļ��ж�ȡ��ǰIcon
                if (icon.TextureFileName != null && icon.TextureFileName.Length > 0)
                {

                    // �ӻ����ж�ȡIcon�������
                    iconTexture = (IconTexture)m_textures[icon.TextureFileName];
                    //�������в����ڣ�����ļ��ж�ȡIcon����
                    if (iconTexture == null)
                    {
                        key = icon.TextureFileName;
                        iconTexture = new IconTexture(drawArgs.device, icon.TextureFileName);
                    }
                }
                //��Bitmap�����ж�ȡIcon����
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
                //����Ȼû��������ѭ����һ��
                if (iconTexture == null)
                    continue;
                //��������Ļ�
                if (key != null)
                {
                    //������ŵ�������ȥ
                    m_textures.Add(key, iconTexture);

                    // ����Icon�Ŀ��
                    if (icon.Width == 0)
                        icon.Width = iconTexture.Width;
                    // ����Icon�ĸ߶�
                    if (icon.Height == 0)
                        icon.Height = iconTexture.Height;
                }
            }

            // ����Icon�İ�Χ��
            foreach (RenderableObject ro in m_children)
            {
                Icon icon = ro as Icon;
                if (icon == null)
                    continue;

                if (GetTexture(icon) == null)
                {
                    //�������ֵķ�Χ
                    icon.SelectionRectangle = drawArgs.defaultDrawingFont.MeasureString(null, icon.Name, DrawTextFormat.None, 0);
                }
                else
                {
                    //����ͼ��ķ�Χ
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
        /// �ͷ����еĶ���
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
        /// ������¼�
        /// </summary>
        /// <param name="drawArgs"></param>
        /// <returns></returns>
        public override bool PerformSelectionAction(DrawArgs drawArgs)
        {
            //ִ�����е���Icon��PerformSelectionAction����
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
                //�жϵ�ǰIcon��λ�ã��Ƿ�������Χ��,�������򷵻�
                if (!drawArgs.WorldCamera.ViewFrustum.ContainsPoint(icon.Position))
                    continue;

                //�ж�����λ���Ƿ���ͼ��İ�Χ���ڣ������ڣ��򷵻�
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
                    //����ǰ���������¼�
                    if (DrawArgs.IsLeftMouseButtonDown && !DrawArgs.IsRightMouseButtonDown)
                    {
                        //ʹ��������õ�ͼ�����õķ�Χ
                        if (icon.OnClickZoomAltitude != double.NaN || icon.OnClickZoomHeading != double.NaN || icon.OnClickZoomTilt != double.NaN)
                        {
                            drawArgs.WorldCamera.SetPosition(
                                icon.Latitude,
                                icon.Longitude,
                                icon.OnClickZoomHeading,
                                icon.OnClickZoomAltitude,
                                icon.OnClickZoomTilt);
                        }
                        //ִ�е�ǰIcon��PerformSelection����
                        if (icon.isSelectable)
                            icon.PerformSelectionAction(drawArgs);

                    }
                    //����ǰ������Ҽ��¼�
                    else if (!DrawArgs.IsLeftMouseButtonDown && DrawArgs.IsRightMouseButtonDown)
                    {
                        //�����κδ���
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
        /// ��Ⱦ����1.��Ⱦ���еķ�Icon����2.��Ⱦ���еķ�ѡ��״̬��Icon����3.��Ⱦѡ�е�Icon����
        /// </summary>
        /// <param name="drawArgs"></param>
        public override void Render(DrawArgs drawArgs)
        {
            if (!isOn)
                return;

            if (!isInitialized)
                return;

            // ��Ⱦ���еķ�Icon����
            foreach (RenderableObject ro in m_children)
            {
                if (!ro.IsOn)
                    continue;
                ro.Render(drawArgs);
            }

            int closestIconDistanceSquared = int.MaxValue;
            Icon closestIcon = null;

            // ��ʼ��Ⱦ���еĵķ�Iconͼ��
            m_sprite.Begin(SpriteFlags.AlphaBlend);
            foreach (RenderableObject ro in m_children)
            {
                if (!ro.IsOn)
                    continue;
                Icon icon = ro as Icon;
                if (icon == null)
                    continue;

                //����ͼ���λ��
                Vector3 translationVector = new Vector3(
                (float)(icon.PositionD.X - drawArgs.WorldCamera.ReferenceCenter.X),
                (float)(icon.PositionD.Y - drawArgs.WorldCamera.ReferenceCenter.Y),
                (float)(icon.PositionD.Z - drawArgs.WorldCamera.ReferenceCenter.Z));

                // ���������һ��Icon����
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
                //��Ⱦ���Ǳ�Hover��ͼ��
                if (icon != mouseOverIcon)
                    Render(drawArgs, icon, projectedPoint);
            }

            // ��Ⱦ��Hover��ͼ��
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
        /// ��Ⱦͼ��
        /// </summary>
        protected virtual void Render(DrawArgs drawArgs, Icon icon, Vector3 projectedPoint)
        {
            //�жϵ�ǰIcon�Ƿ��³�ʼ���ˣ����磬�����¶�����ͼ���λ�ã�������isInitialized����false
            if (!icon.isInitialized)
                icon.Initialize(drawArgs);
            //�жϵ�ǰͼ���Ƿ����ӵط�Χ�ڣ������ڣ��򲻽��л���
            if (!drawArgs.WorldCamera.ViewFrustum.ContainsPoint(icon.Position))
                return;

            // �ж�ͼ���Ƿ��������С�ɼ��ķ�Χ�ڣ������ڣ��򲻽��л���
            double distanceToIcon = Vector3.Length(icon.Position - drawArgs.WorldCamera.Position);
            if (distanceToIcon > icon.MaximumDisplayDistance)
                return;
            if (distanceToIcon < icon.MinimumDisplayDistance)
                return;
            //��õ�ǰͼ����������
            IconTexture iconTexture = GetTexture(icon);
            //�ж��Ƿ���MouseOver����
            bool isMouseOver = icon == mouseOverIcon;
            //����MouseOver���������Description�������
            if (isMouseOver)
            {
                //����MouseOver����
                isMouseOver = true;
                //����ǰͼ����Բ����������õ�ǰ�������Hand
                if (icon.isSelectable)
                    DrawArgs.MouseCursor = CursorType.Hand;
                ////��ʾ����������Ϣ,��ʱ����Ҫ
                //string description = icon.Description;
                //if(description==null)
                //    description = icon.ClickableActionURL;
                ////����������Ϣ
                //if(description!=null)
                //{
                //    //�������ֵĻ�������
                //    DrawTextFormat format = DrawTextFormat.NoClip | DrawTextFormat.WordBreak | DrawTextFormat.Bottom;
                //    int left = 10;
                //    if(World.Settings.showLayerManager)
                //        left += World.Settings.layerManagerWidth;
                //    Rectangle rect = Rectangle.FromLTRB(left, 10, drawArgs.screenWidth - 10, drawArgs.screenHeight - 10 );

                //    //���Ʊ߿�
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

                //    // ����������Ϣ
                //    rect.Offset(1,-1);
                //    drawArgs.defaultDrawingFont.DrawText(
                //        m_sprite, description,
                //        rect, 
                //        format, descriptionColor );
                //}
            }

            //��ȡ��ɫ
            int color = isMouseOver ? hotColor : normalColor;
            if (iconTexture == null || isMouseOver || icon.NameAlwaysVisible)
            {
                // ����ͼ�������
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

            //����ͼ��
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
        /// �ӻ����л�õ�ǰͼ�����������������ڣ��򷵻�null
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
        /// ˢ��ͼ�꼯��ͼ��
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
    /// һ��ͼ��ͼ��
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

        private Angle m_rotation = Angle.Zero;//��ת�ĽǶ�
        private bool m_isRotated = false;//Icon�Ƿ���ת
        private Point3d m_positionD = new Point3d();//Icon��λ��
        private bool m_nameAlwaysVisible = false;



        /// <summary>
        /// �����Icon��URL
        /// </summary>
        protected string m_clickableActionURL;

        /// <summary>
        /// ��Icon��γ��
        /// </summary>
        protected double m_latitude;

        /// <summary>
        /// ��Icon�ľ���
        /// </summary>
        protected double m_longitude;


        /// <summary>
        /// Icon�ĸ߶�
        /// </summary>
        public double Altitude;
        /// <summary>
        /// Icon���ļ���·��
        /// </summary>
        public string TextureFileName;

        /// <summary>
        /// Icon����Ӧ��ͼƬ����.
        /// </summary>
        public Bitmap Image;

        /// <summary>
        /// Icon�Ŀ��.
        /// </summary>
        public int Width;

        /// <summary>
        /// Icon�ĸ߶�
        /// </summary>
        public int Height;

        /// <summary>
        /// ���Icon�����ӵ�����ַ��ַ
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
        /// ����ܼ���
        /// </summary>
        public double MaximumDisplayDistance = double.MaxValue;

        /// <summary>
        /// ����ܼ���
        /// </summary>
        public double MinimumDisplayDistance;

        /// <summary>
        /// �ж�����Ƿ��ڴ˾��ͷ�Χ��
        /// </summary>
        public Rectangle SelectionRectangle;

        /// <summary>
        /// ����
        /// </summary>
        public double Latitude
        {
            get { return m_latitude; }
        }

        /// <summary>
        /// γ��
        /// </summary>
        public double Longitude
        {
            get { return m_longitude; }
        }

        /// <summary>
        /// ��תͼ��ĽǶ�
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
        /// �ж�ͼ���Ƿ���ת
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
        /// �Ƿ�һֱ��ʾIcon������
        /// </summary>
        public bool NameAlwaysVisible
        {
            get { return m_nameAlwaysVisible; }
            set { m_nameAlwaysVisible = value; }
        }

        /// <summary>
        /// ��ʼ��һ��Icon���� <see cref= "T:Qrst.Renderable.Icon"/> class 
        /// </summary>
        /// <param name="name">Icon��������</param>
        /// <param name="latitude"> γ��.</param>
        /// <param name="longitude">����.</param>
        public Icon(string name,
            double latitude,
            double longitude)
            : base(name)
        {
            //���þ�γ��
            m_latitude = latitude;
            m_longitude = longitude;
            this.RenderPriority = RenderPriority.Icons;
        }

        /// <summary>
        /// ��ʼ��һ��Icon���� <see cref= "T:Qrst.Renderable.Icon"/> class 
        /// </summary>
        /// <param name="name">Icon��������</param>
        /// <param name="latitude"> γ��.</param>
        /// <param name="longitude">����.</param>
        /// <param name="heightAboveSurface">Icon�ĸ߶�.</param>
        public Icon(string name,
            double latitude,
            double longitude,
            double heightAboveSurface)
            : base(name)
        {
            //���þ�γ��
            m_latitude = latitude;
            m_longitude = longitude;
            //���ø߶�
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
        /// ����Icon��λ��
        /// </summary>
        /// <param name="latitude"> γ��.</param>
        /// <param name="longitude">����.</param>
        public void SetPosition(double latitude, double longitude)
        {
            m_latitude = latitude;
            m_longitude = longitude;

            // ���¼���Icon�����꣬�ڳ�ʼ��������
            isInitialized = false;
        }

        /// <summary>
        /// ����Icon��λ��
        /// </summary>
        /// <param name="latitude"> γ��.</param>
        /// <param name="longitude">����.</param>
        /// <param name="altitude"> �߶�.</param>
        public void SetPosition(double latitude, double longitude, double altitude)
        {
            m_latitude = latitude;
            m_longitude = longitude;
            Altitude = altitude;

            // ���¼���Icon�����꣬�ڳ�ʼ��������
            isInitialized = false;
        }

        #region RenderableObject methods

        /// <summary>
        /// ��ʼ��Icon����
        /// </summary>
        /// <param name="drawArgs"></param>
        public override void Initialize(DrawArgs drawArgs)
        {
            double samplesPerDegree = 50.0 / (drawArgs.WorldCamera.ViewRange.Degrees);//���㵱ǰÿһ���ж��ٸ�����
            double elevation = drawArgs.CurrentWorld.TerrainAccessor.GetElevationAt(m_latitude, m_longitude, samplesPerDegree);//���㵱ǰ��γ�ȵĺ���ϵ��Ϣ
            double altitude = (World.Settings.VerticalExaggeration * Altitude + World.Settings.VerticalExaggeration * elevation);//����Icon��ʵ����ʾ�߶�=�������*Altitude+���θ߶�

            //ת��Ϊ��Ļ������Ϣ
            Position = MathEngine.SphericalToCartesian(m_latitude, m_longitude,
                altitude + drawArgs.WorldCamera.WorldRadius);

            //ת������Ļ����
            m_positionD = MathEngine.SphericalToCartesianD(
                Angle.FromDegrees(m_latitude),
                Angle.FromDegrees(m_longitude),
                altitude + drawArgs.WorldCamera.WorldRadius);

            isInitialized = true;
        }

        /// <summary>
        /// �ͷ�Icon����
        /// </summary>
        public override void Dispose()
        {

        }

        ///// <summary>
        ///// ����¼�
        ///// </summary>
        ///// <param name="drawArgs"></param>
        ///// <returns></returns>
        //public override bool PerformSelectionAction(DrawArgs drawArgs)
        //{
        //    return false;
        //}

        /// <summary>
        /// ��¼���ϴε���ͼ����
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
    /// �ر�ͼ��
    /// </summary>
    public class PlaceIcon : Icon
    {
        /// <summary>
        /// ��ʼ��һ��Icon���� <see cref= "T:Qrst.Renderable.Icon"/> class 
        /// </summary>
        /// <param name="name">Icon��������</param>
        /// <param name="latitude"> γ��.</param>
        /// <param name="longitude">����.</param>
        public PlaceIcon(string name,
            double latitude,
            double longitude)
            : base(name, latitude, longitude)
        {
        }

        /// <summary>
        /// ��ʼ��һ��Icon���� <see cref= "T:Qrst.Renderable.Icon"/> class 
        /// </summary>
        /// <param name="name">Icon��������</param>
        /// <param name="latitude"> γ��.</param>
        /// <param name="longitude">����.</param>
        /// <param name="heightAboveSurface">Icon�ĸ߶�.</param>
        public PlaceIcon(string name,
            double latitude,
            double longitude,
            double heightAboveSurface)
            : base(name, latitude, longitude, heightAboveSurface)
        {

        }

        ///// <summary>
        ///// ����¼�
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
