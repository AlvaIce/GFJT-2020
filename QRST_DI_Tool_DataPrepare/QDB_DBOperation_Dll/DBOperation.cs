using System;
using System.Collections.Generic;
using QDB_DBOperation_Dll.Common;
using System.Data;
using System.Xml;

namespace QDB_DBOperation_Dll
{
    /// <summary>
    /// 数据库操作
    /// </summary>
    public class DBOperation
    {
        #region 属性
        /// <summary>
        /// 连接对象
        /// </summary>
        MySqlBaseUtilities mysqlUtil;
        #endregion

        #region 用户登入
        //*参数：用户名、密码*//
        //*判断用户是否存在、密码是否正确、更改用户最后一次登录时间*//

        /// <summary>
        /// 添加新用户
        /// 如果当前用户不存在，则需要先注册用户，并在表hb_userinfo中增加该用户-密码加密
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="userPass">密码</param>
        public int AddUser(string userName, string userPass)
        {
            int rows = 0;
            mysqlUtil = new MySqlBaseUtilities();

            int userID = Convert.ToInt32(mysqlUtil.RunSQLStringScalar("SELECT  MAX(ID)+1 FROM  hb_userinfo"));
            string encryptPass = PassEncryptAndDecrypt.Encrypt(userPass);
            string sqlStr = string.Format("insert into hb_userinfo(ID,NAME,PASSWORD,LASTDATETIME) values({0},'{1}','{2}','{3}')", userID, userName, encryptPass, DateTime.Now);
            rows = mysqlUtil.ExecuteSql(sqlStr);
            return rows;
        }
        /// <summary>
        /// 用户登入--更新用户最后一次登录时间
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="userPass">密码</param>
        /// <param name="userState">用户在线or离线</param>
        /// <returns>返回是否更新成功</returns>
        public bool UpdateUserLogTimeAndState(string userName, string userPass, string userState)
        {
            bool UpdateSuccess = false;
            mysqlUtil = new MySqlBaseUtilities();
            //首先判断数据库中用户名是否存在及密码是否正确，然后再执行更改
            string encryptPass = PassEncryptAndDecrypt.Encrypt(userPass);
            string sqlStr = string.Format("select Count(*) from hb_userinfo where NAME='{0}' and PASSWORD='{1}'", userName, encryptPass);
            int userRow = Convert.ToInt32(mysqlUtil.RunSQLStringScalar(sqlStr));
            if (userRow != 0 && userRow != -1)
            {
                string sqlUpdate = string.Format("update hb_userinfo set LASTDATETIME='{0}',IsOnline='{1}' where NAME='{2}' and PASSWORD='{3}'", DateTime.Now, userState, userName, encryptPass);
                int resultrow = mysqlUtil.ExecuteSql(sqlUpdate);
                if (resultrow != 0 && resultrow != -1)
                {
                    UpdateSuccess = true;
                    CreateLookPanelXml();
                }
                else
                {
                    UpdateSuccess = false;
                }
            }
            return UpdateSuccess;
        }
        #endregion

        #region 提交任务--xy
        //*参数：任务类型、任务过程、任务总负责人*//
        //*处理：根据当前日期生成任务单号、任务表task_expand入库、订单扩展表orders_expand入库*//

