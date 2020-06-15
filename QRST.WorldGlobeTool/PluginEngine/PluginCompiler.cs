using System;
using System.Text;
using System.Reflection;
using System.IO;
using QRST.WorldGlobeTool.Utility;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Specialized;

namespace QRST.WorldGlobeTool.PluginEngine
{
    /// <summary>
    /// 加载插件脚本，编译并执行他们
    /// </summary>
    public class PluginCompiler
    {
        /// <summary>
        /// 插件作用的控件
        /// </summary>
        QRSTWorldGlobeControl qrstWorldGlobe;
        /// <summary>
        /// 日志描述标志字符串
        /// </summary>
        const string LogCategory = "PLUG";
        /// <summary>
        /// 代码提供者表
        /// </summary>
        Hashtable codeDomProviders = new Hashtable();
        /// <summary>
        /// 调用编译器的参数
        /// </summary>
        CompilerParameters cp = new CompilerParameters();
        /// <summary>
        /// 发现的插件列表
        /// </summary>
        ArrayList m_plugins = new ArrayList();
        /// <summary>
        /// 引用列表
        /// </summary>
        StringCollection m_worldWindReferencesList = new StringCollection();
        /// <summary>
        /// 插件的根目录
        /// </summary>
        string m_pluginRootDirectory;

