using System;
using QRST.WorldGlobeTool.Net.WMS;

namespace QRST.WorldGlobeTool.Globe
{
    /// <summary>
    /// 三维球体接口
    /// </summary>
    public interface IGlobe
    {
        void SetDisplayMessages(System.Collections.IList messages);
        void SetLatLonGridShow(bool show);
        void SetLayers(System.Collections.IList layers);
        void SetVerticalExaggeration(double exageration);
        void SetViewDirection(String type, double horiz, double vert, double elev);
        void SetViewPosition(double degreesLatitude, double degreesLongitude,
            double metersElevation);
        void SetWmsImage(WmsDescriptor imageA, WmsDescriptor imageB, double alpha);
    }
}
