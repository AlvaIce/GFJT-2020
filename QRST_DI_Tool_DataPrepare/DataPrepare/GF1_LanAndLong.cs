namespace DataPrepare
{
    //GF1信息类
    public class GF1_LanAndLong
    {
        public string Name { set; get; }
        public double DATALOWERLEFTLAT { set; get; }
        public double DATALOWERLEFTLONG { set; get; }
        public double DATALOWERRIGHTLAT { set; get; }
        public double DATALOWERRIGHTLONG { set; get; }
        public double DATAUPPERRIGHTLAT { set; get; }
        public double DATAUPPERRIGHTLONG { set; get; }
        public double DATAUPPERLEFTLAT { set; get; }
        public double DATAUPPERLEFTLONG { set; get; }
        public string ProductID { set; get; }
        public string QRST_CODE { set; get; }
        public string SatelliteID { set; get; }
        public string SensorID { set; get; }
    }

    //瓦片信息类
    public class Tile_Info
    {
        public string level{set;get;}
        public string row { set; get; }
        public string column { set; get; }

        public Tile_Info()
        {
            level = "";
            row = "";
            column = "";
        }
    }
}
