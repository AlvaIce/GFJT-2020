using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Threading.Tasks;
using QRST_DI_TS_Process.Tasks;

namespace QRST_DI_MS_Console.UserInterfaces
{
    public partial class CtrlTaskModel : DevExpress.XtraEditors.XtraUserControl
    {
        //任务单击事件
        public delegate void TaskClickEventHandler(object sender, EventArgs e);
        public event TaskClickEventHandler TaskClick;

        private TaskClass _taskClass;

         public TaskClass taskClass
        {
             set
            {
                 if (value != null)
                 {
                     _taskClass = value;

                     labelControlTaskDescription.Text = value.Description;
                     labelControlPara.Text = value.paraMemo;
                 } 
                 else
                 {
                     labelControlTaskDescription.Text = "null";
                     labelControlPara.Text = "null";
                 }
            }
             get
            {
                return _taskClass;
            }
        }

         public int Step
         {
             set {labelControlStep.Text = value.ToString() ;}
             get { return int.Parse(labelControlStep.Text); }
         }

     

        public CtrlTaskModel()
        {
            InitializeComponent();
        }

        private void CtrlTaskModel_Load(object sender, EventArgs e)
        {

        }


        /// <summary>
        /// 单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CtrlTaskModel_Click(object sender, EventArgs e)
        {
            ClickTask();
        }

        public void ClickTask()
        { 
            if (TaskClick != null)
            {
                TaskClick(this,null);
            }
            //将该控件改为选中状态
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))));
        }

        public void SelectStateCancled()
        {
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
        }
    }
}
