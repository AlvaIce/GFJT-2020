using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using QRST_DI_TS_Process.Orders;
using QRST_DI_TS_Process.Site;
 
namespace QRST_DI_MS_Desktop.UserInterfaces
{
    public partial class FrmAddTask : DevExpress.XtraEditors.XtraForm
    {
        //定义相关变量
        List<OrderDef> orderDefLst;                     //从数据库中获取的模板列表
        OrderClass orderClass;                               //表示最终添加的订单 

        DialogResult dialogResult;                          //添加环境参数的结果

        //定义添加完成事件
        public delegate void OrderFinishedEventHandler(object sender, MessageEventArgs me);
        public event OrderFinishedEventHandler MsgEvent;

        public FrmAddTask()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 确认添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfirm_Click(object sender, EventArgs e)
        {
			if (textOrderPara.Text!="")
            {
                orderClass.OrderName = textOrderName.Text;
                EnumOrderPriority priority;
                if (Enum.TryParse(cmbPriority.Text, out priority))
                {
                    orderClass.Priority = priority;
                }
                if (!string.IsNullOrEmpty(cmbServerSite.Text))
                {
					if (!cmbServerSite.Text.Equals("any"))						
						orderClass.TSSiteIP =cmbServerSite.Text;
                }
                
                orderClass.Owner = textOwner.Text;
				orderClass.OrderParams = orderClass.OrderParams;
                if (MsgEvent != null)
                {
                    MsgEvent(this, new MessageEventArgs(orderClass));
                }
                this.Close();
            }     
        }

        /// <summary>
        /// 取消添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 加载窗体时，初始化左侧的订单列表树
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmAddTask_Load(object sender, EventArgs e)
        {
            //加载订单列表

            List<OrderDef> orderDefLst = OrderDefManager.GetOrderDefFromDB("");
            for (int i = 0 ; i < orderDefLst.Count ; i++)
            {
                TreeNode tn = new TreeNode() { Name = orderDefLst[i].QRST_CODE, Text = orderDefLst[i].Description, Tag = orderDefLst[i] };
                if (orderDefLst[i].Type == EnumOrderType.Installed.ToString())
                {
                    treeView1.Nodes[0].Nodes.Add(tn);
                }
                else if (orderDefLst[i].Type == EnumOrderType.Customized.ToString())
                {
                    treeView1.Nodes[1].Nodes.Add(tn);
                }
                else if (orderDefLst[i].Type == EnumOrderType.Batch.ToString())
                {
                    treeView1.Nodes[3].Nodes.Add(tn);
                }
                else
                    treeView1.Nodes[2].Nodes.Add(tn);
            }
            treeView1.SelectedNode = treeView1.Nodes[0].Nodes[0];
            treeView1.ExpandAll();
            //加载站点信息
			cmbServerSite.Properties.Items.Add("any");
            cmbServerSite.Properties.Items.AddRange(TServerSiteManager.GetAllSiteIP());
            cmbPriority.Properties.Items.AddRange(new string[] { EnumOrderPriority.Emergency.ToString(), EnumOrderPriority.High.ToString(), EnumOrderPriority.Low.ToString(), EnumOrderPriority.Normal.ToString(), EnumOrderPriority.Ugent.ToString() });
			cmbServerSite.SelectedIndex = 0;
        }



        /// <summary>
        /// 设置订单参数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            FrmAddOrderPara frmAddOrderPara = new FrmAddOrderPara();
            frmAddOrderPara.orderClass = orderClass;
            dialogResult = frmAddOrderPara.ShowDialog(this);
			orderClass = frmAddOrderPara.orderClass;
			textOrderPara.Text = string.Join(",",orderClass.OrderParams);
            //List<string[]> str = orderClass.TaskParams;
        }

        /// <summary>
        /// 选中一种特定的订单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Nodes.Count == 0)
            {
                OrderDef ordef = (OrderDef)e.Node.Tag;
                orderClass = OrderDefManager.OrderDef2OrderClass(ordef);
                Show();
            }
        }

        /// <summary>
        /// 将选中的订单信息显示
        /// </summary>
        void Show()
        {
            textOrderName.Text = orderClass.OrderName;
            textOwner.Text = orderClass.Owner;
            cmbPriority.Text = orderClass.Priority.ToString();
            // cmbServerSite.Text = orderClass.TSSite.IPAdress();
            //if (orderClass.Type == EnumOrderType.Batch)
            //{
            //    txtBatchFileDir.Enabled = true;
            //    simpleButton1.Enabled = false;
            //    textOrderPara.Enabled = false;
            //    textTaskPara.Enabled = false;
            //}
            //else
            //{
            //    txtBatchFileDir.Enabled = false;
            //    simpleButton1.Enabled = true;
            //    textOrderPara.Enabled = true;
            //    textTaskPara.Enabled = true;
            //}
        }


        public class MessageEventArgs : EventArgs
        {
            public MessageEventArgs(OrderClass msg)
            {
                this.orderClass = msg;
            }

            public OrderClass orderClass;   //需要传递给主窗体中的消息  
        }

		private bool Check()
		{
			if (dialogResult == DialogResult.OK)
			{
				return true;
			}
			else
			{
				XtraMessageBox.Show("您还没有进行订单参数的设置！");
				return false;
			}
		}

        //private void txtBatchFileDir_Click(object sender, EventArgs e)
        //{
        //    string[] orderPara = new string[1];
        //    using (FolderBrowserDialog dlg = new FolderBrowserDialog { ShowNewFolderButton = false })
        //    {
        //        if (dlg.ShowDialog() == DialogResult.OK)
        //        {
        //            txtBatchFileDir.Text = dlg.SelectedPath;
        //            orderPara[0] = dlg.SelectedPath;
        //            orderClass.OrderParams = orderPara;
        //        }
        //    }
        //}
    }
}