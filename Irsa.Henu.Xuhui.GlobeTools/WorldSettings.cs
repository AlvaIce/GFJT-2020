using Microsoft.DirectX.Direct3D;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Xml.Serialization;

namespace Qrst
{

    /// <summary>
    /// ������������
    /// </summary>
    public class WorldSettings
    {
        #region ��������
        /// <summary>
        /// ��ȡ������ʱ����ٱ�
        /// </summary>
        public float TimeMultiplier
        {
            get { return TimeKeeper.TimeMultiplier; }
            set { TimeKeeper.TimeMultiplier = value; }
        }

        internal int maxSimultaneousDownloads = 1;
        /// <summary>
        /// ��ȡ���������ͬʱ���ظ���
        /// </summary>
        public int MaxSimultaneousDownloads
        {
            get { return maxSimultaneousDownloads; }
            set
            {
                if (value > 20)
                    maxSimultaneousDownloads = 20;
                else if (value < 1)
                    maxSimultaneousDownloads = 1;
                else
                    maxSimultaneousDownloads = value;
            }
        }

        private TimeSpan terrainTileRetryInterval = TimeSpan.FromMinutes(30);
        /// <summary>
        /// ��ȡ���������ָ߳�ͼ�����������ʱ��.��С��1����.
        /// </summary>
        public TimeSpan TerrainTileRetryInterval
        {
            get
            {
                return terrainTileRetryInterval;
            }
            set
            {
                TimeSpan minimum = TimeSpan.FromMinutes(1);
                if (value < minimum)
                    value = minimum;
                terrainTileRetryInterval = value;
            }
        }

        internal int downloadQueuedColor = Color.FromArgb(50, 128, 168, 128).ToArgb();
        /// <summary>
        /// ��ȡ�������������ݵĽ��Ȳ۵���ɫ.
        /// </summary>
        public Color DownloadQueuedColor
        {
            get { return Color.FromArgb(downloadQueuedColor); }
            set { downloadQueuedColor = value.ToArgb(); }
        }

        private Units m_displayUnits = Units.Metric;
        /// <summary>
        /// ��ȡ�����ü�����λ��Ĭ������
        /// </summary>
        public Units DisplayUnits
        {
            get
            {
                return m_displayUnits;
            }
            set
            {
                m_displayUnits = value;
            }
        }

        /// <summary>
        /// ����Log404����
        /// </summary>
        public bool Log404Errors
        {
            get { return Qrst.Net.WebDownload.Log404Errors; }
            set { Qrst.Net.WebDownload.Log404Errors = value; }
        }

        #endregion

        #region ����ɢ��Ч������
        internal bool enableAtmosphericScattering = false;
        /// <summary>
        /// ���ԣ��Ƿ��������ɢ��Ч��.
        /// </summary>
        public bool EnableAtmosphericScattering
        {
            get { return enableAtmosphericScattering; }

            set { enableAtmosphericScattering = value; }
        }

        internal bool forceCpuAtmosphere = true;
        /// <summary>
        /// ���ԣ��Ƿ�ǿ��CPU���GPU�������ɢ��.
        /// </summary>
        public bool ForceCpuAtmosphere
        {
            get { return forceCpuAtmosphere; }
            set { forceCpuAtmosphere = value; }
        }

        #endregion

        #region Ĭ����������||Tocbar����||����ͼ����ɫ����|

        #region Ĭ����������
        /// <summary>
        /// Ĭ����������
        /// </summary>
        internal string defaultFontName = "Tahoma";

        /// <summary>
        /// Ĭ�������С
        /// </summary>
        internal float defaultFontSize = 9.0f;

        /// <summary>
        /// Ĭ�������ʽ���Ӵ֡�б�ߡ�����
        /// </summary>
        internal FontStyle defaultFontStyle = FontStyle.Regular;

        #endregion

        #region Tocbar����

