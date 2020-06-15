using System.Windows.Forms;
using System.Collections.Generic;
using QRST_DI_DS_Metadata.MetaDataDefiner.Mdl;
namespace QRST_DI_MS_Console.UserInterfaces
{
    partial class mucDataImport
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.xtraTabControlDataImport = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.xtraTabPage3 = new DevExpress.XtraTab.XtraTabPage();
            this.xtraTabPage4 = new DevExpress.XtraTab.XtraTabPage();
            this.ctrlCommonDataImport1 = new ctrlCommonDataImport();
            this.ctrlCommonDataImport3 = new ctrlCommonDataImport();
            this.ctrlCommonDataImport2 = new ctrlCommonDataImport();
            this.ctrlCommonDataImport4 = new ctrlCommonDataImport();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControlDataImport)).BeginInit();
            this.xtraTabControlDataImport.SuspendLayout();
            this.SuspendLayout();
            // 
            // xtraTabControlDataImport
            // 
            this.xtraTabControlDataImport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControlDataImport.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControlDataImport.Name = "xtraTabControlDataImport";
            this.xtraTabControlDataImport.SelectedTabPage = this.xtraTabPage1;
            this.xtraTabControlDataImport.Size = new System.Drawing.Size(847, 579);
            this.xtraTabControlDataImport.TabIndex = 0;
            this.xtraTabControlDataImport.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2,
            this.xtraTabPage3,
            this.xtraTabPage4});
            for (int i = 0; i < TheUniversal.subDbLst.Count; i++)
            {
                TreeNode tn = TheUniversal.subDbLst[i].GetDbNode();
                if (tn != null && tn.FirstNode != null)
                {
                    TreeView treeview = new TreeView();
                    foreach (TreeNode ctn in tn.Nodes)
                    {
                        treeview.Nodes.Add(ctn);
                    }
                    treeview.ExpandAll();
                    treeDir.Add(tn.Text, treeview);
                }
            }
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Controls.Add(this.ctrlCommonDataImport1);
            this.xtraTabPage1.Size = new System.Drawing.Size(841, 552);
            this.xtraTabPage1.Text = "矢量数据";
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Controls.Add(this.ctrlCommonDataImport3);
            this.xtraTabPage2.Name = "xtraTabPage2";
            foreach (TreeNode node in this.ctrlCommonDataImport3.left.treeList[4].Nodes)
            {
                foreach (TreeNode cnode in node.Nodes)
                {
                    foreach (TreeNode childNode in cnode.Nodes)
                    {
                        if (childNode.Text == "成果文档")
                        {
                            this.ctrlCommonDataImport3.left.treeList[4].SelectedNode = childNode;
                            break;
                        }
                    }
                }
            }
            this.ctrlCommonDataImport3.left.button_Click(this.ctrlCommonDataImport3.left.button5,null);
            this.ctrlCommonDataImport3.left.treeList[4].Focus();
            this.ctrlCommonDataImport3.left.Enabled = false;
            Refresh();
            this.xtraTabPage2.Size = new System.Drawing.Size(841, 552);
            this.xtraTabPage2.Text = "用户文档";
            // 
            // xtraTabPage3
            // 
            this.xtraTabPage3.Name = "xtraTabPage3";
            this.xtraTabPage3.Controls.Add(this.ctrlCommonDataImport2);
            foreach (TreeNode node in this.ctrlCommonDataImport2.left.treeList[4].Nodes)
            {
                foreach (TreeNode cnode in node.Nodes)
                {
                    foreach (TreeNode childNode in cnode.Nodes)
                    {
                        if (childNode.Text == "栅格数据")
                        {
                            this.ctrlCommonDataImport2.left.treeList[4].SelectedNode = childNode;
                            break;
                        }
                    }
                }
            }
            this.ctrlCommonDataImport2.left.button_Click(this.ctrlCommonDataImport2.left.button5, null);
            this.ctrlCommonDataImport2.left.treeList[4].Focus();
            this.ctrlCommonDataImport2.left.Enabled = false;
            Refresh();
            this.xtraTabPage3.Size = new System.Drawing.Size(841, 552);
            this.xtraTabPage3.Text = "用户栅格数据";
            // 
            // xtraTabPage4
            // 
            this.xtraTabPage4.Name = "xtraTabPage4";
            this.xtraTabPage4.Controls.Add(this.ctrlCommonDataImport4);
            this.ctrlCommonDataImport4.left.Enabled = false;
            foreach (TreeNode node in this.ctrlCommonDataImport4.left.treeList[4].Nodes)
            {
                foreach (TreeNode cnode in node.Nodes)
                {
                    foreach (TreeNode childNode in cnode.Nodes)
                    {
                        if (childNode.Text == "用户工具")
                        {
                            this.ctrlCommonDataImport4.left.treeList[4].SelectedNode = childNode;
                            break;
                        }
                    }
                }
            }
            this.ctrlCommonDataImport4.left.button_Click(this.ctrlCommonDataImport4.left.button5, null);
            this.ctrlCommonDataImport4.left.treeList[4].Focus();
            this.ctrlCommonDataImport4.left.Enabled = false;
            Refresh();
            this.xtraTabPage4.Size = new System.Drawing.Size(841, 552);
            this.xtraTabPage4.Text = "用户工具";

            // 
            // ctrlCommonDataImport1
            // 
            this.ctrlCommonDataImport1.dataType = QRST_DI_DataImportTool.DataImport.DataType.Vector;
            this.ctrlCommonDataImport1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlCommonDataImport1.IsSingleImport = true;
            this.ctrlCommonDataImport1.Location = new System.Drawing.Point(3, 3);
            this.ctrlCommonDataImport1.Name = "ctrlCommonDataImport1";
            this.ctrlCommonDataImport1.Size = new System.Drawing.Size(944, 462);
            this.ctrlCommonDataImport1.TabIndex = 0;
            // 
            // ctrlCommonDataImport3
            // 
            this.ctrlCommonDataImport3.dataType = QRST_DI_DataImportTool.DataImport.DataType.UserDocument;
            this.ctrlCommonDataImport3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlCommonDataImport3.IsSingleImport = true;
            this.ctrlCommonDataImport3.Location = new System.Drawing.Point(3, 3);
            this.ctrlCommonDataImport3.Name = "ctrlCommonDataImport3";
            this.ctrlCommonDataImport3.Size = new System.Drawing.Size(944, 462);
            this.ctrlCommonDataImport3.TabIndex = 0;
            // 
            // ctrlCommonDataImport2
            // 
            this.ctrlCommonDataImport2.dataType = QRST_DI_DataImportTool.DataImport.DataType.UserRaster;
            this.ctrlCommonDataImport2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlCommonDataImport2.IsSingleImport = true;
            this.ctrlCommonDataImport2.Location = new System.Drawing.Point(3, 3);
            this.ctrlCommonDataImport2.Name = "ctrlCommonDataImport2";
            this.ctrlCommonDataImport2.Size = new System.Drawing.Size(944, 462);
            this.ctrlCommonDataImport2.TabIndex = 0;
            // 
            // ctrlCommonDataImport4
            // 
            this.ctrlCommonDataImport4.dataType = QRST_DI_DataImportTool.DataImport.DataType.UserToolKit;
            this.ctrlCommonDataImport4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlCommonDataImport4.IsSingleImport = true;
            this.ctrlCommonDataImport4.Location = new System.Drawing.Point(3, 3);
            this.ctrlCommonDataImport4.Name = "ctrlCommonDataImport4";
            this.ctrlCommonDataImport4.Size = new System.Drawing.Size(944, 462);
            this.ctrlCommonDataImport4.TabIndex = 0;
            // 
            // mucDataImport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.xtraTabControlDataImport);
            this.Name = "mucDataImport";
            this.Size = new System.Drawing.Size(847, 579);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControlDataImport)).EndInit();
            this.xtraTabControlDataImport.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl xtraTabControlDataImport;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage3;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage4;
        private ctrlCommonDataImport ctrlCommonDataImport1;
        private ctrlCommonDataImport ctrlCommonDataImport3;
        private ctrlCommonDataImport ctrlCommonDataImport2;
        private ctrlCommonDataImport ctrlCommonDataImport4;
        Dictionary<string, TreeView> treeDir = new Dictionary<string, TreeView>();
    }
}
