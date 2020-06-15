using System.IO;

namespace QRST.WorldGlobeTool.PluginEngine
{
    /// <summary>
    /// 存储关于插件的相关信息
    /// </summary>
    public class PluginInfo
    {
        /// <summary>
        /// 插件实例
        /// </summary>
        Plugin m_plugin;
        /// <summary>
        /// 插件的文件名
        /// </summary>
        string m_fullPath;
        /// <summary>
        /// 插件名称
        /// </summary>
        string m_name;
        /// <summary>
        /// 插件的描述信息
        /// </summary>
        string m_description;
        /// <summary>
        /// 插件的开发者名称
        /// </summary>
        string m_developer;
        /// <summary>
        /// 插件的网址
        /// </summary>
        string m_webSite;
        /// <summary>
        /// 插件需要的由逗号分隔的附加引用库列表
        /// </summary>
        string m_references;
        /// <summary>
        /// 插件是否总是在应用程序启动时加载
        /// </summary>
        bool isLoadedAtStartup;

        /// <summary>
        /// 获取或设置插件实例
        /// </summary>
        public Plugin Plugin
        {
            get
            {
                return m_plugin;
            }
            set
            {
                m_plugin = value;
            }
        }

        /// <summary>
        /// 获取或设置插件的文件名
        /// </summary>
        public string FullPath
        {
            get
            {
                return m_fullPath;
            }
            set
            {
                m_fullPath = value;
            }
        }

        /// <summary>
        /// 获取插件编号（或名称）
        /// </summary>
        public string ID
        {
            get
            {
                if (m_fullPath != null)
                    return Path.GetFileNameWithoutExtension(m_fullPath);
                return m_name;
            }
        }

        /// <summary>
        /// 获取或设置插件的名称（从插件描述头的“NAME”标签中读取）
        /// </summary>
        public string Name
        {
            get
            {
                if (m_name == null)
                    ReadMetaData();

                return m_name;
            }
            set
            {
                m_name = value;
            }
        }

        /// <summary>
        /// 获取或设置插件的描述信息（从插件描述头的“DESCRIPTION”标签中读取）
        /// </summary>
        public string Description
        {
            get
            {
                if (m_description == null)
                    ReadMetaData();

                return m_description;
            }
            set
            {
                m_description = value;
            }
        }

        /// <summary>
        /// 获取插件的开发者名称（从插件描述头的“DEVELOPER”标签中读取）
        /// </summary>
        public string Developer
        {
            get
            {
                if (m_developer == null)
                    ReadMetaData();

                return m_developer;
            }
        }

        /// <summary>
        /// 获取插件的网址（从插件描述头的“WEBSITE”标签中读取）
        /// </summary>
        public string WebSite
        {
            get
            {
                if (m_webSite == null)
                    ReadMetaData();

                return m_webSite;
            }
        }

        /// <summary>
        /// 获取插件需要的由逗号分隔的附加引用库列表
        /// </summary>
        public string References
        {
            get
            {
                if (m_references == null)
                    ReadMetaData();

                return m_references;
            }
        }

        /// <summary>
        /// 获取插件当前是否加载
        /// </summary>
        public bool IsCurrentlyLoaded
        {
            get
            {
                if (m_plugin == null)
                    return false;
                return m_plugin.IsLoaded;
            }
        }

        /// <summary>
        /// 设置插件是否在应用程序启动时加载
        /// </summary>
        public bool IsLoadedAtStartup
        {
            get
            {
                return false;
            }
            set
            {
                isLoadedAtStartup = value;
            }
        }

        /// <summary>
        /// 从源文件的头标签中读取元数据字符串
        /// </summary>
        private void ReadMetaData()
        {
            try
            {
                if (m_fullPath == null)  //Source code comments not available
                    return;

                // 初始化变量
                if (m_name == null)
                    m_name = "";
                if (m_description == null)
                    m_description = "";
                if (m_developer == null)
                    m_developer = "";
                if (m_webSite == null)
                    m_webSite = "";
                if (m_references == null)
                    m_references = "";

                using (TextReader tr = File.OpenText(m_fullPath))
                {
                    while (true)
                    {
                        string line = tr.ReadLine();
                        if (line == null)
                            break;

                        FindTagInLine(line, "NAME", ref m_name);
                        FindTagInLine(line, "DESCRIPTION", ref m_description);
                        FindTagInLine(line, "DEVELOPER", ref m_developer);
                        FindTagInLine(line, "WEBSITE", ref m_webSite);
                        FindTagInLine(line, "REFERENCES", ref m_references);
                    }
                }
            }
            catch (IOException)
            {
                // Ignore
            }
            finally
            {
                if (m_name.Length == 0)
                    // If name is not defined, use the filename
                    // 如果名称项没有定义，采用文件名
                    m_name = Path.GetFileNameWithoutExtension(m_fullPath);
            }
        }

        /// <summary>
        /// Extract tag value from input source line.
        /// 从输入的源文本行中提取标签的值
        /// </summary>
        /// <param name="inputLine">从文件中读取到的问本行</param>
        /// <param name="tag">标签</param>
        /// <param name="value">标签的值</param>
        static void FindTagInLine(string inputLine, string tag, ref string value)
        {
            if (value != string.Empty)	// Already found
                return;

            // Pattern: _TAG:_<value>EOL  匹配查找
            tag = " " + tag + ": ";
            int index = inputLine.IndexOf(tag);
            if (index < 0)
                return;

            value = inputLine.Substring(index + tag.Length);
        }
    }
}