        /// <summary>
        /// ��ʾTocBar
        /// </summary>
        public bool showLayerManager = false;
        /// <summary>
        /// Toc��������ʾLayer������
        /// </summary>
        internal string layerManagerFontName = "΢���ź�";

        /// <summary>
        /// Toc��������ʾLayer������Ĵ�С
        /// </summary>
        internal float layerManagerFontSize = 10;

        /// <summary>
        /// Toc��������ʾLayer���������ʽ���Ӵ֡�б�ߡ�����
        /// </summary>
        internal FontStyle layerManagerFontStyle = FontStyle.Regular;
        /// <summary>
        /// Toc����Ĵ�С
        /// </summary>
        internal int layerManagerWidth = 300;

        /// <summary>
        /// TocBar�ı���ɫ
        /// </summary>
        internal int menuBackColor = Color.FromArgb(150, 40, 40, 40).ToArgb();
        /// <summary>
        /// Tocbar�ı߿�
        /// </summary>
        internal int menuOutlineColor = Color.FromArgb(200, 132, 157, 189).ToArgb();
        /// <summary>
        /// Tocbar��Scrollbar����ɫ
        /// </summary>
        internal int scrollbarColor = System.Drawing.Color.FromArgb(200, 132, 157, 189).ToArgb();
        /// <summary>
        /// Tocbar��Scrollbar�����к����ɫ
        /// </summary>
        internal int scrollbarHotColor = System.Drawing.Color.FromArgb(170, 255, 255, 255).ToArgb();

        #endregion

        /// <summary>
        /// ��ʾ����ʮ�ּ�
        /// </summary>
        public bool showCrosshairs = false;
        /// <summary>
        /// ƽ���������ã��Ƿ���ʾƽ������
        /// </summary>
        internal bool antiAliasedText = false;
        /// <summary>
        /// ���ÿ��ˢ�´�����Ĭ����50��
        /// </summary>
        internal int throttleFpsHz = 50;
        /// <summary>
        /// �Ƿ���ʾ����ͼ��
        /// </summary>
        internal bool showDownloadIndicator = true;


        internal int downloadTerrainRectangleColor = Color.FromArgb(50, 0, 0, 255).ToArgb();
        internal int downloadProgressColor = Color.FromArgb(50, 255, 0, 0).ToArgb();
        internal int downloadLogoColor = Color.FromArgb(180, 255, 255, 255).ToArgb();

