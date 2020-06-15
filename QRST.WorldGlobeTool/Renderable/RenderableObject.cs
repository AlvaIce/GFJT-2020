using System;
using System.Collections;
using System.Windows.Forms;
using System.ComponentModel;
using Microsoft.DirectX;
using QRST.WorldGlobeTool.Utility;

namespace QRST.WorldGlobeTool.Renderable
{
    /// <summary>
    /// 可渲染的图层对象
    /// </summary>
    public abstract class RenderableObject : IRenderable, IComparable
    {

        #region 字段列表

        /// <summary>
        /// 标记图层是否被初始化了.
        /// </summary>
        public bool IsInitialized;
        /// <summary>
        /// 标记此图层是否可以被操作.
        /// </summary>
        public bool IsSelectable;
        /// <summary>
        /// 标记此图层的父亲图层.
        /// </summary>
        public RenderableObjectList ParentList;
        /// <summary>
        /// 图层名称
        /// </summary>
        protected string name;
        /// <summary>
        /// 描述信息
        /// </summary>
        protected string m_description = null;
        /// <summary>
        /// 元数据信息
        /// </summary>
        protected Hashtable _metaData = new Hashtable();
        /// <summary>
        /// 位置信息
        /// </summary>
        protected Vector3 position;
        /// <summary>
        /// 图层的旋转
        /// </summary>
        protected Quaternion orientation;
        /// <summary>
        /// 是否可见
        /// </summary>
        protected bool isOn = true;
        /// <summary>
        /// 是否在三维地图状态下
        /// </summary>
        protected bool is3DMapMode = false;
        /// <summary>
        /// 透明度
        /// </summary>
        protected byte m_opacity = 255;
        /// <summary>
        /// 图层绘制优先级
        /// </summary>
        protected RenderPriority m_renderPriority = RenderPriority.SurfaceImages;
        /// <summary>
        /// 图层所属的星球
        /// </summary>
        protected World m_world;

        #endregion

        #region  事件

        /// <summary>
        /// 单击事件委托
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="drawArgs"></param>
        public delegate void Click(RenderableObject sender, DrawArgs drawArgs);
        /// <summary>
        /// 图层单击事件
        /// </summary>
        public event Click OnClick;
        
        #endregion

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

