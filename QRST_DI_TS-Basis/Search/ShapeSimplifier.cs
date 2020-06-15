using System;
using System.Collections.Generic;
using DotSpatial.Topology;
 
namespace QRST_DI_TS_Basis.Search
{
    public class ShapeSimplifier
    {
        public static double MaxAngle = 165;
        public static IList<Coordinate> Simplifier(IList<Coordinate> shape)
        {

            if (shape.Count < 4)
            {
                return shape;
            }

            List<Coordinate> simpleshp = new List<Coordinate>();
            simpleshp.Add(shape[0]);

            int i = 1;
            Coordinate start = shape[i - 1];
            Coordinate mid = shape[i];
            Coordinate end = shape[i + 1];
            i++;
            bool doit = true;

            while (doit)//i < shape.Count - 2)
            {
                //循环判断是否移除中间点，通过夹角角度是否大于MaxAngle度

                double corner0 = -999;      //表示未赋值
                double corner1 = -999;

                if (start.Y == mid.Y)
                {
                    if (start.X == mid.X)
                    {
                        //如果中间点和起始点重合 删除（跳过插入结果集）
                        if (i < shape.Count - 1)
                        {
                            start = start;
                            mid = end;
                            end = shape[i + 1];     //i已经++过了
                            i++;
                            doit = true;
                        }
                        else
                        {
                            doit = false;
                        }
                        continue;
                    }
                    corner0 = (mid.X > start.X) ? 90 : -90;
                }

                if (mid.Y == end.Y)
                {
                    if (end.X == mid.X)
                    {
                        if (i < shape.Count - 1)
                        {
                            //如果中间点和结束点重合 删除（跳过插入结果集）
                            start = start;
                            mid = end;
                            end = shape[i + 1];
                            i++;
                            doit = true;
                        }
                        else
                        {
                            doit = false;
                        }
                      
                        continue;
                    }
                    corner1 = (end.X > mid.X) ? 90 : -90;
                }

                if (corner0 == -999)
                {
                    int startquad = getQuad(start, mid);
                    double originc=180 * Math.Atan(Math.Abs((mid.X - start.X) / (start.Y - mid.Y))) / Math.PI;
                    switch (startquad)
                    {
                        case 1:
                            corner0 = -originc;
                            break;
                        case 2:
                            corner0 = originc;
                            break;
                        case 3:
                            corner0 = 180 - originc;
                            break;
                        case 4:
                            corner0 = 180 + originc;
                            break;
                        default:
                            break;
                    }


                }


                if (corner1 == -999)
                {
                    int endquad = getQuad(end, mid);
                    double originc = 180 * Math.Atan(Math.Abs((end.X - mid.X) / (end.Y - mid.Y))) / Math.PI;
                    switch (endquad)
                    {
                        case 1:
                            corner1 = originc;
                            break;
                        case 2:
                            corner1 = -originc;
                            break;
                        case 3:
                            corner1 = 180 + originc;
                            break;
                        case 4:
                            corner1 = 180 - originc;
                            break;
                        default:
                            break;
                    }
                }

                double cor = ((corner0 + corner1) > 180) ? (360 - (corner0 + corner1)) : (corner0 + corner1);

                if (cor > MaxAngle)
                {
                    if (i < shape.Count - 1)
                    {
                        //如果夹角大于阈值角，则不添加当前点
                        start = start;      //去掉当前点后，起始点仍为原起点
                        mid = end;
                        end = shape[i + 1];
                        i++;
                        doit = true;
                    }
                    else
                    {
                        doit = false;
                    }
                    continue;
                }
                else
                {
                    simpleshp.Add(mid);        //                    simpleshp.Add(shape[i + 1]);
                    if (i < shape.Count - 1)
                    {
                        //如果夹角不大于阈值角，添加当前点
                        start = mid;
                        mid = end;
                        end = shape[i + 1];
                        i++;
                        doit = true;
                    }
                    else
                    {
                        doit = false;
                    }
                    continue;
                }
            }

            for (int j = i; j < shape.Count; j++)
            {
                simpleshp.Add(shape[j]);
            }

            return simpleshp;
        }

        /// <summary>
        /// 判断在第几象限
        /// </summary>
        /// <param name="start"></param>
        /// <param name="mid"></param>
        /// <returns></returns>
        private static int getQuad(Coordinate start, Coordinate mid)
        {
            if (start.X>=mid.X&&start.Y>mid.Y)
            {
                return 1;
            }
            else if(start.X<mid.X&&start.Y>=mid.Y)
            {
                return 2;
            }
            else if (start.X<=mid.X&&start.Y<mid.Y)
            {
                return 3;
            }
            else if (start.X>mid.X&&start.Y<=mid.Y)
            {
                return 4;
            }
            return 0;
        }
    }
}
