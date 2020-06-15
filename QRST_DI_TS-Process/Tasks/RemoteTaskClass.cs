/*
 * RemoteTaskClass 远程并行执行任务
 *  参数说明
 *     参数0为指定站点QRSTCODE可多数，“ALL”表示全部，例arg[0]="MIDB-14-1;MIDB-14-3;MIDB-14-4"
 *     参数1为实际执行的SubTask的TaskName (用于创建SubTask实例)
 *     剩余的参数为执行SubTask执行参数
 */
using System.Collections.Generic;
using QRST_DI_TS_Process.Site;
using QRST_DI_TS_Process.Orders;

namespace QRST_DI_TS_Process.Tasks
{
    public class RemoteTaskClass:TaskClass
    {
        public override string TaskName
        {
            get
            {
                return "RemoteTaskClass";
            }
            set { }
        }
        public RemoteTaskClass()
            : base()
        {
            isCreated = false;
            RemoteTSSites = new List<TServerSite>();
            RemoteSubOrders = new List<OrderClass>();
        }
        /// <summary>
        /// 参数0为指定站点QRSTCODE可多数，“ALL”表示全部，例arg[0]="MIDB-14-1;MIDB-14-3;MIDB-14-4"
        /// 参数1为实际执行的TaskName 
        /// </summary>
        /// <param name="parentorder"></param>
        public void Create(OrderClass parentorder)
        {
            base.Create(parentorder);

            isCreated = false;
            //初始化建立远程执行站点列表
            if (this.ProcessArgu[0].Trim().ToUpper() == "ALL")
            {
                RemoteTSSites.AddRange(TServerSiteManager.StorageSites.ToArray());
            }
            else
            {
                string[] sitesstr = this.ProcessArgu[0].Trim().TrimEnd(";".ToCharArray()).Split(";".ToCharArray());
                foreach (string sitestr in sitesstr)
                {
                    string code = sitestr.Trim();
                    if (code != "")
                    {
                        TServerSite site = TServerSiteManager.getSiteFromSiteCode(code);
                        if (site != null && RemoteTSSites.IndexOf(site) == -1)
                        {
                            RemoteTSSites.Add(site);
                        }
                    }
                }
            }
            //生成远程执行任务实例
            SubTask = TaskClass.CreateTaskClassByName(this.ProcessArgu[1]);
            //生成远程执行子订单
            List<TaskClass> tasks = new List<TaskClass>();
            List<string[]> taskparams = new List<string[]>();
            //  远程执行子订单任务列表
            tasks.Add(SubTask);
            string[] paramstrs = new string[this.ProcessArgu.Length - 2];
            for (int ii = 2; ii < this.ProcessArgu.Length; ii++)
            {
                paramstrs[ii] = this.ProcessArgu[ii].ToString();
            }
            taskparams.Add(paramstrs);
            //  为每站点创建远程执行子订单
            int i = 0;
            foreach (TServerSite site in RemoteTSSites)
            {
                RemoteSubOrders.Add(OrderManager.CreateNewOrder(
                    string.Format("Sub-{0}({1})", ParentOrder.OrderName, i.ToString()),
                    string.Format("{0}-{1}", ParentOrder.OrderCode, i.ToString()),
                    ParentOrder.Priority,
                    ParentOrder.Owner,
                    tasks,
                    taskparams,
                    site.IPAdress));
            }
            isCreated = true;
        }

        public List<TServerSite> RemoteTSSites { get; set; }

        public TaskClass SubTask { get; set; }

        public List<OrderClass> RemoteSubOrders { get; set; }

        public override void Suspend()
        {
            base.Suspend();
        }

        public override void Process()
        {
            foreach (OrderClass suborder in RemoteSubOrders)
            {
                OrderManager.SubmitOrder(suborder);
            }
        }

        public override void Cancel()
        {
            //取消 再研究
            base.Cancel();
        }
    }
}
