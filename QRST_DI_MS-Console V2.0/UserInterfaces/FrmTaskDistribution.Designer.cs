namespace QRST_DI_MS_Desktop.UserInterfaces
{
    partial class FrmTaskDistribution
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.documentManager1 = new DevExpress.XtraBars.Docking2010.DocumentManager(this.components);
            this.tabbedView1 = new DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView(this.components);
            this.subSystem = new DevExpress.XtraEditors.GroupControl();
            this.radioGroup1 = new DevExpress.XtraEditors.RadioGroup();
            this.subModel = new DevExpress.XtraEditors.GroupControl();
            this.checkedListBox = new DevExpress.XtraEditors.CheckedListBoxControl();
            this.subContent = new DevExpress.XtraEditors.GroupControl();
            this.taskName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.taskDescription = new DevExpress.XtraEditors.MemoEdit();
            this.btnConfirm = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.subData = new DevExpress.XtraEditors.GroupControl();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.documentManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabbedView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.subSystem)).BeginInit();
            this.subSystem.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.subModel)).BeginInit();
            this.subModel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.checkedListBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.subContent)).BeginInit();
            this.subContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.taskDescription.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.subData)).BeginInit();
            this.subData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // documentManager1
            // 
            this.documentManager1.MdiParent = this;
            this.documentManager1.View = this.tabbedView1;
            this.documentManager1.ViewCollection.AddRange(new DevExpress.XtraBars.Docking2010.Views.BaseView[] {
            this.tabbedView1});
            // 
            // subSystem
            // 
            this.subSystem.Controls.Add(this.radioGroup1);
            this.subSystem.Location = new System.Drawing.Point(13, 13);
            this.subSystem.Name = "subSystem";
            this.subSystem.Size = new System.Drawing.Size(300, 170);
            this.subSystem.TabIndex = 4;
            this.subSystem.Text = "子系统选择";
            // 
            // radioGroup1
            // 
            this.radioGroup1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioGroup1.Location = new System.Drawing.Point(2, 23);
            this.radioGroup1.Name = "radioGroup1";
            this.radioGroup1.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "交通路网规划与可行性分析子系统"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "交通道路勘察设计应用示范子系统"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "交通路网监控与应急子系统"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "高分交通出行服务子系统"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "航运与环境监测子系统"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "机场规划建设与环境监测子系统"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "高分交通综合数据库运行管理系统"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "高分交通一张图可视化系统")});
            this.radioGroup1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.radioGroup1.Size = new System.Drawing.Size(296, 145);
            this.radioGroup1.TabIndex = 1;
            this.radioGroup1.SelectedIndexChanged += new System.EventHandler(this.radioGroup1_TabIndexChanged);
            // 
            // subModel
            // 
            this.subModel.Controls.Add(this.checkedListBox);
            this.subModel.Location = new System.Drawing.Point(319, 13);
            this.subModel.Name = "subModel";
            this.subModel.Size = new System.Drawing.Size(300, 170);
            this.subModel.TabIndex = 5;
            this.subModel.Text = "服务分发模块";
            // 
            // checkedListBox
            // 
            this.checkedListBox.CheckOnClick = true;
            this.checkedListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBox.Items.AddRange(new DevExpress.XtraEditors.Controls.CheckedListBoxItem[] {
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem(null, "道路网提取"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem(null, "道路附属设施信息提取")});
            this.checkedListBox.Location = new System.Drawing.Point(2, 23);
            this.checkedListBox.Name = "checkedListBox";
            this.checkedListBox.Size = new System.Drawing.Size(296, 145);
            this.checkedListBox.TabIndex = 0;
            this.checkedListBox.ItemCheck += new DevExpress.XtraEditors.Controls.ItemCheckEventHandler(this.checkedListBox_ItemCheck);
            // 
            // subContent
            // 
            this.subContent.Controls.Add(this.taskName);
            this.subContent.Controls.Add(this.label2);
            this.subContent.Controls.Add(this.label1);
            this.subContent.Controls.Add(this.taskDescription);
            this.subContent.Location = new System.Drawing.Point(625, 13);
            this.subContent.Name = "subContent";
            this.subContent.Size = new System.Drawing.Size(300, 170);
            this.subContent.TabIndex = 7;
            this.subContent.Text = "任务设置";
            // 
            // taskName
            // 
            this.taskName.Location = new System.Drawing.Point(59, 30);
            this.taskName.Name = "taskName";
            this.taskName.Size = new System.Drawing.Size(236, 21);
            this.taskName.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "说明：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "任务名称：";
            // 
            // taskDescription
            // 
            this.taskDescription.Location = new System.Drawing.Point(2, 73);
            this.taskDescription.Name = "taskDescription";
            this.taskDescription.Size = new System.Drawing.Size(296, 95);
            this.taskDescription.TabIndex = 0;
            // 
            // btnConfirm
            // 
            this.btnConfirm.Location = new System.Drawing.Point(751, 495);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(75, 23);
            this.btnConfirm.TabIndex = 8;
            this.btnConfirm.Text = "确认";
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(850, 495);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // subData
            // 
            this.subData.Controls.Add(this.gridControl1);
            this.subData.Location = new System.Drawing.Point(13, 200);
            this.subData.Name = "subData";
            this.subData.Size = new System.Drawing.Size(912, 280);
            this.subData.TabIndex = 10;
            this.subData.Text = "已选择数据";
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(2, 23);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(908, 255);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.IndicatorWidth = 20;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsBehavior.ReadOnly = true;
            this.gridView1.OptionsSelection.MultiSelect = true;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // FrmTaskDistribution
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(934, 528);
            this.Controls.Add(this.subData);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.subContent);
            this.Controls.Add(this.subModel);
            this.Controls.Add(this.subSystem);
            this.IsMdiContainer = true;
            this.Name = "FrmTaskDistribution";
            this.Text = "任务分发窗体";
            ((System.ComponentModel.ISupportInitialize)(this.documentManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabbedView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.subSystem)).EndInit();
            this.subSystem.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.subModel)).EndInit();
            this.subModel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.checkedListBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.subContent)).EndInit();
            this.subContent.ResumeLayout(false);
            this.subContent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.taskDescription.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.subData)).EndInit();
            this.subData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.Docking2010.DocumentManager documentManager1;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnConfirm;
        private DevExpress.XtraEditors.GroupControl subContent;
        private DevExpress.XtraEditors.MemoEdit taskDescription;
        private DevExpress.XtraEditors.GroupControl subModel;
        private DevExpress.XtraEditors.CheckedListBoxControl checkedListBox;
        private DevExpress.XtraEditors.GroupControl subSystem;
        private DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView tabbedView1;
        private DevExpress.XtraEditors.RadioGroup radioGroup1;
        private DevExpress.XtraEditors.GroupControl subData;
        public DevExpress.XtraGrid.GridControl gridControl1;
        public DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private System.Windows.Forms.TextBox taskName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}