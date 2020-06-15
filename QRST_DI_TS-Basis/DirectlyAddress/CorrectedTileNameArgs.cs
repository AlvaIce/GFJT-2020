using System;
using QRST_DI_Resources;

namespace QRST_DI_TS_Basis.DirectlyAddress
{
    public class CorrectedTileNameArgs : TileNameArgs
    { 

        /*新瓦片命名规则
影像数据：HJ1A_CCD1_2001030507_L20000521090_FFFF_4_241_599-1.c.tif
                HJ1A_CCD1_2001030524_L20000521090_6402_4_241_599.c.png
                卫星_传感器_年月日时(2001030507要补足10位，小时未知时用24）_源数据id_满幅度云量（16进制，未知填FF）_层级_行号_列号-波段.小型号（c代表影像数据，p代表产品，s代表样本）.tif（快视图为.png，不含波段信息）
         * HJ1A_CCD1_2001121224_L20000521090_6402_4_241_599-1.c.tif
         *HJ1A_CCD1_2011121224_L20000521090_6402_4_241_599.c.png
         *HJ1A_CCD1_2011121224_L20000521092_6402_5_243_599-Ami.c.tif
         *[0]HJ1A
         *[1]CCD1
         *[2]2011112122     (到小时，文件夹只到日)
         *[3]L20000521092
         *[4]6402（满幅度、云量16进制）
         *[5]5
         *[6]243
         *[7]599-1.c.tif 599.c.png 599-Ami.c.tif     
         *path:\\172.16.0.185\QRST_DB_Tile\0\4\HJ1A\CCD1\20111212\HJ1A_CCD1_2001121224_L20000521090_5442_4_241_599-1.tif
         *newPath:\\172.16.0.185\QRST_DB_Tile\0\4\241\599\CCD1\20111212\HJ1A_CCD1_2001121224_L20000521090_5442_4_241_599-1.tif
         *\\ip\QRST_DB_Tile\配号\层级\行号\列号\传感器\日期（精确到日）\文件名
         *joki 131218 支持瓦片号定为查找
         */

        public static string GetTileName(string satellite, string sensor, string datetime, string srcIDs, string level, string row, string col, string datatype, bool oldNameStyle = false)
        {
            if (oldNameStyle)
            {
                //HJ1A_CCD1_2001121223_L20000521090_4_241_599-1.tif
                string tilename = string.Format("{0}_{1}_{2}_{3}_{4}_{5}_{6}", satellite, sensor, datetime.Trim().Substring(0, 8), srcIDs, level, row, col);
                if (datatype.Trim().ToLower() == "preview")
                {
                    tilename = tilename + ".png";
                }
                else
                {
                    tilename = tilename + string.Format("-{0}.tif", datatype.Trim());
                }
                return tilename;
            }
            else
            {
                /*HJ1A_CCD1_2001121224_L20000521090_FFFF_4_241_599-1.c.tif
                 *HJ1A_CCD1_2011121224_L20000521090_FFFF_4_241_599.c.png
                 *HJ1A_CCD1_2011121224_L20000521092_FFFF_5_243_599-Ami.c.tif
                 */

                string tilename = string.Format("{0}_{1}_{2}_{3}_FFFF_{4}_{5}_{6}", satellite, sensor, (datetime.Length == 8) ? datetime + "24" : datetime, srcIDs, level, row, col);
                if (datatype.Trim().ToLower() == "preview")
                {
                    tilename = tilename + ".c.png";
                }
                else
                {
                    tilename = tilename + string.Format("-{0}.c.tif", datatype.Trim());
                }

                return tilename;
            }
        }

