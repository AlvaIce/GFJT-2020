namespace QRST_DI_MS_TOOLS_DataImportorUI.Raster
{
    partial class ctrlRasterProdImportor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ctrlRasterProdImportor));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.dataGridViewX1 = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.colCheck = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colProdDir = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProducor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProduceTime = new DevComponents.DotNetBar.Controls.DataGridViewDateTimeInputColumn();
            this.colRemark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSourceDataName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProdType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSize = new DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip();
            this.全选ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.全不选ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.还原初始状态ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btn_ImportData = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonOpenFolder = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewX1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.SystemColors.Control;
            this.richTextBox1.Enabled = false;
            this.richTextBox1.Location = new System.Drawing.Point(833, 38);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(265, 330);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = resources.GetString("richTextBox1.Text");
            // 
            // dataGridViewX1
            // 
            this.dataGridViewX1.AllowUserToAddRows = false;
            this.dataGridViewX1.AllowUserToDeleteRows = false;
            this.dataGridViewX1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewX1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCheck,
            this.colProdDir,
            this.colProducor,
            this.colProduceTime,
            this.colRemark,
            this.colSourceDataName,
            this.colProdType,
            this.colSize});
            this.dataGridViewX1.ContextMenuStrip = this.contextMenuStrip1;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewX1.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewX1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dataGridViewX1.Location = new System.Drawing.Point(3, 65);
            this.dataGridViewX1.Name = "dataGridViewX1";
            this.dataGridViewX1.RowTemplate.Height = 23;
            this.dataGridViewX1.Size = new System.Drawing.Size(824, 303);
            this.dataGridViewX1.TabIndex = 2;
            // 
            // colCheck
            // 
            this.colCheck.Frozen = true;
            this.colCheck.HeaderText = "";
            this.colCheck.Name = "colCheck";
            this.colCheck.Width = 30;
            // 
            // colProdDir
            // 
            this.colProdDir.Frozen = true;
            this.colProdDir.HeaderText = "产品文件夹";
            this.colProdDir.Name = "colProdDir";
            this.colProdDir.ReadOnly = true;
            this.colProdDir.Width = 200;
            // 
            // colProducor
            // 
            this.colProducor.Frozen = true;
            this.colProducor.HeaderText = "生产者（单位）";
            this.colProducor.Name = "colProducor";
            this.colProducor.Width = 120;
            // 
            // colProduceTime
            // 
            // 
            // 
            // 
            this.colProduceTime.BackgroundStyle.Class = "DataGridViewDateTimeBorder";
            this.colProduceTime.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.colProduceTime.Frozen = true;
            this.colProduceTime.HeaderText = "生产日期";
            this.colProduceTime.InputHorizontalAlignment = DevComponents.Editors.eHorizontalAlignment.Left;
            // 
            // 
            // 
            this.colProduceTime.MonthCalendar.AnnuallyMarkedDates = new System.DateTime[0];
            // 
            // 
            // 
            this.colProduceTime.MonthCalendar.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.colProduceTime.MonthCalendar.CalendarDimensions = new System.Drawing.Size(1, 1);
            // 
            // 
            // 
            this.colProduceTime.MonthCalendar.CommandsBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.colProduceTime.MonthCalendar.DisplayMonth = new System.DateTime(2016, 11, 1, 0, 0, 0, 0);
            this.colProduceTime.MonthCalendar.MarkedDates = new System.DateTime[0];
            this.colProduceTime.MonthCalendar.MonthlyMarkedDates = new System.DateTime[0];
            // 
            // 
            // 
            this.colProduceTime.MonthCalendar.NavigationBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.colProduceTime.MonthCalendar.WeeklyMarkedDays = new System.DayOfWeek[0];
            this.colProduceTime.Name = "colProduceTime";
            this.colProduceTime.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // colRemark
            // 
            this.colRemark.Frozen = true;
            this.colRemark.HeaderText = "备注说明";
            this.colRemark.Name = "colRemark";
            // 
            // colSourceDataName
            // 
            this.colSourceDataName.Frozen = true;
            this.colSourceDataName.HeaderText = "源数据名称";
            this.colSourceDataName.Name = "colSourceDataName";
            this.colSourceDataName.Width = 180;
            // 
            // colProdType
            // 
            this.colProdType.Frozen = true;
            this.colProdType.HeaderText = "产品类型";
            this.colProdType.Name = "colProdType";
            this.colProdType.Width = 80;
            // 
            // colSize
            // 
            // 
            // 
            // 
            this.colSize.BackgroundStyle.Class = "DataGridViewNumericBorder";
            this.colSize.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.colSize.HeaderText = "大小";
            this.colSize.Increment = 1D;
            this.colSize.Name = "colSize";
            this.colSize.ReadOnly = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.全选ToolStripMenuItem,
            this.全不选ToolStripMenuItem,
            this.还原初始状态ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(149, 70);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // 全选ToolStripMenuItem
            // 
            this.全选ToolStripMenuItem.Name = "全选ToolStripMenuItem";
            this.全选ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.全选ToolStripMenuItem.Text = "全选";
            this.全选ToolStripMenuItem.Click += new System.EventHandler(this.全选ToolStripMenuItem_Click);
            // 
            // 全不选ToolStripMenuItem
            // 
            this.全不选ToolStripMenuItem.Name = "全不选ToolStripMenuItem";
            this.全不选ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.全不选ToolStripMenuItem.Text = "全不选";
            this.全不选ToolStripMenuItem.Click += new System.EventHandler(this.全不选ToolStripMenuItem_Click);
            // 
            // 还原初始状态ToolStripMenuItem
            // 
            this.还原初始状态ToolStripMenuItem.Name = "还原初始状态ToolStripMenuItem";
            this.还原初始状态ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.还原初始状态ToolStripMenuItem.Text = "还原初始状态";
            this.还原初始状态ToolStripMenuItem.Click += new System.EventHandler(this.还原初始状态ToolStripMenuItem_Click);
            // 
            // btn_ImportData
            // 
            this.btn_ImportData.Location = new System.Drawing.Point(323, 374);
            this.btn_ImportData.Name = "btn_ImportData";
            this.btn_ImportData.Size = new System.Drawing.Size(245, 23);
            this.btn_ImportData.TabIndex = 3;
            this.btn_ImportData.Text = "执行入库";
            this.btn_ImportData.UseVisualStyleBackColor = true;
            this.btn_ImportData.Click += new System.EventHandler(this.btn_ImportData_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(317, 12);
            this.label1.TabIndex = 16;
            this.label1.Text = "请选择产品文件夹（单景）或其所在的上级目录（批量）：";
            // 
            // buttonOpenFolder
            // 
            this.buttonOpenFolder.Location = new System.Drawing.Point(752, 38);
            this.buttonOpenFolder.Name = "buttonOpenFolder";
            this.buttonOpenFolder.Size = new System.Drawing.Size(75, 23);
            this.buttonOpenFolder.TabIndex = 15;
            this.buttonOpenFolder.Text = "...";
            this.buttonOpenFolder.UseVisualStyleBackColor = true;
            this.buttonOpenFolder.Click += new System.EventHandler(this.buttonOpenFolder_Click);
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(14, 38);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(732, 21);
            this.textBox1.TabIndex = 14;
            // 
            // ctrlRasterProdImportor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonOpenFolder);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btn_ImportData);
            this.Controls.Add(this.dataGridViewX1);
            this.Controls.Add(this.richTextBox1);
            this.Name = "ctrlRasterProdImportor";
            this.Size = new System.Drawing.Size(1101, 405);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewX1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private DevComponents.DotNetBar.Controls.DataGridViewX dataGridViewX1;
        private System.Windows.Forms.Button btn_ImportData;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonOpenFolder;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colCheck;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProdDir;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProducor;
        private DevComponents.DotNetBar.Controls.DataGridViewDateTimeInputColumn colProduceTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRemark;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSourceDataName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProdType;
        private DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn colSize;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 全选ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 全不选ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 还原初始状态ToolStripMenuItem;
    }
}