        /// <summary>
        /// 任务单入库
        /// </summary>
        /// <param name="taskType">任务类型</param>
        /// <param name="taskProcess">任务过程：数据准备#预处理#大气校正等，用#隔开</param>
        /// <param name="leaderName">任务总负责人</param>
        /// <param name="dataPath">数据路径：数据1路径#数据2路径#。。。</param>
        /// <param name="dataPath">数据类型：一个任务一种数据类型</param>
        /// <returns>入库是否成功</returns>
        public bool AddTask(string taskType, string taskProcess, string leaderName, string dataPath, string dataType)
        {
            string[] processArr = taskProcess.Split('#');
            string operatorStr = string.Empty;
            for (int i = 0; i < processArr.Length; i++)
            {
                operatorStr += "<" + processArr[i] + ":>";//<过程1：人1，人2><过程2：>
            }

            bool IsSuccess = false;
            mysqlUtil = new MySqlBaseUtilities();
            string taskID = GetNewCode();
            string sqluserID = string.Format("SELECT ID FROM hb_userinfo WHERE NAME = '{0}'", leaderName);
            DataSet ds = mysqlUtil.GetDataSet(sqluserID);
            string leaderID = ds.Tables[0].Rows[0][0].ToString();
            string sqlInser = string.Format("insert into task_expand(TaskID,TaskType,TaskProcess,SubmitTime,LeaderID,IsCompleted,DataPath,DataType,Operator) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')", taskID, taskType, taskProcess, DateTime.Now, leaderID, -1, dataPath, dataType, operatorStr);
            int resultrow = mysqlUtil.ExecuteSql(sqlInser);
            if (resultrow != 0 && resultrow != -1)
            {
                IsSuccess = true;
            }
            else
            {
                IsSuccess = false;
            }
            return IsSuccess;
        }

        /// <summary>
        /// 生成任务单号
        /// </summary>
        /// <returns>任务单号</returns>
        protected static string GetNewCode()
        {
            string type = "T";
            string date = ((DateTime.Now.Year - 1900) * 365 + DateTime.Now.DayOfYear).ToString("00000");
            string time = ((int)DateTime.Now.TimeOfDay.TotalSeconds).ToString("00000");
            string msecond = DateTime.Now.Millisecond.ToString("000");
            System.Threading.Thread.Sleep(1);
            //返回任务单号
            return string.Format("{0}{1}{2}{3}", type, date, time, msecond);
        }
        #endregion

        #region 数据准备 --给何涛的方法
        /// <summary>
        /// 获取任务单号和数据路径- 每次获取一条 --step1
        /// </summary>
        /// <returns>返回任务单号--数据路径DataSet</returns>
        public DataSet GetTaskIDAndDataPath()
        {
            mysqlUtil = new MySqlBaseUtilities();
            string sqlStr = "select TaskID,DataPath from task_expand where IsCompleted=-1 limit 1";
            DataSet ds = mysqlUtil.GetDataSet(sqlStr);
            return ds;
        }
        /// <summary>
        /// 获取任务中待处理数据编号
        /// </summary>
        /// <returns>数据编号DataSet</returns>
        public DataSet GetQRSTCODEs(string taskID)
        {
            mysqlUtil = new MySqlBaseUtilities();
            string sqlStr = string.Format("select QRST_CODE from orders_expand where TaskID= '{0}'", taskID);
            DataSet ds = mysqlUtil.GetDataSet(sqlStr);
            return ds;
        }
        /// <summary>
        /// zsm 20161021
        /// </summary>
        /// <param name="taskID"></param>
        /// <returns></returns>
        public DataSet getdataname(string taskID)//该方法查询的是qrst_db库里的orders_expand
        {
            mysqlUtil = new MySqlBaseUtilities();
            string sqlStr = string.Format("select dataname from orders_expand where TaskID= '{0}'", taskID);
            DataSet ds = mysqlUtil.GetDataSet(sqlStr);
            return ds;
        }

        //public DataSet getdataname(string taskID)
        //{
        //   // mysqlUtil = new MySqlBaseUtilities();
        //    string sqlStr = string.Format("select dataname from orders_expand where TaskID= '{0}'", taskID);
        //    mysqlUtil = new MySqlBaseUtilities(Constant.ConnectionStringEVDB);
        //    DataSet ds = mysqlUtil.GetDataSet(sqlStr);
        //    return ds;
        //}

        /// <summary>
        /// zsm 20161025 根据ID得到三级产品名称
        /// </summary>
        /// <param name="taskID"></param>
        /// <returns></returns>
        public DataSet getthreeproductname(string taskID)
        {
            mysqlUtil = new MySqlBaseUtilities();
            string sqlStr = string.Format("select Algs from task_expand where TaskID= '{0}'", taskID);
            DataSet ds = mysqlUtil.GetDataSet(sqlStr);
            return ds;
        }