        /// <summary>
        /// 获取或设置插件的根目录
        /// </summary>
        public string PluginRootDirectory
        {
            get
            {
                return m_pluginRootDirectory;
            }
            set
            {
                m_pluginRootDirectory = value;

                try
                {
                    Directory.CreateDirectory(m_pluginRootDirectory);
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// 获取发现的插件列表
        /// </summary>
        public ArrayList Plugins
        {
            get
            {
                return m_plugins;
            }
        }

        /// <summary>
        /// 初始化“PluginCompiler”类的新实例
        /// </summary>
        /// <param name="qrstWorldGlobe">三维球主程序</param>
        /// <param name="pluginDirectory">插件目录</param>
        public PluginCompiler(QRSTWorldGlobeControl qrstWorldGlobe, string pluginDirectory)
        {
            this.qrstWorldGlobe = qrstWorldGlobe;

            //添加可用的代码编译提供者
            AddCodeProvider(new Microsoft.CSharp.CSharpCodeProvider());
            AddCodeProvider(new Microsoft.VisualBasic.VBCodeProvider());
            AddCodeProvider(new Microsoft.JScript.JScriptCodeProvider());

            //设置编译参数
            cp.GenerateExecutable = false;
            cp.GenerateInMemory = true;
            cp.IncludeDebugInformation = false;

            // 加载所有的引用程序集
            AssemblyName[] assemblyNames = Assembly.GetExecutingAssembly().GetReferencedAssemblies();
            foreach (AssemblyName assemblyName in assemblyNames)
            {
                try
                {
                    Assembly.Load(assemblyName);
                }
                catch { }
            }
            //引用所有已经加载的程序集
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                try
                {
                    if (assembly.Location.Length > 0)
                        m_worldWindReferencesList.Add(assembly.Location);
                }
                catch (NotSupportedException)
                {
                    // In-memory compiled assembly etc.
                }
            }

            PluginRootDirectory = pluginDirectory;
        }

        /// <summary>
        /// 添加一个编译器到可用代码编译提供者列表中，包含文件名和编译器信息
        /// </summary>
        public void AddCodeProvider(CodeDomProvider cdp)
        {
            codeDomProviders.Add("." + cdp.FileExtension, cdp);
        }

        /// <summary>
        /// 从输入程序集中加载插件（WorldWind原版插件）
        /// </summary>
        public void FindPlugins(Assembly assembly)
        {
            foreach (Type t in assembly.GetTypes())
            {
                if (!t.IsClass)
                    continue;

                if (!t.IsPublic)
                    continue;

                if (t.BaseType != typeof(Plugin))
                    continue;

                try
                {
                    PluginInfo pi = new PluginInfo();
                    pi.Plugin = (Plugin)assembly.CreateInstance(t.ToString());
                    pi.Name = t.Name;
                    pi.Description = "QRST.QRSTWorldGlobe内部加载插件。";
                    m_plugins.Add(pi);
                }
                catch
                {
                    // Ignore exceptions during entry point search.
                }
            }
        }


        /// <summary>
        /// 构建/更新可用插件列表
        /// </summary>
        public void FindPlugins()
        {
            if (!Directory.Exists(m_pluginRootDirectory))
                return;

            foreach (string directory in Directory.GetDirectories(m_pluginRootDirectory))
                AddPlugin(directory);

            AddPlugin(m_pluginRootDirectory);
        }

        /// <summary>
        /// 从指定路径中加载插件
        /// </summary>
        void AddPlugin(string path)
        {
            foreach (string filename in Directory.GetFiles(path))
            {
                bool isAlreadyInList = false;
                foreach (PluginInfo info in m_plugins)
                {
                    if (info.FullPath == filename)
                    {
                        isAlreadyInList = true;
                        break;
                    }
                }

                if (isAlreadyInList)
                    continue;

                string extension = Path.GetExtension(filename).ToLower();
                if (HasCompiler(extension) || IsPreCompiled(extension))
                {
                    PluginInfo plugin = new PluginInfo();
                    plugin.FullPath = filename;
                    m_plugins.Add(plugin);
                }
            }
        }

        /// <summary>
        /// 加载那些设置了随WorldWind启动时加载的插件
        /// </summary>
        public void LoadStartupPlugins()
        {
            foreach (PluginInfo pi in m_plugins)
            {
                if (pi.IsLoadedAtStartup)
                {
                    try
                    {
                        // 变异
                        Log.Write(Log.Levels.Debug, LogCategory, "loading " + pi.Name + " ...");
                        Load(pi);
                    }
                    catch (Exception caught)
                    {
                        // Plugin failed to load
                        string message = "Plugin " + pi.Name + " failed: " + caught.Message;
                        Log.Write(Log.Levels.Error, LogCategory, message);
                        Log.Write(caught);

                        // Disable automatic load of this plugin on startup
                        pi.IsLoadedAtStartup = false;

                    }
                }
            }
        }

        /// <summary>
        /// 判断一个文件扩展名是否是可编译的插件
        /// </summary>
        /// <param name="fullPath">File extension to check.</param>
        public bool HasCompiler(string fileExtension)
        {
            CodeDomProvider cdp = (CodeDomProvider)codeDomProviders[fileExtension];
            return cdp != null;
        }

        /// <summary>
        /// 判断一个文件扩展名是否是一个预编译的插件
        /// </summary>
        static public bool IsPreCompiled(string fileExtension)
        {
            return fileExtension == ".dll";
        }

        /// <summary>
        /// 根据插件信息加载插件
        /// </summary>
        public void Load(PluginInfo pi)
        {
            if (pi.Plugin == null)
            {
                // 找到合适的编译器
                string extension = Path.GetExtension(pi.FullPath).ToLower();
                Assembly asm = null;
                if (extension == ".dll")
                {
                    asm = Assembly.LoadFile(pi.FullPath);
                }
                else
                {
                    CodeDomProvider cdp = (CodeDomProvider)codeDomProviders[extension];
                    if (cdp == null)
                        return;
                    asm = Compile(pi, cdp);
                }

                pi.Plugin = GetPluginInterface(asm);
            }

            string pluginPath = QRSTWorldGlobeControl.m_PluginsDir;
            if (pi.FullPath != null && pi.FullPath.Length > 0)
                pluginPath = Path.GetDirectoryName(pi.FullPath);

            pi.Plugin.PluginLoad(qrstWorldGlobe, pluginPath);
        }

        /// <summary>
        /// 如果指定插件已经加载，则卸载它
        /// </summary>
        public void Unload(PluginInfo pi)
        {
            if (!pi.IsCurrentlyLoaded)
                return;
            pi.Plugin.PluginUnload();
        }

        /// <summary>
        /// 卸载/删除一个插件
        /// </summary>
        /// <param name="pi"></param>
        public void Uninstall(PluginInfo pi)
        {
            Unload(pi);

            File.Delete(pi.FullPath);

            m_plugins.Remove(pi);
        }

        /// <summary>
        /// 释放资源，关闭插件
        /// </summary>
        public void Dispose()
        {
            foreach (PluginInfo pi in m_plugins)
            {
                try
                {
                    Unload(pi);
                }
                catch (Exception caught)
                {
                    Log.Write(Log.Levels.Error, "PLUG", "Plugin unload failed: " + caught.Message);
                }
            }
        }

        /// <summary>
        /// 使用指定的编译器编译文件到程序集中
        /// </summary>
        Assembly Compile(PluginInfo pi, CodeDomProvider cdp)
        {
            if (cdp is Microsoft.JScript.JScriptCodeProvider)
                // JSCript doesn't support unsafe code
                cp.CompilerOptions = "";
            else
                cp.CompilerOptions = "/unsafe";

            // Add references  添加引用
            cp.ReferencedAssemblies.Clear();
            foreach (string reference in m_worldWindReferencesList)
                cp.ReferencedAssemblies.Add(reference);

            // Add reference to core functions for VB.Net users 
            if (cdp is Microsoft.VisualBasic.VBCodeProvider)
                cp.ReferencedAssemblies.Add("Microsoft.VisualBasic.dll");

            // Add references specified in the plugin
            foreach (string reference in pi.References.Split(','))
                AddCompilerReference(pi.FullPath, reference.Trim());

            CompilerResults cr = cdp.CompileAssemblyFromFile(cp, pi.FullPath);
            if (cr.Errors.HasErrors || cr.Errors.HasWarnings)
            {
                // Handle compiler errors  处理编译错误
                StringBuilder error = new StringBuilder();
                foreach (CompilerError err in cr.Errors)
                {
                    string type = (err.IsWarning ? "Warning" : "Error");
                    if (error.Length > 0)
                        error.Append(Environment.NewLine);
                    error.AppendFormat("{0} {1}: Line {2} Column {3}: {4}", type, err.ErrorNumber, err.Line, err.Column, err.ErrorText);
                }
                Log.Write(Log.Levels.Error, LogCategory, error.ToString());
                if (cr.Errors.HasErrors)
                    throw new Exception(error.ToString());
            }

            // Success, return our new assembly
            return cr.CompiledAssembly;
        }

        /// <summary>
        /// 添加引用到本地程序集或者在全局缓存中的程序集
        /// </summary>
        /// <param name="pluginDirectory">用来搜索的插件目录</param>
        /// <param name="assemblyName">程序集名称</param>
        void AddCompilerReference(string pluginDirectory, string assemblyName)
        {
            try
            {
                if (assemblyName.Length <= 0)
                    return;

                Assembly referencedAssembly = Assembly.Load(assemblyName);
                if (referencedAssembly == null)
                {
                    // Try plug-in directory
                    string pluginReferencePath = Path.Combine(Path.GetDirectoryName(pluginDirectory),
                        assemblyName);
                    referencedAssembly = Assembly.LoadFile(pluginReferencePath);

                    if (referencedAssembly == null)
                        throw new ApplicationException("Search for required library '" + assemblyName + "' failed.");
                }

                cp.ReferencedAssemblies.Add(referencedAssembly.Location);
            }
            catch (Exception caught)
            {
                throw new ApplicationException("Failed to load '" + assemblyName + "': " + caught.Message);
            }
        }

        /// <summary>
        /// 查找派生自“Plugin”的类并返回这个类的实例
        /// </summary>
        static Plugin GetPluginInterface(Assembly asm)
        {
            foreach (Type t in asm.GetTypes())
            {
                if (!t.IsClass)
                    continue;

                if (!t.IsPublic)
                    continue;

                if (t.BaseType != typeof(Plugin))
                    continue;

                try
                {
                    Plugin pluginInstance = (Plugin)asm.CreateInstance(t.ToString());
                    return pluginInstance;
                }
                catch (MissingMethodException)
                {
                    throw;
                }
                catch
                {
                    // Ignore exceptions during entry point search.
                }
            }

            throw new ArgumentException("Plugin does not derive from base class Plugin.");
        }
    }
}
