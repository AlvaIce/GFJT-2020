using System.Data;
using QRST_DI_Resources;
using QRST_DI_SS_DBInterfaces.IDBEngine;
using QRST_DI_SS_Basis.MetaData;
 
namespace QRST_DI_TS_Basis.DirectlyAddress
{
    public class DirectlyAddressingIPMod
    {
        public static DataSet _dtMod = null;
        /// <summary>
        /// IP 配号映射表
        /// </summary>
        public static DataSet IPModDataSet
        {
            get
            {
                if (_dtMod==null)
                {
                    UpdateIPModDataSet();
                }
                return _dtMod;
            }
        }
        /// <summary>
        /// 从库中获取IP配号表
        /// </summary>
        /// <returns>IP配号表</returns>
        public static DataSet GetTileDsMod()
        {
            //迁移到服务进程 jianghua 20170415
            //IDataBaseUtilities Operator = new MySqlBaseUtilities();
            //DataSet dtMod = Operator.GetDataSet(sql);
            string sql = "select addressip,modid,ISCENTER from tileserversitesinfo";
            IDbBaseUtilities baseUtilities = Constant.IdbServerUtilities.GetSubDBUtil("midb");
            DataSet dtMod = baseUtilities.GetDataSet(sql);
            return dtMod;
        }

        /// <summary>
        /// 当数据表更新后用，重新获取IP Mod映射表
        /// </summary>
        public static void UpdateIPModDataSet()
        {
            _dtMod = GetTileDsMod();
        }


    }
}
