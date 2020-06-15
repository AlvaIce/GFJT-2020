using System;
using System.Collections.Generic;
using System.Data;
using QRST_DI_SS_Basis.MetaData;
using QRST_DI_SS_DBInterfaces.IDBEngine;
using QRST_DI_TS_Process.Orders;
using QRST_DI_TS_Process.Tasks.InstalledTasks;
using QRST_DI_Resources;

namespace QRST_DI_TS_Process.Tasks
{
    [Serializable]
    public abstract class TaskClass
    {
        private static IDbOperating slLiteOperating = Constant.IdbOperating;
        private static IDbBaseUtilities sqLiteBaseUtilities;
        #region static
        /// <summary>
        /// 获取内置任务实例
        /// </summary>
        /// <param name="taskname"></param>
        /// <returns></returns>
        public static TaskClass CreateNewInstalledTask(string taskname)
        {
            //新内置任务需提前注册
            TaskClass task = null;
            switch (taskname)
            {
                case "ITInsertDocMetaData":
                    task = new ITInsertDocMetaData();
                    break;
                case "ITImportDocFile":
                    task = new ITImportDocFile();
                    break;
                case "ITDownLoadDOC":
                    task = new ITDownLoadDOC();
                    break;
                case "ITDownLoadDataP2P":
                    task = new ITDownLoadDataP2P();
                    break;
                case "ITUnzipGFtargz4XmlJpg":
                    task = new ITUnzipGFtargz4XmlJpg();
                    break;
                case "ITUnzipHJtargz":
                    task = new ITUnzipHJtargz();
                    break;
                case "ITStoreTiles":
                    task = new ITStoreTiles();
                    break;
                case "ITStoreHJData":
                    task = new ITStoreHJData();
                    break;
                case "ITSendMessage":
                    task = new ITSendMessage();
                    break;
                case "ITInsertHJMetaData":
                    task = new ITInsertHJMetaData();
                    break;
                case "ITCreateHJDataInputWSP":
                    task = new ITCreateHJDataInputWSP();
                    break;
                case "ITOutputLoginfo":
                    task = new ITOutputLoginfo();
                    break;
                case "ITCopyFiles":
                    task = new ITCopyFiles();
                    break;
                case "ITCreateNoProcessWorkspace":
                    task = new ITCreateNoProcessWorkspace();
                    break;
                case "ITInsertCbersMetaData":
                    task = new ITInsertCbersMetaData();
                    break;
                case "ITInsertModisMetaData":
                    task = new ITInsertModisMetaData();
                    break;
                case "ITInsertNOAAMetaData":
                    task = new ITInsertNOAAMetaData();
                    break;
                case "ITInsertRasterMetaData":
                    task = new ITInsertRasterMetaData();
                    break;
                case "ITCreateThumbnail":
                    task = new ITCreateThumbnail();
                    break;
                case "ITCreateNOAABitMap":
                    task = new ITCreateNOAABitMap();
                    break;
                case "ITInsertVectorMeta":
                    task = new ITInsertVectorMeta();
                    break;
                case "ITImportVectorFiles":
                    task = new ITImportVectorFiles();
                    break;
                case "ITImportRasterFiles":
                    task = new ITImportRasterFiles();
                    break;
                case "ITCreateHJImage":
                    task = new ITCreateHJImage();
                    break;
                case "ITCreateCbersImage":
                    task = new ITCreateCbersImage();
                    break;
                case "ITInsertNormalHJMetaData":
                    task = new ITInsertNormalHJMetaData();
                    break;
                case "ITInsertSortedVectorMeta":
                    task = new ITInsertSortedVectorMeta();
                    break;
                case "ITInsertDEMMetaData":
                    task = new ITInsertDEMMetaData();
                    break;
                case "ITGetMetaDataFile":
                    task = new ITGetMetaDataFile();
                    break;
                case "ITImportUserData":
                    task = new ITImportUserData();
                    break;
                case "ITDownLoadData":
                    task = new ITDownLoadData();
                    break;
                case "ITUnzipTarFile":
                    task = new ITUnzipTarFile();
                    break;
                case "ITImportStandardCmp":
                    task = new ITImportStandardCmp();
                    break;
                case "ITImportStandWfl":
                    task = new ITImportStandWfl();
                    break;
                case "ITImportUserProduct":
                    task = new ITImportUserProduct();
                    break;
                case "ITInsertTMMetaData":
                    task = new ITInsertTMMetaData();
                    break;
                case "ITInsertTZJCoastlandAlosMetaData":
                    task = new ITInsertTZJCoastlandAlosMetaData();
                    break;
                case "ITInsertSZWorldView2MetaData":
                    task = new ITInsertSZWorldView2MetaData();
                    break;
                case "ITInsertHHAlosMetaData":
                    task = new ITInsertHHAlosMetaData();
                    break;
                case "ITInsertTJQBIRDMetaData":
                    task = new ITInsertTJQBIRDMetaData();
                    break;
                case "ITInsertTJ5MRSImageMetaData":
                    task = new ITInsertTJ5MRSImageMetaData();
                    break;
                case "ITRasterDataImportNew":
                    task = new ITRasterDataImportNew();
                    break;
                case "ITUnzipZipFile":
                    task = new ITUnzipZipFile();
                    break;
                case "ITImportZipCmp":
                    task = new ITImportZipCmp();
                    break;
                case "ITStoreTilesNew":
                    task = new ITStoreTilesNew();
                    break;
                case "ITStoreGFCorData":
                    task = new ITStoreGFCorData();
                    break;
                case "ITGFDataImport":
                    task = new ITGFDataImport();
                    break;
                case "ITUnzipZY3targzXmlJpg":
                    task = new ITUnzipZY3targzXmlJpg();
                    break;
                case "ITZY3DataImport":
                    task = new ITZY3DataImport();
                    break;
                case "ITHJDataImport":
                    task = new ITHJDataImport();
                    break;
                case "ITUnzipHJtargz4XmlJpg":
                    task = new ITUnzipHJtargz4XmlJpg();
                    break;
                case "ITDataQualityInspection":
                    task = new ITDataQualityInspection();
                    break;
                case "ITPerareGF1Data":
                    task = new ITPerareGF1Data();
                    break;
                case "ITCreateDataInputWSP":
                    task = new ITCreateDataInputWSP();
                    break;
                case "ITZY02cDataImport":
                    task = new ITZY02cDataImport();
                    break;
                case "ITNormalDataImport":
                    task = new ITNormalDataImport();
                    break;
				case "ITDownLoadTiles":
					task = new ITDownLoadTiles();
					break;
				case "ITSJ9ADataImport":
					task = new ITSJ9ADataImport();
					break;
				case "ITHJ1CDataImport":
					task = new ITHJ1CDataImport();
					break;
                case "ITCombineTileIntoDir":
                    task = new ITCombineTileIntoDir();
                    break;
                case "ITFetchAndZipTileFiles":
                    task = new ITFetchAndZipTileFiles();
                    break;
                case "ITMoveZipToDestDir":
                    task = new ITMoveZipToDestDir();
                    break;
                case "ITGF3DataImport":
                    task = new ITGF3DataImport();
                    break;
                case "ITCreateBCDDataWorkspace":
                    task = new ITCreateBCDDataWorkspace();
                    break;
                case "ITUnzipTargz4XmlJpg":
                    task = new ITUnzipTargz4XmlJpg();
                    break;
                case "ITBCDDataImport":
                    task = new ITBCDDataImport();
                    break;
                case "ITBCDDataQualityInspection":
                    task = new ITBCDDataQualityInspection();
                    break;
                case "ITCreateGF5DataWorkspace":
                    task = new ITCreateGF5DataWorkspace();
                    break;
                case "ITGF5DataQualityInspection":
                    task = new ITGF5DataQualityInspection();
                    break;
                case "ITGF5DataImport":
                    task = new ITGF5DataImport();
                    break;
                case "ITCreateGF6DataWorkspace":
                    task = new ITCreateGF6DataWorkspace();
                    break;
                case "ITGF6DataQualityInspection":
                    task = new ITGF6DataQualityInspection();
                    break;
                case "ITGF6DataImport":
                    task = new ITGF6DataImport();
                    break;
                default:
                    break;
            }
            return task;
        }