        /// <summary>
        /// �Ƿ���ʾ����ͼ��
        /// </summary>
        public bool ShowDownloadIndicator
        {
            get { return showDownloadIndicator; }
            set { showDownloadIndicator = value; }
        }
        /// <summary>
        /// Tocbar��Scrollbar�����к����ɫ
        /// </summary>
        public Color ScrollbarHotColor
        {
            get { return Color.FromArgb(scrollbarHotColor); }
            set { scrollbarHotColor = value.ToArgb(); }
        }
        /// <summary>
        /// Tocbar��Scrollbar����ɫ
        /// </summary>
        public Color ScrollbarColor
        {
            get { return Color.FromArgb(scrollbarColor); }
            set { scrollbarColor = value.ToArgb(); }
        }
        /// <summary>
        /// Tocbar�ı߿�
        /// </summary>
        public Color MenuOutlineColor
        {
            get { return Color.FromArgb(menuOutlineColor); }
            set { menuOutlineColor = value.ToArgb(); }
        }
        /// <summary>
        /// TocBar�ı���ɫ
        /// </summary>
        public Color MenuBackColor
        {
            get { return Color.FromArgb(menuBackColor); }
            set { menuBackColor = value.ToArgb(); }
        }
        /// <summary>
        /// ����Logo����ɫ
        /// </summary>
        public Color DownloadLogoColor
        {
            get { return Color.FromArgb(downloadLogoColor); }
            set { downloadLogoColor = value.ToArgb(); }
        }
        /// <summary>
        /// ����Logo���ȵ���ɫ
        /// </summary>
        public Color DownloadProgressColor
        {
            get { return Color.FromArgb(downloadProgressColor); }
            set { downloadProgressColor = value.ToArgb(); }
        }
        /// <summary>
        /// ���ظ߳̽��ȵ���ɫ
        /// </summary>
        public Color DownloadTerrainRectangleColor
        {
            get { return Color.FromArgb(downloadTerrainRectangleColor); }
            set { downloadTerrainRectangleColor = value.ToArgb(); }
        }
        /// <summary>
        /// ��ȡ��������ʾTocBar
        /// </summary>
        public bool ShowLayerManager
        {
            get { return showLayerManager; }
            set { showLayerManager = value; }
        }
        /// <summary>
        /// ��ȡ��������ʾ������ʮ��
        /// </summary>
        public bool ShowCrosshairs
        {
            get { return showCrosshairs; }
            set { showCrosshairs = value; }
        }
        /// <summary>
        /// ��ȡ������Ĭ�����������
        /// </summary>
        public string DefaultFontName
        {
            get { return defaultFontName; }
            set { defaultFontName = value; }
        }
        /// <summary>
        /// ��ȡ������Ĭ������Ĵ�С
        /// </summary>
        public float DefaultFontSize
        {
            get { return defaultFontSize; }
            set { defaultFontSize = value; }
        }
        /// <summary>
        /// ��ȡ������Ĭ���������ʽ���Ӵ�|��б|����
        /// </summary>
        public FontStyle DefaultFontStyle
        {
            get { return defaultFontStyle; }
            set { defaultFontStyle = value; }
        }
        /// <summary>
        /// ��ȡ������Tocbar����������
        /// </summary>
        public string LayerManagerFontName
        {
            get { return layerManagerFontName; }
            set { layerManagerFontName = value; }
        }
        /// <summary>
        /// ��ȡ������Tocbar����Ĵ�С
        /// </summary>
        public float LayerManagerFontSize
        {
            get { return layerManagerFontSize; }
            set { layerManagerFontSize = value; }
        }
        /// <summary>
        /// ��ȡ������Tocbar�������ʽ���Ӵ�|��б|����
        /// </summary>
        public FontStyle LayerManagerFontStyle
        {
            get { return layerManagerFontStyle; }
            set { layerManagerFontStyle = value; }
        }
        /// <summary>
        /// ��ȡ������Tocbar�Ŀ��
        /// </summary>
        public int LayerManagerWidth
        {
            get { return layerManagerWidth; }
            set { layerManagerWidth = value; }
        }
        /// <summary>
        /// ��ȡ�������Ƿ�ƽ������
        /// </summary>
        public bool AntiAliasedText
        {
            get { return antiAliasedText; }
            set { antiAliasedText = value; }
        }
        #endregion

        #region ��γ�ȸ�������||�ռ���Ϣ��ʾ����

        /// <summary>
        /// �Ƿ���ʾ��γ�����ߣ���ʼΪ��.
        /// </summary>
        public bool showLatLonLines = false;

        /// <summary>
        /// ��γ�ߵ���ɫ.
        /// </summary>
        public int latLonLinesColor = System.Drawing.Color.FromArgb(200, 160, 160, 160).ToArgb();

        /// <summary>
        /// �������ɫ.
        /// </summary>
        public int equatorLineColor = System.Drawing.Color.FromArgb(230, 255, 199, 183).ToArgb();

        /// <summary>
        /// �Ƿ񻭻ع���
        /// </summary>
        internal bool showTropicLines = true;

        /// <summary>
        /// �ع��ߵ���ɫ
        /// </summary>
        public int tropicLinesColor = System.Drawing.Color.FromArgb(230, 125, 150, 160).ToArgb();

