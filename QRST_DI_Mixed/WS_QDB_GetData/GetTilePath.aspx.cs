using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QRST_DI_TS_Basis.DirectlyAddress;
using QRST_DI_Resources;

public partial class GetTilePath : System.Web.UI.Page
{
    private static DirectlyAddressing _tileDA = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Constant.ServiceIsConnected)
        {
            Constant.InitializeTcpConnection();
        }
        _tileDA = new DirectlyAddressing(DirectlyAddressingIPMod.IPModDataSet);
        string tilename = Request.QueryString["tilename"];
        string tilepath = "";
        if (tilename != null)
        {
            tilepath = _tileDA.GetPathByFileName(tilename);
        }
        byte[] buffer = System.Text.Encoding.Default.GetBytes(tilepath);
        Response.OutputStream.Write(buffer,0,buffer.Count());
    }
}