        /// <summary>
        /// zsm 20161021
        /// </summary>
        /// <param name="dataname"></param>
        /// <returns></returns>
        public DataSet getonedataname(string qrstcode)
        {
            //mysqlUtil = new MySqlBaseUtilities();            
            string sqlStr = string.Format("select Name from prod_gf1 where QRST_CODE= '{0}'", qrstcode);
            mysqlUtil = new MySqlBaseUtilities(Constant.ConnectionStringEVDB);
            DataSet ds = mysqlUtil.GetDataSet(sqlStr);
            return ds;
        }

        /// <summary>
        /// zsm 20161021
        /// </summary>
        /// <param name="dataname"></param>
        /// <returns></returns>
        public DataSet getcode(string dataname)
        {
            //mysqlUtil = new MySqlBaseUtilities();            
            string sqlStr = string.Format("select QRST_CODE from prod_gf1 where Name= '{0}'", dataname);
            mysqlUtil = new MySqlBaseUtilities(Constant.ConnectionStringEVDB);
            DataSet ds = mysqlUtil.GetDataSet(sqlStr);
            return ds;
        }


        /// <summary>
        /// 获取任务单号和数据路径- 每次获取一条 --step1
        /// </summary>
        /// <returns>返回任务单号--数据路径DataSet</returns>
        public DataSet GetTaskIDAndQRSTCODE()
        {
            mysqlUtil = new MySqlBaseUtilities();
            string sqlStr = "select TaskID,QRST_CODE from task_expand where IsCompleted=-1 limit 1";
            DataSet ds = mysqlUtil.GetDataSet(sqlStr);
            return ds;
        }
        /// <summary>
        /// 数据准备生成订单号-插入订单扩展表
        /// 并 更新订单状态为数据准备1-----step2
        /// </summary>
        /// <param name="taskID">任务单号</param>
        /// <param name="orderCode">订单号数组</param>
        public void InsertAndUpdateOrders(string taskID, string orderCode, string currentProcess, string workspace, DateTime submitTime)
        {
            AddOrders(orderCode, workspace, submitTime);
            AddOrderExpand(taskID, orderCode);
            //UpdateOrder_CurrState("1", orderCode, "1");//1数据准备 0 等待领取
            UpdateOrder_NextState(orderCode, currentProcess);
            CreateLookPanelXml();//订单状态改变 生成xml
        }


        /// <summary>
        /// 数据准备之后根据任务单号更改任务单状态为0 未完成 step3
        /// </summary>
        /// <param name="taskID">任务单号</param>
        public bool UpdateTaskIsCompleted(string taskID)
        {
            bool isSuccess = false;
            int rowAffect = 0;
            string sqlStr = string.Format("update task_expand set IsCompleted='{0}' where TaskID= '{1}'", 0, taskID);
            mysqlUtil = new MySqlBaseUtilities();
            rowAffect = mysqlUtil.ExecuteSql(sqlStr);
            if (rowAffect != -1 && rowAffect != 0)
            {
                isSuccess = true;
                CreateLookPanelXml();
            }
            else
            {
                isSuccess = false;
            }
            return isSuccess;
        }

        /// <summary>
        /// 数据准备生成订单号，入库orders表
        /// </summary>
        /// <param name="ordercode">订单号</param>
        /// <param name="workspace">工作空间</param>
        /// <param name="submitTime">订单提交时间</param>
        private bool AddOrders(string ordercode, string workspace, DateTime submitTime)
        {
            bool isSuccess = false;
            int rowAffect = 0;
            mysqlUtil = new MySqlBaseUtilities();
            string sqlID = "select Max(id)+1 from orders";
            int id = Convert.ToInt32(mysqlUtil.RunSQLStringScalar(sqlID));
            string sqlAdd = string.Format("insert into orders(id,OrderCode,SubmitTime,WorkSpace) values('{0}','{1}','{2}','{3}')", id, ordercode, submitTime, workspace);
            rowAffect = mysqlUtil.ExecuteSql(sqlAdd);
            if (rowAffect != -1 && rowAffect != 0)
            {
                isSuccess = true;
            }
            else
            {
                isSuccess = false;
            }
            return isSuccess;
        }

