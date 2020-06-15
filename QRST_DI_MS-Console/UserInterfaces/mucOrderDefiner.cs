using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using QRST_DI_TS_Process.Tasks;
using QRST_DI_TS_Process.Orders;

namespace QRST_DI_MS_Console.UserInterfaces
{
    public partial class mucOrderDefiner : DevExpress.XtraEditors.XtraUserControl
    {

       public int currentSelectedTask = -1;  //当前选中任务

        //任务单击事件
        public delegate void TaskClickEventHandler(object sender, EventArgs e);
        public event TaskClickEventHandler TaskClick;

        //任务树列表选择事件
        public delegate void OrderListSelectedEventHandler(object sender, TreeViewEventArgs e);
        public event OrderListSelectedEventHandler OrderListSelected;

        
        public mucOrderDefiner()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 选择拖放对象
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeViewTask_ItemDrag(object sender, ItemDragEventArgs e)
        {
            TreeNode taskNode = (TreeNode)e.Item;
            if (taskNode.Parent != null)  //叶子节点
            {
                DoDragDrop(taskNode, DragDropEffects.Copy | DragDropEffects.Move);
            }
            
        }

        /// <summary>
        /// 进入时，如果是一个叶子节点，则更换箭头Effect
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void flowLayoutPanel1_DragEnter(object sender, DragEventArgs e)
        {
            TreeNode tn = new TreeNode();
            if (e.Data.GetDataPresent(tn.GetType().ToString()))
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
            
        }
        
        /// <summary>
        /// 拖放结束，将选择的任务添加到工作流面板中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void flowLayoutPanel1_DragDrop(object sender, DragEventArgs e)
        {
            TreeNode tn = new TreeNode();
             tn =   (TreeNode)e.Data.GetData(tn.GetType().ToString());
            TaskClass tc = (TaskClass)tn.Tag;
            CtrlTaskModel taskModel = new CtrlTaskModel() { taskClass = tc, Step = flowLayoutPanel1.Controls.Count + 1 };
            taskModel.TaskClick += TaskModel_Click;
            flowLayoutPanel1.Controls.Add(taskModel);
        }

        private void InitialTaskTree()
        {
            List<TaskClass> tasksLst = TaskClass.GetTasksByType(EnumTaskType.Installed);
            treeViewTask.Nodes[0].Nodes.Clear();
            treeViewTask.Nodes[1].Nodes.Clear();
            treeViewTask.Nodes[2].Nodes.Clear();
            treeViewTask.Nodes[3].Nodes.Clear();

            foreach(TaskClass var1 in tasksLst)
            {
                TreeNode tn = new TreeNode() {Name = var1.TaskName,Text = var1.Description,Tag = var1 };
                treeViewTask.Nodes[0].Nodes.Add(tn);
            }
            List<TaskClass> tasksLst1 = TaskClass.GetTasksByType(EnumTaskType.Customized);
            foreach (TaskClass var1 in tasksLst1)
            {
                TreeNode tn = new TreeNode() { Name = var1.TaskName, Text = var1.Description, Tag = var1 };
                treeViewTask.Nodes[1].Nodes.Add(tn);
            }
            List<TaskClass> tasksLst2 = TaskClass.GetTasksByType(EnumTaskType.Remote);
            foreach (TaskClass var1 in tasksLst2)
            {
                TreeNode tn = new TreeNode() { Name = var1.TaskName, Text = var1.Description, Tag = var1 };
                treeViewTask.Nodes[2].Nodes.Add(tn);
            }
            List<TaskClass> tasksLst3 = TaskClass.GetTasksByType(EnumTaskType.UnKnown);
            foreach (TaskClass var1 in tasksLst3)
            {
                TreeNode tn = new TreeNode() { Name = var1.TaskName, Text = var1.Description, Tag = var1 };
                treeViewTask.Nodes[3].Nodes.Add(tn);
            }
            treeViewTask.ExpandAll();
        }

        private void mucOrderDefiner_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            { 
                InitialTaskTree();
                GetOrderTreeDataSource();
            }
            
        }

        /// <summary>
        /// 清空面板中的任务列表
        /// </summary>
       public void ClearTasks()
        {
            flowLayoutPanel1.Controls.Clear();
            currentSelectedTask = -1;
        }

