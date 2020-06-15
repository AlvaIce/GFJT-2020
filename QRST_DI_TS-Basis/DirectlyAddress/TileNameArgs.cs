using System.IO;
 
namespace QRST_DI_TS_Basis.DirectlyAddress
{
    public abstract class TileNameArgs
    {
        public enum TileType
        {
            ProdTile = 0,
            CorrectedTile = 1,
            ClassifySampleTile = 2
        }

        public string Filename { get; set; }
        public bool Created { get; set; }
        public bool IsOldNameStyle { get; set; }
        public bool IsThumbnail { get; set; }
        public TileType Type { get; set; }
        public string TileLevel { get; set; }
        public string Col { get; set; }
        public string Row { get; set; }
        public string DateTime { get; set; }
        public abstract string GetFilePath(string storageIP, string ipMod);
        //获取相对路径
        public abstract string GetRelatePath(string ipMod);
        public abstract string GetOldStyleFilename();
        public abstract string GetNewStyleFilename();


        /// <summary>
        /// 根据文件名提取切片的属性参数
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static TileNameArgs GetTileNameArgs(string filename)
        {
            TileNameArgs tnargs = new CorrectedTileNameArgs(filename);
            if (!tnargs.Created)
            {
                tnargs = new ProdTileNameArgs(filename);
            }
            else if (!tnargs.Created)
            {
                tnargs = new ClassifySampleTileNameArgs(filename);
            }
            return tnargs;
        }


        public static bool GetTilePrefixWithoutAvailabilityCloud(string filename, out string tilePrefix)
        {
            tilePrefix = "";

            TileNameArgs tna = GetTileNameArgs(filename);
            if (!tna.Created)
            {
                return false;
            }
            else
            {
                switch (tna.Type)
                {
                    case TileNameArgs.TileType.ProdTile:
                        tilePrefix = Path.GetFileNameWithoutExtension(ProdTileNameArgs.GetTileName(((ProdTileNameArgs)tna).ProdType, ((ProdTileNameArgs)tna).DateTime, ((ProdTileNameArgs)tna).SrcProdIDsString, ((ProdTileNameArgs)tna).TileLevel, ((ProdTileNameArgs)tna).Row, ((ProdTileNameArgs)tna).Col));
                        return true;
                    case TileNameArgs.TileType.CorrectedTile:
                        tilePrefix = Path.GetFileNameWithoutExtension(CorrectedTileNameArgs.GetTileName(((CorrectedTileNameArgs)tna).Satellite, ((CorrectedTileNameArgs)tna).Sensor, ((CorrectedTileNameArgs)tna).DateTime, ((CorrectedTileNameArgs)tna).SrcProdID, ((CorrectedTileNameArgs)tna).TileLevel, ((CorrectedTileNameArgs)tna).Row, ((CorrectedTileNameArgs)tna).Col, ((CorrectedTileNameArgs)tna).DataType));
                        return true;
                    case TileNameArgs.TileType.ClassifySampleTile:
                        tilePrefix = Path.GetFileNameWithoutExtension(ClassifySampleTileNameArgs.GetTileName(((ClassifySampleTileNameArgs)tna).CategoryCode, ((ClassifySampleTileNameArgs)tna).SampleTypeID, ((ClassifySampleTileNameArgs)tna).DateTime, ((ClassifySampleTileNameArgs)tna).SrcTileID, ((ClassifySampleTileNameArgs)tna).TileLevel, ((ClassifySampleTileNameArgs)tna).Row, ((ClassifySampleTileNameArgs)tna).Col));
                        //没有满幅度云量信息
                        return true;
                    default:
                        return false;
                }
            }
        }

        public static string GetTileSubType(string tilename)
        {
            string[] splitstrs = Path.GetFileNameWithoutExtension(tilename).Split('.');
            if (splitstrs.Length>1)
            {
                return splitstrs[splitstrs.Length - 1].Trim().ToLower();
            }
            else
            {
                return "";
            }
        }
    }
}
