using System;
using System.Collections.Generic;
using System.IO;
using QRST_DI_DS_Metadata.MetaDataCls;
using QRST_DI_DS_Metadata.Paths;
using QRST_DI_TS_Process.JCGXCallBack;
using QRST_DI_Resources;
using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetaData;
using QRST_DI_SS_DBInterfaces.IDBEngine;

namespace QRST_DI_TS_Process.Tasks.InstalledTasks
{
    public class ITImportUserData:TaskClass
    {
        private static IDbOperating _sqLiteOperating = Constant.IdbOperating;
        private static IDbBaseUtilities _baseUtilities = null;
        /// <summary>
        /// 任务名,定义唯一标识，不可动态修改
        /// </summary>
        public override string TaskName
        {
            get { return "ITImportUserData"; }
            set { }
        }

        

        public override void Process()
        {
            Exception exResult = null;    //用于记录任务是否发生异常

            string dataType = this.ProcessArgu[0];
            string sourceFileAddress = this.ProcessArgu[1];
            string opIP = this.ProcessArgu[2];
            MetaData metaData = null;
            try
            {
                this.ParentOrder.Logs.Add("开始解析元数据信息！");
                DirectoryInfo dirInfo = new DirectoryInfo(this.ParentOrder.OrderWorkspace);
                List<System.IO.FileInfo> fileInfoLst = new List<System.IO.FileInfo>();
                fileInfoLst.AddRange(dirInfo.GetFiles("*.xml"));
                
                if (fileInfoLst.Count > 0)
                {
                    try
                    {
                        switch (dataType)
                        {
                            case "AlgorithmCmp":
                                {
                                    metaData = new MetaAlgorithmCmp();
                                    break;
                                }
                            case "Toolkit":
                                {
                                    metaData = new MetaDataUserToolKit();
                                    break;
                                }
                            case "Document":
                                {
                                    metaData = new MetaDataUserDocument();
                                    break;
                                }
                            case "UserRaster":
                                {
                                    metaData = new MetaDataUserRaster();
                                    break;
                                }
                            case "StandardTool":
                                {
                                    metaData = new MetaDataStandardToolkit();
                                    break;
                                }
                        }
                        if (metaData == null)
                        {
                            throw new Exception(String.Format("没有找到对应的入库类{0}型！", dataType));
                        }
                        else
                        {
                            metaData.ReadAttributes(string.Format(@"{0}\{1}",this.ParentOrder.OrderWorkspace, fileInfoLst[0].ToString()));
                            this.ParentOrder.Logs.Add("成功解析元数据！");
                            if (metaData is MetaDataStandardToolkit)
                            {
                                _baseUtilities = _sqLiteOperating.GetSubDbUtilities(EnumDBType.MADB);
                                metaData.ImportData(_baseUtilities);
                            }
                            else
                            {
                                _baseUtilities = _sqLiteOperating.GetSubDbUtilities(EnumDBType.ISDB);
                                metaData.ImportData(_baseUtilities);
                            }
                            
                            this.ParentOrder.Logs.Add("元数据入库成功！");

                            //将数据入库
                            this.ParentOrder.Logs.Add("开始导入数据！");
                            //获取数据应该存放的完整路径
                            string tableCode = StoragePath.GetTableCodeByQrstCode(metaData.QRST_CODE);
                            StoragePath storePath = new StoragePath(tableCode);
                            string destPath = storePath.GetDataPath(metaData.QRST_CODE);

                            if (!Directory.Exists(destPath))
                            {
                                Directory.CreateDirectory(destPath);
                            }

                            #region  方案1：采用局域网内部拷贝文件 zxw
                            //将数据拷贝到目标路径，调用数据中心Java包（这里先用局域网内部拷贝做测试）
							if (opIP == "zhsjk")
							{
								DirectoryInfo direInfo = new DirectoryInfo(sourceFileAddress);
								List<System.IO.FileInfo> fileInfoLst1 = new List<System.IO.FileInfo>();
								fileInfoLst1.AddRange(direInfo.GetFiles("*.tar.gz"));
								if (fileInfoLst1.Count > 0)
								{
									destPath = string.Format(@"{0}\{1}", destPath, fileInfoLst1[0].ToString());
									File.Copy(string.Format(@"{0}\{1}", sourceFileAddress, fileInfoLst1[0].ToString()), destPath);
									this.ParentOrder.Logs.Add("数据导入完成！");
									return;
								}
								else
								{
									throw new Exception("没有找到数据文件");
								}								
							}
                            #endregion

                            #region 方案2：采用数据总线传输 zxw
                            QRST_DI_DS_DataTransfer.FileListInfo fileinfoLst = QRST_DI_DS_DataTransfer.DataTransferByDataBus.GetFileList(Constant.DataBusServerUrl, sourceFileAddress);
                            //获取上传的源文件
                            string targzFile = "";
                            if (fileinfoLst != null && fileinfoLst.file_set != null)
                            {
                                for (int i = 0; i < fileinfoLst.file_set.Length; i++)
                                {
                                    if (fileinfoLst.file_set[i].file_name.EndsWith(".tar.gz") || fileinfoLst.file_set[i].file_name.EndsWith(".TAR.GZ"))
                                    {
                                        targzFile = fileinfoLst.file_set[i].file_name;
                                        break;
                                    }
                                }
                                if (targzFile == "")
                                {
                                    throw new Exception("没有找到数据文件");
                                }
                                else
                                {
                                    //将数据文件下载到工作空间
                                    string errorMsg = "";
                                    string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                                    destPath = destPath + @"\";
                                    if (QRST_DI_DS_DataTransfer.DataTransferByDataBus.DownLoadData(Constant.DataBusServerUrl, sourceFileAddress, targzFile, destPath, appDirectory, out errorMsg))
                                    {
                                        //提取工具数据的公共信息
                                        if (metaData is MetaDataStandardToolkit)
                                        {
                                            //提取公共信息
                                            this.ParentOrder.Logs.Add("开始录入公共信息！");
                                            DirectoryInfo d = new DirectoryInfo(destPath);
                                            double filesize = 0;
                                            if(d.GetFiles().Length >0)
                                            {
                                                filesize = d.GetFiles()[0].Length/(1024*1024);
                                            }
                                            MetaDataStandardPublicInfo publicinfo = new MetaDataStandardPublicInfo();
                                            publicinfo.DataCode = metaData.QRST_CODE;
                                            publicinfo.DataSize = filesize;
                                            _baseUtilities = _sqLiteOperating.GetSubDbUtilities(EnumDBType.MADB);
                                            MetaDataStandardPublicInfo.Add(publicinfo, _baseUtilities);
                                            this.ParentOrder.Logs.Add("公共信息录入完毕！");
                                        }


                                        this.ParentOrder.Logs.Add("数据导入完成！");
                                    }
                                    else
                                    {
                                        throw new Exception("数据导入失败：" + errorMsg);
                                    }
                                }
                            }
                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                else
                {
                    throw new Exception("未找到元数据的xml文件！");
                }
            }
            catch (Exception ex)
            {
                exResult = ex;
                //删除没能导入成功的数据残余
                if(metaData != null)
                {
                    MetaData.DeleteData(metaData.QRST_CODE);
                }
                throw ex;
            }
            finally   //向集成共享发送消息
            {
				if (opIP != "zhsjk")
				{
					//调用集成共享提供的CallBack_Up方法，这里模拟为向控制台输出
					try
					{
						using (WS_DB_CallbackPortTypeClient client = new WS_DB_CallbackPortTypeClient(Constant.JcgxEndPointName))
						{
							int i = 0;
							if (exResult == null)
								i = client.CallBack_Up(opIP, "OK");
							else
								i = client.CallBack_Up(opIP, "数据入库失败！");
							this.ParentOrder.Logs.Add("完成集成共享消息发送！" + i.ToString());
						}
					}
					catch (Exception ex)
					{
						this.ParentOrder.Logs.Add("集成共享消息发送失败：" + ex.ToString());
					}
				}
            }
        }
    }
}