        /// <summary>
        /// 用户领取任务之后，新增记录订单扩展表orders_expand
        /// </summary>
        /// <param name="taskID">任务单号</param>
        /// <param name="orderCode">订单号数组</param>
        /// <returns></returns>
        private bool AddOrderExpand(string taskID, string orderCode)
        {
            bool isSuccess = false;
            int rowsAffect = 0;
            mysqlUtil = new MySqlBaseUtilities();
            string sqlStr = string.Format("insert into orders_expand (OrderCode,TaskID) values('{0}','{1}')", orderCode, taskID);
            rowsAffect = mysqlUtil.ExecuteSql(sqlStr);
            if (rowsAffect != -1 && rowsAffect != 0)
            {
                isSuccess = true;
            }
            else
            {
                isSuccess = false;
            }
            return isSuccess;
        }
        #endregion

        #region 领取任务
        //参数:用户名；
        //处理：根据用户名判断是否已有该用户、用户的过程名、订单号

        /// <summary>
        /// 根据用户当前过程，更新任务表中的过程参与者
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="userProcess">当前用户处理的过程名称</param>
        /// <param name="orderCode">用户处理的订单号</param>
        /// <returns></returns>
        public bool UpdateTaskOperator(string userName, string userProcess, string orderCode)
        {
            bool isSuccess = false;
            string taskOperator = string.Empty;
            mysqlUtil = new MySqlBaseUtilities();
            int rowAffect = 0;

            //2.判断过程参与者中是否已有该用户，如果没有则更新任务表中的Operator字段值
            DataSet processDS = GetTaskByOrderCode(orderCode);//获取任务过程 任务单号的DataSet
            if (processDS != null && processDS.Tables.Count != 0)
            {
                string taskProcess = processDS.Tables[0].Rows[0]["TaskProcess"].ToString();//任务过程
                string taskID = processDS.Tables[0].Rows[0]["TaskID"].ToString();//任务ID
                taskOperator = processDS.Tables[0].Rows[0]["Operator"].ToString();//过程参与者

                string[] processArr = taskProcess.Split('#');
                int currProcessID = GetUserProcessID(processArr, userProcess);//用户当前过程的ID号

                DataTable proAndOperatorDT = GetOperatorProcess(taskOperator);//过程参与者字段
                string sql = string.Format("select Count(OperatorName) from {0} where ProcessName='{1}' and OperatorName='{2}'", proAndOperatorDT.TableName, userProcess, userName);
                //int personRow = mysqlUtil.ExecuteSql(sql);

                DataRow[] personRow = proAndOperatorDT.Select(string.Format("OperatorName='{0}'", userName));

                if (personRow.Length == 0)
                {
                    DataRow dr = proAndOperatorDT.NewRow();
                    dr["ProcessID"] = currProcessID;
                    dr["ProcessName"] = userProcess;
                    dr["OperatorName"] = userName;
                    proAndOperatorDT.Rows.Add(dr);
                }
                List<string> listTeeeeee = new List<string>();
                foreach (string item in processArr)
                {
                    string strProcess = item;
                    string strPersonName = string.Empty;
                    DataRow[] dataRowItem = proAndOperatorDT.Select(string.Format("ProcessName='{0}'", strProcess));
                    for (int i = 0; i < dataRowItem.Length; i++)
                    {
                        if (i == 0)
                        {
                            strPersonName = dataRowItem[i]["OperatorName"].ToString();
                        }
                        else
                            strPersonName += string.Format(",{0}", dataRowItem[i]["OperatorName"]);

                    }
                    listTeeeeee.Add(string.Format("<{0}:{1}>", strProcess, strPersonName));
                }
                string rukuProceess = string.Empty;
                foreach (string item in listTeeeeee)
                {
                    rukuProceess += item;
                }

                ////
                string sqltask = string.Format("update task_expand set Operator='{0}' where TaskID='{1}'", rukuProceess, taskID);
                rowAffect = mysqlUtil.ExecuteSql(sqltask);
                if (rowAffect != -1 && rowAffect != 0)
                {
                    isSuccess = true;
                    CreateLookPanelXml();
                }
                else
                {
                    isSuccess = false;
                }
            }
            return isSuccess;
        }

