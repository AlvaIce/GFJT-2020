namespace QRST_DI_MS_Console.UserInterfaces
{
    partial class FrmExportMetadata
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
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtStorePath = new DevExpress.XtraEditors.TextEdit();
            this.btnSelectPath = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.radioGroupDivideChar = new DevExpress.XtraEditors.RadioGroup();
            this.checkedListBoxColumns = new DevExpress.XtraEditors.CheckedListBoxControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.btnSelectAll = new DevExpress.XtraEditors.SimpleButton();
            this.btnUnselectAll = new DevExpress.XtraEditors.SimpleButton();
            this.btnExport = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancle = new DevExpress.XtraEditors.SimpleButton();
            this.progressBarControl1 = new DevExpress.XtraEditors.ProgressBarControl();
            this.lblExportInfo = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.txtStorePath.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroupDivideChar.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkedListBoxColumns)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.progressBarControl1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(12, 25);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(72, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "文件存放路径";
            // 
            // txtStorePath
            // 
            this.txtStorePath.Location = new System.Drawing.Point(90, 22);
            this.txtStorePath.Name = "txtStorePath";
            this.txtStorePath.Properties.ReadOnly = true;
            this.txtStorePath.Size = new System.Drawing.Size(415, 21);
            this.txtStorePath.TabIndex = 1;
            // 
            // btnSelectPath
            // 
            this.btnSelectPath.Location = new System.Drawing.Point(512, 19);
            this.btnSelectPath.Name = "btnSelectPath";
            this.btnSelectPath.Size = new System.Drawing.Size(64, 23);
            this.btnSelectPath.TabIndex = 2;
            this.btnSelectPath.Text = "...";
            this.btnSelectPath.Click += new System.EventHandler(this.btnSelectPath_Click);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(24, 68);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(60, 14);
            this.labelControl2.TabIndex = 3;
            this.labelControl2.Text = "栏位定界符";
            // 
            // radioGroupDivideChar
            // 
            this.radioGroupDivideChar.EditValue = "#";
            this.radioGroupDivideChar.Location = new System.Drawing.Point(90, 58);
            this.radioGroupDivideChar.Name = "radioGroupDivideChar";
            this.radioGroupDivideChar.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("#", "#"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(";", ";"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("$", "$"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("@", "@")});
            this.radioGroupDivideChar.Size = new System.Drawing.Size(415, 35);
            this.radioGroupDivideChar.TabIndex = 4;
            // 
            // checkedListBoxColumns
            // 
            this.checkedListBoxColumns.Location = new System.Drawing.Point(90, 114);
            this.checkedListBoxColumns.Name = "checkedListBoxColumns";
            this.checkedListBoxColumns.Size = new System.Drawing.Size(415, 212);
            this.checkedListBoxColumns.TabIndex = 5;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(24, 114);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(60, 14);
            this.labelControl3.TabIndex = 6;
            this.labelControl3.Text = "可导出的列";
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Location = new System.Drawing.Point(512, 114);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(64, 23);
            this.btnSelectAll.TabIndex = 7;
            this.btnSelectAll.Text = "全选";
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // btnUnselectAll
            // 
            this.btnUnselectAll.Location = new System.Drawing.Point(512, 143);
            this.btnUnselectAll.Name = "btnUnselectAll";
            this.btnUnselectAll.Size = new System.Drawing.Size(64, 23);
            this.btnUnselectAll.TabIndex = 8;
            this.btnUnselectAll.Text = "全不选";
            this.btnUnselectAll.Click += new System.EventHandler(this.btnUnselectAll_Click);
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(408, 381);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 9;
            this.btnExport.Text = "导出";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnCancle
            // 
            this.btnCancle.Location = new System.Drawing.Point(489, 381);
            this.btnCancle.Name = "btnCancle";
            this.btnCancle.Size = new System.Drawing.Size(75, 23);
            this.btnCancle.TabIndex = 10;
            this.btnCancle.Text = "取消";
            this.btnCancle.Click += new System.EventHandler(this.btnCancle_Click);
            // 
            // progressBarControl1
            // 
            this.progressBarControl1.Location = new System.Drawing.Point(90, 357);
            this.progressBarControl1.Name = "progressBarControl1";
            this.progressBarControl1.Size = new System.Drawing.Size(415, 18);
            this.progressBarControl1.TabIndex = 11;
            // 
            // lblExportInfo
            // 
            this.lblExportInfo.Location = new System.Drawing.Point(90, 337);
            this.lblExportInfo.Name = "lblExportInfo";
            this.lblExportInfo.Size = new System.Drawing.Size(48, 14);
            this.lblExportInfo.TabIndex = 12;
            this.lblExportInfo.Text = "导出信息";
            // 
            // FrmExportMetadata
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 416);
            this.Controls.Add(this.lblExportInfo);
            this.Controls.Add(this.progressBarControl1);
            this.Controls.Add(this.btnCancle);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnUnselectAll);
            this.Controls.Add(this.btnSelectAll);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.checkedListBoxColumns);
            this.Controls.Add(this.radioGroupDivideChar);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.btnSelectPath);
            this.Controls.Add(this.txtStorePath);
            this.Controls.Add(this.labelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmExportMetadata";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "元数据导出向导";
            ((System.ComponentModel.ISupportInitialize)(this.txtStorePath.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroupDivideChar.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkedListBoxColumns)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.progressBarControl1.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txtStorePath;
        private DevExpress.XtraEditors.SimpleButton btnSelectPath;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.RadioGroup radioGroupDivideChar;
        private DevExpress.XtraEditors.CheckedListBoxControl checkedListBoxColumns;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.SimpleButton btnSelectAll;
        private DevExpress.XtraEditors.SimpleButton btnUnselectAll;
        private DevExpress.XtraEditors.SimpleButton btnExport;
        private DevExpress.XtraEditors.SimpleButton btnCancle;
        private DevExpress.XtraEditors.ProgressBarControl progressBarControl1;
        private DevExpress.XtraEditors.LabelControl lblExportInfo;
    }
}