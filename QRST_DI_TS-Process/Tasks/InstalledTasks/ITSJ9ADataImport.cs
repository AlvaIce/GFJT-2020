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
    class ITSJ9ADataImport : TaskClass
    {
        private static IDbOperating _sqLiteOperating = Constant.IdbOperating;
        private IDbBaseUtilities _baseUtilities;
		/// <summary>
		/// 任务名,定义唯一标识，不可动态修改
		/// </summary>
		public override string TaskName
		{
			get
			{
				return "ITSJ9ADataImport";
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
					MetaDataSJ9A metadataSj9A = new MetaDataSJ9A();
					metadataSj9A.ReadAttributes(xmlFiles[0]);
					metadataSj9A.Name = Path.GetFileName(srcFile);
					if (!metadataSj9A.Name.Contains("SJ"))
					{
						ParentOrder.Logs.Add("数据类型不匹配！");
						throw new Exception("数据类型不匹配！");
					}
				    _baseUtilities = _sqLiteOperating.GetSubDbUtilities(EnumDBType.EVDB);
                    metadataSj9A.ImportData(_baseUtilities);
					ParentOrder.Logs.Add("完成元数据入库！");

					//导入原始数据和纠正后数据
					string tableCode = StoragePath.GetTableCodeByQrstCode(metadataSj9A.QRST_CODE);
					StoragePath storePath = new StoragePath(tableCode);
					string destPath = storePath.GetDataPath(metadataSj9A.QRST_CODE);
					try
					{
						if (Directory.Exists(destPath)) //删除旧的文件
						{
							Directory.Delete(destPath, true);
						}
						Directory.CreateDirectory(destPath);
						//拷贝源文件
						ParentOrder.Logs.Add("开始拷贝源文件！");
						string srcdestPath = string.Format(@"{0}\{1}", destPath, metadataSj9A.Name);
						if (!File.Exists(srcFile))
						{
							ParentOrder.Logs.Add("没有找到源文件！");

						}
						//ksk修改，将Copy改为move。降低站点所在电脑的磁盘占有量。若存在同名文件，删除旧文件。
						//File.Copy(srcFile, srcdestPath, true);


						if (File.Exists(srcdestPath))
							File.Delete(srcdestPath);
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
                                string thumbfilename = metadataSj9A.QRST_CODE + ".jpg";   //默认值
                                string thumbFullPath = StoragePath.GetThumbPathByFileName(thumbbasePath, thumbfilename);
                                if (!Directory.Exists(thumbFullPath)) Directory.CreateDirectory(thumbFullPath);
                                try
                                {       //当快视图正被加载中时是无法覆盖的
                                    File.Copy(thumbnailFiles[i], Path.Combine(thumbFullPath, thumbfilename), true);
                                }
                                catch { }
                            }
                            string thumbnailDestPath = string.Format(@"{0}\{1}", destPath, Path.GetFileName(thumbnailFiles[i]));
                            try
                            {       //当快视图正被加载中时是无法覆盖的
                                File.Copy(thumbnailFiles[i], thumbnailDestPath, true);
                            }
                            catch { } 
						}
						ParentOrder.Logs.Add("完成拷贝缩略图！");

						this.ParentOrder.Tag = metadataSj9A;
					}
					catch (Exception e)
					{
						string MySqlCon = Constant.ConnectionStringEVDB;
					    _baseUtilities = _sqLiteOperating.GetSubDbUtilities(EnumDBType.EVDB);
                        string sql = String.Format("delete from prod_sj9a where QRST_CODE ='{0}'", metadataSj9A.QRST_CODE);
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
						//catch (Exception ex)
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
					//string destCorrectedData = metadataSj9A.GetCorrectedDataPath();
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

	}
}
