/**************************************************
 * OrderBaseClass 为订单基类
 * 
 * QRST_DI 订单：指一个或多个任务组成的工作流订单
 * 
 * Version 1.0.0.0    huangxz 2012-11-13
 **************************************************/
using System;
using System.Collections.Generic;
using QRST_DI_TS_Process.Tasks;
using QRST_DI_Resources;
using QRST_DI_MS_Basis.Log;
using System.Threading;
using QRST_DI_DS_Metadata.DataImportPlugin;
using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetaData;


namespace QRST_DI_TS_Process.Orders
{
    [Serializable]
    public class OrderClass : IOrderInterface
    {
        public OrderClass()
        {

            Tasks = new List<TaskClass>();
            TaskParams = new List<string[]>();
            Priority = EnumOrderPriority.Normal;
            TaskPhase = 0;
        }

        //订单名称 可为空
        private string ordername="";
        public string OrderName
        {
            get
            {
                if (ordername == "")
                {
                    return OrderCode;
                }
                return ordername;
            }
            set { ordername = value; }
        }
        //订单编号 不可为空
        public string OrderCode { get; set; }
        //订单任务流
        public List<TaskClass> Tasks { get; set; }
        //订单优先级
        public EnumOrderPriority Priority { get; set; }
        //任务阶段
        public int TaskPhase { get; set; }
        //任务流参数
        public string[] OrderParams { get; set; }
        //任务流参数
        public List<string[]> TaskParams { get; set; }
        //订单类型
        public EnumOrderType Type { get; set; }
        //订单状态
        public EnumOrderStatusType Status { get; set; }
        //订单处理信息
        public OrderLog Logs { get; set; }
        //分配站点者IP
        public string TSSiteIP { get; set; }
        //提交者
        public string Owner { get; set; }

        //提交时间
        public DateTime SubmitTime { get; set; }

        public object Tag;
        //订单工作空间
        public string SetOrderWorkspace = "";       //设置路径
		public string OrderWorkspace
		{
			get
			{
				if (SetOrderWorkspace != "")
				{
					return SetOrderWorkspace;
				}
				if (TSSiteIP != null && TSSiteIP != "null" && TSSiteIP != string.Empty && TSSiteIP != "")
				{
				    switch (Constant.DbStorage)
				    {
				        case EnumDbStorage.MULTIPLE:
                            //默认路径
                            return string.Format(@"\\{0}\{1}\{2}\", TSSiteIP, StaticStrings.QRST_DB_Share, OrderCode);
                           
				        case EnumDbStorage.SINGLE:
                            return string.Format(@"{0}\{1}\{2}\", Constant.PcDBRootPath, StaticStrings.QRST_DB_Share, OrderCode);
                            break;
                        case EnumDbStorage.CLUSTER:


                            break;
				    }
				    
                }
				return "%OrderWorkspace%";
			}
						
		}
        
        public OrderClass(EnumOrderStatusType status1 = EnumOrderStatusType.Waiting)
        {

            Tasks = new List<TaskClass>();
            Status = status1;
            TaskParams = new List<string[]>();
            Priority = EnumOrderPriority.Normal;
            TaskPhase = 0;
            // Logs = new List<string>();
        }


        public void OrderProcess() 
        { 
             
            //签出待处理订单 开始生产 避免多站点重复签出

            //Status = EnumOrderStatusType.Processing;
            //if (!OrderManager.IsOrderUnsigned(this))
            //{
            //    return;
            //}
            //if (!OrderManager.UpdateOrder2DB(this))
            //{
            //    return;
            //}
            //创建工作空间
            CreateWorkingSpace();

            foreach (TaskClass task in Tasks)
            {
                task.Create(this);
            }
            processAction();
            //System.Threading.Tasks.Task processtask = System.Threading.Tasks.Task.Factory.StartNew(new Action(processAction));
            //System.Threading.Tasks.Task.WaitAll(new System.Threading.Tasks.Task[] { processtask });
        }

        protected void CreateWorkingSpace()
        {
            //创建工作空间
            if (!System.IO.Directory.Exists(OrderWorkspace))
            {
                System.IO.Directory.CreateDirectory(OrderWorkspace);
            }
            Logs.Add("工作空间创建完毕！");
        }

        private void processAction()
        {
            for (int i = TaskPhase; i < Tasks.Count; i++)
            {
                MyConsole.WriteLine(string.Format("{4} 正在执行任务：{1}-{2}/{3}（{0}）<<<{5}", Tasks[i].GetType().Name, OrderName, TaskPhase, Tasks.Count, DateTime.Now.ToString(), Thread.CurrentThread.ManagedThreadId.ToString()));              
                try
                {
                    Tasks[i].Process();

                    MyConsole.WriteLine(string.Format("{4} 任务执行完成：{1}-{2}/{3}（{0}）<<<{5}", Tasks[i].GetType().Name, OrderName, TaskPhase, Tasks.Count, DateTime.Now.ToString(), Thread.CurrentThread.ManagedThreadId.ToString()));              
                }
                catch (Exception ex) //订单执行出错，任务中断 zxw 2013/3/22
                {
                    MyConsole.WriteLine(string.Format("{4} 任务执行中断：{1}-{2}/{3}（{0}）<<<{5}", Tasks[i].GetType().Name, OrderName, TaskPhase, Tasks.Count, DateTime.Now.ToString(), Thread.CurrentThread.ManagedThreadId.ToString()));              
                    this.Logs.Add("任务中断:" + ex.ToString());
                    this.Status = EnumOrderStatusType.Error;
                    OrderManager.UpdateOrder2DB(this);
                    return;
                }

                TaskPhase = i + 1;
                if (!OrderManager.UpdateOrder2DB(this))
                {
                    return;
                }
                //判断是否该终端该任务 zxw 2013-1-29
                if (this.Status != EnumOrderStatusType.Processing)
                {
                    return;
                }
            }
            this.Status = EnumOrderStatusType.Completed;
            if (!OrderManager.UpdateOrder2DB(this))
            {
                return;
            }
        }


        public void OrderSuspend()
        {
            //暂停 再研究
            this.Status = EnumOrderStatusType.Suspending;
            OrderManager.UpdateOrder2DB(this);
        }

        public void OrderCancel()
        {
            //取消 再研究
            this.Status = EnumOrderStatusType.Cancelling;
            OrderManager.UpdateOrder2DB(this);
        }


        internal void Create()
        {
            //创建订单日志对象
            OrderLog.sqlUtilities = Constant.IdbOperating.GetSubDbUtilities(EnumDBType.MIDB);
            Logs = new OrderLog();
            Logs.orderCode = OrderCode;
        }

        #region 实现IOrderInterface接口
        public void Addlog(string logMessage)
        {
            if (this.Logs != null)
            {
                this.Logs.Add(logMessage);
            }
        }

        public string GetExecuteSite()
        {
            return TSSiteIP;
        }

        public string GetOrderWorkspace()
        {
            return this.OrderWorkspace;
        }


        #endregion
    }
}
