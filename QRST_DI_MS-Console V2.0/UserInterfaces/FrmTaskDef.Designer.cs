namespace QRST_DI_MS_Desktop.UserInterfaces
{
    partial class FrmTaskDef
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
            this.textEditDescription = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelControlMsg = new DevExpress.XtraEditors.LabelControl();
            this.labelControlDes = new DevExpress.XtraEditors.LabelControl();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnCancle = new DevExpress.XtraEditors.SimpleButton();
            this.btnConfirm = new DevExpress.XtraEditors.SimpleButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.comboBoxEditImportDataType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.memoEditPara = new DevExpress.XtraEditors.MemoEdit();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.textEditDllPath = new DevExpress.XtraEditors.TextEdit();
            this.btnChooseDllPath = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.textEditDescription.Properties)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEditImportDataType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoEditPara.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditDllPath.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(28, 24);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(72, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "任务组件描述";
            // 
            // textEditDescription
            // 
            this.textEditDescription.Location = new System.Drawing.Point(106, 21);
            this.textEditDescription.Name = "textEditDescription";
            this.textEditDescription.Size = new System.Drawing.Size(327, 21);
            this.textEditDescription.TabIndex = 1;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(52, 54);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(48, 14);
            this.labelControl2.TabIndex = 2;
            this.labelControl2.Text = "组件类型";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.White;
            this.groupBox1.Controls.Add(this.labelControlMsg);
            this.groupBox1.Controls.Add(this.labelControlDes);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(526, 74);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            // 
            // labelControlMsg
            // 
            this.labelControlMsg.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControlMsg.Location = new System.Drawing.Point(12, 54);
            this.labelControlMsg.Name = "labelControlMsg";
            this.labelControlMsg.Size = new System.Drawing.Size(0, 14);
            this.labelControlMsg.TabIndex = 2;
            this.labelControlMsg.Visible = false;
            // 
            // labelControlDes
            // 
            this.labelControlDes.Location = new System.Drawing.Point(12, 21);
            this.labelControlDes.Name = "labelControlDes";
            this.labelControlDes.Size = new System.Drawing.Size(180, 14);
            this.labelControlDes.TabIndex = 1;
            this.labelControlDes.Text = "创建一个新的自定义数据入库组件";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnCancle);
            this.groupBox2.Controls.Add(this.btnConfirm);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Location = new System.Drawing.Point(0, 394);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(526, 58);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            // 
            // btnCancle
            // 
            this.btnCancle.Location = new System.Drawing.Point(385, 21);
            this.btnCancle.Name = "btnCancle";
            this.btnCancle.Size = new System.Drawing.Size(75, 25);
            this.btnCancle.TabIndex = 7;
            this.btnCancle.Text = "取消";
            this.btnCancle.Click += new System.EventHandler(this.btnCancle_Click);
            // 
            // btnConfirm
            // 
            this.btnConfirm.Location = new System.Drawing.Point(304, 21);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(75, 25);
            this.btnConfirm.TabIndex = 6;
            this.btnConfirm.Text = "确定";
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.labelControl9);
            this.groupBox3.Controls.Add(this.labelControl1);
            this.groupBox3.Controls.Add(this.textEditDescription);
            this.groupBox3.Controls.Add(this.labelControl2);
            this.groupBox3.Location = new System.Drawing.Point(0, 80);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(526, 86);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            // 
            // labelControl9
            // 
            this.labelControl9.Location = new System.Drawing.Point(107, 54);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(36, 14);
            this.labelControl9.TabIndex = 3;
            this.labelControl9.Text = "自定义";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.comboBoxEditImportDataType);
            this.groupBox4.Controls.Add(this.labelControl3);
            this.groupBox4.Controls.Add(this.memoEditPara);
            this.groupBox4.Controls.Add(this.labelControl6);
            this.groupBox4.Controls.Add(this.labelControl5);
            this.groupBox4.Controls.Add(this.labelControl4);
            this.groupBox4.Location = new System.Drawing.Point(0, 172);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(526, 155);
            this.groupBox4.TabIndex = 7;
            this.groupBox4.TabStop = false;
            // 
            // comboBoxEditImportDataType
            // 
            this.comboBoxEditImportDataType.Location = new System.Drawing.Point(106, 18);
            this.comboBoxEditImportDataType.Name = "comboBoxEditImportDataType";
            this.comboBoxEditImportDataType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboBoxEditImportDataType.Size = new System.Drawing.Size(326, 21);
            this.comboBoxEditImportDataType.TabIndex = 18;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(28, 49);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(168, 14);
            this.labelControl3.TabIndex = 17;
            this.labelControl3.Text = "说明：每一个参数单独作为一行";
            // 
            // memoEditPara
            // 
            this.memoEditPara.EditValue = "";
            this.memoEditPara.Location = new System.Drawing.Point(106, 69);
            this.memoEditPara.Name = "memoEditPara";
            this.memoEditPara.Size = new System.Drawing.Size(327, 71);
            this.memoEditPara.TabIndex = 13;
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(28, 72);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(72, 14);
            this.labelControl6.TabIndex = 9;
            this.labelControl6.Text = "组件参数定义";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(106, 21);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(0, 14);
            this.labelControl5.TabIndex = 5;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(40, 21);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(60, 14);
            this.labelControl4.TabIndex = 4;
            this.labelControl4.Text = "导入的数据";
            // 
            // labelControl7
            // 
            this.labelControl7.Location = new System.Drawing.Point(28, 333);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(382, 14);
            this.labelControl7.TabIndex = 13;
            this.labelControl7.Text = "说明：上传的*.dll所在在的文件夹中必须包含与该文件运行相关的所有项";
            // 
            // labelControl8
            // 
            this.labelControl8.Location = new System.Drawing.Point(52, 374);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(48, 14);
            this.labelControl8.TabIndex = 14;
            this.labelControl8.Text = "组件文件";
            // 
            // textEditDllPath
            // 
            this.textEditDllPath.Location = new System.Drawing.Point(106, 367);
            this.textEditDllPath.Name = "textEditDllPath";
            this.textEditDllPath.Properties.ReadOnly = true;
            this.textEditDllPath.Size = new System.Drawing.Size(327, 21);
            this.textEditDllPath.TabIndex = 15;
            // 
            // btnChooseDllPath
            // 
            this.btnChooseDllPath.Location = new System.Drawing.Point(439, 363);
            this.btnChooseDllPath.Name = "btnChooseDllPath";
            this.btnChooseDllPath.Size = new System.Drawing.Size(75, 25);
            this.btnChooseDllPath.TabIndex = 16;
            this.btnChooseDllPath.Text = "选择";
            this.btnChooseDllPath.Click += new System.EventHandler(this.btnChooseDllPath_Click);
            // 
            // FrmTaskDef
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(526, 452);
            this.Controls.Add(this.btnChooseDllPath);
            this.Controls.Add(this.textEditDllPath);
            this.Controls.Add(this.labelControl8);
            this.Controls.Add(this.labelControl7);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmTaskDef";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "任务组件";
            this.Load += new System.EventHandler(this.FrmTaskDef_Load);
            ((System.ComponentModel.ISupportInitialize)(this.textEditDescription.Properties)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEditImportDataType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoEditPara.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditDllPath.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit textEditDescription;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private System.Windows.Forms.GroupBox groupBox1;
        private DevExpress.XtraEditors.LabelControl labelControlDes;
        private System.Windows.Forms.GroupBox groupBox2;
        private DevExpress.XtraEditors.SimpleButton btnCancle;
        private DevExpress.XtraEditors.SimpleButton btnConfirm;
        private System.Windows.Forms.GroupBox groupBox3;
        private DevExpress.XtraEditors.LabelControl labelControl9;
        private System.Windows.Forms.GroupBox groupBox4;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.TextEdit textEditDllPath;
        private DevExpress.XtraEditors.SimpleButton btnChooseDllPath;
        private DevExpress.XtraEditors.MemoEdit memoEditPara;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.ComboBoxEdit comboBoxEditImportDataType;
        private DevExpress.XtraEditors.LabelControl labelControlMsg;
    }
}