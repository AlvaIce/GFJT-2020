using System;
using System.Net;
using System.IO;
using QRST_DI_TS_Process.JCGXCallBack;
using QRST_DI_Resources;
using QRST_DI_TS_Basis.DirectlyAddress;
using QRST_DI_TS_Process.Site;
 
namespace QRST_DI_TS_Process.Tasks.InstalledTasks
{
    //瓦片下载任务
    class ITDownLoadTiles : TaskClass
	{
		/// <summary>
		/// 任务名,定义唯一标识，不可动态修改
		/// </summary>
		public override string TaskName
		{
			get
			{
				return "ITDownLoadTiles";
			}
			set
			{
			}
		}

		public override void Process()
		{
			string tilepaths = this.ProcessArgu[0];    //切片地址
			//string destPath = this.ProcessArgu[1];     //目的地址
			string opID = this.ProcessArgu[2];

			Exception exResult = null;
			string[] isFTPUPLoad = opID.Split('#');
			try
			{
				//解析数据编码
				this.ParentOrder.Logs.Add("开始解析数据编码！");

				//获取数据文件

				if (isFTPUPLoad[0] == "WPFF")
				{
					#region 数据下载方案： JCGX需求，瓦片分发使用，直接上传FTP
					DirectlyAddressing da = new DirectlyAddressing(DirectlyAddressingIPMod.IPModDataSet);
					string[] tilespath = tilepaths.Split('#');
					foreach (string item in tilespath)
					{
						string ip = "-1";
						string tempName = item + ".jpg";
						string desPath = da.GetPathByFileName(tempName, out ip);
						if (ip == "-1" || !File.Exists(desPath))
							desPath = string.Format(@"{0}{1}", GetFailedTilePath(), tempName);
						string parentPath = Path.GetDirectoryName(desPath);
						string[] strpath = Directory.GetFiles(parentPath);
						foreach (string tifName in strpath)
						{
							if (tifName.Contains(item))
							{
								try
								{
									UploadFtp(tifName, isFTPUPLoad[1], isFTPUPLoad[2], isFTPUPLoad[3]);
									this.ParentOrder.Logs.Add(tifName + "成功上传至" + isFTPUPLoad[1]);
								}
								catch
								{
									continue;
								}
							}
						}
						
						//int i=1;
						//while (i <= 4)
						//{
						//    string tempTifname = item + "-" + i + "tif";
						//    i++;
						//    string ip = "-1";
						//    string desPath = da.GetPathByFileName(tempTifname, out ip);
						//    if (ip != "-1")
						//    {
						//        try
						//        {
						//            UploadFtp(desPath, isFTPUPLoad[1], isFTPUPLoad[2], isFTPUPLoad[3]);
						//        }
						//        catch
						//        {
						//            continue;
						//        }
						//    }
						//}
					}
					#endregion
				}


			}
			catch (Exception ex)
			{
				exResult = ex;
				//throw ex;
			}
			finally
			{
				try
				{
					//20140418 ksk修改，webService的ip地址存入数据库中。
					string address = String.Format("http://{0}/JCGXService/services/WS_DB_Callback.WS_DB_CallbackHttpSoap12Endpoint/", Constant.WPFFEndPointAddress);
					using (WS_DB_CallbackPortTypeClient client = new WS_DB_CallbackPortTypeClient(Constant.JcgxEndPointName, address))
					{
						int i = 0;
						if (exResult == null)
							i = client.CallBack_Down(opID, "OK");
						else
							i = client.CallBack_Down(opID, "数据下载失败！");
						this.ParentOrder.Logs.Add("完成瓦片分发消息发送！" + i.ToString());
					}
				}
				catch (Exception ex)
				{
					this.ParentOrder.Logs.Add("瓦片分发消息发送失败：" + ex.ToString());
				}
			}
		}
		/// <summary>
		/// 获取入库失败切片的存放路径，IP为数据库中标为ISCENTER的站点
		/// </summary>
		/// <returns></returns>
		private string GetFailedTilePath()
		{
			string CenterIP = TServerSiteManager.GetCenterSiteIP();
			string pattern = @"^(0|[1-9][0-9]?|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.(0|[1-9][0-9]?|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.(0|[1-9][0-9]?|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.(0|[1-9][0-9]?|1[0-9]{2}|2[0-4][0-9]|25[0-5])$";
		    string path = "";
			if (System.Text.RegularExpressions.Regex.IsMatch(CenterIP, pattern))
			{
                switch (Constant.DbStorage)
                {
                    case EnumDbStorage.MULTIPLE:
                        path = string.Format(@"\\{0}\{1}\{2}\", CenterIP, StorageBasePath.QRST_DB_Tile, StorageBasePath.FailedTile);
                        break;
                    case EnumDbStorage.CLUSTER:
                        break;
                    case EnumDbStorage.SINGLE:
                        path = string.Format(@"{0}\{1}\{2}\", Constant.PcDBRootPath, StorageBasePath.QRST_DB_Tile, StorageBasePath.FailedTile);
                        break;
			    }
			    return path;
			}
			else
			{
				return "";
			}
		}
		/// <summary>
		/// 20140319 ksk添加。Jcgx需求，对外发布使用。
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="ftpServerIP"></param>
		/// <param name="ftpUserID"></param>
		/// <param name="ftpPassword"></param>
		/// <returns></returns>
		public bool UploadFtp(string filename, string ftpServerIP, string ftpUserID, string ftpPassword)
		{
			string[] strs = ftpServerIP.Split('/');
			string ftpPath = string.Join("/", strs, 3, strs.Length - 3);
			string ftpIP = string.Join("/", strs, 0, 3);
			FtpCheckDirectoryExist(ftpIP, ftpUserID, ftpPassword, ftpPath);
			bool isSucess = false;
			FileInfo fileInf = new FileInfo(filename);
			//string uri = "ftp://" + ftpServerIP + "/" + fileInf.Name;

			// Create FtpWebRequest object from the Uri provided 
			FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(String.Format("{0}{1}", ftpServerIP, fileInf.Name)));
			try
			{
				// Provide the WebPermission Credintials
				if (!ftpUserID.Equals("") && !ftpPassword.Equals(""))
					reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);

				// By default KeepAlive is true, where the control connection is not closed 
				// after a command is executed. 
				reqFTP.KeepAlive = false;

				// Specify the command to be executed. 
				reqFTP.Method = WebRequestMethods.Ftp.UploadFile;

				// Specify the data transfer type. 
				reqFTP.UseBinary = true;

				// Notify the server about the size of the uploaded file 
				reqFTP.ContentLength = fileInf.Length;

				// The buffer size is set to 2kb 
				const int buffLength = 8196;
				byte[] buff = new byte[buffLength];
				int contentLen;

				// Opens a file stream (System.IO.FileStream) to read the file to be uploaded 
				//FileStream fs = fileInf.OpenRead(); 
				FileStream fs = fileInf.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

				// Stream to which the file to be upload is written 
				Stream strm = reqFTP.GetRequestStream();

				// Read from the file stream 2kb at a time 
				contentLen = fs.Read(buff, 0, buffLength);

				// Till Stream content ends 
				while (contentLen != 0)
				{
					// Write Content from the file stream to the FTP Upload Stream 
					strm.Write(buff, 0, contentLen);
					contentLen = fs.Read(buff, 0, buffLength);
				}

				// Close the file stream and the Request Stream 
				strm.Close();
				fs.Close();
				isSucess = true;
				return isSucess;
			}
			catch (Exception ex)
			{
				reqFTP.Abort();
				//  Logging.WriteError(ex.Message + ex.StackTrace);
				return isSucess;
			}
		}