        /// <summary>
        /// 解析【过程参与者Operator字段值】,生成DataTable
        /// </summary>
        /// <param name="operatorProcess">task_expand中过程参与者Operator字段 </param> 
        /// <returns></returns>
        private DataTable GetOperatorProcess(string operatorProcess)
        {
            DataTable ProcessOpeDataTable = new DataTable();
            ProcessOpeDataTable.Columns.Add("ProcessID", typeof(System.Data.SqlTypes.SqlInt32));
            ProcessOpeDataTable.Columns.Add("ProcessName", typeof(System.Data.SqlTypes.SqlString));
            ProcessOpeDataTable.Columns.Add("OperatorName", typeof(System.Data.SqlTypes.SqlString));

            char[] splitchar = { '<', '>' };
            string[] theProcess = operatorProcess.Split(splitchar, StringSplitOptions.RemoveEmptyEntries);//<数据准备：人1,人2,人3> 去除空格
            string[] proAndOperator = null;
            string[] operators = null;

            for (int i = 0; i < theProcess.Length; i++)
            {

                int id = i + 1;
                proAndOperator = theProcess[i].Split(':');//[过程1][人1,人2,人3]
                operators = proAndOperator[1].Split(',');//[人1][人2][人3]
                string processName = proAndOperator[0].ToString();
                for (int j = 0; j < operators.Length; j++)
                {
                    //DataRow dr=new DataRow();
                    //dr["ProcessID"] = id;
                    //dr["ProcessName"] = processName;
                    //dr["OperatorName"] = operators[j];
                    ProcessOpeDataTable.Rows.Add(id, processName, operators[j]);
                }
            }
            return ProcessOpeDataTable;
        }

        /// <summary>
        /// 获取用户当前过程的序号
        /// </summary>
        /// <param name="processArr">任务过程数组</param>
        /// <param name="processName">当前过程</param>
        /// <returns>当前过程序号</returns>
        private int GetUserProcessID(string[] processArr, string processName)
        {
            Dictionary<string, int> dir = new Dictionary<string, int>();
            for (int i = 0; i < processArr.Length; i++)
            {
                dir.Add(processArr[i], i + 1);
            }
            int processID = dir[processName];
            return processID;
        }

        /// <summary>
        /// 根据过程ID和订单号,获取orders_expand中CurrentProcess对应的过程名称
        /// </summary>
        /// <param name="processArr">任务过程数组</param>
        /// <param name="processName">当前过程ID</param>
        /// <returns>过程名</returns>
        private string GetUserProcessName(string ordercode, int processID)
        {
            string processName = string.Empty;
            DataSet ds = GetTaskByOrderCode(ordercode);
            if (ds != null && ds.Tables.Count != 0)
            {
                string process = ds.Tables[0].Rows[0]["TaskProcess"].ToString();
                string[] processArr = process.Split('#');
                Dictionary<int, string> dir = new Dictionary<int, string>();
                for (int i = 0; i < processArr.Length; i++)
                {
                    dir.Add(i + 1, processArr[i]);
                }
                processName = dir[processID];
            }

            return processName;
        }

