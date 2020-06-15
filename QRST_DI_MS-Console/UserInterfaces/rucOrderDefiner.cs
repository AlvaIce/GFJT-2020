using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using QRST_DI_TS_Process.Orders;
using QRST_DI_TS_Process.Tasks;
using DevExpress.XtraEditors;

namespace QRST_DI_MS_Console.UserInterfaces
{
    class rucOrderDefiner : RibbonPageBaseUC
    {
        private DevExpress.XtraBars.BarButtonItem barButtonDefineOrder;
        private DevExpress.XtraBars.BarButtonItem barButtonOrderDelete;
        private DevExpress.XtraBars.BarEditItem barEditItemName;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
        private DevExpress.XtraBars.BarEditItem barEditItemDescription;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit2;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit3;
        private DevExpress.XtraBars.BarEditItem barEditItemOrderPara;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit4;
        private DevExpress.XtraBars.BarButtonItem barButtonDeleteTask;
        private DevExpress.XtraBars.BarButtonItem barButtonMoveUpTask;
        private DevExpress.XtraBars.BarButtonItem barButtonMoveDownTask;
        private DevExpress.XtraBars.BarButtonItem barButtonSave;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup3;
        private DevExpress.XtraBars.BarEditItem barEditPriority;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox1;
        private DevExpress.XtraBars.BarEditItem barEditOrderType;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox2;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup4;

        private bool isAdd = true;     //判断是否为定制订单，为false则为更新订单
    
          public rucOrderDefiner()
            : base()
        {
            InitializeComponent();
        }

          public rucOrderDefiner(object objMUC)
            : base(objMUC)
        {
            InitializeComponent();

            ((mucOrderDefiner)ObjMainUC).OrderListSelected += OrderListSelected;
            ((mucOrderDefiner)ObjMainUC).TaskClick += TaskClick;
        }

