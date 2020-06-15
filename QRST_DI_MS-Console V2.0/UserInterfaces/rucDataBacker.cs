using System;
using System.IO;
 
namespace QRST_DI_MS_Desktop.UserInterfaces
{
    class rucDataBacker : RibbonPageBaseUC
	{
        private DevExpress.XtraBars.BarButtonItem barButtonBackup;
        private DevExpress.XtraBars.BarButtonItem barButtonRecovery;
        private DevExpress.XtraBars.BarButtonItem barButtonFileBackup;
        private DevExpress.XtraBars.BarButtonItem barButtonChangeSettings;
        
        mucDataBacker mucdataBacker;
              public rucDataBacker()
            : base()
        {
            InitializeComponent();
        }
              public rucDataBacker(object obMaintainUC)
            : base(obMaintainUC)
        {
            InitializeComponent();
            mucdataBacker = obMaintainUC as mucDataBacker;
        }
    
        private void InitializeComponent()
        {
            this.barButtonBackup = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonRecovery = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonFileBackup = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonChangeSettings = new DevExpress.XtraBars.BarButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
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
            this.barButtonBackup,
            this.barButtonFileBackup,
            this.barButtonRecovery,
            this.barButtonChangeSettings});
            this.ribbonControl1.MaxItemId = 4;
            this.ribbonControl1.Size = new System.Drawing.Size(687, 149);
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.barButtonBackup, true);
            this.ribbonPageGroup1.ItemLinks.Add(this.barButtonFileBackup, true);
            this.ribbonPageGroup1.ItemLinks.Add(this.barButtonRecovery, true);
            this.ribbonPageGroup1.ItemLinks.Add(this.barButtonChangeSettings, true);
            this.ribbonPageGroup1.Text = "数据操作";
            // 
            // barButtonBackup
            // 
            this.barButtonBackup.Caption = "数据库备份";
            this.barButtonBackup.Id = 1;
            this.barButtonBackup.LargeGlyph = global::QRST_DI_MS_Desktop.Properties.Resources.数据备份恢复;
            this.barButtonBackup.Name = "barButtonBackup";
            this.barButtonBackup.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonBackup_ItemClick);
            // 
            // barButtonRecovery
            // 
            this.barButtonRecovery.Caption = "数据库恢复";
            this.barButtonRecovery.Id = 3;
            this.barButtonRecovery.LargeGlyph = global::QRST_DI_MS_Desktop.Properties.Resources.数据恢复;
            this.barButtonRecovery.Name = "barButtonRecovery";
            this.barButtonRecovery.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonRecovery_ItemClick);
            // 
            // barButtonFileBackup
            // 
            this.barButtonFileBackup.Caption = "文件备份监控";
            this.barButtonFileBackup.Id = 2;
            this.barButtonFileBackup.LargeGlyph = global::QRST_DI_MS_Desktop.Properties.Resources.备份恢复;
            this.barButtonFileBackup.Name = "barButtonFileBackup";
            this.barButtonFileBackup.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonFileBackup_ItemClick);
            // 
            // barButtonChangeSettings
            // 
            this.barButtonChangeSettings.Caption = "备份参数及日志";
            this.barButtonChangeSettings.Id = 4;
            this.barButtonChangeSettings.LargeGlyph = global::QRST_DI_MS_Desktop.Properties.Resources.ChangeAppsetting;
            this.barButtonChangeSettings.Name = "barButtonChangeSettings";
            this.barButtonChangeSettings.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonChangeSettings_ItemClick);
            // 
            // rucDataBacker
            // 
            this.Name = "rucDataBacker";
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            this.ResumeLayout(false);

        }

        private void barButtonBackup_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            mucdataBacker.ChangeTabpage(0);            
        }



        private void barButtonRecovery_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //Cmd.StartCmd(AppDomain.CurrentDomain.BaseDirectory, "TileDataBackup.exe");
            mucdataBacker.ChangeTabpage(1);
        }


        //在指定目录创建文件夹
        public static void CreateFile(string path)
        {
            try
            {
                // Determine whether the directory exists.
                if (!Directory.Exists(path))
                {
                    // Create the directory it does not exist.
                    Directory.CreateDirectory(path);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void barButtonChangeSettings_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            mucdataBacker.ChangeTabpage(3);
        }

        private void barButtonFileBackup_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            mucdataBacker.ChangeTabpage(2);
        }
    }
}
