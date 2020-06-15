using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using QRST_DI_DS_Metadata.MetaDataDefiner;
using QRST_DI_DS_Metadata;
using QRST_DI_MS_Component.VirtualDirUI;
 
namespace QRST_DI_MS_Desktop.UserInterfaces
{
    public partial class mucVirtualDirManager : DevExpress.XtraEditors.XtraUserControl
    {
        public static mucVirtualDirManager MainInstance = null;

        public bool IsCreated { get; set; }

        public mucVirtualDirManager()
        {
            virtualDirUC = new VirtualDirUC();
            virtualDirUC.MainVirtualDirCtrl.OnDataImportItemClick += new EventHandler(MainVirtualDirCtrl_OnDataImportItemClick);
            InitializeComponent();
            IsCreated = false;
            TheUniversal._VirtualDirUC = virtualDirUC;
        }

        void MainVirtualDirCtrl_OnDataImportItemClick(object sender, EventArgs e)
        {
            //选择数据维护子系统
            //选择数据导入tab
            TheUniversal.MainForm.systemselect_ItemClick(this, new DevExpress.XtraBars.ItemClickEventArgs(TheUniversal.MainForm.baritem_sysDataMaintain, null), "数据入库");

            object[] args = sender as object[];
            string typestr = args[0].ToString();
            string vdpath = args[1].ToString();
            string vdcode = args[2].ToString();

            TheUniversal.RucDataImport.ClickDownloadItem(typestr, vdpath, vdcode);

        }

        public void Create()
        {
            virtualDirUC.InitializeVirtualDir(TheUniversal.currentUser.NAME, TheUniversal.currentUser.PASSWORD);


            IsCreated = true;
        }

        public void UpdateVirtualDir()
        {
            virtualDirUC.UpdateVirtualDir();
        }


    }
}
