using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.UserSkins;
using DevExpress.XtraBars.Helpers;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraNavBar;
using DevExpress.XtraEditors;
using QRST_DI_DS_Basis.DBEngine;
using QRST_DI_DS_Metadata.MetaDataDefiner;
using QRST_DI_MS_Console.UserInterfaces;
using QRST_DI_TS_Process;
using QRST_DI_TS_Process.Site;
using QRST_DI_MS_Basis.Log;

namespace QRST_DI_MS_Console
{
    public partial class frmMSConsole : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public MSUserInterface newMSUI;
        public frmMSConsole()
        {
            //注册使用非默认皮肤包
            BonusSkins.Register();
            OfficeSkins.Register();
            DevExpress.Skins.SkinManager.EnableFormSkins();
            //系统控件初始化
            InitializeComponent();
            //初始化皮肤选取面板
            SkinHelper.InitSkinGalleryDropDown(gdd_skins);

            //注册绑定子系统功能UI
           InitializeMSUserInterfase();

        }

        private void InitializeMSUserInterfase()
        {
            //实例化全局变量
            MySqlBaseUtilities mySqlBaseUtilities = new MySqlBaseUtilities();
            DataTable subDbArr = mySqlBaseUtilities.GetDataSet("select * from subdbinfo ").Tables[0];

            TheUniversal.subDbLst = new List<SiteDb>();
            for (int i = 0 ; i < subDbArr.Rows.Count ; i++)
            {
                string name = subDbArr.Rows[i]["NAME"].ToString();
                string qrst_code = subDbArr.Rows[i]["QRST_CODE"].ToString();
                string description = subDbArr.Rows[i]["DESCRIPTION"].ToString();
                string connectStr = subDbArr.Rows[i]["ConnectStr"].ToString();
                SiteDb subdb = new SiteDb(name, qrst_code, connectStr, description);
                TheUniversal.subDbLst.Add(subdb);
            }
            TheUniversal.nbcMain = nbc_sysNavi;
            TSPCommonReference.Create();
            TServerSiteManager.UpdateStorageSiteList();
            TheUniversal.sysLog = new SysLog(TheUniversal.currentUser);
            barStaticItem1.Caption = string.Format("当前用户：{0}",TheUniversal.currentUser.NAME);
            //清空控件组
            this.ribbon.Pages.Clear();
            this.pnl_Main.Controls.Clear();

            #region 添加系统固有page
            //添加关于功能面板
            this.ribbon.Pages.Add(rbpg_about);
            
            #endregion

          

            #region 添加系统模块page
            //3D可视化检索模块
            muc3DSearcher muc3dsearcher = new muc3DSearcher();
            ruc3DSearcher ruc3dsearcher = new ruc3DSearcher(muc3dsearcher,this);
            AddNewMSUI(ruc3dsearcher, muc3dsearcher, nbi_3DSearcher);

            //查看详细结果面板
            mucDetailViewer mucdetailview = new mucDetailViewer();
            rucDetailViewer rucdetailview = new rucDetailViewer(mucdetailview);
            AddNewMSUI(rucdetailview, mucdetailview, nbi_DetailViewer);

            //波谱特征数据库查询浏览界面
            //mucSpectrumSearch mucspectrumSearch = new mucSpectrumSearch();
            //rucSpectrumSearch rucspectrumSearch = new rucSpectrumSearch(mucspectrumSearch);
            //AddNewMSUI(rucspectrumSearch,mucspectrumSearch,nbi_SpectrumFeature);

            //更新维护面板
            mucDataMaintainer mucmaintain = new mucDataMaintainer();
            rucDataMaintainer rucmaintain = new rucDataMaintainer(mucmaintain);
            AddNewMSUI(rucmaintain, mucmaintain, nbi_DataMaintainer);

            //备份恢复面板
            mucDataBacker mucbacker = new mucDataBacker();
            rucDataBacker rucbacker = new rucDataBacker(mucbacker);
            AddNewMSUI(rucbacker, mucbacker, nbi_DataBacker);

            //分类统计面板
            mucDataAnalyst mucanalyst = new mucDataAnalyst();
            rucDataAnalyst rucanalyst = new rucDataAnalyst(mucanalyst);
            AddNewMSUI(rucanalyst, mucanalyst, nbi_DataAnalyst);

            //元数据定制
            mucMetadataDefiner mucmetadatadefiner = new mucMetadataDefiner();
            rucMetadataDefiner rucmetadatadefiner = new rucMetadataDefiner(mucmetadatadefiner);
            AddNewMSUI(rucmetadatadefiner,mucmetadatadefiner,nbi_MetadataMaker);
            
            ////元数据修改
            mucMetadataModifier mucmetadatamodifier = new mucMetadataModifier();
            rucMetadataModifier rucmatadatamodifier = new rucMetadataModifier(mucmetadatamodifier);
            AddNewMSUI(rucmatadatamodifier, mucmetadatamodifier, nbi_MetadataModifier);

            //添加,修改卫星 传感器
            mucSatalliteSensor mucsatallitesensor = new mucSatalliteSensor();
            rucSatalliteSensor rucsatallitesensor = new rucSatalliteSensor(mucsatallitesensor);
            AddNewMSUI(rucsatallitesensor, mucsatallitesensor, nbi_SatalliteSensor);

            //nbi_ExternalDB 外部数据库集成
            mucExternalDbIntergration mucexternalDbintergration = new mucExternalDbIntergration();
            rucExternalDbIntergration rucexternalDbIntergration = new rucExternalDbIntergration(mucexternalDbintergration);
            AddNewMSUI(rucexternalDbIntergration, mucexternalDbintergration,nbi_ExternalDB);
            //任务提交
            //mucTaskSubmitter muctasksubmitter = new mucTaskSubmitter();
            //rucTaskSubmitter ructasksubmitter = new rucTaskSubmitter(muctasksubmitter);
            //AddNewMSUI(ructasksubmitter, muctasksubmitter, nbi_TaskSubmitter);

            //订单定制
            mucOrderDefiner mucorderdefiner = new mucOrderDefiner();
            rucOrderDefiner rucorderdefiner = new rucOrderDefiner(mucorderdefiner);
            AddNewMSUI(rucorderdefiner, mucorderdefiner, nbi_OrderDefiner);


            //任务监控
            mucTaskMonitor muctaskmonitor = new mucTaskMonitor();
            rucOrderManager rucorderManager = new rucOrderManager(muctaskmonitor);
            AddNewMSUI(rucorderManager, muctaskmonitor, nbi_TaskMonitor);
            
            //任务定制
            mucTaskDefiner muctaskdefiner = new mucTaskDefiner();
            rucTaskDefiner ructaskdefiner = new rucTaskDefiner(muctaskdefiner);
            AddNewMSUI(ructaskdefiner, muctaskdefiner, nbi_TaskDefiner);
            ////添加数据阵列站点管理  nbi_DSSiteManager
            mucSiteStatusMonitor mucsitestatusmonitor = new mucSiteStatusMonitor();
            rucSiteStatusMonitor rucsitestatusmonitor = new rucSiteStatusMonitor(mucsitestatusmonitor);
            AddNewMSUI(rucsitestatusmonitor, mucsitestatusmonitor, nbi_DSSiteManager);

            //添加数据质量检查
            mucDataQualityInspection mucdataqualityinspection = new mucDataQualityInspection();
            rucDataQualityInspection rucdataqualityinspection = new rucDataQualityInspection(mucdataqualityinspection);
            AddNewMSUI(rucdataqualityinspection, mucdataqualityinspection, nbi_DataQualityInspection);
            //添加服务阵列站点管理
            mucTSSiterManager muctssitermanager = new mucTSSiterManager();
            rucTSSiterManager ructssitermanager = new rucTSSiterManager(muctssitermanager);
            AddNewMSUI(ructssitermanager, muctssitermanager, nbi_TSSiterManager);

            //添加用户管理
            mucUserManager mucusermanager = new mucUserManager();
            rucUserManager rucusermanager = new rucUserManager(mucusermanager);
            AddNewMSUI(rucusermanager, mucusermanager, nbi_UserManager);

            //系统状态监控
            mucSysMonitor mucsysmonitor = new mucSysMonitor();
            rucSysMonitor rucsysmonitor = new rucSysMonitor(mucsysmonitor);
            AddNewMSUI(rucsysmonitor,mucsysmonitor,nbi_SysMonitor);       


            #endregion

            //选择初始默认功能面板(初始显示三维检索面板)
            nbg_Searcher.Expanded = true;
            nbc_sysNavi.SelectedLink = nbi_3DSearcher.Links[0];
            ribbon.SelectedPage = ruc3dsearcher.RibbonPage;

            //根据用户访问权限，控制功能模块的显示
            int[][] roleArr = TheUniversal.currentUser.GetRightArr();
            for (int i = 0 ; i < roleArr.Length ;i++ )
            {
                if (roleArr[i][0] == 0)
                {
                    nbc_sysNavi.Groups[i].Visible = false;
                }
                else
                {
                    for (int j = 1 ; j < roleArr[i].Length ;j++ )
                    {
                        if (roleArr[i][j] == 0)
                        {
                            nbc_sysNavi.Groups[i].ItemLinks[j - 1].Visible = false;
                        }
                    }
                }
            }

            this.WindowState = FormWindowState.Maximized;
        }