        #region abstract抽象方法 virtual虚方法

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
                return IsInitialized;
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
        double m_maxdd = double.MaxValue;
        /// <summary>
        /// 最高能见度
        /// </summary>
        public virtual double MaximumDisplayDistance
        {
            get { return m_maxdd; }
            set { m_maxdd = value; }
        }
        double m_mindd = 0;
        /// <summary>
        /// 最低能见度
        /// </summary>
        public virtual double MinimumDisplayDistance
        {
            get { return m_mindd; }
            set { m_mindd = value; }
        }
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
        /// 获取或设置是否处于三维地图状态下
        /// </summary>
        [Description("获取或设置是否处于三维地图状态下.")]
        public virtual bool Is3DMapMode
        {
            get
            {
                return this.is3DMapMode;
            }
            set
            {
                this.is3DMapMode = value;
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

        /// <summary>
        /// 获取或设置图层的描述信息
        /// </summary>
        public string Description
        {
            get { return m_description; }
            set { m_description = value; }
        }

        #endregion

        #region 图层管理器右键方法具体实现

        /// <summary>
        /// Tocbar的右键弹出菜单构建
        /// </summary>
        /// <param name="menu">ContextMenu对象</param>
        public virtual void BuildContextMenu(ContextMenu menu)
        {
            menu.MenuItems.Add("缩放到图层", new EventHandler(OnGotoClick));
            menu.MenuItems.Add("删除当前图层", new EventHandler(OnDeleteClick));
        }

        public virtual Extension Extension
        {
            get
            {
                double north = Convert.ToDouble(this.MetaData["north"]);
                double south = Convert.ToDouble(this.MetaData["south"]);
                double west = Convert.ToDouble(this.MetaData["west"]);
                double east = Convert.ToDouble(this.MetaData["east"]);
                return new Extension(north, south, west, east);
            }
        }

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
                        {//瓦片集
                            QuadTileSet qts = (QuadTileSet)ro;
                            //设置摄像机位置
                            DrawArgs.Camera.SetPosition((qts.North + qts.South) / 2, (qts.East + qts.West) / 2);
                            //垂直视域范围
                            double perpendicularViewRange = (qts.North - qts.South > qts.East - qts.West ? qts.North - qts.South : qts.East - qts.West);
                            //高度，海拔
                            DrawArgs.Camera.Altitude = qts.LayerRadius * Math.Sin(MathEngine.DegreesToRadians(perpendicularViewRange * 0.5));
                            break;
                        }
                        else if (ro is Icon)
                        {//图标
                            Icon ico = (Icon)ro;
                            DrawArgs.Camera.SetPosition(ico.Latitude, ico.Longitude);
                            if (ico.Altitude != 0)
                                DrawArgs.Camera.Altitude = ico.Altitude;
                            else
                                DrawArgs.Camera.Altitude = ico.MaximumDisplayDistance - 100000;
                            break;
                        }
                        else if (ro is Icons)
                        {
                            double lat = 0, lon = 0;
                            Icons icons = ro as Icons;
                            foreach (Icon icon in icons.ChildObjects)
                            {
                                lat += icon.Latitude;
                                lon += icon.Longitude;
                            }
                            lat /= ((Icons)ro).ChildObjects.Count;
                            lon /= ((Icons)ro).ChildObjects.Count;
                            DrawArgs.Camera.SetPosition(lat, lon);
                            if (icons.MaximumDisplayDistance > 16521634)
                            {
                                icons.MaximumDisplayDistance = 16521634;
                            }
                            DrawArgs.Camera.Altitude = icons.MaximumDisplayDistance - 100000;
                            break;
                        }
                        else if (ro is GCP)
                        {
                            GCP gcp = (GCP)ro;
                            DrawArgs.Camera.SetPosition(gcp.Latitude, gcp.Longitude);
                            break;
                        }
                        else if (ro is GCPs)
                        {
                            double lat = 0, lon = 0;
                            foreach (GCP gcp in ((GCPs)ro).ChildObjects)
                            {
                                lat += gcp.Latitude;
                                lon += gcp.Longitude;
                            }
                            lat /= ((GCPs)ro).ChildObjects.Count;
                            lon /= ((GCPs)ro).ChildObjects.Count;
                            DrawArgs.Camera.SetPosition(lat, lon);
                            break;
                        }
                        else
                        {
                            DrawArgs.Camera.SetPosition((ro.Extension.North + ro.Extension.South) / 2, (ro.Extension.West + ro.Extension.East) / 2);
                            //垂直视域范围
                            double perpendicularViewRange = (ro.Extension.North - ro.Extension.South > ro.Extension.East - ro.Extension.West ? ro.Extension.North - ro.Extension.South : ro.Extension.East - ro.Extension.West);
                            //高度，海拔
                            double alt = (float)(ro.World.EquatorialRadius) * Math.Sin(MathEngine.DegreesToRadians(perpendicularViewRange * 1.1));
                            if (alt > ro.MaximumDisplayDistance-100000)
                            {
                                alt = ro.MaximumDisplayDistance-100000;
                            }
                            DrawArgs.Camera.Altitude = alt;
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
            catch (Exception ex)
            {
                Utility.Log.DebugWrite(ex);
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
                MessageBox.Show("无法删除当前子图层", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                throw new Exception("无法删除此图层");
            }
        }

        #endregion

        /// <summary>
        /// 判断两个图层的优先级别
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>1:大于，0：等于，-1：小于</returns>
        public int CompareTo(object obj)
        {
            RenderableObject robj = obj as RenderableObject;
            if (obj == null)
                return 1;
            return this.m_renderPriority.CompareTo(robj.RenderPriority);
        }

        /// <summary>
        /// 返回这个图层的名字。
        /// </summary>
        public override string ToString()
        {
            return name;
        }

    }
}
