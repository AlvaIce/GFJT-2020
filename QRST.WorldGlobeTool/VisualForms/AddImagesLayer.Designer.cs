namespace QRST.WorldGlobeTool.VisualForms
{
    partial class AddImagesLayer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddImagesLayer));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxJPGOrPNGLayerName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxJPGOrPNGFilePath = new System.Windows.Forms.TextBox();
            this.buttonSelectJPGOrPNG = new System.Windows.Forms.Button();
            this.ucGeoBoundingBoxJPGOrPNG = new QRST.WorldGlobeTool.VisualForms.UCGeoBoundingBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxJPGOrPNGProjection = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.numericUpDownJPGOrPNGBackvalue = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.numericUpDownJPGOrPNGOpacity = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonJPGOrPNGAdd = new System.Windows.Forms.Button();
            this.buttonJPGOrPNGCancel = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxGeoTiffLayerName = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxGeoTiffFilePath = new System.Windows.Forms.TextBox();
            this.buttonSelectGeoTiff = new System.Windows.Forms.Button();
            this.ucGeoBoundingBoxGeoTiff = new QRST.WorldGlobeTool.VisualForms.UCGeoBoundingBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textBoxGeoTiffProjection = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.numericUpDownGeoTiffBackvalue = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.numericUpDownGeoTiffOpacity = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.buttonGeoTiffAdd = new System.Windows.Forms.Button();
            this.buttonGeoTiffCancel = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.radioButtonGeoTiffWhole = new System.Windows.Forms.RadioButton();
            this.radioButtonGeoTiffTile = new System.Windows.Forms.RadioButton();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.numericUpDownGeoTiffLevelZeroDegree = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownGeoTiffPyramidLevelCount = new System.Windows.Forms.NumericUpDown();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownJPGOrPNGBackvalue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownJPGOrPNGOpacity)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownGeoTiffBackvalue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownGeoTiffOpacity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownGeoTiffLevelZeroDegree)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownGeoTiffPyramidLevelCount)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(6, 6);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(618, 495);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tableLayoutPanel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(6);
            this.tabPage1.Size = new System.Drawing.Size(610, 469);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "JPG/PNG图层";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 62F));
            this.tableLayoutPanel1.Controls.Add(this.textBoxJPGOrPNGLayerName, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.textBoxJPGOrPNGFilePath, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonSelectJPGOrPNG, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.ucGeoBoundingBoxJPGOrPNG, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.textBoxJPGOrPNGProjection, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.numericUpDownJPGOrPNGBackvalue, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.label6, 2, 6);
            this.tableLayoutPanel1.Controls.Add(this.numericUpDownJPGOrPNGOpacity, 3, 6);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.buttonJPGOrPNGAdd, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.buttonJPGOrPNGCancel, 3, 7);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(6, 6);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 8;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(598, 457);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // textBoxJPGOrPNGLayerName
            // 
            this.textBoxJPGOrPNGLayerName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.textBoxJPGOrPNGLayerName, 3);
            this.textBoxJPGOrPNGLayerName.Location = new System.Drawing.Point(74, 49);
            this.textBoxJPGOrPNGLayerName.Name = "textBoxJPGOrPNGLayerName";
            this.textBoxJPGOrPNGLayerName.Size = new System.Drawing.Size(456, 21);
            this.textBoxJPGOrPNGLayerName.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "图层名称：";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "选择文件：";
            // 
            // textBoxJPGOrPNGFilePath
            // 
            this.textBoxJPGOrPNGFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.textBoxJPGOrPNGFilePath, 3);
            this.textBoxJPGOrPNGFilePath.Location = new System.Drawing.Point(74, 9);
            this.textBoxJPGOrPNGFilePath.Name = "textBoxJPGOrPNGFilePath";
            this.textBoxJPGOrPNGFilePath.Size = new System.Drawing.Size(456, 21);
            this.textBoxJPGOrPNGFilePath.TabIndex = 1;
            // 
            // buttonSelectJPGOrPNG
            // 
            this.buttonSelectJPGOrPNG.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.buttonSelectJPGOrPNG.Location = new System.Drawing.Point(536, 6);
            this.buttonSelectJPGOrPNG.Name = "buttonSelectJPGOrPNG";
            this.buttonSelectJPGOrPNG.Size = new System.Drawing.Size(56, 27);
            this.buttonSelectJPGOrPNG.TabIndex = 2;
            this.buttonSelectJPGOrPNG.Text = "浏览...";
            this.buttonSelectJPGOrPNG.UseVisualStyleBackColor = true;
            this.buttonSelectJPGOrPNG.Click += new System.EventHandler(this.buttonSelectJPGOrPNG_Click);
            // 
            // ucGeoBoundingBoxJPGOrPNG
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.ucGeoBoundingBoxJPGOrPNG, 3);
            this.ucGeoBoundingBoxJPGOrPNG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucGeoBoundingBoxJPGOrPNG.Location = new System.Drawing.Point(74, 93);
            this.ucGeoBoundingBoxJPGOrPNG.Name = "ucGeoBoundingBoxJPGOrPNG";
            this.ucGeoBoundingBoxJPGOrPNG.Size = new System.Drawing.Size(456, 231);
            this.ucGeoBoundingBoxJPGOrPNG.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 351);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "投影信息：";
            // 
            // textBoxJPGOrPNGProjection
            // 
            this.textBoxJPGOrPNGProjection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.textBoxJPGOrPNGProjection, 3);
            this.textBoxJPGOrPNGProjection.Location = new System.Drawing.Point(74, 346);
            this.textBoxJPGOrPNGProjection.Name = "textBoxJPGOrPNGProjection";
            this.textBoxJPGOrPNGProjection.Size = new System.Drawing.Size(456, 21);
            this.textBoxJPGOrPNGProjection.TabIndex = 8;
            this.textBoxJPGOrPNGProjection.Text = "GEOGCS_WGS_1984";
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 391);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "背 景 值：";
            // 
            // numericUpDownJPGOrPNGBackvalue
            // 
            this.numericUpDownJPGOrPNGBackvalue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownJPGOrPNGBackvalue.Location = new System.Drawing.Point(74, 386);
            this.numericUpDownJPGOrPNGBackvalue.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownJPGOrPNGBackvalue.Name = "numericUpDownJPGOrPNGBackvalue";
            this.numericUpDownJPGOrPNGBackvalue.Size = new System.Drawing.Size(148, 21);
            this.numericUpDownJPGOrPNGBackvalue.TabIndex = 11;
            this.numericUpDownJPGOrPNGBackvalue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(311, 391);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 10;
            this.label6.Text = "透 明 度：";
            // 
            // numericUpDownJPGOrPNGOpacity
            // 
            this.numericUpDownJPGOrPNGOpacity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownJPGOrPNGOpacity.Location = new System.Drawing.Point(382, 386);
            this.numericUpDownJPGOrPNGOpacity.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownJPGOrPNGOpacity.Name = "numericUpDownJPGOrPNGOpacity";
            this.numericUpDownJPGOrPNGOpacity.Size = new System.Drawing.Size(148, 21);
            this.numericUpDownJPGOrPNGOpacity.TabIndex = 12;
            this.numericUpDownJPGOrPNGOpacity.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDownJPGOrPNGOpacity.Value = new decimal(new int[] {
            255,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 202);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "空间参考：";
            // 
            // buttonJPGOrPNGAdd
            // 
            this.buttonJPGOrPNGAdd.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonJPGOrPNGAdd.Location = new System.Drawing.Point(110, 423);
            this.buttonJPGOrPNGAdd.Name = "buttonJPGOrPNGAdd";
            this.buttonJPGOrPNGAdd.Size = new System.Drawing.Size(75, 27);
            this.buttonJPGOrPNGAdd.TabIndex = 13;
            this.buttonJPGOrPNGAdd.Text = "添加";
            this.buttonJPGOrPNGAdd.UseVisualStyleBackColor = true;
            this.buttonJPGOrPNGAdd.Click += new System.EventHandler(this.buttonJPGOrPNGAdd_Click);
            // 
            // buttonJPGOrPNGCancel
            // 
            this.buttonJPGOrPNGCancel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonJPGOrPNGCancel.Location = new System.Drawing.Point(418, 423);
            this.buttonJPGOrPNGCancel.Name = "buttonJPGOrPNGCancel";
            this.buttonJPGOrPNGCancel.Size = new System.Drawing.Size(75, 27);
            this.buttonJPGOrPNGCancel.TabIndex = 14;
            this.buttonJPGOrPNGCancel.Text = "取消";
            this.buttonJPGOrPNGCancel.UseVisualStyleBackColor = true;
            this.buttonJPGOrPNGCancel.Click += new System.EventHandler(this.buttonJPGOrPNGCancel_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.tableLayoutPanel2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(6);
            this.tabPage2.Size = new System.Drawing.Size(610, 469);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "GeoTiff图层";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 5;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 62F));
            this.tableLayoutPanel2.Controls.Add(this.textBoxGeoTiffLayerName, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.label7, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.label8, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.textBoxGeoTiffFilePath, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.buttonSelectGeoTiff, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.ucGeoBoundingBoxGeoTiff, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.label9, 0, 7);
            this.tableLayoutPanel2.Controls.Add(this.textBoxGeoTiffProjection, 1, 7);
            this.tableLayoutPanel2.Controls.Add(this.label10, 0, 8);
            this.tableLayoutPanel2.Controls.Add(this.numericUpDownGeoTiffBackvalue, 1, 8);
            this.tableLayoutPanel2.Controls.Add(this.label11, 2, 8);
            this.tableLayoutPanel2.Controls.Add(this.numericUpDownGeoTiffOpacity, 3, 8);
            this.tableLayoutPanel2.Controls.Add(this.label12, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.buttonGeoTiffAdd, 1, 9);
            this.tableLayoutPanel2.Controls.Add(this.buttonGeoTiffCancel, 3, 9);
            this.tableLayoutPanel2.Controls.Add(this.label13, 0, 5);
            this.tableLayoutPanel2.Controls.Add(this.radioButtonGeoTiffWhole, 1, 5);
            this.tableLayoutPanel2.Controls.Add(this.radioButtonGeoTiffTile, 2, 5);
            this.tableLayoutPanel2.Controls.Add(this.label14, 0, 6);
            this.tableLayoutPanel2.Controls.Add(this.label15, 2, 6);
            this.tableLayoutPanel2.Controls.Add(this.numericUpDownGeoTiffLevelZeroDegree, 1, 6);
            this.tableLayoutPanel2.Controls.Add(this.numericUpDownGeoTiffPyramidLevelCount, 3, 6);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(6, 6);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 10;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(598, 457);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // textBoxGeoTiffLayerName
            // 
            this.textBoxGeoTiffLayerName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.SetColumnSpan(this.textBoxGeoTiffLayerName, 3);
            this.textBoxGeoTiffLayerName.Location = new System.Drawing.Point(74, 49);
            this.textBoxGeoTiffLayerName.Name = "textBoxGeoTiffLayerName";
            this.textBoxGeoTiffLayerName.Size = new System.Drawing.Size(456, 21);
            this.textBoxGeoTiffLayerName.TabIndex = 4;
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 54);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 3;
            this.label7.Text = "图层名称：";
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 14);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 12);
            this.label8.TabIndex = 0;
            this.label8.Text = "选择文件：";
            // 
            // textBoxGeoTiffFilePath
            // 
            this.textBoxGeoTiffFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.SetColumnSpan(this.textBoxGeoTiffFilePath, 3);
            this.textBoxGeoTiffFilePath.Location = new System.Drawing.Point(74, 9);
            this.textBoxGeoTiffFilePath.Name = "textBoxGeoTiffFilePath";
            this.textBoxGeoTiffFilePath.Size = new System.Drawing.Size(456, 21);
            this.textBoxGeoTiffFilePath.TabIndex = 1;
            // 
            // buttonSelectGeoTiff
            // 
            this.buttonSelectGeoTiff.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.buttonSelectGeoTiff.Location = new System.Drawing.Point(536, 6);
            this.buttonSelectGeoTiff.Name = "buttonSelectGeoTiff";
            this.buttonSelectGeoTiff.Size = new System.Drawing.Size(56, 27);
            this.buttonSelectGeoTiff.TabIndex = 2;
            this.buttonSelectGeoTiff.Text = "浏览...";
            this.buttonSelectGeoTiff.UseVisualStyleBackColor = true;
            this.buttonSelectGeoTiff.Click += new System.EventHandler(this.buttonSelectGeoTiff_Click);
            // 
            // ucGeoBoundingBoxGeoTiff
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.ucGeoBoundingBoxGeoTiff, 3);
            this.ucGeoBoundingBoxGeoTiff.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucGeoBoundingBoxGeoTiff.Location = new System.Drawing.Point(74, 93);
            this.ucGeoBoundingBoxGeoTiff.Name = "ucGeoBoundingBoxGeoTiff";
            this.ucGeoBoundingBoxGeoTiff.Size = new System.Drawing.Size(456, 151);
            this.ucGeoBoundingBoxGeoTiff.TabIndex = 6;
            // 
            // label9
            // 
            this.label9.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 351);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 12);
            this.label9.TabIndex = 7;
            this.label9.Text = "投影信息：";
            // 
            // textBoxGeoTiffProjection
            // 
            this.textBoxGeoTiffProjection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.SetColumnSpan(this.textBoxGeoTiffProjection, 3);
            this.textBoxGeoTiffProjection.Location = new System.Drawing.Point(74, 346);
            this.textBoxGeoTiffProjection.Name = "textBoxGeoTiffProjection";
            this.textBoxGeoTiffProjection.Size = new System.Drawing.Size(456, 21);
            this.textBoxGeoTiffProjection.TabIndex = 8;
            this.textBoxGeoTiffProjection.Text = "GEOGCS_WGS_1984";
            // 
            // label10
            // 
            this.label10.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 391);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(65, 12);
            this.label10.TabIndex = 9;
            this.label10.Text = "背 景 值：";
            // 
            // numericUpDownGeoTiffBackvalue
            // 
            this.numericUpDownGeoTiffBackvalue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownGeoTiffBackvalue.Location = new System.Drawing.Point(74, 386);
            this.numericUpDownGeoTiffBackvalue.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownGeoTiffBackvalue.Name = "numericUpDownGeoTiffBackvalue";
            this.numericUpDownGeoTiffBackvalue.Size = new System.Drawing.Size(148, 21);
            this.numericUpDownGeoTiffBackvalue.TabIndex = 11;
            this.numericUpDownGeoTiffBackvalue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label11
            // 
            this.label11.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(311, 391);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(65, 12);
            this.label11.TabIndex = 10;
            this.label11.Text = "透 明 度：";
            // 
            // numericUpDownGeoTiffOpacity
            // 
            this.numericUpDownGeoTiffOpacity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownGeoTiffOpacity.Location = new System.Drawing.Point(382, 386);
            this.numericUpDownGeoTiffOpacity.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownGeoTiffOpacity.Name = "numericUpDownGeoTiffOpacity";
            this.numericUpDownGeoTiffOpacity.Size = new System.Drawing.Size(148, 21);
            this.numericUpDownGeoTiffOpacity.TabIndex = 12;
            this.numericUpDownGeoTiffOpacity.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDownGeoTiffOpacity.Value = new decimal(new int[] {
            255,
            0,
            0,
            0});
            // 
            // label12
            // 
            this.label12.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(3, 162);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(65, 12);
            this.label12.TabIndex = 5;
            this.label12.Text = "空间参考：";
            // 
            // buttonGeoTiffAdd
            // 
            this.buttonGeoTiffAdd.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonGeoTiffAdd.Location = new System.Drawing.Point(110, 423);
            this.buttonGeoTiffAdd.Name = "buttonGeoTiffAdd";
            this.buttonGeoTiffAdd.Size = new System.Drawing.Size(75, 27);
            this.buttonGeoTiffAdd.TabIndex = 13;
            this.buttonGeoTiffAdd.Text = "添加";
            this.buttonGeoTiffAdd.UseVisualStyleBackColor = true;
            this.buttonGeoTiffAdd.Click += new System.EventHandler(this.buttonGeoTiffAdd_Click);
            // 
            // buttonGeoTiffCancel
            // 
            this.buttonGeoTiffCancel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonGeoTiffCancel.Location = new System.Drawing.Point(418, 423);
            this.buttonGeoTiffCancel.Name = "buttonGeoTiffCancel";
            this.buttonGeoTiffCancel.Size = new System.Drawing.Size(75, 27);
            this.buttonGeoTiffCancel.TabIndex = 14;
            this.buttonGeoTiffCancel.Text = "取消";
            this.buttonGeoTiffCancel.UseVisualStyleBackColor = true;
            this.buttonGeoTiffCancel.Click += new System.EventHandler(this.buttonGeoTiffCancel_Click);
            // 
            // label13
            // 
            this.label13.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(3, 271);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(65, 12);
            this.label13.TabIndex = 15;
            this.label13.Text = "加载方式：";
            // 
            // radioButtonGeoTiffWhole
            // 
            this.radioButtonGeoTiffWhole.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.radioButtonGeoTiffWhole.AutoSize = true;
            this.radioButtonGeoTiffWhole.Checked = true;
            this.radioButtonGeoTiffWhole.Location = new System.Drawing.Point(74, 269);
            this.radioButtonGeoTiffWhole.Name = "radioButtonGeoTiffWhole";
            this.radioButtonGeoTiffWhole.Size = new System.Drawing.Size(71, 16);
            this.radioButtonGeoTiffWhole.TabIndex = 16;
            this.radioButtonGeoTiffWhole.TabStop = true;
            this.radioButtonGeoTiffWhole.Text = "贴图模式";
            this.radioButtonGeoTiffWhole.UseVisualStyleBackColor = true;
            this.radioButtonGeoTiffWhole.CheckedChanged += new System.EventHandler(this.radioButtonGeoTiffWhole_CheckedChanged);
            // 
            // radioButtonGeoTiffTile
            // 
            this.radioButtonGeoTiffTile.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.radioButtonGeoTiffTile.AutoSize = true;
            this.radioButtonGeoTiffTile.Location = new System.Drawing.Point(228, 269);
            this.radioButtonGeoTiffTile.Name = "radioButtonGeoTiffTile";
            this.radioButtonGeoTiffTile.Size = new System.Drawing.Size(71, 16);
            this.radioButtonGeoTiffTile.TabIndex = 17;
            this.radioButtonGeoTiffTile.Text = "切片模式";
            this.radioButtonGeoTiffTile.UseVisualStyleBackColor = true;
            this.radioButtonGeoTiffTile.CheckedChanged += new System.EventHandler(this.radioButtonGeoTiffTile_CheckedChanged);
            // 
            // label14
            // 
            this.label14.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(3, 305);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(65, 24);
            this.label14.TabIndex = 18;
            this.label14.Text = "金字塔0级\r\n跨度(°)：";
            // 
            // label15
            // 
            this.label15.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(299, 311);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(77, 12);
            this.label15.TabIndex = 19;
            this.label15.Text = "金字塔层数：";
            // 
            // numericUpDownGeoTiffLevelZeroDegree
            // 
            this.numericUpDownGeoTiffLevelZeroDegree.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownGeoTiffLevelZeroDegree.DecimalPlaces = 2;
            this.numericUpDownGeoTiffLevelZeroDegree.Location = new System.Drawing.Point(74, 306);
            this.numericUpDownGeoTiffLevelZeroDegree.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.numericUpDownGeoTiffLevelZeroDegree.Name = "numericUpDownGeoTiffLevelZeroDegree";
            this.numericUpDownGeoTiffLevelZeroDegree.Size = new System.Drawing.Size(148, 21);
            this.numericUpDownGeoTiffLevelZeroDegree.TabIndex = 20;
            this.numericUpDownGeoTiffLevelZeroDegree.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // numericUpDownGeoTiffPyramidLevelCount
            // 
            this.numericUpDownGeoTiffPyramidLevelCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownGeoTiffPyramidLevelCount.Location = new System.Drawing.Point(382, 306);
            this.numericUpDownGeoTiffPyramidLevelCount.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.numericUpDownGeoTiffPyramidLevelCount.Name = "numericUpDownGeoTiffPyramidLevelCount";
            this.numericUpDownGeoTiffPyramidLevelCount.Size = new System.Drawing.Size(148, 21);
            this.numericUpDownGeoTiffPyramidLevelCount.TabIndex = 21;
            this.numericUpDownGeoTiffPyramidLevelCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // AddImagesLayer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(630, 507);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AddImagesLayer";
            this.Padding = new System.Windows.Forms.Padding(6);
            this.Text = "添加栅格图层";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownJPGOrPNGBackvalue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownJPGOrPNGOpacity)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownGeoTiffBackvalue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownGeoTiffOpacity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownGeoTiffLevelZeroDegree)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownGeoTiffPyramidLevelCount)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxJPGOrPNGFilePath;
        private System.Windows.Forms.Button buttonSelectJPGOrPNG;
        private System.Windows.Forms.TextBox textBoxJPGOrPNGLayerName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private UCGeoBoundingBox ucGeoBoundingBoxJPGOrPNG;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxJPGOrPNGProjection;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numericUpDownJPGOrPNGBackvalue;
        private System.Windows.Forms.NumericUpDown numericUpDownJPGOrPNGOpacity;
        private System.Windows.Forms.Button buttonJPGOrPNGAdd;
        private System.Windows.Forms.Button buttonJPGOrPNGCancel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TextBox textBoxGeoTiffLayerName;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxGeoTiffFilePath;
        private System.Windows.Forms.Button buttonSelectGeoTiff;
        private UCGeoBoundingBox ucGeoBoundingBoxGeoTiff;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBoxGeoTiffProjection;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown numericUpDownGeoTiffBackvalue;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown numericUpDownGeoTiffOpacity;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button buttonGeoTiffAdd;
        private System.Windows.Forms.Button buttonGeoTiffCancel;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.RadioButton radioButtonGeoTiffWhole;
        private System.Windows.Forms.RadioButton radioButtonGeoTiffTile;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.NumericUpDown numericUpDownGeoTiffLevelZeroDegree;
        private System.Windows.Forms.NumericUpDown numericUpDownGeoTiffPyramidLevelCount;
    }
}