        /// <summary>
        /// 交换面板中的任务顺序
        /// </summary>
        /// <param name="fromIndex"></param>
        /// <param name="toIndex"></param>
        public void MoveTasks(int fromIndex,int toIndex)
       {
           if (fromIndex >= 0 && fromIndex < flowLayoutPanel1.Controls.Count && toIndex >= 0 && toIndex < flowLayoutPanel1.Controls.Count)
            {
                TaskClass tempTaskClass = ((CtrlTaskModel)flowLayoutPanel1.Controls[toIndex]).taskClass;
                ((CtrlTaskModel)flowLayoutPanel1.Controls[toIndex]).taskClass = ((CtrlTaskModel)flowLayoutPanel1.Controls[fromIndex]).taskClass;
                ((CtrlTaskModel)flowLayoutPanel1.Controls[fromIndex]).taskClass = tempTaskClass;
                ((CtrlTaskModel)flowLayoutPanel1.Controls[toIndex]).ClickTask();
            }   
       }

        /// <summary>
        /// 向上移
        /// </summary>
        /// <param name="controlIndex"></param>
        public void MoveUp(int controlIndex)
        {
            if (controlIndex > 0)
            {
                MoveTasks(controlIndex, controlIndex - 1);
            }
        }
        /// <summary>
        /// 向下移
        /// </summary>
        /// <param name="controlIndex"></param>
        public void MoveDown(int controlIndex)
        {
            if (controlIndex < flowLayoutPanel1.Controls.Count-1)
            {
                MoveTasks(controlIndex, controlIndex + 1);
            }
        }

        public void DeleteTask()
        {
            if (currentSelectedTask > 0)
            {
                flowLayoutPanel1.Controls.RemoveAt(currentSelectedTask -1);
                currentSelectedTask = -1;
            }
        }

        //task模板单击
        private void TaskModel_Click(object sender, EventArgs e)
        {
            if (currentSelectedTask > 0)
            {
                ((CtrlTaskModel)flowLayoutPanel1.Controls[currentSelectedTask - 1]).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            }
            
            //将该控件改为选中状态
            currentSelectedTask = ((CtrlTaskModel)sender).Step;

            if(TaskClick != null)
            {
                TaskClick(sender,e);
            }
        }

        /// <summary>
        /// 一出控件后，重新生成步骤
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void flowLayoutPanel1_ControlRemoved(object sender, ControlEventArgs e)
        {
            for (int i = 0 ; i < flowLayoutPanel1.Controls.Count ; i++)
            {
                ((CtrlTaskModel)flowLayoutPanel1.Controls[i]).Step = i + 1;
            }
        }

        /// <summary>
        /// 选中订单列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeOrderList_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (OrderListSelected != null)
            {
                OrderListSelected(sender,e);
            }
        }

        /// <summary>
        /// 加载订单列表树
        /// </summary>
        public void GetOrderTreeDataSource()
        {
            List<OrderDef> orderDefLst = OrderDefManager.GetOrderDefFromDB("");
            treeOrderList.Nodes[0].Nodes.Clear();
            treeOrderList.Nodes[1].Nodes.Clear();
            treeOrderList.Nodes[2].Nodes.Clear();
            for (int i = 0 ; i < orderDefLst.Count ; i++)
            {
                TreeNode tn = new TreeNode() { Name = orderDefLst[i].QRST_CODE, Text = orderDefLst[i].Description, Tag = orderDefLst[i] };
                if (orderDefLst[i].Type == EnumOrderType.Installed.ToString())
                {
                    treeOrderList.Nodes[0].Nodes.Add(tn);
                }
                else if (orderDefLst[i].Type == EnumOrderType.Customized.ToString())
                {
                    treeOrderList.Nodes[1].Nodes.Add(tn);
                }
                else
                    treeOrderList.Nodes[2].Nodes.Add(tn);
            }
          //  treeOrderList.SelectedNode = treeView1.Nodes[0].Nodes[0];
            treeOrderList.ExpandAll();
        }

        /// <summary>
        /// 在任务列表中加载任务
        /// </summary>
        /// <param name="orderDefLst"></param>
        public void LoadTaskLst(List<TaskClass> taskLst)
        {
            for (int i = 0 ; i < taskLst.Count ;i++ )
            {
                CtrlTaskModel taskModel = new CtrlTaskModel() { taskClass = taskLst[i], Step = flowLayoutPanel1.Controls.Count + 1 };
                taskModel.TaskClick += TaskModel_Click;
                flowLayoutPanel1.Controls.Add(taskModel);
            }
        }

        /// <summary>
        /// 收集定制的任务
        /// </summary>
        /// <returns></returns>
       public string GetTasksFromList()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0 ; i < flowLayoutPanel1.Controls.Count ;i++ )
            {
                TaskClass taskClass = ((CtrlTaskModel)flowLayoutPanel1.Controls[i]).taskClass;
                if (i != flowLayoutPanel1.Controls.Count-1)
                {
                    sb.Append(taskClass.TaskName+",");
                }
                else
                    sb.Append(taskClass.TaskName);
            }
            return sb.ToString();
        }
    }
}
