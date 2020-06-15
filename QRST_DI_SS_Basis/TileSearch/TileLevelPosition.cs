using System.Collections.Generic;
using QRST_DI_Basis;

namespace QRST_DI_SS_Basis.TileSearch
{
    public class TileLevelPosition
    {
        /// <summary>
        /// 切片等级
        /// </summary>
        public string TileLevel;
        /// <summary>
        /// 切片行列范围（最小行号，最小列号，最大行号，最大列号）
        /// </summary>
        public int[] tileRowandColumn;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tileLevel">切片等级</param>
        /// <param name="position">查询的经纬度范围，最小纬度、最小经度、最大纬度、最大经度</param>
        public TileLevelPosition(string tileLevel, List<string> position)
        {
            if (position.Count >= 4)
            {
                TileLevel = tileLevel;

                tileRowandColumn = RowColumnTransform.GetRowAndColum(position.ToArray(), tileLevel);
            }
        }
    }
}
