using System;
using System.Collections.Generic;
using System.IO;
using QRST_DI_TS_Process.JCGXCallBack;
using QRST_DI_Resources;

namespace QRST_DI_TS_Process.Tasks.InstalledTasks
{
    public class ITGetMetaDataFile: TaskClass
    {
        /// <summary>
        /// 任务名,定义唯一标识，不可动态修改
        /// </summary>
        public override string TaskName
        {
            get { return "ITGetMetaDataFile"; }
            set { }
        }

        public override void Process()
        {
            string sourceFileAddress = this.ProcessArgu[0];
            string opIP = this.ProcessArgu[1];
            Exception exResult=null;
            //调用java包获取网络目标地址存放的xml文件,将该文件存入订单所在的工作空间
            try
            {
                this.ParentOrder.Logs.Add("开始获取数据元数据文件！");
                #region  获取元数据文件方案1：在局域网内部拷贝
                //先假设在一个局域网内部拷贝文件
				if (opIP == "zhsjk")
				{
					if (System.IO.Directory.Exists(sourceFileAddress))
					{
						DirectoryInfo dirInfo = new DirectoryInfo(sourceFileAddress);
						List<FileInfo> fileInfoLst = new List<FileInfo>();
						fileInfoLst.AddRange(dirInfo.GetFiles("*.xml"));
						if (fileInfoLst.Count > 0)
						{
							string filePath = string.Format(@"{0}\{1}", sourceFileAddress, fileInfoLst[0].ToString());
							string destPath = this.ParentOrder.OrderWorkspace + fileInfoLst[0].ToString();
							System.IO.File.Copy(filePath, destPath);
							return;
						}
						else
						{
							throw new Exception("获取数据元数据文件失败");
						}
					}
				}
                #endregion

                #region 获取元数据文件方案2：从数据总线获取
                QRST_DI_DS_DataTransfer.FileListInfo fileinfoLst = QRST_DI_DS_DataTransfer.DataTransferByDataBus.GetFileList(Constant.DataBusServerUrl, sourceFileAddress);
                string xmlFile = "";
                if (fileinfoLst != null && fileinfoLst.file_set != null)
                {
                    for (int i = 0; i < fileinfoLst.file_set.Length; i++)
                    {
                        if (fileinfoLst.file_set[i].file_name.EndsWith(".xml") || fileinfoLst.file_set[i].file_name.EndsWith(".XML"))
                        {
                            xmlFile = fileinfoLst.file_set[i].file_name;
                            break;
                        }
                    }
                    if (xmlFile == "")
                    {
                        throw new Exception("没有找到元数据文件");
                    }
                    else
                    {
                        //将元数据文件下载到工作空间
                        string errorMsg = "";
                        string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                        if (QRST_DI_DS_DataTransfer.DataTransferByDataBus.DownLoadData(Constant.DataBusServerUrl, sourceFileAddress, xmlFile, this.ParentOrder.OrderWorkspace, appDirectory, out errorMsg))
                        {
                        }
                        else
                        {
                            throw new Exception("元数据文件下载失败：" + errorMsg);
                        }
                    }
                }
                else
                {
                    throw new Exception("获取数据文件列表失败");
                }
                #endregion
                this.ParentOrder.Logs.Add("成功获取元数据文件！");
            }
            catch (Exception ex)
            {
                this.ParentOrder.Logs.Add("获取数据元数据文件失败："+ex.ToString());
                exResult = ex;
                throw ex;
            }
            finally
            {
                //调用集成共享的CallBack_Down方法
                try
                {
                    if (exResult != null)
                    {
                        using (WS_DB_CallbackPortTypeClient client = new WS_DB_CallbackPortTypeClient(Constant.JcgxEndPointName))
                        {
                            int i = 0;
                            i = client.CallBack_Up(opIP, exResult.ToString());
                            this.ParentOrder.Logs.Add("完成集成共享消息发送！" + "元数据文件获取失败");
                        }
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
