using System;

namespace QRST_DI_SS_Basis.MetaData
{
    [Serializable]
    public enum EnumDBType
    {
        /// <summary>
        /// 运行管理库
        /// </summary>
        MIDB = 0,
        /// <summary>
        /// 基础空间数据库
        /// </summary>
        BSDB = 1,
        /// <summary>
        /// 模型算法数据库
        /// </summary>
        MADB = 2,
        /// <summary>
        /// 遥感应用特征数据库
        /// </summary>
        RCDB = 3,
        /// <summary>
        /// 实验验证数据库
        /// </summary>
        EVDB = 4,
        /// <summary>
        /// 数据产品库
        /// </summary>
        IPDB = 5,
        /// <summary>
        /// 信息服务库
        /// </summary>
        ISDB = 6,
        /// <summary>
        /// 信息产品库
        /// </summary>
        INDB = 7
    }
}