        /// <summary>
        /// 云量和满幅度输入10进制0-100
        /// HJ1A_CCD1_2011041923-6400_L20000521090-L20000521097_4_248_592.tif
        /// </summary>
        /// <param name="satellite"></param>
        /// <param name="sensor"></param>
        /// <param name="datetime"></param>
        /// <param name="srcIDs"></param>
        /// <param name="availibility">满幅度10进制0-100</param>
        /// <param name="cloud">云量10进制0-100</param>
        /// <param name="level"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="datatype">1 2 3 4 Preview Azimuth Zenith</param>
        /// <returns></returns>
        public static string GetTileName(string satellite, string sensor, string datetime, string srcIDs, int availibility, int cloud, string level, string row, string col, string datatype, bool oldNameStyle = false)
        {
            if (availibility == 255 && cloud == 255)
            {
                //应该取消掉
                return GetTileName(satellite, sensor, datetime, srcIDs, level, row, col, datatype, oldNameStyle);
            }

            if (oldNameStyle)
            {
                //HJ1A_CCD1_2001121223_L20000521090_4_241_599-1.tif
                string tilename = string.Format("{0}_{1}_{2}_{3}#{4}{5}_{6}_{7}_{8}", satellite, sensor, datetime.Trim().Substring(0, 8), srcIDs, availibility.ToString("X2"), cloud.ToString("X2"), level, row, col);
                if (datatype.Trim().ToLower() == "preview")
                {
                    tilename = tilename + ".png";
                }
                else
                {
                    tilename = tilename + string.Format("-{0}.tif", datatype.Trim());
                }
                return tilename;
            }
            else
            {
                /*HJ1A_CCD1_2001121224_L20000521090_FFFF_4_241_599-1.c.tif
                 *HJ1A_CCD1_2011121224_L20000521090_FFFF_4_241_599.c.png
                 *HJ1A_CCD1_2011121224_L20000521092_FFFF_5_243_599-Ami.c.tif
                 */

                string tilename = string.Format("{0}_{1}_{2}_{3}_{4}{5}_{6}_{7}_{8}", satellite, sensor, (datetime.Length == 8) ? datetime + "24" : datetime, srcIDs, availibility.ToString("X2"), cloud.ToString("X2"), level, row, col);
                if (datatype.Trim().ToLower() == "preview")
                {
                    tilename = tilename + ".c.png";
                }
                else
                {
                    tilename = tilename + string.Format("-{0}.c.tif", datatype.Trim());
                }

                return tilename;
            }
        }
        public string Satellite { get; set; }
        public string Sensor { get; set; }
        public string SrcProdID { get; set; }
        public string DataType { get; set; }
        public int Availability { get; set; }
        public int Cloud { get; set; }


