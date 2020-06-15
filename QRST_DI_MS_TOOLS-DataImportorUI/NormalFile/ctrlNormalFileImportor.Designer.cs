namespace QRST_DI_MS_TOOLS_DataImportorUI.NormalFile
{
    partial class ctrlNormalFileImportor
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
            this.cbImportDataLst = new System.Windows.Forms.CheckedListBox();
            this.btnRemoveAll = new System.Windows.Forms.Button();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.rtextRemark = new System.Windows.Forms.RichTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonOpenFolder = new System.Windows.Forms.Button();
            this.chk_DirImportMode = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.btn_ImportData = new System.Windows.Forms.Button();
            this.btn_ClearList = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cbImportDataLst
            // 
            this.cbImportDataLst.FormattingEnabled = true;
            this.cbImportDataLst.Location = new System.Drawing.Point(35, 38);
            this.cbImportDataLst.Name = "cbImportDataLst";
            this.cbImportDataLst.Size = new System.Drawing.Size(582, 148);
            this.cbImportDataLst.TabIndex = 15;
            // 
            // btnRemoveAll
            // 
            this.btnRemoveAll.Location = new System.Drawing.Point(362, 9);
            this.btnRemoveAll.Name = "btnRemoveAll";
            this.btnRemoveAll.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveAll.TabIndex = 17;
            this.btnRemoveAll.Text = "全不选";
            this.btnRemoveAll.UseVisualStyleBackColor = true;
            this.btnRemoveAll.Click += new System.EventHandler(this.btnRemoveAll_Click);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Location = new System.Drawing.Point(281, 9);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(75, 23);
            this.btnSelectAll.TabIndex = 16;
            this.btnSelectAll.Text = "全选";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // rtextRemark
            // 
            this.rtextRemark.Location = new System.Drawing.Point(37, 205);
            this.rtextRemark.Name = "rtextRemark";
            this.rtextRemark.Size = new System.Drawing.Size(580, 41);
            this.rtextRemark.TabIndex = 32;
            this.rtextRemark.Text = "";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(35, 190);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 33;
            this.label3.Text = "备注说明：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(33, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(161, 12);
            this.label2.TabIndex = 36;
            this.label2.Text = "选择要导入的文件或文件夹：";
            // 
            // buttonOpenFolder
            // 
            this.buttonOpenFolder.Location = new System.Drawing.Point(200, 9);
            this.buttonOpenFolder.Name = "buttonOpenFolder";
            this.buttonOpenFolder.Size = new System.Drawing.Size(75, 23);
            this.buttonOpenFolder.TabIndex = 35;
            this.buttonOpenFolder.Text = "添加文件";
            this.buttonOpenFolder.UseVisualStyleBackColor = true;
            this.buttonOpenFolder.Click += new System.EventHandler(this.buttonOpenFolder_Click);
            // 
            // chk_DirImportMode
            // 
            this.chk_DirImportMode.AutoSize = true;
            this.chk_DirImportMode.Enabled = false;
            this.chk_DirImportMode.Location = new System.Drawing.Point(36, 254);
            this.chk_DirImportMode.Name = "chk_DirImportMode";
            this.chk_DirImportMode.Size = new System.Drawing.Size(108, 16);
            this.chk_DirImportMode.TabIndex = 37;
            this.chk_DirImportMode.Text = "保留文件夹关系";
            this.chk_DirImportMode.UseVisualStyleBackColor = true;
            this.chk_DirImportMode.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(150, 255);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 12);
            this.label4.TabIndex = 38;
            this.label4.Text = "添加至虚拟文件夹";
            this.label4.Visible = false;
            // 
            // textBox2
            // 
            this.textBox2.Enabled = false;
            this.textBox2.Location = new System.Drawing.Point(257, 252);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(360, 21);
            this.textBox2.TabIndex = 39;
            this.textBox2.Text = "root\\sdfsd\\";
            this.textBox2.Visible = false;
            // 
            // btn_ImportData
            // 
            this.btn_ImportData.Location = new System.Drawing.Point(201, 279);
            this.btn_ImportData.Name = "btn_ImportData";
            this.btn_ImportData.Size = new System.Drawing.Size(236, 23);
            this.btn_ImportData.TabIndex = 40;
            this.btn_ImportData.Text = "执行导入";
            this.btn_ImportData.UseVisualStyleBackColor = true;
            this.btn_ImportData.Click += new System.EventHandler(this.btn_ImportData_Click);
            // 
            // btn_ClearList
            // 
            this.btn_ClearList.Location = new System.Drawing.Point(443, 9);
            this.btn_ClearList.Name = "btn_ClearList";
            this.btn_ClearList.Size = new System.Drawing.Size(75, 23);
            this.btn_ClearList.TabIndex = 17;
            this.btn_ClearList.Text = "清空";
            this.btn_ClearList.UseVisualStyleBackColor = true;
            this.btn_ClearList.Click += new System.EventHandler(this.btn_ClearList_Click);
            // 
            // ctrlNormalFileImportor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btn_ImportData);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.chk_DirImportMode);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonOpenFolder);
            this.Controls.Add(this.rtextRemark);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbImportDataLst);
            this.Controls.Add(this.btn_ClearList);
            this.Controls.Add(this.btnRemoveAll);
            this.Controls.Add(this.btnSelectAll);
            this.Name = "ctrlNormalFileImportor";
            this.Size = new System.Drawing.Size(640, 317);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox cbImportDataLst;
        private System.Windows.Forms.Button btnRemoveAll;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.RichTextBox rtextRemark;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonOpenFolder;
        private System.Windows.Forms.CheckBox chk_DirImportMode;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button btn_ImportData;
        private System.Windows.Forms.Button btn_ClearList;
    }
}
