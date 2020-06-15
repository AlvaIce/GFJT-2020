namespace QRST_DI_MS_Component_DataImportorUI.MouldDoc
{
    partial class ctrlMouldDocImportor
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
            this.rtextRemark = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonOpenFolder = new System.Windows.Forms.Button();
            this.btn_ImportData = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.cmbDocType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textAUTHORS = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.textKEYWORDs = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.textTitle = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.datetimepicker_DocDate = new System.Windows.Forms.DateTimePicker();
            this.rtxtDESC = new System.Windows.Forms.RichTextBox();
            this.ctrlVirtualDirSetting1 = new QRST_DI_MS_Component.DataImportUI.ctrlVirtualDirSetting();
            this.SuspendLayout();
            // 
            // rtextRemark
            // 
            this.rtextRemark.Location = new System.Drawing.Point(331, 140);
            this.rtextRemark.Name = "rtextRemark";
            this.rtextRemark.Size = new System.Drawing.Size(286, 96);
            this.rtextRemark.TabIndex = 32;
            this.rtextRemark.Text = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(33, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 12);
            this.label2.TabIndex = 36;
            this.label2.Text = "选择要导入的文件：";
            // 
            // buttonOpenFolder
            // 
            this.buttonOpenFolder.Location = new System.Drawing.Point(542, 29);
            this.buttonOpenFolder.Name = "buttonOpenFolder";
            this.buttonOpenFolder.Size = new System.Drawing.Size(75, 23);
            this.buttonOpenFolder.TabIndex = 35;
            this.buttonOpenFolder.Text = "添加文件";
            this.buttonOpenFolder.UseVisualStyleBackColor = true;
            this.buttonOpenFolder.Click += new System.EventHandler(this.buttonOpenFolder_Click);
            // 
            // btn_ImportData
            // 
            this.btn_ImportData.Location = new System.Drawing.Point(201, 268);
            this.btn_ImportData.Name = "btn_ImportData";
            this.btn_ImportData.Size = new System.Drawing.Size(236, 23);
            this.btn_ImportData.TabIndex = 40;
            this.btn_ImportData.Text = "执行导入";
            this.btn_ImportData.UseVisualStyleBackColor = true;
            this.btn_ImportData.Click += new System.EventHandler(this.btn_ImportData_Click);
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(70, 29);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(466, 21);
            this.textBox1.TabIndex = 41;
            // 
            // cmbDocType
            // 
            this.cmbDocType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDocType.FormattingEnabled = true;
            this.cmbDocType.Items.AddRange(new object[] {
            "小论文",
            "大论文",
            "ppt",
            "技术文档",
            "合同",
            "专利",
            "软件著作权"});
            this.cmbDocType.Location = new System.Drawing.Point(400, 72);
            this.cmbDocType.Name = "cmbDocType";
            this.cmbDocType.Size = new System.Drawing.Size(175, 20);
            this.cmbDocType.TabIndex = 57;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(329, 121);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 54;
            this.label1.Text = "备注说明：";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(329, 98);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(65, 12);
            this.label11.TabIndex = 48;
            this.label11.Text = "文档日期：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(41, 143);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 47;
            this.label5.Text = "摘要：";
            // 
            // textAUTHORS
            // 
            this.textAUTHORS.Location = new System.Drawing.Point(112, 118);
            this.textAUTHORS.Name = "textAUTHORS";
            this.textAUTHORS.Size = new System.Drawing.Size(199, 21);
            this.textAUTHORS.TabIndex = 45;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(329, 75);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(65, 12);
            this.label10.TabIndex = 43;
            this.label10.Text = "文档类型：";
            // 
            // textKEYWORDs
            // 
            this.textKEYWORDs.Location = new System.Drawing.Point(112, 95);
            this.textKEYWORDs.Name = "textKEYWORDs";
            this.textKEYWORDs.Size = new System.Drawing.Size(199, 21);
            this.textKEYWORDs.TabIndex = 51;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(41, 121);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 42;
            this.label6.Text = "作者：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(41, 98);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 49;
            this.label7.Text = "关键词：";
            // 
            // textTitle
            // 
            this.textTitle.Location = new System.Drawing.Point(112, 72);
            this.textTitle.Name = "textTitle";
            this.textTitle.Size = new System.Drawing.Size(199, 21);
            this.textTitle.TabIndex = 46;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(41, 75);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 12);
            this.label8.TabIndex = 44;
            this.label8.Text = "文档名称：";
            // 
            // datetimepicker_DocDate
            // 
            this.datetimepicker_DocDate.Location = new System.Drawing.Point(400, 95);
            this.datetimepicker_DocDate.Name = "datetimepicker_DocDate";
            this.datetimepicker_DocDate.Size = new System.Drawing.Size(175, 21);
            this.datetimepicker_DocDate.TabIndex = 58;
            // 
            // rtxtDESC
            // 
            this.rtxtDESC.Location = new System.Drawing.Point(112, 140);
            this.rtxtDESC.Name = "rtxtDESC";
            this.rtxtDESC.Size = new System.Drawing.Size(199, 96);
            this.rtxtDESC.TabIndex = 32;
            this.rtxtDESC.Text = "";
            // 
            // ctrlVirtualDirSetting1
            // 
            this.ctrlVirtualDirSetting1.Location = new System.Drawing.Point(70, 242);
            this.ctrlVirtualDirSetting1.Name = "ctrlVirtualDirSetting1";
            this.ctrlVirtualDirSetting1.Size = new System.Drawing.Size(533, 25);
            this.ctrlVirtualDirSetting1.TabIndex = 59;
            // 
            // ctrlMouldDocImportor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ctrlVirtualDirSetting1);
            this.Controls.Add(this.datetimepicker_DocDate);
            this.Controls.Add(this.cmbDocType);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textAUTHORS);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.textKEYWORDs);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textTitle);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btn_ImportData);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonOpenFolder);
            this.Controls.Add(this.rtxtDESC);
            this.Controls.Add(this.rtextRemark);
            this.Name = "ctrlMouldDocImportor";
            this.Size = new System.Drawing.Size(785, 377);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtextRemark;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonOpenFolder;
        private System.Windows.Forms.Button btn_ImportData;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ComboBox cmbDocType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textAUTHORS;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textKEYWORDs;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textTitle;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DateTimePicker datetimepicker_DocDate;
        private System.Windows.Forms.RichTextBox rtxtDESC;
        public QRST_DI_MS_Component.DataImportUI.ctrlVirtualDirSetting ctrlVirtualDirSetting1;
    }
}