        /// <summary>
        /// ��ȡ�������Ƿ���ʾ��γ�ȸ���
        /// </summary>
        public bool ShowLatLonLines
        {
            get { return showLatLonLines; }
            set { showLatLonLines = value; }
        }
        /// <summary>
        /// ��ȡ�����þ�γ�ȸ�����ɫ
        /// </summary>
        public Color LatLonLinesColor
        {
            get { return Color.FromArgb(latLonLinesColor); }
            set { latLonLinesColor = value.ToArgb(); }
        }
        /// <summary>
        /// ��ȡ�����þ�γ�ȸ��������ɫ
        /// </summary>
        public Color EquatorLineColor
        {
            get { return Color.FromArgb(equatorLineColor); }
            set { equatorLineColor = value.ToArgb(); }
        }
        /// <summary>
        /// ��ȡ�������Ƿ���ʾγ�ȸ����ع���
        /// </summary>
        public bool ShowTropicLines
        {
            get { return showTropicLines; }
            set { showTropicLines = value; }
        }
        /// <summary>
        /// ��ȡ������γ�ȸ����ع��ߵ���ɫ
        /// </summary>
        public Color TropicLinesColor
        {
            get { return Color.FromArgb(tropicLinesColor); }
            set { tropicLinesColor = value.ToArgb(); }
        }

        /// <summary>
        /// �Ƿ���ʾλ����Ϣ
        /// </summary>
        public bool showPosition = true;
        /// <summary>
        /// ��ȡ�����ÿռ���Ϣ
        /// </summary>
        public bool ShowPosition
        {
            get { return showPosition; }
            set { showPosition = value; }
        }
        #endregion

        #region ������Ĳ�������
        /// <summary>
        /// �������γ��
        /// </summary>
        internal Angle cameraLatitude = Angle.FromDegrees(30.0);
        /// <summary>
        /// ������ľ���
        /// </summary>
        internal Angle cameraLongitude = Angle.FromDegrees(110.0);
        /// <summary>
        /// ������߶� ��ʼֵΪ10000000��
        /// </summary>
        internal double cameraAltitudeMeters = 10000000;
        /// <summary>
        /// ��������
        /// </summary>
        internal Angle cameraHeading = Angle.FromDegrees(0.0);
        /// <summary>
        /// ��������
        /// </summary>
        internal Angle cameraTilt = Angle.FromDegrees(0.0);

        /// <summary>
        /// �������������ƶ�
        /// </summary>
        internal bool cameraIsPointGoto = false;
        /// <summary>
        /// ������Ƿ��ж���,��ΪTrue������ƶ�����󣬵���ͻᳯ���Ǹ�����һֱת
        /// </summary>
        internal bool cameraHasMomentum = false;

        #region ����ƶ���������һ��΢����ת�����ԣ����������Ա��붼����ΪTrue����
        /// <summary>
        /// ������Ƿ��й���.
        /// </summary>
        internal bool cameraHasInertia = true;
        /// <summary>
        /// ������Ƿ�ƽ��
        /// </summary>
        internal bool cameraSmooth = true;

        /// <summary>
        /// ����������ƶ��ı�׼ֵ
        /// </summary>
        internal float cameraSlerpStandard = 0.35f;
        /// <summary>
        /// ������������ƶ�ֵ
        /// </summary>
        internal float cameraSlerpInertia = 0.05f;
        /// <summary>
        /// ������������ƶ�%
        /// </summary>
        internal float cameraSlerpPercentage = 0.05f;
        #endregion

        #region �ӳ�������
        /// <summary>
        /// �����˲ʱ�ӳ���
        /// </summary>
        internal Angle cameraFov = Angle.FromRadians(Math.PI * 0.25f);
        /// <summary>
        /// �������С�ӳ���  ��ʼΪ5
        /// </summary>
        internal Angle cameraFovMin = Angle.FromDegrees(5);
        /// <summary>
        /// ���������ӳ�  ��ʼΪ150
        /// </summary>
        internal Angle cameraFovMax = Angle.FromDegrees(150);

        #endregion

        #region �����ZoomIn��ZoomOut���ٶ�

        /// <summary>
        /// ������Ŵ���Сϵ���� ��ʼ��Ϊ 0.015
        /// </summary>
        internal float cameraZoomStepFactor = 0.02f;
        /// <summary>
        /// ������Ŵ���С���ٱ���  ��ʼΪ10
        /// </summary>
        internal float cameraZoomAcceleration = 10f;

        #endregion

        internal float cameraRotationSpeed = 3.5f;
        internal bool elevateCameraLookatPoint = true;

        /// <summary>
        /// û�о���ʲô��
        /// </summary>
        public bool ElevateCameraLookatPoint
        {
            get { return elevateCameraLookatPoint; }
            set { elevateCameraLookatPoint = value; }
        }
        /// <summary>
        /// ��ȡ������������ľ���
        /// </summary>
        public Angle CameraLatitude
        {
            get { return cameraLatitude; }
            set { cameraLatitude = value; }
        }
        /// <summary>
        /// ��ȡ�������������γ��
        /// </summary>
        public Angle CameraLongitude
        {
            get { return cameraLongitude; }
            set { cameraLongitude = value; }
        }
        /// <summary>
        /// ��ȡ������������ĸ߶�
        /// </summary>
        public double CameraAltitude
        {
            get { return cameraAltitudeMeters; }
            set { cameraAltitudeMeters = value; }
        }
        /// <summary>
        /// ��ȡ������������ķ�ת��
        /// </summary>
        public Angle CameraHeading
        {
            get { return cameraHeading; }
            set { cameraHeading = value; }
        }
        /// <summary>
        /// ��ȡ���������������б��
        /// </summary>
        public Angle CameraTilt
        {
            get { return cameraTilt; }
            set { cameraTilt = value; }
        }
        /// <summary>
        /// ��ȡ�������Ƿ����������ƶ���
        /// </summary>
        public bool CameraIsPointGoto
        {
            get { return cameraIsPointGoto; }
            set { cameraIsPointGoto = value; }
        }
        /// <summary>
        /// ��ȡ�������Ƿ�ʹ���ƽ��
        /// </summary>
        public bool CameraSmooth
        {
            get { return cameraSmooth; }
            set { cameraSmooth = value; }
        }
        /// <summary>
        /// ��ȡ������������Ƿ��й������ƶ�
        /// </summary>
        public bool CameraHasInertia
        {
            get { return cameraHasInertia; }
            set
            {
                cameraHasInertia = value;
                cameraSlerpPercentage = cameraHasInertia ? cameraSlerpInertia : cameraSlerpStandard;
            }
        }
        /// <summary>
        /// ��ȡ���������������ƶ���һֱ�ƶ�
        /// </summary>
        public bool CameraHasMomentum
        {
            get { return cameraHasMomentum; }
            set { cameraHasMomentum = value; }
        }
        /// <summary>
        /// ��ȡ���������ƶ��Ĺ���
        /// </summary>
        public float CameraSlerpInertia
        {
            get { return cameraSlerpInertia; }
            set
            {
                cameraSlerpInertia = value;
                if (cameraHasInertia)
                    cameraSlerpPercentage = cameraSlerpInertia;
            }
        }
        /// <summary>
        /// ��ȡ���������ƶ��Ĺ����ı�׼ֵ
        /// </summary>
        public float CameraSlerpStandard
        {
            get { return cameraSlerpStandard; }
            set
            {
                cameraSlerpStandard = value;
                if (!cameraHasInertia)
                    cameraSlerpPercentage = cameraSlerpStandard;
            }
        }
        /// <summary>
        /// ��ȡ�������������Fovֵ
        /// </summary>
        public Angle CameraFov
        {
            get { return cameraFov; }
            set { cameraFov = value; }
        }
        /// <summary>
        /// ��ȡ���������������СFOVֵ
        /// </summary>
        public Angle CameraFovMin
        {
            get { return cameraFovMin; }
            set { cameraFovMin = value; }
        }
        /// <summary>
        /// ��ȡ����������������FOVֵ
        /// </summary>
        public Angle CameraFovMax
        {
            get { return cameraFovMax; }
            set { cameraFovMax = value; }
        }
        /// <summary>
        /// ��ȡ�����������Zoom������
        /// </summary>
        public float CameraZoomStepFactor
        {
            get { return cameraZoomStepFactor; }
            set
            {
                const float maxValue = 0.3f;
                const float minValue = 1e-4f;

                if (value >= maxValue)
                    value = maxValue;
                if (value <= minValue)
                    value = minValue;
                cameraZoomStepFactor = value;
            }
        }
        /// <summary>
        /// ��ȡ�����������Zoom�����ʵļ��ٱ���
        /// </summary>
        public float CameraZoomAcceleration
        {
            get { return cameraZoomAcceleration; }
            set
            {
                const float maxValue = 50f;
                const float minValue = 1f;

                if (value >= maxValue)
                    value = maxValue;
                if (value <= minValue)
                    value = minValue;

                cameraZoomAcceleration = value;
            }
        }