        public CorrectedTileNameArgs(string filename)
        {  
            //增加了Availability和Cloud字段，需要更新
            base.Created = false;
            base.IsThumbnail = false;
            base.IsOldNameStyle = false;
            base.Type = TileType.CorrectedTile;
            Availability = 255;
            Cloud = 255;
            Filename = filename;
            string[] args = filename.Split("_".ToCharArray());
            string ext = System.IO.Path.GetExtension(filename).ToLower();

            if (!(ext == ".png" || ext == ".jpg" || ext == ".tif" || ext == ".pgw" || ext == ".jgw" || ext == ".tfw"))
            {
                return;
            }

            string tilesubtype = GetTileSubType(filename);
            if (tilesubtype == "c" && args.Length == 8)
            {
                /*HJ1A_CCD1_2001121224_L20000521090_6402_4_241_599-1.c.tif
                 *HJ1A_CCD1_2011121224_L20000521090_6402_4_241_599.c.png
                 *HJ1A_CCD1_2011121224_L20000521092_6402_5_243_599-Ami.c.tif
                 */

                Satellite = args[0];
                Sensor = args[1];
                DateTime = args[2];
                SrcProdID = args[3];
                Availability = Convert.ToInt16(args[4].Substring(0, 2), 16);
                Cloud = Convert.ToInt16(args[4].Substring(2, 2), 16);

                TileLevel = args[5];
                Row = args[6];
                if (args[7].Contains("-"))
                {
                    Col = args[7].Substring(0, args[7].IndexOf("-"));
                    DataType = args[7].Substring(args[7].IndexOf("-") + 1, args[7].IndexOf(".") - args[7].IndexOf("-") - 1);
                    if (DataType.ToLower() == "tiny" || DataType.ToLower() == "mini")
                    {
                        IsThumbnail = true;
                        Created = false;
                        return;
                    }
                }
                else
                {
                    Col = args[7].Substring(0, args[7].IndexOf("."));
                    DataType = "Preview";
                }

                if (DataType.Trim().Length==0)
                {
                    Created = false;
                    return;
                }
                Created = true;
            }
            else if (tilesubtype == "" && args.Length == 7)
            {
                //HJ1A_CCD1_20011212_L20000521090_4_241_599-1.tif
                //HJ1A_CCD1_20011212_L20000521090_4_241_599.png
                //HJ1A_CCD1_20011213_L20000521090#6400_4_241_599-1.tif
                //HJ1A_CCD1_20011213_L20000521090#6400_4_241_599.png

                Satellite = args[0];
                Sensor = args[1];
                DateTime = args[2];
                SrcProdID = args[3];
                if (SrcProdID.Contains("#"))
                {
                    string[] elem = SrcProdID.Split("#".ToCharArray());
                    SrcProdID = elem[0];
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
                }

                TileLevel = args[4];
                Row = args[5];
                if (args[6].Contains("-"))
                {
                    Col = args[6].Substring(0, args[6].IndexOf("-"));
                    DataType = args[6].Substring(args[6].IndexOf("-") + 1, args[6].IndexOf(".") - args[6].IndexOf("-") - 1);
                    if (DataType.ToLower() == "tiny" || DataType.ToLower() == "mini")
                    {
                        IsThumbnail = true;
                        Created = false;
                        return;
                    }
                }
                else
                {
                    Col = args[6].Substring(0, args[6].IndexOf("."));
                    DataType = "Preview";
                }


                Created = true;
                base.IsOldNameStyle = true;
            }            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="storageIP"></param>
        /// <returns>“\\172.16.0.185\QRST_DB_Tile\0\4\241\599\CCD1\20111212\HJ1A_CCD1_2001121223_L20000521090_4_241_599-1.tif”</returns>
        public override string GetFilePath(string storageIP, string ipMod)
        {

            if (!Created && !IsThumbnail)
            {
                return "-1";
            }

            string path = null;
            //增加单机本地存储路径
            switch (Constant.DbStorage)
            {
                case EnumDbStorage.MULTIPLE:
                    path = string.Format(@"\\{0}\{1}\{2}\{3}\{4}\{5}\{6}\{7}\{8}",
               storageIP, StorageBasePath.QRST_DB_Tile, ipMod, TileLevel, Row, Col, Sensor, DateTime.Trim().Substring(0, 8), Filename);

                    break;
                case EnumDbStorage.SINGLE:
                           path = string.Format(@"{0}\{1}\{2}\{3}\{4}\{5}\{6}\{7}\{8}",
            Constant.PcDBRootPath, StorageBasePath.QRST_DB_Tile, ipMod, TileLevel, Row, Col, Sensor, DateTime.Trim().Substring(0, 8), Filename);
                    break;
            }
           
            //过渡期，瓦片快视图可能是jpg或png
            if (!System.IO.File.Exists(path) && System.IO.Path.GetExtension(Filename).ToLower().Equals(".png"))
            {
                string pathtmp = path.Substring(0, path.Length - 4);
                pathtmp = string.Format("{0}.jpg", path);
                if (System.IO.File.Exists(pathtmp))         //zsm
                {
                    path = pathtmp;
                }
            }
            //过渡期，瓦片快视图可能是jpg或png
            if (!System.IO.File.Exists(path) && System.IO.Path.GetExtension(Filename).ToLower().Equals(".jpg"))
            {
                string pathtmp = path.Substring(0, path.Length - 4);
                pathtmp = string.Format("{0}.png", path);
                if (System.IO.File.Exists(pathtmp))         //zsm
                {
                    path = pathtmp;
                }
            }
            return path;
        }


        public override string GetRelatePath(string ipMod)
        {

            if (!Created && !IsThumbnail)
            {
                return "-1";
            }

            string path = string.Format(@"{0}\{1}\{2}\{3}\{4}\{5}\{6}\{7}", StorageBasePath.QRST_DB_Tile, ipMod, TileLevel, Row, Col, Sensor, DateTime.Trim().Substring(0, 8), Filename);

            //过渡期，瓦片快视图可能是jpg或png
            if (!System.IO.File.Exists(path) && System.IO.Path.GetExtension(Filename).ToLower().Equals(".png"))
            {
                path = path.Substring(0, path.Length - 4);
                path = string.Format("{0}.jpg", path);
            }
            //过渡期，瓦片快视图可能是jpg或png
            if (!System.IO.File.Exists(path) && System.IO.Path.GetExtension(Filename).ToLower().Equals(".jpg"))
            {
                path = path.Substring(0, path.Length - 4);
                path = string.Format("{0}.png", path);
            }
            return path;
        }


        public override string GetNewStyleFilename()
        {
            if (!Created && !IsThumbnail)
            {
                return "-1";
            }

            //bug  .pgw会被改为.png 保留原来后缀名
            string newfilename = GetTileName(Satellite, Sensor, DateTime, SrcProdID, Availability, Cloud, TileLevel, Row, Col, DataType, false);
            newfilename = System.IO.Path.GetFileNameWithoutExtension(newfilename) + System.IO.Path.GetExtension(Filename);

            //更新
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

            string oldfilename = GetTileName(Satellite, Sensor, DateTime, SrcProdID, Availability, Cloud, TileLevel, Row, Col, DataType, true);
            oldfilename = System.IO.Path.GetFileNameWithoutExtension(oldfilename) + System.IO.Path.GetExtension(Filename);

            //更新
            if (IsOldNameStyle)
            {
                Filename = oldfilename;
            }
            return oldfilename;
        }
    }
}
