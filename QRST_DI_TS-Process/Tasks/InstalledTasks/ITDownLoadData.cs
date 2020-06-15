using System;
using System.Data;
using QRST_DI_Resources;
using QRST_DI_DS_Metadata.Paths;
using System.IO;
using QRST_DI_TS_Process.JCGXCallBack;
using System.Net;
using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetaData;
using QRST_DI_SS_DBInterfaces.IDBEngine;
 
namespace QRST_DI_TS_Process.Tasks.InstalledTasks
{
    public class ITDownLoadData : TaskClass
    {
        /// <summary>
        /// 任务名,定义唯一标识，不可动态修改
        /// </summary>
        public override string TaskName
        {
            get { return "ITDownLoadData"; }
            set { }
        }

        public override void Process()
        {
            if (!this.ProcessArgu[0].Contains("#"))
            {
                string qrst_code = this.ProcessArgu[0];    //要下载数据的数据编码
                string destPath = this.ProcessArgu[1];     //目的地址
                string opID = this.ProcessArgu[2];             //操作号ID
                string gfdataName = this.ProcessArgu[3];             //要下载数据的数据名称
                string webservice=this.ProcessArgu[4];
                Exception exResult = null;
                string[] isFTPUPLoad = opID.Split('#');
                try
                {
                    //判断dataName==""，如果不为空，qrst_code=getqrst_codebydataname
                    if (gfdataName == "")// 调用1方法是（三个参数） 因为网络组传过来的name就是空的，他们要求也要执行，这是老网站，传四个参数是新网站，网络组要求有点乱。
                    {
                        this.ParentOrder.Logs.Add("根据QRSTCODE获取数据");
                        //解析数据编码
                        this.ParentOrder.Logs.Add("开始解析数据编码！");

                        int index = qrst_code.IndexOf('-');
                        string industrycode = qrst_code.Substring(0, index);

                        if (industrycode != Constant.INDUSTRYCODE)
                        {
                            throw new Exception("行业编码错误");
                        }
                        //根据数据编码获取数据
                        string tableCode = StoragePath.GetTableCodeByQrstCode(qrst_code);
                        StoragePath storPath = new StoragePath(tableCode);
                        string sourcePath = storPath.GetDataPath(qrst_code);


                        if (sourcePath != "" && Directory.Exists(sourcePath))      //数据目录存在
                        {
                            //获取数据文件
                            DirectoryInfo dirInfo = new DirectoryInfo(sourcePath);
                            FileInfo[] fileinfo = dirInfo.GetFiles();
                            if (fileinfo.Length <= 0)
                            {
                                throw new Exception("数据不存在！");
                            }
                            else
                            {


                                #region 数据下载方案1：局域网内部拷贝
                                //sourcePath = string.Format(@"{0}\{1}", sourcePath, fileinfo[0].ToString());
                                //destPath = string.Format(@"{0}\{1}", destPath, qrst_code);
                                //Directory.CreateDirectory(destPath);
                                //destPath = string.Format(@"{0}\{1}", destPath, fileinfo[0].ToString());

                                //this.ParentOrder.Logs.Add("已经获取到源数据路径！");
                                //this.ParentOrder.Logs.Add("开始下载数据！");
                                //File.Copy(sourcePath, destPath);
                                //this.ParentOrder.Logs.Add("完成数据下载！");

                                //更新SNFF_FTP,暂时替代DC,下载方法与NEWFTP一致(20160507)
                                #endregion
                                if (isFTPUPLoad[0] == "NEWFTP" || isFTPUPLoad[0] == "SNFF_FTP" || isFTPUPLoad[0] == "OLDNEWFTP")
                                {
                                    this.ParentOrder.Logs.Add("已经获取到源数据路径！");
                                    this.ParentOrder.Logs.Add("开始下载数据！");
                                    #region 数据下载方案： JCGX需求，对外发布使用，直接上传FTP
                                    foreach (var item in fileinfo)
                                    {
                                        UploadFtp(item.FullName, isFTPUPLoad[1], isFTPUPLoad[2], isFTPUPLoad[3]);
                                    }
                                    this.ParentOrder.Logs.Add("结束数据上传！");
                                    #endregion
                                }
                                else
                                {
                                    #region  数据下载方案2：调用DC的数据传输Java包，将数据存放到指定位置
                                    this.ParentOrder.Logs.Add("已经获取到源数据路径！");
                                    this.ParentOrder.Logs.Add("开始下载数据！");
                                    string errorMsg = "";
                                    string dcOutMsg = "";
                                    string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                                    if (QRST_DI_DS_DataTransfer.DataTransferByDataBus.UpLoadData(Constant.DataBusServerUrl, destPath, sourcePath, appDirectory, out errorMsg, out dcOutMsg))
                                    {
                                        this.ParentOrder.Logs.Add("完成数据下载！");
                                    }
                                    else
                                    {
                                        throw new Exception("数据下载出错：" + errorMsg);
                                    }
                                    this.ParentOrder.Logs.Add("DC outMsg:" + dcOutMsg);
                                }
                                    #endregion
                            }
                        }
                        else
                        {
                            throw new Exception("数据不存在！");
                        }
                    }
                    else
                    {
                        this.ParentOrder.Logs.Add("根据文件名获取数据");

                        string sqlStr = string.Format("select QRST_CODE from prod_gf1 where Name= '{0}'", gfdataName);
                        IDbOperating sqLiteOperating = Constant.IdbOperating;
                        IDbBaseUtilities sqLiteBase = sqLiteOperating.GetSubDbUtilities(EnumDBType.EVDB);
                        //MySqlBaseUtilities mysqlUtil = new MySqlBaseUtilities(Constant.ConnectionStringEVDB);
                        DataSet ds = sqLiteBase.GetDataSet(sqlStr);

                        if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                qrst_code = dr["QRST_CODE"].ToString();
                            }


                            //解析数据编码
                            this.ParentOrder.Logs.Add("获取到QRSTCODE，开始解析数据编码！");

                            int index = qrst_code.IndexOf('-');
                            string industrycode = qrst_code.Substring(0, index);

                            if (industrycode != Constant.INDUSTRYCODE)
                            {
                                throw new Exception("行业编码错误");
                            }
                            //根据数据编码获取数据
                            string tableCode = StoragePath.GetTableCodeByQrstCode(qrst_code);
                            StoragePath storPath = new StoragePath(tableCode);
                            string sourcePath = storPath.GetDataPath(qrst_code);


                            if (sourcePath != "" && Directory.Exists(sourcePath))      //数据目录存在
                            {
                                //获取数据文件
                                DirectoryInfo dirInfo = new DirectoryInfo(sourcePath);
                                FileInfo[] fileinfo = dirInfo.GetFiles();
                                if (fileinfo.Length <= 0)
                                {
                                    throw new Exception("数据不存在！");
                                }
                                else
                                {


                                    #region 数据下载方案1：局域网内部拷贝
                                    //sourcePath = string.Format(@"{0}\{1}", sourcePath, fileinfo[0].ToString());
                                    //destPath = string.Format(@"{0}\{1}", destPath, qrst_code);
                                    //Directory.CreateDirectory(destPath);
                                    //destPath = string.Format(@"{0}\{1}", destPath, fileinfo[0].ToString());

                                    //this.ParentOrder.Logs.Add("已经获取到源数据路径！");
                                    //this.ParentOrder.Logs.Add("开始下载数据！");
                                    //File.Copy(sourcePath, destPath);
                                    //this.ParentOrder.Logs.Add("完成数据下载！");

                                    //更新SNFF_FTP,暂时替代DC,下载方法与NEWFTP一致(20160507)
                                    #endregion
                                    if (isFTPUPLoad[0] == "NEWFTP" || isFTPUPLoad[0] == "SNFF_FTP" || isFTPUPLoad[0] == "OLDNEWFTP")
                                    {
                                        this.ParentOrder.Logs.Add("已经获取到源数据路径！");
                                        this.ParentOrder.Logs.Add("开始下载数据！");
                                        #region 数据下载方案： JCGX需求，对外发布使用，直接上传FTP
                                        foreach (var item in fileinfo)
                                        {
                                            UploadFtp(item.FullName, isFTPUPLoad[1], isFTPUPLoad[2], isFTPUPLoad[3]);
                                        }
                                        this.ParentOrder.Logs.Add("结束数据上传！");
                                        #endregion
                                    }
                                    else
                                    {
                                        #region  数据下载方案2：调用DC的数据传输Java包，将数据存放到指定位置
                                        this.ParentOrder.Logs.Add("已经获取到源数据路径！");
                                        this.ParentOrder.Logs.Add("开始下载数据！");
                                        string errorMsg = "";
                                        string dcOutMsg = "";
                                        string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                                        if (QRST_DI_DS_DataTransfer.DataTransferByDataBus.UpLoadData(Constant.DataBusServerUrl, destPath, sourcePath, appDirectory, out errorMsg, out dcOutMsg))
                                        {
                                            this.ParentOrder.Logs.Add("完成数据下载！");
                                        }
                                        else
                                        {
                                            throw new Exception("数据下载出错：" + errorMsg);
                                        }
                                        this.ParentOrder.Logs.Add("DC outMsg:" + dcOutMsg);
                                    }
                                        #endregion
                                }
                            }
                            else
                            {
                                throw new Exception("数据不存在！");
                            }
                        }
                        else
                        {
                            this.ParentOrder.Logs.Add("根据数据名称得到的数据qrst_code为空！");
                        }
                    }

                }
                catch (Exception ex)
                {
                    exResult = ex;
                    this.ParentOrder.Logs.Add(ex.Message);
                    //throw ex; 
                }
                finally
                {
                    if (webservice == "")
                    {
                    if (isFTPUPLoad[0] == "SNFF")
                    {
                        try
                        {
                            //string address = "http://192.168.1.145:8080/JCGXService/services/WS_DB_Callback.WS_DB_CallbackHttpSoap12Endpoint/";
                            //30
                            string address = String.Format("http://{0}/JCGXService/services/WS_DB_Callback.WS_DB_CallbackHttpSoap12Endpoint/", Constant.SnffEndPointAddress);
                            using (WS_DB_CallbackPortTypeClient client = new WS_DB_CallbackPortTypeClient(Constant.JcgxEndPointName, address))
                            {
                                int i = 0;
                                if (exResult == null)
                                    i = client.CallBack_Down(opID, "OK");
                                else
                                    i = client.CallBack_Down(opID, "数据下载失败！");
                                this.ParentOrder.Logs.Add("完成所内分发系统中心服务器消息发送！" + i.ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            this.ParentOrder.Logs.Add("所内分发系统中心服务器消息发送失败：" + ex.ToString());
                        }
                    }
                    //集成共享要求把回调消息分开,调用JCGX的Call_back方法(20160507)
                    else if (isFTPUPLoad[0] == "SNFF_FTP")
                    {
                        try
                        {
                            //string address = "http://192.168.1.145:8080/JCGXService/services/WS_DB_Callback.WS_DB_CallbackHttpSoap12Endpoint/";
                            //20
                            string address = String.Format("http://{0}/JCGXService/services/WS_DB_Callback.WS_DB_CallbackHttpSoap12Endpoint/", Constant.JcgxEndPointAddress);
                            using (WS_DB_CallbackPortTypeClient client = new WS_DB_CallbackPortTypeClient(Constant.JcgxEndPointName, address))
                            {
                                int i = 0;
                                if (exResult == null)
                                    i = client.CallBack_Down(opID, "OK");
                                else
                                    i = client.CallBack_Down(opID, "数据下载失败！");
                                this.ParentOrder.Logs.Add("完成集成共享(SNFF_FTP)消息发送！" + i.ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            this.ParentOrder.Logs.Add("集成共享(SNFF_FTP)消息发送失败：" + ex.ToString());
                        }
                    }
                    //集成共享要求把回调消息分开,调用JCGX的Call_back方法(20160621)
                    else if (isFTPUPLoad[0] == "NEWFTP")
                    {
                        try
                        {
                            //string address = "http://192.168.1.145:8080/JCGXService/services/WS_DB_Callback.WS_DB_CallbackHttpSoap12Endpoint/";
                            //3.101
                            string address = String.Format("http://{0}/JCGXService/services/WS_DB_Callback.WS_DB_CallbackHttpSoap12Endpoint/", Constant.NEWFTPEndPointAddress);
                            using (WS_DB_CallbackPortTypeClient client = new WS_DB_CallbackPortTypeClient(Constant.JcgxEndPointName, address))
                            {
                                int i = 0;
                                if (exResult == null)
                                    i = client.CallBack_Down(opID, "OK");
                                else
                                    i = client.CallBack_Down(opID, "数据下载失败！");
                                this.ParentOrder.Logs.Add("完成集成共享(NEWFTP)消息发送！" + i.ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            this.ParentOrder.Logs.Add("集成共享(NEWFTP)消息发送失败：" + ex.ToString());
                        }
                    }

                    else if (isFTPUPLoad[0] == "OLDNEWFTP")
                    {
                        try
                        {
                            string address = String.Format("http://{0}/JCGXService/services/WS_DB_Callback.WS_DB_CallbackHttpSoap12Endpoint/", Constant.OLDNEWFTPEndPointAddress);
                            using (WS_DB_CallbackPortTypeClient client = new WS_DB_CallbackPortTypeClient(Constant.JcgxEndPointName, address))
                            {
                                int i = 0;
                                if (exResult == null)
                                    i = client.CallBack_Down(opID, "OK");
                                else
                                    i = client.CallBack_Down(opID, "数据下载失败！");
                                this.ParentOrder.Logs.Add("完成集成共享(OLDNEWFTP)消息发送！" + i.ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            this.ParentOrder.Logs.Add("集成共享(OLDNEWFTP)消息发送失败：" + ex.ToString());
                        }
                    }
                    //调用集成共享的CallBack_Down方法
                    else
                    {
                        try
                        {
                            //20140318 ksk修改，使用含有两个参数的构造方法，将webService的ip地址存入数据库中。
                            string address = String.Format("http://{0}/JCGXService/services/WS_DB_Callback.WS_DB_CallbackHttpSoap12Endpoint/", Constant.JcgxEndPointAddress);
                            using (WS_DB_CallbackPortTypeClient client = new WS_DB_CallbackPortTypeClient(Constant.JcgxEndPointName, address))
                            {
                                int i = 0;
                                if (exResult == null)
                                    i = client.CallBack_Down(opID, "OK");
                                else
                                    i = client.CallBack_Down(opID, "数据下载失败！");
                                this.ParentOrder.Logs.Add("完成集成共享消息发送！" + i.ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            this.ParentOrder.Logs.Add("集成共享消息发送失败：" + ex.ToString());
                            }
                        }
                    }
                    else
                    {
                        try
                        {
                            //string address = "http://192.168.1.145:8080/JCGXService/services/WS_DB_Callback.WS_DB_CallbackHttpSoap12Endpoint/";
                            //30
                            string address = String.Format("http://{0}/JCGXService/services/WS_DB_Callback.WS_DB_CallbackHttpSoap12Endpoint/", webservice);
                            using (WS_DB_CallbackPortTypeClient client = new WS_DB_CallbackPortTypeClient(Constant.JcgxEndPointName, address))
                            {
                                int i = 0;
                                if (exResult == null)
                                    i = client.CallBack_Down(opID, "OK");
                                else
                                    i = client.CallBack_Down(opID, "数据下载失败！");
                                this.ParentOrder.Logs.Add("消息发送！" + i.ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            this.ParentOrder.Logs.Add("消息发送失败：" + ex.ToString());
                        }
                    }
                }
            }
                //应JCGX要求 传入第四个参数数据名称
                //ProcessArgu[0]中包含qrst_code和数据Name
                //
            else 
            {
                string[] sArray = this.ProcessArgu[0].Split('#');
                string name = sArray[1];         //要下载数据名称
                string qrst_code = sArray[0];    //要下载数据的数据编码
                string destPath = this.ProcessArgu[1];     //目的地址
                string opID = this.ProcessArgu[2];             //操作号ID

                Exception exResult = null;
                string[] isFTPUPLoad = opID.Split('#');
                try
                {
                    //解析数据编码
                    this.ParentOrder.Logs.Add("开始解析数据编码！");

                    int index = qrst_code.IndexOf('-');
                    string industrycode = qrst_code.Substring(0, index);
                    if (industrycode != Constant.INDUSTRYCODE)
                    {
                        throw new Exception("行业编码错误");
                    }
                    //根据数据编码获取数据
                    string tableCode = StoragePath.GetTableCodeByQrstCode(qrst_code);
                    StoragePath storPath = new StoragePath(tableCode);
                   // string sourcePath = storPath.GetDataPath(qrst_code);
                    string sourcePath = storPath.GetDataPath(this.ProcessArgu[0]);

                    //JCGX要求:反馈消息分两次,第一次反馈消息----判断数据是否存在
                    //如存在,返回(opID,Yes), 若不存在,返回(OpID,No)
                    if (isFTPUPLoad[0] == "SNFF")
                    {
                        try
                        {
                            //string address = "http://192.168.1.145:8080/JCGXService/services/WS_DB_Callback.WS_DB_CallbackHttpSoap12Endpoint/";
                            //30
                            string address = String.Format("http://{0}/JCGXService/services/WS_DB_Callback.WS_DB_CallbackHttpSoap12Endpoint/", Constant.SnffEndPointAddress);
                            using (WS_DB_CallbackPortTypeClient client = new WS_DB_CallbackPortTypeClient(Constant.JcgxEndPointName, address))
                            {
                                int i = 0;
                                if (sourcePath != "" && Directory.Exists(sourcePath))
                                    i = client.CallBack_Down(opID, "Yes");
                                else
                                    i = client.CallBack_Down(opID, "No");
                                this.ParentOrder.Logs.Add("告知JCGX，库中有无此数据！" + i.ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            this.ParentOrder.Logs.Add("所内分发系统中心服务器消息发送失败：" + ex.ToString());
                        }
                    }
                    //集成共享要求把回调消息分开,调用JCGX的Call_back方法(20160507)
                    else if (isFTPUPLoad[0] == "SNFF_FTP")
                    {
                        try
                        {
                            //string address = "http://192.168.1.145:8080/JCGXService/services/WS_DB_Callback.WS_DB_CallbackHttpSoap12Endpoint/";
                            //20
                            string address = String.Format("http://{0}/JCGXService/services/WS_DB_Callback.WS_DB_CallbackHttpSoap12Endpoint/", Constant.JcgxEndPointAddress);
                            using (WS_DB_CallbackPortTypeClient client = new WS_DB_CallbackPortTypeClient(Constant.JcgxEndPointName, address))
                            {
                                int i = 0;
                                if (sourcePath != "" && Directory.Exists(sourcePath))
                                    i = client.CallBack_Down(opID, "Yes");
                                else
                                    i = client.CallBack_Down(opID, "No");
                                this.ParentOrder.Logs.Add("告知JCGX，库中有无此数据！" + i.ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            this.ParentOrder.Logs.Add("集成共享(SNFF_FTP)消息发送失败：" + ex.ToString());
                        }
                    }
                    //集成共享要求把回调消息分开,调用JCGX的Call_back方法(20160621)
                    else if (isFTPUPLoad[0] == "NEWFTP")
                    {
                        try
                        {
                            //string address = "http://192.168.1.145:8080/JCGXService/services/WS_DB_Callback.WS_DB_CallbackHttpSoap12Endpoint/";
                            //3.101
                            string address = String.Format("http://{0}/JCGXService/services/WS_DB_Callback.WS_DB_CallbackHttpSoap12Endpoint/", Constant.NEWFTPEndPointAddress);
                            using (WS_DB_CallbackPortTypeClient client = new WS_DB_CallbackPortTypeClient(Constant.JcgxEndPointName, address))
                            {
                                int i = 0;
                                if (sourcePath != "" && Directory.Exists(sourcePath))
                                    i = client.CallBack_Down(opID, "Yes");
                                else
                                    i = client.CallBack_Down(opID, "No");
                                this.ParentOrder.Logs.Add("告知JCGX，库中有无此数据！" + i.ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            this.ParentOrder.Logs.Add("集成共享(NEWFTP)消息发送失败：" + ex.ToString());
                        }
                    }
                    else if (isFTPUPLoad[0] == "OLDNEWFTP")
                    {
                        try
                        {
                            string address = String.Format("http://{0}/JCGXService/services/WS_DB_Callback.WS_DB_CallbackHttpSoap12Endpoint/", Constant.OLDNEWFTPEndPointAddress);
                            using (WS_DB_CallbackPortTypeClient client = new WS_DB_CallbackPortTypeClient(Constant.JcgxEndPointName, address))
                            {
                                int i = 0;
                                if (sourcePath != "" && Directory.Exists(sourcePath))
                                    i = client.CallBack_Down(opID, "Yes");
                                else
                                    i = client.CallBack_Down(opID, "No");
                                this.ParentOrder.Logs.Add("告知JCGX，库中有无此数据！" + i.ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            this.ParentOrder.Logs.Add("集成共享(OLDNEWFTP)消息发送失败：" + ex.ToString());
                        }
                    }
                    //调用集成共享的CallBack_Down方法
                    else
                    {
                        try
                        {
                            //20140318 ksk修改，使用含有两个参数的构造方法，将webService的ip地址存入数据库中。
                            string address = String.Format("http://{0}/JCGXService/services/WS_DB_Callback.WS_DB_CallbackHttpSoap12Endpoint/", Constant.JcgxEndPointAddress);
                            using (WS_DB_CallbackPortTypeClient client = new WS_DB_CallbackPortTypeClient(Constant.JcgxEndPointName, address))
                            {
                                int i = 0;
                                if (sourcePath != "" && Directory.Exists(sourcePath))
                                    i = client.CallBack_Down(opID, "Yes");
                                else
                                    i = client.CallBack_Down(opID, "No");
                                this.ParentOrder.Logs.Add("告知JCGX，库中有无此数据！" + i.ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            this.ParentOrder.Logs.Add("集成共享消息发送失败：" + ex.ToString());
                        }
                    }
                    if (sourcePath != "" && Directory.Exists(sourcePath))      //数据目录存在
                    {
                        //获取数据文件
                        DirectoryInfo dirInfo = new DirectoryInfo(sourcePath);
                        FileInfo[] fileinfo = dirInfo.GetFiles();
                        if (fileinfo.Length <= 0)
                        {
                            throw new Exception("数据不存在！");
                        }
                        else
                        {
                            #region 数据下载方案1：局域网内部拷贝
                            //sourcePath = string.Format(@"{0}\{1}", sourcePath, fileinfo[0].ToString());
                            //destPath = string.Format(@"{0}\{1}", destPath, qrst_code);
                            //Directory.CreateDirectory(destPath);
                            //destPath = string.Format(@"{0}\{1}", destPath, fileinfo[0].ToString());

                            //this.ParentOrder.Logs.Add("已经获取到源数据路径！");
                            //this.ParentOrder.Logs.Add("开始下载数据！");
                            //File.Copy(sourcePath, destPath);
                            //this.ParentOrder.Logs.Add("完成数据下载！");

                            //更新SNFF_FTP,暂时替代DC,下载方法与NEWFTP一致(20160507)
                            #endregion
                            if (isFTPUPLoad[0] == "NEWFTP" || isFTPUPLoad[0] == "SNFF_FTP" || isFTPUPLoad[0] == "OLDNEWFTP")
                            {
                                this.ParentOrder.Logs.Add("已经获取到源数据路径！");
                                this.ParentOrder.Logs.Add("开始下载数据！");
                                #region 数据下载方案： JCGX需求，对外发布使用，直接上传FTP
                                foreach (var item in fileinfo)
                                {
                                    UploadFtp(item.FullName, isFTPUPLoad[1], isFTPUPLoad[2], isFTPUPLoad[3]);
                                }
                                this.ParentOrder.Logs.Add("结束数据上传！");
                                #endregion
                            }
                            else
                            {
                                #region  数据下载方案2：调用DC的数据传输Java包，将数据存放到指定位置
                                this.ParentOrder.Logs.Add("已经获取到源数据路径！");
                                this.ParentOrder.Logs.Add("开始下载数据！");
                                string errorMsg = "";
                                string dcOutMsg = "";
                                string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                                if (QRST_DI_DS_DataTransfer.DataTransferByDataBus.UpLoadData(Constant.DataBusServerUrl, destPath, sourcePath, appDirectory, out errorMsg, out dcOutMsg))
                                {
                                    this.ParentOrder.Logs.Add("完成数据下载！");
                                }
                                else
                                {
                                    throw new Exception("数据下载出错：" + errorMsg);
                                }
                                this.ParentOrder.Logs.Add("DC outMsg:" + dcOutMsg);
                            }
                                #endregion
                        }
                    }
                    else
                    {
                        throw new Exception("数据不存在！");
                    }
                }
                catch (Exception ex)
                {
                    exResult = ex;
                    this.ParentOrder.Logs.Add(ex.Message);
                    //throw ex; 
                }
                finally
                {
                    if (isFTPUPLoad[0] == "SNFF")
                    {
                        try
                        {
                            //string address = "http://192.168.1.145:8080/JCGXService/services/WS_DB_Callback.WS_DB_CallbackHttpSoap12Endpoint/";
                            //30
                            string address = String.Format("http://{0}/JCGXService/services/WS_DB_Callback.WS_DB_CallbackHttpSoap12Endpoint/", Constant.SnffEndPointAddress);
                            using (WS_DB_CallbackPortTypeClient client = new WS_DB_CallbackPortTypeClient(Constant.JcgxEndPointName, address))
                            {
                                int i = 0;
                                if (exResult == null)
                                    i = client.CallBack_Down(opID, "OK");
                                else
                                    i = client.CallBack_Down(opID, "数据下载失败！");
                                this.ParentOrder.Logs.Add("完成所内分发系统中心服务器消息发送！" + i.ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            this.ParentOrder.Logs.Add("所内分发系统中心服务器消息发送失败：" + ex.ToString());
                        }
                    }
                    //集成共享要求把回调消息分开,调用JCGX的Call_back方法(20160507)
                    else if (isFTPUPLoad[0] == "SNFF_FTP")
                    {
                        try
                        {
                            //string address = "http://192.168.1.145:8080/JCGXService/services/WS_DB_Callback.WS_DB_CallbackHttpSoap12Endpoint/";
                            //20
                            string address = String.Format("http://{0}/JCGXService/services/WS_DB_Callback.WS_DB_CallbackHttpSoap12Endpoint/", Constant.JcgxEndPointAddress);
                            using (WS_DB_CallbackPortTypeClient client = new WS_DB_CallbackPortTypeClient(Constant.JcgxEndPointName, address))
                            {
                                int i = 0;
                                if (exResult == null)
                                    i = client.CallBack_Down(opID, "OK");
                                else
                                    i = client.CallBack_Down(opID, "数据下载失败！");
                                this.ParentOrder.Logs.Add("完成集成共享(SNFF_FTP)消息发送！" + i.ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            this.ParentOrder.Logs.Add("集成共享(SNFF_FTP)消息发送失败：" + ex.ToString());
                        }
                    }
                    //集成共享要求把回调消息分开,调用JCGX的Call_back方法(20160621)
                    else if (isFTPUPLoad[0] == "NEWFTP")
                    {
                        try
                        {
                            //string address = "http://192.168.1.145:8080/JCGXService/services/WS_DB_Callback.WS_DB_CallbackHttpSoap12Endpoint/";
                            //3.101
                            string address = String.Format("http://{0}/JCGXService/services/WS_DB_Callback.WS_DB_CallbackHttpSoap12Endpoint/", Constant.NEWFTPEndPointAddress);
                            using (WS_DB_CallbackPortTypeClient client = new WS_DB_CallbackPortTypeClient(Constant.JcgxEndPointName, address))
                            {
                                int i = 0;
                                if (exResult == null)
                                    i = client.CallBack_Down(opID, "OK");
                                else
                                    i = client.CallBack_Down(opID, "数据下载失败！");
                                this.ParentOrder.Logs.Add("完成集成共享(NEWFTP)消息发送！" + i.ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            this.ParentOrder.Logs.Add("集成共享(NEWFTP)消息发送失败：" + ex.ToString());
                        }
                    }
                    else if (isFTPUPLoad[0] == "OLDNEWFTP")
                    {
                        try
                        {
                            string address = String.Format("http://{0}/JCGXService/services/WS_DB_Callback.WS_DB_CallbackHttpSoap12Endpoint/", Constant.OLDNEWFTPEndPointAddress);
                            using (WS_DB_CallbackPortTypeClient client = new WS_DB_CallbackPortTypeClient(Constant.JcgxEndPointName, address))
                            {
                                int i = 0;
                                if (exResult == null)
                                    i = client.CallBack_Down(opID, "OK");
                                else
                                    i = client.CallBack_Down(opID, "数据下载失败！");
                                this.ParentOrder.Logs.Add("完成集成共享(OLDNEWFTP)消息发送！" + i.ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            this.ParentOrder.Logs.Add("集成共享(OLDNEWFTP)消息发送失败：" + ex.ToString());
                        }
                    }
                    //调用集成共享的CallBack_Down方法
                    else
                    {
                        try
                        {
                            //20140318 ksk修改，使用含有两个参数的构造方法，将webService的ip地址存入数据库中。
                            string address = String.Format("http://{0}/JCGXService/services/WS_DB_Callback.WS_DB_CallbackHttpSoap12Endpoint/", Constant.JcgxEndPointAddress);
                            using (WS_DB_CallbackPortTypeClient client = new WS_DB_CallbackPortTypeClient(Constant.JcgxEndPointName, address))
                            {
                                int i = 0;
                                if (exResult == null)
                                    i = client.CallBack_Down(opID, "OK");
                                else
                                    i = client.CallBack_Down(opID, "数据下载失败！");
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
			string ftpPath = string.Join("/", strs, 3, strs.Length-3);
			string ftpIP = string.Join("/", strs, 0,  3);
			FtpCheckDirectoryExist(ftpIP, ftpUserID, ftpPassword,ftpPath);
			bool isSucess = false;
			FileInfo fileInf = new FileInfo(filename);
			//string uri = "ftp://" + ftpServerIP + "/" + fileInf.Name;

			// Create FtpWebRequest object from the Uri provided 
			FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(String.Format("{0}/{1}", ftpServerIP, fileInf.Name)));
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
                this.ParentOrder.Logs.Add("上传集成共享FTP异常：" + ex.Message);
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
		public static void FtpCheckDirectoryExist(string ip,string name,string pwd,string destFilePath)
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
						FtpMakeDir(ip,name,pwd,curDir);
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