		//上传文件
		//string filename, string ftpServer, string ftpUserID, string ftpPassword
		//internal static Boolean FtpUpload(string ftpPath, string localFile)
		//{
		//    //检查目录是否存在，不存在创建
		//    FtpCheckDirectoryExist(ftpPath);
		//    FileInfo fi = new FileInfo(localFile);
		//    FileStream fs = fi.OpenRead();
		//    long length = fs.Length;
		//    FtpWebRequest req = (FtpWebRequest)WebRequest.Create(ftpServerIP + ftpPath + fi.Name);
		//    req.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
		//    req.Method = WebRequestMethods.Ftp.UploadFile;
		//    req.ContentLength = length;
		//    req.Timeout = 10 * 1000;
		//    try
		//    {
		//        Stream stream = req.GetRequestStream();
		//        int BufferLength = 2048; //2K   
		//        byte[] b = new byte[BufferLength];
		//        int i;
		//        while ((i = fs.Read(b, 0, BufferLength)) > 0)
		//        {
		//            stream.Write(b, 0, i);
		//        }
		//        stream.Close();
		//        stream.Dispose();
		//    }
		//    catch (Exception e)
		//    {
		//        return false;
		//    }
		//    finally
		//    {
		//        fs.Close();
		//        req.Abort();
		//    }
		//    req.Abort();
		//    return true;
		//}

		//判断文件的目录是否存,不存则创建
		public static void FtpCheckDirectoryExist(string ip, string name, string pwd, string destFilePath)
		{
			//string fullDir = FtpParseDirectory(destFilePath);
			string[] dirs = destFilePath.Split('/');
			string curDir = "/";
			for (int i = 0; i < dirs.Length; i++)
			{
				string dir = dirs[i];
				//如果是以/开始的路径,第一个为空  
				if (dir != null && dir.Length > 0)
				{
					try
					{
						curDir += dir + "/";
						FtpMakeDir(ip, name, pwd, curDir);
					}
					catch (Exception)
					{
					}
				}
			}
		}

		public static string FtpParseDirectory(string destFilePath)
		{
			return destFilePath.Substring(0, destFilePath.LastIndexOf("/"));
		}

		//创建目录
		public static Boolean FtpMakeDir(string ftpServerIP, string ftpUserID, string ftpPassword, string localFile)
		{
			FtpWebRequest req = (FtpWebRequest)WebRequest.Create(ftpServerIP + localFile);
			req.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
			req.Method = WebRequestMethods.Ftp.MakeDirectory;
			try
			{
				FtpWebResponse response = (FtpWebResponse)req.GetResponse();
				response.Close();
			}
			catch (Exception)
			{
				req.Abort();
				return false;
			}
			req.Abort();
			return true;
		}

	}
}