        float m_cameraDoubleClickZoomFactor = 2.0f;
        /// <summary>
        /// ��ȡ��������������˫��Zoom��ֵ
        /// </summary>
        public float CameraDoubleClickZoomFactor
        {
            get { return m_cameraDoubleClickZoomFactor; }
            set
            {
                m_cameraDoubleClickZoomFactor = value;
            }
        }
        /// <summary>
        /// ��ȡ�������������ת���ٶ�
        /// </summary>
        public float CameraRotationSpeed
        {
            get { return cameraRotationSpeed; }
            set { cameraRotationSpeed = value; }
        }

        #endregion

        #region ��Ⱦ����������||̫������.

        /// <summary>
        ///  �����ʽ,Ĭ��ΪDDS��ʽ
        /// </summary>
        private Format textureFormat = Format.Dxt3;
        /// <summary>
        /// �Ƿ�ʹ��С���������ȼ��ĸ����߳�   ��ʼΪ��
        /// </summary>
        private bool m_UseBelowNormalPriorityUpdateThread = false;
        /// <summary>
        /// �Ƿ�������Ⱦ����   ��ʼΪ��
        /// </summary>
        private bool m_AlwaysRenderWindow = false;
        /// <summary>
        /// �Ƿ����ص�ͼ��ת��ΪDDS��ʽ
        /// </summary>
        private bool convertDownloadedImagesToDds = true;
        /// <summary>
        /// ��ȡ�������Ƿ�ͼ��ת��ΪDDS��ʽ�ļ�
        /// </summary>
        public bool ConvertDownloadedImagesToDds
        {
            get
            {
                return convertDownloadedImagesToDds;
            }
            set
            {
                convertDownloadedImagesToDds = value;
            }
        }
        /// <summary>
        /// ��ȡ�������Ƿ�һֱ��ȾWindow
        /// </summary>
        public bool AlwaysRenderWindow
        {
            get
            {
                return m_AlwaysRenderWindow;
            }
            set
            {
                m_AlwaysRenderWindow = value;
            }
        }
        /// <summary>
        /// ��ȡ����������ĸ�ʽ
        /// </summary>
        public Format TextureFormat
        {
            get
            {
                //	return Format.Dxt3;
                return textureFormat;
            }
            set
            {
                textureFormat = value;
            }
        }
        /// <summary>
        /// ��ȡ�������Ƿ�ʹ�����������ȼ����µĸ����߳�
        /// </summary>
        public bool UseBelowNormalPriorityUpdateThread
        {
            get
            {
                return m_UseBelowNormalPriorityUpdateThread;
            }
            set
            {
                m_UseBelowNormalPriorityUpdateThread = value;
            }
        }

        #region ̫����Ϣ����
        /// <summary>
        /// �Ƿ��ܹ���ʾ̫����Ӱ
        /// </summary>
        private bool m_enableSunShading = true;
        /// <summary>
        /// ��ȡ�������Ƿ���ʾ̫����Ӱ
        /// </summary>
        public bool EnableSunShading
        {
            get
            {
                return m_enableSunShading;
            }
            set
            {
                m_enableSunShading = value;
            }
        }

