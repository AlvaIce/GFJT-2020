namespace DataPrepare
{
    /// <summary>
    /// 判断多边形A和多边形B是否相交的类
    ///JudgeIntersect为Intersect重新改写后的类.这个类和Intersect类初始化方法调用返回都一样。
    /// 此类JudgeIntersect未经测试。
    /// </summary>
    class JudgeIntersect
    {
        point[] A = new point[4];//p保存原始影像的四个坐标点
        point[] B = new point[4];//tileP保存网格的四个坐标点

        //初始化坐标点时对经度+180，对纬度+90
        public JudgeIntersect(GF1_LanAndLong gf1, double[] latAndlong)
        {
            for (int i = 0; i < 4; i++)
            {
                A[i] = new point();
                B[i] = new point();
            }

            A[0].x = gf1.DATALOWERLEFTLAT + 90;
            A[0].y = gf1.DATALOWERLEFTLONG + 180;
            A[1].x = gf1.DATALOWERRIGHTLAT + 90;
            A[1].y = gf1.DATALOWERRIGHTLONG + 180;
            A[2].x = gf1.DATAUPPERRIGHTLAT + 90;
            A[2].y = gf1.DATAUPPERRIGHTLONG + 180;
            A[3].x = gf1.DATAUPPERLEFTLAT + 90;
            A[3].y = gf1.DATAUPPERLEFTLONG + 180;

            B[0].x = latAndlong[0] + 90;
            B[0].y = latAndlong[1] + 180;
            B[1].x = latAndlong[0] + 90;
            B[1].y = latAndlong[3] + 180;
            B[2].x = latAndlong[2] + 90;
            B[2].y = latAndlong[3] + 180;
            B[3].x = latAndlong[2] + 90;
            B[3].y = latAndlong[1] + 180;
        }

        public bool VLL()
        {
            bool result = VLL(A, B);
            if (result == false)
            {
                result = VLL(B,A);
            }
            return result;
        }

        
        //铅垂线法判定shape2的各个点是否在shape1多边形内，判定shape2每个角点的水平向左射线与shape1d的边相交于左侧的边有几条，如果有1条，该角点在原shape1内，如果为2或者0条，则在shape1外        
        public bool VLL(point[] shape1,point[] shape2)
        {
            double[] k = new double[4];
            double[] b = new double[4];
            int[] tag = new int[4];
            bool[] k_ExistTag = new bool[4] { true, true, true, true };

            for (int j = 0; j < shape1.Length; j++)
            {
                //分为斜率存在和不存在两种情况
                if (shape1[j].x - shape1[(j + 1) % shape1.Length].x != 0)
                {
                    k[j] = (shape1[j].y - shape1[(j + 1) % shape1.Length].y) / (shape1[j].x - shape1[(j + 1) % shape1.Length].x);
                    b[j] = shape1[j].y - k[j] * shape1[j].x;
                }

                if (shape1[j].x - shape1[(j + 1) % shape1.Length].x == 0)
                {
                    k_ExistTag[j] = false;
                }
            }

            //依次判断A多边形的四个点向作水平射线与B多边形四条边相交点的个数，并记为tag，tag为奇数代表A的某个点在多边形B内
            for (int i = 0; i < shape2.Length; i++)
            {
                for (int j = 0; j < shape1.Length; j++)
                {
                    if (k_ExistTag[j] == true)
                    {
                        if ((shape2[i].x - shape1[j].x) * (shape2[i].x - shape1[(j + 1) % shape1.Length].x) <= 0 && shape2[i].y > k[j] * shape2[i].x + b[j])
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

            for (int i = 0; i < shape2.Length; i++)
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
