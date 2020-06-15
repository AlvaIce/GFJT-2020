using System;
using QRST_DI_Resources;
 
namespace QRST_DI_TS_Basis.DirectlyAddress
{
    public class ProdTileNameArgs : TileNameArgs
    {
        /*产品类型_年月日时(2001030507要补足10位，小时未知时用24）_源数据id（多个源数据用-分隔）_满幅度云量（16进制，未知填FF）_层级_行号_列号.小型号（c代表影像数据，p代表产品，s代表样本）.tif（快视图为.png）
        /*NDVI_2011041924_L20000521090-L20000521097_6401_4_248_592.p.tif
         *BRDF_2012041924_L20000521093-L20000521097_6401_5_248_598.p.tif
         *EVI_2010041924_L20000521093-L20000521097_6401_4_248_598.p.tif
         *[0]NDVI
         *[1]2011041923     (到小时，文件夹只到日)
         *[2]L20000521090-L20000521097     
         *[3]4
         *[4]248
         *[5]592.tif 599.jpg  
         *path:\\172.16.0.185\QRST_DB_Tile\0\4\NDVI\20111212\NDVI_2011041923_L20000521090-L20000521097_4_248_592.tif
         *newPath:\\172.16.0.185\QRST_DB_Tile\0\4\241\599\NDVI\20111212\NDVI_2011041923_L20000521090-L20000521097_4_241_599.tif   joki 131218 支持瓦片号定位查找
         */
        public string ProdType { get; set; }
        public string[] SrcProdIDs { get; set; }
        public string SrcProdIDsString { get; set; }
        public int Availability { get; set; }
        public int Cloud { get; set; }

        public static string GetTileName(string prodtype, string datetime, string srcIDs, string level, string row, string col,bool oldNameStyle=false)
        {
            if (oldNameStyle)
            {
                //NDVI_2011041923_L20000521090-L20000521097_4_241_599.tif
                string tilename = string.Format("{0}_{1}_{2}_{3}_{4}_{5}.tif", prodtype, datetime.Trim().Substring(0, 8), srcIDs, level, row, col);

                return tilename;
            }
            else
            {
                /*NDVI_2011041924_L20000521090-L20000521097_FFFF_4_248_592.p.tif
                 *BRDF_2012041924_L20000521093-L20000521097_FFFF_5_248_598.p.tif
                 *EVI_2010041924_L20000521093-L20000521097_FFFF_4_248_598.p.tif
                         */
                string tilename = string.Format("{0}_{1}_{2}_FFFF_{3}_{4}_{5}.p.tif", prodtype, (datetime.Length == 8) ? datetime + "24" : datetime, srcIDs, level, row, col);

                return tilename;
            }
        }

        /// <summary>
        /// 云量和满幅度输入10进制0-100
        /// NDVI_2011041923-6400_L20000521090-L20000521097_4_248_592.tif
        /// </summary>
        /// <param name="prodtype"></param>
        /// <param name="datetime"></param>
        /// <param name="srcIDs"></param>
        /// <param name="availibility">满幅度10进制0-100</param>
        /// <param name="cloud">云量10进制0-100</param>
        /// <param name="level"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public static string GetTileName(string prodtype, string datetime, string srcIDs, int availibility, int cloud, string level, string row, string col, bool oldNameStyle = false)
        {
            if (availibility == 255 && cloud == 255)
            {
                //应该取消掉
                return GetTileName(prodtype, datetime, srcIDs, level, row, col, oldNameStyle);
            }

            if (oldNameStyle)
            {
                //NDVI_2011041923_L20000521090-L20000521097#6444_4_241_599.tif
                string tilename = string.Format("{0}_{1}_{2}#{3}{4}_{5}_{6}_{7}.tif", prodtype, datetime.Trim().Substring(0, 8), srcIDs, availibility.ToString("X2"), cloud.ToString("X2"), level, row, col);
                return tilename;
            }
            else
            {
                /*NDVI_2011041924_L20000521090-L20000521097_6401_4_248_592.p.tif
                 *BRDF_2012041924_L20000521093-L20000521097_6401_5_248_598.p.tif
                 *EVI_2010041924_L20000521093-L20000521097_6401_4_248_598.p.tif
                         */
                string tilename = string.Format("{0}_{1}_{2}_{3}{4}_{5}_{6}_{7}.p.tif", prodtype, (datetime.Length == 8) ? datetime + "24" : datetime, srcIDs, availibility.ToString("X2"), cloud.ToString("X2"), level, row, col);

                return tilename;
            }
        }

