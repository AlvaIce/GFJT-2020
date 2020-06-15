using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QRST_DI_MS_Console.UserInterfaces
{
    public partial class rucDataImport : RibbonPageBaseUC
    {
        public rucDataImport()
        {
            InitializeComponent();
        }

        private void DataImportTools_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void ImportButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string buttonName = e.Item.Description;
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "请选择数据路径";
            DialogResult dr = fbd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                IOGF1dataBatchImport import = new IOGF1dataBatchImport();
                import.CreateGF1BatchImport(fbd.SelectedPath, buttonName);
            }
        }

        private void TileImport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "请选择切片数据路径";
            DialogResult dr = fbd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                ITStoreTilesNew tileImport = new ITStoreTilesNew();
                tileImport.TileImport(fbd.SelectedPath);
            }
        }
    }
}
