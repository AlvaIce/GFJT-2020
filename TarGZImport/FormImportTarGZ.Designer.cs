using QRST_DI_MS_Component_DataImportorUI.TAR.GZ;

namespace TarGZImport
{
    partial class FormImportTarGZ
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
            this.formImportTarGZ1 = new UCImportTarGZ();
            this.SuspendLayout();
            // 
            // formImportTarGZ1
            // 
            this.formImportTarGZ1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.formImportTarGZ1.Location = new System.Drawing.Point(0, 0);
            this.formImportTarGZ1.Name = "formImportTarGZ1";
            this.formImportTarGZ1.Size = new System.Drawing.Size(716, 434);
            this.formImportTarGZ1.TabIndex = 0;
            // 
            // FormImportTarGZ
            // 
            this.ClientSize = new System.Drawing.Size(716, 434);
            this.Controls.Add(this.formImportTarGZ1);
            this.Name = "FormImportTarGZ";
            this.Text = "原始压缩包数据导入(1.7.0314.1040)";
            this.ResumeLayout(false);

        }

        #endregion

        private UCImportTarGZ formImportTarGZ1;

    }
}