          private void InitializeComponent()
          {
              this.barButtonDefineOrder = new DevExpress.XtraBars.BarButtonItem();
              this.barButtonOrderDelete = new DevExpress.XtraBars.BarButtonItem();
              this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
              this.barEditItemName = new DevExpress.XtraBars.BarEditItem();
              this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
              this.barEditOrderType = new DevExpress.XtraBars.BarEditItem();
              this.repositoryItemComboBox2 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
              this.barEditItemOrderPara = new DevExpress.XtraBars.BarEditItem();
              this.repositoryItemTextEdit4 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
              this.barEditItemDescription = new DevExpress.XtraBars.BarEditItem();
              this.repositoryItemTextEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
              this.barEditPriority = new DevExpress.XtraBars.BarEditItem();
              this.repositoryItemComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
              this.repositoryItemTextEdit3 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
              this.ribbonPageGroup3 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
              this.barButtonDeleteTask = new DevExpress.XtraBars.BarButtonItem();
              this.barButtonMoveUpTask = new DevExpress.XtraBars.BarButtonItem();
              this.barButtonMoveDownTask = new DevExpress.XtraBars.BarButtonItem();
              this.ribbonPageGroup4 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
              this.barButtonSave = new DevExpress.XtraBars.BarButtonItem();
              ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
              ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
              ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox2)).BeginInit();
              ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit4)).BeginInit();
              ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit2)).BeginInit();
              ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).BeginInit();
              ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit3)).BeginInit();
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
            this.barButtonDefineOrder,
            this.barButtonOrderDelete,
            this.barEditItemName,
            this.barEditItemDescription,
            this.barEditItemOrderPara,
            this.barButtonDeleteTask,
            this.barButtonMoveUpTask,
            this.barButtonMoveDownTask,
            this.barButtonSave,
            this.barEditPriority,
            this.barEditOrderType});
              this.ribbonControl1.MaxItemId = 14;
              this.ribbonControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemTextEdit1,
            this.repositoryItemTextEdit2,
            this.repositoryItemTextEdit3,
            this.repositoryItemTextEdit4,
            this.repositoryItemComboBox1,
            this.repositoryItemComboBox2});
              this.ribbonControl1.Size = new System.Drawing.Size(976, 149);
              // 
              // ribbonPage1
              // 
              this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup2,
            this.ribbonPageGroup3,
            this.ribbonPageGroup4});
              // 
              // ribbonPageGroup1
              // 
              this.ribbonPageGroup1.ItemLinks.Add(this.barButtonDefineOrder);
              this.ribbonPageGroup1.ItemLinks.Add(this.barButtonOrderDelete, true);
              this.ribbonPageGroup1.Text = "订单操作";
              // 
              // barButtonDefineOrder
              // 
              this.barButtonDefineOrder.Caption = "订单定制";
              this.barButtonDefineOrder.Id = 1;
              this.barButtonDefineOrder.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.订单定制2;
              this.barButtonDefineOrder.Name = "barButtonDefineOrder";
              this.barButtonDefineOrder.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
              this.barButtonDefineOrder.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonDefineOrder_ItemClick);
              // 
              // barButtonOrderDelete
              // 
              this.barButtonOrderDelete.Caption = "订单删除";
              this.barButtonOrderDelete.Enabled = false;
              this.barButtonOrderDelete.Id = 3;
              this.barButtonOrderDelete.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.订单删除1;
              this.barButtonOrderDelete.Name = "barButtonOrderDelete";
              this.barButtonOrderDelete.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonOrderDelete_ItemClick);
              // 
              // ribbonPageGroup2
              // 
              this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemName);
              this.ribbonPageGroup2.ItemLinks.Add(this.barEditOrderType);
              this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemOrderPara, true);
              this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemDescription);
              this.ribbonPageGroup2.ItemLinks.Add(this.barEditPriority, true);
              this.ribbonPageGroup2.Name = "ribbonPageGroup2";
              this.ribbonPageGroup2.Text = "订单信息设置";
              // 
              // barEditItemName
              // 
              this.barEditItemName.Caption = "订单名称";
              this.barEditItemName.Edit = this.repositoryItemTextEdit1;
              this.barEditItemName.Id = 4;
              this.barEditItemName.Name = "barEditItemName";
              this.barEditItemName.Width = 150;
              // 
              // repositoryItemTextEdit1
              // 
              this.repositoryItemTextEdit1.AutoHeight = false;
              this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
              // 
              // barEditOrderType
              // 
              this.barEditOrderType.Caption = "订单类型";
              this.barEditOrderType.Edit = this.repositoryItemComboBox2;
              this.barEditOrderType.Id = 13;
              this.barEditOrderType.Name = "barEditOrderType";
              this.barEditOrderType.Width = 150;
              // 
              // repositoryItemComboBox2
              // 
              this.repositoryItemComboBox2.AutoHeight = false;
              this.repositoryItemComboBox2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
              this.repositoryItemComboBox2.Items.AddRange(new object[] {
            "Installed",
            "Customized"});
              this.repositoryItemComboBox2.Name = "repositoryItemComboBox2";
              // 
              // barEditItemOrderPara
              // 
              this.barEditItemOrderPara.Caption = "订单参数";
              this.barEditItemOrderPara.Edit = this.repositoryItemTextEdit4;
              this.barEditItemOrderPara.Id = 7;
              this.barEditItemOrderPara.Name = "barEditItemOrderPara";
              this.barEditItemOrderPara.Width = 200;
              // 
              // repositoryItemTextEdit4
              // 
              this.repositoryItemTextEdit4.AutoHeight = false;
              this.repositoryItemTextEdit4.Name = "repositoryItemTextEdit4";
              // 
              // barEditItemDescription
              // 
              this.barEditItemDescription.Caption = "功能描述";
              this.barEditItemDescription.Edit = this.repositoryItemTextEdit2;
              this.barEditItemDescription.Id = 5;
              this.barEditItemDescription.Name = "barEditItemDescription";
              this.barEditItemDescription.Width = 200;
              // 
              // repositoryItemTextEdit2
              // 
              this.repositoryItemTextEdit2.AutoHeight = false;
              this.repositoryItemTextEdit2.Name = "repositoryItemTextEdit2";
              // 
              // barEditPriority
              // 
              this.barEditPriority.Caption = "优先级别";
              this.barEditPriority.Edit = this.repositoryItemComboBox1;
              this.barEditPriority.Id = 12;
              this.barEditPriority.Name = "barEditPriority";
              this.barEditPriority.Width = 100;
              // 
              // repositoryItemComboBox1
              // 
              this.repositoryItemComboBox1.AutoHeight = false;
              this.repositoryItemComboBox1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
              this.repositoryItemComboBox1.Items.AddRange(new object[] {
            "Normal",
            "Emergency"});
              this.repositoryItemComboBox1.Name = "repositoryItemComboBox1";
              // 
              // repositoryItemTextEdit3
              // 
              this.repositoryItemTextEdit3.AutoHeight = false;
              this.repositoryItemTextEdit3.Name = "repositoryItemTextEdit3";
              // 
              // ribbonPageGroup3
              // 
              this.ribbonPageGroup3.ItemLinks.Add(this.barButtonDeleteTask, true);
              this.ribbonPageGroup3.ItemLinks.Add(this.barButtonMoveUpTask, true);
              this.ribbonPageGroup3.ItemLinks.Add(this.barButtonMoveDownTask, true);
              this.ribbonPageGroup3.Name = "ribbonPageGroup3";
              this.ribbonPageGroup3.Text = "工作流操作";
              // 
              // barButtonDeleteTask
              // 
              this.barButtonDeleteTask.Caption = "删除";
              this.barButtonDeleteTask.Enabled = false;
              this.barButtonDeleteTask.Id = 8;
              this.barButtonDeleteTask.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.删除1;
              this.barButtonDeleteTask.Name = "barButtonDeleteTask";
              this.barButtonDeleteTask.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonDeleteTask_ItemClick);
              // 
              // barButtonMoveUpTask
              // 
              this.barButtonMoveUpTask.Caption = "上移";
              this.barButtonMoveUpTask.Enabled = false;
              this.barButtonMoveUpTask.Id = 9;
              this.barButtonMoveUpTask.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.上移;
              this.barButtonMoveUpTask.Name = "barButtonMoveUpTask";
              this.barButtonMoveUpTask.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonMoveUpTask_ItemClick);
              // 
              // barButtonMoveDownTask
              // 
              this.barButtonMoveDownTask.Caption = "下移";
              this.barButtonMoveDownTask.Enabled = false;
              this.barButtonMoveDownTask.Id = 10;
              this.barButtonMoveDownTask.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.下移;
              this.barButtonMoveDownTask.Name = "barButtonMoveDownTask";
              this.barButtonMoveDownTask.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonMoveDownTask_ItemClick);
              // 
              // ribbonPageGroup4
              // 
              this.ribbonPageGroup4.ItemLinks.Add(this.barButtonSave);
              this.ribbonPageGroup4.Name = "ribbonPageGroup4";
              this.ribbonPageGroup4.Text = "保存";
              // 
              // barButtonSave
              // 
              this.barButtonSave.Caption = "保存";
              this.barButtonSave.Enabled = false;
              this.barButtonSave.Id = 11;
              this.barButtonSave.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.保存2;
              this.barButtonSave.Name = "barButtonSave";
              this.barButtonSave.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonSave_ItemClick);
              // 
              // rucOrderDefiner
              // 
              this.Name = "rucOrderDefiner";
              this.Size = new System.Drawing.Size(976, 150);
              ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
              ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
              ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox2)).EndInit();
              ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit4)).EndInit();
              ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit2)).EndInit();
              ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).EndInit();
              ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit3)).EndInit();
              this.ResumeLayout(false);

          }

        /// <summary>
        /// 当任务面板发生单击事件发生
        /// </summary>
          void TaskClick(object sender, EventArgs e)
          {
              if (((mucOrderDefiner)ObjMainUC).currentSelectedTask == 1 )
              {
                  barButtonMoveUpTask.Enabled = false;
              }
              else
              {
                  barButtonMoveUpTask.Enabled = true;
              }
              if (((mucOrderDefiner)ObjMainUC).currentSelectedTask == ((mucOrderDefiner)ObjMainUC).flowLayoutPanel1.Controls.Count)
              {
                  barButtonMoveDownTask.Enabled = false;
              }
              else
              {
                  barButtonMoveDownTask.Enabled = true;
              }
              barButtonDeleteTask.Enabled = true;
          }

        /// <summary>
        /// 选中一个订单时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
          void OrderListSelected(object sender, TreeViewEventArgs e)
          {
              ClearInterFace();
              if ( e.Node != null &&e.Node.Parent != null)
              {
                  barButtonOrderDelete.Enabled= true;

                  OrderDef orderDef = (OrderDef)((mucOrderDefiner)ObjMainUC).treeOrderList.SelectedNode.Tag;
                  LoadTaskDef(orderDef);

                  if (orderDef.Type == EnumOrderType.Installed.ToString())
                  {
                      ribbonPageGroup3.Visible = false;
                  }
                  else


                  {
                      ribbonPageGroup3.Visible = true;
                  }

                  isAdd = false;
                  barButtonSave.Enabled = true;
              }
              else
              {
                  barButtonOrderDelete.Enabled = true;
                  barButtonSave.Enabled = false;
              }
          }
         
        /// <summary>
        /// 删除选中任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
          private void barButtonDeleteTask_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
          {
              int index = ((mucOrderDefiner)ObjMainUC).currentSelectedTask;
              if ( index> 0)
              {
                  ((mucOrderDefiner)ObjMainUC).DeleteTask();
              }
              if (((mucOrderDefiner)ObjMainUC).flowLayoutPanel1.Controls.Count == 0)
              {
                  barButtonDeleteTask.Enabled = false;
              }
          }

         /// <summary>
         /// 上移
         /// </summary>
         /// <param name="sender"></param>
         /// <param name="e"></param>
          private void barButtonMoveUpTask_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
          {
              ((mucOrderDefiner)ObjMainUC).MoveUp(((mucOrderDefiner)ObjMainUC).currentSelectedTask-1);
          }

        /// <summary>
        /// 下移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
          private void barButtonMoveDownTask_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
          {
              ((mucOrderDefiner)ObjMainUC).MoveDown(((mucOrderDefiner)ObjMainUC).currentSelectedTask-1);
          }

        /// <summary>
        /// 清空输入界面，包括输入框和任务面板
        /// </summary>
        private void ClearInterFace()
          {
              barEditItemDescription.EditValue = "";
              barEditItemName.EditValue = "";
              barEditItemOrderPara.EditValue = "";
              barEditOrderType.EditValue = "";
              barEditPriority.EditValue = "";

              ((mucOrderDefiner)ObjMainUC).ClearTasks();
              barButtonMoveUpTask.Enabled = false;
              barButtonMoveDownTask.Enabled = false;
              barButtonDeleteTask.Enabled = false;
          }

        /// <summary>
        /// 定制新订单
        /// 1.清空面板
        /// 2.将订单列表选中项设置为-1
        /// 3.启用保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonDefineOrder_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ClearInterFace();
            ((mucOrderDefiner)ObjMainUC).treeOrderList.SelectedNode = ((mucOrderDefiner)ObjMainUC).treeOrderList.Nodes[0];
            barButtonSave.Enabled = true;

            isAdd = true;
        }

        /// <summary>
        /// 订单更新
        /// 加载选中的订单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonUpdateOrder_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
      
        }

        /// <summary>
        /// 订单删除
        /// 删除选中的订单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonOrderDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OrderDef orderDef = (OrderDef)((mucOrderDefiner)ObjMainUC).treeOrderList.SelectedNode.Tag;
            System.Windows.Forms.DialogResult result = DevExpress.XtraEditors.XtraMessageBox.Show(this, "确定要删除订单'" + orderDef.Description + "'？", "提示", System.Windows.Forms.MessageBoxButtons.YesNo);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
               //删除该订单，重新加载订单树
                OrderDefManager.DeleteOrderDef(orderDef.id);

                ClearInterFace();
                ((mucOrderDefiner)ObjMainUC).GetOrderTreeDataSource();
                ((mucOrderDefiner)ObjMainUC).treeOrderList.SelectedNode = ((mucOrderDefiner)ObjMainUC).treeOrderList.Nodes[0];
            }
        }
        
        /// <summary>
        /// 将选中的taskDef加载到界面
        /// </summary>
        private void LoadTaskDef(OrderDef  orderDef)
        {
            if(orderDef != null)
            {
                barEditItemDescription.EditValue = orderDef.Description;
                barEditItemName.EditValue = orderDef.Name;
                barEditItemOrderPara.EditValue = orderDef.OrderParams;
                barEditOrderType.EditValue = orderDef.Type;
                barEditPriority.EditValue = orderDef.Priority;

                //加载任务列表
                List<TaskClass> tasksLst = OrderManager.Str2TaskList(orderDef.Tasks);

                ((mucOrderDefiner)ObjMainUC).LoadTaskLst(tasksLst);
            }

        }

        /// <summary>
        /// 保存更新或添加的orderDef
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Check())
            {
                OrderDef orderDef = GetInfoFromInterface();
                orderDef.SubmitTime = DateTime.Now;
                if (isAdd)   //添加定制的orderDef
                {
                  
                    OrderDefManager.AddOrderDef(orderDef);
                }
                else      //更新定制的Def
                {
                    orderDef.id = ((OrderDef)((mucOrderDefiner)ObjMainUC).treeOrderList.SelectedNode.Tag).id;
                    orderDef.QRST_CODE = ((OrderDef)((mucOrderDefiner)ObjMainUC).treeOrderList.SelectedNode.Tag).QRST_CODE;
                    OrderDefManager.UpdateOrderDef(orderDef);
                }
                ((mucOrderDefiner)ObjMainUC).GetOrderTreeDataSource();
                ClearInterFace();
                
                //重新加载订单树

            }
        }
        
        /// <summary>
        /// 检核用户的输入是否合法
        /// </summary>
        /// <returns></returns>
        private bool Check()
        {
            if (string.IsNullOrEmpty(barEditItemName.EditValue.ToString()))
            {
                XtraMessageBox.Show("请输入订单名称！");
                return false;
            }
            if (string.IsNullOrEmpty(barEditItemDescription.EditValue.ToString()))
            {
                XtraMessageBox.Show("请输入订单描述！");
                return false;
            }
            return true;
        }

        private OrderDef GetInfoFromInterface()
        {
            OrderDef orderDef = new OrderDef();

            orderDef.Description = barEditItemDescription.EditValue.ToString();
            orderDef.Name = barEditItemName.EditValue.ToString();
            orderDef.Priority = barEditPriority.EditValue.ToString();
            orderDef.OrderParams = barEditItemOrderPara.EditValue.ToString();
            orderDef.Type = barEditOrderType.EditValue.ToString();
            orderDef.Tasks = ((mucOrderDefiner)ObjMainUC).GetTasksFromList();
            return orderDef;
        }


    }
}
