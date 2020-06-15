namespace QRST_DI_DataImportTool
{
    partial class ctrlCommonDataImport
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnChooseSingleFile = new System.Windows.Forms.Button();
            this.textSingleFilePath = new System.Windows.Forms.TextBox();
            this.radioSingleImport = new System.Windows.Forms.RadioButton();
            this.radioBatchImport = new System.Windows.Forms.RadioButton();
            this.btnChooseFolder = new System.Windows.Forms.Button();
            this.textFolderPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panelMain = new System.Windows.Forms.Panel();
            this.btnFinish = new System.Windows.Forms.Button();
            this.btnCancle = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnChooseSingleFile
            // 
            this.btnChooseSingleFile.Location = new System.Drawing.Point(646, 50);
            this.btnChooseSingleFile.Name = "btnChooseSingleFile";
            this.btnChooseSingleFile.Size = new System.Drawing.Size(73, 23);
            this.btnChooseSingleFile.TabIndex = 12;
            this.btnChooseSingleFile.Text = "选择...";
            this.btnChooseSingleFile.UseVisualStyleBackColor = true;
            this.btnChooseSingleFile.Click += new System.EventHandler(this.btnChooseSingleFile_Click);
            // 
            // textSingleFilePath
            // 
            this.textSingleFilePath.Location = new System.Drawing.Point(132, 50);
            this.textSingleFilePath.Name = "textSingleFilePath";
            this.textSingleFilePath.ReadOnly = true;
            this.textSingleFilePath.Size = new System.Drawing.Size(512, 21);
            this.textSingleFilePath.TabIndex = 11;
            // 
            // radioSingleImport
            // 
            this.radioSingleImport.AutoSize = true;
            this.radioSingleImport.Location = new System.Drawing.Point(17, 50);
            this.radioSingleImport.Name = "radioSingleImport";
            this.radioSingleImport.Size = new System.Drawing.Size(83, 16);
            this.radioSingleImport.TabIndex = 10;
            this.radioSingleImport.Text = "单数据导入";
            this.radioSingleImport.UseVisualStyleBackColor = true;
            this.radioSingleImport.CheckedChanged += new System.EventHandler(this.radioSingleImport_CheckedChanged);
            // 
            // radioBatchImport
            // 
            this.radioBatchImport.AutoSize = true;
            this.radioBatchImport.Checked = true;
            this.radioBatchImport.Location = new System.Drawing.Point(17, 19);
            this.radioBatchImport.Name = "radioBatchImport";
            this.radioBatchImport.Size = new System.Drawing.Size(95, 16);
            this.radioBatchImport.TabIndex = 9;
            this.radioBatchImport.TabStop = true;
            this.radioBatchImport.Text = "批量数据导入";
            this.radioBatchImport.UseVisualStyleBackColor = true;
            this.radioBatchImport.CheckedChanged += new System.EventHandler(this.radioBatchImport_CheckedChanged);
            // 
            // btnChooseFolder
            // 
            this.btnChooseFolder.Location = new System.Drawing.Point(646, 14);
            this.btnChooseFolder.Name = "btnChooseFolder";
            this.btnChooseFolder.Size = new System.Drawing.Size(73, 23);
            this.btnChooseFolder.TabIndex = 8;
            this.btnChooseFolder.Text = "选择...";
            this.btnChooseFolder.UseVisualStyleBackColor = true;
            this.btnChooseFolder.Click += new System.EventHandler(this.btnChooseFolder_Click);
            // 
            // textFolderPath
            // 
            this.textFolderPath.Location = new System.Drawing.Point(132, 14);
            this.textFolderPath.Name = "textFolderPath";
            this.textFolderPath.ReadOnly = true;
            this.textFolderPath.Size = new System.Drawing.Size(512, 21);
            this.textFolderPath.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 93);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 13;
            this.label1.Text = "数据详情";
            // 
            // panelMain
            // 
            this.panelMain.BackColor = System.Drawing.Color.White;
            this.panelMain.Location = new System.Drawing.Point(17, 124);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(702, 301);
            this.panelMain.TabIndex = 14;
            // 
            // btnFinish
            // 
            this.btnFinish.Enabled = false;
            this.btnFinish.Location = new System.Drawing.Point(554, 433);
            this.btnFinish.Name = "btnFinish";
            this.btnFinish.Size = new System.Drawing.Size(73, 23);
            this.btnFinish.TabIndex = 13;
            this.btnFinish.Text = "完成";
            this.btnFinish.UseVisualStyleBackColor = true;
            this.btnFinish.Click += new System.EventHandler(this.btnFinish_Click);
            // 
            // btnCancle
            // 
            this.btnCancle.Location = new System.Drawing.Point(633, 433);
            this.btnCancle.Name = "btnCancle";
            this.btnCancle.Size = new System.Drawing.Size(73, 23);
            this.btnCancle.TabIndex = 14;
            this.btnCancle.Text = "取消";
            this.btnCancle.UseVisualStyleBackColor = true;
            this.btnCancle.Click += new System.EventHandler(this.btnCancle_Click);
            // 
            // ctrlCommonDataImport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnCancle);
            this.Controls.Add(this.btnFinish);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnChooseSingleFile);
            this.Controls.Add(this.textSingleFilePath);
            this.Controls.Add(this.radioSingleImport);
            this.Controls.Add(this.radioBatchImport);
            this.Controls.Add(this.btnChooseFolder);
            this.Controls.Add(this.textFolderPath);
            this.Name = "ctrlCommonDataImport";
            this.Size = new System.Drawing.Size(733, 468);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnChooseSingleFile;
        private System.Windows.Forms.TextBox textSingleFilePath;
        private System.Windows.Forms.RadioButton radioSingleImport;
        private System.Windows.Forms.RadioButton radioBatchImport;
        private System.Windows.Forms.Button btnChooseFolder;
        private System.Windows.Forms.TextBox textFolderPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Button btnFinish;
        private System.Windows.Forms.Button btnCancle;
    }
}
