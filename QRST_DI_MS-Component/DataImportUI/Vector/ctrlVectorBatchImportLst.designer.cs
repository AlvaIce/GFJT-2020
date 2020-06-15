namespace QRST_DI_MS_Component_DataImportorUI.Vector
{
    partial class ctrlVectorBatchImportLst
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
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.btnRemoveAll = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.ctrlVectorUserMetaDataSetting1 = new QRST_DI_MS_Component_DataImportorUI.Vector.CtrlVectorUserMetaDataSetting();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cbImportDataLst
            // 
            this.cbImportDataLst.FormattingEnabled = true;
            this.cbImportDataLst.Location = new System.Drawing.Point(3, 19);
            this.cbImportDataLst.Name = "cbImportDataLst";
            this.cbImportDataLst.Size = new System.Drawing.Size(582, 148);
            this.cbImportDataLst.TabIndex = 0;
            this.cbImportDataLst.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.cbImportDataLst_ItemCheck);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Location = new System.Drawing.Point(591, 19);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(75, 23);
            this.btnSelectAll.TabIndex = 1;
            this.btnSelectAll.Text = "全选";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // btnRemoveAll
            // 
            this.btnRemoveAll.Location = new System.Drawing.Point(591, 48);
            this.btnRemoveAll.Name = "btnRemoveAll";
            this.btnRemoveAll.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveAll.TabIndex = 2;
            this.btnRemoveAll.Text = "全不选";
            this.btnRemoveAll.UseVisualStyleBackColor = true;
            this.btnRemoveAll.Click += new System.EventHandler(this.btnRemoveAll_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(591, 77);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 3;
            this.button3.Text = "刷新";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // ctrlVectorUserMetaDataSetting1
            // 
            this.ctrlVectorUserMetaDataSetting1.Location = new System.Drawing.Point(73, 173);
            this.ctrlVectorUserMetaDataSetting1.Name = "ctrlVectorUserMetaDataSetting1";
            this.ctrlVectorUserMetaDataSetting1.Size = new System.Drawing.Size(510, 154);
            this.ctrlVectorUserMetaDataSetting1.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 14;
            this.label1.Text = "数据详情";
            // 
            // ctrlVectorBatchImportLst
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbImportDataLst);
            this.Controls.Add(this.ctrlVectorUserMetaDataSetting1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.btnRemoveAll);
            this.Controls.Add(this.btnSelectAll);
            this.Name = "ctrlVectorBatchImportLst";
            this.Size = new System.Drawing.Size(693, 319);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox cbImportDataLst;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Button btnRemoveAll;
        private System.Windows.Forms.Button button3;
        private CtrlVectorUserMetaDataSetting ctrlVectorUserMetaDataSetting1;
        private System.Windows.Forms.Label label1;
    }
}
