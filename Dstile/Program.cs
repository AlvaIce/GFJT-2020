using System;
using System.Collections.Generic;
using ProjNet.CoordinateSystems;
using System.IO;
using System.Threading.Tasks;
using System.Drawing;
using ProjNet.CoordinateSystems.Projections;

namespace Dstile
{
    public class Dstile
    {
        public static int CPUCORE = 16;//CPU的内核
        public static bool IsDEM = false;
        public static Dictionary<int, double> DicLevelDegree = new Dictionary<int, double>();
        /// <summary>
        /// 切影像瓦片
        /// </summary>
        /// <param name="args">4个参数。
        /// 参数1：输入影像路径；
        /// 参数2：输出路径
        /// 参数3：起始层级
        /// 参数4：切片层数</param>
        static void Main(string[] args)
        {
            DicLevelDegree.Add(0, 36);
            DicLevelDegree.Add(1, 18);
            DicLevelDegree.Add(2, 9);
            DicLevelDegree.Add(3, 4.5);
            DicLevelDegree.Add(4, 2.25);
            DicLevelDegree.Add(5, 1.125);
            DicLevelDegree.Add(6, 0.5625);
            DicLevelDegree.Add(7, 0.28125);
            DicLevelDegree.Add(8, 0.140625);
            DicLevelDegree.Add(9, 0.0703125);
            DicLevelDegree.Add(10, 0.03515625);
            DicLevelDegree.Add(11, 0.17578125);
            DicLevelDegree.Add(12, 0.008789063);
            DicLevelDegree.Add(13, 0.004394531);
            DicLevelDegree.Add(14, 0.002197266);
            DicLevelDegree.Add(15, 0.001098633);
            DicLevelDegree.Add(16, 0.000549316);
            /*级别  瓦片跨度    像素分辨率  列数    行数    瓦片个数
             *                 (111322米/度)
             * 0	36	        7827.34	    10	    5	    50
             * 1	18	        3913.67	    20	    10	    200
             * 2	9	        1956.835	40	    20	    800
             * 3	4.5	        978.4175	80	    40	    3200
             * 4	2.25	    489.20875	160	    80	    12800
             * 5	1.125	    244.604375	320	    160	    51200
             * 6	0.5625	    122.3021875	640	    320	    204800
             * 7	0.28125	    61.15109375	1280	640	    819200
             * 8	0.140625	30.57554688	2560	1280	3276800
             * 9	0.0703125	15.28777344	5120	2560	13107200
             * 10	0.03515625	7.643886719	10240	5120	52428800
             * 11	0.017578125	3.821943359	20480	10240	209715200
             * 12	0.008789063	1.91097168	40960	20480	838860800
             * 13	0.004394531	0.95548584	81920	40960	3355443200
             * 14	0.002197266	0.47774292	163840	81920	13421772800
             * 15	0.001098633	0.23887146	327680	163840	53687091200
             * 16	0.000549316	0.11943573	655360	327680	2.14748E+11
            */

            
            //args = new string[] { @"D:\testimage\testimage\worlddem.tif", @"D:\testimage\testimageoutFinal", "3", "1", "DEM" };//接收4个参数
            

            string inputDir = Path.GetDirectoryName(args[0]);//1.输入文件夹路径
            string outputDir = args[1];//2.保存文件夹路径
            int startlevel=Convert.ToInt16(args[2]);
            double lztsd = DicLevelDegree[startlevel];//3.第0层每个格网的度数
            int levelCount = Convert.ToInt32(args[3]);//4.总共要切分几层

            if (args.Length < 4)
                Console.WriteLine("输入参数不正确！示例： inputDir,outputDir,lztsd,levelCount");
            if (args.Length == 5 && args[4] == "DEM")
            {
                IsDEM = true;
            }

            object thislock = new object();//安全锁，为了并行共享统一资源，而不发生抢的现象

            Envelop mapEnvelop = new Envelop();//获取地图的最大、最小范围
            string[] files = new string[] { args[0] };  //Directory.GetFiles(inputDir,"*.jpg");
            for (int fCount = 0; fCount < files.Length; fCount++)
            {

                try
                {
                    string inputFile = files[fCount];
                    Envelop imageEnvelop = Dstile.GetEnvelop(inputFile, false, "GEOGCS[\"WGS 84\",DATUM[\"WGS_1984\",SPHEROID[\"WGS 84\",6378137,298.2572326660126,AUTHORITY[\"EPSG\",\"7030\"]],AUTHORITY[\"EPSG\",\"6326\"]],PRIMEM[\"Greenwich\",0],UNIT[\"degree\",0.0174532925199433],AUTHORITY[\"EPSG\",\"4326\"]]");

                    if (fCount == 0)
                    {
                        mapEnvelop.East = imageEnvelop.East;
                        mapEnvelop.West = imageEnvelop.West;
                        mapEnvelop.North = imageEnvelop.North;
                        mapEnvelop.South = imageEnvelop.South;
                        continue;
                    }
                    getMapEnvelop(imageEnvelop, ref mapEnvelop);

                }
                catch { }
            }

            //开始进行切图
            for (int i = 0; i < levelCount; i++)
            {
                Level levelInfo = new Level(lztsd, startlevel + i);//存储当前Level的信息
                int yCount = 0;
                int xCount = 0;
                int startX = 0;
                int startY = 0;
                string localpath = "";
                //根据地图的最大最小范围，计算在当前层所处的起始格网号，和总共在该层占了几个格网
                yCount = (int)((mapEnvelop.North - mapEnvelop.South) / levelInfo.LevelPerDegree + 1);
                xCount = (int)((mapEnvelop.East - mapEnvelop.West) / levelInfo.LevelPerDegree + 1);
                startX = (int)((mapEnvelop.West + 180) / levelInfo.LevelPerDegree);
                startY = (int)((mapEnvelop.South + 90) / levelInfo.LevelPerDegree);

                localpath = "";
                localpath = outputDir + "\\" + levelInfo.LevelNumber.ToString();
                //循环获取每个格网的缩略图
                for (int y = startY; y <= startY + yCount; y++)
                {
                    //创建文件夹
                    localpath = outputDir + "\\" + levelInfo.LevelNumber.ToString() + "\\" + string.Format("{0}", y);
                    if (!Directory.Exists(localpath))
                    {
                        Directory.CreateDirectory(localpath);
                    }

                    //存储单个格网的信息，当达到CPU内核个数的时候，就开始多核并行切图
                    Tile[] tiles = new Tile[CPUCORE];
                    int tileIndex = 0;
                    for (int x = startX; x <= startX + xCount; x++)
                    {
                        #region 存储当前切片的相关信息
                        SharpMap.Map thismap = new SharpMap.Map(new Size(512, 512));
                        for (int fCount = 0; fCount < files.Length; fCount++)
                        {
                            string inputFile = files[fCount];
                            SharpMap.Layers.GdalGridRasterLayer layer;
                            layer = new SharpMap.Layers.GdalGridRasterLayer(System.IO.Path.GetFileNameWithoutExtension(inputFile), inputFile, thislock);
                            layer.SRID = 4326;
                            layer.Enabled = true;
                            thismap.Layers.Add(layer);
                        }
                        //设置Map的背景色为透明色
                        thismap.BackColor = System.Drawing.Color.Transparent;
                        //图像的formate
                        System.Drawing.Imaging.ImageCodecInfo imageEncoder = Dstile.GetEncoderInfo("image/png");
                        //图片的大小
                        thismap.Size = new System.Drawing.Size(512, 512);
                        tiles[tileIndex] = new Tile(x, y, thismap, levelInfo.LevelPerDegree, levelInfo.LevelNumber, localpath);
                        tileIndex++;

                        #endregion

                        //判断是否达到了CPU内核个数，若达到，则开始进行切图
                        if (tileIndex == CPUCORE)
                        {
                            Task[] tasks = new Task[CPUCORE];
                            //产生4个线程执行切图
                            for (int t = 0; t < tasks.Length; t++)
                            {
                                tasks[t] = new Task(argument => GenerateMap((Tile)argument), tiles[t]);
                                tasks[t].Start();
                            }
                            Task.WaitAll(tasks);
                            tileIndex = 0;
                        }
                    }

                    //判断，还剩几个没有做的任务，并行切图
                    if (tileIndex != 0)
                    {
                        Task[] tasks = new Task[tileIndex];
                        //产生tileIndex个线程执行切图
                        for (int t = 0; t < tasks.Length; t++)
                        {
                            tasks[t] = new Task(argument => GenerateMap((Tile)argument), tiles[t]);
                            tasks[t].Start();
                        }
                        Task.WaitAll(tasks);
                        tileIndex = 0;
                    }
                }
                //循环，开始切下一层数据，每下一层，都是上一层的1/2。
                lztsd = lztsd / 2;
            }

            Console.WriteLine("切片完成！");
            File.CreateText(string.Format(@"{0}\Dstile_Done.txt", outputDir));
        }
        /// <summary>
        /// 切图
        /// </summary>
        /// <param name="tileInfo"></param>
        private static void GenerateMap(Tile tileInfo)
        {
            int x = tileInfo.x;
            int y = tileInfo.y;
            SharpMap.Map thismap = tileInfo.map;
            //请求图像的范围
            double tileNorth = 0.0;
            double tileSorth = 0.0;
            double tileWest = 0.0;
            double tileEast = 0.0;
            string bboxString = "";
            bboxString = "-180" + "," + "-90" + "," + "180" + "," + "90";
            SharpMap.Geometries.BoundingBox bbox = Dstile.ParseBBOX(bboxString);

            tileWest = x * tileInfo.LevelPerDegree - 180;
            tileEast = (x + 1) * tileInfo.LevelPerDegree - 180;
            tileSorth = y * tileInfo.LevelPerDegree - 90;
            tileNorth = (y + 1) * tileInfo.LevelPerDegree - 90;
            bboxString = tileWest.ToString() + "," + tileSorth.ToString() + "," + tileEast.ToString() + "," + tileNorth.ToString();
            bbox = Dstile.ParseBBOX(bboxString);

            //判断图像的拉伸范围
            thismap.PixelAspectRatio = ((double)512 / (double)512) / (bbox.Width / bbox.Height);
            thismap.BackColor = System.Drawing.Color.Transparent;
            thismap.Center = bbox.GetCentroid();
            thismap.Zoom = bbox.Width;
            thismap.Projection = CoordinatesDescription.GCS;
            //文件在本机保存的路径

            if (!IsDEM)
            {
                string localfile = tileInfo.savePath;
                if (File.Exists(localfile))
                {
                    System.Drawing.Image img = System.Drawing.Image.FromFile(localfile);
                    //获取图像
                    img = thismap.GetMap(img);
                    ////保存到本地磁盘
                    img.Save(localfile + ".tmp");
                    img.Dispose();
                    File.Delete(localfile);
                    img = System.Drawing.Image.FromFile(localfile + ".tmp") as Bitmap;
                    img.Save(localfile);
                    img.Dispose();
                    File.Delete(localfile + ".tmp");

                }
                else
                {
                    System.Drawing.Image img = null;
                    //获取图像
                    img = thismap.GetMap();

                    ////丢弃全黑瓦片
                    //for (int xx = 0; xx < img.Width; xx++)
                    //{
                    //    for (int yy = 0; yy < img.Height; yy++)
                    //    {
                    //        if (((Bitmap)img).GetPixel(xx, yy) != Color.Black)
                    //        {
                    //            //保存到本地磁盘
                    img.Save(localfile);
                    img.Dispose();
                    FileInfo fi = new FileInfo(localfile);
                    if (fi.Length < 1200)
                    {
                        File.Delete(localfile);
                    }
                    else
                    {
                        Console.WriteLine("Generate Tile:" + Path.GetFileNameWithoutExtension(localfile));
                    }//            return;
                    //        }
                    //    }
                    //}

                    //img.Dispose();
                }
            }
            else
            {
                //切DEM数据 默认采用bil Int16
                string bilsavefilepath = tileInfo.savePath + ".bil";
                string hdrsavefilepath = tileInfo.savePath + ".hdr";
                string stxsavefilepath = tileInfo.savePath + ".stx";

                short[] demtiledata = thismap.GetDEMTileDataset(tileWest, tileNorth, tileInfo.LevelPerDegree);
                if (demtiledata != null)
                {

                    if (File.Exists(bilsavefilepath))
                        File.Delete(bilsavefilepath);
                    if (File.Exists(hdrsavefilepath))
                        File.Delete(hdrsavefilepath);
                    if (File.Exists(stxsavefilepath))
                        File.Delete(stxsavefilepath);
                    byte[] bts = new byte[demtiledata.Length * 2];
                    for (int ii = 0; ii < demtiledata.Length; ii++)
                    {
                        short2Byte(demtiledata[ii], bts, ii * 2);
                    }

                    FileStream fs = File.Create(bilsavefilepath);
                    fs.Write(bts, 0, bts.Length);
                    fs.Close();

                    string[] contents = new string[]{
                    "BYTEORDER      M",
                    "LAYOUT         BIL",
                    "NROWS          512",
                    "NCOLS          512",
                    "NBANDS         1",
                    "NBITS          16",
                    "BANDROWBYTES   1024",
                    "TOTALROWBYTES  1024",
                    "BANDGAPBYTES   0",
                    "NODATA         -32767",
                    string.Format("ULXMAP         {0}",tileWest),
                    string.Format("ULYMAP         {0}",tileNorth),
                    string.Format("XDIM           {0}",tileInfo.LevelPerDegree/512),
                    string.Format("YDIM           {0}",tileInfo.LevelPerDegree/512)};
                    File.WriteAllLines(hdrsavefilepath, contents);


                    Console.WriteLine("Generate Tile:" + Path.GetFileNameWithoutExtension(bilsavefilepath));

                }
            }

        } 

