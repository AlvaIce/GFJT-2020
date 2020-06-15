using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using QRST_DI_TS_Process.Site;
using QRST_DI_TS_Basis.DirectlyAddress;
using MySql.Data.MySqlClient;
using System.Net;
using QRST_DI_DS_Basis;


namespace DataPrepare
{
    public partial class Form1 : Form
    {

        private delegate void AddMessageDelegate(string message);
        public static Form1 mainForm;
        public static JCGXMessageCenterConnection.MessageCenterClient mcc;
        static WS_QDB_SubmitOrder.SubmitOrderSoapClient ssc;
        Dictionary<string, string> OrderWorkspace = new Dictionary<string, string>();
        Dictionary<string, List<string>> TaskOrders;      //集成共享任务单与数据库数据订单对应表
        Dictionary<string, List<string>> TaskQrstCodes;      //集成共享任务单与数据库数据订单对应表

        public Form1()
        {
            try
            {


                InitializeComponent();
                TaskOrders = new Dictionary<string, List<string>>();
                TaskQrstCodes = new Dictionary<string, List<string>>();
                ssc = new WS_QDB_SubmitOrder.SubmitOrderSoapClient();
                JCGXMessageCenterConnection.ReadConfig rc = new JCGXMessageCenterConnection.ReadConfig();
                //mcc = JCGXMessageCenterConnection.MessageCenterClient.GetInstance(Constant.MessageCentorIP, int.Parse(Constant.MessageCentorPort), Constant.SystemName);
                Console.WriteLine(Constant.SystemName);
                mcc = JCGXMessageCenterConnection.MessageCenterClient.GetInstance(rc.McIP, rc.McPort, Constant.SystemName);

                JCGXMessageCenterConnection.MessageCenterClient.MCClientReceiveMessageEvent +=
                    new Action<string>(MessageCenterClient_MCClientReceiveMessageEvent);

                JCGXMessageCenterConnection.ListenManager.GetInstance().ServerStart();


                ZHSJKMsgHandler zhsjkMsg = new ZHSJKMsgHandler();
                zhsjkMsg.ZHSJKMonitorServiceStart();

                Log = "服务已启动";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            //DataPreparing(new string[] { "T4157852205865" });
        }

        public void MessageCenterClient_MCClientReceiveMessageEvent(string message)
        {

            Log = "接收到消息：" + message;

            //判断自己系统的开始字符串;订单号#数据准备#开始#系统名称#
            //T4276641463093#数据入库#开始#综合数据库系统#P2766415783130
            if (message.Contains("数据准备#开始#"))
            {
                string[] parameters = message.Split("#".ToCharArray());
                DataPreparing(parameters);
            }

            else if (message.Contains("数据入库#开始#"))
            {
                string[] parameters = message.Split("#".ToCharArray());
                if (Constant.NeedOutput == "true")
                {//D:\瓦片处理成果\20170302\P2766553722007
                    string outputtilepath = Path.Combine(Constant.OutputTilePath, DateTime.Today.ToString("yyyyMMdd"), parameters[4]);
                    Log = "要求导出瓦片数据，开始导出到" + outputtilepath;
                    ExportTileData(parameters, outputtilepath);
                }
                if (Constant.Need2App == "false")
                {
                    string outputtilepath = Path.Combine(Constant.AppTilePath);
                    //\\192.168.3.127\KSHShareFolders\DBNeedPushToAppFiles
                    //string outputtilepath = Path.Combine(Constant.OutputTilePath);
                    Log = "要求将关注瓦片导出输出到APP文件夹，开始导出到" + outputtilepath;
                    ExportTileData2App(parameters, outputtilepath);
                }

                DataImporting(parameters);
                //if (Constant.zipdata == "true")
                //{
                //    string outputtilepath = Path.Combine(Constant.zipdata);
                //    Log = "要求将打包瓦片数据导出输出到共享文件夹文件夹，开始导出到" + outputtilepath;
                //    ExportTileData2App(parameters, outputtilepath);
                //}
            }

            else if (message.Contains("AndroidApp"))
            {
                SearchRawDataNotCut search = new SearchRawDataNotCut(message);
                search.InsertNeedTileInfo();
                List<string> result = search.getRawDataNotCutTile();
                string strReturn = StaticTools.getReturnMsg(result, SearchRawDataNotCut.taskUserName);
                Log = "处理完毕，返回结果为：" + strReturn;
                StaticTools.SendMessage(strReturn);
            }
        }

        private void ExportTileData2App(string[] parameters, string outputtilepath)
        {
            string taskID = parameters[0];
            string ordercode = parameters[4];
            string workspace = ssc.GetOrderWorkspace(ordercode);
            string scrTilepath = Path.Combine(workspace, "TiledData");
            // string scrTilepath= Path.Combine(Constant.OutputTilePath, DateTime.Today.ToString("yyyyMMdd"), parameters[4]);
            if (Directory.Exists(scrTilepath))//\\192.168.2.113\QRST_DB_Share\P2766552889313\TiledData
            {
                SendMessage(string.Format("{0}#瓦片到APP#开始#{1}#{2}", taskID, Constant.SystemName, ordercode));

                //QRST_DI_DS_Basis.DirectoryUtil.CopyDirTraversal(scrTilepath, outputtilepath);
                string[] PNGfis = Directory.GetFiles(scrTilepath, "*.png", SearchOption.AllDirectories);
                string[] JPGfis = Directory.GetFiles(scrTilepath, "*.jpg", SearchOption.AllDirectories);
                List<string> fis = new List<string>();
                fis.AddRange(PNGfis);
                fis.AddRange(JPGfis);
                string tileLRCStrs = "";

                foreach (string fi in fis)
                {
                    string tileLRCStr = "";
                    try
                    {
                        //if (IsAppNeed(fi, out tileLRCStr))
                        //{
                        //\\192.168.3.127\KSHShareFolders\DBNeedPushToAppFiles\GF1_PMS1_20130529_L1A0000067488_7_1296_2959.png
                        // string ceshi = Path.Combine(outputtilepath, Path.GetFileName(fi));
                        File.Copy(fi, Path.Combine(outputtilepath, Path.GetFileName(fi)));
                        //\\192.168.2.117\QRST_DB_Share\P2766415783130\TiledData\GF1_MSS1\Preview\GF1_PMS1_20130529_L1A0000067488_7_1296_2959.png

                        tileLRCStrs = (tileLRCStrs != "") ? string.Format("{0},{1}", tileLRCStrs, tileLRCStr) : tileLRCStr;
                        //}
                    }
                    catch (Exception ex)
                    {

                        throw ex;
                    }
                }

                Log = "瓦片数据导出到APP完成！";
                SendMessage(string.Format("{0}#瓦片到APP#结束#{1}#{2}#{3}", taskID, Constant.SystemName, ordercode, tileLRCStrs));
            }
            else
            {
                Log = "异常，未找到瓦片数据！";
            }
        }

        private bool IsAppNeed(string tilefile, out string tileLRCStr)
        {
            string[] strs = tilefile.Split(new char[] { '\\' });
            tilefile = strs[strs.Length - 1];
            bool rst = false;
            tileLRCStr = "";

            TileNameArgs tna = new CorrectedTileNameArgs(tilefile);
            if (!tna.Created)
            {
                tna = new ProdTileNameArgs(tilefile);
            }

            if (!tna.Created)
            {
                return rst;
            }
            int counttile = 0;
            string lv = tna.TileLevel;
            string row = tna.Col;
            string col = tna.Row;

            MySqlConnection con = new MySqlConnection(Constant.GetFromAppSettingTable("ConnectionStringISDB"));

            con.Open();
            string sql = string.Format(@"select count(*) from app_need_tile where level ='{0}' and row = '{1}' and col = '{2}'", lv, row, col);
            MySqlCommand cmd = new MySqlCommand(sql, con);
            counttile = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();

            if (counttile != 0)
            {
                rst = true;
                tileLRCStr = string.Format("{0}_{1}_{2}", lv, row, col);
            }
            return rst;
        }

        private void ExportTileData(string[] parameters, string outputtilepath)
        {
            string ordercode = parameters[4];
            string workspace = ssc.GetOrderWorkspace(ordercode);
            string scrTilepath = Path.Combine(workspace, "TiledData");
            if (Directory.Exists(scrTilepath))
            {//\\192.168.2.113\QRST_DB_Share\P2766553722007\TiledData
                QRST_DI_DS_Basis.DirectoryUtil.CopyDirTraversal(scrTilepath, outputtilepath);
                Log = "瓦片数据导出完成！";
            }
            else
            {
                Log = "异常，未找到瓦片数据！";
            }
        }
        public void SendMessage(String msg)
        {
            mcc.SendMessage(msg);

            Log = "发送消息：" + msg;
        }

        string log;
        public string Log
        {
            get
            {
                return log;
            }
            set
            {
                log = log + "\r\n" + DateTime.Now.ToString() + value;
                if (this.richTextBox1.InvokeRequired)
                {
                    Action<string> actionStr = UpdateTextbox;
                    this.richTextBox1.Invoke(actionStr, log);
                }
                else
                {
                    UpdateTextbox(log);
                }
            }
        }

        public void UpdateTextbox(string log)
        {
            richTextBox1.Text = log;
            richTextBox1.Select(richTextBox1.TextLength, 0);
            richTextBox1.ScrollToCaret();
            richTextBox1.Refresh();
        }

        //数据入库
        private void DataImporting(string[] parameters)
        {
            string taskID = "";
            string orderID = "";

            string rowcol = "";

            try
            {
                taskID = parameters[0];
                Log = "正在执行任务：" + taskID;
                orderID = parameters[4];
                if (parameters.Length == 6)
                {
                    rowcol = parameters[5];//网络定制传6个参数,手机端传5个参数,这个手机端入库时这个参数为空,出错
                    //string[] rowcols = rowcol.Split(new char[] { ';' });
                    //List<string> rclist = new List<string>(rowcols);//数组转换list
                    //foreach (string rc in rowcols)
                    //{
                    //    string name = "";
                    //    string taskidrclist = string.Format("{0}#{1}", taskID, rc);
                    //    taskidrclist += name + "#";
                    //}
                    string taskidrclist = string.Format("{0}#{1}", taskID, rowcol);
                    //string taskidrowcols = string.Format("{0}#{1}", taskID, rowcols);

                    if (!TaskOrders.ContainsKey(taskID))
                    {
                        List<string> lstOrder = new List<string>();
                        TaskOrders.Add(taskID, lstOrder);
                    }

                    if (!TaskOrders[taskID].Contains(orderID))
                    {
                        TaskOrders[taskID].Add(orderID);
                    }


                    bool success = ssc.ResumeGF1DataImportOrder(orderID);//预处理组再次提交相同taskID下的相同的订单，success就是false 跳到else提示下单失败
                    if (success)
                    {
                        Log = "正在执行任务：" + taskID + "数据订单：" + orderID;
                        System.Threading.Tasks.Task task = new System.Threading.Tasks.Task(new Action<object>(IsDITaskComplished), taskidrclist);
                        task.Start();
                    }
                    else
                    {
                        Log = "任务：" + taskID + "数据订单：" + orderID + "下单失败";
                    }
                }
                else
                {
                    taskID = parameters[0];
                    Log = "正在执行任务：" + taskID;
                    orderID = parameters[4];
                    if (!TaskOrders.ContainsKey(taskID))
                    {
                        List<string> lstOrder = new List<string>();
                        TaskOrders.Add(taskID, lstOrder);
                    }
                    if (!TaskOrders[taskID].Contains(orderID))
                    {
                        TaskOrders[taskID].Add(orderID);
                    }
                    bool success = ssc.ResumeGF1DataImportOrder(orderID);
                    if (success)
                    {
                        Log = "正在执行任务：" + taskID + "数据订单：" + orderID;
                        System.Threading.Tasks.Task task = new System.Threading.Tasks.Task(new Action<object>(IsDITaskComplished_forApp), taskID);
                        task.Start();
                    }
                    else
                    {
                        Log = "任务：" + taskID + "数据订单：" + orderID + "下单失败";
                    }

                }
            }
            catch (Exception ex)
            {
                Log = string.Format("异常（TaskID:{0} OrderID:{1}）：{2} ", taskID, orderID, ex.Message);
            }

        }

        //数据准备
        public void DataPreparing(string[] parameters)
        {
            OrderWorkspace.Clear();
            string taskID = parameters[0];
            Log = "正在执行任务：" + taskID;
            SendMessage(string.Format("{0}#数据准备#开始#{1}", taskID, Constant.SystemName));
            List<string> lstOrder = new List<string>();
            List<string> lstCode = new List<string>();

            if (TaskOrders.ContainsKey(taskID))
            {
                TaskOrders.Remove(taskID);
            }
            TaskOrders.Add(taskID, lstOrder);
            if (TaskQrstCodes.ContainsKey(taskID))
            {
                TaskQrstCodes.Remove(taskID);
            }
            TaskQrstCodes.Add(taskID, lstCode);

            //读task表,返回TaskID和DataPath             
            //MySqlBaseUtilities mysql = new MySqlBaseUtilities();
            //string selSql = "select TaskId,DataPath from midb.task_expand where IsCompleted = -1 limit 1";                
            //DataSet ds = mysql.GetDataSet(selSql);
            //mysql.Close();
            QDB_DBOperation_Dll.DBOperation dbo = new QDB_DBOperation_Dll.DBOperation();
            #region
            //zsm 20161021
            DataSet dataname = dbo.getdataname(taskID);
            if (dataname != null && dataname.Tables[0] != null && dataname.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dn in dataname.Tables[0].Rows)
                {
                    try
                    {
                        string dname = dn["dataname"].ToString();
                        DataSet ds = dbo.getcode(dname);
                        if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                try
                                {
                                    string qc = dr["QRST_CODE"].ToString();
                                    Log = "开始数据准备" + taskID + "订单数据：" + qc;

                                    //调用webservice生成文件的工作空间和订单号(一一对应)
                                    string ordercode;
                                    string[] qrst_code = new string[] { qc };

                                    ordercode = ssc.SubmitInstalledOrder("IOGF1CorrectedDataImport", qrst_code);
                                    TaskOrders[taskID].Add(ordercode);
                                    TaskQrstCodes[taskID].Add(qc);
                                }

                                catch (Exception ex)
                                {

                                    throw ex;
                                }

                            }

                            //System.Threading.Tasks.Task task = new System.Threading.Tasks.Task(new Action<object>(IsDPTaskComplished), taskID);
                            //task.Start();

                        }
                        else
                        {
                            SendMessage(string.Format("{0}#根据该数据名称在原始库里没有查到该数据code#数据准备失败#{1}", dname, Constant.SystemName));
                            return;

                        }

                    }
                    catch (Exception ex)
                    {

                        throw ex;
                    }
                }
                System.Threading.Tasks.Task task = new System.Threading.Tasks.Task(new Action<object>(IsDPTaskComplished), taskID);
                task.Start();
            }
            else
            {
                SendMessage(string.Format("{0}#根据该ID没有查到该数据名称#数据准备失败#{1}", taskID, Constant.SystemName));

            }
            #endregion


