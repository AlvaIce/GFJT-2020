using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using QRST_DI_TS_Process.Orders;
using QRST_DI_TS_Process.Orders.InstalledOrders;
using QRST_DI_TS_Process;
using System.Configuration;
using QRST_DI_MS_Basis.UserRole;
using QRST_DI_Resources;
using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetaData;
using QRST_DI_SS_DBInterfaces.IDBEngine;
using Microsoft.JScript;
using Iesi.Collections.Generic;

namespace QRST_DI_MS_Component_DataImportorUI.TAR.GZ
{
    public partial class UCImportTarGZ : UserControl
    {
        //多线程访问共享资源，加锁。
        private Object objThread = new Object();
        //将后台线程的信息显示在前台界面上
        private delegate void AddMessageDelegate1(string message);
        string[] FiltedFileNames1 = null;
        bool isSearch = false;

        public Dictionary<string, string> ImportMessage = new Dictionary<string, string>();
        DateTime startTime, endTime;
        string RecordTextPath = "";
        //String recordErrorFilePath = "";
        FileSystemWatcher fsWatcher = null;
        AutomaticTarImport autoImport = null;
        //2016/12/20
        //定义线程调用的委托  
        private delegate void SetTextCallback(string text);

        public void SetText(string text)
        {
           
            if (textBox1.InvokeRequired)
            {
                SetTextCallback dt = new SetTextCallback(SetText);
                textBox1.Invoke(dt, new object[] { text });
            }
            else
            {
                textBox1.Text = text; ////更新textbox内容
                //textBox1.Refresh();
            }
        }

        public void Addmessege1(string messsage)
        {
            if (richTextBox1.InvokeRequired)
            {
                AddMessageDelegate1 d = Addmessege1;
                richTextBox1.Invoke(d, messsage);
            }
            else
            {
                richTextBox1.AppendText(messsage);
            }
        }

        private delegate void ButtonHandleDelegate();
        public void ButtonHandle()
        {
            if (buttonImport.InvokeRequired)
            {
                ButtonHandleDelegate d = ButtonHandle;
                buttonImport.Invoke(d);
            }
            else
            {
                buttonImport.Enabled = true;
            }
        }
        string[] FiltedFileNames;


        Dictionary<string, FileInfo> dicNameFi;
        public UCImportTarGZ()
        {
            InitializeComponent();
            if(!Constant.ServiceIsConnected)
            {
                Constant.InitializeTcpConnection();
            }
        }

