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
    public class ITGF6DataImport : TaskClass
    {

        private int delNum = 0;     //删除次数指示器
        private IDbBaseUtilities postgresqlUtilities;
        private IDbOperating postgresqlOperating;

        public override string TaskName
        {
            get
            {
                return "ITGF6DataImport";
            }
            set
            {
            }
        }

        public override void Process()
        {

            //数据阵列 数据归档
            //  迁移数据至归档目录
            this.ParentOrder.Logs.Add(string.Format("开始源数据归档。"));
            string orderworkspace = ProcessArgu[0];
            try
            {
                //提取元数据信息
                string[] xmlFiles = Directory.GetFiles(StorageBasePath.SharePath_OrignalData(orderworkspace), "*.xml");
                string[] thumbnailFiles = Directory.GetFiles(StorageBasePath.SharePath_OrignalData(orderworkspace), "*.jpg");
                string[] srcFiles = Directory.GetFiles(orderworkspace); //\\192.168.200.44\QRST_DB_Share\P3439748780451\GF6_PMS_E39.1_N26.2_20190221_L1A1119849734.tar.gz
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
                    foreach (String xmlFileName in xmlFiles)
                    {
                        if (ITGF6DataQualityInspection.xmlDataCheck(xmlFileName, null))
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



                    MetaDataGF6 metadataGF6 = new MetaDataGF6();

                    metadataGF6.ReadAttributes(xmlfile);
                    metadataGF6.Name = Path.GetFileName(srcFile);

                    string filePath = Path.Combine(this.ParentOrder.OrderWorkspace, metadataGF6.Name);
                    metadataGF6.size = (new FileInfo(filePath).Length * 1.0) / 1024;

                    if (!metadataGF6.Name.Contains("GF6"))
                    {
                        ParentOrder.Logs.Add("数据类型不匹配！");
                        throw new Exception("数据类型不匹配！");
                    }
                    postgresqlOperating = Constant.IdbOperating;
                    postgresqlUtilities = postgresqlOperating.GetSubDbUtilities(EnumDBType.EVDB);
                    metadataGF6.ImportData(postgresqlUtilities);
                    ParentOrder.Logs.Add("完成元数据入库！");

                    //导入原始数据和纠正后数据
                    string tableCode = StoragePath.GetTableCodeByQrstCode(metadataGF6.QRST_CODE);
                    StoragePath storePath = new StoragePath(tableCode);
                    string destPath = storePath.GetDataPath(metadataGF6.QRST_CODE);
                    try
                    {
                        if (Directory.Exists(destPath)) //删除旧的文件
                        {
                            Directory.Delete(destPath, true);
                        }
                        Directory.CreateDirectory(destPath);
                        //拷贝源文件
                        ParentOrder.Logs.Add("开始拷贝源文件！");
                        string srcdestPath = string.Format(@"{0}\{1}", destPath, metadataGF6.Name);  //
                        if (!File.Exists(srcFile))
                        {
                            ParentOrder.Logs.Add("没有找到源文件！");
                        }
                        //ksk修改，将Copy改为move。降低站点所在电脑的磁盘占有量。若存在同名文件，删除旧文件。
                        //File.Copy(srcFile, srcdestPath, true);

                        if (File.Exists(srcdestPath))
                            File.Delete(srcdestPath);
                        ParentOrder.Logs.Add("开始拷贝源文件！");
                        File.Move(srcFile, srcdestPath);

                        ParentOrder.Logs.Add("完成拷贝源文件！");

                        ParentOrder.Logs.Add("开始拷贝缩略图！");
                        //拷贝缩略图
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

                                string thumbfilename = metadataGF6.QRST_CODE + ".jpg";   //默认值
                                int thumbfilecount = Directory.GetFiles(StorageBasePath.SharePath_OrignalData(orderworkspace), "*_thumb.jpg").Length;
                                if (thumbfilecount < 2)
                                {
                                    thumbfilename = metadataGF6.QRST_CODE + ".jpg";
                                }
                                else
                                {
                                    if (thumbnailFiles[i].Contains("WFV"))
                                    {
                                        thumbfilename = metadataGF6.QRST_CODE + "-" + i + ".jpg";
                                    }
                                    else if (thumbnailFiles[i].Contains("MSS"))
                                    {
                                        thumbfilename = metadataGF6.QRST_CODE + "-" + i + ".jpg";
                                    }
                                    else if (thumbnailFiles[i].Contains("IRS"))
                                    {
                                        thumbfilename = metadataGF6.QRST_CODE + "-" + i + ".jpg";
                                    }
                                    else if (thumbnailFiles[i].Contains("PAN"))
                                    {
                                        thumbfilename = metadataGF6.QRST_CODE + "-" + i + ".jpg";
                                    }
                                    else if (thumbnailFiles[i].Contains("PMS"))
                                    {
                                        thumbfilename = metadataGF6.QRST_CODE + "-" + i + ".jpg";
                                    }
                                    else
                                    {
                                        thumbfilename = metadataGF6.QRST_CODE + "-" + i + ".jpg";
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
                        this.ParentOrder.Tag = metadataGF6;
                        /*if (ProcessArgu[1].ToLower() == "true")
                        {
                            //通知生产线进行0 - 2级数据切片
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
                        string sql = String.Format("delete from prod_gf6 where QRST_CODE ='{0}'", metadataGF6.QRST_CODE);
                        postgresqlUtilities.ExecuteSql(sql);
                        deleateOrderFiles(orderworkspace);
                        this.ParentOrder.Tag = metadataGF6;
                        throw new Exception(e.Message.ToString());

                    }


                }
                else
                {
                    throw new Exception("没有找到元数据.xml文件！");
                }

                //是否要删除订单文件夹
                // DeleteFolder(orderworkspace);
            }
            catch (Exception ex)
            {
                ParentOrder.Logs.Add(ex.ToString());
                throw ex;
            }

        }

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
