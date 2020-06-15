using System;
using System.Diagnostics;
using System.Collections;
using System.IO;
using System.Net;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Qrst.Camera;
using Qrst;
using Qrst.Net;
using Utility;

namespace Qrst
{
    /// <summary>
    /// ����Ⱦ��������
    /// </summary>
    public class DrawArgs : IDisposable
    {
        /// <summary>
        /// D3D�豸����
        /// </summary>
        public Device device;
        /// <summary>
        /// ���������
        /// </summary>
        public System.Windows.Forms.Control parentControl;
        /// <summary>
        /// ��̬������
        /// </summary>
        public static System.Windows.Forms.Control ParentControl = null;
        /// <summary>
        /// �߽����
        /// </summary>
        public int numBoundaryPointsTotal;
        /// <summary>
        /// Ҫ����Ⱦ�ı߽����
        /// </summary>
        public int numBoundaryPointsRendered;
        public int numBoundariesDrawn;

        /// <summary>
        /// Ĭ�ϵ�3D����
        /// </summary>
        public Font defaultDrawingFont;
        /// <summary>
        /// Icon�����ֵ�����
        /// </summary>
        public Font iconNameFont;


        /// <summary>
        /// ��Ļ���
        /// </summary>
        public int screenWidth;
        /// <summary>
        /// ��Ļ�߶�
        /// </summary>
        public int screenHeight;
        /// <summary>
        /// ���һ������������Ļ�����λ��
        /// </summary>
        public static System.Drawing.Point LastMousePosition;
        /// <summary>
        /// ������ʾ�����Tile
        /// </summary>
        public int numberTilesDrawn;
        /// <summary>
        /// ��ǰ�������Ļ����λ�á�
        /// </summary>
        public System.Drawing.Point CurrentMousePosition;
        /// <summary>
        /// ���Ͻǵ��ı�����
        /// </summary>
        public string UpperLeftCornerText = "";

        /// <summary>
        /// ���������
        /// </summary>
        CameraBase m_WorldCamera;
        /// <summary>
        /// ��ǰ�������
        /// </summary>
        public World m_CurrentWorld = null;
        /// <summary>
        /// ��¼���Ƿ��ǰ����˵������
        /// </summary>
        public static bool IsLeftMouseButtonDown = false;
        /// <summary>
        /// ��¼���Ƿ��ǰ����˵�����ҽ�
        /// </summary>
        public static bool IsRightMouseButtonDown = false;
        /// <summary>
        /// ���ض��ж���
        /// </summary>
        public static DownloadQueue DownloadQueue = new DownloadQueue();
        /// <summary>
        /// �����뵱ǰ�����������
        /// </summary>
        public int TexturesLoadedThisFrame = 0;
        private static System.Drawing.Bitmap bitmap;
        public static System.Drawing.Graphics Graphics = null;
        public bool RenderWireFrame = false;


        /// <summary>
        /// �����뵱ǰ����Χ�ڵ������������
        /// </summary>
        protected static Hashtable m_textures = new Hashtable();
        /// <summary>
        /// ��ȡ�����뵱ǰ����Χ�ڵ������������
        /// </summary>
        public static Hashtable Textures
        {
            get { return m_textures; }
        }
        /// <summary>
        /// ��ȡ˵�������������
        /// </summary>
        public static CameraBase Camera = null;
        /// <summary>
        /// ��ȡ���������������
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
        /// ��ȡ�����õ�ǰ�������
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
        /// ��ǰ��ʼ��ʱ�ľ���ʱ��
        /// </summary>
        public static long CurrentFrameStartTicks;

        /// <summary>
        /// �ܹ���ʱ�����ľ���ʱ��
        /// </summary>
        public static float LastFrameSecondsElapsed;

        /// <summary>
        /// ������ʽ
        /// </summary>
        static CursorType mouseCursor;
        /// <summary>
        /// �ϴβ���������ʽ
        /// </summary>
        static CursorType lastCursor;

        /// <summary>
        /// �Ƿ�������
        /// </summary>
        bool repaint = true;
        /// <summary>
        /// �Ƿ��������
        /// </summary>
        bool isPainting;

