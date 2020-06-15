namespace QRST_DI_MS_Component.Common
{
    partial class FrmDownLoad
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
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxDataPath = new System.Windows.Forms.TextBox();
            this.radio_sourcedata = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.btnChooseBtn = new System.Windows.Forms.Button();
            this.btnStartDownLoad = new System.Windows.Forms.Button();
            this.btnCancle = new System.Windows.Forms.Button();
            this.lblAvailableSpace = new System.Windows.Forms.Label();
            this.radio_correcteddata = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "存放位置";
            // 
            // textBoxDataPath
            // 
            this.textBoxDataPath.Location = new System.Drawing.Point(115, 22);
            this.textBoxDataPath.Name = "textBoxDataPath";
            this.textBoxDataPath.Size = new System.Drawing.Size(274, 21);
            this.textBoxDataPath.TabIndex = 1;
            // 
            // radio_sourcedata
            // 
            this.radio_sourcedata.Checked = true;
            this.radio_sourcedata.Location = new System.Drawing.Point(8, 3);
            this.radio_sourcedata.Name = "radio_sourcedata";
            this.radio_sourcedata.Size = new System.Drawing.Size(132, 28);
            this.radio_sourcedata.TabIndex = 3;
            this.radio_sourcedata.TabStop = true;
            this.radio_sourcedata.Text = "原始数据";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "下载选项";
            // 
            // btnChooseBtn
            // 
            this.btnChooseBtn.Location = new System.Drawing.Point(395, 21);
            this.btnChooseBtn.Name = "btnChooseBtn";
            this.btnChooseBtn.Size = new System.Drawing.Size(60, 20);
            this.btnChooseBtn.TabIndex = 5;
            this.btnChooseBtn.Text = "...";
            this.btnChooseBtn.Click += new System.EventHandler(this.btnChooseBtn_Click);
            // 
            // btnStartDownLoad
            // 
            this.btnStartDownLoad.Location = new System.Drawing.Point(278, 135);
            this.btnStartDownLoad.Name = "btnStartDownLoad";
            this.btnStartDownLoad.Size = new System.Drawing.Size(100, 20);
            this.btnStartDownLoad.TabIndex = 6;
            this.btnStartDownLoad.Text = "开始下载";
            this.btnStartDownLoad.Click += new System.EventHandler(this.btnStartDownLoad_Click);
            // 
            // btnCancle
            // 
            this.btnCancle.Location = new System.Drawing.Point(386, 135);
            this.btnCancle.Name = "btnCancle";
            this.btnCancle.Size = new System.Drawing.Size(64, 20);
            this.btnCancle.TabIndex = 7;
            this.btnCancle.Text = "取消";
            this.btnCancle.Click += new System.EventHandler(this.btnCancle_Click);
            // 
            // lblAvailableSpace
            // 
            this.lblAvailableSpace.Location = new System.Drawing.Point(63, 46);
            this.lblAvailableSpace.Name = "lblAvailableSpace";
            this.lblAvailableSpace.Size = new System.Drawing.Size(0, 12);
            this.lblAvailableSpace.TabIndex = 8;
            // 
            // radio_correcteddata
            // 
            this.radio_correcteddata.AutoSize = true;
            this.radio_correcteddata.Location = new System.Drawing.Point(152, 9);
            this.radio_correcteddata.Name = "radio_correcteddata";
            this.radio_correcteddata.Size = new System.Drawing.Size(119, 16);
            this.radio_correcteddata.TabIndex = 9;
            this.radio_correcteddata.TabStop = true;
            this.radio_correcteddata.Text = "校正数据";
            this.radio_correcteddata.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.radio_sourcedata);
            this.panel1.Controls.Add(this.radio_correcteddata);
            this.panel1.Location = new System.Drawing.Point(111, 64);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(278, 39);
            this.panel1.TabIndex = 10;
            // 
            // FrmDownLoad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(467, 165);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblAvailableSpace);
            this.Controls.Add(this.btnCancle);
            this.Controls.Add(this.btnStartDownLoad);
            this.Controls.Add(this.btnChooseBtn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxDataPath);
            this.Controls.Add(this.label1);
            this.Name = "FrmDownLoad";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "下载选项";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxDataPath;
        private System.Windows.Forms.RadioButton radio_sourcedata;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnChooseBtn;
        private System.Windows.Forms.Button btnStartDownLoad;
        private System.Windows.Forms.Button btnCancle;
        private System.Windows.Forms.Label lblAvailableSpace;
        private System.Windows.Forms.RadioButton radio_correcteddata;
        private System.Windows.Forms.Panel panel1;
    }
}