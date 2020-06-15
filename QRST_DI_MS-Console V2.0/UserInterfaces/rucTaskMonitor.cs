namespace QRST_DI_MS_Desktop.UserInterfaces
{
    public partial class rucTaskMonitor : RibbonPageBaseUC
    { 
        public rucTaskMonitor():base()
        {
            InitializeComponent();
        }
        public rucTaskMonitor(object objMUC):base(objMUC)
    {
            InitializeComponent();
    }

        private void barListItem1_ListItemClick(object sender, DevExpress.XtraBars.ListItemClickEventArgs e)
        {

        }
    }
}