        /** 
            * 将short转成byte[2] 
            * @param a 
            * @param b 
            * @param offset b中的偏移量 
            */
        public static void short2Byte(short a, byte[] b, int offset)
        {
            b[offset] = (byte)(a >> 8);
            b[offset + 1] = (byte)(a);
        }

        /// <summary>
        /// 计算地图的最大，最小区域范围（所有影像范围的一个交集）
        /// </summary>
        /// <param name="imageEnvelop"></param>
        /// <param name="mapEnvelop"></param>
        public static void getMapEnvelop(Envelop imageEnvelop, ref Envelop mapEnvelop)
        {
            mapEnvelop.East = Math.Max(mapEnvelop.East, imageEnvelop.East);
            mapEnvelop.West = Math.Min(mapEnvelop.West, imageEnvelop.West);
            mapEnvelop.North = Math.Max(mapEnvelop.North, imageEnvelop.North);
            mapEnvelop.South = Math.Min(mapEnvelop.South, imageEnvelop.South);
        }

        public static System.Drawing.Imaging.ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            foreach (System.Drawing.Imaging.ImageCodecInfo encoder in System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders())
                if (encoder.MimeType == mimeType)
                    return encoder;
            return null;
        }
        /// <summary>
        /// 获得当前影像的范围
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="isShapeFile"></param>
        /// <param name="gcsWkt"></param>
        /// <returns></returns>
        public static Envelop GetEnvelop(string fileName, bool isShapeFile, string gcsWkt)
        {
            ICoordinateSystem srcCoor = null;
            SharpMap.Layers.Layer layer = null;
            if (isShapeFile)
            {

                SharpMap.Layers.VectorLayer l = new SharpMap.Layers.VectorLayer(Path.GetFileNameWithoutExtension(fileName));
                l.DataSource = new SharpMap.Data.Providers.ShapeFile(fileName, true);
                l.SRID = 4326;
                srcCoor = ((SharpMap.Data.Providers.ShapeFile)l.DataSource).CoordinateSystem;
                layer = l;
            }
            else
            {
                SharpMap.Layers.GdalRasterLayer l = new SharpMap.Layers.GdalRasterLayer(Path.GetFileNameWithoutExtension(fileName), fileName);
                l.SRID = 4326;
                l.Enabled = true;
                srcCoor = l.GetProjection();
                layer = l;
            }

            Envelop envelop = new Envelop();

            //if (srcCoor != null)
            //{
            //    CoordinateSystemFactory cFac = new CoordinateSystemFactory();
            //    ICoordinateSystem tarCoor = cFac.CreateFromWkt(gcsWkt);
            //    ICoordinateTransformation transform = null;
            //    try
            //    {
            //        transform = new CoordinateTransformationFactory().CreateFromCoordinateSystems(srcCoor, tarCoor);
            //    }
            //    catch (Exception e)
            //    {
            //        return new Envelop();
            //    }

            //    SharpMap.Geometries.BoundingBox imageBox = GeometryTransform.TransformBox(layer.Envelope, transform.MathTransform);

            //    envelop.North = imageBox.Top;
            //    envelop.South = imageBox.Bottom;
            //    envelop.West = imageBox.Left;
            //    envelop.East = imageBox.Right;
            //}
            //else
            //{
            envelop.East = layer.Envelope.Right;
            envelop.West = layer.Envelope.Left;
            envelop.North = layer.Envelope.Top;
            envelop.South = layer.Envelope.Bottom;
            //}
            return envelop;

        }
        /// <summary>
        /// 得到BoundingBox
        /// </summary>
        /// <param name="strBBOX"></param>
        /// <returns></returns>
        public static SharpMap.Geometries.BoundingBox ParseBBOX(string strBBOX)
        {
            string[] strVals = strBBOX.Split(new char[] { ',' });
            if (strVals.Length != 4)
                return null;
            double minx = 0; double miny = 0;
            double maxx = 0; double maxy = 0;
            if (!double.TryParse(strVals[0], System.Globalization.NumberStyles.Float, SharpMap.Map.NumberFormatEnUs, out minx))
                return null;
            if (!double.TryParse(strVals[2], System.Globalization.NumberStyles.Float, SharpMap.Map.NumberFormatEnUs, out maxx))
                return null;
            if (maxx < minx)
                return null;

            if (!double.TryParse(strVals[1], System.Globalization.NumberStyles.Float, SharpMap.Map.NumberFormatEnUs, out miny))
                return null;
            if (!double.TryParse(strVals[3], System.Globalization.NumberStyles.Float, SharpMap.Map.NumberFormatEnUs, out maxy))
                return null;
            if (maxy < miny)
                return null;

            return new SharpMap.Geometries.BoundingBox(minx, miny, maxx, maxy);
        }

    }

    public class Level
    {
        public double LevelPerDegree
        {
            get;
            set;
        }
        public int LevelNumber
        {
            get;
            set;
        }

        public Level(double levelPerDegree, int level)
        {
            this.LevelPerDegree = levelPerDegree;
            this.LevelNumber = level;
        }
    }

    public class Tile
    {
        public int x;
        public int y;
        public SharpMap.Map map
        {
            get;
            set;
        }
        public double LevelPerDegree
        {
            get;
            set;
        }
        public int Level
        {
            get;
            set;
        }
        public string savePath
        {
            get;
            set;
        }
        public Tile(int x, int y, SharpMap.Map map, double LevelPerDegree, int Level, string savePath)
        {
            this.x = x;
            this.y = y;
            this.map = map;
            this.LevelPerDegree = LevelPerDegree;
            this.Level = Level;
            if (!Dstile.IsDEM)
            {
                this.savePath = savePath + "\\" + string.Format("{0}", y) + "_" + string.Format("{0}", x) + ".jpg";
            }
            else
            {
                this.savePath = savePath + "\\" + string.Format("{0}", y) + "_" + string.Format("{0}", x);
            }

        }

    }

    public struct Envelop
    {
        public double South;
        public double North;
        public double West;
        public double East;
    }


}
