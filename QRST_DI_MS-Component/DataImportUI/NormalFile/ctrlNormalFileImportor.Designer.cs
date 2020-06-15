namespace QRST_DI_MS_Component_DataImportorUI.NormalFile
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
            this.chk_KeepRelateDir = new System.Windows.Forms.CheckBox();
            this.btn_ImportData = new System.Windows.Forms.Button();
            this.btn_ClearList = new System.Windows.Forms.Button();
            this.cmbFileCatalog = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ctrlVirtualDirSetting1 = new QRST_DI_MS_Component.DataImportUI.ctrlVirtualDirSetting();
            this.SuspendLayout();
            // 
            // cbImportDataLst
            // 
            this.cbImportDataLst.FormattingEnabled = true;
            this.cbImportDataLst.Location = new System.Drawing.Point(35, 46);
            this.cbImportDataLst.Name = "cbImportDataLst";
            this.cbImportDataLst.Size = new System.Drawing.Size(582, 180);
            this.cbImportDataLst.TabIndex = 15;
            // 
            // btnRemoveAll
            // 
            this.btnRemoveAll.Location = new System.Drawing.Point(507, 20);
            this.btnRemoveAll.Name = "btnRemoveAll";
            this.btnRemoveAll.Size = new System.Drawing.Size(53, 23);
            this.btnRemoveAll.TabIndex = 17;
            this.btnRemoveAll.Text = "全不选";
            this.btnRemoveAll.UseVisualStyleBackColor = true;
            this.btnRemoveAll.Click += new System.EventHandler(this.btnRemoveAll_Click);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Location = new System.Drawing.Point(448, 20);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(53, 23);
            this.btnSelectAll.TabIndex = 16;
            this.btnSelectAll.Text = "全选";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // rtextRemark
            // 
            this.rtextRemark.Location = new System.Drawing.Point(37, 244);
            this.rtextRemark.Name = "rtextRemark";
            this.rtextRemark.Size = new System.Drawing.Size(580, 41);
            this.rtextRemark.TabIndex = 32;
            this.rtextRemark.Text = "";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(35, 229);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 33;
            this.label3.Text = "备注说明：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(33, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 12);
            this.label2.TabIndex = 36;
            this.label2.Text = "选择文件或文件夹：";
            // 
            // buttonOpenFolder
            // 
            this.buttonOpenFolder.Location = new System.Drawing.Point(148, 20);
            this.buttonOpenFolder.Name = "buttonOpenFolder";
            this.buttonOpenFolder.Size = new System.Drawing.Size(75, 23);
            this.buttonOpenFolder.TabIndex = 35;
            this.buttonOpenFolder.Text = "添加文件";
            this.buttonOpenFolder.UseVisualStyleBackColor = true;
            this.buttonOpenFolder.Click += new System.EventHandler(this.buttonOpenFolder_Click);
            // 
            // chk_KeepRelateDir
            // 
            this.chk_KeepRelateDir.AutoSize = true;
            this.chk_KeepRelateDir.Enabled = false;
            this.chk_KeepRelateDir.Location = new System.Drawing.Point(509, 291);
            this.chk_KeepRelateDir.Name = "chk_KeepRelateDir";
            this.chk_KeepRelateDir.Size = new System.Drawing.Size(108, 16);
            this.chk_KeepRelateDir.TabIndex = 37;
            this.chk_KeepRelateDir.Text = "保留文件夹关系";
            this.chk_KeepRelateDir.UseVisualStyleBackColor = true;
            // 
            // btn_ImportData
            // 
            this.btn_ImportData.Location = new System.Drawing.Point(201, 318);
            this.btn_ImportData.Name = "btn_ImportData";
            this.btn_ImportData.Size = new System.Drawing.Size(236, 23);
            this.btn_ImportData.TabIndex = 40;
            this.btn_ImportData.Text = "执行导入";
            this.btn_ImportData.UseVisualStyleBackColor = true;
            this.btn_ImportData.Click += new System.EventHandler(this.btn_ImportData_Click);
            // 
            // btn_ClearList
            // 
            this.btn_ClearList.Location = new System.Drawing.Point(564, 20);
            this.btn_ClearList.Name = "btn_ClearList";
            this.btn_ClearList.Size = new System.Drawing.Size(53, 23);
            this.btn_ClearList.TabIndex = 17;
            this.btn_ClearList.Text = "清空";
            this.btn_ClearList.UseVisualStyleBackColor = true;
            this.btn_ClearList.Click += new System.EventHandler(this.btn_ClearList_Click);
            // 
            // cmbFileCatalog
            // 
            this.cmbFileCatalog.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFileCatalog.FormattingEnabled = true;
            this.cmbFileCatalog.Location = new System.Drawing.Point(294, 22);
            this.cmbFileCatalog.Name = "cmbFileCatalog";
            this.cmbFileCatalog.Size = new System.Drawing.Size(114, 20);
            this.cmbFileCatalog.TabIndex = 42;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(235, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 43;
            this.label1.Text = "归档类型:";
            // 
            // ctrlVirtualDirSetting1
            // 
            this.ctrlVirtualDirSetting1.Location = new System.Drawing.Point(37, 286);
            this.ctrlVirtualDirSetting1.Name = "ctrlVirtualDirSetting1";
            this.ctrlVirtualDirSetting1.Size = new System.Drawing.Size(466, 26);
            this.ctrlVirtualDirSetting1.TabIndex = 41;
            // 
            // ctrlNormalFileImportor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbFileCatalog);
            this.Controls.Add(this.ctrlVirtualDirSetting1);
            this.Controls.Add(this.btn_ImportData);
            this.Controls.Add(this.chk_KeepRelateDir);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonOpenFolder);
            this.Controls.Add(this.rtextRemark);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbImportDataLst);
            this.Controls.Add(this.btn_ClearList);
            this.Controls.Add(this.btnRemoveAll);
            this.Controls.Add(this.btnSelectAll);
            this.Name = "ctrlNormalFileImportor";
            this.Size = new System.Drawing.Size(640, 367);
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
        private System.Windows.Forms.CheckBox chk_KeepRelateDir;
        private System.Windows.Forms.Button btn_ImportData;
        private System.Windows.Forms.Button btn_ClearList;
        public QRST_DI_MS_Component.DataImportUI.ctrlVirtualDirSetting ctrlVirtualDirSetting1;
        public System.Windows.Forms.ComboBox cmbFileCatalog;
        private System.Windows.Forms.Label label1;
    }
}
