/*
 * 作者：zxw
 * 创建时间：2013-09-24
 * 描述：
*/
using System;
using QRST_DI_DS_Metadata.Paths;
using System.IO;
using System.Reflection;
using QRST_DI_DS_Metadata.MetaDataCls;
using QRST_DI_DS_Metadata.DataImportPlugin;
using QRST_DI_SS_DBInterfaces.IDBEngine;
using QRST_DI_Resources;

namespace QRST_DI_TS_Process.Tasks.InstalledTasks
{
    public class ITNormalDataImport : TaskClass
    {
        private static IDbOperating _sqLiteOperating = Constant.IdbOperating;
        private IDbBaseUtilities _baseUtilities;
        /// <summary>
        /// 任务名,定义唯一标识，不可动态修改
        /// </summary>
        public override string TaskName
        {
            get { return "ITNormalDataImport"; }
            set { }
        }

        public override void Process()
        {
            try
            {
                if(this.ProcessArgu.Length<=0)
                {
                    throw new Exception("该任务没有赋予参数！");
                }
                string dataCode = this.ProcessArgu[0];
                string pluginPara = this.ProcessArgu[1];
                taskdef _taskdef = taskdef.CreateTaskDefByName(dataCode);
                if(_taskdef==null||string.IsNullOrEmpty(_taskdef.ProcessExec))
                {
                    throw new Exception(string.Format("没能获取到'{0}'的注册插件信息！"));
                }
                 //获取数据导入插件
                string localPluginPath = StoragePath.GetLocalPluginPath(_taskdef.ProcessExec);
           
                if (File.Exists(localPluginPath))
                {
                    //检查组件的版本时间，如果本地版本不是最新，则下载最新版本的组件
                    DateTime localCreateTime= taskdef.GetDllCreateTime(localPluginPath);
                    if (_taskdef.CreateTime > localCreateTime)  //组件不是最新版本，删除本地版本
                    {
                        Directory.Delete(Path.GetDirectoryName(localPluginPath),true);
                    }
                }
                if (!File.Exists(localPluginPath))    //若本地数据导入插件不存在，则从公共插件库中寻找插件，并下载到本地
                {
                    string publicPluginPath = StoragePath.GetPluginPath(_taskdef.ProcessExec);
                    if(!File.Exists(publicPluginPath))
                    {
                        throw new Exception(string.Format("没能找到注册的插件'{0}'",_taskdef.ProcessExec));
                    }

                    //将公共插件库中的插件下载到本地
                    string localPluginDir = Path.GetDirectoryName(localPluginPath);
                    if (!Directory.Exists(localPluginDir))
                    {
                        Directory.CreateDirectory(localPluginDir);
                    }
                    string []relateFiles = Directory.GetFiles(Path.GetDirectoryName(publicPluginPath));
                    for (int i = 0; i < relateFiles.Length;i++ )
                    {
                        File.Copy(relateFiles[i], string.Format(@"{0}\{1}", localPluginDir,Path.GetFileName(relateFiles[i])),true);
                    }
                }

                Assembly assembil = Assembly.LoadFile(localPluginPath);
                Type[] types = assembil.GetTypes();
                //查找实现了IDataImport接口的类型
                Type interfaceType = null;
                Type dataImportType = null;
                for (int i = 0; i < types.Length;i++ )
                {
                    Type[] interfacetypes = types[i].GetInterfaces();
                    for (int j = 0; j < interfacetypes.Length;j++ )
                    {
                        if (interfacetypes[j].Name == "IDataImport")
                        {
                            interfaceType = interfacetypes[j];
                            dataImportType = types[i];
                            break;
                        }
                    }
                    if(interfaceType != null)
                    {
                        break;
                    }
                }
                if(interfaceType == null)
                {
                    throw new Exception(string.Format("在插件'{0}'中没能找到数据导入的实现！", Path.GetFileName(localPluginPath)));
                }
                //构造数据导入对象
                IDataImport dataImportObj = (IDataImport)Activator.CreateInstance(dataImportType);
                //设置导入对象参数
                pluginPara = pluginPara.TrimEnd(";".ToCharArray());
                string[] paras = pluginPara.Split(";".ToCharArray());
                dataImportObj.SetParameter(paras);
                //设置父订单
                dataImportObj.SetParentOrder(this.ParentOrder);
                //数据环境准备
                this.ParentOrder.Logs.Add("开始进行数据入库准备！");
                if (!dataImportObj.DataPrepare())
                {
                    throw new Exception("数据准备失败！");
                }
                //提取导入元数据信息
                MetaDataObject metadataObj = new MetaDataObject();
                this.ParentOrder.Logs.Add("开始解析元数据！");
                metadataObj.fieldVvalue = dataImportObj.GetMetadata();
                metadataObj.relatePath = metadataObj.fieldVvalue["relatePath"];
                _baseUtilities = _sqLiteOperating.GetsqlBaseObj(dataCode.Substring(0, 4));

                //MySqlBaseUtilities sqlBase = TSPCommonReference.dbOperating.GetsqlBaseObj(dataCode.Substring(0,4));
                metadataObj.table_code = dataCode;
                metadataObj.keyField = dataImportObj.GetKeyField();
                metadataObj.ImportData(_baseUtilities);
                this.ParentOrder.Logs.Add("导入元数据成功！");
                //导入原始数据
                this.ParentOrder.Logs.Add("开始原始数据入库！");
                string[] srcPaths = dataImportObj.GetSourceFilePath();
                StoragePath storePath = new StoragePath(dataCode);
                string destPath = storePath.GetDataPath(metadataObj.QRST_CODE);
                if (Directory.Exists(destPath)) //删除旧的文件
                {
                    Directory.Delete(destPath, true);
                }
                Directory.CreateDirectory(destPath);
                for (int i = 0; i < srcPaths.Length;i++ )  //拷贝文件
                {
                    string srcdestPath = string.Format(@"{0}\{1}", destPath, Path.GetFileName(srcPaths[i]));
                    File.Copy(srcPaths[i],srcdestPath,true);
                }
                this.ParentOrder.Logs.Add("原始数据入库完成！");
                //导入缩略图信息
                string[] Thumbnails = dataImportObj.GetThnmbnail();
                if(Thumbnails.Length >0)
                {
                    this.ParentOrder.Logs.Add("开始缩略图入库！");
                    for (int i = 0; i < Thumbnails.Length;i++ )
                    {
                        //ksk 添加if语句，功能是生成以QRST_CODE命名的jpg，集成共享使用。
                        if (destPath.Contains("zhsjk"))
                        {
                            string thumbbasePath = Path.Combine(StoragePath.StoreBasePath, "Thumb");
                            string thumbfilename = metadataObj.QRST_CODE + ".jpg";   //默认值
                            string thumbFullPath = StoragePath.GetThumbPathByFileName(thumbbasePath, thumbfilename);
                            if (!Directory.Exists(thumbFullPath)) Directory.CreateDirectory(thumbFullPath);
                            try
                            {       //当快视图正被加载中时是无法覆盖的
                                File.Copy(Thumbnails[i], Path.Combine(thumbFullPath, thumbfilename), true);
                            }
                            catch { }
                        }
                        string thumbnailDestPath = string.Format(@"{0}\{1}", destPath, Path.GetFileName(Thumbnails[i]));
                        try
                        {       //当快视图正被加载中时是无法覆盖的
                            File.Copy(Thumbnails[i], thumbnailDestPath, true);
                        }
                        catch { } 
                    }
                    this.ParentOrder.Logs.Add("缩略图入库完成！");
                }
                this.ParentOrder.Logs.Add("数据入库完成！");
            }
            catch (Exception ex)
            {
                this.ParentOrder.Logs.Add(string.Format("数据导入异常：{0}", ex.Message));
                throw ex;
            }
        }
    }
}