        /// <summary>
        /// ʱ���̫��ͬ�� ��ʾ
        /// </summary>
        private bool m_sunSynchedWithTime = true;
        /// <summary>
        /// ��ȡ������ʱ���̫���Ƿ�ͬ��
        /// </summary>
        public bool SunSynchedWithTime
        {
            get
            {
                return m_sunSynchedWithTime;
            }
            set
            {
                m_sunSynchedWithTime = value;
            }
        }
        /// <summary>
        /// ̫������.
        /// </summary>
        private double m_sunElevation = Math.PI / 4;
        /// <summary>
        /// ��ȡ������̫������.��̫��λ����ʱ�䲻ͬ��ʱ.Ĭ��Ϊ : Math.PI / 4.
        /// </summary>
        public double SunElevation
        {
            get
            {
                return m_sunElevation;
            }
            set
            {
                m_sunElevation = value;
            }
        }
        /// <summary>
        /// ̫�����ֵ
        /// </summary>
        private double m_sunHeading = -Math.PI / 4;
        /// <summary>
        /// ��ȡ������̫�����ֵ
        /// </summary>
        public double SunHeading
        {
            get
            {
                return m_sunHeading;
            }
            set
            {
                m_sunHeading = value;
            }
        }
        /// <summary>
        /// ̫���������ľ���.̫���������1.5��ǧ��
        /// </summary>
        private double m_sunDistance = 150000000000;
        /// <summary>
        /// ��ȡ������̫���������ľ���.(��)
        /// </summary>
        public double SunDistance
        {
            get
            {
                return m_sunDistance;
            }
            set
            {
                m_sunDistance = value;
            }
        }

        /// <summary>
        /// ̫����Ӱ��Χ������ɫ
        /// </summary>
        private int m_shadingAmbientColor = System.Drawing.Color.FromArgb(50, 50, 50).ToArgb();
        /// <summary>
        /// ��ȡ������̫����Ӱ��Χ������ɫ
        /// </summary>
        public System.Drawing.Color ShadingAmbientColor
        {
            get
            {
                return System.Drawing.Color.FromArgb(m_shadingAmbientColor);
            }
            set
            {
                m_shadingAmbientColor = value.ToArgb();
            }
        }
        /// <summary>
        /// ��׼��Χ������ɫ
        /// </summary>
        private int m_standardAmbientColor = System.Drawing.Color.FromArgb(64, 64, 64).ToArgb();
        /// <summary>
        /// ��ȡ������̫����׼��Χ������ɫ
        /// </summary>
        public System.Drawing.Color StandardAmbientColor
        {
            get
            {
                return System.Drawing.Color.FromArgb(m_standardAmbientColor);
            }
            set
            {
                m_standardAmbientColor = value.ToArgb();
            }
        }

        #endregion


        #endregion

        #region �߳�������Ϣ������.

        private float minSamplesPerDegree = 3.0f;
        /// <summary>
        /// ��ȡ������ÿ�ȵ���С������
        /// </summary>
        public float MinSamplesPerDegree
        {
            get
            {
                return minSamplesPerDegree;
            }
            set
            {
                minSamplesPerDegree = value;
            }
        }

        //��ʱ������
        private bool useWorldSurfaceRenderer = true;
        /// <summary>
        /// ��ȡ�������Ƿ�ʹ�õ������Ķ��ֵ��ο��ӻ���Ⱦӳ��㣬��ʼΪ��.����ʱ�����ף�
        /// </summary>
        public bool UseWorldSurfaceRenderer
        {
            get
            {
                return useWorldSurfaceRenderer;
            }
            set
            {
                useWorldSurfaceRenderer = value;
            }
        }