        /// <summary>
        /// ����������洢�����еĻ�������
        /// </summary>
        Hashtable fontList = new Hashtable();

        /// <summary>
        /// D3D����������
        /// </summary>
        public static Device Device = null;

        /// <summary>
        /// ����ʱ������״
        /// </summary>
        System.Windows.Forms.Cursor measureCursor;


        /// <summary>
        /// ��ʼ��һ����Ⱦ�������� <see cref= "T:Qrst.DrawArgs"/> class.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="parentForm"></param>
        public DrawArgs(Device device, System.Windows.Forms.Control parentForm)
        {
            //����ȱ����˾�̬�ģ��ֱ����˶��������
            this.parentControl = parentForm; //�󶨸����壨Ҫ���л��ƵĴ��壩
            DrawArgs.ParentControl = parentForm;//�󶨸���̬����
            DrawArgs.Device = device;//�� ������ ����
            this.device = device;

            //��WorldSetting�У���ȡĬ�ϻ����������ʽ
            defaultDrawingFont = CreateFont(World.Settings.defaultFontName, World.Settings.defaultFontSize);
            //�������ڣ����½�һ��΢���ź�����
            if (defaultDrawingFont == null)
                defaultDrawingFont = CreateFont("΢���ź�", 10, System.Drawing.FontStyle.Bold);
            //����ͼ����������
            iconNameFont = CreateFont("΢���ź�", 10, System.Drawing.FontStyle.Bold);

            //����Ҫ���л��Ƶ���档
            bitmap = new System.Drawing.Bitmap(256, 256, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            DrawArgs.Graphics = System.Drawing.Graphics.FromImage(bitmap);

        }

        /// <summary>
        /// ��ʼ׼�����л���
        /// </summary>
        public void BeginRender()
        {
            //����Ⱦ֮ǰ�Ƚ��г�ʼ��
            this.numberTilesDrawn = 0;
            this.TexturesLoadedThisFrame = 0;
            this.UpperLeftCornerText = "";
            this.numBoundaryPointsRendered = 0;
            this.numBoundaryPointsTotal = 0;
            this.numBoundariesDrawn = 0;
            this.isPainting = true;
        }
        /// <summary>
        /// ��������
        /// </summary>
        public void EndRender()
        {
            this.isPainting = false;
        }

        /// <summary>
        /// ��ʾRender���ͼ��զEndRender����֮�����.
        /// </summary>
        public void Present()
        {
            //��ʾҪ֮ǰ����õ�Ҫ������Ⱦ�Ķ���
            device.Present();
        }

        #region ��������
        /// <summary>
        /// ��������
        /// </summary>
        public Font CreateFont(string familyName, float emSize)
        {
            return CreateFont(familyName, emSize, System.Drawing.FontStyle.Regular);
        }

        /// <summary>
        /// ��������
        /// </summary>
        public Font CreateFont(string familyName, float emSize, System.Drawing.FontStyle style)
        {
            try
            {
                FontDescription description = new FontDescription();
                description.FaceName = familyName;
                description.Height = (int)(1.9 * 10);

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
                return defaultDrawingFont;
            }
        }

        /// <summary>
        /// ��������
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

                return defaultDrawingFont;
            }
        }
        #endregion


        /// <summary>
        /// ������״��
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
        /// ���¸�����������״
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
                        measureCursor = ImageHelper.LoadCursor("measure.cur");
                    parent.Cursor = measureCursor;
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
        /// ��ȡ�Ƿ����ڽ��л���
        /// </summary>
        public bool IsPainting
        {
            get
            {
                return this.isPainting;
            }
        }
        /// <summary>
        /// ��ȡ�������Ƿ�������»���
        /// </summary>
        public bool Repaint
        {
            get
            {
                return this.repaint;
            }
            set
            {
                this.repaint = value;
            }
        }

        #region IDisposable Members

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

    /// <summary>
    /// �����ʽ
    /// </summary>
    public enum CursorType
    {
        Arrow = 0,
        Hand,
        Cross,
        Measure,
        SizeWE,
        SizeNS,
        SizeNESW,
        SizeNWSE
    }
}