        public ProdTileNameArgs(string filename)
        {   //增加了Availability和Cloud字段，需要更新
            base.IsThumbnail = false;
            base.Created = false;
            base.IsOldNameStyle = false;
            base.Type = TileType.ProdTile;
            Availability = 255;
            Cloud = 255;
            Filename = filename;
            string[] args = filename.Split("_".ToCharArray());
            string ext = System.IO.Path.GetExtension(filename).ToLower();

            if (!(ext == ".png"|| ext == ".jpg" || ext == ".tif"||ext == ".pgw"|| ext == ".jgw" || ext == ".tfw"))
            {
                return;
            }

            string tilesubtype = GetTileSubType(filename);

            if (tilesubtype == "p" && args.Length == 7)
            {
                /*NDVI_2011041924_L20000521090-L20000521097_6401_4_248_592.p.tif
                 *BRDF_2012041924_L20000521093-L20000521097_6401_5_248_598.p.tif
                 *EVI_2010041924_L20000521093-L20000521097_6401_4_248_598.p.tif
                 */

                ProdType = args[0];
                DateTime = args[1];
                SrcProdIDsString = args[2];
                SrcProdIDs = args[2].Split("-".ToCharArray());
                try
                {
                    Availability = Convert.ToInt16(args[3].Substring(0, 2), 16);
                    Cloud = Convert.ToInt16(args[3].Substring(2, 2), 16);
                }
                catch
                {
                    Availability = 255;
                    Cloud = 255;
                }

                TileLevel = args[4];
                Row = args[5];
                if (args[6].Contains("-"))
                {
                    Col = args[6].Substring(0, args[6].IndexOf("-"));
                    string dtype = args[6].Substring(args[6].IndexOf("-") + 1, args[6].IndexOf(".") - args[6].IndexOf("-") - 1);
                    if (dtype.ToLower() == "tiny" || dtype.ToLower() == "mini")
                    {
                        IsThumbnail = true;
                        Created = false;
                        return;
                    }
                }
                else
                {
                    Col = args[6].Substring(0, args[6].IndexOf("."));
                }
                Created = true;
            }
            else if (tilesubtype == "" && args.Length == 6)
            {
                //NDVI_2011041923_L20000521090-L20000521097#6444_4_241_599.tif

                ProdType = args[0];
                DateTime = args[1];
                SrcProdIDsString = (args[2].IndexOf("#") != -1) ? args[2].Substring(0, args[2].LastIndexOf("#")) : args[2];
                SrcProdIDs = args[2].Split("-".ToCharArray());
                string[] elem = SrcProdIDs[SrcProdIDs.Length - 1].Split("#".ToCharArray());
                SrcProdIDs[SrcProdIDs.Length - 1] = elem[0];
                try
                {
                    Availability = Convert.ToInt16(elem[1].Substring(0, 2), 16);
                    Cloud = Convert.ToInt16(elem[1].Substring(2, 2), 16);
                }
                catch
                {
                    Availability = 255;
                    Cloud = 255;
                }

                TileLevel = args[3];
                Row = args[4];
                if (args[5].Contains("-"))
                {
                    Col = args[5].Substring(0, args[5].IndexOf("-"));
                    string dtype = args[6].Substring(args[6].IndexOf("-") + 1, args[6].IndexOf(".") - args[6].IndexOf("-") - 1);
                    if (dtype.ToLower() == "tiny" || dtype.ToLower() == "mini")
                    {
                        IsThumbnail = true;
                        Created = false;
                        return;
                    }
                }
                else
                {
                    Col = args[5].Substring(0, args[5].IndexOf("."));
                }
                Created = true;
                base.IsOldNameStyle = true;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="storageIP"></param>
        /// <returns>“\\172.16.0.185\QRST_DB_Tile\0\4\241\599\NDVI\20111212\NDVI_2001121223_L20000521090_4_241_599.tif”</returns>
        public override string GetFilePath(string storageIP, string ipMod)
        {
            if (!Created&&!IsThumbnail)
            {
                return "-1";
            }
            string path = null;
            switch (Constant.DbStorage)
            {
                case EnumDbStorage.MULTIPLE:
                    path = string.Format(@"\\{0}\{1}\{2}\{3}\{4}\{5}\{6}\{7}\{8}",
               storageIP, StorageBasePath.QRST_DB_Tile, ipMod, TileLevel, Row, Col, ProdType, DateTime.Trim().Substring(0, 8), Filename);
                    break;
                case EnumDbStorage.SINGLE:
                    path = string.Format(@"{0}\{1}\{2}\{3}\{4}\{5}\{6}\{7}\{8}",
            Constant.PcDBRootPath, StorageBasePath.QRST_DB_Tile, ipMod, TileLevel, Row, Col, ProdType, DateTime.Trim().Substring(0, 8), Filename);
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

            string path = string.Format(@"{0}\{1}\{2}\{3}\{4}\{5}\{6}\{7}",
                 StorageBasePath.QRST_DB_Tile, ipMod, TileLevel, Row, Col, ProdType, DateTime.Trim().Substring(0, 8), Filename);
            return path;
        }

        public override string GetNewStyleFilename()
        {
            if (!Created && !IsThumbnail)
            {
                return "-1";
            }

            string newfilename = GetTileName(ProdType, DateTime, SrcProdIDsString, Availability, Cloud, TileLevel, Row, Col, false);
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

                string oldfilename = GetTileName(ProdType, DateTime, SrcProdIDsString, Availability, Cloud, TileLevel, Row, Col, true);
                oldfilename = System.IO.Path.GetFileNameWithoutExtension(oldfilename) + System.IO.Path.GetExtension(Filename);


            if (IsOldNameStyle)
            {
                 Filename = oldfilename;
            }
            return oldfilename;
        }
    }
}
