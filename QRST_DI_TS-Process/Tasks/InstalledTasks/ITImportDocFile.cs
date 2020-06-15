using System;
using System.IO;
using QRST_DI_DS_Metadata.Paths;

namespace QRST_DI_TS_Process.Tasks.InstalledTasks
{
    class ITImportDocFile:TaskClass
    {
        /// <summary>
        /// 任务名,定义唯一标识，不可动态修改
        /// </summary>
        public override string TaskName
        {
            get
            {
                return "ITImportDocFile";
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
            string orderworkspace = ProcessArgu[0];                               //共享路径
            string code = this.ParentOrder.OrderParams[11];

            try
            {
               #region
         //       //提取元数据信息
         //       string[] xmlFiles = Directory.GetFiles(StorageBasePath.SharePath_OrignalData(orderworkspace), "*.xml");
         //       string[] thumbnailFiles = Directory.GetFiles(StorageBasePath.SharePath_OrignalData(orderworkspace), "*.jpg");
         //       string[] srcFiles = Directory.GetFiles(orderworkspace);
         //       string srcFile = "";
         //       if (srcFiles.Length == 0)
         //       {
         //           throw new Exception("没有找到源文件！");
         //       }
         //       else
         //       {
         //           srcFile = srcFiles[0];
         //       }
         //       if (xmlFiles.Length > 0)
         //       {
         //           ParentOrder.Logs.Add("开始元数据入库！");               
         //           MetaDataDoc metadataDoc = new MetaDataDoc();
         //           metadataDoc.ReadAttributes(xmlFiles[0]);
         //           metadataDoc.TITLE = Path.GetFileName(srcFile);
         //           if (!metadataDoc.TITLE.Contains("DOC"))
         //           {
         //               ParentOrder.Logs.Add("数据类型不匹配！");
         //               throw new Exception("数据类型不匹配！");
         //           }
         //           metadataDoc.ImportData(TSPCommonReference.dbOperating.EVDB);
         //           ParentOrder.Logs.Add("完成元数据入库！");

         //           //导入原始数据和纠正后数据
         //           string tableCode = StoragePath.GetTableCodeByQrstCode(metadataDoc.QRST_CODE);
         //           StoragePath storePath = new StoragePath(tableCode);
         //           string destPath = storePath.GetDataPath(metadataDoc.QRST_CODE);
         //           try
         //           {
         //               if (Directory.Exists(destPath)) //删除旧的文件
         //               {
         //                   Directory.Delete(destPath, true);
         //               }
         //               Directory.CreateDirectory(destPath);
         //               //拷贝源文件
         //               ParentOrder.Logs.Add("开始拷贝源文件！");
         //               string srcdestPath = string.Format(@"{0}\{1}", destPath, metadataDoc.TITLE);
         //               if (!File.Exists(srcFile))
         //               {
         //                   ParentOrder.Logs.Add("没有找到源文件！");

         //               }
         //               //ksk修改，将Copy改为move。降低站点所在电脑的磁盘占有量。若存在同名文件，删除旧文件。
         //               //File.Copy(srcFile, srcdestPath, true);


         //               if (File.Exists(srcdestPath))
         //                   File.Delete(srcdestPath);
         //               File.Move(srcFile, srcdestPath);

         //               ParentOrder.Logs.Add("完成拷贝源文件！");

         //               ParentOrder.Logs.Add("开始拷贝缩略图！");
         //               //拷贝缩略图
         //               for (int i = 0; i < thumbnailFiles.Length; i++)
         //               {
         //                   //ksk 添加if语句，功能是生成以QRST_CODE命名的jpg，集成共享使用。
         //                   if (destPath.Contains("zhsjk"))
         //                   {
         //                       string thumbPath = StoragePath.StoreBasePath;
         //                       string thumbFullPath = Path.Combine(thumbPath, "Thumb");
         //                       if (!Directory.Exists(thumbFullPath))
         //                           Directory.CreateDirectory(thumbFullPath);
         //                       if (thumbnailFiles[i].Contains("THUMB"))
         //                       {
         //                           File.Copy(thumbnailFiles[i], Path.Combine(thumbFullPath, metadataDoc.QRST_CODE + ".jpg"), true);
         //                       }
         //                   }
         //                   string thumbnailDestPath = string.Format(@"{0}\{1}", destPath, Path.GetFileName(thumbnailFiles[i]));
         //                   File.Copy(thumbnailFiles[i], thumbnailDestPath, true);
         //               }
         //               ParentOrder.Logs.Add("完成拷贝缩略图！");
         //               this.ParentOrder.Tag = metadataDoc;
         //           }
         //           catch (Exception e)
         //           {
         //               string MySqlCon = Constant.ConnectionStringEVDB;
         //               MySqlConnection con = new MySqlConnection(MySqlCon);
         //               MySqlCommand cmd = new MySqlCommand();
         //               string sql = String.Format("delete from prod_hj1c where QRST_CODE ='{0}'", metadataDoc.QRST_CODE);
         //               cmd.Connection = con;
         //               cmd.CommandText = sql;
         //               try
         //               {
         //                   con.Open();
         //                   cmd.ExecuteNonQuery();
         //               }
         //               catch (Exception ex)
         //               {
         //                   throw ex;
         //               }
         //               finally
         //               {
         //                   con.Close();
         //                   throw e;
         //               }
         //           }

         //           //拷贝校正后数据
         //           //ParentOrder.Logs.Add("开始拷贝拷贝校正后数据！");
         //           //string destCorrectedData = metadataHJ1C.GetCorrectedDataPath();
         //           //if (Directory.Exists(destCorrectedData))
         //           //{
         //           //    Directory.Delete(destCorrectedData, true);
         //           //}
         //           //CopyFolder(StorageBasePath.SharePath_CorrectedData(orderworkspace), destCorrectedData);
         //           //ParentOrder.Logs.Add("完成拷贝校正后数据！");
         //       }
         //       else
         //       {
         //           throw new Exception("没有找到元数据.xml文件！");
         //       }

         //       //是否要删除订单文件夹
         //       // DeleteFolder(orderworkspace);
        #endregion
                            
                //导入原始数据和纠正后数据
                string tableCode = StoragePath.GetTableCodeByQrstCode(code);
                StoragePath storePath = new StoragePath(tableCode);
                string destPath = storePath.GetDataPath(code);
                Directory.CreateDirectory(destPath);             //需要判断路径是否存在我改天在该,不用判断了 这个路径一定不存在的(这个code是新生成的) 必须创建
                //拷贝源文件
                ParentOrder.Logs.Add("开始拷贝源文件！");
               

                string[] srcFiles = Directory.GetFiles(orderworkspace);
                foreach (string src in srcFiles)
                {
                    string srcdestPath = string.Format(@"{0}\{1}", destPath, Path.GetFileName(src));
                    if (!File.Exists(src))
                    {
                        ParentOrder.Logs.Add("没有找到源文件！");

                    }
                   
                    if (File.Exists(srcdestPath))
                        File.Delete(srcdestPath);
                    File.Move(src, srcdestPath);
                   // File.Copy(src, srcdestPath);
                    ParentOrder.Logs.Add("完成拷贝源文件！");
                }
            }
            catch (Exception ex)
            {
                ParentOrder.Logs.Add(ex.ToString());
                throw ex;
            }

        }

    }
}
