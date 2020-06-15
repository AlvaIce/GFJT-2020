using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QRST_DI_TS_Basis.DirectlyAddress;
using QRST_DI_Resources;

public partial class GetTile : System.Web.UI.Page
{

    private static DirectlyAddressing _tileDA = null;
    //protected void Page_Load(object sender, EventArgs e)
    //{
    //    string tilename = Request.QueryString["tilename"];
    //    //tilename = "GF1_WFV1_20131231_L1A0000143299_8_484_1131.jpg";
    //    if (tilename != null)
    //    {
    //        string tilepath = _tileDA.GetPathByFileName(tilename);
    //        if (System.IO.File.Exists(tilepath))
    //        {
    //            if (tilename.EndsWith("jpg"))
    //            {
    //                Response.ContentType = "image/jpeg";
    //                System.Drawing.Image img = System.Drawing.Image.FromFile(tilepath);

    //                img.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
    //            }
    //            else if (tilename.EndsWith("tif"))
    //            {
    //                Response.ContentType = "image/tiff";
    //                System.Drawing.Image img = System.Drawing.Image.FromFile(tilepath);  

    //                img.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Tiff);
    //            }
    //        }
    //    } 

    public static QRST_DI_TS_Process.Site.TServerSite _optTSite = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        string tilename = Request.QueryString["tilename"];
        //http:\\....\WS_QDB_GetData\GetTile.aspx?tilename=GF1_PMS2_20131203_L1A0000124162_7_1190_2823-1.tif
        if (!Constant.ServiceIsConnected)
        {
            Constant.InitializeTcpConnection();
        }
        _tileDA = new DirectlyAddressing(DirectlyAddressingIPMod.IPModDataSet);
        if (tilename != null)
        {
            //string tilepath = _tileDA.GetPathByFileName(tilename);
            Response.ContentType = "image/jpeg";
            while (_optTSite == null || !_optTSite.TCPService.IsRunning)
            {
                _optTSite = QRST_DI_TS_Process.Site.TServerSiteManager.GetOptimalStorageSite();
            }
            int[][] imgdata = null;
            try
            {
                imgdata = _optTSite.TCPService.GetTileImageData(tilename);
            }
            catch 
            {
                QRST_DI_TS_Process.Site.TServerSiteManager.UpdateOptimalStorageSiteList();
                _optTSite = QRST_DI_TS_Process.Site.TServerSiteManager.GetOptimalStorageSite();
                imgdata = _optTSite.TCPService.GetTileImageData(tilename);
            }
           
            System.Drawing.Bitmap btmap = new System.Drawing.Bitmap(imgdata.Length,imgdata[0].Length);
            for (int i = 0; i < imgdata.Length; i++)
            {
                for (int j = 0; j < imgdata[i].Length; j++)
                {
                    btmap.SetPixel(i, j, System.Drawing.Color.FromArgb(imgdata[i][j]));
                }
            }
            string tmppath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Request.PhysicalPath), "tmpTileJPG.jpg");
            //img.Save(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Request.PhysicalPath), "tmpTileJPG.jpg"));
            //img.Save(@"\tmpTileJPG.jpg");
            //img = System.Drawing.Image.FromFile(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Request.PhysicalPath), "tmpTileJPG.jpg"));
            btmap.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
        }
    }
}