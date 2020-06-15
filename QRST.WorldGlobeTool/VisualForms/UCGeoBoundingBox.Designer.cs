namespace QRST.WorldGlobeTool.VisualForms
{
    partial class UCGeoBoundingBox
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.numericUpDownWest = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.numericUpDownNorth = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownEast = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownSouth = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownAltitude = new System.Windows.Forms.NumericUpDown();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWest)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownNorth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownEast)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSouth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAltitude)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.99881F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.99882F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.99881F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.00356F));
            this.tableLayoutPanel1.Controls.Add(this.numericUpDownWest, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label3, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.label4, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.label5, 4, 2);
            this.tableLayoutPanel1.Controls.Add(this.numericUpDownNorth, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.numericUpDownEast, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.numericUpDownSouth, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.numericUpDownAltitude, 4, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66737F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66736F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66736F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66736F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66319F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66736F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(294, 151);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // numericUpDownWest
            // 
            this.numericUpDownWest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownWest.DecimalPlaces = 2;
            this.numericUpDownWest.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDownWest.Location = new System.Drawing.Point(3, 78);
            this.numericUpDownWest.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.numericUpDownWest.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
            this.numericUpDownWest.Name = "numericUpDownWest";
            this.numericUpDownWest.Size = new System.Drawing.Size(62, 21);
            this.numericUpDownWest.TabIndex = 1;
            this.numericUpDownWest.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDownWest.ThousandsSeparator = true;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(81, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "北(°)";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "西(°)";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(149, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "东(°)";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(81, 113);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "南(°)";
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(235, 63);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "高程(m)";
            // 
            // numericUpDownNorth
            // 
            this.numericUpDownNorth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownNorth.DecimalPlaces = 2;
            this.numericUpDownNorth.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDownNorth.Location = new System.Drawing.Point(71, 28);
            this.numericUpDownNorth.Maximum = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.numericUpDownNorth.Minimum = new decimal(new int[] {
            90,
            0,
            0,
            -2147483648});
            this.numericUpDownNorth.Name = "numericUpDownNorth";
            this.numericUpDownNorth.Size = new System.Drawing.Size(62, 21);
            this.numericUpDownNorth.TabIndex = 9;
            this.numericUpDownNorth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDownNorth.ThousandsSeparator = true;
            // 
            // numericUpDownEast
            // 
            this.numericUpDownEast.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownEast.DecimalPlaces = 2;
            this.numericUpDownEast.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDownEast.Location = new System.Drawing.Point(139, 78);
            this.numericUpDownEast.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.numericUpDownEast.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
            this.numericUpDownEast.Name = "numericUpDownEast";
            this.numericUpDownEast.Size = new System.Drawing.Size(62, 21);
            this.numericUpDownEast.TabIndex = 10;
            this.numericUpDownEast.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDownEast.ThousandsSeparator = true;
            // 
            // numericUpDownSouth
            // 
            this.numericUpDownSouth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownSouth.DecimalPlaces = 2;
            this.numericUpDownSouth.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDownSouth.Location = new System.Drawing.Point(71, 128);
            this.numericUpDownSouth.Maximum = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.numericUpDownSouth.Minimum = new decimal(new int[] {
            90,
            0,
            0,
            -2147483648});
            this.numericUpDownSouth.Name = "numericUpDownSouth";
            this.numericUpDownSouth.Size = new System.Drawing.Size(62, 21);
            this.numericUpDownSouth.TabIndex = 11;
            this.numericUpDownSouth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDownSouth.ThousandsSeparator = true;
            // 
            // numericUpDownAltitude
            // 
            this.numericUpDownAltitude.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownAltitude.Location = new System.Drawing.Point(227, 78);
            this.numericUpDownAltitude.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.numericUpDownAltitude.Name = "numericUpDownAltitude";
            this.numericUpDownAltitude.Size = new System.Drawing.Size(64, 21);
            this.numericUpDownAltitude.TabIndex = 12;
            this.numericUpDownAltitude.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDownAltitude.ThousandsSeparator = true;
            // 
            // UCGeoBoundingBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "UCGeoBoundingBox";
            this.Size = new System.Drawing.Size(294, 151);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWest)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownNorth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownEast)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSouth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAltitude)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.NumericUpDown numericUpDownWest;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numericUpDownNorth;
        private System.Windows.Forms.NumericUpDown numericUpDownEast;
        private System.Windows.Forms.NumericUpDown numericUpDownSouth;
        private System.Windows.Forms.NumericUpDown numericUpDownAltitude;
    }
}