        /// <summary>
        /// 根据订单号获取任务ID和任务过程
        /// </summary>
        /// <param name="orderCode">订单号</param>
        /// <returns>任务ID 任务过程TaskProcess字段 过程参与者Operator字段 任务过程的DataSet</returns>
        private DataSet GetTaskByOrderCode(string orderCode)
        {
            mysqlUtil = new MySqlBaseUtilities();
            string sqlProcess = string.Format("SELECT orders_expand.TaskID,task_expand.TaskProcess,task_expand.Operator FROM      orders_expand INNER JOIN task_expand ON orders_expand.TaskID = task_expand.TaskID WHERE   (orders_expand.OrderCode = '{0}')", orderCode);
            DataSet taskIDAndProcessDS = mysqlUtil.GetDataSet(sqlProcess);
            return taskIDAndProcessDS;
        }

        #endregion

        #region 任务完成
        /// <summary>
        /// 根据任务单号更新任务状态--完成 未完成
        /// </summary>
        /// <param name="taskID">任务单号</param>
        /// <param name="taskState">任务状态</param>
        /// <returns>返回影响行数</returns>
        public int UpdateTaskState(string taskID, string taskState)
        {
            int isCompleted = 0;
            if (taskState != string.Empty && taskState == "未完成")
            {
                isCompleted = 0;
            }
            if (taskState != string.Empty && taskState == "完成")
            {
                isCompleted = 1;
            }
            mysqlUtil = new MySqlBaseUtilities();
            string sqlStr = string.Format("update task_expand set IsCompleted ='{0}' where TaskID='{1}'", isCompleted, taskID);
            return mysqlUtil.ExecuteSql(sqlStr);
        }
        #endregion

        #region to预处理
        /// <summary>
        /// 获取视图task_orderexpand_view信息--预处理任务树DataSet///再加参数当前过程名  暂未加
        /// </summary>
        /// <returns>预处理任务树DataSet：任务单号-任务过程-订单号-工作空间-订单当前过程</returns>
        public DataSet GetTaskOrderDS()
        {
            DataSet taskorderDS = new DataSet();
            string sql = "select * from task_tree_view";
            mysqlUtil = new MySqlBaseUtilities();
            taskorderDS = mysqlUtil.GetDataSet(sql);
            return taskorderDS;
        }

        //参数：订单号、过程名；
        //处理：1.更新task_expand中的状态
        //     2.更新订单表orders_expand中的当前过程和状态

        /// <summary>
        /// 预处理开始处理订单----更新订单表orders_expand中的当前过程和状态 --状态0未领取-1正在处理
        /// </summary>
        /// <param name="currentProcess">订单当前过程名称</param>
        /// <param name="orderCode">订单号</param>
        /// <param name="state">订单状态</param>
        public bool UpdateOrder_CurrState(string currentProcess, string orderCode, string state)
        {
            int rowAffect = 0;
            bool isSuccess = false;
            DataSet processDS = GetTaskByOrderCode(orderCode);
            if (processDS != null && processDS.Tables.Count != 0)
            {
                string taskID = processDS.Tables[0].Rows[0]["TaskID"].ToString();
                string taskProces = processDS.Tables[0].Rows[0]["TaskProcess"].ToString();
                string[] processArr = taskProces.Split('#');
                int currProceeID = GetUserProcessID(processArr, currentProcess);
                string sqlUpdateOrder = string.Format("update orders_expand set CurrentProcess='{0}',State='{1}' where OrderCode='{2}'", currProceeID, state, orderCode);
                rowAffect = mysqlUtil.ExecuteSql(sqlUpdateOrder);
                if (rowAffect != -1 && rowAffect != 0)
                {
                    isSuccess = true;
                    CreateLookPanelXml();
                }
                else
                {
                    isSuccess = false;
                }
            }
            return isSuccess;
        }

