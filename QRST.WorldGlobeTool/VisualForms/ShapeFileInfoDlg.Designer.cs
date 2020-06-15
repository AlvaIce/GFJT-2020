namespace QRST.WorldGlobeTool.VisualForms
{
    partial class ShapeFileInfoDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShapeFileInfoDlg));
            this.dataGridViewShapeDBF = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewShapeDBF)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewShapeDBF
            // 
            this.dataGridViewShapeDBF.AllowUserToAddRows = false;
            this.dataGridViewShapeDBF.AllowUserToDeleteRows = false;
            this.dataGridViewShapeDBF.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridViewShapeDBF.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewShapeDBF.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewShapeDBF.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewShapeDBF.Name = "dataGridViewShapeDBF";
            this.dataGridViewShapeDBF.ReadOnly = true;
            this.dataGridViewShapeDBF.RowTemplate.Height = 23;
            this.dataGridViewShapeDBF.Size = new System.Drawing.Size(558, 451);
            this.dataGridViewShapeDBF.TabIndex = 0;
            // 
            // ShapeFileInfoDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(558, 451);
            this.Controls.Add(this.dataGridViewShapeDBF);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ShapeFileInfoDlg";
            this.Text = "矢量文件属性表(.dbf)";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewShapeDBF)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewShapeDBF;
    }
}