        private float verticalExaggeration = 3.0f;
        /// <summary>
        /// ��ȡ�����ÿ������,��,ˮƽ�봹ֱ�ı�. ���ﷶΧ��:[1��20].���������Χ,���׳��쳣.
        /// </summary>
        public float VerticalExaggeration
        {
            get
            {
                return verticalExaggeration;
            }
            set
            {
                if (value > 20)
                    throw new ArgumentException("Vertical exaggeration out of range: " + value);
                if (value <= 0)
                    verticalExaggeration = Single.Epsilon;
                else
                    verticalExaggeration = value;
            }
        }

        #endregion

        #region Tools

        //JOKI
        private Object m_CurrentWwTool;

        [Browsable(true), Category("Tools")]
        [Description("Current WorldWind Tool. Object for BaseWwTool")]
        public Object CurrentWwTool
        {
            get
            {
                //	return Format.Dxt3;
                return m_CurrentWwTool;
            }
            set
            {
                if (value==null)
                {
                    QrstAxGlobeControl.mouseToolUsing = false;
                }
                else
                {
                    QrstAxGlobeControl.mouseToolUsing = true;
                }
                m_CurrentWwTool = value;
            }
        }
        #endregion

        #region Measure tool : �������ߵ���������

        internal MeasureMode measureMode; //��������,�ǵ�����,���Ƕ����.
        internal bool measureShowGroundTrack = true; //����ʱ��,�Ƿ���ʾ����ɫ
        internal int measureLineGroundColor = Color.FromArgb(222, 0, 255, 0).ToArgb(); //�����ı���ɫ.
        internal int measureLineLinearColor = Color.FromArgb(255, 255, 0, 0).ToArgb(); //������ʱ��,���ߵ���ɫ

        /// <summary>
        /// ������Ϣ�������ߵ���ɫ (����ΪColor).
        /// </summary>
        [XmlIgnore]
        [Browsable(true), Category("UI")]
        [Editor(typeof(ColorEditor), typeof(UITypeEditor))]
        [Description("Color of the linear distance measure line.")]
        public Color MeasureLineLinearColor
        {
            get { return Color.FromArgb(measureLineLinearColor); }
            set { measureLineLinearColor = value.ToArgb(); }
        }

        /// <summary>
        /// ���ԣ������ߵ���ɫXML ������Ϊint��
        /// </summary>
        [Browsable(false)]
        public int MeasureLineLinearColorXml
        {
            get { return measureLineLinearColor; }
            set { measureLineLinearColor = value; }
        }

        /// <summary>
        /// ���ԣ������ߵ�ɫ (����Ϊcolor)
        /// </summary>
        [XmlIgnore]
        [Browsable(true), Category("UI")]
        [Editor(typeof(ColorEditor), typeof(UITypeEditor))]
        [Description("Color of the ground track measure line.")]
        public Color MeasureLineGroundColor
        {
            get { return Color.FromArgb(measureLineGroundColor); }
            set { measureLineGroundColor = value.ToArgb(); }
        }
        /// <summary>
        /// ���ԣ������ߵ�ɫXML (����Ϊint)
        /// </summary>
        [Browsable(false)]
        public int MeasureLineGroundColorXml
        {
            get { return measureLineGroundColor; }
            set { measureLineGroundColor = value; }
        }
        /// <summary>
        /// ���ԣ�����ʱ�Ƿ���ʾ�����ټ�
        /// </summary>
        [Browsable(true), Category("UI")]
        [Description("Display the ground track column in the measurement statistics table.")]
        public bool MeasureShowGroundTrack
        {
            get { return measureShowGroundTrack; }
            set { measureShowGroundTrack = value; }
        }

        /// <summary>
        /// ���ԣ�����ģʽ
        /// </summary>
        [Browsable(true), Category("UI")]
        [Description("Measure tool operation mode.")]
        public MeasureMode MeasureMode
        {
            get { return measureMode; }
            set { measureMode = value; }
        }

        #endregion

        public override string ToString()
        {
            return "QrstGlobe";
        }
    }
}