            #region
            //DataSet ds = dbo.GetQRSTCODEs(taskID);
            ////判断是否有待处理任务
            //if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            //{
            //    foreach (DataRow dr in ds.Tables[0].Rows)
            //    {
            //        try
            //        {
            //            string qc = dr["QRST_CODE"].ToString();
            //            Log = "开始数据准备" + taskID + "订单数据：" + qc;

            //            //调用webservice生成文件的工作空间和订单号(一一对应)
            //            string ordercode;
            //            string[] qrst_code = new string[] { qc };

            //            ordercode = ssc.SubmitInstalledOrder("IOGF1CorrectedDataImport", qrst_code);
            //            TaskOrders[taskID].Add(ordercode);
            //            TaskQrstCodes[taskID].Add(qc);
            //            //string workspacetmp = workspace.Replace(@"\", @"\\");

            //            ////更新表,调wh的开始任务方法
            //            ////orders orders_expand 入库 并更新orders_expand 中的过程为数据准备的下一个过程，状态为0 待处理
            //            ////string tmp = ds.Tables[0].Rows[0]["TaskID"].ToString();

            //            ////dbo.InsertAndUpdateOrders("01", ordercode, "数据准备", workspace, DateTime.Now);
            //            //dbo.InsertAndUpdateOrders(ds.Tables[0].Rows[0]["TaskID"].ToString(), ordercode, "数据准备", workspacetmp, DateTime.Now);


