namespace QRST_DI_MS_Component.VirtualDirUI
{
    partial class VirtualDirUC
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            mainVirtualDirCtrl = new ctrlVirtualDir();
            this.splitContainer1.Panel1.Controls.Add(mainVirtualDirCtrl);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 

            // 
            // splitContainer1.Panel2
            // 


            this.splitContainer1.Size = new System.Drawing.Size(739, 452);

            //这行代码的意思就是控制是否平均分配两个panel
            this.splitContainer1.SplitterDistance = 367;

            // mainVirtualDirCtrl
            // 
            mainVirtualDirCtrl.AutoScroll = true;
            mainVirtualDirCtrl.AutoSize = true;
            mainVirtualDirCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            mainVirtualDirCtrl.Location = new System.Drawing.Point(0, 0);

            mainVirtualDirCtrl.Title = "Root";
            mainVirtualDirCtrl.Size = new System.Drawing.Size(345, 452);
            mainVirtualDirCtrl.TabIndex = 0;



            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "ReportManage";
            this.Size = new System.Drawing.Size(739, 452);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();

            //将panel1平铺
            this.splitContainer1.Panel2Collapsed = true;
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private ctrlVirtualDir mainVirtualDirCtrl;
        private ctrlVirtualDir otherUseVirtualDirCtrl;
    }
}
