namespace DataPrepare
{
    /// <summary>
    /// 判断高分影像和瓦片是否相交的类
    ///此类后来进行了重构。JudgeIntersect为该Intersect重新改写后的类.这个类和JudgeIntersect类初始化方法调用返回都一样。
    /// 此类经过测试，但重构后的JudgeIntersect未经测试
    /// </summary>
    class Intersect
    {
        point[] GF1_P = new point[4];//p保存原始影像的四个坐标点
        point[] tile_P = new point[4];//tileP保存网格的四个坐标点

        //初始化坐标点时对经度+180，对纬度+90
        public Intersect(GF1_LanAndLong gf1, double[] latAndlong)
        {
            for (int i = 0; i < 4; i++)
            {
                GF1_P[i] = new point();
                tile_P[i] = new point();
            }

            GF1_P[0].x = gf1.DATALOWERLEFTLAT + 90;
            GF1_P[0].y = gf1.DATALOWERLEFTLONG + 180;
            GF1_P[1].x = gf1.DATALOWERRIGHTLAT + 90;
            GF1_P[1].y = gf1.DATALOWERRIGHTLONG + 180;
            GF1_P[2].x = gf1.DATAUPPERRIGHTLAT + 90;
            GF1_P[2].y = gf1.DATAUPPERRIGHTLONG + 180;
            GF1_P[3].x = gf1.DATAUPPERLEFTLAT + 90;
            GF1_P[3].y = gf1.DATAUPPERLEFTLONG + 180;

            tile_P[0].x = latAndlong[0] + 90;
            tile_P[0].y = latAndlong[1] + 180;
            tile_P[1].x = latAndlong[0] + 90;
            tile_P[1].y = latAndlong[3] + 180;
            tile_P[2].x = latAndlong[2] + 90;
            tile_P[2].y = latAndlong[3] + 180;
            tile_P[3].x = latAndlong[2] + 90;
            tile_P[3].y = latAndlong[1] + 180;
        }

        
        //铅垂线法判定点是否在多边形内，判定切片每个角点与原始影像相交于左侧的边有几条，如果有1条，该角点在原始影像内，如果为2或者0条，则在影像外        
        public bool VLL()
        {
            double[] k = new double[4];
            double[] b = new double[4];
            int[] tag = new int[4];
            bool[] k_ExistTag = new bool[4] { true, true, true, true };

            for (int j = 0; j < GF1_P.Length; j++)
            {
                //由于在原始遥感影像中不会存在k值不存在的情况，所以对k不存在不作考虑。求出多边形每条边的斜率k和截距b

                if (GF1_P[j].x - GF1_P[(j + 1) % GF1_P.Length].x != 0)
                {
                    k[j] = (GF1_P[j].y - GF1_P[(j + 1) % GF1_P.Length].y) / (GF1_P[j].x - GF1_P[(j + 1) % GF1_P.Length].x);
                    b[j] = GF1_P[j].y - k[j] * GF1_P[j].x;
                }

                if (GF1_P[j].x - GF1_P[(j + 1) % GF1_P.Length].x == 0)
                {
                    k_ExistTag[j] = false;
                }
            }

            //依次判断A多边形的四个点向作水平射线与B多边形四条边相交点的个数，并记为tag，tag为奇数代表A的某个点在多边形B内
            for (int i = 0; i < tile_P.Length; i++)
            {
                for (int j = 0; j < GF1_P.Length; j++)
                {
                    if (k_ExistTag[j] == true)
                    {
                        if ((tile_P[i].x - GF1_P[j].x) * (tile_P[i].x - GF1_P[(j + 1) % GF1_P.Length].x) <= 0 && tile_P[i].y > k[j] * tile_P[i].x + b[j])
                        {
                            tag[i]++;
                        }
                    }
                    else
                    {
                        tag[i] = 0;
                    }
                }
            }

            for (int i = 0; i < tile_P.Length; i++)
            {
                if (tag[i] == 1)
                {
                    return true; //如果A多边形四个点有一个点在B多边形内，就说明A与B两个多边形相交
                }
            }
            return false;
        }
    }

}

