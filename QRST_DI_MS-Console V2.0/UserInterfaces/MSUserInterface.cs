/*
 * 
 * 
 */
  
using System.Collections.Generic;
using System.Windows.Forms;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraNavBar;

namespace QRST_DI_MS_Desktop.UserInterfaces
{
    public class MSUserInterface
    {
        public static List<MSUserInterface> listMSUI = new List<MSUserInterface>();
        public static MSUserInterface GetMSUIbyRibbonpage(RibbonPage rpage)
        {
            foreach (MSUserInterface msui in listMSUI)
            {
                if (msui.uiRibbonPage==rpage)
                {
                    return msui;
                }
            }
            return null;
        }
        public static MSUserInterface GetMSUIbyMainUC(UserControl mainuc)
        {
            foreach (MSUserInterface msui in listMSUI)
            {
                if (msui.uiMainUC == mainuc)
                {
                    return msui;
                }
            }
            return null;
        }
        public static MSUserInterface GetMSUIbySysNav(NavBarItem sysnavbaritem)
        {
            foreach (MSUserInterface msui in listMSUI)
            {
                if (msui.uiSysNavBarItem == sysnavbaritem)
                {
                    return msui;
                }
            }
            return null;
        }


        public MSUserInterface(RibbonPageBaseUC ribbonpageUC, UserControl mainpanel, NavBarItem sysnavbaritem)
        :base()
        {
            this.uiRibbonPage = ribbonpageUC.RibbonPage;
            this.uiMainUC = mainpanel;
            this.uiSysNavBarItem = sysnavbaritem;
        }

        public MSUserInterface(RibbonPageBaseUC ribbonpageUC, UserControl mainpanel)
            : base()
        {
            this.uiRibbonPage = ribbonpageUC.RibbonPage;
            this.uiMainUC = mainpanel;
        }

        public RibbonPage uiRibbonPage { get; private set; }
        public UserControl uiMainUC { get; private set; }
        public NavBarItem uiSysNavBarItem { get; set; }
    }
}
