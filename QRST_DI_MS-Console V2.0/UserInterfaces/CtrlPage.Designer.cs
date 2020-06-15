namespace QRST_DI_MS_Desktop.UserInterfaces
{
    partial class CtrlPage
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
            this.btnFirstPage = new DevExpress.XtraEditors.SimpleButton();
            this.btnPrePage = new DevExpress.XtraEditors.SimpleButton();
            this.btnNextPage = new DevExpress.XtraEditors.SimpleButton();
            this.btnLastPage = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.spinCurrentPage = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.cmbPageSize = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.lblPageInfo = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.spinCurrentPage.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbPageSize.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btnFirstPage
            // 
            this.btnFirstPage.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnFirstPage.Location = new System.Drawing.Point(479, 3);
            this.btnFirstPage.Name = "btnFirstPage";
            this.btnFirstPage.Size = new System.Drawing.Size(50, 23);
            this.btnFirstPage.TabIndex = 0;
            this.btnFirstPage.Text = "首页";
            this.btnFirstPage.Click += new System.EventHandler(this.btnFirstPage_Click);
            // 
            // btnPrePage
            // 
            this.btnPrePage.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnPrePage.Location = new System.Drawing.Point(535, 3);
            this.btnPrePage.Name = "btnPrePage";
            this.btnPrePage.Size = new System.Drawing.Size(50, 23);
            this.btnPrePage.TabIndex = 1;
            this.btnPrePage.Text = "上一页";
            this.btnPrePage.Click += new System.EventHandler(this.btnPrePage_Click);
            // 
            // btnNextPage
            // 
            this.btnNextPage.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnNextPage.Location = new System.Drawing.Point(591, 4);
            this.btnNextPage.Name = "btnNextPage";
            this.btnNextPage.Size = new System.Drawing.Size(50, 23);
            this.btnNextPage.TabIndex = 2;
            this.btnNextPage.Text = "下一页";
            this.btnNextPage.Click += new System.EventHandler(this.btnNextPage_Click);
            // 
            // btnLastPage
            // 
            this.btnLastPage.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnLastPage.Location = new System.Drawing.Point(647, 3);
            this.btnLastPage.Name = "btnLastPage";
            this.btnLastPage.Size = new System.Drawing.Size(50, 23);
            this.btnLastPage.TabIndex = 3;
            this.btnLastPage.Text = "尾页";
            this.btnLastPage.Click += new System.EventHandler(this.btnLastPage_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelControl1.Location = new System.Drawing.Point(461, 8);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(12, 14);
            this.labelControl1.TabIndex = 4;
            this.labelControl1.Text = "页";
            // 
            // spinCurrentPage
            // 
            this.spinCurrentPage.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.spinCurrentPage.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinCurrentPage.Location = new System.Drawing.Point(405, 5);
            this.spinCurrentPage.Name = "spinCurrentPage";
            this.spinCurrentPage.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.spinCurrentPage.Properties.MaxValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinCurrentPage.Properties.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinCurrentPage.Size = new System.Drawing.Size(50, 21);
            this.spinCurrentPage.TabIndex = 5;
            this.spinCurrentPage.EditValueChanged += new System.EventHandler(this.spinCurrentPage_EditValueChanged);
            // 
            // labelControl2
            // 
            this.labelControl2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelControl2.Location = new System.Drawing.Point(346, 8);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(53, 14);
            this.labelControl2.TabIndex = 6;
            this.labelControl2.Text = "条记录|第";
            // 
            // cmbPageSize
            // 
            this.cmbPageSize.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.cmbPageSize.Location = new System.Drawing.Point(276, 6);
            this.cmbPageSize.Name = "cmbPageSize";
            this.cmbPageSize.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.cmbPageSize.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbPageSize.Size = new System.Drawing.Size(64, 21);
            this.cmbPageSize.TabIndex = 7;
            this.cmbPageSize.SelectedIndexChanged += new System.EventHandler(this.cmbPageSize_SelectedIndexChanged);
            // 
            // labelControl3
            // 
            this.labelControl3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelControl3.Location = new System.Drawing.Point(246, 8);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(24, 14);
            this.labelControl3.TabIndex = 8;
            this.labelControl3.Text = "每页";
            // 
            // lblPageInfo
            // 
            this.lblPageInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.lblPageInfo.Location = new System.Drawing.Point(3, 9);
            this.lblPageInfo.Name = "lblPageInfo";
            this.lblPageInfo.Size = new System.Drawing.Size(127, 14);
            this.lblPageInfo.TabIndex = 9;
            this.lblPageInfo.Text = "第0页|共0页|共0条记录";
            // 
            // CtrlPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblPageInfo);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.cmbPageSize);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.spinCurrentPage);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.btnLastPage);
            this.Controls.Add(this.btnNextPage);
            this.Controls.Add(this.btnPrePage);
            this.Controls.Add(this.btnFirstPage);
            this.Name = "CtrlPage";
            this.Size = new System.Drawing.Size(700, 30);
            ((System.ComponentModel.ISupportInitialize)(this.spinCurrentPage.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbPageSize.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnFirstPage;
        private DevExpress.XtraEditors.SimpleButton btnPrePage;
        private DevExpress.XtraEditors.SimpleButton btnNextPage;
        private DevExpress.XtraEditors.SimpleButton btnLastPage;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SpinEdit spinCurrentPage;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.ComboBoxEdit cmbPageSize;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl lblPageInfo;

    }
}
