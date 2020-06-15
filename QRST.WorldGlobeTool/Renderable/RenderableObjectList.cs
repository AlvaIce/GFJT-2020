using System;
using System.Collections;
using QRST.WorldGlobeTool.Utility;
using Microsoft.DirectX;
using System.ComponentModel;

namespace QRST.WorldGlobeTool.Renderable
{
    /// <summary>
    /// 代表图层管理树种的一个父节点，包含一个子节点列表
    /// Represents a parent node in the layer manager tree.  Contains a list of sub-nodes.
    /// </summary>
    public class RenderableObjectList : RenderableObject
    {
        #region  字段
        public override Extension Extension
        {
            get
            {
                Extension ext = new Extension();
                foreach (RenderableObject ro in this.ChildObjects)
                {
                    ext.Include(ro.Extension);
                }
                return ext;
            }
        }
        /// <summary>
        /// 子节点列表
        /// </summary>
        protected ArrayList m_children = new ArrayList();
        /// <summary>
        /// 数据源
        /// </summary>
        string m_DataSource = null;
        /// <summary>
        /// 刷新时间间隔
        /// </summary>
        TimeSpan m_RefreshInterval = TimeSpan.MaxValue;
        /// <summary>
        /// 刷新定时器
        /// </summary>
        System.Timers.Timer m_RefreshTimer = null;
        /// <summary>
        /// 图层集合所属的星球
        /// </summary>
        World m_ParentWorld = null;
        /// <summary>
        /// 缓存
        /// </summary>
        Cache m_Cache = null;
        /// <summary>
        /// 是否使不可折叠
        /// </summary>
        private bool m_disableExpansion = false;
        /// <summary>
        /// 是否已经开始第一次刷新
        /// </summary>
        private bool hasSkippedFirstRefresh = false;
        /// <summary>
        /// 是否只显示一个图层
        /// </summary>
        private bool m_IsShowOnlyOneLayer;

        #endregion

        #region  属性

        public override World World
        {
            get {
                if (m_world==null&&this.ParentList!=null)
                {
                    return this.ParentList.World;
                }
                return m_world;
            }
        }

        public void SetWorld(World world)
        {
            m_world = world;
        }


        /// <summary>
        /// 使不可折叠
        /// </summary>		
        public bool DisableExpansion
        {
            get { return m_disableExpansion; }
            set { m_disableExpansion = value; }
        }

        /// <summary>
        /// 刷新定时器
        /// </summary>
        public System.Timers.Timer RefreshTimer
        {
            get
            {
                return m_RefreshTimer;
            }
        }

        /// <summary>
        /// 获得所有的子图层。
        /// </summary>
        [Browsable(false)]
        public virtual ArrayList ChildObjects
        {
            get
            {
                return this.m_children;
            }
        }

        /// <summary>
        /// Number of child objects.获得所有的子图层的个数
        /// </summary>
        [Browsable(false)]
        public virtual int Count
        {
            get
            {
                return this.m_children.Count;
            }
        }

        /// <summary>
        /// 获取或设置是否只显示一个图层
        /// </summary>
        public bool IsShowOnlyOneLayer
        {
            get { return m_IsShowOnlyOneLayer; }
            set { m_IsShowOnlyOneLayer = value; }
        }

        #endregion

        #region  构造函数

        /// <summary>
        /// 初始化RenderableObjectList类的一个新实例
        /// </summary>
        /// <param name="name">图层列表名称</param>
        public RenderableObjectList(string name)
            : base(name, new Vector3(0, 0, 0), new Quaternion())
        {
            this.IsSelectable = true;
            
        }

