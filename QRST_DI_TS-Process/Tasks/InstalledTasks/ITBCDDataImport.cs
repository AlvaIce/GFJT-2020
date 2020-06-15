using System;
using System.IO;
using QRST_DI_TS_Basis.DirectlyAddress;
using QRST_DI_DS_Metadata.MetaDataCls;
using QRST_DI_DS_Metadata.Paths;
using QRST_DI_Resources;
using QRST_DI_SS_Basis.MetaData;
using QRST_DI_SS_DBInterfaces.IDBEngine;

namespace QRST_DI_TS_Process.Tasks.InstalledTasks
{
    /// <summary>
    /// BCD数据入库任务类
    /// </summary>
    class ITBCDDataImport : TaskClass
    {
        //删除次数指示器
        private int delNum = 0;
        private IDbBaseUtilities _baseUtilities;
        private IDbOperating _sqLiteOperating;

        /// <summary>
        /// BCD数据入库任务类名称
        /// </summary>
        public override string TaskName
        {
            get
            {
                return "ITBCDDataImport";
            }
            set
            {
            }
        }

        /// <summary>
        /// BCD数据入库任务类逻辑
        /// </summary>
		public override void Process()
        {
            this.ParentOrder.Logs.Add(string.Format("开始源数据归档。"));
            string orderworkspace = ProcessArgu[0];                               //订单工作空间
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
                    foreach (String xmlFileName in xmlFiles)// 遍历所有的xml文件，
                    {
                        if (ITBCDDataQualityInspection.xmlDataCheck(xmlFileName, null))
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

                    //读取BCD数据元数据
                    MetaDataBCD metadatabcd = new MetaDataBCD();
                    metadatabcd.ReadAttributes(xmlfile);
                    metadatabcd.Name = Path.GetFileName(srcFile);

                    //统计数据量的大小
                    string filePath = Path.Combine(this.ParentOrder.OrderWorkspace, metadatabcd.Name);
                    metadatabcd.size = (new FileInfo(filePath).Length * 1.0) / 1024;


                    if (!metadatabcd.Name.Contains("GF1B") && !metadatabcd.Name.Contains("GF1C") && !metadatabcd.Name.Contains("GF1D"))
                    {
                        ParentOrder.Logs.Add("数据类型不匹配！");
                        throw new Exception("数据类型不匹配！");
                    }
                    _sqLiteOperating = Constant.IdbOperating;
                    _baseUtilities = _sqLiteOperating.GetSubDbUtilities(EnumDBType.EVDB);
                    metadatabcd.ImportData(_baseUtilities);
                    ParentOrder.Logs.Add("完成元数据入库！");

                    //导入原始数据和纠正后数据
                    string tableCode = StoragePath.GetTableCodeByQrstCode(metadatabcd.QRST_CODE);
                    StoragePath storePath = new StoragePath(tableCode);
                    string destPath = storePath.GetDataPath(metadatabcd.QRST_CODE);
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
                        string srcdestPath = string.Format(@"{0}\{1}", destPath, metadatabcd.Name);
                        if (!File.Exists(srcFile))
                        {
                            ParentOrder.Logs.Add("没有找到源文件！");
                        }

                        if (File.Exists(srcdestPath))
                            File.Delete(srcdestPath);
                        ParentOrder.Logs.Add("开始拷贝源文件！");
                        File.Move(srcFile, srcdestPath);
                        ParentOrder.Logs.Add("完成拷贝源文件！");

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
                        //拷贝缩略图
                        for (int i = 0; i < thumbnailFiles.Length; i++)
                        {
                            //ksk 添加if语句，功能是生成以QRST_CODE命名的jpg，集成共享使用。
                            if (destPath.Contains("zhsjk"))
                            {
                                string thumbbasePath = Path.Combine(StoragePath.StoreBasePath, "Thumb");

                                string thumbfilename = metadatabcd.QRST_CODE + ".jpg";   //默认值
                                int thumbfilecount = Directory.GetFiles(StorageBasePath.SharePath_OrignalData(orderworkspace), "*_thumb.jpg").Length;
                                if (thumbfilecount < 2)
                                {
                                    thumbfilename = metadatabcd.QRST_CODE + ".jpg";
                                }
                                else
                                {
                                    if (thumbnailFiles[i].Contains("WFV"))
                                    {
                                        thumbfilename = metadatabcd.QRST_CODE + "-" + i + ".jpg";
                                    }
                                    else if (thumbnailFiles[i].Contains("MSS"))
                                    {
                                        thumbfilename = metadatabcd.QRST_CODE + "-" + i + ".jpg";
                                    }
                                    else if (thumbnailFiles[i].Contains("IRS"))
                                    {
                                        thumbfilename = metadatabcd.QRST_CODE + "-" + i + ".jpg";
                                    }
                                    else if (thumbnailFiles[i].Contains("PAN"))
                                    {
                                        thumbfilename = metadatabcd.QRST_CODE + "-" + i + ".jpg";
                                    }
                                    else if (thumbnailFiles[i].Contains("PMS"))
                                    {
                                        thumbfilename = metadatabcd.QRST_CODE + "-" + i + ".jpg";
                                    }
                                    else
                                    {
                                        thumbfilename = metadatabcd.QRST_CODE + "-" + i + ".jpg";
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

                        // RPC回调接口，此处无需
                        /*if (ProcessArgu[1].ToLower() == "true")
                        {
                            //通知生产线进行0-2级数据切片
                            try
                            {
                                HproseTcpClient tcpClient = new HproseTcpClient(Constant.cutTileRPCAddress);
                                tcpClient.Invoke("CutTiles", new string[] { srcdestPath });
                            }
                            catch (Exception e)
                            {
                                throw new Exception("生产线切片算法服务异常");
                            }
                        }*/


                        deleateOrderFiles(orderworkspace);
                        this.ParentOrder.Tag = metadatabcd;
                    }
                    catch (Exception e)
                    {
                        //删除旧的文件
                        if (Directory.Exists(destPath))
                        {
                            Directory.Delete(destPath, true);
                        }

                        string conn = Constant.ConnectionStringEVDB;
                        _sqLiteOperating = Constant.IdbOperating;
                        _baseUtilities = _sqLiteOperating.GetSubDbUtilities(EnumDBType.EVDB);
                        string sql = String.Format("delete from prod_gf1bcd where QRST_CODE ='{0}'", metadatabcd.QRST_CODE);
                        _baseUtilities.ExecuteSql(sql);
                        deleateOrderFiles(orderworkspace);
                        this.ParentOrder.Tag = metadatabcd;
                        throw new Exception(e.Message.ToString());
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
