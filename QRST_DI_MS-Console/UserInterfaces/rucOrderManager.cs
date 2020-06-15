using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QRST_DI_TS_Process.Orders;
using System.Data;

namespace QRST_DI_MS_Console.UserInterfaces
{
    class rucOrderManager : RibbonPageBaseUC
    {
        private DevExpress.XtraBars.BarButtonItem btnCreateOrder;
        private DevExpress.XtraBars.BarButtonItem btnStart;
        private DevExpress.XtraBars.BarButtonItem btnStop;
        private DevExpress.XtraBars.BarButtonItem btnDelete;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem barProcessingLst;
        private DevExpress.XtraBars.BarButtonItem barFinishedLst;
        private DevExpress.XtraBars.BarEditItem barEditItem1;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox1;
        private DevExpress.XtraBars.BarButtonItem barButtonItemSuspend;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup5;
        private DevExpress.XtraBars.BarEditItem barEditItemOrderCode;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
        private DevExpress.XtraBars.BarEditItem barEditItemStartTime;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemDateEdit1;
        private DevExpress.XtraBars.BarEditItem barEditItemEndTime;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemDateEdit2;
        private DevExpress.XtraBars.BarEditItem barEditItemExeState;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox2;
        private DevExpress.XtraBars.BarEditItem barEditItemKewWord;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit2;
        private DevExpress.XtraBars.BarButtonItem barButtonItemSearch;
        private DevExpress.XtraEditors.Repository.RepositoryItemTimeEdit repositoryItemTimeEdit1;
        private DevExpress.XtraBars.BarButtonItem barButtonItemSelectAll;
        private DevExpress.XtraBars.BarButtonItem barButtonItemUnSelectAll;
        private DevExpress.XtraBars.BarButtonItem barButtonItemRestart;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup3;

                     public rucOrderManager()
            : base()
        {
            InitializeComponent();
        }

