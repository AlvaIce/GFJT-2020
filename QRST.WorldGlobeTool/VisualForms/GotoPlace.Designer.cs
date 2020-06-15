namespace QRST.WorldGlobeTool.VisualForms
{
    partial class GotoPlace
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GotoPlace));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDownLLLon = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownLLLat = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownLLAltitude = new System.Windows.Forms.NumericUpDown();
            this.buttonLLGoto = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonRegionGoto = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBoxProvince = new System.Windows.Forms.ComboBox();
            this.comboBoxCity = new System.Windows.Forms.ComboBox();
            this.comboBoxCounty = new System.Windows.Forms.ComboBox();
            this.numericUpDownRegionAltitude = new System.Windows.Forms.NumericUpDown();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonBlur = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.numericUpDownBlur = new System.Windows.Forms.NumericUpDown();
            this.textBoxBlur = new System.Windows.Forms.TextBox();
            this.dataGridViewBlur = new System.Windows.Forms.DataGridView();
            this.ColumnRegionName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnLat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnLon = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLLLon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLLLat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLLAltitude)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRegionAltitude)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBlur)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBlur)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(6, 6);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(469, 273);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tableLayoutPanel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(6);
            this.tabPage1.Size = new System.Drawing.Size(461, 247);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "经纬度定位";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.numericUpDownLLLon, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.numericUpDownLLLat, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.numericUpDownLLAltitude, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.buttonLLGoto, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(6, 6);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(449, 235);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "经  度(°)：";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 111);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "纬  度(°)：";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 189);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "海拔高度(m)：";
            // 
            // numericUpDownLLLon
            // 
            this.numericUpDownLLLon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownLLLon.DecimalPlaces = 4;
            this.numericUpDownLLLon.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDownLLLon.Location = new System.Drawing.Point(103, 28);
            this.numericUpDownLLLon.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.numericUpDownLLLon.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
            this.numericUpDownLLLon.Name = "numericUpDownLLLon";
            this.numericUpDownLLLon.Size = new System.Drawing.Size(168, 21);
            this.numericUpDownLLLon.TabIndex = 3;
            this.numericUpDownLLLon.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // numericUpDownLLLat
            // 
            this.numericUpDownLLLat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownLLLat.DecimalPlaces = 4;
            this.numericUpDownLLLat.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDownLLLat.Location = new System.Drawing.Point(103, 106);
            this.numericUpDownLLLat.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.numericUpDownLLLat.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
            this.numericUpDownLLLat.Name = "numericUpDownLLLat";
            this.numericUpDownLLLat.Size = new System.Drawing.Size(168, 21);
            this.numericUpDownLLLat.TabIndex = 4;
            this.numericUpDownLLLat.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // numericUpDownLLAltitude
            // 
            this.numericUpDownLLAltitude.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownLLAltitude.DecimalPlaces = 1;
            this.numericUpDownLLAltitude.Location = new System.Drawing.Point(103, 185);
            this.numericUpDownLLAltitude.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.numericUpDownLLAltitude.Name = "numericUpDownLLAltitude";
            this.numericUpDownLLAltitude.Size = new System.Drawing.Size(168, 21);
            this.numericUpDownLLAltitude.TabIndex = 5;
            this.numericUpDownLLAltitude.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDownLLAltitude.Value = new decimal(new int[] {
            2500,
            0,
            0,
            0});
            // 
            // buttonLLGoto
            // 
            this.buttonLLGoto.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonLLGoto.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonLLGoto.Location = new System.Drawing.Point(310, 27);
            this.buttonLLGoto.Name = "buttonLLGoto";
            this.tableLayoutPanel1.SetRowSpan(this.buttonLLGoto, 3);
            this.buttonLLGoto.Size = new System.Drawing.Size(102, 180);
            this.buttonLLGoto.TabIndex = 6;
            this.buttonLLGoto.Text = "定位";
            this.buttonLLGoto.UseVisualStyleBackColor = true;
            this.buttonLLGoto.Click += new System.EventHandler(this.buttonLLGoto_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.tableLayoutPanel2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(461, 247);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "行政区定位";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.buttonRegionGoto, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.label7, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label5, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.label6, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.comboBoxProvince, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.comboBoxCity, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.comboBoxCounty, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.numericUpDownRegionAltitude, 1, 3);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(455, 241);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // buttonRegionGoto
            // 
            this.buttonRegionGoto.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonRegionGoto.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonRegionGoto.Location = new System.Drawing.Point(315, 30);
            this.buttonRegionGoto.Name = "buttonRegionGoto";
            this.tableLayoutPanel2.SetRowSpan(this.buttonRegionGoto, 4);
            this.buttonRegionGoto.Size = new System.Drawing.Size(102, 180);
            this.buttonRegionGoto.TabIndex = 9;
            this.buttonRegionGoto.Text = "定位";
            this.buttonRegionGoto.UseVisualStyleBackColor = true;
            this.buttonRegionGoto.Click += new System.EventHandler(this.buttonRegionGoto_Click);
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(14, 204);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(83, 12);
            this.label7.TabIndex = 4;
            this.label7.Text = "海拔高度(m)：";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(68, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 1;
            this.label4.Text = "省：";
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(68, 84);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 2;
            this.label5.Text = "市：";
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(50, 144);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 12);
            this.label6.TabIndex = 3;
            this.label6.Text = "区/县：";
            // 
            // comboBoxProvince
            // 
            this.comboBoxProvince.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxProvince.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxProvince.FormattingEnabled = true;
            this.comboBoxProvince.Location = new System.Drawing.Point(103, 20);
            this.comboBoxProvince.Name = "comboBoxProvince";
            this.comboBoxProvince.Size = new System.Drawing.Size(171, 20);
            this.comboBoxProvince.TabIndex = 5;
            this.comboBoxProvince.SelectedIndexChanged += new System.EventHandler(this.comboBoxProvince_SelectedIndexChanged);
            // 
            // comboBoxCity
            // 
            this.comboBoxCity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxCity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCity.FormattingEnabled = true;
            this.comboBoxCity.Location = new System.Drawing.Point(103, 80);
            this.comboBoxCity.Name = "comboBoxCity";
            this.comboBoxCity.Size = new System.Drawing.Size(171, 20);
            this.comboBoxCity.TabIndex = 6;
            this.comboBoxCity.SelectedIndexChanged += new System.EventHandler(this.comboBoxCity_SelectedIndexChanged);
            // 
            // comboBoxCounty
            // 
            this.comboBoxCounty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxCounty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCounty.FormattingEnabled = true;
            this.comboBoxCounty.Location = new System.Drawing.Point(103, 140);
            this.comboBoxCounty.Name = "comboBoxCounty";
            this.comboBoxCounty.Size = new System.Drawing.Size(171, 20);
            this.comboBoxCounty.TabIndex = 7;
            this.comboBoxCounty.SelectedIndexChanged += new System.EventHandler(this.comboBoxCounty_SelectedIndexChanged);
            // 
            // numericUpDownRegionAltitude
            // 
            this.numericUpDownRegionAltitude.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownRegionAltitude.DecimalPlaces = 1;
            this.numericUpDownRegionAltitude.Location = new System.Drawing.Point(103, 200);
            this.numericUpDownRegionAltitude.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.numericUpDownRegionAltitude.Name = "numericUpDownRegionAltitude";
            this.numericUpDownRegionAltitude.Size = new System.Drawing.Size(171, 21);
            this.numericUpDownRegionAltitude.TabIndex = 8;
            this.numericUpDownRegionAltitude.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDownRegionAltitude.Value = new decimal(new int[] {
            2500,
            0,
            0,
            0});
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.tableLayoutPanel3);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(461, 247);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "模糊搜索";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.buttonBlur, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.label9, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.label8, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.numericUpDownBlur, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.textBoxBlur, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.dataGridViewBlur, 0, 2);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(455, 241);
            this.tableLayoutPanel3.TabIndex = 1;
            // 
            // buttonBlur
            // 
            this.buttonBlur.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonBlur.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonBlur.Location = new System.Drawing.Point(315, 30);
            this.buttonBlur.Name = "buttonBlur";
            this.tableLayoutPanel3.SetRowSpan(this.buttonBlur, 3);
            this.buttonBlur.Size = new System.Drawing.Size(102, 180);
            this.buttonBlur.TabIndex = 9;
            this.buttonBlur.Text = "定位";
            this.buttonBlur.UseVisualStyleBackColor = true;
            this.buttonBlur.Click += new System.EventHandler(this.buttonBlur_Click);
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(14, 54);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(83, 12);
            this.label8.TabIndex = 4;
            this.label8.Text = "海拔高度(m)：";
            // 
            // label9
            // 
            this.label9.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(8, 14);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(89, 12);
            this.label9.TabIndex = 1;
            this.label9.Text = "行政区域名称：";
            // 
            // numericUpDownBlur
            // 
            this.numericUpDownBlur.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownBlur.DecimalPlaces = 1;
            this.numericUpDownBlur.Location = new System.Drawing.Point(103, 49);
            this.numericUpDownBlur.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.numericUpDownBlur.Name = "numericUpDownBlur";
            this.numericUpDownBlur.Size = new System.Drawing.Size(171, 21);
            this.numericUpDownBlur.TabIndex = 8;
            this.numericUpDownBlur.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDownBlur.Value = new decimal(new int[] {
            2500,
            0,
            0,
            0});
            // 
            // textBoxBlur
            // 
            this.textBoxBlur.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxBlur.Location = new System.Drawing.Point(103, 9);
            this.textBoxBlur.Name = "textBoxBlur";
            this.textBoxBlur.Size = new System.Drawing.Size(171, 21);
            this.textBoxBlur.TabIndex = 10;
            this.toolTip1.SetToolTip(this.textBoxBlur, "输入检索区域名称并按下\"Enter\"进行搜索");
            this.textBoxBlur.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBoxBlur_KeyUp);
            // 
            // dataGridViewBlur
            // 
            this.dataGridViewBlur.AllowUserToAddRows = false;
            this.dataGridViewBlur.AllowUserToDeleteRows = false;
            this.dataGridViewBlur.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewBlur.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewBlur.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnRegionName,
            this.ColumnLat,
            this.ColumnLon});
            this.tableLayoutPanel3.SetColumnSpan(this.dataGridViewBlur, 2);
            this.dataGridViewBlur.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewBlur.Location = new System.Drawing.Point(3, 83);
            this.dataGridViewBlur.MultiSelect = false;
            this.dataGridViewBlur.Name = "dataGridViewBlur";
            this.dataGridViewBlur.ReadOnly = true;
            this.dataGridViewBlur.RowTemplate.Height = 23;
            this.dataGridViewBlur.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewBlur.Size = new System.Drawing.Size(271, 155);
            this.dataGridViewBlur.TabIndex = 11;
            this.toolTip1.SetToolTip(this.dataGridViewBlur, "选择任意一行所表示的区域进行定位");
            // 
            // ColumnRegionName
            // 
            this.ColumnRegionName.HeaderText = "区域名称";
            this.ColumnRegionName.Name = "ColumnRegionName";
            this.ColumnRegionName.ReadOnly = true;
            // 
            // ColumnLat
            // 
            this.ColumnLat.HeaderText = "纬度";
            this.ColumnLat.Name = "ColumnLat";
            this.ColumnLat.ReadOnly = true;
            // 
            // ColumnLon
            // 
            this.ColumnLon.HeaderText = "经度";
            this.ColumnLon.Name = "ColumnLon";
            this.ColumnLon.ReadOnly = true;
            // 
            // GotoPlace
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(481, 285);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "GotoPlace";
            this.Padding = new System.Windows.Forms.Padding(6);
            this.Text = "定位";
            this.Load += new System.EventHandler(this.GotoPlace_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLLLon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLLLat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLLAltitude)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRegionAltitude)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBlur)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBlur)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numericUpDownLLLon;
        private System.Windows.Forms.NumericUpDown numericUpDownLLLat;
        private System.Windows.Forms.NumericUpDown numericUpDownLLAltitude;
        private System.Windows.Forms.Button buttonLLGoto;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBoxProvince;
        private System.Windows.Forms.Button buttonRegionGoto;
        private System.Windows.Forms.ComboBox comboBoxCity;
        private System.Windows.Forms.ComboBox comboBoxCounty;
        private System.Windows.Forms.NumericUpDown numericUpDownRegionAltitude;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button buttonBlur;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown numericUpDownBlur;
        private System.Windows.Forms.TextBox textBoxBlur;
        private System.Windows.Forms.DataGridView dataGridViewBlur;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnRegionName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnLat;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnLon;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}