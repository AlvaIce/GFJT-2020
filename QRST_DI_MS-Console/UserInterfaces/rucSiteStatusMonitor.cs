using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraEditors;

namespace QRST_DI_MS_Console.UserInterfaces
{
    class rucSiteStatusMonitor : RibbonPageBaseUC
    {
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem5;
        private DevExpress.XtraBars.BarButtonItem barButtonItem6;
        private DevExpress.XtraBars.BarButtonItem barButtonItem7;
        private DevExpress.XtraBars.BarEditItem barEditItem1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup3;
        private DevExpress.XtraBars.BarButtonItem barButtonItem8;
        private DevExpress.XtraBars.BarEditItem barEditItemCPU;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckCPU;
        private DevExpress.XtraBars.BarEditItem barEditIteMEM;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckMEMO;
        private DevExpress.XtraBars.BarEditItem barEditItemUnused;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckUnuse;
        private DevExpress.XtraBars.BarEditItem barEditItemUsed;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckUsed;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;

        public rucSiteStatusMonitor()
            : base()
        {
            InitializeComponent();
        }

        public rucSiteStatusMonitor(object objMUC)
            : base(objMUC)
        {
            InitializeComponent();
        }


        private void InitializeComponent()
        {
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.barEditItem1 = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.barEditItemCPU = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemCheckCPU = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.barEditIteMEM = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemCheckMEMO = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.barEditItemUnused = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemCheckUnuse = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.barEditItemUsed = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemCheckUsed = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.ribbonPageGroup3 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.barButtonItem5 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem6 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem7 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem8 = new DevExpress.XtraBars.BarButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckCPU)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckMEMO)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckUnuse)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckUsed)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonControl1
            // 
            // 
            // 
            // 
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.ExpandCollapseItem.Name = "";
            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barButtonItem1,
            this.barButtonItem5,
            this.barButtonItem6,
            this.barButtonItem7,
            this.barEditItem1,
            this.barButtonItem8,
            this.barEditItemCPU,
            this.barEditIteMEM,
            this.barEditItemUnused,
            this.barEditItemUsed});
            this.ribbonControl1.MaxItemId = 14;
            this.ribbonControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCheckCPU,
            this.repositoryItemCheckMEMO,
            this.repositoryItemCheckUnuse,
            this.repositoryItemCheckUsed,
            this.repositoryItemComboBox1});
            this.ribbonControl1.Size = new System.Drawing.Size(687, 149);
            this.ribbonControl1.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Above;
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup2,
            this.ribbonPageGroup3});
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.barButtonItem1);
            this.ribbonPageGroup1.ItemLinks.Add(this.barButtonItem8, true);
            this.ribbonPageGroup1.Text = "节点概况";
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "数据阵列节点列表";
            this.barButtonItem1.Id = 1;
            this.barButtonItem1.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.数据阵列1;
            this.barButtonItem1.Name = "barButtonItem1";
            this.barButtonItem1.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonItem1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem1_ItemClick);
            // 
            // ribbonPageGroup2
            // 
            this.ribbonPageGroup2.ItemLinks.Add(this.barEditItem1, true);
            this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemCPU, true);
            this.ribbonPageGroup2.ItemLinks.Add(this.barEditIteMEM);
            this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemUnused, true);
            this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemUsed);
            this.ribbonPageGroup2.Name = "ribbonPageGroup2";
            this.ribbonPageGroup2.Text = "监控设置";
            // 
            // barEditItem1
            // 
            this.barEditItem1.Caption = "监控频率（秒）";
            this.barEditItem1.Edit = this.repositoryItemComboBox1;
            this.barEditItem1.EditValue = 1;
            this.barEditItem1.Id = 8;
            this.barEditItem1.Name = "barEditItem1";
            this.barEditItem1.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barEditItem1.EditValueChanged += new System.EventHandler(this.barEditItem1_EditValueChanged);
            // 
            // repositoryItemComboBox1
            // 
            this.repositoryItemComboBox1.AutoHeight = false;
            this.repositoryItemComboBox1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox1.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "10"});
            this.repositoryItemComboBox1.Name = "repositoryItemComboBox1";
            // 
            // barEditItemCPU
            // 
            this.barEditItemCPU.Caption = "CPU使用率";
            this.barEditItemCPU.Edit = this.repositoryItemCheckCPU;
            this.barEditItemCPU.EditValue = true;
            this.barEditItemCPU.Id = 10;
            this.barEditItemCPU.Name = "barEditItemCPU";
            this.barEditItemCPU.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barEditItemCPU.EditValueChanged += new System.EventHandler(this.barEditItemCPU_EditValueChanged);
            // 
            // repositoryItemCheckCPU
            // 
            this.repositoryItemCheckCPU.AutoHeight = false;
            this.repositoryItemCheckCPU.Name = "repositoryItemCheckCPU";
            this.repositoryItemCheckCPU.CheckStateChanged += new System.EventHandler(this.repositoryItemCheckCPU_CheckedChanged);
            // 
            // barEditIteMEM
            // 
            this.barEditIteMEM.Caption = "内存使用率";
            this.barEditIteMEM.Edit = this.repositoryItemCheckMEMO;
            this.barEditIteMEM.EditValue = true;
            this.barEditIteMEM.Id = 11;
            this.barEditIteMEM.Name = "barEditIteMEM";
            // 
            // repositoryItemCheckMEMO
            // 
            this.repositoryItemCheckMEMO.AutoHeight = false;
            this.repositoryItemCheckMEMO.Name = "repositoryItemCheckMEMO";
            this.repositoryItemCheckMEMO.CheckStateChanged += new System.EventHandler(this.repositoryItemCheckMEMO_CheckStateChanged);
            // 
            // barEditItemUnused
            // 
            this.barEditItemUnused.Caption = "可用磁盘";
            this.barEditItemUnused.Edit = this.repositoryItemCheckUnuse;
            this.barEditItemUnused.EditValue = true;
            this.barEditItemUnused.Id = 12;
            this.barEditItemUnused.Name = "barEditItemUnused";
            this.barEditItemUnused.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            // 
            // repositoryItemCheckUnuse
            // 
            this.repositoryItemCheckUnuse.AutoHeight = false;
            this.repositoryItemCheckUnuse.Name = "repositoryItemCheckUnuse";
            this.repositoryItemCheckUnuse.CheckStateChanged += new System.EventHandler(this.repositoryItemCheckUnuse_CheckStateChanged);
            // 
            // barEditItemUsed
            // 
            this.barEditItemUsed.Caption = "已用磁盘";
            this.barEditItemUsed.Edit = this.repositoryItemCheckUsed;
            this.barEditItemUsed.EditValue = true;
            this.barEditItemUsed.Id = 13;
            this.barEditItemUsed.Name = "barEditItemUsed";
            // 
            // repositoryItemCheckUsed
            // 
            this.repositoryItemCheckUsed.AutoHeight = false;
            this.repositoryItemCheckUsed.Name = "repositoryItemCheckUsed";
            this.repositoryItemCheckUsed.CheckStateChanged += new System.EventHandler(this.repositoryItemCheckUsed_CheckStateChanged);
            // 
            // ribbonPageGroup3
            // 
            this.ribbonPageGroup3.ItemLinks.Add(this.barButtonItem5);
            this.ribbonPageGroup3.ItemLinks.Add(this.barButtonItem6, true);
            this.ribbonPageGroup3.ItemLinks.Add(this.barButtonItem7, true);
            this.ribbonPageGroup3.Name = "ribbonPageGroup3";
            this.ribbonPageGroup3.Text = "站点管理";
            // 
            // barButtonItem5
            // 
            this.barButtonItem5.Caption = "站点添加";
            this.barButtonItem5.Id = 5;
            this.barButtonItem5.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.站点添加;
            this.barButtonItem5.Name = "barButtonItem5";
            this.barButtonItem5.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            // 
            // barButtonItem6
            // 
            this.barButtonItem6.Caption = "站点更新";
            this.barButtonItem6.Id = 6;
            this.barButtonItem6.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.站点更新;
            this.barButtonItem6.Name = "barButtonItem6";
            this.barButtonItem6.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            // 
            // barButtonItem7
            // 
            this.barButtonItem7.Caption = "站点删除";
            this.barButtonItem7.Id = 7;
            this.barButtonItem7.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.站点删除;
            this.barButtonItem7.Name = "barButtonItem7";
            this.barButtonItem7.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            // 
            // barButtonItem8
            // 
            this.barButtonItem8.Caption = "站点监控信息";
            this.barButtonItem8.Id = 9;
            this.barButtonItem8.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.站点信息;
            this.barButtonItem8.Name = "barButtonItem8";
            this.barButtonItem8.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonItem8.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem8_ItemClick);
            // 
            // rucSiteStatusMonitor
            // 
            this.Name = "rucSiteStatusMonitor";
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckCPU)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckMEMO)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckUnuse)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckUsed)).EndInit();
            this.ResumeLayout(false);

        }

        /// <summary>
        /// 控制内存使用率的显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void repositoryItemCheckMEMO_CheckStateChanged(object sender, EventArgs e)
        {
            ((mucSiteStatusMonitor)ObjMainUC).HideSeries("SeriesMemRate", (bool)((CheckEdit)sender).EditValue);
        }

        /// <summary>
        /// 控制未用磁盘空间的显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void repositoryItemCheckUnuse_CheckStateChanged(object sender, EventArgs e)
        {
            ((mucSiteStatusMonitor)ObjMainUC).HideSeries("SeriesUnusedMemory", (bool)((CheckEdit)sender).EditValue);
        }
        /// <summary>
        /// 控制已用磁盘空间显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void repositoryItemCheckUsed_CheckStateChanged(object sender, EventArgs e)
        {
            ((mucSiteStatusMonitor)ObjMainUC).HideSeries("SeriesUsedMemory", (bool)((CheckEdit)sender).EditValue);
        }

        /// <summary>
        /// 控制cpu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void repositoryItemCheckCPU_CheckedChanged(object sender, EventArgs e)
        {
            ((mucSiteStatusMonitor)ObjMainUC).HideSeries("SeriesCPURate", (bool)((CheckEdit)sender).EditValue);
        }

        /// <summary>
        /// 展开节点列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ((mucSiteStatusMonitor)ObjMainUC).OpenTabPage(0);
        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ((mucSiteStatusMonitor)ObjMainUC).OpenTabPage(1);
        }



        private void barEditItem1_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                int interval = int.Parse(barEditItem1.EditValue.ToString());
                ((mucSiteStatusMonitor)ObjMainUC).SetTimeSpan(interval * 1000);
            }
            catch (Exception)
            {

                return;
            }
        }

        private void barEditItemCPU_EditValueChanged(object sender, EventArgs e)
        {
           
        }
    }
}