                     public rucOrderManager(object objMUC)
            : base(objMUC)
        {
            InitializeComponent();
             ((mucTaskMonitor)objMUC).FocusedRowChanged += FousedRowChanged;
        }
    
    
        private void InitializeComponent()
        {
            this.btnCreateOrder = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.barButtonItemRestart = new DevExpress.XtraBars.BarButtonItem();
            this.btnStart = new DevExpress.XtraBars.BarButtonItem();
            this.btnStop = new DevExpress.XtraBars.BarButtonItem();
            this.btnDelete = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemSelectAll = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemUnSelectAll = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPageGroup3 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barProcessingLst = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemSuspend = new DevExpress.XtraBars.BarButtonItem();
            this.barFinishedLst = new DevExpress.XtraBars.BarButtonItem();
            this.barEditItem1 = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.ribbonPageGroup5 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.barEditItemOrderCode = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.barEditItemExeState = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemComboBox2 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.barEditItemStartTime = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemDateEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.barEditItemEndTime = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemDateEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.barEditItemKewWord = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemTextEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.barButtonItemSearch = new DevExpress.XtraBars.BarButtonItem();
            this.repositoryItemTimeEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTimeEdit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit2.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTimeEdit1)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonControl1
            // 
            // 
            // 
            // 
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.ExpandCollapseItem.Name = "";
            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.btnCreateOrder,
            this.btnStart,
            this.btnStop,
            this.btnDelete,
            this.barButtonItem1,
            this.barProcessingLst,
            this.barFinishedLst,
            this.barEditItem1,
            this.barButtonItemSuspend,
            this.barEditItemOrderCode,
            this.barEditItemStartTime,
            this.barEditItemEndTime,
            this.barEditItemExeState,
            this.barEditItemKewWord,
            this.barButtonItemSearch,
            this.barButtonItemSelectAll,
            this.barButtonItemUnSelectAll,
            this.barButtonItemRestart});
            this.ribbonControl1.MaxItemId = 20;
            this.ribbonControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemComboBox1,
            this.repositoryItemTextEdit1,
            this.repositoryItemDateEdit1,
            this.repositoryItemDateEdit2,
            this.repositoryItemComboBox2,
            this.repositoryItemTextEdit2,
            this.repositoryItemTimeEdit1});
            this.ribbonControl1.Size = new System.Drawing.Size(1047, 149);
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup3,
            this.ribbonPageGroup5,
            this.ribbonPageGroup2});
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.btnCreateOrder);
            this.ribbonPageGroup1.Text = "";
            // 
            // btnCreateOrder
            // 
            this.btnCreateOrder.Caption = "创建新订单";
            this.btnCreateOrder.Id = 1;
            this.btnCreateOrder.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.创建新订单1;
            this.btnCreateOrder.Name = "btnCreateOrder";
            this.btnCreateOrder.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnCreateOrder_ItemClick);
            // 
            // ribbonPageGroup2
            // 
            this.ribbonPageGroup2.ItemLinks.Add(this.barButtonItemRestart, true);
            this.ribbonPageGroup2.ItemLinks.Add(this.btnStart, true);
            this.ribbonPageGroup2.ItemLinks.Add(this.btnStop, true);
            this.ribbonPageGroup2.ItemLinks.Add(this.btnDelete, true);
            this.ribbonPageGroup2.ItemLinks.Add(this.barButtonItemSelectAll, true);
            this.ribbonPageGroup2.ItemLinks.Add(this.barButtonItemUnSelectAll, true);
            this.ribbonPageGroup2.Name = "ribbonPageGroup2";
            this.ribbonPageGroup2.Text = "订单操作";
            this.ribbonPageGroup2.Visible = false;
            // 
            // barButtonItemRestart
            // 
            this.barButtonItemRestart.Caption = "重新执行";
            this.barButtonItemRestart.Id = 19;
            this.barButtonItemRestart.Name = "barButtonItemRestart";
            this.barButtonItemRestart.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemRestart_ItemClick);
            // 
            // btnStart
            // 
            this.btnStart.Caption = "执行";
            this.btnStart.Id = 2;
            this.btnStart.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.开始执行;
            this.btnStart.Name = "btnStart";
            this.btnStart.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.btnStart.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnStart_ItemClick);
            // 
            // btnStop
            // 
            this.btnStop.Caption = "挂起订单";
            this.btnStop.Id = 3;
            this.btnStop.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.暂停执行;
            this.btnStop.Name = "btnStop";
            this.btnStop.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.btnStop.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnStop_ItemClick);
            // 
            // btnDelete
            // 
            this.btnDelete.Caption = "删除订单";
            this.btnDelete.Id = 4;
            this.btnDelete.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.删除订单;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.btnDelete.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnDelete_ItemClick);
            // 
            // barButtonItemSelectAll
            // 
            this.barButtonItemSelectAll.Caption = "全选";
            this.barButtonItemSelectAll.Id = 17;
            this.barButtonItemSelectAll.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.完成;
            this.barButtonItemSelectAll.Name = "barButtonItemSelectAll";
            this.barButtonItemSelectAll.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonItemSelectAll.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemSelectAll_ItemClick);
            // 
            // barButtonItemUnSelectAll
            // 
            this.barButtonItemUnSelectAll.Caption = "全不选";
            this.barButtonItemUnSelectAll.Id = 18;
            this.barButtonItemUnSelectAll.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.全不选;
            this.barButtonItemUnSelectAll.Name = "barButtonItemUnSelectAll";
            this.barButtonItemUnSelectAll.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonItemUnSelectAll.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemUnSelectAll_ItemClick);
            // 
            // ribbonPageGroup3
            // 
            this.ribbonPageGroup3.ItemLinks.Add(this.barButtonItem1);
            this.ribbonPageGroup3.ItemLinks.Add(this.barProcessingLst);
            this.ribbonPageGroup3.ItemLinks.Add(this.barButtonItemSuspend, true);
            this.ribbonPageGroup3.Name = "ribbonPageGroup3";
            this.ribbonPageGroup3.Text = "订单监控";
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "等待订单";
            this.barButtonItem1.Id = 5;
            this.barButtonItem1.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.订单等待;
            this.barButtonItem1.Name = "barButtonItem1";
            this.barButtonItem1.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonItem1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem1_ItemClick);
            // 
            // barProcessingLst
            // 
            this.barProcessingLst.Caption = "正在执行";
            this.barProcessingLst.Id = 6;
            this.barProcessingLst.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.正在执行;
            this.barProcessingLst.Name = "barProcessingLst";
            this.barProcessingLst.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barProcessingLst.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barProcessingLst_ItemClick);
            // 
            // barButtonItemSuspend
            // 
            this.barButtonItemSuspend.Caption = "挂起订单";
            this.barButtonItemSuspend.Id = 9;
            this.barButtonItemSuspend.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.挂起订单;
            this.barButtonItemSuspend.Name = "barButtonItemSuspend";
            this.barButtonItemSuspend.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonItemSuspend.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemSuspend_ItemClick);
            // 
            // barFinishedLst
            // 
            this.barFinishedLst.Caption = "已完成";
            this.barFinishedLst.Id = 7;
            this.barFinishedLst.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.已完成;
            this.barFinishedLst.Name = "barFinishedLst";
            this.barFinishedLst.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barFinishedLst.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barFinishedLst_ItemClick);
            // 
            // barEditItem1
            // 
            this.barEditItem1.Caption = "监控频率(秒):";
            this.barEditItem1.Edit = this.repositoryItemComboBox1;
            this.barEditItem1.Id = 8;
            this.barEditItem1.Name = "barEditItem1";
            // 
            // repositoryItemComboBox1
            // 
            this.repositoryItemComboBox1.AutoHeight = false;
            this.repositoryItemComboBox1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox1.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "10"});
            this.repositoryItemComboBox1.Name = "repositoryItemComboBox1";
            this.repositoryItemComboBox1.SelectedIndexChanged += new System.EventHandler(this.repositoryItemComboBox1_SelectedIndexChanged);
            // 
            // ribbonPageGroup5
            // 
            this.ribbonPageGroup5.ItemLinks.Add(this.barEditItemOrderCode, true);
            this.ribbonPageGroup5.ItemLinks.Add(this.barEditItemExeState);
            this.ribbonPageGroup5.ItemLinks.Add(this.barEditItemStartTime, true);
            this.ribbonPageGroup5.ItemLinks.Add(this.barEditItemEndTime);
            this.ribbonPageGroup5.ItemLinks.Add(this.barEditItemKewWord, true);
            this.ribbonPageGroup5.ItemLinks.Add(this.barButtonItemSearch, true);
            this.ribbonPageGroup5.Name = "ribbonPageGroup5";
            this.ribbonPageGroup5.Text = "订单查询";
            // 
            // barEditItemOrderCode
            // 
            this.barEditItemOrderCode.Caption = "订单编号：";
            this.barEditItemOrderCode.Edit = this.repositoryItemTextEdit1;
            this.barEditItemOrderCode.Id = 10;
            this.barEditItemOrderCode.Name = "barEditItemOrderCode";
            this.barEditItemOrderCode.Width = 100;
            // 
            // repositoryItemTextEdit1
            // 
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            // 
            // barEditItemExeState
            // 
            this.barEditItemExeState.Caption = "执行状态：";
            this.barEditItemExeState.Edit = this.repositoryItemComboBox2;
            this.barEditItemExeState.Id = 13;
            this.barEditItemExeState.Name = "barEditItemExeState";
            this.barEditItemExeState.Width = 100;
            // 
            // repositoryItemComboBox2
            // 
            this.repositoryItemComboBox2.AutoHeight = false;
            this.repositoryItemComboBox2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox2.Items.AddRange(new object[] {
            "Waiting",
            "Error",
            "Suspended",
            "Completed"});
            this.repositoryItemComboBox2.Name = "repositoryItemComboBox2";
            // 
            // barEditItemStartTime
            // 
            this.barEditItemStartTime.Caption = "开始时间：";
            this.barEditItemStartTime.Edit = this.repositoryItemDateEdit1;
            this.barEditItemStartTime.Id = 11;
            this.barEditItemStartTime.Name = "barEditItemStartTime";
            this.barEditItemStartTime.Width = 100;
            // 
            // repositoryItemDateEdit1
            // 
            this.repositoryItemDateEdit1.AutoHeight = false;
            this.repositoryItemDateEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEdit1.Name = "repositoryItemDateEdit1";
            this.repositoryItemDateEdit1.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            // 
            // barEditItemEndTime
            // 
            this.barEditItemEndTime.Caption = "结束时间：";
            this.barEditItemEndTime.Edit = this.repositoryItemDateEdit2;
            this.barEditItemEndTime.Id = 12;
            this.barEditItemEndTime.Name = "barEditItemEndTime";
            this.barEditItemEndTime.Width = 100;
            // 
            // repositoryItemDateEdit2
            // 
            this.repositoryItemDateEdit2.AutoHeight = false;
            this.repositoryItemDateEdit2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEdit2.Name = "repositoryItemDateEdit2";
            this.repositoryItemDateEdit2.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            // 
            // barEditItemKewWord
            // 
            this.barEditItemKewWord.Caption = "关键字：";
            this.barEditItemKewWord.Edit = this.repositoryItemTextEdit2;
            this.barEditItemKewWord.Id = 14;
            this.barEditItemKewWord.Name = "barEditItemKewWord";
            this.barEditItemKewWord.Width = 100;
            // 
            // repositoryItemTextEdit2
            // 
            this.repositoryItemTextEdit2.AutoHeight = false;
            this.repositoryItemTextEdit2.Name = "repositoryItemTextEdit2";
            // 
            // barButtonItemSearch
            // 
            this.barButtonItemSearch.Caption = "检索";
            this.barButtonItemSearch.Id = 15;
            this.barButtonItemSearch.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.查看详细结果版面;
            this.barButtonItemSearch.Name = "barButtonItemSearch";
            this.barButtonItemSearch.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemSearch_ItemClick);
            // 
            // repositoryItemTimeEdit1
            // 
            this.repositoryItemTimeEdit1.AutoHeight = false;
            this.repositoryItemTimeEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repositoryItemTimeEdit1.Name = "repositoryItemTimeEdit1";
            // 
            // rucOrderManager
            // 
            this.Name = "rucOrderManager";
            this.Size = new System.Drawing.Size(1047, 150);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit2.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTimeEdit1)).EndInit();
            this.ResumeLayout(false);

        }

        private void btnCreateOrder_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FrmAddTask frmAddTask = new FrmAddTask();
            frmAddTask.MsgEvent += OrderDefined;
            frmAddTask.ShowDialog(this);
        }

        private void OrderDefined(object sender, QRST_DI_MS_Console.UserInterfaces.FrmAddTask.MessageEventArgs me)
        {
            OrderClass orderClass;
            List<OrderClass> orders;
            if (me.orderClass.Type == QRST_DI_TS_Process.Orders.EnumOrderType.Installed)
            {
                orderClass = OrderManager.CreateInstalledOrder(me.orderClass.OrderName, me.orderClass.OrderParams);
                OrderManager.AddNewOrder2DB(orderClass);
            }
            else if (me.orderClass.Type == QRST_DI_TS_Process.Orders.EnumOrderType.Batch)
            {
                orders = OrderManager.CreateInstalledBatchOrder(me.orderClass.OrderName, me.orderClass.OrderParams);
                foreach (OrderClass orderclass in orders)
                {
                    OrderManager.AddNewOrder2DB(orderclass);
                }
            }
            else
            {
                orderClass = OrderManager.CreateNewOrder(me.orderClass.OrderName, me.orderClass.Priority, me.orderClass.Owner, me.orderClass.Tasks, me.orderClass.TaskParams, me.orderClass.TSSiteIP);
                OrderManager.AddNewOrder2DB(orderClass);
            }           
            ((mucTaskMonitor)ObjMainUC).DisplayProcessOrders();
        }

        void FousedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
           
        }

        /// <summary>
        /// 监控等待的订单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ((mucTaskMonitor)ObjMainUC).DisplayWaitingOrders();
            ribbonPageGroup2.Visible = false;
        }

        /// <summary>
        /// 监控正在执行的订单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barProcessingLst_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ((mucTaskMonitor)ObjMainUC).DisplayProcessOrders();
            ribbonPageGroup2.Visible = false;

        }

        private void barFinishedLst_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ((mucTaskMonitor)ObjMainUC).DisplayFinishedOrders();
            ribbonPageGroup2.Visible = false;
        }

        /// <summary>
        /// 监控周期变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void repositoryItemComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            { 

                int interval = int.Parse(barEditItem1.EditValue.ToString());
                ((mucTaskMonitor)ObjMainUC).SetTimeSpan(interval * 1000);
            }
            catch (Exception)
            {

                return;
            }
        }

        private void barButtonItemSuspend_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ((mucTaskMonitor)ObjMainUC).DisplaySuspendOrders();
            ribbonPageGroup2.Visible = false;
        }

        /// <summary>
        /// 启动选中的订单,将选中的订单设置为Waiting 状态等待站点执行
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ((mucTaskMonitor)ObjMainUC).ChangeSelectOrderStatus(EnumOrderStatusType.Waiting);
        }

        /// <summary>
        /// 停止选中的订单，将选中的订单设置为Suspended 状态，将订单挂起
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStop_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ((mucTaskMonitor)ObjMainUC).ChangeSelectOrderStatus(EnumOrderStatusType.Suspended);
        }

        /// <summary>
        /// 删除选中的订单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ((mucTaskMonitor)ObjMainUC).DeleteChoosedOrders();
        }

        /// <summary>
        /// 查找订单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemSearch_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ((mucTaskMonitor)ObjMainUC).QueryOrder(ConstructQueryCondition());
            ribbonPageGroup2.Visible = true;
        }

        //构造查询条件
        string ConstructQueryCondition()
        {
            //订单编号,订单描述,提交时间,执行站点,执行进度,状态
            StringBuilder sb = new StringBuilder();

            if (barEditItemOrderCode.EditValue != null && barEditItemOrderCode.EditValue.ToString() != "")
            {
                sb.AppendFormat(" 订单编号 = '{0}' and", barEditItemOrderCode.EditValue.ToString());
            }
            if (barEditItemStartTime.EditValue != null && barEditItemStartTime.EditValue.ToString() != "")
            {
                sb.AppendFormat(" 提交时间 >= '{0}' and", barEditItemStartTime.EditValue.ToString());
            }
            if (barEditItemEndTime.EditValue != null && barEditItemEndTime.EditValue.ToString() != "")
            {
                sb.AppendFormat(" 提交时间 <= '{0}' and", barEditItemEndTime.EditValue.ToString());
            }
            if (barEditItemExeState.EditValue != null && barEditItemExeState.EditValue.ToString() != "")
            {
                sb.AppendFormat(" 状态 = '{0}' and", barEditItemExeState.EditValue.ToString());
            }
            if (barEditItemKewWord.EditValue != null && barEditItemKewWord.EditValue.ToString() != "")
            {
                sb.AppendFormat(" (订单描述 like '%{0}%' or 执行站点 like '%{0}%') ", barEditItemKewWord.EditValue.ToString());
            }
            return sb.ToString().TrimEnd("and".ToArray());
        }

        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemSelectAll_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ((mucTaskMonitor)ObjMainUC).SelectAll(true);
        }

        /// <summary>
        /// 全不选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemUnSelectAll_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ((mucTaskMonitor)ObjMainUC).SelectAll(false);
        }

        /// <summary>
        /// 重启订单，第一步开始重新执行订单，将选中的订单设置为Waiting ，将执行进度设置为0，等待站点执行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemRestart_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ((mucTaskMonitor)ObjMainUC).ChangeSelectOrderStatus(EnumOrderStatusType.Waiting, 0);
        }

      
    }
}