        public bool IsCreated { get; private set; }
        public IDbBaseUtilities evdbUtil;
        public userInfo _currentUser;
        public void Create(IDbBaseUtilities evdb, userInfo currentUser)
        {
            evdbUtil = evdb;
            _currentUser = currentUser;
            ctrlVirtualDirSetting1.Create(currentUser.NAME, currentUser.PASSWORD);

        }
        /// <summary>
        /// 选择文件夹按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOpenFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                this.textBox1.Text = fbd.SelectedPath;
            }
        }

        /// <summary>
        /// 新数据验证
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCertify_Click(object sender, EventArgs e)
        {
            //if (this.TreeView_type.SelectedNode == null)
            //{
            //    MessageBox.Show("请选择数据类型");
            //    return;
            //}
            if (!Directory.Exists(this.textBox1.Text))
            {
                MessageBox.Show("路径不合法");
                this.SetText("");
                return;
            }
            this.textBox2.Text = searNewData();
        }
        private String DBRecordPath = @ConfigurationManager.AppSettings["DBRecordPath"]; 
        private void buttonImport_Click(object sender, EventArgs e)
        {
            RecordTextPath = string.Format(@"{0}\ImportTargzLog_{1}.txt",DBRecordPath, DateTime.Today.ToString("yyyyMMdd"));
            //recordErrorFilePath = string.Format(@"{0}\errorFiles_{1}",DBRecordPath, DateTime.Today.ToString("yyyyMMdd"));
            this.buttonImport.Enabled = false;
            this.richTextBox1.Text = "";
            //if (this.TreeView_type.SelectedNode == null)
            //{
            //    MessageBox.Show("请选择数据类型");
            //    return;
            //}
            //dataType = this.TreeView_type.SelectedNode.Tag.ToString();
            Thread th = new Thread(importThread);
            th.Start(this.textBox1.Text);
            //this.buttonImport.Enabled = false;
        }

        /// <summary>
        /// 返回指定文件夹内的数据的重复情况
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            List<string> distinctName = new List<string>();
            string DirPath = this.textBox1.Text;
            FileInfo[] fileInfos;
            DirectoryInfo di_data = new DirectoryInfo(DirPath);
            fileInfos = di_data.GetFiles("*.tar.gz", SearchOption.AllDirectories);

            this.richTextBox1.AppendText("原始数据数目为：" + fileInfos.Length);
            for (int i = 0; i < fileInfos.Length; i++)
            {
                if (!distinctName.Contains(fileInfos[i].Name))
                {
                    distinctName.Add(fileInfos[i].Name);
                }
            }

            this.richTextBox1.AppendText("去重原始数据数目为：" + distinctName.Count);
        }

        public string getDataType(string filename)
        {
            filename = Path.GetFileName(filename);

            string[] TempPath = filename.Split('_');
            if (TempPath.Length <= 1)
                TempPath = filename.Split('-');

            return TempPath[0];
        }

        /// <summary>
        /// 查找指定目录下的数据，返回新数据（需入库数据条数），显示出来。
        /// </summary>
        public string searNewData()
        {
            string DirPath = this.textBox1.Text;
            FileInfo[] fileInfos;
            DirectoryInfo di_data = new DirectoryInfo(DirPath);
            fileInfos = di_data.GetFiles("*.tar.gz", SearchOption.AllDirectories);
            //遍历path 获取tar.gz文件
            dicNameFi = new Dictionary<string, FileInfo>();
            if (!(fileInfos.Length > 0))
            {
                MessageBox.Show("指定文件夹下无数据！！！");
                this.SetText("");
                return "0";
            }
            foreach (FileInfo fi in fileInfos)
            {
                if (dicNameFi.Keys.Contains(fi.Name))
                {
                    dicNameFi.Remove(fi.Name);
                }
                dicNameFi.Add(fi.Name, fi);
            }

            string[] filenames = dicNameFi.Keys.ToArray();
            //没取1000条做一次数据存在检查
            int blockSize = 1000;

            List<string> resultFilted = new List<string>();

            int blockNum = filenames.Length / blockSize;
            for (int i = 0; i < blockNum + 1; i++)
            {
                int beginIndex = i * blockSize;
                int len = blockSize;
                if (i == blockNum)
                {
                    len = filenames.Length % blockSize;
                }
                if (len > 0)
                {
                    string[] blockArr = new string[len];
                    for (int j = 0; j < len; j++)
                    {
                        blockArr[j] = filenames[beginIndex + j];
                    }
                    List<string> blockList = new List<string>();
                    blockList = blockArr.ToList();
                    string[] returnNotInDB = DataCertificate_Multi(blockList).ToArray();
                    resultFilted.AddRange(returnNotInDB);
                }
            }

            FiltedFileNames1 = resultFilted.ToArray();
            isSearch = true;
            //显示不重复数据数目
            return (FiltedFileNames1.Length).ToString();
        }
        public string[] searNewData(string DirPath)
        {

            //string DirPath = this.textBox1.Text;
            FileInfo[] fileInfos;
            DirectoryInfo di_data = new DirectoryInfo(DirPath);
            fileInfos = di_data.GetFiles("*.tar.gz", SearchOption.AllDirectories);
            //遍历path 获取tar.gz文件
            dicNameFi = new Dictionary<string, FileInfo>();
            if (!(fileInfos.Length > 0))
            {
                MessageBox.Show("指定文件夹下无数据！！！");

                this.SetText("");
                return new string[] { };
            }
            foreach (FileInfo fi in fileInfos)
            {
                if (dicNameFi.Keys.Contains(fi.Name))
                {
                    dicNameFi.Remove(fi.Name);
                }
                dicNameFi.Add(fi.Name, fi);
            }

            string[] filenames = dicNameFi.Keys.ToArray();

            int blockSize = 1000;

            List<string> resultFilted = new List<string>();

            int blockNum = filenames.Length / blockSize;
            for (int i = 0; i < blockNum + 1; i++)
            {
                int beginIndex = i * blockSize;
                int len = blockSize;
                if (i == blockNum)
                {
                    len = filenames.Length % blockSize;
                }
                if (len > 0)
                {
                    string[] blockArr = new string[len];
                    for (int j = 0; j < len; j++)
                    {
                        blockArr[j] = filenames[beginIndex + j];
                    }
                    List<string> blockList = new List<string>();
                    blockList = blockArr.ToList();
                    string[] returnNotInDB = DataCertificate_Multi(blockList).ToArray();
                    resultFilted.AddRange(returnNotInDB);
                }
            }

            FiltedFileNames = resultFilted.ToArray();
            isSearch = true;
            return resultFilted.ToArray();
        }
        /// <summary>
        /// 树节点选中事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeView_type_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Level != 1)
            {
                return;
            }
            this.dicNameFi = null;

            //dataType = this.TreeView_type.SelectedNode.Tag.ToString();
        }


        static IDbOperating mySQLOperator =Constant.IdbOperating;

        IDbBaseUtilities MySqlBaseUti;

        DataSet returnDS = null;

        /// <summary>
        /// 判断新的文件是否已经在数据库中
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public bool newFileInDB(string filePath)
        {
            bool result = true;

            int num = 0;
            string tableName = string.Empty;
            while (num < 15)
            {
                switch (num)
                {
                    case 0:
                        tableName = "prod_gf1";
                        break;
                    case 1:
                        tableName = "prod_hj";
                        break;
                    case 2:
                        tableName = "prod_zy3";
                        break;
                    case 3:
                        tableName = "prod_zy02c";
                        break;
                    case 4:
                        tableName = "prod_sj9a";
                        break;
                    case 5:
                        tableName = "prod_hj1c";
                        break;
                    case 6:
                        tableName = "prod_gf3";
                        break;
                    case 7:
                        tableName = "prod_gf1bcd";
                        break;
                    case 8:
                        tableName = "prod_ahsi_gf5";
                        break;
                    case 9:
                        tableName = "prod_aius_gf5";
                        break;
                    case 10:
                        tableName = "prod_dpc_gf5";
                        break;
                    case 11:
                        tableName = "prod_emi_gf5";
                        break;
                    case 12:
                        tableName = "prod_gmi_gf5";
                        break;
                    case 13:
                        tableName = "prod_vims_gf5";
                        break;
                    case 14:
                        tableName = "prod_gf6";
                        break;
                    /*case 10:
                        tableName = "prod_gf7";
                        break;*/
                    default:
                        break;
                }

                string SQLString = string.Format("select name from {0} where name in (", tableName);
                SQLString = string.Format("{0}'{1}',", SQLString, filePath);
                SQLString = SQLString.TrimEnd().TrimEnd(',');
                SQLString += ")";

                //根据数据库名称获取库访问实例
                mySQLOperator = Constant.IdbOperating;

                MySqlBaseUti = mySQLOperator.GetSubDbUtilities(EnumDBType.EVDB);

                returnDS = MySqlBaseUti.GetDataSet(SQLString);

                if (returnDS.Tables.Count > 0 && returnDS.Tables[0].Rows.Count > 0)
                {
                    result = false;
                }
                num++;
            }
            return result;
        }

        /// <summary>
        /// 验证数据是否已在数据库中，返回不在数据库中的数据列表
        /// </summary>
        /// <param name="dataNames"></param>
        /// <param name="dataType">“GF1”“HJ”“ZY3”“ZY02C”</param>
        /// <returns></returns>
        public List<string> DataCertificate_Multi(List<string> dataNames)
        {
            List<string> result = new List<string>();
            if (dataNames == null || dataNames.Count == 0)
            {
                return result;
            }
            result.AddRange(dataNames.ToArray());
            string tableName = string.Empty;

            //遍历全部表
            int ii = 0;
            while (ii < 15)
            {
                switch (ii)
                {
                    case 0:
                        tableName = "prod_gf1";
                        break;
                    case 1:
                        tableName = "prod_hj";
                        break;
                    case 2:
                        tableName = "prod_zy3";
                        break;
                    case 3:
                        tableName = "prod_zy02c";
                        break;
                    case 4:
                        tableName = "prod_sj9a";
                        break;
                    case 5:
                        tableName = "prod_hj1c";
                        break;
                    case 6:
                        tableName = "prod_gf3";
                        break;
                    case 7:
                        tableName = "prod_gf1bcd";
                        break;
                    case 8:
                        tableName = "prod_ahsi_gf5";
                        break;
                    case 9:
                        tableName = "prod_aius_gf5";
                        break;
                    case 10:
                        tableName = "prod_dpc_gf5";
                        break;
                    case 11:
                        tableName = "prod_emi_gf5";
                        break;
                    case 12:
                        tableName = "prod_gmi_gf5";
                        break;
                    case 13:
                        tableName = "prod_vims_gf5";
                        break;
                    case 14:
                        tableName = "prod_gf6";
                        break;
                    /*case 10:
                        tableName = "prod_gf7";
                        break;*/
                    default:
                        break;
                }

                string SQLString = string.Format("select name from {0} where name in (", tableName);
                for (int i = 0; i < dataNames.Count; i++)
                {
                    SQLString = string.Format("{0}'{1}',", SQLString, dataNames[i]);
                }
                SQLString = SQLString.TrimEnd().TrimEnd(',');
                SQLString += ")";

                //根据数据库名称获取库访问实例
                //mySQLOperator = new DBMySqlOperating();

                MySqlBaseUti = mySQLOperator.GetSubDbUtilities(EnumDBType.EVDB);

                returnDS = MySqlBaseUti.GetDataSet(SQLString);

                if (returnDS != null && returnDS.Tables.Count > 0)
                {
                    foreach (DataRow dr in returnDS.Tables[0].Rows)
                    {
                        result.Remove(dr["name"].ToString());
                    }
                }

                ii++;
            }

            return result;
        }



        /// <summary>
        ///获取入库数据的QRSTCODE
        /// </summary>
        /// <param name="dataNames"></param>
        /// <param name="dataType">“GF1”“HJ”“ZY3”“ZY02C”</param>
        /// <returns></returns>
        public List<string> GetQrstCodeByDataNames(List<string> dataNames)
        {
            List<string> result = new List<string>();
            if (dataNames == null || dataNames.Count == 0)
            {
                return result;
            }

            string tableName = string.Empty;

            //根据数据库名称获取库访问实例
            //mySQLOperator = new DBMySqlOperating();

            MySqlBaseUti = mySQLOperator.GetSubDbUtilities(EnumDBType.EVDB);


            //遍历全部表
            int ii = 0;
            while (ii < 15)
            {
                switch (ii)
                {
                    case 0:
                        tableName = "prod_gf1";
                        break;
                    case 1:
                        tableName = "prod_hj";
                        break;
                    case 2:
                        tableName = "prod_zy3";
                        break;
                    case 3:
                        tableName = "prod_zy02c";
                        break;
                    case 4:
                        tableName = "prod_sj9a";
                        break;
                    case 5:
                        tableName = "prod_hj1c";
                        break;
                    case 6:
                        tableName = "prod_gf3";
                        break;
                    case 7:
                        tableName = "prod_gf1bcd";
                        break;
                    case 8:
                        tableName = "prod_ahsi_gf5";
                        break;
                    case 9:
                        tableName = "prod_aius_gf5";
                        break;
                    case 10:
                        tableName = "prod_dpc_gf5";
                        break;
                    case 11:
                        tableName = "prod_emi_gf5";
                        break;
                    case 12:
                        tableName = "prod_gmi_gf5";
                        break;
                    case 13:
                        tableName = "prod_vims_gf5";
                        break;
                    case 14:
                        tableName = "prod_gf6";
                        break;
                    /*case 10:
                        tableName = "prod_gf7";
                        break;*/
                    default:
                        break;
                }

                string SQLString = string.Format("select QRST_CODE from {0} where name in (", tableName);
                for (int i = 0; i < dataNames.Count; i++)
                {
                    SQLString = string.Format("{0}'{1}',", SQLString, dataNames[i]);
                }
                SQLString = SQLString.TrimEnd().TrimEnd(',');
                SQLString += ")";

                returnDS = MySqlBaseUti.GetDataSet(SQLString);

                if (returnDS != null && returnDS.Tables.Count > 0)
                {
                    foreach (DataRow dr in returnDS.Tables[0].Rows)
                    {
                        result.Add(dr["QRST_CODE"].ToString());
                    }
                }

                ii++;
            }

            return result;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void buttonStart_Click(object sender, EventArgs e)
        {

        }
        private void buttonStop_Click(object sender, EventArgs e)
        {

        }
        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 单个高分数据入库 @zhangfeilong
        /// </summary>
        /// <param name="obj"></param>
        private void importSingleFileThread(object obj)
        {
            startTime = DateTime.Now;
            string filePath = (string)obj;
            //if (!TSPCommonReference.isCreated)
            //{
            //    TSPCommonReference.Create();
            //}
            if (!File.Exists(filePath))
            {
                MessageBox.Show("路径不合法");
                this.SetText("");
                return;
            }
            FileInfo file = new FileInfo(filePath);
            string fileName = file.Name;
            string orderCode = null;
            string appendText = "";
            orderCode = importTarGz(filePath);
            endTime = DateTime.Now;
            TimeSpan totalTime = endTime - startTime;
            if (isOrderProcessSucess(fileName, orderCode))
            {
                if (ctrlVirtualDirSetting1.UsingVirtualDir)
                {
                    List<string> fns = new List<string>();
                    fns.Add(fileName);
                    Add2VirtualDir(fns);
                }

                appendText = Environment.NewLine + fileName + "入库成功" + Environment.NewLine + "用时:" + totalTime;
                //2016/12/20
                this.SetText("");
            }
            else
            {
                //添加将入库失败的文件复制到一个文件夹下的功能，@zhangfeilong
                //if (!Directory.Exists(recordErrorFilePath))
                //{
                //    Directory.CreateDirectory(recordErrorFilePath);
                //}
                //string destinationPath = Path.Combine(recordErrorFilePath, fileName);
                //File.Copy(file.FullName, destinationPath, true);
                appendText = Environment.NewLine + "失败文件为：" + fileName + Environment.NewLine + "用时:" + totalTime + Environment.NewLine + "失败文件日志为：" + getErrorLogsByOrderCode(orderCode);
                //2016/12/20
                this.SetText("");
            }
            appendMsgToRecordText(appendText);
            Addmessege1(appendText);
            ButtonHandle();
        }

        private void Add2VirtualDir(List<string> fns)
        {
            List<string> codes = GetQrstCodeByDataNames(fns);
            ctrlVirtualDirSetting1.AddFileLink(codes);
        }

        private void appendMsgToRecordText(string appendText)
        {
            if (!File.Exists(RecordTextPath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(RecordTextPath));
                //appendText = "已执行原始数据操作的GF1数据压缩包有，请注意这些数据未执行数据预处理过程。" + Environment.NewLine;
                StreamWriter sw = File.CreateText(RecordTextPath);
                sw.WriteLine(appendText);
                sw.Flush();
                sw.Close();
            }
            else
            {
                File.AppendAllText(RecordTextPath, appendText);
            }
        }

        private bool isOrderProcessSucess(string fileName, string orderCode)
        {
            bool isProcessed = false;
            int num = 0;
            while (num < 50)
            {
                OrderClass order = OrderManager.GetOrderByCode(orderCode);
                if (order.Status == EnumOrderStatusType.Error || order.Status == EnumOrderStatusType.Suspended)
                {
                    break;
                }
                else if (order.Status == EnumOrderStatusType.Completed)
                {
                    isProcessed = true;
                    break;
                }
                else if (order.Status == EnumOrderStatusType.Processing)
                {
                    Thread.Sleep(10000);
                    num++;
                }
            }
            return isProcessed;
        }


        private void importThread(object obj)
        {
            ImportMessage.Clear();
            startTime = DateTime.Now;
            string filePath = (string)obj;
            if (!Directory.Exists(filePath))
            {
                MessageBox.Show("路径不合法");
                this.SetText("");
                return;
            }

            if (!isSearch)
                FiltedFileNames1 = searNewData(filePath);
            ////遍历tar.gz文件列表，每个文件进行入库操作
            for (int i = 0; i < FiltedFileNames1.Length; i++)
            {
                FileInfo fi = dicNameFi[FiltedFileNames1[i]];
                try
                {
                    importTarGz(fi.FullName);
                }
                catch
                {
                    continue;
                }
            }

            List<string> sucessImport = new List<string>();
            List<string> errorImport = new List<string>();
            List<string> errorLogs = new List<string>();
            foreach (var item in ImportMessage)
            {
                bool isProcessed = false;
                int num = 0;
                while (num < 600)
                {
                    OrderClass orderClass3 = OrderManager.GetOrderByCode(item.Key);
                    if (orderClass3.Status == EnumOrderStatusType.Error || orderClass3.Status == EnumOrderStatusType.Suspended)
                    {
                        errorImport.Add(item.Value);
                        errorLogs.Add("失败文件为：" + item.Value + Environment.NewLine + "失败日志为：\r\n" + getErrorLogsByOrderCode(item.Key));
                        isProcessed = true;
                        break;
                    }
                    else if (orderClass3.Status == EnumOrderStatusType.Completed)
                    {
                        sucessImport.Add(item.Value);
                        isProcessed = true;
                        break;
                    }
                    else if (orderClass3.Status == EnumOrderStatusType.Processing)
                    {
                        Thread.Sleep(10000);
                        num++;
                        isProcessed = false;
                    }
                }
                if (!isProcessed)
                    errorImport.Add(item.Value);
            }
            //删除入库成功的文件
            //foreach (string filename in sucessImport)
            //{
            //    FileInfo file = new FileInfo(filename);
            //    file.Delete();
            //}
            SourceDataDelete dataDelete = new SourceDataDelete(filePath, this);
            List<String> needDeleteDataList = new List<String>();
            FileInfo[] files = dataDelete.getAllFiles(filePath);
            if (files != null && files.Length > 0)
            {
                needDeleteDataList = dataDelete.TraverseSourceDataInDB(files);
            }
            foreach (FileInfo file in files)
            {
                if (needDeleteDataList.Contains(file.Name))
                {
                    file.Delete();
                }
            }

            string errorFile = "";
            string strErrorLogs = "";
            //添加将入库失败的文件复制到一个文件夹下的功能，@zhangfeilong
            //if (!Directory.Exists(recordErrorFilePath))
            //{
            //    Directory.CreateDirectory(recordErrorFilePath);
            //}
            //foreach (string filename in errorImport)
            //{
            //    FileInfo errorFileInfo = new FileInfo(filename);
            //    string destinationPath = Path.Combine(recordErrorFilePath, errorFileInfo.Name);
            //    File.Copy(filename, destinationPath,true);
            //    errorFile += filename + "#";
            //}
            foreach (string errorLog in errorLogs)
            {
                strErrorLogs += errorLog + "\r\n";
            }
            errorFile = errorFile.TrimEnd('#');
            endTime = DateTime.Now;
            TimeSpan totalTime = endTime - startTime;
            int errorCount = FiltedFileNames1.Length - needDeleteDataList.Count; 
            //string appendText = "此次任务已完成!共入库" + FiltedFileNames1.Length + "条记录，入库失败" + errorImport.Count + "条。" + Environment.NewLine + "失败文件为：" + errorFile + Environment.NewLine + "用时:" + totalTime + Environment.NewLine + "失败文件日志为：" + strErrorLogs;
            string appendText = "此次任务已完成!共入库" + FiltedFileNames1.Length + "条记录，入库失败" + errorCount + "条。" + Environment.NewLine + "用时:" + totalTime;
            //2016/12/20
            this.SetText("");

            if (ctrlVirtualDirSetting1.UsingVirtualDir)
            {
                string DirPath = this.textBox1.Text;
                FileInfo[] fileInfos;
                DirectoryInfo di_data = new DirectoryInfo(DirPath);
                fileInfos = di_data.GetFiles("*.tar.gz", SearchOption.AllDirectories);
                for (int i = 0; i < fileInfos.Length / 1000 + 1; i++)
                {
                    //每1000条执行一次
                    if (i * 1000 > fileInfos.Length - 1)
                    {
                        break;
                    }
                    List<string> filenames = new List<string>();
                    for (int j = i * 1000; j < 1000 * (i + 1); j++)
                    {
                        if (j > fileInfos.Length - 1)
                        {
                            break;
                        }
                        filenames.Add(fileInfos[j].Name);
                    }
                    Add2VirtualDir(filenames);
                }
            }

            try
            {
                if (!File.Exists(RecordTextPath))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(RecordTextPath));
                    //appendText = "已执行原始数据操作的GF1数据压缩包有，请注意这些数据未执行数据预处理过程。" + Environment.NewLine;
                    StreamWriter sw = File.CreateText(RecordTextPath);
                    sw.WriteLine(appendText);
                    sw.Flush();
                    sw.Close();
                }
                else
                {
                    //appendText = fifi.Name + Environment.NewLine;
                    File.AppendAllText(RecordTextPath, appendText);
                }


                Addmessege1("此次任务已完成!共入库" + FiltedFileNames1.Length + "条记录，入库失败" + errorCount + "条。" + Environment.NewLine + "失败文件为：" + errorFile + Environment.NewLine + "用时:" + totalTime);
                isSearch = false;
                //FiltedFileNames = null;
                //2016/12/20
                this.SetText("");
            }
            catch
            {
                isSearch = false;
                //continue;
            }

            //}
            ButtonHandle();
        }


        private string getErrorLogsByOrderCode(string orderCode)
        {
            StringBuilder result = new StringBuilder();
            //mySQLOperator = new DBMySqlOperating();
            MySqlBaseUti = mySQLOperator.GetSubDbUtilities(EnumDBType.MIDB);
            string searchSql = string.Format("select message from translog where message like '{0}'", orderCode + "%");
            returnDS = MySqlBaseUti.GetDataSet(searchSql);
            if (returnDS != null && returnDS.Tables.Count > 0)
            {
                foreach (DataRow row in returnDS.Tables[0].Rows)
                {
                    result.Append(row[0].ToString() + "\r\n");
                }
            }
            return result.ToString();
        }

        public string importTarGz(string fileName)
        {
            FileInfo fifi = new FileInfo(fileName);
            string orderCode = "";

            QRST_DI_TS_Process.Orders.OrderClass orderClass1 = null;
            string dataType = getDataType(fileName);
            switch (dataType)
            {
                case "GF1":
                    orderClass1 = new IOGF1DataImport().Create();
                    break;
                case "GF2":
                    orderClass1 = new IOGF1DataImport().Create();
                    break;
                case "GF4":
                    orderClass1 = new IOGF1DataImport().Create();
                    break;
                case "GF3":
                    orderClass1 = new IOGF3DataImport().Create();
                    break;
                case "GF1B":
                case "GF1C":
                case "GF1D":
                    orderClass1 = new IOBCDDataImport().Create();
                    break;
                case "GF5":
                    orderClass1 = new IOGF5DataImport().Create(fileName);
                    break;
                case "GF6":
                    orderClass1 = new IOGF6DataImport().Create();
                    break;
                case "GF7":
                    break;
                case "DEM":
                    break;
                case "DOM":
                    break;
                case "HJ1A":
                case "HJ1B":
                    orderClass1 = new IOHJDataImportStandard().Create();
                    break;
                case "ZY3":
                    orderClass1 = new IOZY3DataImport().Create();
                    break;
                case "ZY02C":
                    orderClass1 = new IOZY02cDataImport().Create();
                    break;
                case "SJ9A":
                    orderClass1 = new IOSJ9ADataImport().Create();
                    break;
                case "HJ1C":
                    orderClass1 = new IOHJ1CDataImport().Create();
                    break;
                default:
                    break;
            }
            OrderManager.SubmitOrder(orderClass1);
            orderCode = orderClass1.OrderCode;
            ImportMessage.Add(orderCode, fifi.FullName);
            string ws = "%OrderWorkspace%";

            while (ws == "%OrderWorkspace%")
            {
                ////循环监控订单是否被执行
                OrderClass orderClass2 = OrderManager.GetOrderByCode(orderCode);
                ws = orderClass2.OrderWorkspace;
                if (ws == "%OrderWorkspace%")
                {
                    System.Threading.Thread.Sleep(1000);
                }
            }
            //避免多线程下，拷贝源数据到订单文件夹而订单文件夹未生成的情况。@jh
            while (!Directory.Exists(ws)) 
            {
                Thread.Sleep(2500); 
            }
            //Copy .tar.gz 到workspace下
            string sourceFilePath = fifi.FullName;
            string destFilePath = Path.Combine(ws, fifi.Name);

            //窗体中显示后台操作
            Addmessege1(String.Format("正在提交数据: {0}{1}", fifi.Name, Environment.NewLine));
            int num = 1;
            while (num < 4)
            {
                try
                {
                    File.Copy(sourceFilePath, destFilePath);
                    break;
                }
                catch (Exception ex)
                {
                    num++;
                    if (num == 4)
                    {
                        Addmessege1(String.Format("数据提交失败: {0}{1}", fifi.Name, Environment.NewLine));
                        this.SetText("");
                        return null;
                        //throw ex;
                    }
                }
            }
            //File.Copy(sourceFilePath, destFilePath);
            //继续处理点单消息
            //submitWS.ResumeGF1DataImportOrder(code);

            ////循环监控订单是否被执行

            string appendText = "";
            num = 0;
            while (num < 35)
            {
                OrderClass orderClass3 = OrderManager.GetOrderByCode(orderCode);
                if (orderClass3 != null && orderClass3.Status == EnumOrderStatusType.Suspended)
                {
                    OrderManager.UpdateOrderStatus(orderClass3.OrderCode, EnumOrderStatusType.Waiting);
                    break;
                }
                Thread.Sleep(1000);
                num++;
            }
            if (num == 35)
            {
                Addmessege1(String.Format("数据提交失败: {0}{1}", fifi.Name, Environment.NewLine));
                this.SetText("");
                return null;
                //throw ex;
            }
            if (!File.Exists(RecordTextPath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(RecordTextPath));
                appendText = "已执行原始数据操作的GF1数据压缩包有，请注意这些数据未执行数据预处理过程。" + Environment.NewLine;
                StreamWriter sw = File.CreateText(RecordTextPath);
                sw.WriteLine(appendText);
                sw.Flush();
                sw.Close();
            }
            else
            {
                appendText = fifi.Name + Environment.NewLine;
                File.AppendAllText(RecordTextPath, appendText);
            }


            //窗体中显示后台操作
            Addmessege1(String.Format("完成数据提交: {0}{1}", fifi.Name, Environment.NewLine)); 
            return orderCode;
            //2016/12/20
            this.SetText("");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                //this.textBox3.Text = fbd.SelectedPath;
            }
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            searNewData();
        }

        private void btStartMonitor_Click(object sender, EventArgs e)
        {
            RecordTextPath = string.Format(@"{0}\ImportTargzLog_{1}.txt", DBRecordPath,DateTime.Today.ToString("yyyyMMdd"));
            //recordErrorFilePath = string.Format(@"{0}\errorFiles_{1}",DBRecordPath, DateTime.Today.ToString("yyyyMMdd"));
            if (this.textBox1.Text == null || this.textBox1.Text =="")
            {
                MessageBox.Show("路径不能为空");
                return;
            }
            this.buttonImport.Enabled = false;
            this.btStopMonitor.Enabled = true;
            this.btStartMonitor.Enabled = false;
            Addmessege1("正在监控此目录.........\n");
            //startMonitor(this.textBox1.Text);
            //if (!TSPCommonReference.isCreated)
            //{
            //    TSPCommonReference.Create();
            //}
            autoImport = new AutomaticTarImport(this.textBox1.Text,this);
            autoImport.startMonitor();

        }

        /// <summary>
        /// 开始进行文件夹监视
        /// </summary>
        /// <param name="obj"></param>
        private void startMonitor(object obj)
        {
            string filePath = (string)obj;
            //if (!TSPCommonReference.isCreated)
            //{
            //    TSPCommonReference.Create();
            //}
            if (!Directory.Exists(filePath))
            {
                MessageBox.Show("路径不合法");
                return;
            }
            watcher(filePath, "*.tar.gz");
        }

        /// <summary>
        /// 文件夹的监视器 @zhangfeilong
        /// </summary>
        /// <param name="path">监视路径</param>
        /// <param name="filter">过滤器</param>
        public void watcher(string path, string filter)
        {
            fsWatcher = new FileSystemWatcher();
            fsWatcher.Path = path;
            //watcher.Filter = filter;
            fsWatcher.Created += new FileSystemEventHandler(OnProcess);
            fsWatcher.EnableRaisingEvents = true;
            fsWatcher.NotifyFilter = NotifyFilters.Attributes | NotifyFilters.CreationTime | NotifyFilters.DirectoryName | NotifyFilters.FileName | NotifyFilters.LastAccess
                                   | NotifyFilters.LastWrite | NotifyFilters.Security | NotifyFilters.Size;
            fsWatcher.IncludeSubdirectories = true;
        }

        private void OnProcess(object source, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Created)
            {
                OnCreated(source, e);
            }
        }

        private void OnCreated(object source, FileSystemEventArgs e)
        {
            if (File.Exists(e.FullPath))
            {
                FileInfo file = new FileInfo(e.FullPath);
                while (true)
                {
                    if (!isFileInUse(e.FullPath) && e.FullPath.EndsWith(".tar.gz"))
                    {
                        if (!newFileInDB(file.Name))
                        {
                            Addmessege1("文件：" + e.FullPath + "已经存在数据库中！" + Environment.NewLine);
                            //2016/12/20
                            this.SetText("");
                        }
                        else
                        {
                            Thread th = new Thread(importSingleFileThread);
                            th.Start(e.FullPath);
                        }
                        break;
                    }
                }
            }
            else if (Directory.Exists(e.FullPath))
            {

            }
            else
            {
                Console.WriteLine("****");
            }


        }

        /// <summary>
        /// 判断文件是否正在使用 @zhangfeilong
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool isFileInUse(string fileName)
        {
            bool inUse = true;
            FileStream fs = null;
            try
            {

                fs = new FileStream(fileName, FileMode.Open, FileAccess.Read,
                FileShare.None);
                inUse = false;
            }
            catch
            {

            }
            finally
            {
                if (fs != null)

                    fs.Close();
            }
            return inUse;//true表示正在使用,false没有使用  
        }

        private void btStopMonitor_Click(object sender, EventArgs e)
        {
            if (fsWatcher != null)
            {
                fsWatcher.EnableRaisingEvents = false;
            }
            this.btStartMonitor.Enabled = true;
            this.buttonImport.Enabled = true;
        }

        private void btDeleteData_Click(object sender, EventArgs e)
        {
            SourceDataDelete dataDelete = new SourceDataDelete(this.textBox1.Text,this);
            Addmessege1(dataDelete.confirmDelete());
        }

        private void btStopMonitor_Click_1(object sender, EventArgs e)
        {
            this.btStartMonitor.Enabled = true;
            this.buttonImport.Enabled = true;
            this.btStopMonitor.Enabled = false;
            autoImport.stopMonitor();
        }
    }
}
