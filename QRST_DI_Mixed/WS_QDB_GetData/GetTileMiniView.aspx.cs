using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QRST_DI_TS_Basis.DirectlyAddress;
using QRST_DI_Resources;

public partial class GetTileMiniView : System.Web.UI.Page
{

    private static DirectlyAddressing _tileDA =null;
    protected void Page_Load(object sender, EventArgs e)
    {
        string tilename = Request.QueryString["tilename"];
        if (!Constant.ServiceIsConnected)
        {
            Constant.InitializeTcpConnection();
        }
        _tileDA = new DirectlyAddressing(DirectlyAddressingIPMod.IPModDataSet);
        //http:\\....\WS_QDB_GetData\GetTileMiniView.aspx?tilename=GF1_PMS2_20131203_L1A0000124162_7_1190_2823-1.tif
        if (tilename != null)
        {
            Response.ContentType = "image/jpeg";

            if (QRST_DI_TS_Process.Site.TServerSiteManager.StorageSites.Count==0)
            {
                QRST_DI_TS_Process.Site.TServerSiteManager.UpdateOptimalStorageSiteList();
            }

            QRST_DI_TS_Process.Site.TServerSite _optTSite = QRST_DI_TS_Process.Site.TServerSiteManager.getSiteFromSiteIP(_tileDA.GetStorageIP(tilename));
           
            if (_optTSite == null || !_optTSite.TCPService.IsRunning)
            {
                _optTSite = QRST_DI_TS_Process.Site.TServerSiteManager.GetOptimalStorageSite();
            }

            try
            {
                if (_optTSite == null)
                {
                    throw new Exception("没有可用数据库服务站点!");
                }

                byte[] imgdata = _optTSite.TCPService.GetTileMiniView(tilename);

                if (imgdata == null)
                {
                    throw new Exception("目标数据快视图不存在!");
                }

                Response.BinaryWrite(imgdata);
            }
            catch (Exception)
            {
                throw;
            }
           
        }
    }
}