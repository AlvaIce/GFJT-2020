using QRST_DI_DS_Metadata.MetaDataCls;
using QRST_DI_DS_Metadata.Paths;
using QRST_DI_Resources;
using QRST_DI_SS_Basis.MetaData;
using QRST_DI_SS_DBInterfaces.IDBEngine;
using QRST_DI_TS_Basis.DirectlyAddress;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace QRST_DI_TS_Process.Tasks.InstalledTasks
{
    class ITGF5DataImport : TaskClass
    {
        //删除次数指示器
        private int delNum = 0;
        private IDbBaseUtilities postgresqlUtilities;
        private IDbOperating postgresqlOperating;

        /// <summary>
        /// GF影像数据入库任务类名称
        /// </summary>
        public override string TaskName
        {
            get
            {
                return "ITGF5DataImport";
            }
            set
            {
            }
        }

        /// <summary>
        /// GF5影像数据入库任务类逻辑
        /// </summary>
		public override void Process()
        {
            this.ParentOrder.Logs.Add(string.Format("开始源数据归档。"));
            string filename = ProcessArgu[1];
            string orderworkspace = ProcessArgu[0];
            try
            {
                string[] xmlFiles = Directory.GetFiles(StorageBasePath.SharePath_OrignalData(orderworkspace), "*.xml");
                string[] thumbnailFiles = Directory.GetFiles(StorageBasePath.SharePath_OrignalData(orderworkspace), "*.jpg");
                string[] srcFiles = Directory.GetFiles(orderworkspace);
                string srcFile = "";
                if (srcFiles.Length == 0)
                {
                    throw new Exception("没有找到源文件！");
                }
                else
                {
                    srcFile = srcFiles[0];
                }
                if (xmlFiles.Length > 0)
                {
                    ParentOrder.Logs.Add("开始元数据入库！");
                    string xmlfile = "";
                    foreach (string xmlFileName in xmlFiles)
                    {
                        if (ITGF5DataQualityInspection.xmlDataCheck(xmlFileName, null))
                        {
                            xmlfile = xmlFileName;
                            break;
                        }
                    }

                    if (xmlfile == "")
                    {
                        ParentOrder.Logs.Add("未找到正确的元数据XML文件！");
                        throw new Exception("未找到正确的元数据XML文件！");
                    }

                    if (filename.Contains("AHSI"))
                    {
                        MetaDataAHSIGF5 metadataGF5 = new MetaDataAHSIGF5();
                        metadataGF5.ReadAttributes(xmlfile);
                        metadataGF5.Name = Path.GetFileName(srcFile);

                        //统计GF1数据量的大小
                        string filePath = Path.Combine(this.ParentOrder.OrderWorkspace, metadataGF5.Name);
                        metadataGF5.size = (new FileInfo(filePath).Length * 1.0) / 1024;

                        if (!metadataGF5.Name.Contains("GF5"))
                        {
                            ParentOrder.Logs.Add("数据类型不匹配！");
                            throw new Exception("数据类型不匹配！");
                        }
                        postgresqlOperating = Constant.IdbOperating;
                        postgresqlUtilities = postgresqlOperating.GetSubDbUtilities(EnumDBType.EVDB);
                        metadataGF5.ImportData(postgresqlUtilities);
                        ParentOrder.Logs.Add("完成元数据入库！");

                        //导入原始数据和纠正后数据
                        string tableCode = StoragePath.GetTableCodeByQrstCode(metadataGF5.QRST_CODE);
                        StoragePath storePath = new StoragePath(tableCode);
                        string destPath = storePath.GetDataPath(metadataGF5.QRST_CODE);
                        try
                        {
                            //删除旧的文件
                            if (Directory.Exists(destPath))
                            {
                                Directory.Delete(destPath, true);
                            }
                            Directory.CreateDirectory(destPath);
                            //拷贝源文件
                            ParentOrder.Logs.Add("开始拷贝源文件！");
                            string srcdestPath = string.Format(@"{0}\{1}", destPath, Path.GetFileName(srcFile));
                            if (!File.Exists(srcFile))
                            {
                                ParentOrder.Logs.Add("没有找到源文件！");
                            }
                            if (File.Exists(srcdestPath))
                                File.Delete(srcdestPath);
                            ParentOrder.Logs.Add("开始拷贝源文件！");
                            File.Move(srcFile, srcdestPath);
                            for (int i = 0; i < xmlFiles.Length; i++)
                            {
                                string path = string.Format(@"{0}\{1}", destPath, Path.GetFileName(xmlFiles[i])); ;
                                try
                                {
                                    File.Move(xmlFiles[i], path);
                                }
                                catch (Exception e)
                                {

                                }
                            }
                            ParentOrder.Logs.Add("完成拷贝源文件！");

                            ParentOrder.Logs.Add("开始拷贝缩略图！");
                            //拷贝缩略图
                            for (int i = 0; i < thumbnailFiles.Length; i++)
                            {
                                //ksk 添加if语句，功能是生成以QRST_CODE命名的jpg，集成共享使用。
                                if (destPath.Contains("zhsjk"))
                                {
                                    string thumbbasePath = Path.Combine(StoragePath.StoreBasePath, "Thumb");

                                    string thumbfilename = metadataGF5.QRST_CODE + ".jpg";   //默认值
                                    int thumbfilecount = Directory.GetFiles(StorageBasePath.SharePath_OrignalData(orderworkspace), "*_thumb.jpg").Length;
                                    if (thumbfilecount < 2)
                                    {
                                        thumbfilename = metadataGF5.QRST_CODE + ".jpg";
                                    }
                                    else
                                    {
                                        if (thumbnailFiles[i].Contains("AHSI"))
                                        {
                                            thumbfilename = metadataGF5.QRST_CODE + "-" + i + ".jpg";
                                        }
                                        else
                                        {
                                            thumbfilename = metadataGF5.QRST_CODE + "-" + i + ".jpg";
                                        }
                                    }
                                    string thumbFullPath = StoragePath.GetThumbPathByFileName(thumbbasePath, thumbfilename);
                                    if (!Directory.Exists(thumbFullPath)) Directory.CreateDirectory(thumbFullPath);
                                    try
                                    {
                                        File.Copy(thumbnailFiles[i], Path.Combine(thumbFullPath, thumbfilename), true);
                                    }
                                    catch { }

                                }
                                string thumbnailDestPath = string.Format(@"{0}\{1}", destPath, Path.GetFileName(thumbnailFiles[i]));
                                try
                                {
                                    File.Copy(thumbnailFiles[i], thumbnailDestPath, true);
                                }
                                catch { }
                            }
                            ParentOrder.Logs.Add("完成拷贝缩略图！");
                            //站点文件目录下删除入库成功订单文件夹@jianghua 2015.8.1.
                            deleateOrderFiles(orderworkspace);

                            //通知生产线进行0-2级数据切片
                            //try
                            //{
                            //    HproseTcpClient tcpClient = new HproseTcpClient(Constant.cutTileRPCAddress);
                            //    tcpClient.Invoke("CutTiles", new string[] { srcdestPath });
                            //}catch(Exception e)
                            //{
                            //    throw new Exception("生产线切片算法服务异常");
                            //}

                        }
                        catch (Exception e)
                        {
                            if (Directory.Exists(destPath)) //删除旧的文件
                            {
                                Directory.Delete(destPath, true);
                            }

                            string postgresqlCon = Constant.ConnectionStringEVDB;
                            postgresqlOperating = Constant.IdbOperating;
                            postgresqlUtilities = postgresqlOperating.GetSubDbUtilities(EnumDBType.EVDB);
                            string sql = String.Format("delete from prod_ahsi_gf5 where QRST_CODE ='{0}'", metadataGF5.QRST_CODE);
                            postgresqlUtilities.ExecuteSql(sql);
                            deleateOrderFiles(orderworkspace);
                            throw new Exception(e.Message.ToString());
                        }
                    }
                    else if (filename.Contains("VIMS"))
                    {
                        MetaDataVIMSGF5 metadataGF5 = new MetaDataVIMSGF5();
                        metadataGF5.ReadAttributes(xmlfile);
                        metadataGF5.Name = Path.GetFileName(srcFile);

                        //统计GF1数据量的大小
                        string filePath = Path.Combine(this.ParentOrder.OrderWorkspace, metadataGF5.Name);
                        metadataGF5.size = (new FileInfo(filePath).Length * 1.0) / 1024;

                        if (!metadataGF5.Name.Contains("GF5"))
                        {
                            ParentOrder.Logs.Add("数据类型不匹配！");
                            throw new Exception("数据类型不匹配！");
                        }
                        postgresqlOperating = Constant.IdbOperating;
                        postgresqlUtilities = postgresqlOperating.GetSubDbUtilities(EnumDBType.EVDB);
                        metadataGF5.ImportData(postgresqlUtilities);
                        ParentOrder.Logs.Add("完成元数据入库！");

                        //导入原始数据和纠正后数据
                        string tableCode = StoragePath.GetTableCodeByQrstCode(metadataGF5.QRST_CODE);
                        StoragePath storePath = new StoragePath(tableCode);
                        string destPath = storePath.GetDataPath(metadataGF5.QRST_CODE);
                        try
                        {
                            //删除旧的文件
                            if (Directory.Exists(destPath))
                            {
                                Directory.Delete(destPath, true);
                            }
                            Directory.CreateDirectory(destPath);
                            //拷贝源文件
                            ParentOrder.Logs.Add("开始拷贝源文件！");
                            string srcdestPath = string.Format(@"{0}\{1}", destPath, Path.GetFileName(srcFile));
                            if (!File.Exists(srcFile))
                            {
                                ParentOrder.Logs.Add("没有找到源文件！");
                            }
                            if (File.Exists(srcdestPath))
                                File.Delete(srcdestPath);
                            ParentOrder.Logs.Add("开始拷贝源文件！");
                            File.Move(srcFile, srcdestPath);

                            ParentOrder.Logs.Add("完成拷贝源文件！");

                            ParentOrder.Logs.Add("开始拷贝缩略图！");
                            //拷贝缩略图
                            for (int i = 0; i < thumbnailFiles.Length; i++)
                            {
                                //ksk 添加if语句，功能是生成以QRST_CODE命名的jpg，集成共享使用。
                                if (destPath.Contains("zhsjk"))
                                {
                                    string thumbbasePath = Path.Combine(StoragePath.StoreBasePath, "Thumb");

                                    string thumbfilename = metadataGF5.QRST_CODE + ".jpg";   //默认值
                                    int thumbfilecount = Directory.GetFiles(StorageBasePath.SharePath_OrignalData(orderworkspace), "*_thumb.jpg").Length;
                                    if (thumbfilecount < 2)
                                    {
                                        thumbfilename = metadataGF5.QRST_CODE + ".jpg";
                                    }
                                    else
                                    {
                                        if (thumbnailFiles[i].Contains("VIMS"))
                                        {
                                            thumbfilename = metadataGF5.QRST_CODE + "-" + i + ".jpg";
                                        }
                                        else
                                        {
                                            thumbfilename = metadataGF5.QRST_CODE + "-" + i + ".jpg";
                                        }
                                    }
                                    string thumbFullPath = StoragePath.GetThumbPathByFileName(thumbbasePath, thumbfilename);
                                    if (!Directory.Exists(thumbFullPath)) Directory.CreateDirectory(thumbFullPath);
                                    try
                                    {
                                        File.Copy(thumbnailFiles[i], Path.Combine(thumbFullPath, thumbfilename), true);
                                    }
                                    catch { }

                                }
                                string thumbnailDestPath = string.Format(@"{0}\{1}", destPath, Path.GetFileName(thumbnailFiles[i]));
                                try
                                {
                                    File.Copy(thumbnailFiles[i], thumbnailDestPath, true);
                                }
                                catch { }
                            }
                            ParentOrder.Logs.Add("完成拷贝缩略图！");
                            //站点文件目录下删除入库成功订单文件夹@jianghua 2015.8.1.
                            deleateOrderFiles(orderworkspace);

                            //通知生产线进行0-2级数据切片
                            //try
                            //{
                            //    HproseTcpClient tcpClient = new HproseTcpClient(Constant.cutTileRPCAddress);
                            //    tcpClient.Invoke("CutTiles", new string[] { srcdestPath });
                            //}catch(Exception e)
                            //{
                            //    throw new Exception("生产线切片算法服务异常");
                            //}

                        }
                        catch (Exception e)
                        {
                            if (Directory.Exists(destPath)) //删除旧的文件
                            {
                                Directory.Delete(destPath, true);
                            }

                            string postgresqlCon = Constant.ConnectionStringEVDB;
                            postgresqlOperating = Constant.IdbOperating;
                            postgresqlUtilities = postgresqlOperating.GetSubDbUtilities(EnumDBType.EVDB);
                            string sql = String.Format("delete from prod_vims_gf5 where QRST_CODE ='{0}'", metadataGF5.QRST_CODE);
                            postgresqlUtilities.ExecuteSql(sql);
                            deleateOrderFiles(orderworkspace);
                            throw new Exception(e.Message.ToString());
                        }
                    }
                    else if (filename.Contains("DPC"))
                    {
                        MetaDataDPCGF5 metadataGF5 = new MetaDataDPCGF5();
                        metadataGF5.ReadAttributes(xmlfile);
                        metadataGF5.Name = Path.GetFileName(srcFile);

                        //统计GF1数据量的大小
                        string filePath = Path.Combine(this.ParentOrder.OrderWorkspace, metadataGF5.Name);
                        metadataGF5.size = (new FileInfo(filePath).Length * 1.0) / 1024;

                        if (!metadataGF5.Name.Contains("GF5"))
                        {
                            ParentOrder.Logs.Add("数据类型不匹配！");
                            throw new Exception("数据类型不匹配！");
                        }
                        postgresqlOperating = Constant.IdbOperating;
                        postgresqlUtilities = postgresqlOperating.GetSubDbUtilities(EnumDBType.EVDB);
                        metadataGF5.ImportData(postgresqlUtilities);
                        ParentOrder.Logs.Add("完成元数据入库！");

                        //导入原始数据和纠正后数据
                        string tableCode = StoragePath.GetTableCodeByQrstCode(metadataGF5.QRST_CODE);
                        StoragePath storePath = new StoragePath(tableCode);
                        string destPath = storePath.GetDataPath(metadataGF5.QRST_CODE);
                        try
                        {
                            //删除旧的文件
                            if (Directory.Exists(destPath))
                            {
                                Directory.Delete(destPath, true);
                            }
                            Directory.CreateDirectory(destPath);
                            //拷贝源文件
                            ParentOrder.Logs.Add("开始拷贝源文件！");
                            string srcdestPath = string.Format(@"{0}\{1}", destPath, Path.GetFileName(srcFile));
                            if (!File.Exists(srcFile))
                            {
                                ParentOrder.Logs.Add("没有找到源文件！");
                            }
                            if (File.Exists(srcdestPath))
                                File.Delete(srcdestPath);
                            ParentOrder.Logs.Add("开始拷贝源文件！");
                            File.Move(srcFile, srcdestPath);

                            ParentOrder.Logs.Add("完成拷贝源文件！");

                            ParentOrder.Logs.Add("开始拷贝缩略图！");
                            //拷贝缩略图
                            for (int i = 0; i < thumbnailFiles.Length; i++)
                            {
                                //ksk 添加if语句，功能是生成以QRST_CODE命名的jpg，集成共享使用。
                                if (destPath.Contains("zhsjk"))
                                {
                                    string thumbbasePath = Path.Combine(StoragePath.StoreBasePath, "Thumb");

                                    string thumbfilename = metadataGF5.QRST_CODE + ".jpg";   //默认值
                                    int thumbfilecount = Directory.GetFiles(StorageBasePath.SharePath_OrignalData(orderworkspace), "*_thumb.jpg").Length;
                                    if (thumbfilecount < 2)
                                    {
                                        thumbfilename = metadataGF5.QRST_CODE + ".jpg";
                                    }
                                    else
                                    {
                                        if (thumbnailFiles[i].Contains("VIMS"))
                                        {
                                            thumbfilename = metadataGF5.QRST_CODE + "-" + i + ".jpg";
                                        }
                                        else
                                        {
                                            thumbfilename = metadataGF5.QRST_CODE + "-" + i + ".jpg";
                                        }
                                    }
                                    string thumbFullPath = StoragePath.GetThumbPathByFileName(thumbbasePath, thumbfilename);
                                    if (!Directory.Exists(thumbFullPath)) Directory.CreateDirectory(thumbFullPath);
                                    try
                                    {
                                        File.Copy(thumbnailFiles[i], Path.Combine(thumbFullPath, thumbfilename), true);
                                    }
                                    catch { }

                                }
                                string thumbnailDestPath = string.Format(@"{0}\{1}", destPath, Path.GetFileName(thumbnailFiles[i]));
                                try
                                {
                                    File.Copy(thumbnailFiles[i], thumbnailDestPath, true);
                                }
                                catch { }
                            }
                            ParentOrder.Logs.Add("完成拷贝缩略图！");
                            //站点文件目录下删除入库成功订单文件夹@jianghua 2015.8.1.
                            deleateOrderFiles(orderworkspace);

                            //通知生产线进行0-2级数据切片
                            //try
                            //{
                            //    HproseTcpClient tcpClient = new HproseTcpClient(Constant.cutTileRPCAddress);
                            //    tcpClient.Invoke("CutTiles", new string[] { srcdestPath });
                            //}catch(Exception e)
                            //{
                            //    throw new Exception("生产线切片算法服务异常");
                            //}

                        }
                        catch (Exception e)
                        {
                            if (Directory.Exists(destPath)) //删除旧的文件
                            {
                                Directory.Delete(destPath, true);
                            }

                            string postgresqlCon = Constant.ConnectionStringEVDB;
                            postgresqlOperating = Constant.IdbOperating;
                            postgresqlUtilities = postgresqlOperating.GetSubDbUtilities(EnumDBType.EVDB);
                            string sql = String.Format("delete from prod_dpc_gf5 where QRST_CODE ='{0}'", metadataGF5.QRST_CODE);
                            postgresqlUtilities.ExecuteSql(sql);
                            deleateOrderFiles(orderworkspace);
                            throw new Exception(e.Message.ToString());
                        }
                    }
                    else if (filename.Contains("GMI"))
                    {
                        MetaDataGMIGF5 metadataGF5 = new MetaDataGMIGF5();
                        metadataGF5.ReadAttributes(xmlfile);
                        metadataGF5.Name = Path.GetFileName(srcFile);

                        //统计GF1数据量的大小
                        string filePath = Path.Combine(this.ParentOrder.OrderWorkspace, metadataGF5.Name);
                        metadataGF5.size = (new FileInfo(filePath).Length * 1.0) / 1024;

                        if (!metadataGF5.Name.Contains("GF5"))
                        {
                            ParentOrder.Logs.Add("数据类型不匹配！");
                            throw new Exception("数据类型不匹配！");
                        }
                        postgresqlOperating = Constant.IdbOperating;
                        postgresqlUtilities = postgresqlOperating.GetSubDbUtilities(EnumDBType.EVDB);
                        metadataGF5.ImportData(postgresqlUtilities);
                        ParentOrder.Logs.Add("完成元数据入库！");

                        //导入原始数据和纠正后数据
                        string tableCode = StoragePath.GetTableCodeByQrstCode(metadataGF5.QRST_CODE);
                        StoragePath storePath = new StoragePath(tableCode);
                        string destPath = storePath.GetDataPath(metadataGF5.QRST_CODE);
                        try
                        {
                            //删除旧的文件
                            if (Directory.Exists(destPath))
                            {
                                Directory.Delete(destPath, true);
                            }
                            Directory.CreateDirectory(destPath);
                            //拷贝源文件
                            ParentOrder.Logs.Add("开始拷贝源文件！");
                            string srcdestPath = string.Format(@"{0}\{1}", destPath, Path.GetFileName(srcFile));
                            if (!File.Exists(srcFile))
                            {
                                ParentOrder.Logs.Add("没有找到源文件！");
                            }
                            if (File.Exists(srcdestPath))
                                File.Delete(srcdestPath);
                            ParentOrder.Logs.Add("开始拷贝源文件！");
                            File.Move(srcFile, srcdestPath);

                            ParentOrder.Logs.Add("完成拷贝源文件！");

                            ParentOrder.Logs.Add("开始拷贝缩略图！");
                            //拷贝缩略图
                            for (int i = 0; i < thumbnailFiles.Length; i++)
                            {
                                //ksk 添加if语句，功能是生成以QRST_CODE命名的jpg，集成共享使用。
                                if (destPath.Contains("zhsjk"))
                                {
                                    string thumbbasePath = Path.Combine(StoragePath.StoreBasePath, "Thumb");

                                    string thumbfilename = metadataGF5.QRST_CODE + ".jpg";   //默认值
                                    int thumbfilecount = Directory.GetFiles(StorageBasePath.SharePath_OrignalData(orderworkspace), "*_thumb.jpg").Length;
                                    if (thumbfilecount < 2)
                                    {
                                        thumbfilename = metadataGF5.QRST_CODE + ".jpg";
                                    }
                                    else
                                    {
                                        if (thumbnailFiles[i].Contains("AHSI"))
                                        {
                                            thumbfilename = metadataGF5.QRST_CODE + "-" + i + ".jpg";
                                        }
                                        else
                                        {
                                            thumbfilename = metadataGF5.QRST_CODE + "-" + i + ".jpg";
                                        }
                                    }
                                    string thumbFullPath = StoragePath.GetThumbPathByFileName(thumbbasePath, thumbfilename);
                                    if (!Directory.Exists(thumbFullPath)) Directory.CreateDirectory(thumbFullPath);
                                    try
                                    {
                                        File.Copy(thumbnailFiles[i], Path.Combine(thumbFullPath, thumbfilename), true);
                                    }
                                    catch { }

                                }
                                string thumbnailDestPath = string.Format(@"{0}\{1}", destPath, Path.GetFileName(thumbnailFiles[i]));
                                try
                                {
                                    File.Copy(thumbnailFiles[i], thumbnailDestPath, true);
                                }
                                catch { }
                            }
                            ParentOrder.Logs.Add("完成拷贝缩略图！");
                            //站点文件目录下删除入库成功订单文件夹@jianghua 2015.8.1.
                            deleateOrderFiles(orderworkspace);

                            //通知生产线进行0-2级数据切片
                            //try
                            //{
                            //    HproseTcpClient tcpClient = new HproseTcpClient(Constant.cutTileRPCAddress);
                            //    tcpClient.Invoke("CutTiles", new string[] { srcdestPath });
                            //}catch(Exception e)
                            //{
                            //    throw new Exception("生产线切片算法服务异常");
                            //}

                        }
                        catch (Exception e)
                        {
                            if (Directory.Exists(destPath)) //删除旧的文件
                            {
                                Directory.Delete(destPath, true);
                            }

                            string postgresqlCon = Constant.ConnectionStringEVDB;
                            postgresqlOperating = Constant.IdbOperating;
                            postgresqlUtilities = postgresqlOperating.GetSubDbUtilities(EnumDBType.EVDB);
                            string sql = String.Format("delete from prod_gmi_gf5 where QRST_CODE ='{0}'", metadataGF5.QRST_CODE);
                            postgresqlUtilities.ExecuteSql(sql);
                            deleateOrderFiles(orderworkspace);
                            throw new Exception(e.Message.ToString());
                        }
                    }
                    else if (filename.Contains("EMI"))
                    {
                        MetaDataEMIGF5 metadataGF5 = new MetaDataEMIGF5();
                        metadataGF5.ReadAttributes(xmlfile);
                        metadataGF5.Name = Path.GetFileName(srcFile);

                        //统计GF1数据量的大小
                        string filePath = Path.Combine(this.ParentOrder.OrderWorkspace, metadataGF5.Name);
                        metadataGF5.size = (new FileInfo(filePath).Length * 1.0) / 1024;

                        if (!metadataGF5.Name.Contains("GF5"))
                        {
                            ParentOrder.Logs.Add("数据类型不匹配！");
                            throw new Exception("数据类型不匹配！");
                        }
                        postgresqlOperating = Constant.IdbOperating;
                        postgresqlUtilities = postgresqlOperating.GetSubDbUtilities(EnumDBType.EVDB);
                        metadataGF5.ImportData(postgresqlUtilities);
                        ParentOrder.Logs.Add("完成元数据入库！");

                        //导入原始数据和纠正后数据
                        string tableCode = StoragePath.GetTableCodeByQrstCode(metadataGF5.QRST_CODE);
                        StoragePath storePath = new StoragePath(tableCode);
                        string destPath = storePath.GetDataPath(metadataGF5.QRST_CODE);
                        try
                        {
                            //删除旧的文件
                            if (Directory.Exists(destPath))
                            {
                                Directory.Delete(destPath, true);
                            }
                            Directory.CreateDirectory(destPath);
                            //拷贝源文件
                            ParentOrder.Logs.Add("开始拷贝源文件！");
                            string srcdestPath = string.Format(@"{0}\{1}", destPath, Path.GetFileName(srcFile));
                            if (!File.Exists(srcFile))
                            {
                                ParentOrder.Logs.Add("没有找到源文件！");
                            }
                            if (File.Exists(srcdestPath))
                                File.Delete(srcdestPath);
                            ParentOrder.Logs.Add("开始拷贝源文件！");
                            File.Move(srcFile, srcdestPath);

                            ParentOrder.Logs.Add("完成拷贝源文件！");

                            ParentOrder.Logs.Add("开始拷贝缩略图！");
                            //拷贝缩略图
                            for (int i = 0; i < thumbnailFiles.Length; i++)
                            {
                                //ksk 添加if语句，功能是生成以QRST_CODE命名的jpg，集成共享使用。
                                if (destPath.Contains("zhsjk"))
                                {
                                    string thumbbasePath = Path.Combine(StoragePath.StoreBasePath, "Thumb");

                                    string thumbfilename = metadataGF5.QRST_CODE + ".jpg";   //默认值
                                    int thumbfilecount = Directory.GetFiles(StorageBasePath.SharePath_OrignalData(orderworkspace), "*_thumb.jpg").Length;
                                    if (thumbfilecount < 2)
                                    {
                                        thumbfilename = metadataGF5.QRST_CODE + ".jpg";
                                    }
                                    else
                                    {
                                        if (thumbnailFiles[i].Contains("AHSI"))
                                        {
                                            thumbfilename = metadataGF5.QRST_CODE + "-" + i + ".jpg";
                                        }
                                        else
                                        {
                                            thumbfilename = metadataGF5.QRST_CODE + "-" + i + ".jpg";
                                        }
                                    }
                                    string thumbFullPath = StoragePath.GetThumbPathByFileName(thumbbasePath, thumbfilename);
                                    if (!Directory.Exists(thumbFullPath)) Directory.CreateDirectory(thumbFullPath);
                                    try
                                    {
                                        File.Copy(thumbnailFiles[i], Path.Combine(thumbFullPath, thumbfilename), true);
                                    }
                                    catch { }

                                }
                                string thumbnailDestPath = string.Format(@"{0}\{1}", destPath, Path.GetFileName(thumbnailFiles[i]));
                                try
                                {
                                    File.Copy(thumbnailFiles[i], thumbnailDestPath, true);
                                }
                                catch { }
                            }
                            ParentOrder.Logs.Add("完成拷贝缩略图！");
                            //站点文件目录下删除入库成功订单文件夹@jianghua 2015.8.1.
                            deleateOrderFiles(orderworkspace);

                            //通知生产线进行0-2级数据切片
                            //try
                            //{
                            //    HproseTcpClient tcpClient = new HproseTcpClient(Constant.cutTileRPCAddress);
                            //    tcpClient.Invoke("CutTiles", new string[] { srcdestPath });
                            //}catch(Exception e)
                            //{
                            //    throw new Exception("生产线切片算法服务异常");
                            //}

                        }
                        catch (Exception e)
                        {
                            if (Directory.Exists(destPath)) //删除旧的文件
                            {
                                Directory.Delete(destPath, true);
                            }

                            string postgresqlCon = Constant.ConnectionStringEVDB;
                            postgresqlOperating = Constant.IdbOperating;
                            postgresqlUtilities = postgresqlOperating.GetSubDbUtilities(EnumDBType.EVDB);
                            string sql = String.Format("delete from prod_emi_gf5 where QRST_CODE ='{0}'", metadataGF5.QRST_CODE);
                            postgresqlUtilities.ExecuteSql(sql);
                            deleateOrderFiles(orderworkspace);
                            throw new Exception(e.Message.ToString());
                        }
                    }
                    else if (filename.Contains("AIUS"))
                    {
                        MetaDataAIUSGF5 metadataGF5 = new MetaDataAIUSGF5();
                        metadataGF5.ReadAttributes(xmlfile);
                        metadataGF5.Name = Path.GetFileName(srcFile);

                        //统计GF5数据量的大小
                        string filePath = Path.Combine(this.ParentOrder.OrderWorkspace, metadataGF5.Name);
                        metadataGF5.size = (new FileInfo(filePath).Length * 1.0) / 1024;

                        if (!metadataGF5.Name.Contains("GF5"))
                        {
                            ParentOrder.Logs.Add("数据类型不匹配！");
                            throw new Exception("数据类型不匹配！");
                        }
                        postgresqlOperating = Constant.IdbOperating;
                        postgresqlUtilities = postgresqlOperating.GetSubDbUtilities(EnumDBType.EVDB);
                        metadataGF5.ImportData(postgresqlUtilities);
                        ParentOrder.Logs.Add("完成元数据入库！");

                        //导入原始数据和纠正后数据
                        string tableCode = StoragePath.GetTableCodeByQrstCode(metadataGF5.QRST_CODE);
                        StoragePath storePath = new StoragePath(tableCode);
                        string destPath = storePath.GetDataPath(metadataGF5.QRST_CODE);
                        try
                        {
                            //删除旧的文件
                            if (Directory.Exists(destPath))
                            {
                                Directory.Delete(destPath, true);
                            }
                            Directory.CreateDirectory(destPath);
                            //拷贝源文件
                            ParentOrder.Logs.Add("开始拷贝源文件！");
                            string srcdestPath = string.Format(@"{0}\{1}", destPath, Path.GetFileName(srcFile));
                            if (!File.Exists(srcFile))
                            {
                                ParentOrder.Logs.Add("没有找到源文件！");
                            }
                            if (File.Exists(srcdestPath))
                                File.Delete(srcdestPath);
                            ParentOrder.Logs.Add("开始拷贝源文件！");
                            File.Move(srcFile, srcdestPath);

                            ParentOrder.Logs.Add("完成拷贝源文件！");

                            ParentOrder.Logs.Add("开始拷贝缩略图！");
                            //拷贝缩略图
                            for (int i = 0; i < thumbnailFiles.Length; i++)
                            {
                                //ksk 添加if语句，功能是生成以QRST_CODE命名的jpg，集成共享使用。
                                if (destPath.Contains("zhsjk"))
                                {
                                    string thumbbasePath = Path.Combine(StoragePath.StoreBasePath, "Thumb");

                                    string thumbfilename = metadataGF5.QRST_CODE + ".jpg";   //默认值
                                    int thumbfilecount = Directory.GetFiles(StorageBasePath.SharePath_OrignalData(orderworkspace), "*_thumb.jpg").Length;
                                    if (thumbfilecount < 2)
                                    {
                                        thumbfilename = metadataGF5.QRST_CODE + ".jpg";
                                    }
                                    else
                                    {
                                        if (thumbnailFiles[i].Contains("AIUS"))
                                        {
                                            thumbfilename = metadataGF5.QRST_CODE + "-" + i + ".jpg";
                                        }
                                        else
                                        {
                                            thumbfilename = metadataGF5.QRST_CODE + "-" + i + ".jpg";
                                        }
                                    }
                                    string thumbFullPath = StoragePath.GetThumbPathByFileName(thumbbasePath, thumbfilename);
                                    if (!Directory.Exists(thumbFullPath)) Directory.CreateDirectory(thumbFullPath);
                                    try
                                    {
                                        File.Copy(thumbnailFiles[i], Path.Combine(thumbFullPath, thumbfilename), true);
                                    }
                                    catch { }

                                }
                                string thumbnailDestPath = string.Format(@"{0}\{1}", destPath, Path.GetFileName(thumbnailFiles[i]));
                                try
                                {
                                    File.Copy(thumbnailFiles[i], thumbnailDestPath, true);
                                }
                                catch { }
                            }
                            ParentOrder.Logs.Add("完成拷贝缩略图！");
                            //站点文件目录下删除入库成功订单文件夹@jianghua 2015.8.1.
                            deleateOrderFiles(orderworkspace);

                            //通知生产线进行0-2级数据切片
                            //try
                            //{
                            //    HproseTcpClient tcpClient = new HproseTcpClient(Constant.cutTileRPCAddress);
                            //    tcpClient.Invoke("CutTiles", new string[] { srcdestPath });
                            //}catch(Exception e)
                            //{
                            //    throw new Exception("生产线切片算法服务异常");
                            //}

                        }
                        catch (Exception e)
                        {
                            if (Directory.Exists(destPath)) //删除旧的文件
                            {
                                Directory.Delete(destPath, true);
                            }

                            string postgresqlCon = Constant.ConnectionStringEVDB;
                            postgresqlOperating = Constant.IdbOperating;
                            postgresqlUtilities = postgresqlOperating.GetSubDbUtilities(EnumDBType.EVDB);
                            string sql = String.Format("delete from prod_aius_gf5 where QRST_CODE ='{0}'", metadataGF5.QRST_CODE);
                            postgresqlUtilities.ExecuteSql(sql);
                            deleateOrderFiles(orderworkspace);
                            throw new Exception(e.Message.ToString());
                        }
                    }
                }
                else
                {
                    throw new Exception("没有找到元数据.xml文件！");
                }
            }
            catch (Exception ex)
            {
                ParentOrder.Logs.Add(ex.ToString());
                throw ex;
            }

        }

        /// <summary>
        /// 删除执行成功的订单文件夹
        /// </summary>
        /// <param name="path"></param>
        public void deleateOrderFiles(string orderSpace)
        {
            try
            {
                //清空内存，以防文件占用冲突
                if (Directory.Exists(orderSpace))
                {
                    Directory.Delete(orderSpace, true);
                    ParentOrder.Logs.Add("删除订单文件夹成功！");
                }
                else
                    return;
            }
            catch (Exception ex)
            {
                if (delNum < 20)
                {
                    delNum++;
                    deleateOrderFiles(orderSpace);
                }
                else
                    ParentOrder.Logs.Add(ex.ToString());
            }
        }
    }
}
