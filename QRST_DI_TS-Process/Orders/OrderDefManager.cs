using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetaData;
using QRST_DI_SS_DBInterfaces.IDBEngine;
using QRST_DI_Resources;

namespace QRST_DI_TS_Process.Orders
{
    public class OrderDefManager
    {
        private static IDbBaseUtilities _sqLiteUtilities =
            Constant.IdbOperating.GetSubDbUtilities(EnumDBType.MIDB);
        private static IDbOperating _sqLiteOperating;
        public static List<OrderDef> GetOrderDefFromDB(string whereCondition)
        {
            List<OrderDef> orderLst = new List<OrderDef>();
            string sql = "select * from orderdef ";
            if (!string.IsNullOrEmpty(whereCondition))
            {
                sql = sql + "where " + whereCondition;
            }
;            DataSet ds = _sqLiteUtilities.GetDataSet(sql);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                orderLst.Add(DBRow2OrderDef(ds.Tables[0].Rows[i]));
            }
            return orderLst;
        }

        public static OrderDef DBRow2OrderDef(DataRow dr)
        {
            OrderDef model = new OrderDef();

            if (dr["id"].ToString() != "")
            {
                model.id = int.Parse(dr["id"].ToString());
            }
            model.Name = dr["Name"].ToString();
            model.QRST_CODE = dr["QRST_CODE"].ToString();
            model.TServerSite = dr["TServerSite"].ToString();
            model.Status = dr["Status"].ToString();
            model.Type = dr["Type"].ToString();
            model.Owner = dr["Owner"].ToString();
            if (dr["SubmitTime"].ToString() != "")
            {
                model.SubmitTime = DateTime.Parse(dr["SubmitTime"].ToString());
            }
            model.Tasks = dr["Tasks"].ToString();
            model.TaskParams = dr["TaskParams"].ToString();
            model.Phase = dr["Phase"].ToString();
            model.OrderCode = dr["OrderCode"].ToString();
            model.Priority = dr["Priority"].ToString();
            model.OrderParams = dr["OrderParams"].ToString();
            model.Description = dr["Description"].ToString();
            return model;

        }

        /// <summary>
        /// 将OrderDef转换为OrderClass，转换后的OrderClass缺少对订单参数、任务参数、任务执行站点以及所有者的定义
        /// </summary>
        /// <param name="orderdef"></param>
        /// <returns></returns>
        public static OrderClass OrderDef2OrderClass(OrderDef orderdef)
        {
            OrderClass orderClass;
            if (orderdef != null)
            {
                orderClass = new OrderClass();
                EnumOrderType type;
                Enum.TryParse(orderdef.Type, out type);
                EnumOrderPriority priority;
                Enum.TryParse(orderdef.Priority, out priority);
                orderClass.Tasks = OrderManager.Str2TaskList(orderdef.Tasks);
                orderClass.OrderParams = OrderManager.Str2OrderParams(orderdef.OrderParams);
                orderClass.Type = type;
                orderClass.Priority = priority;
                orderClass.OrderName = orderdef.Name;
                return orderClass;
            }
            return null;
        }

        /// <summary>
        /// 向数据库添加orderDef记录
        /// </summary>
        public static void AddOrderDef(OrderDef orderDef)
        {
            _sqLiteOperating = Constant.IdbOperating;
            //TableLocker dblock = new TableLocker(TSPCommonReference.dbOperating.MIDB);

            _sqLiteOperating.LockTable("orderdef",EnumDBType.MIDB);
            int maxid = _sqLiteUtilities.GetMaxID("ID", "orderdef");
            orderDef.id = maxid;
            orderDef.QRST_CODE = "MIDB-17-" + maxid.ToString();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into orderdef(");
            strSql.Append("id,Name,QRST_CODE,Type,SubmitTime,Tasks,Priority,OrderParams,Description)");
            strSql.Append(" values (");
            strSql.AppendFormat("{8},'{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')", orderDef.Name, orderDef.QRST_CODE, orderDef.Type, orderDef.SubmitTime, orderDef.Tasks, orderDef.Priority, orderDef.OrderParams, orderDef.Description, orderDef.id);

            _sqLiteUtilities.ExecuteSql(strSql.ToString());
            _sqLiteOperating.UnlockTable("orderdef", EnumDBType.MIDB);
        }

        /// <summary>
        /// 修改
        /// </summary>
        public static void UpdateOrderDef(OrderDef orderDef)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update orderdef set ");
            strSql.AppendFormat("Name='{0}',", orderDef.Name);
            strSql.AppendFormat("SubmitTime='{0}',", orderDef.SubmitTime);
            strSql.AppendFormat("Tasks='{0}',", orderDef.Tasks);
            strSql.AppendFormat("Priority='{0}',", orderDef.Priority);
            strSql.AppendFormat("OrderParams='{0}',", orderDef.OrderParams);
            strSql.AppendFormat("Description='{0}'", orderDef.Description);
            strSql.AppendFormat(" where id={0}", orderDef.id);
            
            _sqLiteUtilities.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// 删除orderdef
        /// </summary>
        /// <param name="id"></param>
        public static void DeleteOrderDef(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from orderdef ");
            strSql.AppendFormat(" where id={0} ", id);
            _sqLiteUtilities.ExecuteSql(strSql.ToString());
        }
    }
}
