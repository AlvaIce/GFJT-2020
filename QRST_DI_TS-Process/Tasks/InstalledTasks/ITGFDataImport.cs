using System;
using System.IO;
using QRST_DI_TS_Basis.DirectlyAddress;
using QRST_DI_DS_Metadata.MetaDataCls;
using QRST_DI_DS_Metadata.Paths;
using QRST_DI_Resources;
using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetaData;
using QRST_DI_SS_DBInterfaces.IDBEngine;

namespace QRST_DI_TS_Process.Tasks.InstalledTasks
{
    class ITGFDataImport : TaskClass
	{
        private int delNum = 0;     //删除次数指示器
        private IDbBaseUtilities _baseUtilities;
        private IDbOperating _sqLiteOperating;
        /// <summary>
        /// 任务名,定义唯一标识，不可动态修改
        /// </summary>
        public override string TaskName
		{
			get
			{
				return "ITGFDataImport";
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
			string orderworkspace = ProcessArgu[0];                               //订单工作空间
			try
			{
				//提取元数据信息
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
                    string xmlfile ="";
                    foreach (String xmlFileName in xmlFiles)
                    {
                        if (ITDataQualityInspection.xmlDataCheck(xmlFileName, null))
                        {
                            xmlfile = xmlFileName;
                            break;
                        }
                    }

                    if (xmlfile=="")
                    {
                        ParentOrder.Logs.Add("未找到正确的元数据XML文件！");
                        throw new Exception("未找到正确的元数据XML文件！");
                    }

					MetaDataGF1 metadataGf1 = new MetaDataGF1();
                    metadataGf1.ReadAttributes(xmlfile);
					metadataGf1.Name = Path.GetFileName(srcFile);
                    if (!metadataGf1.Name.Contains("GF1") && !metadataGf1.Name.Contains("GF2")&&!metadataGf1.Name.Contains("GF4"))
					{
						ParentOrder.Logs.Add("数据类型不匹配！");
						throw new Exception("数据类型不匹配！");
					}
                    _sqLiteOperating = Constant.IdbOperating;
                    _baseUtilities = _sqLiteOperating.GetSubDbUtilities(EnumDBType.EVDB);
                    metadataGf1.ImportData(_baseUtilities);
					ParentOrder.Logs.Add("完成元数据入库！");

					//导入原始数据和纠正后数据
					string tableCode = StoragePath.GetTableCodeByQrstCode(metadataGf1.QRST_CODE);
					StoragePath storePath = new StoragePath(tableCode);
					string destPath = storePath.GetDataPath(metadataGf1.QRST_CODE);
					try
					{
						if (Directory.Exists(destPath)) //删除旧的文件
						{
							Directory.Delete(destPath, true);
						}
						Directory.CreateDirectory(destPath);
						//拷贝源文件
						ParentOrder.Logs.Add("开始拷贝源文件！");
						string srcdestPath = string.Format(@"{0}\{1}", destPath, metadataGf1.Name);
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
                        for (int i = 0; i < thumbnailFiles.Length; i++)
                        {
                            //ksk 添加if语句，功能是生成以QRST_CODE命名的jpg，集成共享使用。
                            if (destPath.Contains("zhsjk"))
                            {
                                //int index = destPath.IndexOf("zhsjk");
                                string thumbbasePath = Path.Combine(StoragePath.StoreBasePath, "Thumb");
                                if (thumbnailFiles[i].Contains("thumb"))
                                {
                                    string thumbfilename = metadataGf1.QRST_CODE + ".jpg";   //默认值
                                    int thumbfilecount = Directory.GetFiles(StorageBasePath.SharePath_OrignalData(orderworkspace), "*_thumb.jpg").Length;
                                    if (thumbfilecount < 2)
                                    {
                                        //当只有1个缩略图的时候就不加-1了
                                        thumbfilename = metadataGf1.QRST_CODE + ".jpg";
                                    }
                                    else
                                    {
                                        if (thumbnailFiles[i].Contains("WFV"))
                                        {
                                            thumbfilename = metadataGf1.QRST_CODE + ".jpg";
                                        }
                                        else if (thumbnailFiles[i].Contains("MSS"))
                                        {
                                            thumbfilename = metadataGf1.QRST_CODE + ".jpg";
                                        }
                                        else if (thumbnailFiles[i].Contains("IRS"))
                                        {
                                            thumbfilename = metadataGf1.QRST_CODE + "-1.jpg";   //。。。-1.jpg代表是黑白的，候补快视图
                                        }
                                        else if (thumbnailFiles[i].Contains("PAN"))
                                        {
                                            thumbfilename = metadataGf1.QRST_CODE + "-1.jpg";
                                        }
                                        else if (thumbnailFiles[i].Contains("PMS"))
                                        {
                                            thumbfilename = metadataGf1.QRST_CODE + ".jpg"; //GF4数据没有按MSS PAN区分只有IRS和PMS
                                        }
                                        else
                                        {
                                            thumbfilename = metadataGf1.QRST_CODE + "-1.jpg";
                                        }
                                    }

                                    string thumbFullPath = StoragePath.GetThumbPathByFileName(thumbbasePath, thumbfilename);
                                    if (!Directory.Exists(thumbFullPath)) Directory.CreateDirectory(thumbFullPath);
                                    try
                                    {       //当快视图正被加载中时是无法覆盖的
                                        File.Copy(thumbnailFiles[i], Path.Combine(thumbFullPath, thumbfilename), true);
                                    }
                                    catch { }
                                }
                            }
                            string thumbnailDestPath = string.Format(@"{0}\{1}", destPath, Path.GetFileName(thumbnailFiles[i]));
                            try
                            {       //当快视图正被加载中时是无法覆盖的
                                File.Copy(thumbnailFiles[i], thumbnailDestPath, true);
                            }
                            catch { }
                        }
						ParentOrder.Logs.Add("完成拷贝缩略图！");
                        //站点文件目录下删除入库成功订单文件夹@jianghua 2015.8.1.
                        deleateOrderFiles(orderworkspace);
						this.ParentOrder.Tag = metadataGf1;
					}
					catch(Exception e)
                    {
                        if (Directory.Exists(destPath)) //删除旧的文件
                        {
                            Directory.Delete(destPath, true);
                        }

						string MySqlCon = Constant.ConnectionStringEVDB;
                        _sqLiteOperating = Constant.IdbOperating;
                        _baseUtilities = _sqLiteOperating.GetSubDbUtilities(EnumDBType.EVDB);
                        string sql = String.Format("delete from prod_gf1 where QRST_CODE ='{0}'", metadataGf1.QRST_CODE);
                        _baseUtilities.ExecuteSql(sql);
      //                  MySqlConnection con = new MySqlConnection(MySqlCon);
						//MySqlCommand cmd = new MySqlCommand();
						//cmd.Connection = con;
						//cmd.CommandText = sql;
						//try
						//{
						//	con.Open();
						//	cmd.ExecuteNonQuery();
						//}
						//catch(Exception ex)
						//{
						//	throw ex;
						//}
						//finally
						//{
						//	con.Close();
						//	throw e;
						//}

					}

					//拷贝校正后数据
					//ParentOrder.Logs.Add("开始拷贝拷贝校正后数据！");
					//string destCorrectedData = metadataGf1.GetCorrectedDataPath();
					//if (Directory.Exists(destCorrectedData))
					//{
					//    Directory.Delete(destCorrectedData, true);
					//}
					//CopyFolder(StorageBasePath.SharePath_CorrectedData(orderworkspace), destCorrectedData);
					//ParentOrder.Logs.Add("完成拷贝校正后数据！");
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
                    Directory.Delete(orderSpace,true);
                    ParentOrder.Logs.Add("删除订单文件夹成功！");
                }
                else
                    return;
            }
            catch(Exception ex)
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
