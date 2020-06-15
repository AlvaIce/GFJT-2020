using System.Data;
using System.Text;
 
namespace QRST_DI_MS_Desktop.UserInterfaces
{
    public partial class FrmOrderLogInfo : DevExpress.XtraEditors.XtraForm
    {
  

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderCode">订单号</param>
        public FrmOrderLogInfo(string orderCode)
        {
            InitializeComponent();
            GetOrderLogInfo(orderCode);
        }

        void GetOrderLogInfo(string orderCode)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("订单详细信息：");
            sb.AppendLine("订单号："+orderCode);
            DataSet ds = TheUniversal.MIDB.sqlUtilities.GetDataSet(string.Format("select * from translog where MESSAGE like '{0}%'",orderCode));
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                sb.AppendLine("没有找到该订单的详细日志信息！");
            }
            else
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count;i++ )
                {
                    sb.AppendLine(string.Format("[{0}]:{1}",ds.Tables[0].Rows[i]["LOGTIME"],ds.Tables[0].Rows[i]["MESSAGE"]));
                }
            }
            memoEdit1.Text = sb.ToString();
        }
    }
}