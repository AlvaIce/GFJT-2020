namespace QRST.WorldGlobeTool.PluginEngine
{
    /// <summary>
    /// 被所有插件（由“PluginCompiler”加载）继承的抽象插件基类，
    /// 确保尽可能的轻量级使得插件的编写尽量简单。
    /// </summary>
    public abstract class Plugin
    {
        /// <summary>
        /// 绑定到主三维球体应用程序对象
        /// </summary>
        protected QRSTWorldGlobeControl m_Application;

        /// <summary>
        /// 加载插件的目录
        /// </summary>
        protected string m_PluginDirectory;

        /// <summary>
        /// 插件运行标志（true：运行时，false：退出插件时复位）
        /// </summary>
        protected bool m_isLoaded;

        /// <summary>
        /// WorldWind主应用程序对象引用（被弃用：使用“ParentApplication”属性代替）
        /// 尽量不采用这个属性，会引起歧义
        /// </summary>
        public virtual QRSTWorldGlobeControl Application
        {
            get
            {
                return m_Application;
            }
        }

        /// <summary>
        /// 获取WorldWind主应用程序对象引用
        /// </summary>
        public virtual QRSTWorldGlobeControl ParentApplication
        {
            get
            {
                return m_Application;
            }
        }

        /// <summary>
        /// 获取加载插件的目录
        /// </summary>
        public virtual string PluginDirectory
        {
            get
            {
                return m_PluginDirectory;
            }
        }

        /// <summary>
        /// 获取插件是否正在加载运行
        /// </summary>
        public virtual bool IsLoaded
        {
            get
            {
                return m_isLoaded;
            }
        }

        /// <summary>
        /// 加载插件，插件的入口点
        /// </summary>
        public virtual void Load()
        {
            // Override with plugin initialization code.
        }

        /// <summary>
        /// 卸载插件，修改WorldWind或者在后台运行的插件应该重写此方法
        /// </summary>
        public virtual void Unload()
        {
            // Override with plugin dispose code.
        }

        /// <summary>
        /// 基本的加载方法，调用加载方法
        /// </summary>
        /// <param name="parent"></param>
        public virtual void PluginLoad(QRSTWorldGlobeControl parent, string pluginDirectory)
        {
            if (m_isLoaded)  // Already loaded
                return;

            m_Application = parent;
            m_PluginDirectory = pluginDirectory;
            Load();
            m_isLoaded = true;
        }

        /// <summary>
        /// 基本的卸载方法，调用卸载方法
        /// </summary>
        public virtual void PluginUnload()
        {
            Unload();
            m_isLoaded = false;
        }
    }
}
