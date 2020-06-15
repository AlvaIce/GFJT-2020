namespace QRST_DI_MS_Console.UserInterfaces
{
    partial class rucTaskSubmitter
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnChooseTask = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup3 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.btnCancleTask = new DevExpress.XtraBars.BarButtonItem();
            this.btnSubmitTask = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonGroup1 = new DevExpress.XtraBars.BarButtonGroup();
            this.barButtonGroup2 = new DevExpress.XtraBars.BarButtonGroup();
            this.barButtonGroup3 = new DevExpress.XtraBars.BarButtonGroup();
            this.ribbonPageGroup4 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.btnInputParam = new DevExpress.XtraBars.BarButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
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
            this.btnChooseTask,
            this.btnCancleTask,
            this.btnSubmitTask,
            this.barButtonGroup1,
            this.barButtonGroup2,
            this.barButtonGroup3,
            this.btnInputParam});
            this.ribbonControl1.MaxItemId = 9;
            this.ribbonControl1.Size = new System.Drawing.Size(687, 149);
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup4,
            this.ribbonPageGroup2,
            this.ribbonPageGroup3});
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.btnChooseTask);
            // 
            // btnChooseTask
            // 
            this.btnChooseTask.Caption = "选择任务";
            this.btnChooseTask.Id = 1;
            this.btnChooseTask.Name = "btnChooseTask";
            // 
            // ribbonPageGroup2
            // 
            this.ribbonPageGroup2.ItemLinks.Add(this.btnSubmitTask);
            this.ribbonPageGroup2.Name = "ribbonPageGroup2";
            this.ribbonPageGroup2.Text = "ribbonPageGroup2";
            // 
            // ribbonPageGroup3
            // 
            this.ribbonPageGroup3.ItemLinks.Add(this.btnCancleTask);
            this.ribbonPageGroup3.Name = "ribbonPageGroup3";
            this.ribbonPageGroup3.Text = "ribbonPageGroup3";
            // 
            // btnCancleTask
            // 
            this.btnCancleTask.Caption = "取消任务";
            this.btnCancleTask.Id = 2;
            this.btnCancleTask.Name = "btnCancleTask";
            // 
            // btnSubmitTask
            // 
            this.btnSubmitTask.Caption = "提交任务";
            this.btnSubmitTask.Id = 4;
            this.btnSubmitTask.Name = "btnSubmitTask";
            // 
            // barButtonGroup1
            // 
            this.barButtonGroup1.Caption = "barButtonGroup1";
            this.barButtonGroup1.Id = 5;
            this.barButtonGroup1.Name = "barButtonGroup1";
            // 
            // barButtonGroup2
            // 
            this.barButtonGroup2.Caption = "barButtonGroup2";
            this.barButtonGroup2.Id = 6;
            this.barButtonGroup2.Name = "barButtonGroup2";
            // 
            // barButtonGroup3
            // 
            this.barButtonGroup3.Caption = "barButtonGroup3";
            this.barButtonGroup3.Id = 7;
            this.barButtonGroup3.Name = "barButtonGroup3";
            // 
            // ribbonPageGroup4
            // 
            this.ribbonPageGroup4.ItemLinks.Add(this.btnInputParam);
            this.ribbonPageGroup4.Name = "ribbonPageGroup4";
            this.ribbonPageGroup4.Text = "ribbonPageGroup4";
            // 
            // btnInputParam
            // 
            this.btnInputParam.Caption = "输入参数";
            this.btnInputParam.Id = 8;
            this.btnInputParam.Name = "btnInputParam";
            // 
            // rucTaskSubmitter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "rucTaskSubmitter";
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.BarButtonItem btnChooseTask;
        private DevExpress.XtraBars.BarButtonItem btnCancleTask;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup3;
        private DevExpress.XtraBars.BarButtonItem btnSubmitTask;
        private DevExpress.XtraBars.BarButtonGroup barButtonGroup1;
        private DevExpress.XtraBars.BarButtonGroup barButtonGroup2;
        private DevExpress.XtraBars.BarButtonGroup barButtonGroup3;
        private DevExpress.XtraBars.BarButtonItem btnInputParam;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup4;
    }
}
