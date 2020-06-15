using QRST_DI_Resources;

namespace QRST_DI_TS_Basis.DirectlyAddress
{
    public class ClassifySampleTileNameArgs : TileNameArgs
    {
        /* 分类样本CS标识与样本类型编号_样本序号_年月日时(2001030507要补足10位，小时未知时用24）_源数据id_层级_行号_列号.小型号（c代表影像数据，p代表产品，s代表样本）.tiff（快视图为.png）
        /*CS101121_11_2014112424_L20000521097_4_248_592.s.tiff
         *[0]CS 表示分类样本
         *[1]101121     样本类型编号
         *[2]11 该样本记录序号     
         *[3]20141121 数据时间
         *[4]L20000521097 原数据唯一码
         *[5]4 层级
         *[6]248 列号
         *[7]592 行号  
         *newPath:\\172.16.0.185\QRST_DB_Tile\0\4\248\592\CS\20141121\CS_101121_11_20141121_L20000521097_4_248_592.tiff
         */
 
        public static string GetTileName(string categoryCode, string sampleTypeID, string datetime, string srcTileIDs, string level, string row, string col, bool oldNameStyle = false)
        {
            //CS_101121_11_20141121_L20000521097_4_248_592.tiff
            if (oldNameStyle)
            {
                string tilename = string.Format("CS_{0}_{1}_{2}_{3}_{4}_{5}_{6}.tiff", categoryCode, sampleTypeID, datetime.Trim().Substring(0, 8), srcTileIDs, level, row, col);

                return tilename;
            }
            else
            {
                //CS101121_11_2014112424_L20000521097_4_248_592.s.tiff
                string tilename = string.Format("CS{0}_{1}_{2}_{3}_{4}_{5}_{6}.s.tiff", categoryCode, sampleTypeID, (datetime.Length == 8) ? datetime + "24" : datetime, srcTileIDs, level, row, col);

                return tilename;
            }
        }

        public string CategoryCode { get; set; }
        public string SrcTileID { get; set; }
        public string SampleTypeID { get; set; }
        public ClassifySampleTileNameArgs(string filename)
        {
            base.IsThumbnail = false;
            base.Created = false;
            base.IsOldNameStyle = false;
            base.Type = TileType.ClassifySampleTile;
            Filename = filename;
            string[] args = filename.Split("_".ToCharArray());
            string ext = System.IO.Path.GetExtension(filename).ToLower();


            if (!(ext == ".png" || ext == ".jpg" || ext == ".tif" || ext == ".tiff" || ext == ".pgw" || ext == ".jgw" || ext == ".tfw"))
            {
                return;
            }

            string tilesubtype = GetTileSubType(filename);

            if (tilesubtype == "s" && args.Length == 7)
            {
                /*CS101121_11_20141124_L20000521097_4_248_592.s.tiff*/

                CategoryCode = args[0].Substring(2);
                SampleTypeID = args[1];
                DateTime = args[2];
                SrcTileID = args[3];
                TileLevel = args[4];
                Row = args[5];
                Col = args[6].Substring(0, args[6].IndexOf("."));

                Created = true;
            }
            else if (tilesubtype == "" && args.Length == 8)
            {
                //CS_101121_11_20141121_L20000521097_4_248_592.tiff

                CategoryCode = args[1];
                SampleTypeID = args[2];
                DateTime = args[3];
                SrcTileID = args[4];
                TileLevel = args[5];
                Row = args[6];
                Col = args[7].Substring(0, args[7].IndexOf("."));
                Created = true;
                base.IsOldNameStyle = true;
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="storageIP"></param>
        /// <returns>\\172.16.0.185\QRST_DB_Tile\0\4\248\592\CS\20141121\CS_101121_11_20141121_L20000521097_4_248_592.tiff</returns>
        public override string GetFilePath(string storageIP, string ipMod)
        {
            if (!Created && !IsThumbnail)
            {
                return "-1";
            }

            string path=null ;
            switch (Constant.DbStorage)
            {
                case EnumDbStorage.SINGLE:
                    path = string.Format(@"{0}\{1}\{2}\{3}\{4}\{5}\{6}\{7}\{8}",
Constant.PcDBRootPath, StorageBasePath.QRST_DB_Tile, ipMod, TileLevel, Row, Col, CategoryCode, DateTime.Trim().Substring(0, 8), Filename);

                    break;
                case EnumDbStorage.MULTIPLE:
                    path = string.Format(@"\\{0}\{1}\{2}\{3}\{4}\{5}\{6}\{7}\{8}",
storageIP, StorageBasePath.QRST_DB_Tile, ipMod, TileLevel, Row, Col, CategoryCode, DateTime.Trim().Substring(0, 8), Filename);
                    break;
               
            }
            return path;
        }

        public override string GetRelatePath(string ipMod)
        {
            if (!Created && !IsThumbnail)
            {
                return "-1";
            }

            string path = string.Format(@"{0}\{1}\{2}\{3}\{4}\{5}\{6}\{7}\{8}",
                 StorageBasePath.QRST_DB_Tile, ipMod, TileLevel, Row, Col, CategoryCode, DateTime.Trim().Substring(0, 8), Filename);
            return path;
        }

        public override string GetNewStyleFilename()
        {
            if (!Created && !IsThumbnail)
            {
                return "-1";
            }

            string newfilename = GetTileName(CategoryCode, SampleTypeID, DateTime, SrcTileID, TileLevel, Row, Col, false); ;
            newfilename = System.IO.Path.GetFileNameWithoutExtension(newfilename) + System.IO.Path.GetExtension(Filename);

            if (!IsOldNameStyle)
            {
                Filename = newfilename;
            }
            return newfilename;
        }

        public override string GetOldStyleFilename()
        {
            if (!Created && !IsThumbnail)
            {
                return "-1";
            }

            string oldfilename = GetTileName(CategoryCode, SampleTypeID, DateTime, SrcTileID, TileLevel, Row, Col, true);
            oldfilename = System.IO.Path.GetFileNameWithoutExtension(oldfilename) + System.IO.Path.GetExtension(Filename);


            if (IsOldNameStyle)
            {
                Filename = oldfilename;
            }
            return oldfilename;
        }
    }
}