            //            ////上传数据，提交订单
            //            ////string temp = workspace + @"\" + System.IO.Path.GetFileName(fp);
            //            //System.IO.File.Copy(fp, workspace + @"\" + System.IO.Path.GetFileName(fp));
            //            //ssc.SubmitNewPordOrder(ordercode);

            //        }

            //        catch (Exception ex)
            //        {

            //            throw;
            //        }

            //    }

            //    System.Threading.Tasks.Task task = new System.Threading.Tasks.Task(new Action<object>(IsDPTaskComplished), taskID);
            //    task.Start();
            //    ////更新表,数据准备完成之后，更改任务表task_expand 中的IsCompleted字段 由-1更改为0
            //    //dbo.UpdateTaskIsCompleted(ds.Tables[0].Rows[0]["TaskID"].ToString());
            //}
            #endregion

        }

        public void IsDPTaskComplished(object objtaskID)
        {
            string taskid = objtaskID.ToString();
            bool[] orderiscompleted = new bool[TaskOrders[taskid].Count];
            for (int i = 0; i < orderiscompleted.Length; i++)
            {
                orderiscompleted[i] = false;
            }

            while (true)
            {
                bool allcompleted = true;

                for (int i = 0; i < orderiscompleted.Length; i++)
                {
                    if (orderiscompleted[i])
                    {
                        continue;
                    }
                    try
                    {
                        QDB_DBOperation_Dll.DBOperation dbo = new QDB_DBOperation_Dll.DBOperation();
                        string ordercode = TaskOrders[taskid][i];
                        string qrstcode = TaskQrstCodes[taskid][i];
                        string name = (new QDB_DBOperation_Dll.DBOperation()).getonedataname(qrstcode).Tables[0].Rows[0]["Name"].ToString();
                        string orderstatus = ssc.GetOrderStatus(ordercode);
                        string workspace = ssc.GetOrderWorkspace(ordercode);
                        if (workspace == "-1" || !System.IO.Directory.Exists(workspace)     //如果不Exists可能是共享文件夹访问权限的问题
                            || (orderstatus != "Suspended" && orderstatus != "Completed"))
                        {
                            allcompleted = false;
                        }
                        else
                        {
                            OrderWorkspace.Add(ordercode, workspace);
                            orderiscompleted[i] = true;
                            Log = "完成数据准备" + taskid + "订单数据：" + qrstcode;

                            string messages = "";
                            // if (taskid.Contains("WCPO")) (taskid != "")
                            if (taskid.Contains("WCPO"))//王栋要求如果订单以WCPO开头，则传数据名称name，否则传数据编码qrstcode
                            {

                                messages = string.Format("{0}#数据准备#结束#{1}#{2}#{3}#{4}", taskid, Constant.SystemName, name, ordercode, workspace);
                            }
                            else
                            {
                                messages = string.Format("{0}#数据准备#结束#{1}#{2}#{3}#{4}", taskid, Constant.SystemName, qrstcode, ordercode, workspace);

                            }

                            SendMessage(messages);
                        }
                    }
                    catch
                    {
                        allcompleted = false;
                    }
                }

                if (allcompleted)
                {
                    Log = "完成" + taskid + "全部数据准备！";
                    //SendMessage(string.Format("{0}#数据准备#结束#{1}#", taskid, Constant.SystemName));
                    break;
                }
                else
                {
                    //没有任务 休息3秒
                    System.Threading.Thread.Sleep(3000);
                }
            }
        }

        public void IsDITaskComplished(object objtaskID)
        {
            string objtaskid = objtaskID.ToString();
            string[] taskrclisk = objtaskid.Split("#".ToCharArray());


            //string taskid = objtaskID.ToString();
            string taskid = taskrclisk[0];

            // string[] rclisted = taskrclisk[1].Split(';');
            string[] rclisted = taskrclisk[1].TrimEnd(';').Split(';');
            bool[] orderiscompleted = new bool[TaskOrders[taskid].Count];
            for (int i = 0; i < orderiscompleted.Length; i++)
            {
                orderiscompleted[i] = false;
            }

            while (true)
            {
                bool allcompleted = true;

                for (int i = 0; i < orderiscompleted.Length; i++)
                {
                    if (orderiscompleted[i])
                    {
                        continue;
                    }
                    try
                    {
                        string ordercode = TaskOrders[taskid][i];
                        string orderstatus = ssc.GetOrderStatus(ordercode);
                        if (orderstatus != "Suspended" && orderstatus != "Completed")
                        {
                            allcompleted = false;
                        }
                        else
                        {
                            orderiscompleted[i] = true;
                            Log = "完成数据入库" + taskid + "订单号：" + ordercode;
                            string strPath = "";
                            string workspace = ssc.GetOrderWorkspace(ordercode);//\\192.168.2.145\QRST_DB_Share\P2764561729151\
                            strPath = tilesPath(workspace);
                            //zsm 20161025 传给网络组一个波段的文件
                            string newstrpath = "";
                            bool isCorrectedTile = GetProdType(taskid);//判断是否是三级产品
                            #region
                            //if (taskid.Contains("WCPO"))
                            if (taskid != "")
                            {

                                string[] strs = strPath.Split(new char[] { ',' });

                                //List<string> lrcadd = new List<string>();
                                foreach (string strsed in strs)
                                {
                                    if (!isCorrectedTile && !strsed.Trim().EndsWith("-1.c.tif"))
                                    {
                                        continue;
                                    }
                                    foreach (string lrc in rclisted)
                                    {
                                        if (lrc == "")
                                        {
                                            continue;
                                        }
                                        if (strsed.Contains(lrc))
                                        {
                                            //找到匹配的tile
                                            newstrpath += strsed + ",";
                                            break;
                                        }
                                        else
                                        {
                                            continue;
                                        }
                                    }
                                    #region
                                    ////string[] strr = strsed.Split("_".ToCharArray());
                                    //string[] strr = strsed.Split(new char[] { '_','-' });
                                    //if (strr.Length == 10)
                                    //{
                                    //    level = strr[4];
                                    //    row = strr[5];
                                    //    col = strr[6];
                                    //    string lrc = string.Format("{0}_{1}_{2}", level, row, col);
                                    //    // lrcadd.Add(lrc);
                                    //    if (rclisted.Contains(lrc))//判断网络组传的值是否包含lrc
                                    //    {
                                    //        newstrpath += strsed;
                                    //    }
                                    //    else
                                    //    {
                                    //        continue;
                                    //    }
                                    //}
                                    //else
                                    //{
                                    //    continue;

                                    //}
                                    #endregion
                                }

                                strPath = newstrpath;
                            }


                            #endregion
                            #region
                            //if (taskid.Contains("WCPO"))
                            //{
                            //    string row, col;
                            //    string[] strs = strPath.Split(new char[] { ',' });
                            //    string newstrpath = "";
                            //    foreach (string strsed in strs)
                            //    {
                            //        string[] strr = strsed.Split("_".ToCharArray());
                            //        if (strr.Length == 7)
                            //        {
                            //            row = strr[5];
                            //            col = strr[6];

                            //            string rows = parametersd[0];//行
                            //            string cols = parametersd[1];//列

                            //            if (row == rows && col == cols)
                            //            {
                            //                newstrpath += strr;
                            //            }
                            //            else
                            //            {
                            //                continue;

                            //            }
                            //        }
                            //    }
                            //    strPath = newstrpath;
                            //}
                            //else
                            //{
                            //    continue;
                            //}
                            #endregion

                            if (strPath == "")
                                strPath = "error";



                            SendMessage(string.Format("{0}#数据入库#结束#{1}#{2}#{3}", taskid, Constant.SystemName, ordercode, strPath));
                            #region
                            //zsm 20161025
                            //以行列号为名称把相同行列号所有波段数据打包
                            //收集所有的压缩包再次以taskID为名称打包为一个压缩包放到网络组指定的路径下（配置表里的路径）
                            if (isCorrectedTile)
                            {
                                //先发送打包开始，最后发送打包结束
                                SendMessage(string.Format("{0}#瓦片数据打包#开始#{1}#{2}", taskid, Constant.SystemName, ordercode));
                                string[] alltitles = strPath.TrimEnd(',').Split(",".ToCharArray());

                                List<string> zipfiles = new List<string>();
                                foreach (string rowcoll in rclisted)//比如rowcoll==7_111_112;
                                {
                                    if (rowcoll == "")
                                    {
                                        continue;
                                    }
                                    string samerowcoltitle = "";
                                    string samerowcoltitlePath = "";
                                    foreach (string titles in alltitles)
                                    {

                                        if (titles.Contains(rowcoll + "-1"))
                                        {
                                            samerowcoltitle = titles + ",";//相同行列号的文件名称找到了，存放的路径一定一样的，根据文件名截取共享路径
                                            //上面我得到的数据名称是遍历的最后一个符合行列号的名称，因为我要一个数据名称就能得到原始路径（共享文件夹的路径）             
                                            break;
                                        }
                                        else
                                        {
                                            continue;
                                        }

                                    }
                                    if (samerowcoltitle == "")
                                    {
                                        continue;

                                    }
                                    //samerowcoltitlePath==\\192.168.10.205\QRST_DB_Prototype\QRST_DB_Tile\8\7\1141\2957\PMS2\20160708
                                    samerowcoltitlePath = samerowcoltitle.Substring(0, samerowcoltitle.LastIndexOf("\\"));
                                    string samedatanametitles = "";
                                    if (Directory.Exists(samerowcoltitlePath))
                                    {
                                        string[] allfile = Directory.GetFiles(samerowcoltitlePath);
                                        //samerowcoltitle==\\192.168.10.205\QRST_DB_Prototype\QRST_DB_Tile\8\7\1141\2957\PMS2\20160708\GF1_PMS2_20160708_L1A0001692639_7_1141_2957-1.tif
                                        //string pipeiname = samerowcoltitle.Substring(0, samerowcoltitle.LastIndexOf("_"));//不能是-因为后缀名有的不是-结束有点是.jpg或者.pgw 后期在改为“-”
                                        int startindex = samerowcoltitle.LastIndexOf("-");
                                        int endindex = samerowcoltitle.LastIndexOf("\\");
                                        //zipnamerowcol=GF1_PMS2_20160708_L1A0001692639_7_1141_2957
                                        string zipnamerowcol = samerowcoltitle.Substring(endindex + 1, startindex - endindex - 1);
                                        foreach (string onefile in allfile)
                                        {
                                            if (onefile.Contains(zipnamerowcol))
                                            {
                                                samedatanametitles += onefile + ",";
                                            }
                                        }
                                        //string[] samedataname = samedatanametitles.Split(",".ToCharArray());
                                        string[] samedataname = samedatanametitles.TrimEnd(',').Split(",".ToCharArray());
                                        string newzipfilename = string.Format(@"{0}\{1}.gff", Constant.zipdata, zipnamerowcol);
                                        DataPacking.ZipFile(samedataname, newzipfilename);
                                        zipfiles.Add(newzipfilename);
                                    }
                                    else
                                    {
                                        throw new Exception("数据路径不存在！");
                                    }


                                }

                                //再次打包 打包相同行列号压缩包数据
                                //string[] allfiles = Directory.GetFiles(Constant.zipdata);


                                DataPacking.ZipFile(zipfiles.ToArray(), string.Format(@"{0}\{1}.gff", Constant.zipdata, taskid));
                                foreach (string files in zipfiles.ToArray())
                                {
                                    File.Delete(files);
                                }
                                zipfiles.Clear();
                                SendMessage(string.Format("{0}#orpsuccess#{1}.gff", taskid, taskid));
                                // SendMessage(string.Format("{0}#瓦片数据打包#结束#{1}#{2}", taskid, Constant.SystemName, ordercode));
                                // SendMessage(string.Format("{0}#数据入库#结束#{1}#{2}", taskid, Constant.SystemName, ordercode));
                            }
                            //else
                            //{
                            //    SendMessage(string.Format("{0}#数据入库#结束#{1}#{2}#{3}", taskid, Constant.SystemName, ordercode, strPath));

                            //}
                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        allcompleted = false;
                        throw ex;
                    }
                }

                if (allcompleted)
                {
                    //Log = "完成" + taskid + "全部数据入库！";
                    //SendMessage(string.Format("{0}#数据入库#结束#{1}#", taskid, Constant.SystemName));
                    TaskOrders.Remove(taskid);
                    TaskQrstCodes.Remove(taskid);
                    break;
                }
                else
                {
                    //没有任务 休息3秒
                    System.Threading.Thread.Sleep(3000);
                }
            }
        }
        public void IsDITaskComplished_forApp(object objtaskID)
        {
            string taskid = objtaskID.ToString();

            bool[] orderiscompleted = new bool[TaskOrders[taskid].Count];
            for (int i = 0; i < orderiscompleted.Length; i++)
            {
                orderiscompleted[i] = false;
            }

            while (true)
            {
                bool allcompleted = true;

                for (int i = 0; i < orderiscompleted.Length; i++)
                {
                    if (orderiscompleted[i])
                    {
                        continue;
                    }
                    try
                    {
                        string ordercode = TaskOrders[taskid][i];
                        string orderstatus = ssc.GetOrderStatus(ordercode);
                        if (orderstatus != "Suspended" && orderstatus != "Completed")
                        {
                            allcompleted = false;
                        }
                        else
                        {
                            orderiscompleted[i] = true;
                            Log = "完成数据入库" + taskid + "订单号：" + ordercode;
                            string strPath = "";
                            string workspace = ssc.GetOrderWorkspace(ordercode);
                            //foreach (var item in OrderWorkspace)


                            //{
                            //    if(item.Key==ordercode)
                            //        strPath = tilesPath(item.Value);


                            //}
                            strPath = tilesPath(workspace);

                            if (strPath == "")
                                strPath = "error";
                            SendMessage(string.Format("{0}#数据入库#结束#{1}#{2}#{3}", taskid, Constant.SystemName, ordercode, strPath));
                        }
                    }
                    catch
                    {
                        allcompleted = false;

                    }
                }
                if (allcompleted)
                {
                    //Log = "完成" + taskid + "全部数据入库！";
                    //SendMessage(string.Format("{0}#数据入库#结束#{1}#", taskid, Constant.SystemName));
                    TaskOrders.Remove(taskid);
                    TaskQrstCodes.Remove(taskid);
                    break;
                }
                else
                {
                    //没有任务 休息3秒
                    System.Threading.Thread.Sleep(3000);
                }
            }
        }

        /// <summary>
        /// zsm 20161025 查询判断是否为三级产品
        /// </summary>
        /// <param name="objtaskid">taskid</param>
        /// <returns></returns>
        private bool GetProdType(string objtaskid)
        {
            QDB_DBOperation_Dll.DBOperation dbo = new QDB_DBOperation_Dll.DBOperation();
            DataSet dataname = dbo.getthreeproductname(objtaskid);
            if (dataname != null && dataname.Tables[0] != null && dataname.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dn in dataname.Tables[0].Rows)
                {
                    try
                    {
                        string sjproductname = dn["Algs"].ToString();
                        if (sjproductname.Contains("ORP"))//ORGT
                        {
                            Log = "该objtaskid下的产品是三级产品" + objtaskid + "三级产品：" + sjproductname;
                            return true;
                        }
                        else
                        {
                            return false;

                        }
                    }
                    catch (Exception ex)
                    {

                        throw ex;
                    }
                }

            }
            else
            {
                return false;
            }
            throw new NotImplementedException();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // this.Hide();
            this.ShowInTaskbar = false;
            notifyIcon1.Visible = true;
            this.Visible = true;
        }

        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            if (this.WindowState == FormWindowState.Normal)
            {

                this.ShowInTaskbar = false;
                this.WindowState = FormWindowState.Minimized;
            }
            else
            {
                //this.Visible = true;
                this.ShowInTaskbar = true;
                this.WindowState = FormWindowState.Normal;
            }
        }

        private string tilesPath(string tilesPath)
        {
            string tilePath = String.Empty;
            DirectoryInfo di_data = new DirectoryInfo(tilesPath);
            //List<string> tilefileNames = new List<string>();
            DirectlyAddressing da = new DirectlyAddressing(DirectlyAddressingIPMod.IPModDataSet);
            foreach (FileInfo fi in di_data.GetFiles("*.*", SearchOption.AllDirectories))
            {
                if (fi.Name.ToUpper().EndsWith(".TIF") || fi.Name.ToUpper().EndsWith(".JPG") || fi.Name.ToUpper().EndsWith(".PNG"))
                {
                    string ip = "-1";
                    string desPath = da.GetPathByFileName(fi.Name, out ip);
                    if (ip != "-1" && File.Exists(desPath))
                    {
                        //tilefileNames.Add(desPath);
                        tilePath += desPath + ",";
                    }
                    else
                    {
                        string failedpath = string.Format(@"{0}{1}", GetFailedTilePath(), fi.Name);
                        if (File.Exists(failedpath))
                            tilePath += failedpath + ",";
                        //tilefileNames.Add(failedpath);
                    }
                }
            }
            if (tilePath.Length > 0)
                tilePath = tilePath.TrimEnd(',');
            return tilePath;
        }

        /// <summary>
        /// 获取入库失败切片的存放路径，IP为数据库中标为ISCENTER的站点
        /// </summary>
        /// <returns></returns>
        private string GetFailedTilePath()
        {
            string CenterIP = TServerSiteManager.GetCenterSiteIP();
            string pattern = @"^(0|[1-9][0-9]?|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.(0|[1-9][0-9]?|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.(0|[1-9][0-9]?|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.(0|[1-9][0-9]?|1[0-9]{2}|2[0-4][0-9]|25[0-5])$";

            if (System.Text.RegularExpressions.Regex.IsMatch(CenterIP, pattern))
            {
                return string.Format(@"\\{0}\{1}\{2}\", CenterIP, StorageBasePath.QRST_DB_Tile, StorageBasePath.FailedTile);
            }
            else
            {
                return "";
            }
        }

        public void AddTextBoxMessege(string messsage)
        {
            messsage = DateTime.Now.ToString() + " " + messsage + "\n";
            if (richTextBox1.InvokeRequired)
            {

                AddMessageDelegate addMessageDelegate = AddTextBoxMessege;
                richTextBox1.Invoke(addMessageDelegate, messsage);
            }
            else
            {
                richTextBox1.AppendText(messsage);
            }
        }
    }
}
