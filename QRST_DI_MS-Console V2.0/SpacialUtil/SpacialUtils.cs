using System;
using QRST.WorldGlobeTool.Geometries;

namespace QRST_DI_MS_Desktop.SpacialUtil
{ 
    public class SpacialUtils
    {
        /// <summary>
        /// 计算四边形的外接矩形
        /// </summary>
        /// <param name="upperleft"></param>
        /// <param name="upperRight"></param>
        /// <param name="lowerLeft"></param>
        /// <param name="lowerRight"></param>
        /// <returns>返回外接矩形的左上角和右下角的点坐标</returns>
        public static Point2d[] GetOutExtent(Point2d upperleft,Point2d upperRight,Point2d lowerLeft,Point2d lowerRight)
        {
            Point2d[] outerExtent = new Point2d[2];
            outerExtent[0] = new Point2d(Math.Max(upperleft.X,upperRight.X),Math.Min(upperleft.Y,lowerLeft.Y));
            outerExtent[1] = new Point2d(Math.Min(lowerLeft.X,lowerRight.X),Math.Max(upperRight.Y,lowerRight.Y));
            return outerExtent;
        }

        /// <summary>
        /// 根据球体分割系数Kr和Kc，以及行列号，计算出该格网的矩形坐标
        /// </summary>
        /// <param name="Kr">行分割系数</param>
        /// <param name="Kc">列分割系数</param>
        /// <param name="row">格网的行号</param>
        /// <param name="col">格网的列号</param>
        /// <returns>左上角和右下角的点坐标</returns>
        public static Point2d[] GetGridExtent(int Kr,int Kc,int row,int col)
        {
            if(Kr>0&&Kc>0&&row>0&&col>0&&row<Kr&&col<Kc)
            {
                Point2d[] gridExtent = new Point2d[2]; 
                gridExtent[0] = new Point2d();                     //左上角
                gridExtent[0].X = (1- row) * (180 / Kr)+90;
                gridExtent[0].Y = (col - 1) * (360 / Kc) - 180;

                gridExtent[1] = new Point2d();
                gridExtent[1].X = (0 - row) * (180 / Kr) + 90;
                gridExtent[1].Y = col * (360 / Kc) - 180;

                return gridExtent;
            }
            else
             return null;
        }

        /// <summary>
        /// 根据矩形坐标获取网格行列号范围
        /// </summary>
        /// <param name="Kr"></param>
        /// <param name="Kc"></param>
        /// <param name="upperleft"></param>
        /// <param name="lowerright"></param>
        /// <returns></returns>
        public static int[] GetRowColRange(int Kr, int Kc,Point2d upperleft,Point2d lowerright)
        {
            int minrow, maxrow, mincol, maxcol;
            if(Kr>0&&Kc>0)
            {
                minrow = (int)(90 - upperleft.X) / (180 / Kr);
                maxrow = (int)(90 - lowerright.X) / (180 / Kr);
                mincol = ((int)upperleft.Y+180) / (360 / Kc);
                maxcol = ((int)lowerright.Y+180)/(360/Kc);
                return new int[] {minrow,maxrow,mincol,maxcol };
            }
            return null;
        }
    }
}