        /// <summary>
        /// 初始化RenderableObjectList类的一个新实例
        /// </summary>
        /// <param name="name">图层列表名称</param>
        /// <param name="dataSource">数据源</param>
        /// <param name="refreshInterval">定时刷新时间间隔</param>
        /// <param name="parentWorld">父世界</param>
        /// <param name="cache">缓存</param>
        public RenderableObjectList(
            string name,
            string dataSource,
            TimeSpan refreshInterval,
            World parentWorld,
            Cache cache
            )
            : base(name, new Vector3(0, 0, 0), new Quaternion())
        {
            IsSelectable = true; //可以交互
            m_DataSource = dataSource; //设置数据源
            m_RefreshInterval = refreshInterval; // 刷新的时间间隔
            m_ParentWorld = parentWorld; // 父世界是什么
            m_Cache = cache;  //缓存
            m_RefreshTimer = new System.Timers.Timer(
                refreshInterval.Hours * 60 * 60 * 1000 +
                refreshInterval.Minutes * 60 * 1000 +
                refreshInterval.Seconds * 1000
                );  //要进行刷新的时间间隔
            m_RefreshTimer.Elapsed += new System.Timers.ElapsedEventHandler(m_RefreshTimer_Elapsed); //注册时间间隔刷新事件。
        }

        #endregion

        #region  公共重载方法

        /// <summary>
        /// 初始化图层显示信息
        /// </summary>
        /// <param name="drawArgs">绘制参数</param>
        public override void Initialize(DrawArgs drawArgs)
        {
            if (!this.IsOn)//不显示的情况下直接返回
                return;

            //try
            //{
            foreach (RenderableObject ro in this.m_children)
            {
                try
                {
                    if (ro.IsOn)
                        ro.Initialize(drawArgs);  //调用该类型对象的初始化方法
                }
                catch (Exception caught)
                {
                    Log.Write(Log.Levels.Error, "ROBJ-Initialize", string.Format("{0}: {1} ({2})",
                        Name, caught.Message, ro.Name));
                }
            }
            //}
            //catch
            //{}

            this.IsInitialized = true;
        }

        /// <summary>
        /// 更新图层列表
        /// </summary>
        /// <param name="drawArgs">绘制参数</param>
        public override void Update(DrawArgs drawArgs)
        {
            if (!this.IsOn)
                return;

            if (!this.IsInitialized)
                this.Initialize(drawArgs);

            foreach (RenderableObject ro in this.m_children)
            {
                if (ro.ParentList == null)
                    ro.ParentList = this;
                try
                {
                    if (ro.IsOn)
                        ro.Update(drawArgs); //调用自己相关的更新函数
                }
                catch (Exception caught)
                {
                    Log.Write(Log.Levels.Error, "ROBJ-Update", string.Format("{0}: {1} ({2})",
                        Name, caught.Message, ro.Name));
                }
            }
        }

        /// <summary>
        /// 用户与地球交互的方法
        /// </summary>
        /// <param name="drawArgs">绘制参数</param>
        /// <returns></returns>
        public override bool PerformSelectionAction(DrawArgs drawArgs)
        {
            if (!this.IsOn)
                return false;

            try
            {
                foreach (RenderableObject ro in this.m_children)
                {
                    if (ro.IsOn && ro.IsSelectable)
                        if (ro.PerformSelectionAction(drawArgs))  //调用自己相关的PerformSelectionAction
                            return true;  //ZYM:20130707-功能待定，有异议
                }
            }
            catch
            {
            }
            return false;
        }