        /// <summary>
        /// 创建新的UI
        /// </summary>
        /// <param name="rpgUC">UI的工具条</param>
        /// <param name="mainUC">UI的主面板</param>
        /// <param name="sysNav">UI的导航Bar</param>
        private void AddNewMSUI(RibbonPageBaseUC rpgUC, UserControl mainUC, NavBarItem sysNav)
        {
            MSUserInterface newMSUI = new MSUserInterface(rpgUC, mainUC, sysNav);
            MSUserInterface.listMSUI.Add(newMSUI);

            this.ribbon.Pages.Insert(0, newMSUI.uiRibbonPage);

            newMSUI.uiMainUC.Dock = DockStyle.Fill;
            this.pnl_Main.Controls.Add(newMSUI.uiMainUC);
        }
        
        private void nbc_sysNavi_SelectedLinkChanged(object sender, DevExpress.XtraNavBar.ViewInfo.NavBarSelectedLinkChangedEventArgs e)
        {
            //选择选中的导航bar对应的UI面板
            SelectMSUI(e.Link.Item);
        }

        /// <summary>
        /// 根据导航bar更新UI面板
        /// </summary>
        /// <param name="navBarItem"></param>
        private void SelectMSUI(NavBarItem navBarItem)
        {
            foreach (MSUserInterface msui in MSUserInterface.listMSUI)
            {
                if (msui.uiSysNavBarItem == navBarItem)
                {
                    //显示当前选中面板
                    msui.uiMainUC.Visible = true;
                    msui.uiRibbonPage.Visible = true;
                    ribbon.SelectedPage = msui.uiRibbonPage;
                }
                else
                {
                    //隐藏非当前功能面板
                    msui.uiMainUC.Visible = false;
                    msui.uiRibbonPage.Visible = false;
                }
            }
        }
        /// <summary>
        /// 非单击切换选中导航项方法，目前只用于转到详细信息面板
        /// </summary>
        /// <param name="strIn"></param>
        public void setNaviItemSelected(string strIn)
        {
            if (strIn==this.nbi_DetailViewer.Caption)
            {
                for (int i = 0; i < nbc_sysNavi.Items.Count; i++)
                {
                    if (nbg_Searcher.ItemLinks[i].Caption.ToString() == strIn)
                    {
                        nbc_sysNavi.SelectedLink = nbg_Searcher.ItemLinks[i];
                        break;
                    }
                }
            }

        }
        /// <summary>
        /// 根据UI对象更新UI面板
        /// </summary>
        /// <param name="pmsui"></param>
        private void SelectMSUI(MSUserInterface pmsui)
        {
            foreach (MSUserInterface msui in MSUserInterface.listMSUI)
            {
                if (msui == pmsui)
                {
                    //显示当前选中面板
                    msui.uiMainUC.Visible = true;
                    msui.uiRibbonPage.Visible = true;
                    ribbon.SelectedPage = msui.uiRibbonPage;
                }
                else
                {
                    //隐藏非当前功能面板
                    msui.uiMainUC.Visible = false;
                    msui.uiRibbonPage.Visible = false;
                }
            }
        }
    }
}