        protected static string getQRSTCODEbyTaskName(string taskname)
        {
            string code = "";
            string sql = string.Format(@"Select qrst_code from taskdef where name='{0}'", taskname);
            sqLiteBaseUtilities = slLiteOperating.GetSubDbUtilities(EnumDBType.MIDB);
            DataSet ds = sqLiteBaseUtilities.GetDataSet(sql);

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    code = ds.Tables[0].Rows[0]["qrst_code"].ToString();
                }
            }
            return code;
        }

        public static TaskClass CreateTaskClassByName(string taskname)
        {
            TaskClass task = null;
            string sql = string.Format(@"Select * from taskdef where name='{0}'", taskname);
            sqLiteBaseUtilities = slLiteOperating.GetSubDbUtilities(EnumDBType.MIDB);
            DataSet ds = sqLiteBaseUtilities.GetDataSet(sql);

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    task = DBRow2TaskCls(ds.Tables[0].Rows[0]);
                }
            }

            return task;
        }

        /// <summary>
        /// 根据任务类型获取所有任务
        /// </summary>
        /// <param name="taskType"></param>
        /// <returns></returns>
        public static List<TaskClass> GetTasksByType(EnumTaskType taskType)
        {
            List<TaskClass> tasksLst = new List<TaskClass>();
            string sql = string.Format(@"Select * from taskdef where Type='{0}'", taskType.ToString());
            sqLiteBaseUtilities = slLiteOperating.GetSubDbUtilities(EnumDBType.MIDB);
            DataSet ds = sqLiteBaseUtilities.GetDataSet(sql);

            if (ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    tasksLst.Add(DBRow2TaskCls(ds.Tables[0].Rows[i]));
                }
            }
            return tasksLst;
        }



        public static TaskClass DBRow2TaskCls(DataRow dr)
        {
            TaskClass task = null;
            string taskname = dr["Name"].ToString();
            EnumTaskType tasktype = EnumTaskType.UnKnown;
            Enum.TryParse(dr["type"].ToString(), out tasktype);


            switch (tasktype)
            {
                case EnumTaskType.Remote:
                    task = new RemoteTaskClass();
                    task.TaskName = taskname;
                    task.TaskType = tasktype;
                    break;
                case EnumTaskType.Customized:
                    task = new CustomizedTaskClass();
                    task.TaskName = taskname;
                    task.TaskType = tasktype;
                    break;
                case EnumTaskType.Installed:
                    task = CreateNewInstalledTask(taskname);
                    task.TaskName = taskname;
                    task.TaskType = tasktype;
                    break;
                case EnumTaskType.UnKnown:
                default:
                    return task;
            }
            task.Description = dr["Description"].ToString();
            task.SuspendAvailable = (dr["Suspendable"].ToString() == "1") ? true : false;
            task.ProcessExec = dr["Processexec"].ToString();
            task.Code = dr["QRST_CODE"].ToString();
            task.paraMemo = dr["Params"].ToString();
            return task;
        }


        public static void AddNewTaskDef2DB(TaskClass newtask)
        {
            string newqrstcode = slLiteOperating.GetNewQrstCodeFromTableName("taskdef", EnumDBType.MIDB);
            int newid = slLiteOperating.GetNewID("taskdef", EnumDBType.MIDB);

            sqLiteBaseUtilities = slLiteOperating.GetSubDbUtilities(EnumDBType.MIDB);
            string deleteSql = string.Format(@"Delete from taskdef where name='{0}'", newtask.TaskName);
            sqLiteBaseUtilities.ExecuteSql(deleteSql);

            string insertSql = string.Format(@"insert into taskdef(ID,Name,QRST_CODE,type,Description,Suspendable,Processexec) values ({0},'{1}','{2}','{3}','{4}',{5},'{6}');",
                newid, newtask.TaskName, newqrstcode, Enum.GetName(typeof(EnumTaskType), newtask.TaskType), newtask.Description, (newtask.SuspendAvailable) ? 1 : 0, newtask.ProcessExec.Replace(@"\", @"\\"));
            sqLiteBaseUtilities.ExecuteSql(insertSql);

        }

        public static void UpdateTaskDef2DB(TaskClass task)
        {
            sqLiteBaseUtilities = slLiteOperating.GetSubDbUtilities(EnumDBType.MIDB);
            string sql = string.Format(@"update taskdef Set Name='{0}',type='{1}',Description='{2}',Suspendable={3},Processexec='{4}' where QRST_CODE='{5}';",
                task.TaskName, Enum.GetName(typeof(EnumTaskType), task.TaskType), task.Description, (task.SuspendAvailable) ? 1 : 0, task.ProcessExec.Replace(@"\", @"\\"), task.Code);
            sqLiteBaseUtilities.ExecuteSql(sql);

        }

        public static void DeleteTask(string qrst_code)
        {
            sqLiteBaseUtilities = slLiteOperating.GetSubDbUtilities(EnumDBType.MIDB);
            string deletesql = string.Format("delete from taskdef where QRST_CODE = '{0}'", qrst_code);
            sqLiteBaseUtilities.ExecuteSql(deletesql);
        }

        #endregion


        /// <summary>
        /// 任务名,定义唯一标识，不可动态修改
        /// </summary>
        public abstract string TaskName { get; set; }
        //任务说明
        public string Description { get; set; }
        //任务编号
        public string Code { get; set; }
        //任务类型
        public EnumTaskType TaskType { get; set; }
        //外部执行参数
        public string[] ProcessArgu { get; set; }
        //外部执行程序
        public string ProcessExec { get; set; }
        //是否支持暂停
        public bool SuspendAvailable { get; set; }
        //任务执行状态
        public EnumTaskStatus Status { get; set; }
        //任务执行输入输出工作空间
        public string TaskWorkspace { get; set; }
        //任务执行回执
        public object TaskResult { get; set; }
        /// <summary>
        /// 关联订单对象
        /// </summary>
        public OrderClass ParentOrder { get; set; }
        /// <summary>
        /// 是否应用赋值化
        /// </summary>
        public bool isCreated { get; set; }
        /// <summary>
        /// 在taskDef中定义的参数说明
        /// </summary>
        public string paraMemo { get; set; }


        public TaskClass()
        {
            Code = "";
            TaskType = EnumTaskType.UnKnown;
            Status = EnumTaskStatus.Waiting;
            TaskResult = null;
            ParentOrder = null;
            isCreated = false;
        }

        public void Create(OrderClass parentorder)
        {
            int idx = parentorder.Tasks.IndexOf(this);
            if (idx == -1)
            {
                throw new System.Exception("ParentOrder error,Task Created fails!");
            }

            isCreated = true;
            ParentOrder = parentorder;

            this.ProcessArgu = ConstructParams(parentorder.TaskParams[idx]);

        }

        private string[] ConstructParams(string[] p)
        {
            string[] ps = new string[p.Length];

            for (int i = 0; i < p.Length; i++)
            {
                string pp = p[i];
                if (p[i].IndexOf("%OrderWorkspace%") != -1)
                {
                    pp = p[i].Replace("%OrderWorkspace%", ParentOrder.OrderWorkspace);
                }
                ps[i] = pp;
            }
            return ps;
        }


        public virtual void Process()
        { }

        public virtual void Suspend()
        { }

        public virtual void Cancel()
        { }

        public virtual int GetProgressInfo()
        { return -1; }

    }
}