        /// <summary>
        /// 预处理--预处理过程完成之后，更新订单扩展表的过程为下一个过程号，状态更新为为0等待领取状态
        /// </summary>
        /// <param name="orderCode">订单号</param>
        /// <param name="currentProcess">当前过程名称</param>
        public bool UpdateOrder_NextState(string orderCode, string currentProcess)
        {
            bool isSuccess = false;
            int rowAffect = 0;
            DataSet processDS = GetTaskByOrderCode(orderCode);
            if (processDS != null && processDS.Tables.Count != 0)
            {
                string taskID = processDS.Tables[0].Rows[0]["TaskID"].ToString();
                string taskProces = processDS.Tables[0].Rows[0]["TaskProcess"].ToString();
                string[] processArr = taskProces.Split('#');
                int currProceeID = GetUserProcessID(processArr, currentProcess);
                string sqlUpdateOrder = string.Format("update orders_expand set CurrentProcess='{0}',State='{1}' where OrderCode='{2}'", currProceeID + 1, 0, orderCode);//int--string
                rowAffect = mysqlUtil.ExecuteSql(sqlUpdateOrder);
                if (rowAffect != -1 && rowAffect != 0)
                {
                    isSuccess = true;
                    CreateLookPanelXml();
                }
                else
                { isSuccess = false; }
            }
            return isSuccess;
        }
        #endregion

        #region 看板xml
        /// <summary>
        /// 创建看板xml
        /// </summary>
        /// <returns></returns>
        private void CreateLookPanelXml()
        {
            mysqlUtil = new MySqlBaseUtilities();
            string sql = "select * from task_lookpanel_view where 任务是否完成=0";
            DataSet lookpanelDS = mysqlUtil.GetDataSet(sql);
            if (lookpanelDS != null && lookpanelDS.Tables.Count != 0)
            {
                string xmlSavePath = GetXmlPath();
                GetXmlFromDataSet(xmlSavePath, lookpanelDS);
            }
        }

        /// <summary>
        /// 从数据库读取共享文件夹路径，拼接xml文件路径
        /// </summary>
        private string GetXmlPath()
        {
            string xmlPath = string.Empty;
            mysqlUtil = new MySqlBaseUtilities();
            string sqlStr = "select CommonSharePath from db02_sitesinfo limit 1";
            DataSet sharePathDS = mysqlUtil.GetDataSet(sqlStr);
            if (sharePathDS != null && sharePathDS.Tables.Count != 0)
            {
                string commonsharePath = sharePathDS.Tables[0].Rows[0][0].ToString();
                xmlPath = commonsharePath + @"\QRST_DB_Store\taskxml\" + "t" + DateTime.Now.ToLongTimeString() + ".xml";
            }
            return xmlPath;
        }

        /// DataSet转化为xml文件
        /// </summary>
        /// <param name="strPath">xml文件路径，such as \\192.168.10.19\QRST_DB_Store\taskxml\t+日期命名的xml</param>
        /// <param name="ds">DataSet</param>
        /// <returns></returns>
        private XmlDocument GetXmlFromDataSet(string strPath, DataSet ds)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(strPath);
            if (ds != null && ds.Tables.Count >= 1)
            {
                DataTable dt = ds.Tables[0];
                DataRowCollection dataRow = dt.Rows;

                DataColumnCollection dcc = dt.Columns;
                List<string> listStr = new List<string>();
                foreach (DataColumn dolumn in dcc)
                {
                    listStr.Add(dolumn.ColumnName);
                }
                XmlElement rootElement = xmlDoc.CreateElement("task");
                xmlDoc.AppendChild(rootElement);

                foreach (DataRow item in dataRow)
                {
                    XmlElement rowElement = xmlDoc.CreateElement("row");
                    rootElement.AppendChild(rowElement);
                    foreach (string nodeItem in listStr)
                    {
                        XmlElement itemElement = xmlDoc.CreateElement(nodeItem.Trim());
                        itemElement.InnerText = item.Field<string>(nodeItem);
                        rowElement.AppendChild(itemElement);
                    }
                }
                xmlDoc.Save(strPath);
            }
            return xmlDoc;
        }
        #endregion
    }
}
