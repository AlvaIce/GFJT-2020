namespace TilesImport
{
    partial class FormTileImport
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.ucTileImport1 = new TilesImport.UCTileImport();
            this.SuspendLayout();
            // 
            // ucTileImport1
            // 
            this.ucTileImport1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucTileImport1.Location = new System.Drawing.Point(0, 0);
            this.ucTileImport1.Name = "ucTileImport1";
            this.ucTileImport1.Size = new System.Drawing.Size(715, 359);
            this.ucTileImport1.TabIndex = 0;
            // 
            // FormTileImport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(715, 359);
            this.Controls.Add(this.ucTileImport1);
            this.Name = "FormTileImport";
            this.Text = "切片数据入库V 1.7.0615.1022";
            this.ResumeLayout(false);

        }

        #endregion

        private UCTileImport ucTileImport1;

    }
}

