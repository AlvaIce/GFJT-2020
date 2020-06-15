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
    /// 图层对象
    /// </summary>
    public abstract class RenderableObject : IRenderable, IComparable
    {
        /// <summary>
        /// 标记图层是否被初始化了.
        /// </summary>
        public bool isInitialized;

        /// <summary>
        /// 标记此图层是否可以被操作.
        /// </summary>
        public bool isSelectable;

        /// <summary>
        /// 标记此图层的父亲图层是谁.
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

        #region 构造函数

        /// <summary>
        /// 初始化一个可渲染的图层<see cref= "T:Qrst.Renderable.RenderableObject"/> class.
        /// </summary>
        /// <param name="name">图层的名称</param>
        protected RenderableObject(string name)
        {
            this.name = name;
        }
        /// <summary>
        /// 初始化一个可渲染的图层。
        /// </summary>
        /// <param name="name">名字</param>
        /// <param name="parentWorld">所属于的星球</param>
        protected RenderableObject(string name, World parentWorld)
        {
            this.name = name;
            this.m_world = parentWorld;
        }

        /// <summary>
        /// 初始化一个可渲染的图层。
        /// </summary>
        /// <param name="name">名字</param>
        /// <param name="position">位置</param>
        /// <param name="orientation">转动</param>
        protected RenderableObject(string name, Vector3 position, Quaternion orientation)
        {
            this.name = name;
            this.position = position;
            this.orientation = orientation;
        }

        #endregion

        #region abstract virtual虚方法

        /// <summary>
        /// 图层初始化方法！虚函数！
        /// </summary>
        /// <param name="drawArgs"></param>
        public abstract void Initialize(DrawArgs drawArgs);
        /// <summary>
        /// 图层的更新方法！虚函数！
        /// </summary>
        /// <param name="drawArgs"></param>
        public abstract void Update(DrawArgs drawArgs);
        /// <summary>
        /// 图层的渲染方法！虚函数！
        /// </summary>
        /// <param name="drawArgs"></param>
        public abstract void Render(DrawArgs drawArgs);

        /// <summary>
        /// 获取该图层是否已经初始化了。
        /// </summary>
        public virtual bool Initialized
        {
            get
            {
                return isInitialized;
            }
        }

        /// <summary>
        /// 图层的释放函数！虚函数.
        /// </summary>
        public abstract void Dispose();

        /// <summary>
        /// 实现用户与地球交互的方法。比如：鼠标点击。
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

        #region 属性列表

        /// <summary>
        /// 设置或获取绘制的优先级别
        /// </summary>
        [Description("设置或获取绘制的优先级别.")]
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
        /// 设置或获取图层的透明度
        /// </summary>
        [Description("(0=不可见, 255=无透明).")]
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
        /// 获取图层的元数据信息
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
        /// 获取或设置图层是否显示
        /// </summary>
        [Description("获取或设置图层是否显示.")]
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
        /// 获取或设置图层的名字
        /// </summary>
        [Description("获取或设置图层的名字.")]
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
        /// 获取或设置图层中心点的笛卡儿坐标位置
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
        /// 获取或设置图层的旋转。
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
        /// 获取该图层所属的星球
        /// </summary>
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public virtual World World
        {
            get
            {
                return m_world;
            }
        }


        //获取或设置图层的描述信息
        public string Description
        {
            get { return m_description; }
            set { m_description = value; }
        }

        #endregion

        #region 图层管理器右键方法具体实现

        ///<summary>
        /// 使照相机ZOOM到图层的位置。
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
        /// 删除图层的具体实现
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
        /// 删除一个图层。
        /// </summary>
        public virtual void Delete()
        {

            RenderableObjectList list = this.ParentList;

            string xmlConfigFile = (string)this.MetaData["XmlSource"];

            if (this.ParentList.Name == "Earth")
            {

                string message = "是否删除图层 '" + this.Name + "' ?";
                if (DialogResult.Yes != MessageBox.Show(message, "删除图层", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2))
                    return;
                this.ParentList.Remove(this);
            }
            else
            {
                throw new Exception("无法删除此图层");
            }
        }

        #endregion

        /// <summary>
        /// 判断两个图层的优先级别
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
        /// Tocbar的右键弹出菜单构建
        /// </summary>
        /// <param name="menu">ContextMenu对象</param>
        public virtual void BuildContextMenu(ContextMenu menu)
        {
            menu.MenuItems.Add("缩放到图层", new EventHandler(OnGotoClick));
            menu.MenuItems.Add("删除当前图层", new EventHandler(OnDeleteClick));
        }

        /// <summary>
        /// 返回这个图层的名字。
        /// </summary>
        public override string ToString()
        {
            return name;
        }

    }

    /// <summary>
    /// 图层绘制优先级
    /// </summary>
    public enum RenderPriority
    {
        SurfaceImages = 0,//表面
        TerrainMappedImages = 100, //地形图
        AtmosphericImages = 200, //大气图片
        LinePaths = 300, //线路
        Icons = 400,   //图标
        Placenames = 500, //地名
        Custom = 600  //自定义
    }
}
