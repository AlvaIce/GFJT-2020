namespace QRST.WorldGlobeTool.PluginEngine
{
    partial class PluginDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PluginDialog));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonUnLoad = new System.Windows.Forms.Button();
            this.listView = new QRST.WorldGlobeTool.PluginEngine.PluginListView();
            this.columnHeaderAvailableplugins = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderStartup = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonLoad = new System.Windows.Forms.Button();
            this.buttonUnInstall = new System.Windows.Forms.Button();
            this.buttonInstall = new System.Windows.Forms.Button();
            this.labelDescription = new System.Windows.Forms.Label();
            this.description = new System.Windows.Forms.TextBox();
            this.labelDeveloper = new System.Windows.Forms.Label();
            this.developer = new System.Windows.Forms.Label();
            this.labelWebSite = new System.Windows.Forms.Label();
            this.webSite = new System.Windows.Forms.LinkLabel();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel1.Controls.Add(this.buttonUnLoad, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.listView, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonLoad, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonUnInstall, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.buttonInstall, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.labelDescription, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.description, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.labelDeveloper, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.developer, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.labelWebSite, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.webSite, 0, 10);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(6, 6);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 11;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(434, 461);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // buttonUnLoad
            // 
            this.buttonUnLoad.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonUnLoad.Location = new System.Drawing.Point(357, 48);
            this.buttonUnLoad.Name = "buttonUnLoad";
            this.buttonUnLoad.Size = new System.Drawing.Size(74, 23);
            this.buttonUnLoad.TabIndex = 2;
            this.buttonUnLoad.Text = "卸载";
            this.buttonUnLoad.UseVisualStyleBackColor = true;
            this.buttonUnLoad.Click += new System.EventHandler(this.buttonUnLoad_Click);
            // 
            // listView
            // 
            this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderAvailableplugins,
            this.columnHeaderStartup});
            this.listView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView.FullRowSelect = true;
            this.listView.GridLines = true;
            this.listView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listView.Location = new System.Drawing.Point(3, 3);
            this.listView.Name = "listView";
            this.tableLayoutPanel1.SetRowSpan(this.listView, 5);
            this.listView.Size = new System.Drawing.Size(348, 194);
            this.listView.TabIndex = 0;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Details;
            this.listView.SelectedIndexChanged += new System.EventHandler(this.listView_SelectedIndexChanged);
            this.listView.DoubleClick += new System.EventHandler(this.listView_DoubleClick);
            // 
            // columnHeaderAvailableplugins
            // 
            this.columnHeaderAvailableplugins.Text = "可用插件";
            this.columnHeaderAvailableplugins.Width = 180;
            // 
            // columnHeaderStartup
            // 
            this.columnHeaderStartup.Text = "启动";
            // 
            // buttonLoad
            // 
            this.buttonLoad.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonLoad.Location = new System.Drawing.Point(357, 8);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(74, 23);
            this.buttonLoad.TabIndex = 1;
            this.buttonLoad.Text = "加载";
            this.buttonLoad.UseVisualStyleBackColor = true;
            this.buttonLoad.Click += new System.EventHandler(this.buttonLoad_Click);
            // 
            // buttonUnInstall
            // 
            this.buttonUnInstall.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonUnInstall.Location = new System.Drawing.Point(357, 168);
            this.buttonUnInstall.Name = "buttonUnInstall";
            this.buttonUnInstall.Size = new System.Drawing.Size(74, 23);
            this.buttonUnInstall.TabIndex = 4;
            this.buttonUnInstall.Text = "卸载";
            this.buttonUnInstall.UseVisualStyleBackColor = true;
            this.buttonUnInstall.Click += new System.EventHandler(this.buttonUnInstall_Click);
            // 
            // buttonInstall
            // 
            this.buttonInstall.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonInstall.Location = new System.Drawing.Point(357, 128);
            this.buttonInstall.Name = "buttonInstall";
            this.buttonInstall.Size = new System.Drawing.Size(74, 23);
            this.buttonInstall.TabIndex = 3;
            this.buttonInstall.Text = "安装";
            this.buttonInstall.UseVisualStyleBackColor = true;
            this.buttonInstall.Click += new System.EventHandler(this.buttonInstall_Click);
            // 
            // labelDescription
            // 
            this.labelDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelDescription.Location = new System.Drawing.Point(3, 223);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(120, 17);
            this.labelDescription.TabIndex = 6;
            this.labelDescription.Text = "描述:";
            // 
            // description
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.description, 2);
            this.description.Dock = System.Windows.Forms.DockStyle.Fill;
            this.description.Location = new System.Drawing.Point(3, 243);
            this.description.Multiline = true;
            this.description.Name = "description";
            this.description.ReadOnly = true;
            this.description.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.description.Size = new System.Drawing.Size(428, 55);
            this.description.TabIndex = 7;
            // 
            // labelDeveloper
            // 
            this.labelDeveloper.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelDeveloper.Location = new System.Drawing.Point(3, 323);
            this.labelDeveloper.Name = "labelDeveloper";
            this.labelDeveloper.Size = new System.Drawing.Size(74, 18);
            this.labelDeveloper.TabIndex = 8;
            this.labelDeveloper.Text = "开发者:";
            // 
            // developer
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.developer, 2);
            this.developer.Dock = System.Windows.Forms.DockStyle.Top;
            this.developer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.developer.Location = new System.Drawing.Point(3, 341);
            this.developer.Name = "developer";
            this.developer.Size = new System.Drawing.Size(428, 17);
            this.developer.TabIndex = 9;
            // 
            // labelWebSite
            // 
            this.labelWebSite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelWebSite.Location = new System.Drawing.Point(3, 404);
            this.labelWebSite.Name = "labelWebSite";
            this.labelWebSite.Size = new System.Drawing.Size(67, 17);
            this.labelWebSite.TabIndex = 10;
            this.labelWebSite.Text = "网址:";
            // 
            // webSite
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.webSite, 2);
            this.webSite.Dock = System.Windows.Forms.DockStyle.Top;
            this.webSite.Location = new System.Drawing.Point(3, 421);
            this.webSite.Name = "webSite";
            this.webSite.Size = new System.Drawing.Size(428, 17);
            this.webSite.TabIndex = 11;
            this.webSite.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.webSite_LinkClicked);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "开.png");
            this.imageList.Images.SetKeyName(1, "关.png");
            // 
            // PluginDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(446, 473);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PluginDialog";
            this.Padding = new System.Windows.Forms.Padding(6);
            this.Text = "加载/卸载插件";
            this.Load += new System.EventHandler(this.PluginDialog_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ImageList imageList;
        private PluginListView listView;
        private System.Windows.Forms.Button buttonLoad;
        private System.Windows.Forms.Button buttonUnLoad;
        private System.Windows.Forms.Button buttonUnInstall;
        private System.Windows.Forms.Button buttonInstall;
        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.TextBox description;
        private System.Windows.Forms.Label labelDeveloper;
        private System.Windows.Forms.Label developer;
        private System.Windows.Forms.Label labelWebSite;
        private System.Windows.Forms.LinkLabel webSite;
        private System.Windows.Forms.ColumnHeader columnHeaderAvailableplugins;
        private System.Windows.Forms.ColumnHeader columnHeaderStartup;
    }
}