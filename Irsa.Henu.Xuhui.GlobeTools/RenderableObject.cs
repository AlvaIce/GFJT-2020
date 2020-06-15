using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using Microsoft.DirectX;
using Qrst.Menu;

namespace Qrst.Renderable
{
    /// <summary>
    /// ͼ�����
    /// </summary>
    public abstract class RenderableObject : IRenderable, IComparable
    {
        /// <summary>
        /// ���ͼ���Ƿ񱻳�ʼ����.
        /// </summary>
        public bool isInitialized;

        /// <summary>
        /// ��Ǵ�ͼ���Ƿ���Ա�����.
        /// </summary>
        public bool isSelectable;

        /// <summary>
        /// ��Ǵ�ͼ��ĸ���ͼ����˭.
        /// </summary>
        public RenderableObjectList ParentList;

        protected string name;
        protected string m_description = null;
        protected Hashtable _metaData = new Hashtable();
        protected Vector3 position;
        protected Quaternion orientation;
        protected bool isOn = true;
        protected byte m_opacity = 255;
        protected RenderPriority m_renderPriority = RenderPriority.SurfaceImages;
        protected World m_world;
        public delegate void Click(RenderableObject sender, DrawArgs drawArgs);
        public event Click OnClick;

        #region ���캯��

        /// <summary>
        /// ��ʼ��һ������Ⱦ��ͼ��<see cref= "T:Qrst.Renderable.RenderableObject"/> class.
        /// </summary>
        /// <param name="name">ͼ�������</param>
        protected RenderableObject(string name)
        {
            this.name = name;
        }
        /// <summary>
        /// ��ʼ��һ������Ⱦ��ͼ�㡣
        /// </summary>
        /// <param name="name">����</param>
        /// <param name="parentWorld">�����ڵ�����</param>
        protected RenderableObject(string name, World parentWorld)
        {
            this.name = name;
            this.m_world = parentWorld;
        }

        /// <summary>
        /// ��ʼ��һ������Ⱦ��ͼ�㡣
        /// </summary>
        /// <param name="name">����</param>
        /// <param name="position">λ��</param>
        /// <param name="orientation">ת��</param>
        protected RenderableObject(string name, Vector3 position, Quaternion orientation)
        {
            this.name = name;
            this.position = position;
            this.orientation = orientation;
        }

        #endregion

        #region abstract virtual�鷽��

        /// <summary>
        /// ͼ���ʼ���������麯����
        /// </summary>
        /// <param name="drawArgs"></param>
        public abstract void Initialize(DrawArgs drawArgs);
        /// <summary>
        /// ͼ��ĸ��·������麯����
        /// </summary>
        /// <param name="drawArgs"></param>
        public abstract void Update(DrawArgs drawArgs);
        /// <summary>
        /// ͼ�����Ⱦ�������麯����
        /// </summary>
        /// <param name="drawArgs"></param>
        public abstract void Render(DrawArgs drawArgs);

        /// <summary>
        /// ��ȡ��ͼ���Ƿ��Ѿ���ʼ���ˡ�
        /// </summary>
        public virtual bool Initialized
        {
            get
            {
                return isInitialized;
            }
        }

        /// <summary>
        /// ͼ����ͷź������麯��.
        /// </summary>
        public abstract void Dispose();

        /// <summary>
        /// ʵ���û�����򽻻��ķ��������磺�������
        /// </summary>
        public virtual bool PerformSelectionAction(DrawArgs drawArgs)
        {
            if (this.OnClick == null)
                return false;
            else
            {
                this.OnClick(this, drawArgs);
                return true;
            }
        }

        #endregion

        #region �����б�

        /// <summary>
        /// ���û��ȡ���Ƶ����ȼ���
        /// </summary>
        [Description("���û��ȡ���Ƶ����ȼ���.")]
        public virtual RenderPriority RenderPriority
        {
            get
            {
                return this.m_renderPriority;
            }
            set
            {
                this.m_renderPriority = value;
                if (ParentList != null)
                    ParentList.SortChildren();
            }
        }

        /// <summary>
        /// ���û��ȡͼ���͸����
        /// </summary>
        [Description("(0=���ɼ�, 255=��͸��).")]
        public virtual byte Opacity
        {
            get
            {
                return this.m_opacity;
            }
            set
            {
                this.m_opacity = value;
                if (value == 0)
                {
                    if (this.isOn)
                        this.IsOn = false;
                }
                else
                {
                    if (!this.isOn)
                        this.IsOn = true;
                }
            }
        }


        /// <summary>
        /// ��ȡͼ���Ԫ������Ϣ
        /// </summary>
        [Browsable(false)]
        public virtual Hashtable MetaData
        {
            get
            {
                return this._metaData;
            }
        }
        /// <summary>
        /// ��ȡ������ͼ���Ƿ���ʾ
        /// </summary>
        [Description("��ȡ������ͼ���Ƿ���ʾ.")]
        public virtual bool IsOn
        {
            get
            {
                return this.isOn;
            }
            set
            {
                if (isOn && !value)
                    this.Dispose();
                this.isOn = value;
            }
        }