        /// <summary>
        /// 渲染图层列表对象
        /// </summary>
        /// <param name="drawArgs">绘制参数</param>
        public override void Render(DrawArgs drawArgs)
        {
            if (!this.IsOn)
                return;

            try
            {
                lock (this.m_children.SyncRoot)
                {
                    foreach (RenderableObject ro in this.m_children)
                    {
                        if (ro.IsOn)
                            ro.Render(drawArgs); //调用自己的 Render方法
                    }
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            try
            {
                this.IsInitialized = false;

                foreach (RenderableObject ro in this.m_children)
                    ro.Dispose();  //调用各自的释放资源的方法

                if (m_RefreshTimer != null && m_RefreshTimer.Enabled)
                    m_RefreshTimer.Stop();
            }
            catch
            {
            }
        }

        #endregion

        #region  公共虚方法

        /// <summary>
        /// 通过名字获取Child中的一个RenderableObject对象
        /// </summary>
        /// <param name="name">对象名称</param>
        /// <returns>可渲染对象</returns>
        public virtual RenderableObject GetObject(string name)
        {
            try
            {
                foreach (RenderableObject ro in this.m_children)
                {
                    if (ro.Name.Equals(name))
                        return ro;
                }
            }
            catch
            {
            }
            return null;
        }

        /// <summary>
        /// Enables layer with specified name
        /// 使图层或图层列表可以用。
        /// </summary>
        /// <returns>False if layer not found.</returns>     
        public virtual bool Enable(string name)
        {
            if (name == null || name.Length == 0)
                return true;

            string lowerName = name.ToLower();
            foreach (RenderableObject ro in m_children)
            {
                if (ro.Name.ToLower() == lowerName)
                {
                    ro.IsOn = true;
                    return true;
                }

                RenderableObjectList rol = ro as RenderableObjectList;
                if (rol == null)
                    continue;

                // Recurse down
                if (rol.Enable(name))
                {
                    rol.isOn = true;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 使所有的子图层不可用
        /// </summary>
        public virtual void TurnOffAllChildren()
        {
            foreach (RenderableObject ro in this.m_children)
                ro.IsOn = false;
        }

        /// <summary>
        /// 添加一个子图层到这个图层列表。
        /// Add a child object to this layer.
        /// </summary>
        public virtual void Add(RenderableObject ro)
        {
            lock (this.m_children.SyncRoot)
            {
                RenderableObjectList dupList = null;
                RenderableObject duplicate = null;
                ro.ParentList = this;
                foreach (RenderableObject childRo in m_children)
                {
                    if (childRo is RenderableObjectList && childRo.Name == ro.Name)
                    {
                        dupList = (RenderableObjectList)childRo;
                        break;
                    }
                    else if (childRo.Name == ro.Name)
                    {
                        duplicate = childRo;
                        break;
                    }
                }

                if (dupList != null)
                {
                    RenderableObjectList rol = (RenderableObjectList)ro;

                    foreach (RenderableObject childRo in rol.ChildObjects)
                    {
                        dupList.Add(childRo);
                    }
                }
                else
                {
                    if (duplicate != null)
                    {
                        for (int i = 1; i < 100; i++)
                        {//TODO:ZYM-20130707发现，需要解决工具加载时图层列表中出现多个相同名称的问题
                            ro.Name = string.Format("{0} [{1}]", duplicate.Name, i);
                            bool found = false;
                            foreach (RenderableObject childRo in m_children)
                            {
                                if (childRo.Name == ro.Name)
                                {
                                    found = true;
                                    break;
                                }
                            }
                            if (!found)
                                break;
                        }
                    }
                    m_children.Add(ro);
                }
                SortChildren();
            }
        }

        /// <summary>
        /// 删除所有的子图层
        /// </summary>
        public virtual void RemoveAll()
        {
            try
            {
                while (m_children.Count > 0)
                {
                    RenderableObject ro = (RenderableObject)m_children[0];
                    m_children.RemoveAt(0);
                    ro.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 从子图层列表中移除一个图层
        /// Removes a layer from the child layer list
        /// </summary>
        /// <param name="objectName">被移除的图层对象的名称Name of object to remove</param>
        public virtual void Remove(string objectName)
        {
            lock (this.m_children.SyncRoot)
            {
                for (int i = 0; i < this.m_children.Count; i++)
                {
                    RenderableObject ro = (RenderableObject)this.m_children[i];
                    if (ro.Name.Equals(objectName))
                    {
                        ro.ParentList = null;
                        ro.Dispose();
                        this.m_children.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 从子图层列表中移除一个图层
        /// Removes a layer from the child layer list
        /// </summary>
        /// <param name="layer">被移除的图层对象Layer to be removed.</param>
        public virtual void Remove(RenderableObject layer)
        {
            lock (this.m_children.SyncRoot)
            {
                layer.ParentList = null;
                layer.Dispose();
                this.m_children.Remove(layer);
            }
        }
        
        /// <summary>
        /// 通过优先级对子节点进行排序
        /// Sorts the children list according to priority
        /// TODO: Redesign the render tree to perhaps a list, to enable proper sorting 
        /// </summary>
        public virtual void SortChildren()
        {
            int index = 0;
            while (index + 1 < m_children.Count)
            {
                RenderableObject a = (RenderableObject)m_children[index];
                RenderableObject b = (RenderableObject)m_children[index + 1];
                if (a.RenderPriority > b.RenderPriority)
                {
                    // Swap
                    m_children[index] = b;
                    m_children[index + 1] = a;
                    index = 0;
                    continue;
                }
                index++;
            }
        }

        #endregion

        
        #region  私有方法

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldRenderable"></param>
        /// <param name="newRenderable"></param>
        private void updateRenderable(RenderableObject oldRenderable, RenderableObject newRenderable)
        {
            if (oldRenderable is Icon && newRenderable is Icon)
            {
                Icon oldIcon = (Icon)oldRenderable;
                Icon newIcon = (Icon)newRenderable;

                oldIcon.SetPosition((float)newIcon.Latitude, (float)newIcon.Longitude, (float)newIcon.Altitude);
            }
            else if (oldRenderable is GCP && newRenderable is GCP)
            {
                GCP oldGCP = (GCP)oldRenderable;
                GCP newGCP = (GCP)newRenderable;

                oldGCP.SetPosition((float)newGCP.Latitude, (float)newGCP.Longitude, (float)newGCP.Altitude);
            }
            else if (oldRenderable is RenderableObjectList && newRenderable is RenderableObjectList)
            {
                RenderableObjectList oldList = (RenderableObjectList)oldRenderable;
                RenderableObjectList newList = (RenderableObjectList)newRenderable;

                compareRefreshLists(newList, oldList);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newList"></param>
        /// <param name="curList"></param>
        private void compareRefreshLists(RenderableObjectList newList, RenderableObjectList curList)
        {
            ArrayList addList = new ArrayList();
            ArrayList delList = new ArrayList();

            foreach (RenderableObject newObject in newList.ChildObjects)
            {
                bool foundObject = false;
                foreach (RenderableObject curObject in curList.ChildObjects)
                {
                    string xmlSource = curObject.MetaData["XmlSource"] as string;

                    if (xmlSource != null && xmlSource == m_DataSource && newObject.Name == curObject.Name)
                    {
                        foundObject = true;
                        updateRenderable(curObject, newObject);
                        break;
                    }
                }

                if (!foundObject)
                {
                    addList.Add(newObject);
                }
            }

            foreach (RenderableObject curObject in curList.ChildObjects)
            {
                bool foundObject = false;
                foreach (RenderableObject newObject in newList.ChildObjects)
                {
                    string xmlSource = newObject.MetaData["XmlSource"] as string;
                    if (xmlSource != null && xmlSource == m_DataSource && newObject.Name == curObject.Name)
                    {
                        foundObject = true;
                        break;
                    }
                }

                if (!foundObject)
                {
                    string src = (string)curObject.MetaData["XmlSource"];

                    if (src != null || src == m_DataSource)
                        delList.Add(curObject);
                }
            }

            foreach (RenderableObject o in addList)
            {
                curList.Add(o);
            }

            foreach (RenderableObject o in delList)
            {
                curList.Remove(o);
            }
        }

        /// <summary>
        /// 计时器定时触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_RefreshTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!hasSkippedFirstRefresh)
            {
                hasSkippedFirstRefresh = true;
                return;
            }
        }

        /// <summary>
        /// 开始计时，为了刷新（过一个时间间隔进行刷新）
        /// </summary>
        private void StartRefreshTimer()
        {
            if (m_RefreshTimer != null)
            {
                m_RefreshTimer.Start();
            }
        }

        #endregion

    }
}