        /// <summary>
        /// ��ȡ������ͼ�������
        /// </summary>
        [Description("��ȡ������ͼ�������.")]
        public virtual string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        /// <summary>
        /// ��ȡ������ͼ�����ĵ�ĵѿ�������λ��
        /// </summary>
        [Browsable(false)]
        public virtual Vector3 Position
        {
            get
            {
                return this.position;
            }
            set
            {
                this.position = value;
            }
        }

        /// <summary>
        /// ��ȡ������ͼ�����ת��
        /// </summary>
        [Browsable(false)]
        public virtual Quaternion Orientation
        {
            get
            {
                return this.orientation;
            }
            set
            {
                this.orientation = value;
            }
        }

        /// <summary>
        /// ��ȡ��ͼ������������
        /// </summary>
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public virtual World World
        {
            get
            {
                return m_world;
            }
        }


        //��ȡ������ͼ���������Ϣ
        public string Description
        {
            get { return m_description; }
            set { m_description = value; }
        }

        #endregion

        #region ͼ��������Ҽ���������ʵ��

        ///<summary>
        /// ʹ�����ZOOM��ͼ���λ�á�
        /// </summary>
        protected virtual void OnGotoClick(object sender, EventArgs e)
        {
            lock (this.ParentList.ChildObjects.SyncRoot)
            {
                for (int i = 0; i < this.ParentList.ChildObjects.Count; i++)
                {
                    RenderableObject ro = (RenderableObject)this.ParentList.ChildObjects[i];
                    if (ro.Name.Equals(name))
                    {
                        if (ro is QuadTileSet)
                        {
                            QuadTileSet qts = (QuadTileSet)ro;
                            DrawArgs.Camera.SetPosition((qts.North + qts.South) / 2, (qts.East + qts.West) / 2);
                            double perpendicularViewRange = (qts.North - qts.South > qts.East - qts.West ? qts.North - qts.South : qts.East - qts.West);
                            double altitude = qts.LayerRadius * Math.Sin(MathEngine.DegreesToRadians(perpendicularViewRange * 0.5));

                            DrawArgs.Camera.Altitude = altitude;

                            break;
                        }
                        else
                        {
                            double north = Convert.ToDouble(ro.MetaData["north"]);
                            double south = Convert.ToDouble(ro.MetaData["south"]);
                            double west = Convert.ToDouble(ro.MetaData["west"]);
                            double east = Convert.ToDouble(ro.MetaData["east"]);
                            DrawArgs.Camera.SetPosition((north + south) / 2, (west + east) / 2);

                        }
                        if (ro is Icon)
                        {
                            Icon ico = (Icon)ro;
                            DrawArgs.Camera.SetPosition(ico.Latitude, ico.Longitude);
                            DrawArgs.Camera.Altitude /= 2;

                            break;
                        }

                    }
                }
            }
        }

        /// <summary>
        /// ɾ��ͼ��ľ���ʵ��
        /// </summary>
        protected virtual void OnDeleteClick(object sender, EventArgs e)
        {
            try
            {
                this.Delete();
            }
            catch
            {
            }

        }

        /// <summary>
        /// ɾ��һ��ͼ�㡣
        /// </summary>
        public virtual void Delete()
        {

            RenderableObjectList list = this.ParentList;

            string xmlConfigFile = (string)this.MetaData["XmlSource"];

            if (this.ParentList.Name == "Earth")
            {

                string message = "�Ƿ�ɾ��ͼ�� '" + this.Name + "' ?";
                if (DialogResult.Yes != MessageBox.Show(message, "ɾ��ͼ��", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2))
                    return;
                this.ParentList.Remove(this);
            }
            else
            {
                throw new Exception("�޷�ɾ����ͼ��");
            }
        }

        #endregion

        /// <summary>
        /// �ж�����ͼ������ȼ���
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            RenderableObject robj = obj as RenderableObject;
            if (obj == null)
                return 1;
            return this.m_renderPriority.CompareTo(robj.RenderPriority);
        }
        /// <summary>
        /// Tocbar���Ҽ������˵�����
        /// </summary>
        /// <param name="menu">ContextMenu����</param>
        public virtual void BuildContextMenu(ContextMenu menu)
        {
            menu.MenuItems.Add("���ŵ�ͼ��", new EventHandler(OnGotoClick));
            menu.MenuItems.Add("ɾ����ǰͼ��", new EventHandler(OnDeleteClick));
        }

        /// <summary>
        /// �������ͼ������֡�
        /// </summary>
        public override string ToString()
        {
            return name;
        }

    }

    /// <summary>
    /// ͼ��������ȼ�
    /// </summary>
    public enum RenderPriority
    {
        SurfaceImages = 0,//����
        TerrainMappedImages = 100, //����ͼ
        AtmosphericImages = 200, //����ͼƬ
        LinePaths = 300, //��·
        Icons = 400,   //ͼ��
        Placenames = 500, //����
        Custom = 600  //�Զ���
    }
}
